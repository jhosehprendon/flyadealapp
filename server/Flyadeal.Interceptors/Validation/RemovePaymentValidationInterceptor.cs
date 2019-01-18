using Newskies.WebApi.Helpers;
using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Flyadeal.Interceptors.Validation
{
    public class RemovePaymentValidationInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var paymentNumber = (short)value;
            var sessionBagService = validationContext.GetSessionBagService();
            var booking = Task.Run(async () => await sessionBagService.Booking()).Result;
            if (booking != null)
            {
                var payment = booking.Payments != null ? booking.Payments.ToList().Find(p => p.PaymentNumber == paymentNumber) : null;
                if (payment != null && payment.PaymentMethodType != Newskies.WebApi.Contracts.Enumerations.PaymentMethodType.Voucher)
                    return new ValidationResult("Payment is not a voucher and cannot be removed. ");
            }
            return ValidationResult.Success;
        }
    }
}
