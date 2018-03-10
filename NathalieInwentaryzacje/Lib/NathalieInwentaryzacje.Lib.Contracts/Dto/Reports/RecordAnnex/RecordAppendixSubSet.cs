using System;
using System.Collections.Generic;
using System.Linq;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto.Reports.RecordAnnex
{
    public class RecordAppendixSubSet
    {
        public string Title { get; set; }

        public List<RecordAppendixReportRowInfo> Rows { get; set; }

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
