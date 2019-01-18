using Newskies.WebApi.Helpers;
using Newskies.WebApi.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Newskies.WebApi.Validation
{
    public class TitleAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var titleCode = value as string ?? "";
            titleCode = titleCode.Trim();
            if (string.IsNullOrEmpty(titleCode))
                return new ValidationResult("Title required.");
            var sessionBagService = validationContext.GetSessionBagService();
            var resourcesService = validationContext.GetService(typeof(IResourcesService)) as ResourcesService;
            try
            {
                var response = Task.Run(async () => await resourcesService.GetTitleList(Task.Run(async () => await sessionBagService.CultureCode()).Result)).Result;
                var title = response.TitleList.FirstOrDefault(c => c.TitleKey.Equals(titleCode));
                if (title == null)
                    return new ValidationResult(string.Format("Invalid title value: {0}.", titleCode));
            }
            catch (Exception e)
            {
                return new ValidationResult("Unable to validate title. " + e.Message);
            }
            return ValidationResult.Success;
        }
    }
}
