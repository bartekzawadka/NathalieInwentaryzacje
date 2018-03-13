using NathalieInwentaryzacje.Lib.Contracts.Dto.Reports;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto.RecordSummary
{
    public class RecordSummaryInfo : BaseReportInfo
    {
        public RecordsTotalsInfo TotalsDataset { get; set; }
    }
}
