using System.Linq;
using NathalieInwentaryzacje.Lib.Contracts.Dto.RecordSummary;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Summary;

namespace NathalieInwentaryzacje.Lib.Bll.Mappers
{
    public class SummaryMapper
    {
        public static Summary ToSummary(RecordSummaryInfo info)
        {
            return new Summary
            {
                Sum = info.TotalsDataset.Sum,
                Entries = info.TotalsDataset.Rows?.Select(x => new SummaryEntry
                {
                    Name = x.Name,
                    Value = x.Value,
                    AdditionalInfo = x.AdditionalInfo,
                    IsReadOnly = x.IsReadOnly
                }).ToArray()
            };
        }

        public static RecordSummaryInfo ToSummaryInfo(Summary summary)
        {
            return new RecordSummaryInfo
            {
                TotalsDataset = new RecordsTotalsInfo
                {
                    Rows = summary.Entries?.Select(x => new RecordTotalRowInfo
                    {
                        Value = x.Value,
                        Name = x.Name,
                        AdditionalInfo = x.AdditionalInfo,
                        IsReadOnly = x.IsReadOnly
                    }).ToList()
                }
            };
        }
    }
}
