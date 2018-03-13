using System;
using NathalieInwentaryzacje.Lib.Contracts.Dto.RecordSummary;

namespace NathalieInwentaryzacje.Lib.Contracts.Interfaces
{
    public interface ISummaryManager
    {
        RecordSummaryInfo GetSummary(DateTime recordDate);

        void SaveSummary(RecordSummaryInfo info);
    }
}
