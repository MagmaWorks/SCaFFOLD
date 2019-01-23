using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcCore
{
    public static class OutputToODT
    {
        public static void WriteToODT(ICalc calculation, bool includeInputs, bool includeBody, bool includeOutputs)
        {
            var filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\test.docx";
            using (WordprocessingDocument wordDocument =
                WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                var mainPart = wordDocument.MainDocumentPart;
                mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
                Body body = mainPart.Document.AppendChild(new Body());
                Table tableOfInputs = new Table();
                foreach (var item in calculation.GetInputs())
                {
                    TableRow row = new TableRow();
                    TableCell cell = new TableCell();
                }
            }
        }
    }
}
