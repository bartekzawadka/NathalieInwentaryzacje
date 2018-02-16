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

        [Required(ErrorMessage = "Ścieżka do pliku szablonu musi być zdefiniowana")]
        public string TemplateFilePath { get; set; }

        public bool IsNew => string.IsNullOrEmpty(Id);
    }
}
