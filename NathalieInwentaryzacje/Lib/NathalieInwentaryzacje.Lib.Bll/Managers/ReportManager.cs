using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NathalieInwentaryzacje.Common.Utils.Extensions;
using NathalieInwentaryzacje.Lib.Bll.Reporting;
using NathalieInwentaryzacje.Lib.Bll.Reporting.Builders;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Reports;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Reports.RecordAppendix;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Reports.RecordSummary;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Settings;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;

namespace NathalieInwentaryzacje.Lib.Bll.Managers
{
    public class ReportManager : ManagerBase, IReportManager
    {
        private static readonly string ArialuniTff = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIALUNI.TTF");
        private readonly RecordsManager _recordsManager;

        public ReportManager(DataLocationInfo pathInfos) : base(pathInfos)
        {
            _recordsManager = new RecordsManager(pathInfos);
        }

        public void GenerateReports(DateTime recordDate, IEnumerable<GenerateReportEntryInfo> reportEntryInfos, string saveDir, int numberOfItemsPerPage = 40)
        {
            if (!Directory.Exists(saveDir))
                throw new DirectoryNotFoundException("Nie można odnaleźć folderu '" + saveDir + "'");

            foreach (var generateReportEntryInfo in reportEntryInfos)
            {
                var dt = _recordsManager.RecordToDataTable(generateReportEntryInfo.RecordListInfo.RecordDate,
                    generateReportEntryInfo.RecordListInfo.FilePath);

                var buff = BuildReport(new RecordEntryReportInfo(
                    generateReportEntryInfo.RecordListInfo.RecordDate,
                    generateReportEntryInfo.RecordListInfo.DisplayName?.ToUpper(), dt), numberOfItemsPerPage);

                var fileName = Path.GetFileNameWithoutExtension(generateReportEntryInfo.RecordListInfo.FilePath) + "_" +
                               recordDate.ToRecordDateString() +
                               ".pdf";
                var path = Path.Combine(saveDir, fileName);
                File.Delete(path);

                using (var fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fs.Write(buff, 0, buff.Length);
                }
            }
        }

        private static byte[] BuildReport(RecordEntryReportInfo reportInfo, int numberOfItemsPerPage = 40)
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
                var transferedValue = 0.0;

                for (var i = 0; i < partsCount; i++)
                {
                    var set = reportInfo.RecordEntryTable.Select().Skip(i * numberOfItemsPerPage)
                        .Take(numberOfItemsPerPage).ToList();

                    if (set.Count <= 0) continue;

                    CreateHeader(reportInfo.RecordDisplayName, reportInfo.RecordDate, document);

                    rowCount = BuildDataPage(reportInfo.RecordEntryTable.Columns, set, document, rowCount, i > 0,
                        ref transferedValue);
                    document.NewPage();
                }

                document.Close();

                data = ms.ToArray();
            }

            return data;
        }

        public static byte[] BuildRecordAnnexReport(RecordAppendixInfo rai)
        {
            var builder = new RecordAppendixReportBuilder();
            return builder.BuildReport(rai, true);
        }

        public static byte[] BuildRecordSummaryReport(RecordSummaryInfo rai)
        {
            var builder = new RecordSummaryReportBuilder();
            return builder.BuildReport(rai, false);
        }

        private static int BuildDataPage(DataColumnCollection columns, List<DataRow> rows, Document document, int rowCount, bool addPreviousSummaryRow, ref double transferedValue)
        {
            var table = new PdfPTable(columns.Count);
            var widths = new float[columns.Count];
            for (var i = 0; i < widths.Length; i++)
            {
                if (i == 0)
                {
                    widths[i] = 1.2f;
                }
                else if (i == 1 || i == 2)
                {
                    widths[i] = 4f;
                }
                else if (i == 3 || i == 4)
                {
                    widths[i] = 2f;
                }
                else
                {
                    widths[i] = 3f;
                }
            }

            var font = new Font(BaseFont.CreateFont(ArialuniTff, BaseFont.CP1250, true));
            var fontBold = new Font(BaseFont.CreateFont(ArialuniTff, BaseFont.CP1250, true), 11, Font.BOLD);

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

                var preSumValue = new PdfPCell(new Phrase(transferedValue.ToString("C"), fontBold))
                {
                    HorizontalAlignment = 2,
                    VerticalAlignment = 5
                };
                table.AddCell(preSumValue);
            }

            foreach (var dataRow in rows)
            {
                for (var i = 0; i < columns.Count; i++)
                {
                    PdfPCell cell;
                    if (i == 1)
                    {
                        cell = new PdfPCell(new Phrase(
                            dataRow[columns[i].ColumnName].ToString(),
                            font))
                        {
                            HorizontalAlignment = 0,
                            VerticalAlignment = 5
                        };
                    }
                    else if (i == columns.Count - 1)
                    {
                        cell = new PdfPCell(new Phrase(
                            dataRow[columns[i].ColumnName].ToString(),
                            font))
                        {
                            HorizontalAlignment = 2,
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

            var sum = transferedValue;

            foreach (var dataRow in rows)
            {
                var cell = dataRow[columns.Count - 1]?.ToString().Replace("zł", "");
                if (double.TryParse(cell, out var val))
                {
                    sum += val;
                }
            }

            transferedValue = sum;

            var summaryValueCell = new PdfPCell(new Phrase(sum.ToString("C"), fontBold))
            {
                HorizontalAlignment = 2,
                VerticalAlignment = 5
            };

            table.AddCell(summaryValueCell);

            table.SetWidths(widths);
            table.WidthPercentage = 100;

            document.Add(table);

            return rowCount;
        }

        private static void CreateHeader(string title, DateTime date, Document doc)
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

            var para2 = new PdfPCell(new Phrase("Stan na dzień: " + date.ToRecordDateString(), font))
            {
                HorizontalAlignment = 2,
                BorderWidth = 0
            };
            table.AddCell(para2);

            doc.Add(table);
        }
    }
}
