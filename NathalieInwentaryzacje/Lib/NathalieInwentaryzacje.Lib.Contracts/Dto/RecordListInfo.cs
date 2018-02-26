using System;
using System.Collections.Generic;
using System.Linq;
using NathalieInwentaryzacje.Common.Utils.Extensions;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto
{
    public class RecordListInfo
    {
        public DateTime RecordDate { get; set; }

        public string RecordTitle => "Stan na dzień: " + RecordDate.ToRecordDateString();

        public IEnumerable<RecordListItemInfo> RecordsInfo { get; set; }

        public bool CanGenerateReport => RecordsInfo?.Any(x => x.IsFilledIn) ?? false;

        public string RecordId { get; set; }

        public bool CanAddEntries { get; set; }
    }
}
