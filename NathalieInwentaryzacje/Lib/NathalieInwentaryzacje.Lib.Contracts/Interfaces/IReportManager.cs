using System;
using System.Collections.Generic;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Reports;

namespace NathalieInwentaryzacje.Lib.Contracts.Interfaces
{
    public interface IReportManager
    {
        void GenerateReports(DateTime recordDate, IEnumerable<GenerateReportEntryInfo> reportEntryInfos, string saveDir, int numberOfItemsPerPage = 40);
    }
}
