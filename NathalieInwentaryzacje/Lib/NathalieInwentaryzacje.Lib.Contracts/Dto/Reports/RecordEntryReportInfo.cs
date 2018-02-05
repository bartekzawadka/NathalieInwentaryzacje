using System.Data;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto.Reports
{
    public class RecordEntryReportInfo
    {
        public string RecordDate { get; }

        public string RecordDisplayName { get; }

        public DataTable RecordEntryTable { get; }

        public RecordEntryReportInfo(string recordDate, string recordDisplayName, DataTable recordEntryTable)
        {
            RecordDate = recordDate;
            RecordDisplayName = recordDisplayName;
            RecordEntryTable = recordEntryTable;
        }
    }
}
