using Newskies.WebApi.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Flyadeal.Interceptors.Validation
{
    public class ArrayLengthInterceptor : IValidationInterceptor
    {
        protected readonly int _minItems;
        protected readonly int _maxItems;

        public ArrayLengthInterceptor(int minItems, int maxItems)
        {
            _minItems = minItems >= 0 ? minItems : 0;
            _maxItems = maxItems >= 0 && maxItems >= minItems ? maxItems : minItems;
        }

        public virtual ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                var array = (object[])value;
                if (array == null)
                    return new ValidationResult(string.Format("{0} must be an array.", validationContext.DisplayName));
                if (array.Length >= _minItems && array.Length <= _maxItems)
                    return ValidationResult.Success;
                return new ValidationResult(string.Format("{0} length invalid. Array length must be between {1} and {2}.",
                    validationContext.DisplayName, _minItems, _maxItems));
            }
            catch (Exception e)
            {
                return new ValidationResult(e.Message);
            }
        }
    }
}
