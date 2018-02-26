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
using NathalieInwentaryzacje.Lib.Contracts.Dto.Settings;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
using OfficeOpenXml;

namespace NathalieInwentaryzacje.Lib.Bll.Managers
{
    public class RecordsManager : ManagerBase, IRecordsManager
    {
        private readonly ITemplatesManager _templatesManager = new TemplatesManager(Paths);

        public RecordsManager(DataLocationInfo pathInfos) : base(pathInfos)
        {
        }

        public Task<IEnumerable<RecordListInfo>> GetRecords()
        {
            return Task<IEnumerable<RecordListInfo>>.Factory.StartNew(() =>
            {
                var dirs = Directory.GetDirectories(Paths.RecordsPath);
                var records = new List<RecordListInfo>();

                var availableTemplatesCount = _templatesManager.GetTemplates().Count();

                foreach (var s in dirs)
                {
                    var recordXmlPath = Path.Combine(s, "record.xml");
                    var recordInfo = new RecordListInfo();

                    var record = XmlFileSerializer.Deserialize<Record>(recordXmlPath);

                    foreach (var recordEntry in record.Entries)
                    {
                        UpdateRecordFile(record.RecordDate, recordEntry.FilePath,
                            Path.Combine(s, recordEntry.FilePath));
                    }

                    record = XmlFileSerializer.Deserialize<Record>(recordXmlPath);

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
                    recordInfo.CanAddEntries = recordItems.Count < availableTemplatesCount;
                    recordInfo.RecordId = record.RecordId;

                    records.Add(recordInfo);
                }

                return records;
            });
        }

        public void CreateOrUpdateRecord(RecordInfo recordInfo)
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

            var recordsPath = Path.Combine(Paths.RecordsPath, recordInfo.RecordsDate.ToRecordDateString());
            var recordInfoFilePath = Path.Combine(recordsPath, "record.xml");

            var templates = recordInfo.RecordTypes.Where(x => x.IsSelected).Select(x => x.TemplateInfo);

            try
            {
                if (string.IsNullOrEmpty(recordInfo.Id))
                {
                    if (Directory.Exists(recordsPath))
                    {
                        throw new Exception("Inwentaryzacje na dzień " + recordInfo.RecordsDate.ToRecordDateString() +
                                            " już istnieją! Proszę wybrać inną datę lub zmodyfikować istniejące zestawienia");
                    }

                    Directory.CreateDirectory(recordsPath);

                    var record = new Record();
                    var entries = new List<RecordEntry>();

                    foreach (var templateInfo in templates)
                    {
                        var templateFullPath = Path.Combine(Paths.TemplatesPath, templateInfo.TemplateFilePath);
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

                    XmlFileSerializer.Serialize(record, recordInfoFilePath);
                }
                else
                {
                    var record = XmlFileSerializer.Deserialize<Record>(recordInfoFilePath);

                    var existingIds = record.Entries.Select(x => x.TemplateId).ToList();

                    var entriesToAdd = new List<RecordEntry>();

                    foreach (var templateInfo in templates)
                    {
                        if (!existingIds.Contains(templateInfo.Id))
                        {
                            var templateFullPath = Path.Combine(Paths.TemplatesPath, templateInfo.TemplateFilePath);
                            File.Copy(templateFullPath, Path.Combine(recordsPath, templateInfo.TemplateFilePath));

                            entriesToAdd.Add(new RecordEntry
                            {
                                DisplayName = templateInfo.Name,
                                FilePath = templateInfo.TemplateFilePath,
                                TemplateId = templateInfo.Id
                            });
                        }
                    }

                    var entries = record.Entries.ToList();
                    entries.AddRange(entriesToAdd);
                    record.Entries = entries.ToArray();

                    XmlFileSerializer.Serialize(record, recordInfoFilePath);
                }
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

            var path = Path.Combine(Paths.RecordsPath, recordDate.ToRecordDateString());

            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException("Nie odnaleziono inwentaryzacji na dzień " +
                                                     recordDate.ToRecordDateString());

            path = Path.Combine(Paths.RecordsPath, recordDate.ToRecordDateString(), fileName);
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


        public DataTable RecordToDataTable(DateTime recordDate, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName), "Nazwa pliku inwentaryzacji nie może być pusta");

            var basePath = Path.Combine(Paths.RecordsPath, recordDate.ToRecordDateString());

            var path = Path.Combine(basePath, fileName);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Nie odnaleziono pliku '" + fileName +
                                                "' dla inwentaryzacji na dzień " + recordDate.ToRecordDateString());
            }

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

            return dt;
        }

        private static DataTable WorksheetToDataTable(ExcelWorksheet ws)
        {
            var dt = new DataTable(ws.Name);
            var totalCols = ws.Dimension.End.Column;
            var totalRows = ws.Dimension.End.Row;
            const int startRow = 2;

            foreach (var firstRowCell in ws.Cells[1, 1, 1, totalCols])
            {
                dt.Columns.Add(firstRowCell.Text);
            }

            for (var i = startRow; i <= totalRows; i++)
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
                if (sheet.Dimension.Rows > 1 && sheet.Cells[2, 1]?.Value != null)
                {
                    hasBeenUpdated = true;
                }
            }

            var recordFile = Path.Combine(Paths.RecordsPath, recordDate.ToRecordDateString(), "record.xml");
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
