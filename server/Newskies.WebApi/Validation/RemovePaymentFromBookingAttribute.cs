using Newskies.WebApi.Helpers;
using Newskies.WebApi.Services;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Newskies.WebApi.Validation
{
    public class RemovePaymentFromBookingAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var paymentNumber = (short)value;
            if (paymentNumber == 0)
                return new ValidationResult("Invalid payment number. ");
            var sessionBagService = validationContext.GetSessionBagService();
            var booking = Task.Run(async () => await sessionBagService.Booking()).Result;
            if (booking == null)
            {
                return new ValidationResult("There is no booking in the session");
            }
            var payment = booking.Payments != null ? booking.Payments.ToList().Find(p => p.PaymentNumber == paymentNumber) : null;
            if (payment == null)
                return new ValidationResult("Payment does not exist with payment number: " + paymentNumber + ". ");
            if (payment.AuthorizationStatus != Contracts.Enumerations.AuthorizationStatus.Pending)
                return new ValidationResult("Payment is not pending and cannot be removed. ");
            return ValidationResult.Success;
        }
    }
}
