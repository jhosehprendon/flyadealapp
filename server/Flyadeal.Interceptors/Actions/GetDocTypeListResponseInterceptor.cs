using Microsoft.AspNetCore.Mvc.Filters;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flyadeal.Interceptors.Actions
{

    public class GetDocTypeListResponseInterceptor : IResponseInterceptor
    {
        public async Task<object> OnResponse(object result, ResultExecutingContext context, Dictionary<string, string> settings)
        {
            var response = result as Newskies.WebApi.Contracts.GetDocTypeListResponse;
            if (response == null)
            {
                return await Task.FromResult(response);
            }
            var sessionBagService = context.HttpContext.RequestServices.GetService(typeof(ISessionBagService)) as SessionBagService;
            if (sessionBagService == null || (await sessionBagService.Booking()) == null)
            {
                return await Task.FromResult(response);
            }
            response.DocTypeList = FilterDocumentTypes(await sessionBagService.Booking(), response.DocTypeList);
            return await Task.FromResult(response);
        }

        internal static DocType[] FilterDocumentTypes(Booking booking, DocType[] docTypes)
        {
            if (docTypes == null || booking == null || booking.Journeys == null)
            {
                return docTypes;
            }
            return booking.Journeys.Any(j => j.Segments.Any(s => s.International)) ? docTypes.Where(d => d.DocTypeCode == "P" || d.DocTypeCode == "N").ToArray() : docTypes;
        }
    }
}
