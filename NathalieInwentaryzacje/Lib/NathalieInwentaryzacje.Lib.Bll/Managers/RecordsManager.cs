using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NathalieInwentaryzacje.Common.Utils.Extensions;
using NathalieInwentaryzacje.Lib.Bll.Serializers;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Records;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Reports;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
using OfficeOpenXml;

namespace NathalieInwentaryzacje.Lib.Bll.Managers
{
    public class RecordsManager : IRecordsManager
    {
        private static readonly string RecordsPath = Path.Combine(Path.GetFullPath("."), "Data", "Inwentaryzacje");
        private static readonly string TemplatesPath = Path.Combine(Path.GetFullPath("."), "Data", "Szablony");

        static RecordsManager()
        {
            if (!Directory.Exists(RecordsPath))
                Directory.CreateDirectory(RecordsPath);
        }

        public IEnumerable<RecordListInfo> GetRecords()
        {
            var dirs = Directory.GetDirectories(RecordsPath);
            var records = new List<RecordListInfo>();

            Parallel.ForEach(dirs, s =>
            {
                var recordXmlPath = Path.Combine(s, "record.xml");
                var recordInfo = new RecordListInfo();

                var record = XmlFileSerializer.Deserialize<Record>(recordXmlPath);
                var recordItems = record.Entries.Select(recordEntry => new RecordListItemInfo
                {
                    DisplayName = recordEntry.DisplayName,
                    FilePath = recordEntry.FilePath,
                    TemplateId = recordEntry.TemplateId,
                    IsFilledIn = recordEntry.IsFilledIn,
                    RecordDate = record.RecordDate
                })
                    .ToList();

                recordInfo.RecordDate = record.RecordDate;
                recordInfo.RecordsInfo = recordItems;

                records.Add(recordInfo);
            });

            return records;
        }

        public void CreateRecord(NewRecordInfo recordInfo)
        {
            if (recordInfo == null)
            {
                throw new Exception("Obiekt nowej inwentaryzacji nie został zdefiniowany");
            }

            if (recordInfo.RecordsDate == null)
            {
                throw new Exception("Data inwentaryzacji nie może być pusta!");
            }

            if (recordInfo.RecordTypes == null)
            {
                throw new Exception("Nie wskazano szablonów inwentaryzacji");
            }

            var recordsPath = Path.Combine(RecordsPath, recordInfo.RecordsDate.ToRecordDateString());
            if (Directory.Exists(recordsPath))
            {
                throw new Exception("Inwentaryzacje na dzień " + recordInfo.RecordsDate.ToRecordDateString() +
                                    " już istnieją! Proszę wybrać inną datę lub zmodyfikować istniejące zestawienia");
            }

            try
            {
                Directory.CreateDirectory(recordsPath);

                var templates = recordInfo.RecordTypes.Where(x => x.IsSelected).Select(x => x.TemplateInfo);

                var record = new Record();
                var entries = new List<RecordEntry>();

                foreach (var templateInfo in templates)
                {
                    var templateFullPath = Path.Combine(TemplatesPath, templateInfo.TemplateFilePath);
                    File.Copy(templateFullPath, Path.Combine(recordsPath, templateInfo.TemplateFilePath));

                    entries.Add(new RecordEntry
                    {
                        DisplayName = templateInfo.Name,
                        FilePath = templateInfo.TemplateFilePath,
                        TemplateId = templateInfo.Id
                    });
                }

                record.Entries = entries.ToArray();
                record.RecordId = Guid.NewGuid().ToString();
                record.RecordDate = recordInfo.RecordsDate.Value;

                XmlFileSerializer.Serialize(record, Path.Combine(recordsPath, "record.xml"));
            }
            catch
            {
                Directory.Delete(recordsPath, true);
                throw;
            }
        }

        public void OpenRecordFileEdit(DateTime recordDate, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName), "Nazwa pliku inwentaryzacji nie może być pusta");

