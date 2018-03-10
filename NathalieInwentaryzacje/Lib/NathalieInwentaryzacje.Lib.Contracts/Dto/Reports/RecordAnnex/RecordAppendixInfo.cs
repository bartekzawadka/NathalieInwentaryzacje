using System.Collections.Generic;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto.Reports.RecordAnnex
{
    public class RecordAppendixInfo : BaseReportInfo
    {
        public int AppendixNumber { get; set; } = 1;

        public IList<RecordAppendixSubSet> SubSets { get; set; }
    }
}
