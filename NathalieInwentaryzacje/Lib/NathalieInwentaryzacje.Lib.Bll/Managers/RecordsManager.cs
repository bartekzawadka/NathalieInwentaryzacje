using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NathalieInwentaryzacje.Lib.Bll.Mappers;
using NathalieInwentaryzacje.Lib.Bll.Serializers;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Records;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;

namespace NathalieInwentaryzacje.Lib.Bll.Managers
{
    public class RecordsManager : IRecordsManager
    {
        private static readonly string RecordsPath = Path.Combine(Path.GetFullPath("."), "Data", "Inwentaryzacje");
        private static readonly string RecordDateDirFormat = "yyyy-MM-dd";

        private readonly ITemplatesManager _templatesManager = new TemplatesManager();

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
                var files = Directory.GetFiles(s, "*.xml");
                var recordInfo = new RecordListInfo();
                var recordItems = new List<RecordListItemInfo>();
                foreach (var file in files)
                {
                    var record = XmlFileSerializer.Deserialize<Record>(file);
                    recordItems.Add(new RecordListItemInfo
                    {
                        IsFilledIn = record.Entries != null && record.Entries.Length > 0,
                        Name = record.Name,
                        TemplateId = record.TemplateId,
                        RecordDate = record.RecordDate
                    });
                }

                recordInfo.RecordDate =
                    DateTime.ParseExact(s.Substring(s.LastIndexOf("\\", StringComparison.Ordinal) + 1), RecordDateDirFormat,
                        CultureInfo.InvariantCulture);
                recordInfo.RecordsInfo = recordItems;

                records.Add(recordInfo);
            });

            return records;
        }

        public RecordEntryInfo GetRecordEntry(DateTime recordDate, string recordEntryName)
        {
            var path = Path.Combine(RecordsPath, recordDate.ToString(RecordDateDirFormat));
            if (!Directory.Exists(path))
                throw new Exception("Nie odnaleziono inwentaryzacji na dzień " +
                                    recordDate.ToString(RecordDateDirFormat));

            var fName = recordEntryName;
            if (!recordEntryName.Contains(".xml"))
                fName += ".xml";

            var file = Path.Combine(path, fName);

            var record = XmlFileSerializer.Deserialize<Record>(file);
            var template = _templatesManager.GetTemplate(record.TemplateId);

            return RecordMapper.ToRecordEntryInfo(record);
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

            var recordsPath = Path.Combine(RecordsPath, recordInfo.RecordsDate.Value.ToString(RecordDateDirFormat));
            if (Directory.Exists(recordsPath))
            {
                throw new Exception("Inwentaryzacje na dzień " + recordInfo.RecordsDate.Value.ToString("yyyy-MM-dd") +
                                    " już istnieją! Proszę wybrać inną datę lub zmodyfikować istniejące zestawienia");
            }

            try
            {
                Directory.CreateDirectory(recordsPath);

                var templates = recordInfo.RecordTypes.Where(x => x.IsSelected).Select(x => x.TemplateInfo);

                foreach (var templateInfo in templates)
                {
                    var record = CreateRecordFromTemplate(templateInfo, recordInfo.RecordsDate.Value);
                    XmlFileSerializer.Serialize(record, Path.Combine(recordsPath, templateInfo.Name + ".xml"));
                }
            }
            catch
            {
                Directory.Delete(recordsPath, true);
                throw;
            }
        }

        public void SaveRecord(string recordEntryName, RecordEntryInfo recordEntryInfo)
        {
            var filePath = Path.Combine(RecordsPath, recordEntryInfo.RecordDate.ToString(RecordDateDirFormat),
                recordEntryName + ".xml");

            XmlFileSerializer.Serialize(RecordMapper.ToRecord(recordEntryInfo), filePath);
        }

        private Record CreateRecordFromTemplate(TemplateInfo info, DateTime? recordDate)
        {
            if (!info.IsEnabled)
            {
                throw new Exception("Ten szablon jest już niedostępny!");
            }

            if (recordDate == null)
            {
                throw new Exception("Data inwentaryzacji musi być określona");
            }

            return new Record
            {
                TemplateId = info.Id,
                Name = info.Name,
                RecordDate = recordDate.Value,
                RecordId = Guid.NewGuid().ToString()
            };
        }
    }
}
