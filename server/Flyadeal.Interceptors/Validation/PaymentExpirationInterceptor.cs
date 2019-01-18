using Newskies.WebApi.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using Flyadeal.Interceptors.Helpers;
using System.Linq;
using Newskies.WebApi.Contracts;

namespace Flyadeal.Interceptors.Validation
{
    public class PaymentExpirationInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dateObj = value as DateTime?;
            if (dateObj == null || !dateObj.HasValue)
            {
                var addPaymentToBookingRequestData = validationContext.ObjectInstance as AddPaymentToBookingRequestData;
                if (addPaymentToBookingRequestData == null || string.IsNullOrEmpty(addPaymentToBookingRequestData.PaymentMethodCode))
                    return ValidationResult.Success;
                if (Constants.CardPaymentMethodCodes.ToList().Contains(addPaymentToBookingRequestData.PaymentMethodCode))
                    return new ValidationResult(string.Format("{0} required for payment method {1}.", validationContext.MemberName, addPaymentToBookingRequestData.PaymentMethodCode));
                return ValidationResult.Success;
            }
            var date = dateObj.Value;
            if (date.Year > DateTime.Now.Year || (date.Year == DateTime.Now.Year && date.Month >= DateTime.Now.Month))
                return ValidationResult.Success;
            return new ValidationResult(string.Format("Invalid {0} date. It must be a future date.", validationContext.MemberName));
        }
    }
}
