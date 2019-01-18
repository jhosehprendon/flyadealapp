using Flyadeal.Interceptors.Helpers;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Helpers;
using Newskies.WebApi.Services;
using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Flyadeal.Interceptors.Validation
{
    public class FeeCodeValidationInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var feeCode = value as string;
            if (!Constants.FeeCodesAllowed.ToList().Contains(feeCode))
            {
                return new ValidationResult(string.Format("Fee code {0} not supported. ", feeCode));
            }

            if (validationContext.ObjectInstance.GetType() == typeof(SellFeeRequestData))
            {
                var sessionBagService = validationContext.GetSessionBagService();
                // only allow SMSF fee to be sold once
                if (feeCode == Constants.FeeCodeSMS)
                {
                    var booking = Task.Run(async () => await sessionBagService.Booking()).Result;
                    var existingFee = booking.Passengers.ToList().Find(p => p.PassengerFees.ToList().Find(f => f.FeeCode == feeCode) != null);
                    if (existingFee != null)
                    {
                        return new ValidationResult("Multiple selling of fee not allowed. ");
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
