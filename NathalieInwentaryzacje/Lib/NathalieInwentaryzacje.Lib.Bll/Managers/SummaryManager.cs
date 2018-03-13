using System;
using System.IO;
using System.Linq;
using NathalieInwentaryzacje.Common.Utils.Extensions;
using NathalieInwentaryzacje.Lib.Bll.Mappers;
using NathalieInwentaryzacje.Lib.Bll.Serializers;
using NathalieInwentaryzacje.Lib.Contracts.Dto.RecordSummary;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Settings;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Summary;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;

namespace NathalieInwentaryzacje.Lib.Bll.Managers
{
    public class SummaryManager : ManagerBase, ISummaryManager
    {
        private readonly RecordsManager _recordsManager = new RecordsManager(Paths);

        public SummaryManager(DataLocationInfo paths) : base(paths)
        {
        }

        public RecordSummaryInfo GetSummary(DateTime recordDate)
        {
            if (recordDate == new DateTime())
            {
                throw new ArgumentNullException(nameof(recordDate), "Data inwentaryzacji nie została wskazana");
            }

            var path = Path.Combine(Paths.RecordsPath, recordDate.ToRecordDateString(), "Summary.xml");

            var rsi = new RecordSummaryInfo
            {
                TotalsDataset = new RecordsTotalsInfo
                {
                    Rows = _recordsManager.GetRecordSums(recordDate)
                },
                RecordDate = recordDate
            };

            if (File.Exists(path))
            {
                var summary = XmlFileSerializer.Deserialize<Summary>(path);

                var rsiNames = rsi.TotalsDataset.Rows.Select(x => x.Name).ToList();

                foreach (var summaryEntry in summary.Entries)
                {
                    if (!rsiNames.Contains(summaryEntry.Name))
                    {
                        rsi.TotalsDataset.Rows.Add(new RecordTotalRowInfo
                        {
                            Name = summaryEntry.Name,
                            Value = summaryEntry.Value,
                            AdditionalInfo = summaryEntry.AdditionalInfo,
                            IsReadOnly = summaryEntry.IsReadOnly
                        });
                    }
                }
            }

            SaveSummary(rsi);

            return rsi;
        }

        public void SaveSummary(RecordSummaryInfo info)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info), "Obiekt danych do raportu zbiorczego nie może być pusty");

            if (info.RecordDate == new DateTime())
                throw new ArgumentNullException(nameof(info.RecordDate), "Data inwentaryzacji nie została określona");

            XmlFileSerializer.Serialize(SummaryMapper.ToSummary(info),
                Path.Combine(Paths.RecordsPath, info.RecordDate.ToRecordDateString(), "Summary.xml"));
        }
    }
}
