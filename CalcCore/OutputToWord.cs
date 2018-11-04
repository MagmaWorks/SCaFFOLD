﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;

namespace CalcCore
{
    public static class OutputToWord
    {
        public static void WriteToWord(ICalc calculation)
        {
            _Application winWord = new Application();
            object missing = Type.Missing;
            winWord.Visible = true;
            Document document = winWord.Documents.Add(ref missing, ref missing, ref missing, ref missing);
            object objEndOfDocFlag = "\\endofdoc";
            var math = document.OMaths;
            Range range;
            Paragraph para = document.Content.Paragraphs.Add();

            HeaderFooter header = document.Sections[1].Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary];
            header.Range.Text = "\nWhitby Wood"+
                "\nCreated by: "
                + System.Environment.UserName
                + " on "
                + DateTime.Today.Date.ToShortDateString();

            para.Range.Text = calculation.TypeName +
                ": " +
                calculation.InstanceName;
            object styleName = "Heading 1";
            para.Range.set_Style(ref styleName);
            para.Range.InsertParagraphAfter();

            para.Range.Text = "Inputs";
            styleName = "Heading 3";
            para.Range.set_Style(ref styleName);
            para.Range.InsertParagraphAfter();

            Table objTab1;
            Range objWordRng = document.Bookmarks.get_Item(ref objEndOfDocFlag).Range; //go to end of document
            float pageWidth = winWord.PointsToCentimeters(document.Sections[1].PageSetup.TextColumns[1].Width);

            objTab1 = document.Tables.Add(objWordRng, 2, 3, ref missing, ref missing); //add table object in word document
            objTab1.Range.ParagraphFormat.SpaceAfter = 6;
            //objTab1.Rows[1].Range.Font.Bold = 1;
            objTab1.AllowAutoFit = false;
            objTab1.Columns[1].Width = winWord.CentimetersToPoints(2.5F);
            objTab1.Columns[2].Width = winWord.CentimetersToPoints(pageWidth - 5F);
            objTab1.Columns[3].Width = winWord.CentimetersToPoints(2.5F);
            objTab1.Borders[WdBorderType.wdBorderVertical].Visible = true;
            objTab1.Cell(1, 1).Range.Text = "Symbol";
            objTab1.Cell(1, 2).Range.Text = "Name";
            objTab1.Cell(1, 3).Range.Text = "Value and units";
            int row = 2;
            var inputs = calculation.GetInputs();
            foreach (var input in inputs)
            {
                if (input.Symbol != "")
                {
                    range = objTab1.Cell(row, 1).Range;
                    range.Text = input.Symbol;
                    math.Add(range);
                }
                if (input.Unit != "" || input.ValueAsString != "")
                {
                    range = objTab1.Cell(row, 3).Range;
                    range.Text = input.ValueAsString + input.Unit;
                    math.Add(range);
                }
                objTab1.Cell(row, 2).Range.Text = input.Name;
                objTab1.Rows.Add(ref missing);
                row++;
            }
            objTab1.Rows[1].Borders[WdBorderType.wdBorderBottom].Visible = true;
            objTab1.Rows[1].Borders[WdBorderType.wdBorderBottom].LineWidth = WdLineWidth.wdLineWidth150pt;
            objTab1.Rows[1].Borders[WdBorderType.wdBorderBottom].Color = (WdColor)(240 + 0x100 * 89 + 0x10000 * 36); //converts RGB to Hex

            para.Range.Text = "Formulae";
            styleName = "Heading 3";
            para.Range.set_Style(ref styleName);
            para.Range.InsertParagraphAfter();

