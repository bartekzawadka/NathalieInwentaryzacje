using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Reports;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;

namespace NathalieInwentaryzacje.Lib.Reporting
{
    public class ReportManager : IReportManager
    {
        private static readonly string ArialuniTff = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIALUNI.TTF");

        public byte[] BuildReport(RecordEntryReportInfo reportInfo, int numberOfItemsPerPage = 40)
        {
            byte[] data;

            var document = new Document(PageSize.A4);
            using (var ms = new MemoryStream())
            {
                var writer = PdfWriter.GetInstance(document, ms);
                writer.PageEvent = new PdfFooter();
                document.Open();

                var partsCount = reportInfo.RecordEntryTable.Rows.Count / numberOfItemsPerPage + 1;
                var rowCount = 1;

                for (var i = 0; i < partsCount; i++)
                {
                    var set = reportInfo.RecordEntryTable.Select().Skip(i * numberOfItemsPerPage)
                        .Take(numberOfItemsPerPage).ToList();

                    if (set.Count <= 0) continue;

                    CreateHeader(reportInfo.RecordDisplayName, reportInfo.RecordDate, document);

                    rowCount = BuildDataPage(reportInfo.RecordEntryTable.Columns, set, document, rowCount, i>0);
                    document.NewPage();
                }

                document.Close();

                data = ms.ToArray();
            }

            return data;
        }

        private static int BuildDataPage(DataColumnCollection columns, List<DataRow> rows, Document document, int rowCount, bool addPreviousSummaryRow)
        {
            var table = new PdfPTable(columns.Count + 1);
            var widths = new float[columns.Count + 1];
            widths[0] = 1f;
            for (var i = 1; i < widths.Length; i++)
            {
                if (i == 1)
                {
                    widths[i] = 5f;
                }
                else
                {
                    widths[i] = 3f;
                }
            }

            var font = new Font(BaseFont.CreateFont(ArialuniTff, BaseFont.CP1250, true));
            var fontBold = new Font(BaseFont.CreateFont(ArialuniTff, BaseFont.CP1250, true), 11, Font.BOLD);

            var lpCell = new PdfPCell(new Phrase("Lp", font))
            {
                HorizontalAlignment = 1,
                VerticalAlignment = 2,
                BackgroundColor = BaseColor.LIGHT_GRAY
            };

            table.AddCell(lpCell);

            foreach (DataColumn dataColumn in columns)
            {
                var cell = new PdfPCell(new Phrase(dataColumn.ColumnName,
                    font))
                {
                    HorizontalAlignment = 1,
                    VerticalAlignment = 5,
                    BackgroundColor = BaseColor.LIGHT_GRAY
                };
                table.AddCell(cell);
            }

            if (addPreviousSummaryRow)
            {
                var preSumTitle = new PdfPCell(new Phrase("Z PRZENIESIENIA", fontBold))
                {
                    HorizontalAlignment = 0,
                    VerticalAlignment = 5,
                    Colspan = table.NumberOfColumns - 1
                };
                table.AddCell(preSumTitle);

                var preSumValue = new PdfPCell(new Phrase("WARTOŚĆ", fontBold))
                {
                    HorizontalAlignment = 1,
                    VerticalAlignment = 5
                };
                table.AddCell(preSumValue);
            }

            foreach (DataRow dataRow in rows)
            {
                var lpValue = new PdfPCell(new Phrase(rowCount.ToString(),
                    font))
                {
                    HorizontalAlignment = 1,
                    VerticalAlignment = 5
                };

                table.AddCell(lpValue);

                for (var i = 0; i < columns.Count; i++)
                {
                    PdfPCell cell;
                    if (i == 0)
                    {
                        cell = new PdfPCell(new Phrase(
                            dataRow[columns[i].ColumnName].ToString(),
                            font))
                        {
                            HorizontalAlignment = 0,
                            VerticalAlignment = 5
                        };
                    }
                    else
                    {
                        cell = new PdfPCell(new Phrase(
                            dataRow[columns[i].ColumnName].ToString(),
                            font))
                        {
                            HorizontalAlignment = 1,
                            VerticalAlignment = 5
                        };
                    }

                    table.AddCell(cell);
                }

                rowCount++;
            }

            var summaryCell = new PdfPCell(new Phrase("SUMA", fontBold))
            {
                HorizontalAlignment = 0,
                VerticalAlignment = 5,
                Colspan = table.NumberOfColumns - 1
            };
            table.AddCell(summaryCell);

            var summaryValueCell = new PdfPCell(new Phrase("WARTOŚĆ", fontBold))
            {
                HorizontalAlignment = 1,
                VerticalAlignment = 5
            };

            table.AddCell(summaryValueCell);

            table.SetWidths(widths);
            table.WidthPercentage = 100;

            document.Add(table);

            return rowCount;
        }

        private static void CreateHeader(string title, string date, Document doc)
        {
            var table = new PdfPTable(2)
            {
                WidthPercentage = 100,
                SpacingAfter = 15f,
                HorizontalAlignment = Element.ALIGN_CENTER
            };

            var font = new Font(BaseFont.CreateFont(ArialuniTff, BaseFont.CP1250, true), 13, Font.BOLD);

            var para1 = new PdfPCell(new Phrase(title, font))
            {
                HorizontalAlignment = 0,
                FixedHeight = 22f,
                BorderWidth = 0
            };
            table.AddCell(para1);

            var para2 = new PdfPCell(new Phrase("Stan na dzień: " + date, font))
            {
                HorizontalAlignment = 2,
                BorderWidth = 0
            };
            table.AddCell(para2);

            doc.Add(table);
        }
    }
}
