using System.Collections.Generic;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Reports;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto.RecordAppendix
{
    public class RecordAppendixInfo : BaseReportInfo
    {
        public int AppendixNumber { get; set; } = 1;

        public IList<RecordAppendixSubSet> SubSets { get; set; }
    }
}
