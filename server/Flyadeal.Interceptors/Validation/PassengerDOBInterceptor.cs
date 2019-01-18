using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Constants;
using System;
using System.Threading.Tasks;
using Newskies.WebApi.Helpers;

namespace Flyadeal.Interceptors.Validation
{
    class PassengerDOBInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var sessionBag = validationContext.GetSessionBagService();
            var booking = Task.Run(async () => await sessionBag.Booking()).Result;
            if (booking == null)
                return new ValidationResult("Unable to validate seat request. No booking found in session. ");
            

            var passengers = value as Passenger[];
            if (passengers == null)
            {
                return new ValidationResult("Could not validate Passenger DOB, null check failed.");
            }
            foreach (Passenger pax in passengers)
            {
                var minAge = 0;
                var maxAge = 0;
                var age = Helpers.ValidationHelper.GetAge(DateTime.UtcNow, pax.PassengerTypeInfo.DOB);
                if (pax.PassengerTypeInfo.PaxType.Equals(Global.CHILD_CODE, StringComparison.Ordinal))
                {
                    minAge = Helpers.ValidationHelper.ChildMinAge;
                    maxAge = Helpers.ValidationHelper.ChildMaxAge;
                } else if (pax.PassengerTypeInfo.PaxType.Equals(Global.ADULT_CODE, StringComparison.Ordinal))
                {
                    minAge = Helpers.ValidationHelper.AdultMinAge;
                    maxAge = Helpers.ValidationHelper.AdultMaxAge;
                }
                if (age > maxAge || age < minAge)
                {
                    return new ValidationResult($"Invalid DOB for Passenger: {pax.Names[0].FirstName} {pax.Names[0].LastName}");
                }
                //Check infant age, also must be 8 days old before flight.
                var infant = pax.Infant;
                if (infant != null)
                {
                    minAge = Helpers.ValidationHelper.InfantMinAge;
                    maxAge = Helpers.ValidationHelper.InfantMaxAge;
                    age = Helpers.ValidationHelper.GetAge(DateTime.UtcNow, infant.DOB);
                    var std = booking.Journeys[0].Segments[0].STD;
                    if (age > maxAge || age < minAge || std.AddDays(-Helpers.ValidationHelper.InfantMinDaysOld) < infant.DOB)
                        return new ValidationResult($"Invalid DOB for Passenger: {infant.Names[0].FirstName} {infant.Names[0].LastName}");
                }
            }

            return ValidationResult.Success;
        }
    }
}
