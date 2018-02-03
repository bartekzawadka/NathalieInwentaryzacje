using System;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto
{
    public class RecordListItemInfo
    {
        public DateTime RecordDate { get; set; }

        public string TemplateId { get; set; }

        public string Name { get; set; }

        public bool IsFilledIn { get; set; }
    }
}
