using System;
using System.Collections.Generic;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Reports;

namespace NathalieInwentaryzacje.Lib.Contracts.Interfaces
{
    public interface IRecordsManager
    {
        IEnumerable<RecordListInfo> GetRecords();

        void CreateRecord(NewRecordInfo recordInfo);

        void OpenRecordFileEdit(DateTime recordDate, string fileName);

        IEnumerable<RecordEntryReportInfo> GetRecordsReportInfo(DateTime recordDate,
            IEnumerable<string> fileNames);
    }
}
