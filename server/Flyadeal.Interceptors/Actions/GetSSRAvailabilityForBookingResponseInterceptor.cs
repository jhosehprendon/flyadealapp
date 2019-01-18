using Microsoft.AspNetCore.Mvc.Filters;
using Newskies.WebApi.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dto = Newskies.WebApi.Contracts;

namespace Flyadeal.Interceptors.Actions
{
    public class GetSSRAvailabilityForBookingResponseInterceptor : IResponseInterceptor
    {
        public async Task<object> OnResponse(object response, ResultExecutingContext context, Dictionary<string, string> settings)
        {
            var ssrsResponse = response as dto.GetSSRAvailabilityForBookingResponse;
            if (ssrsResponse == null || ssrsResponse.SSRAvailabilityForBookingResponse == null || ssrsResponse.SSRAvailabilityForBookingResponse.SSRSegmentList == null)
            {
                return await Task.FromResult(response);
            }

            /// Remove meal SSRs which have 0.00 cost. FADIBE-436
            for (var i=0; i< ssrsResponse.SSRAvailabilityForBookingResponse.SSRSegmentList.Length; i++)
            {
                var ssrSegment = ssrsResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[i];
                if (ssrSegment.AvailablePaxSSRList == null)
                {
                    continue;
                }
                var availablePaxSsrs = ssrSegment.AvailablePaxSSRList.ToList();
                availablePaxSsrs.RemoveAll(s => s.SSRCode.EndsWith("ML") && SsrTotalCost(s.PaxSSRPriceList) == 0);
                ssrSegment.AvailablePaxSSRList = availablePaxSsrs.ToArray();
            }
            return await Task.FromResult(ssrsResponse);
        }

        private decimal SsrTotalCost(dto.PaxSSRPrice[] paxSSRPriceList)
        {
            var cost = 0M;
            if (paxSSRPriceList != null)
            {
                foreach (var paxSsrPrice in paxSSRPriceList.ToList())
                {
                    if (paxSsrPrice.PaxFee != null && paxSsrPrice.PaxFee.ServiceCharges != null)
                    {
                        paxSsrPrice.PaxFee.ServiceCharges.ToList().ForEach(c => cost += c.Amount);
                    }
                }
            }
            return cost;
        }
    }
}
