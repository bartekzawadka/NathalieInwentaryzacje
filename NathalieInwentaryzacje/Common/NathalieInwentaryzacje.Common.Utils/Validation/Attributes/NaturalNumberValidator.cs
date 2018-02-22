using System;
using System.ComponentModel.DataAnnotations;

namespace NathalieInwentaryzacje.Common.Utils.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property|AttributeTargets.Field|AttributeTargets.Parameter)]
    public class NaturalNumberValidator: ValidationAttribute
    {
        public string NullValueErrorMessage { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult(NullValueErrorMessage ?? "Wartość nie może być pusta");

            if(!int.TryParse(value.ToString(), out var num))
                return new ValidationResult(ErrorMessage);

            if(num <= 0)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
