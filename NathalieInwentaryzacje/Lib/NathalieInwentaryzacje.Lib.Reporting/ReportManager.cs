using System.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Reports;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;

namespace NathalieInwentaryzacje.Lib.Reporting
{
    public class ReportManager : IReportManager
    {
        public byte[] BuildReport(RecordEntryReportInfo reportInfo)
        {
            byte[] data;

            var document = new Document(PageSize.A4);
            using (var ms = new MemoryStream())
            {
                var writer = PdfWriter.GetInstance(document, ms);
                writer.PageEvent = new PdfFooter();
                document.Open();

                CreateHeader(reportInfo.RecordDisplayName, reportInfo.RecordDate, document);

                var table = new PdfPTable(reportInfo.RecordEntryTable.Columns.Count);
                var widths = new float[reportInfo.RecordEntryTable.Columns.Count];
                for (var i = 0; i < widths.Length; i++)
                {
                    widths[i] = 4f;
                }

                table.SetWidths(widths);
                table.WidthPercentage = 100;

                foreach (DataColumn dataColumn in reportInfo.RecordEntryTable.Columns)
                {
                    var cell = new PdfPCell(new Phrase(dataColumn.ColumnName,
                        FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)))
                    {
                        HorizontalAlignment = 1,
                        BackgroundColor = BaseColor.LIGHT_GRAY
                    };
                    table.AddCell(cell);
                }

                foreach (DataRow dataRow in reportInfo.RecordEntryTable.Rows)
                {
                    for (var i = 0; i < reportInfo.RecordEntryTable.Columns.Count; i++)
                    {
                        PdfPCell cell;
                        if (i == 0)
                        {
                            cell = new PdfPCell(new Phrase(
                                dataRow[reportInfo.RecordEntryTable.Columns[i].ColumnName].ToString(),
                                FontFactory.GetFont(FontFactory.HELVETICA, 12)))
                            {
                                HorizontalAlignment = 0
                            };
                        }
                        else
                        {
                            cell = new PdfPCell(new Phrase(
                                dataRow[reportInfo.RecordEntryTable.Columns[i].ColumnName].ToString(),
                                FontFactory.GetFont(FontFactory.HELVETICA, 12)))
                            {
                                HorizontalAlignment = 1
                            };
                        }

                        table.AddCell(cell);
                    }
                }
                document.Add(table);
                document.Close();

                data = ms.ToArray();
            }

            return data;
        }

        private static void CreateHeader(string title, string date, Document doc)
        {
            var table = new PdfPTable(2)
            {
                WidthPercentage = 100,
                SpacingAfter = 15f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };

            var para1 = new PdfPCell(new Phrase(title, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13)))
            {
                HorizontalAlignment = 0,
                FixedHeight = 22f,
                BorderWidth = 0
            };
            table.AddCell(para1);

            var para2 = new PdfPCell(new Phrase(date, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13)))
            {
                HorizontalAlignment = 2,
                BorderWidth = 0
            };
            table.AddCell(para2);

            doc.Add(table);
        }
    }
}
