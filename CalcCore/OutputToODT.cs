using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using M = DocumentFormat.OpenXml.Math;
using System.Windows.Forms;

namespace CalcCore
{
    public static class OutputToODT
    {
        public static void WriteToODT(ICalc calculation, bool includeInputs, bool includeBody, bool includeOutputs)
        {
            string filePath;
            try
            {
                var saveDialog = new SaveFileDialog();
                saveDialog.Filter = @"Word files |*.docx";
                saveDialog.FileName = calculation.InstanceName + @".docx";
                saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if(saveDialog.ShowDialog() != DialogResult.OK) return;
                filePath = saveDialog.FileName;
                var libraryPath = AppDomain.CurrentDomain.BaseDirectory + "Libraries";
                File.Copy(libraryPath + @"\Calculation_Template.docx", filePath, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Oops..." + Environment.NewLine + ex.Message);
                return;
            }
            
            using (WordprocessingDocument wordDocument =
                WordprocessingDocument.Open(filePath, true))
            {
                var mainPart = wordDocument.MainDocumentPart;
                Body body = mainPart.Document.AppendChild(new Body());

                var headerPart = mainPart.HeaderParts.First();
                var headerCalcType = new Paragraph(new Run(new Text(calculation.TypeName)));
                headerCalcType.PrependChild<ParagraphProperties>(new ParagraphProperties() { ParagraphStyleId = new ParagraphStyleId() { Val = "NoSpacing" } });
                var headerCalcInstance = new Paragraph(new Run(new Text(calculation.InstanceName)));
                headerCalcInstance.PrependChild<ParagraphProperties>(new ParagraphProperties() { ParagraphStyleId = new ParagraphStyleId() { Val = "NoSpacing" } });
                var headerDate = new Paragraph(new Run(new Text(DateTime.Today.ToLongDateString())));
                headerDate.PrependChild<ParagraphProperties>(new ParagraphProperties() { ParagraphStyleId = new ParagraphStyleId() { Val = "NoSpacing" } });
                headerPart.RootElement.Append(headerCalcType, headerCalcInstance, headerDate);

                if (includeInputs)
                {
                    Paragraph para = new Paragraph(new Run(new Text("Inputs")));
                    var paraProps = para.PrependChild<ParagraphProperties>(new ParagraphProperties());
                    paraProps.ParagraphStyleId = new ParagraphStyleId() { Val = "Heading1" };
                    body.Append(para);
                    var tableOfInputs = genTable(calculation.GetInputs());
                    body.Append(tableOfInputs);
                }

                if (includeBody)
                {
                    Paragraph para = new Paragraph(new Run(new Text("Formulae")));
                    var paraProps = para.PrependChild<ParagraphProperties>(new ParagraphProperties());
                    paraProps.ParagraphStyleId = new ParagraphStyleId() { Val = "Heading1" };
                    body.Append(para);

                    var FormulaeTable = genFormulaeTable(calculation.GetFormulae(), mainPart);
                    body.AppendChild(FormulaeTable);
                }

                if (includeOutputs)
                {
                    var para = new Paragraph(new Run(new Text("Outputs")));
                    var paraProps = para.PrependChild<ParagraphProperties>(new ParagraphProperties());
                    paraProps.ParagraphStyleId = new ParagraphStyleId() { Val = "Heading1" };
                    body.Append(para);

                    var tableOfOutputs = genTable(calculation.GetOutputs());
                    body.Append(tableOfOutputs);
                }

            }
        }

        private static Table genFormulaeTable(List<Formula> formulae, MainDocumentPart mainPart)
        {
            Table tableOfInputs = new Table();
            var tableGrid = new TableGrid();
            tableGrid.AppendChild(new GridColumn());
            tableGrid.AppendChild(new GridColumn());
            tableGrid.AppendChild(new GridColumn());
            tableOfInputs.AppendChild(tableGrid);
            var tableProps = new TableProperties();
            tableProps.AppendChild(new TableLayout() { Type = TableLayoutValues.Fixed });
            tableProps.AppendChild(new TableWidth() { Width = "10000", Type = TableWidthUnitValues.Dxa });
            tableOfInputs.AppendChild(tableProps);

            foreach (var item in formulae)
            {
                TableRow row = new TableRow();
                var para = new Paragraph();
                para.AppendChild((new Run(new Text(item.Ref))));
                TableCell cell1 = new TableCell();
                cell1.Append(para);
                cell1.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "1200" }));
                TableCell cell2 = new TableCell();
                cell2.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "7600" }));
                cell2.AppendChild(new Paragraph(new Run(new Text(item.Narrative))));
                foreach (var formula in item.Expression)
                {
                    var mathPara = new Paragraph();
                    var myMath = new M.OfficeMath(new M.Run(new M.Text(formula + Environment.NewLine) { Space = SpaceProcessingModeValues.Preserve }));
                    mathPara.AppendChild(myMath);
                    mathPara.AppendChild(new Run(new Text(" ") { Space = SpaceProcessingModeValues.Preserve }));
                    cell2.AppendChild(mathPara);
                }

                if (item.Image != null)
                {
                    ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Png);
                    var tempFile = Path.GetTempFileName();
                    PngBitmapEncoder png = new PngBitmapEncoder();
                    png.Frames.Add(BitmapFrame.Create(item.Image));
                    var width = Math.Min(10d, item.Image.Width * 2.54 / 96);
                    var height = (item.Image.Height / item.Image.Width) * width;
                    using (Stream stm = File.Create(tempFile))
                    {
                        png.Save(stm);
                    }
                    using (FileStream stream = new FileStream(tempFile, FileMode.Open))
                    {
                        imagePart.FeedData(stream);
                    }
                    var paraImage = AddImageToBody(mainPart.GetIdOfPart(imagePart), width, height);
                    cell2.AppendChild(paraImage);
                }
                
                TableCell cell3 = new TableCell();
                cell3.Append(new Paragraph(new Run(new Text(item.Conclusion))));
                cell3.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "1200" }));

                row.Append(cell1, cell2, cell3);
                tableOfInputs.AppendChild(row);
            }
            return tableOfInputs;
        }

        private static Table genTable(List<CalcCore.CalcValueBase> calcVals)
        {
            Table tableOfInputs = new Table();
            var tableGrid = new TableGrid();
            tableGrid.AppendChild(new GridColumn());
            tableGrid.AppendChild(new GridColumn());
            tableGrid.AppendChild(new GridColumn());
            tableOfInputs.AppendChild(tableGrid);
            var tableProps = new TableProperties();
            tableProps.AppendChild(new TableLayout() { Type = TableLayoutValues.Fixed });
            tableProps.AppendChild(new TableWidth() { Width = "10000", Type = TableWidthUnitValues.Dxa });
            tableOfInputs.AppendChild(tableProps);

            foreach (var item in calcVals)
            {
                TableRow row = new TableRow();
                var para = new Paragraph();
                var myMath = new M.OfficeMath(new M.Run(new M.Text(item.Symbol) { Space = SpaceProcessingModeValues.Preserve }));
                para.AppendChild(myMath);
                para.AppendChild(new Run(new Text(" ") { Space = SpaceProcessingModeValues.Preserve }));
                TableCell cell1 = new TableCell();
                cell1.Append(para);
                cell1.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "1200" }));
                TableCell cell2 = new TableCell(new Paragraph(new Run(new Text(item.Name))));
                cell2.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "7600" }));
                para = new Paragraph();
                if (item.Type == CalcValueType.DOUBLE)
                {
                    myMath = new DocumentFormat.OpenXml.Math.OfficeMath(new M.Run(new M.Text(item.ValueAsString + item.Unit) { Space = SpaceProcessingModeValues.Preserve }));
                    para.AppendChild(myMath);
                    para.AppendChild(new Run(new Text(" ") { Space = SpaceProcessingModeValues.Preserve }));
                }
                else
                {
                    para.AppendChild(new Run(new Text(item.ValueAsString)));
                }
                TableCell cell3 = new TableCell();
                cell3.Append(para);
                cell3.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "1200" }));
                row.Append(cell1, cell2, cell3);
                tableOfInputs.AppendChild(row);

            }
            return tableOfInputs;

        }

        private static Paragraph AddImageToBody(string relationshipId, double width, double height)
        {
            var cx = (int)(width * 914400 / 2.54);
            var cy = (int)(height * 914400 / 2.54);
            
            // Define the reference of the image.
            var element =
                 new Drawing(
                     new DW.Inline(
                         new DW.Extent() { Cx = cx, Cy = cy},
                         new DW.EffectExtent()
                         {
                             LeftEdge = 0L,
                             TopEdge = 0L,
                             RightEdge = 0L,
                             BottomEdge = 0L
                         },
                         new DW.DocProperties()
                         {
                             Id = (UInt32Value)1U,
                             Name = "Picture 1"
                         },
                         new DW.NonVisualGraphicFrameDrawingProperties(
                             new A.GraphicFrameLocks() { NoChangeAspect = true }),
                         new A.Graphic(
                             new A.GraphicData(
                                 new PIC.Picture(
                                     new PIC.NonVisualPictureProperties(
                                         new PIC.NonVisualDrawingProperties()
                                         {
                                             Id = (UInt32Value)0U,
                                             Name = "New Bitmap Image.png"
                                         },
                                         new PIC.NonVisualPictureDrawingProperties()),
                                     new PIC.BlipFill(
                                         new A.Blip(
                                             new A.BlipExtensionList(
                                                 new A.BlipExtension()
                                                 {
                                                     Uri =
                                                        "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                 })
                                         )
                                         {
                                             Embed = relationshipId,
                                             CompressionState =
                                             A.BlipCompressionValues.Print
                                         },
                                         new A.Stretch(
                                             new A.FillRectangle())),
                                     new PIC.ShapeProperties(
                                         new A.Transform2D(
                                             new A.Offset() { X = 0L, Y = 0L },
                                             new A.Extents() { Cx = cx, Cy = cy}),
                                         new A.PresetGeometry(
                                             new A.AdjustValueList()
                                         )
                                         { Preset = A.ShapeTypeValues.Rectangle }))
                             )
                             { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                     )
                     {
                         DistanceFromTop = (UInt32Value)0U,
                         DistanceFromBottom = (UInt32Value)0U,
                         DistanceFromLeft = (UInt32Value)0U,
                         DistanceFromRight = (UInt32Value)0U,
                         EditId = "50D07946"
                     });

            // Append the reference to body, the element should be in a Run.
            return new Paragraph(new Run(element));
        }
    }
}
