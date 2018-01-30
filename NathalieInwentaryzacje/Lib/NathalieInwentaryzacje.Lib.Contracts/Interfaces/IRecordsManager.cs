using System;
using System.Collections.Generic;
using System.Text;
using NathalieInwentaryzacje.Lib.Contracts.Dto;

namespace NathalieInwentaryzacje.Lib.Contracts.Interfaces
{
    public interface IRecordsManager
    {
        IEnumerable<RecordInfo> GetRecords();
    }
}
