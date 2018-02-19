using System;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto
{
    public class RecordListItemInfo
    {
        public string FilePath { get; set; }

        public string TemplateId { get; set; }

        public string DisplayName { get; set; }

        public bool IsFilledIn { get; set; }

        public DateTime RecordDate { get; set; }
    }
}