            objWordRng = document.Bookmarks.get_Item(ref objEndOfDocFlag).Range; //go to end of document
            objTab1 = document.Tables.Add(objWordRng, 2, 3, ref missing, ref missing); //add table object in word document
            objTab1.Range.ParagraphFormat.SpaceAfter = 6;
            //objTab1.Rows[1].Range.Font.Bold = 1;
            objTab1.AllowAutoFit = false;
            objTab1.Columns[1].Width = winWord.CentimetersToPoints(2.5F);
            objTab1.Columns[2].Width = winWord.CentimetersToPoints(pageWidth - 5F);
            objTab1.Columns[3].Width = winWord.CentimetersToPoints(2.5F);
            objTab1.Borders[WdBorderType.wdBorderVertical].Visible = true;
            objTab1.Cell(1, 1).Range.Text = "Ref.";
            objTab1.Cell(1, 2).Range.Text = "Narrative and calculations";
            objTab1.Cell(1, 3).Range.Text = "Output";
            row = 2;
            var formulae = calculation.GetFormulae();
            foreach (var line in formulae)
            {
                objTab1.Cell(row, 1).Range.Text = line.Ref;
                bool first = true;
                if (line.Narrative != "")
                { objTab1.Cell(row, 2).Range.Paragraphs.Last.Range.Text = line.Narrative; first = false; }
                foreach (var item in line.Expression)
                {
                    if (item != "")
                    {
                        Paragraph newEntry;
                        if (!first)
                            newEntry = objTab1.Cell(row, 2).Range.Paragraphs.Add();
                        else
                        { newEntry = objTab1.Cell(row, 2).Range.Paragraphs.Last; first = false; }
                        newEntry.Range.Text = item;
                        math.Add(newEntry.Range);
                    }
                }
                objTab1.Cell(row, 3).Range.Text = line.Conclusion;
                objTab1.Rows.Add(ref missing);
                row++;
            }
            objTab1.Rows[1].Borders[WdBorderType.wdBorderBottom].Visible = true;
            objTab1.Rows[1].Borders[WdBorderType.wdBorderBottom].LineWidth = WdLineWidth.wdLineWidth150pt;
            objTab1.Rows[1].Borders[WdBorderType.wdBorderBottom].Color = (WdColor)(240 + 0x100 * 89 + 0x10000 * 36); //converts RGB to Hex
            for (int i = 2; i < objTab1.Rows.Count+1; i++)
            {
                objTab1.Rows[i].Borders[WdBorderType.wdBorderBottom].Visible = true;
                objTab1.Rows[i].Borders[WdBorderType.wdBorderBottom].LineWidth = WdLineWidth.wdLineWidth150pt;
                objTab1.Rows[i].Borders[WdBorderType.wdBorderBottom].Color = WdColor.wdColorGray25;
            }


            para.Range.Text = "Calculated values";
            styleName = "Heading 3";
            para.Range.set_Style(ref styleName);
            para.Range.InsertParagraphAfter();

            objWordRng = document.Bookmarks.get_Item(ref objEndOfDocFlag).Range; //go to end of document
            objTab1 = document.Tables.Add(objWordRng, 2, 3, ref missing, ref missing); //add table object in word document
            objTab1.Range.ParagraphFormat.SpaceAfter = 6;
            //objTab1.Rows[1].Range.Font.Bold = 1;
            objTab1.AllowAutoFit = false;
            objTab1.Columns[1].Width = winWord.CentimetersToPoints(2.5F);
            objTab1.Columns[2].Width = winWord.CentimetersToPoints(pageWidth - 5F);
            objTab1.Columns[3].Width = winWord.CentimetersToPoints(2.5F);
            objTab1.Borders[WdBorderType.wdBorderVertical].Visible = true;
            objTab1.Cell(1, 1).Range.Text = "Symbol";
            objTab1.Cell(1, 2).Range.Text = "Name";
            objTab1.Cell(1, 3).Range.Text = "Value and units";
            row = 2;
            var outputs = calculation.GetOutputs();
            foreach (var output in outputs)
            {
                if (output.Type == CalcValueType.DOUBLE)
                {
                    if (output.Symbol != "")
                    {
                        range = objTab1.Cell(row, 1).Range;
                        range.Text = output.Symbol;
                        math.Add(range);
                    }
                    if (output.Unit != "" || output.ValueAsString != "")
                    {
                        range = objTab1.Cell(row, 3).Range;
                        double val = (output as CalcDouble).Value;
                        if (val >= 1000)
                            range.Text = (output as CalcDouble).Value.ToString("F0") + output.Unit;
                        else
                            range.Text = (output as CalcDouble).Value.ToString("G3") + output.Unit;

                        math.Add(range);
                    }
                }

                objTab1.Cell(row, 2).Range.Text = output.Name;
                objTab1.Rows.Add(ref missing);
                row++;
            }
            objTab1.Rows[1].Borders[WdBorderType.wdBorderBottom].Visible = true;
            objTab1.Rows[1].Borders[WdBorderType.wdBorderBottom].LineWidth = WdLineWidth.wdLineWidth150pt;
            objTab1.Rows[1].Borders[WdBorderType.wdBorderBottom].Color = (WdColor)(240 + 0x100 * 89 + 0x10000 * 36); //converts RGB to Hex

            for (int i = 1; i < math.Count+1; i++)
            {
                math[i].Type = WdOMathType.wdOMathInline;
            } 
            math.BuildUp();
        }
    }
}
