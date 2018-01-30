using System;
using System.Collections.Generic;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Templates;

namespace NathalieInwentaryzacje.Lib.Contracts.Interfaces
{
    public interface IRecordsManager
    {
        IEnumerable<RecordInfo> GetRecords();

        void CreateRecord(DateTime recordDate, IEnumerable<TemplateInfo> templates);
    }
}
