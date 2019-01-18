using Newskies.WebApi.Services;
using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;

namespace Flyadeal.Interceptors.Validation
{
    public class CommitBookingValidationInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var bookingService = validationContext.GetService(typeof(IBookingService)) as BookingService;
            var booking = bookingService.GetSessionBooking(true).Result;
            if (booking == null)
            {
                return new ValidationResult("No booking in session. ");
            }
            if (booking.BookingContacts == null || booking.BookingContacts.Length == 0)
            {
                return new ValidationResult("Missing booking contact. ");
            }
            return ValidationResult.Success;
        }
    }
}
