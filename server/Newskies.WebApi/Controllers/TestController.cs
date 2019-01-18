using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Newskies.SessionManager;
using dto = Newskies.WebApi.Contracts;
using Navitaire.WebServices.DataContracts.Booking;
using Navitaire.WebServices.DataContracts.Common.Enumerations;
using Newskies.UtilitiesManager;
using Navitaire.WebServices.DataContracts.Common;
using System.Collections.Generic;
using Newskies.WebApi.Services;
using Newskies.WebApi.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Newskies.WebApi.Filters;

namespace Newskies.WebApi.Controllers
{
    [NonProduction]
    public class TestController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IFlightsService _flightsService;
        private readonly IContactsService _contactsService;
        private readonly IPassengersService _passengersService;
        private readonly IBookingService _bookingService;
        private readonly ISessionBagService _sessionBag;
        private readonly IUserSessionService _userSessionService;
        private readonly IPaymentsService _paymentService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IQueueService _queueService;

        public TestController(IPaymentsService paymentService, ISessionBagService sessionBag, IUserSessionService userSessionService, IMapper mapper,
            IPassengersService passengersService, IBookingService bookingService, IFlightsService flightService, IContactsService contactsService, IHttpContextAccessor httpContextAccessor,
            IQueueService queueService)
        {
            _paymentService = paymentService;
            _mapper = mapper;
            _flightsService = flightService;
            _contactsService = contactsService;
            _passengersService = passengersService;
            _bookingService = bookingService;
            _sessionBag = sessionBag;
            _userSessionService = userSessionService;
            _httpContextAccessor = httpContextAccessor;
            _queueService = queueService;
        }

