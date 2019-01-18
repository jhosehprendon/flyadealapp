using System.Linq;
using AutoMapper;
using Navitaire.NewSkies.WebServices.DataContracts.Common.Enumerations;
using Navitaire.WebServices.DataContracts.Booking;
using Navitaire.WebServices.DataContracts.Common;
using nskop = Navitaire.WebServices.DataContracts.Operation;
using Navitaire.WebServices.DataContracts.Common.Enumerations;
using Newskies.SessionManager;
using Newskies.UtilitiesManager;
using dto = Newskies.WebApi.Contracts;
using Newskies.WebApi.Constants;
using Newskies.AgentManager;
using static Navitaire.WebServices.DataContracts.Common.ParentMessageBase;
using Navitaire.WebServices.DataContracts.Person;
using Newskies.AccountManager;

namespace Newskies.WebApi.Mapping
{
    public class MappingConfigurationProfile : Profile
    {
        public MappingConfigurationProfile()
        {
            // NSK SessionManager objects maps
            CreateMap<dto.LogonRequestData, LogonRequestData>();
            CreateMap<LogonResponse, dto.LogonResponse>();

            // NSK BookingManager objects maps
            CreateMap<GetAvailabilityResponse, dto.GetAvailabilityResponse>();
            CreateMap<ApplyPromotionResponse, dto.ApplyPromotionResponse>();
            CreateMap<ApplyPromotionRequestData, dto.ApplyPromotionRequestData>().ReverseMap();
            CreateMap<TripAvailabilityResponse, dto.TripAvailabilityResponse>();
            CreateMap<TripAvailabilityResponseVer2, dto.TripAvailabilityResponse>();
            CreateMap<TripAvailabilityWithSSRResponse, dto.TripAvailabilityResponse>();
            CreateMap<LowFareTripAvailabilityResponse, dto.LowFareTripAvailabilityResponse>();
            CreateMap<LowFareAvailabilityResponse, dto.LowFareAvailabilityResponse>();
            CreateMap<UpdateContactsResponse, dto.UpdateContactsResponse>();
            CreateMap<SellResponse, dto.SellResponse>();
            CreateMap<Navitaire.WebServices.DataContracts.Common.OtherServiceInformation, dto.OtherServiceInformation>();
            CreateMap<CancelResponse, dto.CancelResponse>();
            CreateMap<GetSSRAvailabilityForBookingResponse, dto.GetSSRAvailabilityForBookingResponse>();
            CreateMap<SSRAvailabilityForBookingResponse, dto.SSRAvailabilityForBookingResponse>();
            CreateMap<JourneyDateMarket, dto.JourneyDateMarket>();
            CreateMap<JourneyDateMarketVer2, dto.JourneyDateMarket>()
                .ForMember(m => m.Journeys, m => m.MapFrom(n => n.AvailableJourneys));
            CreateMap<Journey, dto.Journey>().ReverseMap();
            CreateMap<Segment, dto.Segment>().ReverseMap();
            CreateMap<AvailableJourney, dto.Journey>()
                .ForMember(m => m.Segments, m => m.MapFrom(n => n.AvailableSegment));
            CreateMap<AvailableSegment, dto.Segment>();
            CreateMap<AvailableFare2, dto.AvailableFare2>().ReverseMap();
            CreateMap<Leg, dto.Leg>().ReverseMap();
            CreateMap<LegInfo, dto.LegInfo>().ReverseMap();
            CreateMap<PaxFare, dto.PaxFare>().ReverseMap();
            CreateMap<PaxSegment, dto.PaxSegment>().ReverseMap();
            CreateMap<PaxSSR, dto.PaxSSR>().ReverseMap();
            CreateMap<Fare, dto.Fare>().ReverseMap().ReverseMap();
            CreateMap<BookingServiceCharge, dto.BookingServiceCharge>().ReverseMap();
            CreateMap<FareDesignator, dto.FareDesignator>().ReverseMap();
            CreateMap<FlightDesignator, dto.FlightDesignator>().ReverseMap();
            CreateMap<PriceVariation, dto.PriceVariation>();
            CreateMap<SSRPrice, dto.SSRPrice>();
            CreateMap<dto.TripAvailabilityRequest, TripAvailabilityRequest>()
                .ForMember(m => m.LoyaltyFilter, m => m.MapFrom(n => LoyaltyFilter.PointsAndMonetary));
            CreateMap<dto.AvailabilityRequest, AvailabilityRequest>()
                .ForMember(m => m.FareClassControl, m => m.MapFrom(n => FareClassControl.CompressByProductClass))
                .ForMember(m => m.AvailabilityFilter, m => m.MapFrom(n => AvailabilityFilter.ExcludeUnavailable))
                .ForMember(m => m.AvailabilityType, m => m.MapFrom(n => AvailabilityType.Default))
                .ForMember(m => m.FareRuleFilter, m => m.MapFrom(n => FareRuleFilter.Default))
                .ForMember(m => m.FlightType, m => m.MapFrom(n => FlightType.All))
                .ForMember(m => m.Dow, m => m.MapFrom(n => DOW.Daily))
                .ForMember(m => m.IncludeAllotments, m => m.MapFrom(n => true))
                .ForMember(m => m.SSRCollectionsMode, m => m.MapFrom(n => SSRCollectionsMode.All))
                .ForMember(m => m.LoyaltyFilter, m => m.MapFrom(n => LoyaltyFilter.PointsAndMonetary))
                .ForMember(m => m.JourneySortKeys, m => m.MapFrom(n => new[] { JourneySortKey.EarliestDeparture, JourneySortKey.LowestFare }))
                .ForMember(m => m.PaxCount, m => m.MapFrom(n => n.PaxTypeCounts.Sum(s => s.PaxTypeCode != Global.INFANT_CODE ? s.PaxCount : 0)))
                .ForMember(m => m.PaxPriceTypes, m => m.ResolveUsing<AvailabilityPaxPriceTypeResolver>());
            CreateMap<TripAvailabilityRequest, TripAvailabilityWithSSRRequest>()
                .ForMember(m => m.IncludeSSRList, m => m.MapFrom(n => new[] { Global.INFANT_CODE }));
            CreateMap<dto.LowFareTripAvailabilityRequest, LowFareTripAvailabilityRequest>()
                .ForMember(m => m.FlightFilter, m => m.MapFrom(n => LowFareFlightFilter.AllFlights))
                //.ForMember(m => m.GetAllDetails, m => m.MapFrom(n => true))
                .ForMember(m => m.GroupBydate, m => m.MapFrom(n => true))
                //.ForMember(m => m.BypassCache, m => m.MapFrom(n => true))
                .ForMember(m => m.IncludeTaxesAndFees, m => m.MapFrom(n => true))
                .ForMember(m => m.LoyaltyFilter, m => m.MapFrom(n => LoyaltyFilter.PointsAndMonetary))
                .ForMember(m => m.PaxCount, m => m.MapFrom(n => n.PaxTypeCounts.Sum(s => s.PaxTypeCode != Global.INFANT_CODE ? s.PaxCount : 0)))
                .ForMember(m => m.PaxPriceTypeList, m => m.ResolveUsing<LowFareAvailabilityPaxPriceTypeResolver>());
            CreateMap<dto.LowFareAvailabilityRequest, LowFareAvailabilityRequest>()
                .ForMember(m => m.DepartureStationList, m => m.MapFrom(n => new[] { n.DepartureStation }))
                .ForMember(m => m.ArrivalStationList, m => m.MapFrom(n => new[] { n.ArrivalStation }));
            CreateMap<DateMarketLowFare, dto.DateMarketLowFare>();
            CreateMap<DateFlightLowFare, dto.DateFlightLowFare>();
            CreateMap<DateMarketSegment, dto.DateMarketSegment>();
            CreateMap<DateFlightLeg, dto.DateFlightLeg>();
            CreateMap<DateFlightFares, dto.DateFlightFares>();
            CreateMap<DateFlightFare, dto.DateFlightFare>();
            CreateMap<DateFlightPaxFare, dto.DateFlightPaxFare>();
            CreateMap<dto.Booking, Booking>();
            CreateMap<dto.SellJourneyByKeyRequestData, SellJourneyByKeyRequestData>()
                .ForMember(m => m.ActionStatusCode, m => m.MapFrom(n => "NN"))
                //.ForMember(m => m.PaxCount, m => m.MapFrom(n => n.PaxTypeCounts.Sum(s => s.PaxCount)))
                .ForMember(m => m.PaxCount, m => m.MapFrom(n => n.PaxTypeCounts.ToList().FindAll(p => p.PaxTypeCode != Global.INFANT_CODE).Sum(s => s.PaxCount)))
                .ForMember(m => m.PaxPriceType, m => m.ResolveUsing<SellJourneyPaxPriceTypeResolver>());
            CreateMap<dto.SellKeyList, SellKeyList>().ReverseMap();
            CreateMap<BookingUpdateResponseData, dto.BookingUpdateResponseData>();
            CreateMap<BookingSum, dto.BookingSum>();
            CreateMap<Success, dto.Success>();
            CreateMap<Warning, dto.Warning>();
            CreateMap<Error, dto.Error>();
            CreateMap<Booking, dto.PriceItinararyResponse>();
            CreateMap<Booking, dto.Booking>();
            CreateMap<BookingInfo, dto.BookingInfo>();
            CreateMap<BookingHold, dto.BookingHold>();
            CreateMap<BookingName, dto.BookingName>().ReverseMap();
            CreateMap<Passenger, dto.Passenger>().ReverseMap();
            CreateMap<PassengerInfant, dto.PassengerInfant>().ReverseMap();
            CreateMap<PassengerInfo, dto.PassengerInfo>().ReverseMap();
            CreateMap<PassengerFee, dto.PassengerFee>().ReverseMap();
            //CreateMap<PassengerAddress, dto.PassengerAddress>().ReverseMap();
            CreateMap<PassengerTravelDocument, dto.PassengerTravelDocument>().ReverseMap();
            //CreateMap<PassengerBag, dto.PassengerBag>();
            CreateMap<PassengerTypeInfo, dto.PassengerTypeInfo>().ReverseMap(); ;
            CreateMap<BookingComment, dto.BookingComment>().ReverseMap();
            CreateMap<BookingContact, dto.BookingContact>();
            CreateMap<dto.BookingContact, BookingContact>()
                .ForMember(m => m.TypeCode, m => m.MapFrom(n => "p"));
            CreateMap<PointOfSale, dto.PointOfSale>().ReverseMap(); ;
            CreateMap<ThreeDSecure, dto.ThreeDSecure>().ReverseMap();
            CreateMap<ThreeDSecureRequest, dto.ThreeDSecureRequest>().ReverseMap();
            CreateMap<PaymentField, dto.PaymentField>().ReverseMap(); ;
            CreateMap<Payment, dto.Payment>().ReverseMap(); ;
            CreateMap<dto.UpdatePassengersRequestData, UpdatePassengersRequestData>();
            CreateMap<dto.UpdateContactsRequestData, UpdateContactsRequestData>();
            CreateMap<SSRSegment, dto.SSRSegment>();
            CreateMap<LegKey, dto.LegKey>();
            CreateMap<AvailablePaxSSR, dto.AvailablePaxSSR>().ReverseMap();
            CreateMap<PaxSSRPrice, dto.PaxSSRPrice>().ReverseMap();
            CreateMap<SSRLeg, dto.SSRLeg>().ReverseMap();
            CreateMap<GetSeatAvailabilityResponse, dto.GetSeatAvailabilityResponse>();
            CreateMap<SeatAvailabilityResponse, dto.SeatAvailabilityResponse>();
            CreateMap<AssignSeatsResponse, dto.AssignSeatsResponse>();
            CreateMap<UnassignSeatsResponse, dto.AssignSeatsResponse>();
            CreateMap<EquipmentInfo, dto.EquipmentInfo>();
            CreateMap<SeatGroupPassengerFee, dto.SeatGroupPassengerFee>();
            CreateMap<SeatAvailabilityLeg, dto.SeatAvailabilityLeg>();
            CreateMap<EquipmentPropertyTypeCodesLookup, dto.EquipmentPropertyTypeCodesLookup>();
            CreateMap<EquipmentProperty, dto.EquipmentProperty>();
            CreateMap<CompartmentInfo, dto.CompartmentInfo>();
            CreateMap<SeatInfo, dto.SeatInfo>();
            CreateMap<AssignedSeatInfo, dto.AssignedSeatInfo>();
            CreateMap<AssignedSeatJourney, dto.AssignedSeatJourney>();
            CreateMap<AssignSeatSegment, dto.AssignSeatSegment>();
            CreateMap<PaxSeat, dto.PaxSeat>().ReverseMap();
            CreateMap<PaxSeatInfo, dto.PaxSeatInfo>().ReverseMap();
            CreateMap<dto.TypeOfSale, TypeOfSale>();
            CreateMap<dto.AddPaymentToBookingRequestData, AddPaymentToBookingRequestData>()
                .ForMember(m => m.Status, m => m.MapFrom(n => BookingPaymentStatus.New))
                .ForMember(m => m.ReferenceType, m => m.MapFrom(n => PaymentReferenceType.Default))
                .ForMember(m => m.WaiveFee, m => m.MapFrom(n => false));
            CreateMap<AddPaymentToBookingResponseData, dto.AddPaymentToBookingResponseData>();
            CreateMap<AddPaymentToBookingResponse, dto.AddPaymentToBookingResponse>();
            CreateMap<ValidationPayment, dto.ValidationPayment>();
            CreateMap<PaymentValidationError, dto.PaymentValidationError>();
            CreateMap<CommitResponse, dto.CommitResponse>();
            CreateMap<GetPostCommitResultsResponse, dto.GetPostCommitResultsResponse>()
                .ForMember(m => m.ShouldContinuePolling, m => m.MapFrom(n => n.ContinuePulling));
            CreateMap<dto.RetrieveBookingRequest, GetByRecordLocator>();
            CreateMap<FindBookingResponseData, dto.FindBookingResponseData>();
            CreateMap<FindBookingData, dto.FindBookingData>();
            CreateMap<dto.PaymentVoucher, PaymentVoucher>();
            CreateMap<RemovePaymentFromBookingResponse, dto.RemovePaymentFromBookingResponse>();
            CreateMap<BookingPointOfSale, dto.BookingPointOfSale>().ReverseMap();
            CreateMap<dto.FindBookingRequestData, FindBookingRequestData>();
            CreateMap<dto.FindByAgencyNumber, FindByAgencyNumber>();
            CreateMap<dto.Filter, Filter>();
            CreateMap<dto.FindByRecordLocator, FindByRecordLocator>();
            CreateMap<dto.FindByContactCustomerNumber, FindByContactCustomerNumber>();
            CreateMap<dto.SellFeeRequestData, SellFeeRequestData>();

            // NSK UtilitiesManager objects maps
            CreateMap<GetStationListResponse, dto.GetStationListResponse>();
            CreateMap<Station, dto.Station>();
            CreateMap<GetMarketListResponse, dto.GetMarketListResponse>();
            CreateMap<Market, dto.Market>();
            CreateMap<GetSSRListResponse, dto.GetSSRListResponse>();
            CreateMap<SSR, dto.SSR>().ReverseMap();
            CreateMap<GetFeeListResponse, dto.GetFeeListResponse>();
            CreateMap<UtilitiesManager.Fee, dto.Fee>();
            CreateMap<GetDocTypeListResponse, dto.GetDocTypeListResponse>();
            CreateMap<DocType, dto.DocType>();
            CreateMap<GetTitleListResponse, dto.GetTitleListResponse>();
            CreateMap<Title, dto.Title>();
            CreateMap<GetCountryListResponse, dto.GetCountryListResponse>();
            CreateMap<Country, dto.Country>();
            CreateMap<GetPaymentMethodsListResponse, dto.GetPaymentMethodsListResponse>();
            CreateMap<PaymentMethod, dto.PaymentMethod>();

            // NSK AgentsManager object maps
            CreateMap<FindAgentsResponse, dto.FindAgentsResponse>();
            CreateMap<FindAgentResponseData, dto.FindAgentResponseData>();
            CreateMap<FindAgentItem, dto.FindAgentItem>();
            CreateMap<AgentIdentifier, dto.AgentIdentifier>().ReverseMap();
            CreateMap<dto.FindAgentRequestData, FindAgentRequestData>();
            CreateMap<Agent, dto.Agent>()
                .ForMember(m => m.LoginName, m => m.MapFrom(n => n.AgentIdentifier.AgentName));
            CreateMap<dto.Agent, Agent>();
            CreateMap<AgentRole, dto.AgentRole>().ReverseMap();
            CreateMap<dto.SearchString, SearchString>();
            CreateMap<dto.Person, Person>()
                .ForMember(m => m.PersonEMailList, m => m.MapFrom(n => new PersonEMail[] { new PersonEMail { EMailAddress = n.EmailAddress, TypeCode = "P", Default = true } }))
                .ForMember(m => m.PersonPhoneList, m => m.MapFrom(n => new PersonPhone[] { new PersonPhone { Number = n.MobilePhone, TypeCode = "H", Default = true } }))
                .ForMember(m => m.PersonNameList, m => m.MapFrom(n => new PersonName[] { new PersonName { Name = Mapper.Map<Name>(n.Name) } }))
                .ForMember(m => m.PersonAddressList, m => m.MapFrom(n => new PersonAddress[] { new PersonAddress { Default = true, TypeCode = "H", Address = new Address { City = n.City, CountryCode = n.ResidentCountry } } }));
            CreateMap<Person, dto.Person>()
                .ForMember(m => m.EmailAddress, m => m.MapFrom(n => n.PersonEMailList.ToList().Find(p => p.TypeCode == "P") != null ? n.PersonEMailList.ToList().Find(p => p.TypeCode == "P").EMailAddress : ""))
                .ForMember(m => m.MobilePhone, m => m.MapFrom(n => n.PersonPhoneList.ToList().Find(p => p.TypeCode == "H") != null ? n.PersonPhoneList.ToList().Find(p => p.TypeCode == "H").Number : ""))
                .ForMember(m => m.Name, m => m.MapFrom(n => n.PersonNameList.Length > 0 ? Mapper.Map<dto.Name>(n.PersonNameList[0].Name) : new dto.Name()))
                .ForMember(m => m.City, m => m.MapFrom(n => n.PersonAddressList.Length > 0 ? n.PersonAddressList[0].Address.City : ""));
            CreateMap<dto.Person, PersonManager.Person>()
                .ForMember(m => m.PersonEMailList, m => m.MapFrom(n => new PersonManager.PersonEMail[] { new PersonManager.PersonEMail { EMailAddress = n.EmailAddress, TypeCode = "P", Default = true } }))
                .ForMember(m => m.PersonPhoneList, m => m.MapFrom(n => new PersonManager.PersonPhone[] { new PersonManager.PersonPhone { Number = n.MobilePhone, TypeCode = "H", Default = true } }))
                .ForMember(m => m.PersonNameList, m => m.MapFrom(n => new PersonManager.PersonName[] { new PersonManager.PersonName { Name = Mapper.Map<PersonManager.Name>(n.Name) } }))
                .ForMember(m => m.PersonAddressList, m => m.MapFrom(n => new PersonManager.PersonAddress[] { new PersonManager.PersonAddress { Default = true, TypeCode = "H", Address = new PersonManager.Address { City = n.City /*, CountryCode = n.ResidentCountry*/ } } }));
            CreateMap<PersonManager.Person, dto.Person>()
                .ForMember(m => m.EmailAddress, m => m.MapFrom(n => n.PersonEMailList.ToList().Find(p => p.TypeCode == "P") != null ? n.PersonEMailList.ToList().Find(p => p.TypeCode == "P").EMailAddress : ""))
                .ForMember(m => m.MobilePhone, m => m.MapFrom(n => n.PersonPhoneList.ToList().Find(p => p.TypeCode == "H") != null ? n.PersonPhoneList.ToList().Find(p => p.TypeCode == "H").Number : ""))
                .ForMember(m => m.Name, m => m.MapFrom(n => n.PersonNameList.Length > 0 ? Mapper.Map<dto.Name>(n.PersonNameList[0].Name) : new dto.Name()))
                .ForMember(m => m.City, m => m.MapFrom(n => n.PersonAddressList.Length > 0 ? n.PersonAddressList[0].Address.City : ""));
            CreateMap<dto.PassengerTravelDocument, TravelDoc>()
                .ForMember(m => m.Default, m => m.MapFrom(n => true))
                .ForMember(m => m.Name, m => m.MapFrom(n => n.Names != null && n.Names.Length > 0 ? new Name { Title = n.Names[0].Title, FirstName = n.Names[0].FirstName, LastName = n.Names[0].LastName } : new Name()));
            CreateMap<TravelDoc, dto.PassengerTravelDocument>()
                .ForMember(m => m.Names, m => m.MapFrom(n => n.Name != null ? new dto.BookingName[] { new dto.BookingName { Title = n.Name.Title, FirstName = n.Name.FirstName, LastName = n.Name.LastName } } : null));
            CreateMap<dto.PassengerTravelDocument, PersonManager.TravelDoc>()
                .ForMember(m => m.Default, m => m.MapFrom(n => true))
                .ForMember(m => m.Name, m => m.MapFrom(n => n.Names != null && n.Names.Length > 0 ? new PersonManager.Name { Title = n.Names[0].Title, FirstName = n.Names[0].FirstName, LastName = n.Names[0].LastName } : new PersonManager.Name()));
            CreateMap<PersonManager.TravelDoc, dto.PassengerTravelDocument>()
                .ForMember(m => m.Names, m => m.MapFrom(n => n.Name != null ? new dto.BookingName[] { new dto.BookingName { Title = n.Name.Title, FirstName = n.Name.FirstName, LastName = n.Name.LastName } } : null));
            //CreateMap<PersonManager.PersonName, dto.PersonName>().ReverseMap();
            CreateMap<PersonManager.Name, dto.Name>().ReverseMap();
            CreateMap<dto.CommitAgentRequestData, CommitAgentRequestData>();
            CreateMap<CommitAgentResponse, dto.CommitAgentResponse>();
            CreateMap<CommitAgentResponseData, dto.CommitAgentResponseData>();
            CreateMap<Organization, dto.Organization>().ReverseMap();
            CreateMap<Address, dto.Address>().ReverseMap();

            // NSK AccountManager object maps
            CreateMap<Account, dto.Account>();

            // NSK OperationManager object maps
            CreateMap<dto.ProcessBaggageRequestData, ProcessBaggageRequestData>();
            CreateMap<dto.PassengerBaggageRequest, PassengerBaggageRequest>();
            CreateMap<dto.BaggageInfo, BaggageInfo>().ReverseMap();
            CreateMap<ProcessBaggageResponse, dto.ProcessBaggageResponse>();
            CreateMap<ProcessBaggageResponseData, dto.ProcessBaggageResponseData>();
            CreateMap<JourneyBaggageResponse, dto.JourneyBaggageResponse>();
            CreateMap<PassengerBaggageResponse, dto.PassengerBaggageResponse>();
            CreateMap<dto.CheckInPassengersRequestData, nskop.CheckInPassengersRequestData>();
            CreateMap<dto.CheckInMultiplePassengerRequest, nskop.CheckInMultiplePassengerRequest>()
                .ForMember(m => m.PassengerStatusCode, m => m.MapFrom(n => "HK")) // "HK" equals Confirmed
                .ForMember(m => m.LiftStatus, m => m.MapFrom(n => LiftStatus.CheckedIn))
                .ForMember(m => m.SeatRequired, m => m.MapFrom(n => true))
                .ForMember(m => m.RetrieveBoardingZone, m => m.MapFrom(n => true))
                .ForMember(m => m.BySegment, m => m.MapFrom(n => false));
            CreateMap<dto.InventoryLegKey, InventoryLegKey>().ReverseMap();
            CreateMap<dto.CheckInPaxRequest, nskop.CheckInPaxRequest>();
            CreateMap<CheckInPassengersResponse, dto.CheckInPassengersResponse>();
            CreateMap<nskop.CheckInPassengersResponseData, dto.CheckInPassengersResponseData>();
            CreateMap<nskop.CheckInMultiplePassengerResponse, dto.CheckInMultiplePassengerResponse>();
            CreateMap<nskop.CheckInPaxResponse, dto.CheckInPaxResponse>();
            CreateMap<Name, dto.Name>().ReverseMap();
            CreateMap<nskop.CheckInError, dto.CheckInError>().ReverseMap();
            CreateMap<dto.BoardingPassRequest, nskop.BoardingPassRequest>();
            CreateMap<nskop.BarCodedBoardingPass, dto.BarCodedBoardingPass>();
            CreateMap<GetBarCodedBoardingPassesResponse, dto.GetBarCodedBoardingPassesResponse>();
            CreateMap<nskop.BarCodedBoardingPassLeg, dto.BarCodedBoardingPassLeg>();
            CreateMap<nskop.BarCodedBoardingPassSegment, dto.BarCodedBoardingPassSegment>();
            CreateMap<nskop.BarCode, dto.BarCode>();
            CreateMap<BarCodeType, dto.Enumerations.BarCodeType>().ReverseMap();
        }
    }
}