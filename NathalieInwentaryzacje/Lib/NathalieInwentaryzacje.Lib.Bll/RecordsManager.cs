using System;
using System.Collections.Generic;
using System.IO;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;

namespace NathalieInwentaryzacje.Lib.Bll
{
    public class RecordsManager : IRecordsManager
    {
        private readonly string _recordsPath = Path.Combine(Path.GetFullPath("."), "Data", "Inwentaryzacje");

        public IEnumerable<RecordInfo> GetRecords()
        {
            if (!Directory.Exists(_recordsPath))
                Directory.CreateDirectory(_recordsPath);

            var years = Directory.GetDirectories(_recordsPath);

            foreach (var year in years)
            {
                var list = GetFiles(year);

                yield return new RecordInfo
                {
                    RecordDate = year.Substring(year.LastIndexOf("\\", StringComparison.Ordinal)+1),
                    RecordsInfo = list
                };
            }
        }

        public IEnumerable<RecordItemInfo> GetRecordItems(string subDir)
        {
            return GetFiles(Path.Combine(_recordsPath, subDir));
        }

        private IEnumerable<RecordItemInfo> GetFiles(string path)
        {
            var files = Directory.GetFiles(path, "*.xml");
            foreach (var file in files)
            {
                var info = new FileInfo(file);
                yield return new RecordItemInfo
                {
                    CreationDate = info.CreationTime,
                    ModificationDate = info.LastWriteTime,
                    Name = info.Name.Substring(0, info.Name.LastIndexOf(".", StringComparison.Ordinal))
                };
            }

        }
    }
}
