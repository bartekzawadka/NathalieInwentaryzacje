﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NathalieInwentaryzacje.Common.Utils.Validation;
using NathalieInwentaryzacje.Common.Utils.Validation.Attributes;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto
{
    public class NewRecordInfo : ValidationBase
    {
        [Required(ErrorMessage = "Proszę wprowadzić datę zestawu inwentaryzacji")]
        [PastDateValidator(ErrorMessage = "Data zestawu inwentaryzacji nie może być późniejsza niż data bieżąca")]
        public DateTime? RecordsDate { get; set; } = DateTime.Now;

        public List<NewRecordTypeInfo> RecordTypes { get; set; }


        public bool IsRecordTypeSelected
        {
            get
            {
                return RecordTypes?.Any(x => x.IsSelected) ?? false;
            }
        }
    }
}
