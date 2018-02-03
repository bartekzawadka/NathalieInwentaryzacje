using System;
using System.Collections.Generic;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto
{
    public class RecordListInfo
    {
        public DateTime RecordDate { get; set; }

        public string RecordTitle => "Stan na dzień: " + RecordDate;

        public IEnumerable<RecordListItemInfo> RecordsInfo { get; set; }
    }
}
