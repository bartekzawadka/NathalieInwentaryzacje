using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace NathalieInwentaryzacje.Common.Utils.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class CollectionMinLengthValidationAttribute : ValidationAttribute
    {
        public int MinCollectionLength { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is ICollection collection)) return ValidationResult.Success;
            return collection.Count < MinCollectionLength ? new ValidationResult(ErrorMessage) : ValidationResult.Success;
        }
    }
}
