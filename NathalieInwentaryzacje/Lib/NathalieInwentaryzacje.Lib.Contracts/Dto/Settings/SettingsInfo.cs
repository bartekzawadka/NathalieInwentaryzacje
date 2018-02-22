using System.ComponentModel.DataAnnotations;
using NathalieInwentaryzacje.Common.Utils.Validation;
using NathalieInwentaryzacje.Common.Utils.Validation.Attributes;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto.Settings
{
    public class SettingsInfo : ValidationBase
    {
    [Required(ErrorMessage = "Adres repozytorium jest wymagany")]
    public string RepoAddress { get; set; }

    [Required(ErrorMessage = "Nazwa użytkownika repozytorium jest wymagana")]
    public string RepoUser { get; set; }

    [Required(ErrorMessage = "Hasło dostępu do repozytorium jest wymagane")]
    public string RepoPassword { get; set; }

    [NaturalNumberValidator(ErrorMessage = "Wartość musi być liczbą większą od 0", NullValueErrorMessage = "Liczba wierszy raportu nie może być pusta")]
    public int NumberOfReportRows { get; set; }
    }
}
