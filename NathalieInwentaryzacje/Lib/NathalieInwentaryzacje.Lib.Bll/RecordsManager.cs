using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;

namespace NathalieInwentaryzacje.Lib.Bll
{
    public class RecordsManager : IRecordsManager
    {
        public Dictionary<string, List<RecordInfo>> GetRecords()
        {
            var path = Path.GetFullPath(".");
            path = Path.Combine(path, "Data", "Inwentaryzacje");

            if (!Directory.Exists(path))
                return null;

            var years = Directory.GetDirectories(path);
            var results = new Dictionary<string, List<RecordInfo>>();

            foreach (var year in years)
            {
                var files = Directory.GetFiles(year, "*.xml");
                var list = new List<RecordInfo>();
                foreach (var file in files)
                {
                    var info = new FileInfo(file);
                    list.Add(new RecordInfo
                    {
                        CreationDate = info.CreationTime,
                        ModificationDate = info.LastWriteTime,
                        Year = info.CreationTime.Year,
                        Name = info.Name
                    });
                }

                results.Add(year.Substring(year.LastIndexOf("\\", StringComparison.Ordinal)+1), list);
            }

            return results;
        }
    }
}
