using System;
using System.ComponentModel.DataAnnotations;
using Newskies.WebApi.Validation;
using Newskies.WebApi.Services;
using System.Threading.Tasks;
using Newskies.WebApi.Helpers;

namespace Flyadeal.Interceptors.Validation
{
    public class DocTypeInterceptor : IValidationInterceptor
    {
        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var docTypeCode = value as string ?? "";
            docTypeCode = docTypeCode.Trim();
            if (string.IsNullOrEmpty(docTypeCode))
                return new ValidationResult("Document type required. ");
            var sessionBagService = validationContext.GetSessionBagService();
            var resourcesService = validationContext.GetService(typeof(IResourcesService)) as ResourcesService;
            try
            {
                var response = Task.Run(async () => await resourcesService.GetDocTypeList(Task.Run(async () => await sessionBagService.CultureCode()).Result)).Result;
                var docType = Array.Find(response.DocTypeList, c => c.DocTypeCode.Equals(docTypeCode));
                if (docType == null)
                    return new ValidationResult(string.Format("Invalid document type: {0}. ", docTypeCode));
            }
            catch (Exception e)
            {
                return new ValidationResult("Unable to validate document type. " + e.Message);
            }
            return ValidationResult.Success;
        }
    }
}
