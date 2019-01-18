using Newskies.WebApi.Contracts;
using Newskies.WebApi.Services;
using System;
using am = AutoMapper;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using nsk = Navitaire.WebServices.DataContracts.Booking;
using System.Reflection;
using System.Threading.Tasks;

namespace Newskies.WebApi.Helpers
{
    public static class ValidationHelper
    {
        /// <summary>
        /// Default Currency Code validation. The currency code of the first journey must match its departing station's currency code.
        /// </summary>
        /// 
        public static ValidationResult DefaultCurrencyValidation(string currencyCode, ValidationContext validationContext)
        {
            try
            {
                if (string.IsNullOrEmpty(currencyCode))
                    return new ValidationResult("Currency code required.");
                var objInstance = validationContext.ObjectInstance;
                var objType = objInstance.GetType();
                var departureStation = string.Empty;
                var stationsService = validationContext.GetService(
                    typeof(IResourcesService)) as ResourcesService;
                var stations = Task.Run(async () => await stationsService.GetStationList()).Result.StationList.ToList();
                if (objType == typeof(TripAvailabilityRequest))
                {
                    var tripAvailabilityRequest = (TripAvailabilityRequest)objInstance;
                    departureStation = tripAvailabilityRequest.AvailabilityRequests[0].DepartureStation;
                }
                else if (objType == typeof(LowFareTripAvailabilityRequest))
                {
                    var lowFareTripAvailabilityRequest = (LowFareTripAvailabilityRequest)objInstance;
                    departureStation = lowFareTripAvailabilityRequest.LowFareAvailabilityRequestList[0].DepartureStation;
                }
                else if (objType == typeof(SellJourneyByKeyRequestData))
                {
                    var sellJourneyByKeyRequestData = (SellJourneyByKeyRequestData)objInstance;
                    var journeys = NewskiesHelper.GetJourneysFromSellKeys(am.Mapper.Map<nsk.SellKeyList[]>(sellJourneyByKeyRequestData.JourneySellKeys)).ToArray();
                    if (journeys.FirstOrDefault().Segments.Count() == 0) // likely that first journey hasn't been selected yet, so look at arrival of second journey
                        departureStation = journeys[1].Segments.LastOrDefault().ArrivalStation;
                    else
                        departureStation = journeys.FirstOrDefault().Segments.FirstOrDefault().DepartureStation;
                }

                if (!string.IsNullOrEmpty(departureStation))
                {
                    var station = stations.Find(m => m.StationCode == departureStation);
                    if (station != null && station.CurrencyCode != currencyCode)
                        return new ValidationResult(string.Format("Incorrect currency code {0} for departure flight. It must be {1}.",
                            currencyCode, station.CurrencyCode));
                }
                return ValidationResult.Success;
            }
            catch (Exception e)
            {
                return new ValidationResult("Unable to validate currency code. " + e.Message);
            }
        }

        public static ValidationResult ValidateJourneySegmentLegIndexes(this Booking booking, int journeyIndex, int segmentIndex, int? legIndex = null)
        {
            if (journeyIndex >= booking.Journeys.Length)
                return new ValidationResult(string.Format("JourneyIndex {0} out of bounds.", journeyIndex));
            if (segmentIndex >= booking.Journeys[journeyIndex].Segments.Length)
                return new ValidationResult(string.Format("SegmentIndex {0} out of bounds.", segmentIndex));
            if (legIndex.HasValue && legIndex.Value >= booking.Journeys[journeyIndex].Segments[segmentIndex].Legs.Length)
                return new ValidationResult(string.Format("LegIndex {0} out of bounds.",legIndex.Value));
            return null;
        }

        public static string GetTypeName(this object value)
        {
            var typeInfo = value.GetType().GetTypeInfo();
            var assemblyName = typeInfo.Assembly.GetName().Name;
            var fullTypeName = typeInfo.FullName;
            return $"{fullTypeName},{assemblyName}";
        }

        public static ISessionBagService GetSessionBagService(this ValidationContext validationContext)
        {
            var sessionBagService = validationContext.GetService(typeof(ISessionBagService)) as SessionBagService;
            if (sessionBagService == null || !Task.Run(async () => await sessionBagService.Initialised()).Result)
                throw new ResponseErrorException(Contracts.Enumerations.ResponseErrorCode.Unauthorised, "Unauthorised");
            return sessionBagService;
        }
    }
}
