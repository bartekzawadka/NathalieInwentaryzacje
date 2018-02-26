using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NathalieInwentaryzacje.Lib.Contracts.Dto;

namespace NathalieInwentaryzacje.Lib.Contracts.Interfaces
{
    public interface IRecordsManager
    {
        Task<IEnumerable<RecordListInfo>> GetRecords();

        void CreateRecord(NewRecordInfo recordInfo);

        void OpenRecordFileEdit(DateTime recordDate, string fileName);
    }
}
