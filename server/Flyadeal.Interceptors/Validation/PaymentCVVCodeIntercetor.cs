using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;
using Flyadeal.Interceptors.Helpers;

namespace Flyadeal.Interceptors.Validation
{
    public class PaymentCVVCodeIntercetor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return ValidationHelper.CardPaymentStringMemberIsValid(value, validationContext, "^[0-9]{3,4}$");
        }
    }
}
