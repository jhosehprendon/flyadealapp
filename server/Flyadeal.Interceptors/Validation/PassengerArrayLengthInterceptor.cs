using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Newskies.WebApi.Helpers;

namespace Flyadeal.Interceptors.Validation
{
    public class PassengerArrayLengthInterceptor : ArrayLengthInterceptor
    {
        public PassengerArrayLengthInterceptor() : this(1, 9) { }

        public PassengerArrayLengthInterceptor(int minItems = 1, int maxItems = 9)
            : base(minItems, maxItems)
        {
        }

        public override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var baseResult = base.IsValid(value, validationContext);
            if (baseResult != null)
                return baseResult;
            try
            {
                var sessionBag = validationContext.GetSessionBagService();
                var booking = Task.Run(async () => await sessionBag.Booking()).Result;
                if (booking == null || booking.Passengers == null)
                    return new ValidationResult(string.Format("Failed to validate {0}. No booking found in session.", validationContext.DisplayName));
                if (string.IsNullOrEmpty(booking.RecordLocator) && ((object[])value).Length != booking.Passengers.Length)
                    return new ValidationResult(string.Format("{0} length invalid. Array length must be{1}.",
                       validationContext.DisplayName, booking.Passengers.Length));
                return ValidationResult.Success;
            }
            catch (Exception e)
            {
                return new ValidationResult(string.Format("Unable to validate {0} array length. {1}", validationContext.DisplayName, e.Message));
            }
        }
    }
}