        // GET api/test
        [HttpGet("api/[controller]")]
        public async Task<IActionResult> Test()
        {
            try
            {
                var result = new List<object>();
                var sessionManager = new SessionManagerClient();
                var logonResponse = await sessionManager.LogonAsync(new LogonRequest
                {
                    //logonRequestData = new LogonRequestData
                    //{
                    //    DomainCode = "WWW",
                    //    AgentName = "awdta1",
                    //    Password = "P@ssw0rd2"
                    //}
                    logonRequestData = new LogonRequestData
                    {
                        DomainCode = "DEF",
                        AgentName = "sculthorpe070884",
                        Password = "Flyadeal12!"
                    }
                });
                result.Add(logonResponse);
                IBookingManager bookingManager = new BookingManagerClient();
                // {
                var bookingResponse = await bookingManager.GetBookingAsync(new GetBookingRequest
                {
                    Signature = logonResponse.Signature,
                    GetBookingReqData = new GetBookingRequestData
                    {
                        GetByRecordLocator = new GetByRecordLocator
                        {
                            RecordLocator = "V4HSFS"/// "EWRT45"
                        }
                    }
                });
                var availRequest = new AvailabilityRequest
                {
                    DepartureStation = "JED",
                    ArrivalStation = "RUH",
                    BeginDate = new DateTime(2017, 10, 01),
                    // BeginTime = new Time(),
                    EndDate = new DateTime(2017, 10, 01),
                    // EndTime = new Time(),
                    PaxPriceTypes = new[] { new PaxPriceType { PaxType = "ADT", PaxCount = 1 } },
                    InboundOutbound = InboundOutbound.Both,
                    FareClassControl = FareClassControl.CompressByProductClass,
                    AvailabilityFilter = AvailabilityFilter.ExcludeUnavailable,
                    AvailabilityType = AvailabilityType.Default,
                    FareRuleFilter = FareRuleFilter.Default,
                    PaxCount = 1,
                    CurrencyCode = "SAR",
                    FareTypes = new[] { "R" },
                    FlightType = FlightType.All,
                    Dow = DOW.Daily,
                    IncludeAllotments = true,
                    IncludeTaxesAndFees = true,
                    SSRCollectionsMode = SSRCollectionsMode.All,
                    LoyaltyFilter = Navitaire.NewSkies.WebServices.DataContracts.Common.Enumerations.LoyaltyFilter.PointsAndMonetary,
                    JourneySortKeys = new[] { JourneySortKey.EarliestDeparture, JourneySortKey.LowestFare },
                    PaxResidentCountry = "SA"
                };
                var availResponse = await bookingManager.GetAvailabilityAsync(new GetAvailabilityRequest
                {
                    Signature = logonResponse.Signature,
                    TripAvailabilityRequest = new TripAvailabilityRequest
                    {
                        LoyaltyFilter = Navitaire.NewSkies.WebServices.DataContracts.Common.Enumerations.LoyaltyFilter.PointsAndMonetary,
                        AvailabilityRequests = new[] { availRequest }
                    }
                });
                dto.GetAvailabilityResponse dtoAvailResponse = _mapper.Map<GetAvailabilityResponse, dto.GetAvailabilityResponse>(availResponse);
                result.Add(dtoAvailResponse);

                var GetTripAvailabilityWithSSRAsyncResp = await bookingManager.GetTripAvailabilityWithSSRAsync(new GetTripAvailabilityWithSSRRequest
                {
                    TripAvailabilityWithSSRRequest = new TripAvailabilityWithSSRRequest
                    {
                        AvailabilityRequests = new AvailabilityRequest[] { availRequest },
                        LoyaltyFilter = Navitaire.NewSkies.WebServices.DataContracts.Common.Enumerations.LoyaltyFilter.PointsAndMonetary,
                        IncludeSSRList = new string[] { "INFT" }
                    },
                    Signature = logonResponse.Signature
                });
                result.Add(GetTripAvailabilityWithSSRAsyncResp);
                /*
                var legKey = new System.Collections.Generic.List<LegKey>();
                foreach (var segment in availResponse.GetTripAvailabilityResponse.Schedules[0][0].Journeys[0].Segments)
                {
                    legKey.Add(new LegKey { ArrivalStation = segment.ArrivalStation, DepartureStation = segment.DepartureStation, CarrierCode = segment.FlightDesignator.CarrierCode.Trim(), DepartureDate = segment.STD, FlightNumber = segment.FlightDesignator.FlightNumber.Trim(), OpSuffix = segment.FlightDesignator.OpSuffix.Trim() });
                }
                GetSSRAvailabilityForBookingRequest getSSRAvailabilityForBookingRequest = new GetSSRAvailabilityForBookingRequest();
                SSRAvailabilityForBookingRequest SSRAvailRequest = new SSRAvailabilityForBookingRequest();
                SSRAvailRequest.SegmentKeyList = legKey.ToArray();
                // "20080303 1L1501 SLCLAX";
                // "20171001 0P 101 JEDRUH";
                // "20171001 0P 101 JEDRUH"
                SSRAvailRequest.CurrencyCode = "SAR";
                SSRAvailRequest.PassengerNumberList = new short[] { 0 };
                SSRAvailRequest.InventoryControlled = true;
                SSRAvailRequest.NonInventoryControlled = true;
                SSRAvailRequest.SeatDependent = true;
                SSRAvailRequest.NonSeatDependent = true;
                getSSRAvailabilityForBookingRequest.SSRAvailabilityForBookingRequest = SSRAvailRequest;
                getSSRAvailabilityForBookingRequest.Signature = logonResponse.Signature;
                var getSSRAvailabilityForBookingresult = await bookingManager.GetSSRAvailabilityForBookingAsync(getSSRAvailabilityForBookingRequest);
                result[2] = getSSRAvailabilityForBookingRequest;
                */
                var legKeyList = new System.Collections.Generic.List<string>();
                foreach (var segment in availResponse.GetTripAvailabilityResponse.Schedules[0][0].Journeys[0].Segments)
                {
                    // "20080303 1L1501 SLCLAX";
                    var legKey = string.Format("{0}{1}{2}{3}{4}{5}",
                        segment.STD.ToString("yyyyMMdd"), segment.FlightDesignator.CarrierCode.PadLeft(3, ' '), segment.FlightDesignator.FlightNumber.PadLeft(4, ' '),
                        segment.FlightDesignator.OpSuffix, segment.DepartureStation, segment.ArrivalStation);
                    legKeyList.Add(legKey);
                }
                GetSSRAvailabilityRequest getSSRAvailabilityRequest = new GetSSRAvailabilityRequest();
                SSRAvailabilityRequest SSRAvailRequest = new SSRAvailabilityRequest();
                SSRAvailRequest.InventoryLegKeys = legKeyList.ToArray();
                SSRAvailRequest.SSRCollectionsMode = SSRCollectionsMode.All;
                getSSRAvailabilityRequest.Signature = logonResponse.Signature;
                getSSRAvailabilityRequest.SSRAvailabilityRequest = SSRAvailRequest;
                var getSSRAvailabilityresult = await bookingManager.GetSSRAvailabilityAsync(getSSRAvailabilityRequest);
                result.Add(getSSRAvailabilityresult);
                // }
                IUtilitiesManager utilities = new UtilitiesManagerClient();
                var stationsResponse = await utilities.GetStationListAsync(new GetStationListRequest
                {
                    Signature = logonResponse.Signature,
                    EnableExceptionStackTrace = true,
                    GetStationListRequestData = new GetStationListRequestData { CultureCode = "en-US" }
                });
                result.Add(stationsResponse);
                IAgentManager agentManager = new AgentManagerClient();
                var rolesResponse = await agentManager.GetRoleListAsync(new GetRoleListRequest { Signature = logonResponse.Signature });
                result.Add(rolesResponse);

                var logoutResponse = await sessionManager.LogoutAsync(new LogoutRequest
                {
                    Signature = logonResponse.Signature
                });
                result.Add(logoutResponse);
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e);
            }
        }

        [HttpGet("api/[controller]/[action]")]
        public async Task<IActionResult> CreateBookingInSession()
        {
            try
            {
                await _sessionBag.Initialise();
                var sellResponse = await _flightsService.SellFlights(new dto.SellJourneyByKeyRequestData
                {
                    CurrencyCode = "SAR",
                    PaxTypeCounts = new dto.PaxTypeCount[] { new dto.PaxTypeCount { PaxCount = 1, PaxTypeCode = "ADT" } },
                    JourneySellKeys = new dto.SellKeyList[] { new dto.SellKeyList { FareSellKey = "0~A~~F3~ADEAL~1003~~1~1~NJEDRUH0010010~X", JourneySellKey = "F3~ 107~ ~~JED~02/10/2018 13:10~RUH~02/10/2018 14:45~~" } }
                });
                var names = new dto.BookingName[] { new dto.BookingName { FirstName = "test", LastName = "test", Title = "MR" } };
                var updateCOnt = await _contactsService.UpdateContacts(new dto.UpdateContactsRequestData
                {
                    BookingContactList = new dto.BookingContact[] { new dto.BookingContact
                 {
                      AddressLine1 = "sdfsdf", City = "sdfsdf", CountryCode = "AU", CultureCode = "en-US", EmailAddress="test@test.com", HomePhone = "234567890", PostalCode = "2343", ProvinceState = "VIC",
                       Names = names
                 }
                }
                });
                var updatePax = await _passengersService.UpdatePassengers(new dto.UpdatePassengersRequestData
                {
                    Passengers = new dto.Passenger[] {
                    new dto.Passenger
                    {
                        Names = names,
                        PassengerTravelDocuments = new dto.PassengerTravelDocument[]
                        {
                            new dto.PassengerTravelDocument
                            {
                                Names = names, DocNumber = "P12121212", DocTypeCode = "P", ExpirationDate = new DateTime(2020,1,1), Gender = dto.Enumerations.Gender.Male, IssuedByCode = "AU"
                            }
                        }, PassengerTypeInfo = new dto.PassengerTypeInfo { DOB = new DateTime(1985,1,1), PaxType = "ADT" },
                         PassengerInfo = new dto.PassengerInfo{ Gender = dto.Enumerations.Gender.Male, Nationality = "US", ResidentCountry = "US"}
                    }
                }
                });
                var booking = await _bookingService.GetSessionBooking(true);
                return new OkObjectResult(booking);
            }
            catch (Exception e)
            {
                return e.ErrorActionResult();
            }
        }

        [HttpPost("api/[controller]/[action]"), ActionFilterInterceptor]
        public async Task<IActionResult> AddPaymentAndCommit([FromBody]dto.AddPaymentToBookingRequestData addPaymentToBookingRequestData)
        {
            try
            {
                //var httpConnectionFeature = _httpContextAccessor.HttpContext.Features.Get<IHttpConnectionFeature>();
                //var ipaddress = httpConnectionFeature?.RemoteIpAddress.ToString();
                //ipaddress = !string.IsNullOrEmpty(ipaddress) ? ipaddress.Trim() : "";
                //ipaddress = ipaddress.Length > 20 ? ipaddress.Substring(0, 20) : ipaddress;
                //var acceptTypes = _httpContextAccessor.HttpContext.Request.Headers["Accept"];
                //var userAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"];
                //var x = await _paymentService.AddPaymentToBooking(new dto.AddPaymentToBookingRequestData
                //{
                //    //AccountNumber = "5123456789012346", // MC normal card
                //    //PaymentMethodCode = "MC",
                //    //AccountNumber = "4005550000000001", // VI normal card
                //    //PaymentMethodCode = "VI",
                //    //AccountNumber = "4557012345678902", // VI 3DS card
                //    //PaymentMethodCode = "VI",
                //    AccountNumber = "5313581000123430", // MC 3DS card
                //    PaymentMethodCode = "MC",
                //    //AccountNumber = "5265742978143572", // MC Decline card
                //    //PaymentMethodCode = "MC",
                //    //AccountNumber = "4532406346746108", // VI Decline card
                //    //PaymentMethodCode = "VI",

                //    CVVCode = "123",
                //    Expiration = new DateTime(2021, 5, 1),
                //    AccountHolderName = "john smith",
                //    //QuotedAmount = sellResponse.BookingUpdateResponseData.Success.PNRAmount.BalanceDue,
                //    //QuotedCurrencyCode = "SAR",

                //    //ThreeDSecureRequest = new dto.ThreeDSecureRequest
                //    //{
                //    //    //BrowserAccept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8",
                //    //    RemoteIpAddress = "139.134.5.36",
                //    //    //BrowserUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.90 Safari/537.36",
                //    //    TermUrl = "http://localhost:5000/tds/blah",
                //    //    BrowserAccept = acceptTypes,
                //    //    //RemoteIpAddress = ipaddress,
                //    //    BrowserUserAgent = userAgent,
                //    //}
                //});
                var addPaymentResult = await _paymentService.AddPaymentToBooking(addPaymentToBookingRequestData);
                var commitResponse = await _bookingService.CommitBooking();
                //var addInProcResponse = await _bookingService.AddInProcessPaymentToBooking();
                var postCommitResponse = await _bookingService.GetPostCommitResults();
                await _sessionBag.SetBookingStateNotInSync();
                return new OkObjectResult(commitResponse);
            }
            catch (Exception e)
            {
                return e.ErrorActionResult();
            }
        }

        [HttpPost("api/[controller]/[action]")]
        public async Task<IActionResult> SellJourneys([FromBody]string[] journeyFareSellKeys)
        {
            var signature = string.Empty;
            var sessionManager = new SessionManagerClient();
            var logonResponse = await sessionManager.LogonAsync(new LogonRequest
            {
                logonRequestData = new LogonRequestData
                {
                    DomainCode = "DEF",
                    AgentName = "sculthorpe070884",
                    Password = "Flyadeal12!"
                }
            });

            var client = new BookingManagerClient();
            var sellKeyList = new SellKeyList[journeyFareSellKeys.Length];
            for (var i = 0; i < journeyFareSellKeys.Length; i++)
            {
                var split = journeyFareSellKeys[i].Split('|');
                sellKeyList[i] = new SellKeyList
                {
                    JourneySellKey = split[0],
                    FareSellKey = split[1]
                };
            }

            var sellRequestData = new SellRequestData
            {
                SellBy = SellBy.JourneyBySellKey,
                SellJourneyByKeyRequest = new SellJourneyByKeyRequest
                {
                    SellJourneyByKeyRequestData = new SellJourneyByKeyRequestData
                    {
                        ActionStatusCode = "NN",
                        CurrencyCode = "SAR",
                        JourneySellKeys = sellKeyList,
                        PaxCount = 1,
                        PaxPriceType = new PaxPriceType[] { new PaxPriceType { PaxCount = 1, PaxType = "ADT" } }
                    }
                }

            };
            var response = await client.SellAsync(348, false, "3.4.13", logonResponse.Signature, sellRequestData);
            return new OkObjectResult(response);
        }

        [HttpPost("api/[controller]/[action]"), RequireSessionBooking]
        public async Task<IActionResult> QueueBooking(string queueCode)
        {
            try
            {
                await _queueService.AddBookingToQueue(queueCode);
                return new OkResult();
            }
            catch (Exception e)
            {
                return e.ErrorActionResult();
            }
        }

    }
}
