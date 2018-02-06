using System.ComponentModel.DataAnnotations;
using NathalieInwentaryzacje.Common.Utils.Validation;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Templates;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto
{
    public class TemplateInfo : ValidationBase
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Nazwa szablonu nie może być pusta")]
        public string Name { get; set; }

        public bool IsEnabled { get; set; } = true;

        public TemplateColumn[] Columns { get; set; }

        public string SumUpColumnName { get; set; }

        public bool IsBlockedForEdit => !string.IsNullOrEmpty(Id);
    }
}
