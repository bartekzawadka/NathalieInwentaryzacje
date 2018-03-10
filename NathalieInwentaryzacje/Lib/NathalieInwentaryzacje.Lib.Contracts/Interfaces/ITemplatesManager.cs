using System.Collections.Generic;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Reports.RecordAppendix;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Templates;

namespace NathalieInwentaryzacje.Lib.Contracts.Interfaces
{
    public interface ITemplatesManager
    {
        IEnumerable<TemplateInfo> GetTemplates(bool includeDisabled = false);

        TemplateInfo GetTemplate(string id);

        void CreateOrUpdateTemplate(TemplateInfo t);

        RecordAppendixInfo GetRecordAppendixTemplate();

        void SaveRecordAppendixTemplate(RecordAppendixInfo appendix);
    }
}
