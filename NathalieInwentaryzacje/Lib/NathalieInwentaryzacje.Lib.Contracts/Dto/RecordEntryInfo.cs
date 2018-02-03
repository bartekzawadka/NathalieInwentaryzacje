using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using NathalieInwentaryzacje.Common.Utils.Validation;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto
{
    public class RecordEntryInfo : ValidationBase
    {
        [Required(ErrorMessage = "Data inwentaryzacji jest wymagana")]
        public DateTime RecordDate { get; set; }

        public string RecordDateText => RecordDate.ToString("yyyy-MM-dd");

        public string Title => string.IsNullOrEmpty(RecordDisplayName)
            ? "Nieznana inwentaryzacja"
            : RecordDisplayName + " (" + RecordDate.ToString("yyyy-MM-dd") + ")";

        public string RecordName { get; set; }

        [Required(ErrorMessage = "Nazwa wyświetlana w raporcie nie może być pusta")]
        public string RecordDisplayName { get; set; }

        public DataTable RecordEntryTable { get; set; }

        public string TemplateId { get; set; }

        public string RecordId { get; set; }
    }
}
