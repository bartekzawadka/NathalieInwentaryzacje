using NathalieInwentaryzacje.Lib.Contracts.Dto;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Templates;

namespace NathalieInwentaryzacje.Lib.Bll.Mappers
{
    public class TemplateMapper
    {
        public static Template ToTemplate(TemplateInfo value)
        {
            return new Template
            {
                Columns = value.Columns,
                Id = value.Id,
                Name = value.Name,
                IsEnabled = value.IsEnabled
            };
        }

        public static TemplateInfo ToTemplateInfo(Template value)
        {
            return new TemplateInfo
            {
                Columns = value.Columns,
                Id = value.Id,
                Name = value.Name,
                IsEnabled = value.IsEnabled
            };
        }
    }
}
