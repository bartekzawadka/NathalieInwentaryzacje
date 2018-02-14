using NathalieInwentaryzacje.Lib.Contracts.Dto.Reports;

namespace NathalieInwentaryzacje.Lib.Contracts.Interfaces
{
    public interface IReportManager
    {
        byte[] BuildReport(RecordEntryReportInfo reportInfo, int numberOfItemsPerPage = 40);
    }
}
