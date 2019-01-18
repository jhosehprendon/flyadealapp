namespace Flyadeal.Interceptors.Validation
{
    public class LowFareAvailabilityRequestsInterceptor : AvailabilityRequestsInterceptor
    {

        public LowFareAvailabilityRequestsInterceptor() : this(1, 2, 7) { }
        public LowFareAvailabilityRequestsInterceptor(int minAvailabilityRequests = 1, int maxAvailabilityRequests = 2, int maxAvailabilitySpanDays = 7) : base(minAvailabilityRequests, maxAvailabilityRequests, maxAvailabilitySpanDays) { }
    }
}
