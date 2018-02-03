using System;
using System.Collections.Generic;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Templates;

namespace NathalieInwentaryzacje.Lib.Contracts.Interfaces
{
    public interface IRecordsManager
    {
        IEnumerable<RecordListInfo> GetRecords();

        void CreateRecord(NewRecordInfo recordInfo);

        RecordEntryInfo GetRecordEntry(DateTime recordDate, string recordEntryName);

        void SaveRecord(string recordEntryName, RecordEntryInfo recordEntryInfo);
    }
}
