using Newskies.WebApi.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Newskies.WebApi.Contracts;
using Flyadeal.Interceptors.Helpers;
using System.Collections.Generic;

namespace Flyadeal.Interceptors.Actions
{
    public class PersonUpdateRequestInterceptor : IRequestInterceptor
    {
        public async Task<object> OnRequest(object request, ActionExecutingContext context, Dictionary<string, string> settings)
        {
            var person = request as Person;
            if (person == null)
                return await Task.FromResult(request);
            var httpContext = context.HttpContext;
            var sessionBagService = httpContext.RequestServices.GetService(typeof(ISessionBagService)) as SessionBagService;
            if (sessionBagService == null)
                return await Task.FromResult(request);
            //if (await sessionBagService.RoleCode() != Constants.MemberRoleCode)
            //    return await Task.FromResult(request);
            person.EmailAddress = await sessionBagService.AgentName();
            return person;
        }
    }
}
