using System;
using System.Data;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto.Reports
{
    public class RecordEntryReportInfo : BaseReportInfo
    {
        public string RecordDisplayName { get; }

        public DataTable RecordEntryTable { get; }

        public RecordEntryReportInfo(DateTime recordDate, string recordDisplayName, DataTable recordEntryTable)
        {
            RecordDate = recordDate;
            RecordDisplayName = recordDisplayName;
            RecordEntryTable = recordEntryTable;
        }
    }
}
