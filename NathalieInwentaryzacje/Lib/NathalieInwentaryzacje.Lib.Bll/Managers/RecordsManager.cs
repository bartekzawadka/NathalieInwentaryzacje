using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NathalieInwentaryzacje.Lib.Bll.Serializers;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Records;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;

namespace NathalieInwentaryzacje.Lib.Bll.Managers
{
    public class RecordsManager : IRecordsManager
    {
        private readonly string _recordsPath = Path.Combine(Path.GetFullPath("."), "Data", "Inwentaryzacje");

        public IEnumerable<RecordInfo> GetRecords()
        {
            //            if (!Directory.Exists(_recordsPath))
            //                Directory.CreateDirectory(_recordsPath);
            //
            //            var years = Directory.GetDirectories(_recordsPath);
            //
            //            foreach (var year in years)
            //            {
            //                var list = GetFiles(year);
            //
            //                yield return new RecordInfo
            //                {
            //                    RecordDate = year.Substring(year.LastIndexOf("\\", StringComparison.Ordinal)+1),
            //                    RecordsInfo = list
            //                };
            //            }

            if (!Directory.Exists(_recordsPath))
                Directory.CreateDirectory(_recordsPath);

            var dirs = Directory.GetDirectories(_recordsPath);
            var records = new List<RecordInfo>();

            Parallel.ForEach(dirs, s =>
            {
                var files = Directory.GetFiles(s, "*.xml");
                var recordInfo = new RecordInfo();
                var recordItems = new List<RecordItemInfo>();
                foreach (var file in files)
                {
                    var record = XmlFileSerializer.Deserialize<Record>(file);
                    recordItems.Add(new RecordItemInfo
                    {
                        IsFilledIn = record.Entries != null && record.Entries.Length > 0,
                        Name = record.Name
                    });
                }

                recordInfo.RecordDate = s.Substring(s.LastIndexOf("\\", StringComparison.Ordinal) + 1);
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

            var recordsPath = Path.Combine(_recordsPath, recordInfo.RecordsDate.Value.ToString("yyyy-MM-dd"));
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
                Name = info.Name,
                RecordDate = recordDate.Value
            };
        }
//
//        private IEnumerable<RecordItemInfo> GetFiles(string path)
//        {
//            var files = Directory.GetFiles(path, "*.xml");
////            var recordInfo = new RecordInfo();
//            var recordItems = new List<RecordItemInfo>();
//            foreach (var file in files)
//            {
//                var record = XmlFileSerializer.Deserialize<Record>(file);
//                recordItems.Add(new RecordItemInfo
//                {
//                    IsFilledIn = record.Entries != null && record.Entries.Length > 0,
//                    Name = record.Name
//                });
//            }
//
////            recordInfo.RecordDate = path.Substring(path.LastIndexOf("\\", StringComparison.Ordinal) + 1);
////            recordInfo.RecordsInfo = recordItems;
////
////            return reco
//        }
    }
}
