using Microsoft.AspNetCore.Mvc.Filters;
using Newskies.WebApi.Services;
using dto=Newskies.WebApi.Contracts;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Flyadeal.Interceptors.Actions
{
    public class CommitBookingResponseInterceptor : IResponseInterceptor
    {
        public async Task<object> OnResponse(object response, ResultExecutingContext context, Dictionary<string, string> settings)
        {
            var commitResponse = response as dto.CommitResponse;
            var sessionBag = context.HttpContext.RequestServices.GetService(typeof(ISessionBagService)) as ISessionBagService;
            var commentsService = context.HttpContext.RequestServices.GetService(typeof(IBookingCommentsService)) as IBookingCommentsService;
            if (commitResponse == null || commitResponse.BookingUpdateResponseData == null || sessionBag == null || commentsService == null
                || commitResponse.BookingUpdateResponseData.Success == null || string.IsNullOrEmpty(commitResponse.BookingUpdateResponseData.Success.RecordLocator))
            {
                return await Task.FromResult(response);
            }
            var bookingComments = await sessionBag.PostCommitBookingComments();
            if (bookingComments == null || bookingComments.Count == 0)
            {
                return await Task.FromResult(response);
            }
            await commentsService.AddBookingComments(bookingComments);
            await sessionBag.SetPostCommitBookingComments(null);
            return await Task.FromResult(response);
        }
    }
}
