using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;
using Flyadeal.Interceptors.Helpers;

namespace Flyadeal.Interceptors.Validation
{
    public class PaymentAccountHolderNameInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return ValidationHelper.CardPaymentStringMemberIsValid(value, validationContext, "^[a-zA-Z ,.'-]{1,60}$");
        }
    }
}
