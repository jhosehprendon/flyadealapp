using AutoMapper;
using Navitaire.WebServices.DataContracts.Booking;
using dto = Newskies.WebApi.Contracts;

namespace Newskies.WebApi.Mapping
{
    public class AvailabilityPaxPriceTypeResolver : IValueResolver<dto.AvailabilityRequest, AvailabilityRequest, PaxPriceType[]>
    {
        public PaxPriceType[] Resolve(dto.AvailabilityRequest source, AvailabilityRequest destination, PaxPriceType[] destMember, ResolutionContext context)
        {
            return ConverterHelpers.ConvertPaxTypeCounts(source.PaxTypeCounts);
        }
    }

    public class SellJourneyPaxPriceTypeResolver : IValueResolver<dto.SellJourneyByKeyRequestData, SellJourneyByKeyRequestData, PaxPriceType[]>
    {
        public PaxPriceType[] Resolve(dto.SellJourneyByKeyRequestData source, SellJourneyByKeyRequestData destination, PaxPriceType[] destMember, ResolutionContext context)
        {
            return ConverterHelpers.ConvertPaxTypeCounts(source.PaxTypeCounts);
        }
    }

    public class LowFareAvailabilityPaxPriceTypeResolver : IValueResolver<dto.LowFareTripAvailabilityRequest, LowFareTripAvailabilityRequest, PaxPriceType[]>
    {
        public PaxPriceType[] Resolve(dto.LowFareTripAvailabilityRequest source, LowFareTripAvailabilityRequest destination, PaxPriceType[] destMember, ResolutionContext context)
        {
            return ConverterHelpers.ConvertPaxTypeCounts(source.PaxTypeCounts);
        }

    }
}