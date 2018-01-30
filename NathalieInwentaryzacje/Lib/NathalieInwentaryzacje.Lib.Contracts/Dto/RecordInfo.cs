using System;
using System.Collections.Generic;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto
{
    public class RecordInfo
    {
        public string RecordDate { get; set; }

        public string RecordTitle => "Stan na dzień: " + RecordDate;

        public IEnumerable<RecordItemInfo> RecordsInfo { get; set; }
    }
}
