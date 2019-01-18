using Newskies.WebApi.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using Newskies.WebApi.Services;
using System.Threading.Tasks;
using Newskies.WebApi.Helpers;

namespace Flyadeal.Interceptors.Validation
{
    public class PaymentMethodCodeInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var code = value as string ?? "";
            if (string.IsNullOrEmpty(code))
                return new ValidationResult(validationContext.MemberName + " required.");
            var sessionBagService = validationContext.GetSessionBagService();
            var resourcesService = validationContext.GetService(typeof(IResourcesService)) as ResourcesService;
            try
            {
                var paymentTypesList = Task.Run(async () => await resourcesService.GetPaymentMethodsList(Task.Run(async () => await sessionBagService.CultureCode()).Result)).Result;
                if (paymentTypesList.PaymentMethodList != null && Array.Find(paymentTypesList.PaymentMethodList,
                    p => p.PaymentMethodCode.Equals(code, StringComparison.OrdinalIgnoreCase)) == null)
                    return new ValidationResult(string.Format("Invalid {0} value: {1}", validationContext.MemberName, value));
            }
            catch (Exception e)
            {
                return new ValidationResult(string.Format("Unable to validate {0}. {1}", validationContext.MemberName, e.Message));
            }
            return ValidationResult.Success;
        }
    }
}