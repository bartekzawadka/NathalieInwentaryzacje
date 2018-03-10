using System;
using System.Collections.Generic;
using System.Linq;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto.Reports.RecordSummary
{
    public class RecordsTotalsInfo
    {
        public IList<RecordTotalRowInfo> Rows { get; set; }

        public decimal Sum
        {
            get
            {
                if (Rows == null || Rows.Count == 0)
                    return Decimal.Zero;

                return Rows.Sum(x => x.Value);
            }
        }
    }
}
