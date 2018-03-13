using System;
using NathalieInwentaryzacje.Lib.Contracts.Dto.RecordAppendix;

namespace NathalieInwentaryzacje.Lib.Contracts.Interfaces
{
    public interface IAppendixManager
    {
        RecordAppendixInfo GetAppendix(DateTime? recordDate);

        void SaveAppendix(RecordAppendixInfo appendix);
    }
}
