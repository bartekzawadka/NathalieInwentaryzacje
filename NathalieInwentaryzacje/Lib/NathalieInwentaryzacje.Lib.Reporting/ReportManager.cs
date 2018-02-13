using System;
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
        private readonly string _arialuniTff = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIALUNI.TTF");

        public byte[] BuildReport(RecordEntryReportInfo reportInfo)
        {
            byte[] data;

            var document = new Document(PageSize.A4);
            using (var ms = new MemoryStream())
            {
                var writer = PdfWriter.GetInstance(document, ms);
                writer.PageEvent = new PdfFooter();
                document.Open();

                document.NewPage();

                CreateHeader(reportInfo.RecordDisplayName, reportInfo.RecordDate, document);

                var table = new PdfPTable(reportInfo.RecordEntryTable.Columns.Count+1);
                var widths = new float[reportInfo.RecordEntryTable.Columns.Count+1];
                widths[0] = 1f;
                for (var i = 1; i < widths.Length; i++)
                {
                    widths[i] = 4f;
                }

                var lpCell = new PdfPCell(new Phrase("Lp", FontFactory.GetFont(_arialuniTff, 12, Font.BOLD)))
                {
                    HorizontalAlignment = 1,
                    BackgroundColor = BaseColor.LIGHT_GRAY
                };

                table.AddCell(lpCell);

                foreach (DataColumn dataColumn in reportInfo.RecordEntryTable.Columns)
                {
                    var cell = new PdfPCell(new Phrase(dataColumn.ColumnName,
                        FontFactory.GetFont(_arialuniTff, 12, Font.BOLD)))
                    {
                        HorizontalAlignment = 1,
                        BackgroundColor = BaseColor.LIGHT_GRAY
                    };
                    table.AddCell(cell);
                }

                var rowCount = 1;

                foreach (DataRow dataRow in reportInfo.RecordEntryTable.Rows)
                {
                    var lpValue = new PdfPCell(new Phrase(rowCount.ToString(),
                        FontFactory.GetFont(_arialuniTff, 12, Font.NORMAL)))
                    {
                        HorizontalAlignment = 1
                    };

                    table.AddCell(lpValue);

                    for (var i = 0; i < reportInfo.RecordEntryTable.Columns.Count; i++)
                    {
                        PdfPCell cell;
                        if (i == 0)
                        {
                            cell = new PdfPCell(new Phrase(
                                dataRow[reportInfo.RecordEntryTable.Columns[i].ColumnName].ToString(),
                                FontFactory.GetFont(_arialuniTff, 12, Font.NORMAL)))
                            {
                                HorizontalAlignment = 0
                            };
                        }
                        else
                        {
                            cell = new PdfPCell(new Phrase(
                                dataRow[reportInfo.RecordEntryTable.Columns[i].ColumnName].ToString(),
                                FontFactory.GetFont(_arialuniTff, 12, Font.NORMAL)))
                            {
                                HorizontalAlignment = 1
                            };
                        }

                        table.AddCell(cell);
                    }

                    rowCount++;
                }

                var summaryCell = new PdfPCell(new Phrase("SUMA", FontFactory.GetFont(_arialuniTff, 12, Font.BOLD)))
                {
                    HorizontalAlignment = 0,
                    Colspan = table.NumberOfColumns - 1
                };
                table.AddCell(summaryCell);

                var summaryValueCell = new PdfPCell(new Phrase("WARTOŚĆ", FontFactory.GetFont(_arialuniTff, 12, Font.BOLD)))
                {
                    HorizontalAlignment = 1
                };

                table.AddCell(summaryValueCell);

                table.SetWidths(widths);
                table.WidthPercentage = 100;

                document.Add(table);

                document.NewPage();

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

            var para2 = new PdfPCell(new Phrase("Stan na dzień: " + date, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13)))
            {
                HorizontalAlignment = 2,
                BorderWidth = 0
            };
            table.AddCell(para2);

            doc.Add(table);
        }
    }
}