            var path = Path.Combine(RecordsPath, recordDate.ToRecordDateString());

            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException("Nie odnaleziono inwentaryzacji na dzień " +
                                                     recordDate.ToRecordDateString());

            path = Path.Combine(RecordsPath, recordDate.ToRecordDateString(), fileName);
            if (!File.Exists(path))
                throw new FileNotFoundException("Nie odnaleziono pliku o nazwie '" + fileName +
                                                "' w inwentaryzacji z dnia " + recordDate.ToRecordDateString());

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = path,
                    WindowStyle = ProcessWindowStyle.Maximized
                }
            };

            process.Start();
            process.WaitForExit();

            UpdateRecordFile(recordDate, fileName, path);
        }

        public RecordEntryReportInfo GetRecordReportInfo(DateTime recordDate, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName), "Nazwa pliku inwentaryzacji nie może być pusta");

            var basePath = Path.Combine(RecordsPath, recordDate.ToRecordDateString());

            var path = Path.Combine(basePath, fileName);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Nie odnaleziono pliku '" + fileName +
                                                "' dla inwentaryzacji na dzień " + recordDate.ToRecordDateString());
            }

            var recordXmlPath = Path.Combine(basePath, "record.xml");
            var record = XmlFileSerializer.Deserialize<Record>(recordXmlPath);
            var recordEntry = record.Entries.Single(x => string.Equals(x.FilePath?.ToLower(), fileName.ToLower()));

            DataTable dt;

            var fInfo = new FileInfo(path);
            using (var package = new ExcelPackage(fInfo))
            {
                if (package.Workbook.Worksheets == null || package.Workbook.Worksheets.Count == 0)
                {
                    throw new Exception("Plik '" + fileName +
                                        "' ma nieprawidłową strukturę. Nie odnaleziono żadnej zakładki");
                }

                var sheet = package.Workbook.Worksheets[1];

                dt = WorksheetToDataTable(sheet);
            }

            return new RecordEntryReportInfo(recordDate.ToRecordDateString(), recordEntry.DisplayName, dt);
        }

        private static DataTable WorksheetToDataTable(ExcelWorksheet ws)
        {
            var dt = new DataTable(ws.Name);
            var totalCols = ws.Dimension.End.Column;
            var totalRows = ws.Dimension.End.Row;
            const int startRow = 2;

            foreach (var firstRowCell in ws.Cells[1,1,1,totalCols])
            {
                dt.Columns.Add(firstRowCell.Text);
            }

            for (var i = startRow; i < totalRows; i++)
            {
                var wsRow = ws.Cells[i, 1, i, totalCols];
                var dr = dt.NewRow();
                foreach (var cell in wsRow)
                {
                    dr[cell.Start.Column - 1] = cell.Text;
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }

        private static void UpdateRecordFile(DateTime recordDate, string fileName, string fullPath)
        {
            var hasBeenUpdated = false;

            var fInfo = new FileInfo(fullPath);
            using (var package = new ExcelPackage(fInfo))
            {
                if (package.Workbook.Worksheets == null || package.Workbook.Worksheets.Count == 0)
                {
                    throw new Exception("Plik '" + fileName +
                                        "' ma nieprawidłową strukturę. Nie odnaleziono żadnej zakładki");
                }

                var sheet = package.Workbook.Worksheets[1];
                if(sheet.Dimension.Rows>1)
                {
                    hasBeenUpdated = true;
                }
            }

            var recordFile = Path.Combine(RecordsPath, recordDate.ToRecordDateString(), "record.xml");
            var record = XmlFileSerializer.Deserialize<Record>(recordFile);

            if (record.Entries == null || record.Entries.Length <= 0) return;

            foreach (var t in record.Entries)
            {
                if (string.Equals(t.FilePath?.ToLower(), fileName.ToLower()))
                {
                    t.IsFilledIn = hasBeenUpdated;
                }
            }

            XmlFileSerializer.Serialize(record, recordFile);
        }
    }
}
