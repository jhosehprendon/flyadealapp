using Navitaire.WebServices.DataContracts.Booking;
using Navitaire.WebServices.DataContracts.Common;
using Newskies.UtilitiesManager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Newskies.WebApi.Helpers
{
    public static class NewskiesHelper
    {
        public static readonly DateTime DATE_TIME_MAX_VALUE = new DateTime(9999, 12, 31, 0, 0, 0, DateTimeKind.Utc);

        public static IEnumerable<Journey> GetJourneysFromSellKeys(SellKeyList[] journeySellKeys)
        {
            return journeySellKeys.ToList().Select(sellKey => CreateJourney(sellKey)).ToList();
        }

        public static string[] GetSegmentSellKeys(string journeySellKey)
        {
            if (string.IsNullOrEmpty(journeySellKey))
            {
                return new string[0];
            }
            return journeySellKey.Split(new char[] { '^' });
        }

        public static bool IsJourneySellKeyValid(string journeyKey, out List<string> validationErrors)
        {
            bool flag = true;
            validationErrors = new List<string>();
            try
            {
                bool flag2 = true;
                List<string> collection = new List<string>();
                foreach (string str in journeyKey.Split(new char[] { '^' }))
                {
                    List<string> list2 = null;
                    flag = flag2 && IsSegmentSellKeyValid(str, out list2);
                    if (list2 != null)
                    {
                        collection.AddRange(list2);
                    }
                }
                if (collection.Count > 0)
                {
                    validationErrors.AddRange(collection);
                }
            }
            catch (Exception exception)
            {
                FormatValidationErrors("Encountered exception during JourneyKey validation: " + exception.Message + ".", validationErrors);
                flag = false;
            }
            return flag;
        }

        internal static void FormatValidationErrors(string strToAppend, List<string> listToAppendTo)
        {
            if ((listToAppendTo != null) && !string.IsNullOrEmpty(strToAppend))
            {
                listToAppendTo.Add(strToAppend);
            }
        }

        public static bool IsSegmentSellKeyValid(string segmentKey, out List<string> validationErrors)
        {
            bool flag = true;
            string[] strArray = null;
            validationErrors = new List<string>();
            if (string.IsNullOrEmpty(segmentKey))
            {
                FormatValidationErrors("Segment key cannot be null or blank.", validationErrors);
                return false;
            }
            try
            {
                strArray = segmentKey.Split(new char[] { '~' });
                if ((strArray.Length != 10) && (strArray.Length != 9))
                {
                    FormatValidationErrors(string.Concat(new object[] { "Should have 10 fields, but actually had ", strArray.Length, " fields." }), validationErrors);
                    flag = false;
                }
                if (string.IsNullOrEmpty(strArray[0]))
                {
                    FormatValidationErrors("'CarrierCode' cannot be null or blank.", validationErrors);
                    flag = false;
                }
                if (string.IsNullOrEmpty(strArray[1]))
                {
                    FormatValidationErrors("'FlightNumber' cannot be null or blank.", validationErrors);
                    flag = false;
                }
                if (string.IsNullOrEmpty(strArray[2]))
                {
                    FormatValidationErrors("'OpSuffix' cannot be null or blank.", validationErrors);
                    flag = false;
                }
                else if (strArray[2].Length != 1)
                {
                    FormatValidationErrors("'OpSuffix' must be exactly 1 char in length.", validationErrors);
                    flag = false;
                }
                if (string.IsNullOrEmpty(strArray[4]))
                {
                    FormatValidationErrors("'DepartureStation' cannot be null or blank.", validationErrors);
                    flag = false;
                }
                flag = flag && !string.IsNullOrEmpty(strArray[6]);
                if (string.IsNullOrEmpty(strArray[6]))
                {
                    FormatValidationErrors("'ArrivalStation' cannot be null or blank.", validationErrors);
                    flag = false;
                }
                DateTime.ParseExact(strArray[5], "g", CultureInfo.InvariantCulture);
                DateTime.ParseExact(strArray[7], "g", CultureInfo.InvariantCulture);
            }
            catch (Exception exception)
            {
                FormatValidationErrors("Encountered exception during FareSellKey validation: " + exception.Message + ".", validationErrors);
                flag = false;
            }
            return flag;
        }

        public static bool IsFareSellKeyValid(string fareSellKey, out List<string> validationErrors)
        {
            validationErrors = new List<string>();
            if (string.IsNullOrEmpty(fareSellKey))
            {
                return false;
            }
            bool flag = true;
            try
            {
                char[] separator = new char[] { '^' };
                foreach (string str in fareSellKey.Split(separator))
                {
                    char[] chArray2 = new char[] { '~' };
                    string[] strArray3 = str.Split(chArray2);
                    if (strArray3.Length != 11)
                    {
                        FormatValidationErrors(string.Concat(new object[] { "Should have ", 11, " fields, but actually had ", strArray3.Length, " fields." }), validationErrors);
                        flag = false;
                    }
                    if (string.IsNullOrEmpty(strArray3[1]))
                    {
                        FormatValidationErrors("'ClassOfService' cannot be null or blank.", validationErrors);
                        flag = false;
                    }
                    if (string.IsNullOrEmpty(strArray3[3]))
                    {
                        FormatValidationErrors("'CarrierCode' cannot be null or blank.", validationErrors);
                        flag = false;
                    }
                    if (string.IsNullOrEmpty(strArray3[4]))
                    {
                        FormatValidationErrors("'FareBasis' cannot be null or blank.", validationErrors);
                        flag = false;
                    }
                    if (string.IsNullOrEmpty(strArray3[5]))
                    {
                        FormatValidationErrors("'RuleNumber' cannot be null or blank.", validationErrors);
                        flag = false;
                    }
                    if (string.IsNullOrEmpty(strArray3[8]))
                    {
                        FormatValidationErrors("'FareSequence' cannot be null or blank.", validationErrors);
                        flag = false;
                    }
                }
            }
            catch (Exception exception)
            {
                FormatValidationErrors("Encountered exception during FareSellKey validation: " + exception.Message + ".", validationErrors);
                flag = false;
            }
            return flag;
        }

        public static Journey CreateJourney(SellKeyList sellKeyList)
        {
            var journey = new Journey();
            var segments = new List<Segment>();
            string[] segmentSellKeys = GetSegmentSellKeys(sellKeyList.JourneySellKey);
            if ((segmentSellKeys != null) && (segmentSellKeys.Length > 0))
            {
                for (int i = 0; i < segmentSellKeys.Length; i++)
                {
                    var key = segmentSellKeys[i];
                    string[] strArray = segmentSellKeys[i].Split('~');
                    var segment = new Segment();
                    segment.ArrivalStation = strArray[6];
                    segment.DepartureStation = strArray[4];
                    segment.FlightDesignator = new FlightDesignator
                    {
                        CarrierCode = strArray[0],
                        FlightNumber = strArray[1],
                        OpSuffix = strArray[2]
                    };
                    segment.STD = DateTime.ParseExact(strArray[5], "g", CultureInfo.InvariantCulture);
                    segment.STA = DateTime.ParseExact(strArray[7], "g", CultureInfo.InvariantCulture);
                    segments.Add(segment);
                }
            }
            journey.Segments = segments.ToArray();
            return journey;
        }

        public static List<SegmentSSRRequest> CreateSegmentSSRRequests(List<Journey> journeys, SSR ssr, int[] paxNumbers, int ssrCount,
            List<Tuple<int, List<Tuple<int, int[]>>>> journeySegmentLegIndexes, Dictionary<int, List<short>> ssrNumbersToCancelDict = null, string note = "")
        {
            var segmentSSRRequests = new List<SegmentSSRRequest>();
            foreach (var journeyTuple in journeySegmentLegIndexes)
            {
                var journeyIndex = journeyTuple.Item1;
                foreach (var segmentTuple in journeyTuple.Item2)
                {
                    var segmentIndex = segmentTuple.Item1;
                    if (segmentTuple.Item2 != null && segmentTuple.Item2.Length != 0) // SSR is sold per leg
                    {
                        foreach (var legIndex in segmentTuple.Item2)
                        {
                            var leg = journeys[journeyIndex].Segments[segmentIndex].Legs[legIndex];
                            var segmentSSRRequest = CreateSegmentSSRRequest(ssr, leg.DepartureStation,
                                leg.ArrivalStation, paxNumbers, leg.FlightDesignator, leg.STD, ssrCount, ssrNumbersToCancelDict, note);
                            segmentSSRRequests.Add(segmentSSRRequest);
                        }
                    }
                    else // SSR is sold per segment
                    {
                        var segment = journeys[journeyIndex].Segments[segmentIndex];
                        var segmentSSRRequest = CreateSegmentSSRRequest(ssr, segment.DepartureStation,
                            segment.ArrivalStation, paxNumbers, segment.FlightDesignator, segment.STD, ssrCount, ssrNumbersToCancelDict, note);
                        segmentSSRRequests.Add(segmentSSRRequest);
                    }
                }
            }
            return segmentSSRRequests;
        }

        private static SegmentSSRRequest CreateSegmentSSRRequest(SSR ssr, string departureStation, string arrivalStation, int[] paxNumbers,
            FlightDesignator flightDesignator, DateTime departureDateTime, int ssrCount, Dictionary<int, List<short>> ssrNumbersToCancelDict = null, string note = "")
        {
            var list = new List<PaxSSR>();
            foreach (var paxNumber in paxNumbers)
            {
                for (var i = 0; i < ssrCount; i++)
                {
                    var isCancel = ssrNumbersToCancelDict != null && ssrNumbersToCancelDict.ContainsKey(paxNumber);
                    short ssrNumber = isCancel ? ssrNumbersToCancelDict[paxNumber].LastOrDefault() : (short)i;
                    list.Add(new PaxSSR
                    {
                        ArrivalStation = arrivalStation,
                        DepartureStation = departureStation,
                        FeeCode = ssr.FeeCode,
                        PassengerNumber = (short)paxNumber,
                        SSRCode = ssr.SSRCode,
                        SSRDetail = string.Empty,
                        Note = note,
                        SSRNumber = ssrNumber
                    });
                    if (isCancel)
                        ssrNumbersToCancelDict[paxNumber].Remove(ssrNumber);
                }
            }
            var segmentSSRRequest = new SegmentSSRRequest
            {
                ArrivalStation = arrivalStation,
                DepartureStation = departureStation,
                FlightDesignator = flightDesignator,
                PaxSSRs = list.ToArray(),
                STD = departureDateTime
            };
            return segmentSSRRequest;
        }

        public static DateTime GetJourneySTD(this Journey journey)
        {
            if (journey.Segments == null || journey.Segments.Length == 0)
                return default(DateTime);
            return journey.Segments[0].STD;
        }

        public static DateTime GetJourneySTA(this Journey journey)
        {
            if (journey.Segments == null || journey.Segments.Length == 0)
                return default(DateTime);
            return journey.Segments[journey.Segments.Length - 1].STA;
        }

        public static string GetUpdateResponseError(BookingUpdateResponseData bookingUpdateResponseData)
        {
            if (bookingUpdateResponseData == null)
                return null;
            if (bookingUpdateResponseData.Error != null)
                return bookingUpdateResponseData.Error.ErrorText;
            if (bookingUpdateResponseData.Warning != null)
                return bookingUpdateResponseData.Warning.WarningText;
            return null;
        }
    }
}





