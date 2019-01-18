using System.ComponentModel.DataAnnotations;
using Newskies.WebApi.Validation;
using Newskies.WebApi.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;
using Newskies.WebApi.Helpers;

namespace Flyadeal.Interceptors.Validation
{
    class PassengerTravelDocumentExpirationDateInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext context)
        {
            var passengers = value as Passenger[];
            if(passengers == null)
            {
                return new ValidationResult("PassengerTravelDocumentExpirationDateInterceptor: Passengers failed null check");
            }

            try
            {
                var sessionBag = context.GetSessionBagService();
                var booking = Task.Run(async () => await sessionBag.Booking()).Result;
                if (booking == null || booking.Passengers == null)
                {
                    return new ValidationResult(string.Format("Failed to validate {0}. No booking found in session.", context.DisplayName));
                }
                
                DateTime compareDate = DateTime.Now.AddDays(1);
                if (booking.Journeys.Any(journey => journey.Segments.Any(segment => segment.International == true)))
                {
                    Journey lastJourney = booking.Journeys[booking.Journeys.Length - 1];
                    compareDate = lastJourney.Segments[lastJourney.Segments.Length - 1].STD;
                }

                foreach (Passenger pax in passengers)
                {
                    foreach (PassengerTravelDocument doc in pax.PassengerTravelDocuments)
                    {
                        if(doc.ExpirationDate.Date < compareDate.Date)
                        {
                            if((booking.Journeys.Any(journey => journey.Segments.Any(segment => segment.International == true))))
                            {
                                return new ValidationResult("Passenger Travel Document cannot expire before the date of last flight in journey.");
                            }
                            return new ValidationResult("Passenger Travel Document cannot expire before date of Booking + 1 day.");
                        }
                    }
                }
                return ValidationResult.Success;
            }
            catch (Exception e)
            {
                return new ValidationResult(String.Format("Unable to validate {0} expiration date. {1}", context.DisplayName, e.Message));
            }
        }
    }
}
