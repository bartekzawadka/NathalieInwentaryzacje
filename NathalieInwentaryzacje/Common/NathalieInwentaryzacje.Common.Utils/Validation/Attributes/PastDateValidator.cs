using System;
using System.ComponentModel.DataAnnotations;

namespace NathalieInwentaryzacje.Common.Utils.Validation.Attributes
{
    [AttributeUsageAttribute(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class PastDateValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is DateTime date))
                return ValidationResult.Success;

            if (date.Date <= DateTime.Now.Date)
                return ValidationResult.Success;

            ErrorMessage = "Wybrana data nie może być późniejsza niż data dzisiejsza.";
            return new ValidationResult(ErrorMessage);
        }
    }
}
