using System.Collections.Generic;
using System.Linq;
using NathalieInwentaryzacje.Common.Utils.Extensions;
using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Appendix;
using NathalieInwentaryzacje.Lib.Contracts.Dto.RecordAppendix;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Templates;

namespace NathalieInwentaryzacje.Lib.Bll.Mappers
{
    public class TemplateMapper
    {
        public static Template ToTemplate(TemplateInfo value)
        {
            return new Template
            {
                Id = value.Id,
                Name = value.Name,
                IsEnabled = value.IsEnabled,
                TemplateFileName = value.TemplateFilePath
            };
        }

        public static TemplateInfo ToTemplateInfo(Template value)
        {
            return new TemplateInfo
            {
                Id = value.Id,
                Name = value.Name,
                IsEnabled = value.IsEnabled,
                TemplateFilePath = value.TemplateFileName
            };
        }
    }
}
