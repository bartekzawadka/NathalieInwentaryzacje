﻿using System;
using System.Collections.Generic;
using NathalieInwentaryzacje.Lib.Contracts.Dto;

namespace NathalieInwentaryzacje.Lib.Contracts.Interfaces
{
    public interface IRecordsManager
    {
        IEnumerable<RecordListInfo> GetRecords();

        void CreateRecord(NewRecordInfo recordInfo);

        void EditRecordFile(DateTime recordDate, string fileName);
    }
}
