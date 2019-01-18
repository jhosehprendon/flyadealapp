using Newskies.WebApi.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Contracts.Enumerations;
using System.Collections.Generic;

namespace Flyadeal.Interceptors.Actions
{
    public class AssignSeatRequestInterceptor : IRequestInterceptor
    {
        public async Task<object> OnRequest(object request, ActionExecutingContext context, Dictionary<string, string> settings)
        {
            var assignSeatRequest = request as AssignSeatRequest;
            if (assignSeatRequest == null || assignSeatRequest.AssignSeatData == null)
                return await Task.FromResult(request);
            var data = assignSeatRequest.AssignSeatData;
            data.SeatAssignmentMode = SeatAssignmentMode.PreSeatAssignment;
            if (data.PaxNumber.HasValue)
            {
                data.WaiveFees = false;
                //data.SeatAssignmentMode = SeatAssignmentMode.PreSeatAssignment;
            }
            else // auto-assign pax seats requested
            {
                data.WaiveFees = true;
                //data.SeatAssignmentMode = SeatAssignmentMode.WebCheckIn;
            }
            return await Task.FromResult(assignSeatRequest);
        }
    }
}