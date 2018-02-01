using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NathalieInwentaryzacje.Lib.Bll.Serializers;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Records;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Templates;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;

namespace NathalieInwentaryzacje.Lib.Bll
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

        public void CreateRecord(DateTime recordDate, IEnumerable<TemplateInfo> templates)
        {
            var recordsPath = Path.Combine(_recordsPath, recordDate.ToString("yyyy-MM-dd"));
            if (Directory.Exists(recordsPath))
            {
                throw new Exception("Inwentaryzacje na dzień " + recordDate.ToString("yyyy-MM-dd") +
                                    " już istnieją! Proszę wybrać inną datę lub zmodyfikować istniejące zestawienia");
            }

            try
            {
                Directory.CreateDirectory(recordsPath);

                foreach (var templateInfo in templates)
                {
                    var record = CreateRecordFromTemplate(templateInfo, recordDate);
                    XmlFileSerializer.Serialize(record, Path.Combine(recordsPath, templateInfo.Name + ".xml"));
                }
            }
            catch
            {
                Directory.Delete(recordsPath, true);
                throw;
            }
        }

//        public IEnumerable<RecordItemInfo> GetRecordItems(string subDir)
//        {
//            return GetFiles(Path.Combine(_recordsPath, subDir));
//        }

        private Record CreateRecordFromTemplate(TemplateInfo info, DateTime recordDate)
        {
            if (!info.IsEnabled)
            {
                throw new Exception("Ten szablon jest już niedostępny!");
            }

            return new Record
            {
                Name = info.Name,
                RecordDate = recordDate
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
