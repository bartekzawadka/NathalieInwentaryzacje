using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NathalieInwentaryzacje.Common.Utils.Validation;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto
{
    public class GenerateReportsInfo : ValidationBase
    {
        public DateTime RecordDate { get; set; }

        public IEnumerable<RecordListItemInfo> RecordsInfo { get; set; }

        [Required(ErrorMessage = "Katalog do zapisu raportów jest wymagany")]
        public string ReportsSaveDir { get; set; }
    }
}
