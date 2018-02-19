using System;
using System.Collections.Generic;
using NathalieInwentaryzacje.Common.Utils.Extensions;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto
{
    public class RecordListInfo
    {
        public DateTime RecordDate { get; set; }

        public string RecordTitle => "Stan na dzień: " + RecordDate.ToRecordDateString();

        public IEnumerable<RecordListItemInfo> RecordsInfo { get; set; }
    }
}
