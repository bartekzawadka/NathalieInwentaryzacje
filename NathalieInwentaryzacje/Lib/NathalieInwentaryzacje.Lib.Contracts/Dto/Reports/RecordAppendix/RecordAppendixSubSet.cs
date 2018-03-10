using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto.Reports.RecordAppendix
{
    public class RecordAppendixSubSet
    {
        [Required(ErrorMessage = "Nazwa wyświetlana zestawienia jest wymagana")]
        public string Title { get; set; }

        public ObservableCollection<RecordAppendixReportRowInfo> Rows { get; set; }

        public decimal Sum
        {
            get
            {
                if(Rows == null || Rows.Count ==0)
                    return Decimal.Zero;

                return Rows.Sum(x => x.Value);
            }
        }
    }
}
