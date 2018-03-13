using System;
using System.Collections.Generic;
using NathalieInwentaryzacje.Lib.Contracts.Dto.RecordAppendix;
using NathalieInwentaryzacje.Lib.Contracts.Dto.RecordSummary;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Reports;

namespace NathalieInwentaryzacje.Lib.Contracts.Interfaces
{
    public interface IReportManager
    {
        void GenerateReports(DateTime recordDate, IEnumerable<GenerateReportEntryInfo> reportEntryInfos, string saveDir, int numberOfItemsPerPage = 40);

        void GenerateSummary(RecordSummaryInfo rai, string saveDir);

        void GenerateAppendix(RecordAppendixInfo rai, string saveDir);
    }
}
