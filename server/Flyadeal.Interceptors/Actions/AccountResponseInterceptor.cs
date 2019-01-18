using Microsoft.AspNetCore.Mvc.Filters;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flyadeal.Interceptors.Actions
{
    public class AccountResponseInterceptor : IResponseInterceptor
    {
        public async Task<object> OnResponse(object response, ResultExecutingContext context, Dictionary<string, string> settings)
        {
            var account = response as Account;
            if (account == null)
            {
                return await Task.FromResult(response); 
            }

            if (account.Status != AccountStatus.Open)
            {
                account.AvailableCredits = 0;
                account.ForeignAmount = 0;
            }
            return await Task.FromResult(account);
        }
    }
}
