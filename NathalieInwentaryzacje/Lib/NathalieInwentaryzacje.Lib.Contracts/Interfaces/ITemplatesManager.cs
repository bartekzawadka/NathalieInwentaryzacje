using System.Collections.Generic;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Templates;

namespace NathalieInwentaryzacje.Lib.Contracts.Interfaces
{
    public interface ITemplatesManager
    {
        IEnumerable<TemplateInfo> GetTemplates();

        void CreateOrUpdateTemplate(TemplateInfo tInfo);
    }
}
