using System;
using System.ComponentModel.DataAnnotations;
using NathalieInwentaryzacje.Common.Utils.Interfaces;

namespace NathalieInwentaryzacje.Common.Utils.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false)]
    public class ValidateCollectionAttribute : RequiredAttribute
    {

        /// <summary>
        /// Gets or sets a value indicating whether this instance is not empty.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is not empty; otherwise, <c>false</c>.
        /// </value>
        public bool IsNotEmpty { get; set; }

        /// <summary>
        /// Gets or sets the not empty message.
        /// </summary>
        /// <value>
        /// The not empty message.
        /// </value>
        public string NotEmptyMessage { get; set; }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IValidationCollection)
            {
                //dodatkowo dokleja info o błędach z kolekcji


                if (IsNotEmpty)
                {
                    if (((IValidationCollection)value).IsEmpty)
                        ErrorMessage = NotEmptyMessage + Environment.NewLine + ((IValidationCollection)value).Error;
                    else
                    {
                        ErrorMessage = ((IValidationCollection)value).Error;
                    }
                    if (((IValidationCollection)value).IsValid && !((IValidationCollection)value).IsEmpty)
                        return ValidationResult.Success;
                }
                else
                {
                    ErrorMessage = ((IValidationCollection)value).Error;
                    if (((IValidationCollection)value).IsValid)
                        return ValidationResult.Success;
                }


                return new ValidationResult(ErrorMessage);
            }
            return base.IsValid(value, validationContext);
        }
    }
}
