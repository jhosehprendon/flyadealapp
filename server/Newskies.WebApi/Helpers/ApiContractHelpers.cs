using Newskies.WebApi.Contracts;
using Newskies.WebApi.Contracts.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Newskies.WebApi.Helpers
{
    public static class ApiContractHelpers
    {
        #region Journey helpers

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
        public static DateTime GetDepartureDate(this Journey journey)
        {
            if (journey.Segments == null || journey.Segments.Length == 0)
                return default(DateTime);
            return journey.Segments[0].STD.Date;
        }

        public static DateTime GetArrivalDate(this Journey journey)
        {
            if (journey.Segments == null || journey.Segments.Length == 0)
                return default(DateTime);
            return journey.Segments[journey.Segments.Length - 1].STA.Date;
        }

        public static string GetDepartureStation(this Journey journey)
        {
            if (journey.Segments == null || journey.Segments.Length == 0)
                return null;
            return journey.Segments[0].DepartureStation;
        }

        public static string GetArrivalStation(this Journey journey)
        {
            if (journey.Segments == null || journey.Segments.Length == 0)
                return null;
            return journey.Segments[journey.Segments.Length - 1].ArrivalStation;
        }

        public static string GetSegmentType(this Journey journey)
        {
            if (journey.Segments == null || journey.Segments.Length == 0)
                return string.Empty;
            return journey.Segments[0].SegmentType;
        }

        //public static FlightType GetJourneyType(this Journey journey)
        //{
        //    if (journey.Segments != null && journey.Segments.Length > 0)
        //    {
        //        if (journey.Segments.Length > 1)
        //            return FlightType.Connect;
        //        if (journey.Segments.Length == 1)
        //            return journey.Segments[0].GetNumberOfStops() <= 0 ? FlightType.NonStop : FlightType.Through;
        //    }
        //    return FlightType.None;
        //}

        public static DateTime GetUTCDeptDateTime(this Journey journey)
        {
            if (journey.Segments != null && journey.Segments.Length > 0)
                return journey.Segments[0].GetUTCDeptDateTime();
            return default(DateTime);
        }

        public static DateTime GetUTCArrvDateTime(this Journey journey)
        {
            if (journey.Segments != null && journey.Segments.Length > 0)
                return journey.Segments[journey.Segments.Length - 1].GetUTCArrvDateTime();
            return default(DateTime);
        }

        public static TimeSpan GetTravelTime(this Journey journey)
        {
            DateTime utcDeptDateTime = journey.GetUTCDeptDateTime();
            DateTime utcArrvDateTime = journey.GetUTCArrvDateTime();
            if (utcDeptDateTime != default(DateTime) && utcArrvDateTime != default(DateTime))
                return utcArrvDateTime.Subtract(utcDeptDateTime);
            return TimeSpan.Zero;
        }

        public static bool IsAllPaxCheckedIn(this Journey journey)
        {
            if (journey.Segments != null && journey.Segments.Length > 0)
                return journey.Segments[0].IsAllPaxCheckedIn();
            return false;
        }

        public static int GetNumberOfStops(this Journey journey)
        {
            int num = 0;
            if (journey.Segments != null)
            {
                for (int index = 0; index < journey.Segments.Length; ++index)
                    num += journey.Segments[index].GetNumberOfStops() + 1;
            }
            return num - 1;
        }

        public static int GetNumberOfLegs(this Journey journey)
        {
            int num = 0;
            if (journey.Segments != null)
            {
                for (int index = 0; index < journey.Segments.Length; ++index)
                    num += journey.Segments[index].Legs.Length;
            }
            return num;
        }

        public static List<string> GetClassesOfService(this Journey journey)
        {
            List<string> stringList = new List<string>();
            if (journey.Segments != null)
            {
                foreach (Segment segment in journey.Segments)
                    stringList.Add(segment.GetClassOfService());
            }
            return stringList;
        }

        public static string GetSegmentClassOfService(this Journey journey)
        {
            string empty = string.Empty;
            if (journey.Segments != null)
            {
                for (int index = 0; index < journey.Segments.Length; ++index)
                {
                    Segment segment = journey.Segments[index];
                    if (empty != segment.GetClassOfService())
                        empty += segment.GetClassOfService();
                }
            }
            return empty;
        }

        public static bool IsPassive(this Journey journey)
        {
            return journey.Segments != null && journey.Segments.Length > 0 && journey.Segments[0].SegmentType == "P";
        }

        public static bool IsNotPassive(this Journey journey)
        {
            return !journey.IsPassive();
        }

        #endregion

        #region Segment helpers

        public static bool IsAllPaxCheckedIn(this Segment segment)
        {
            return segment.PaxSegments != null && segment.PaxSegments.All(ps => ps.LiftStatus != LiftStatus.Default);
        }

        public static int GetNumberOfStops(this Segment segment)
        {
            if (segment.Legs == null || segment.Legs.Length <= 0)
                return 0;
            return segment.Legs.Length - 1;
        }
        public static bool IsPaxCheckedIn(this Segment segment, short passengerNumber)
        {
            if (segment.PaxSegments != null && segment.PaxSegments.Length > 0)
            {
                for (int index = 0; index < segment.PaxSegments.Length; ++index)
                {
                    PaxSegment paxSegment = segment.PaxSegments[index];
                    if (paxSegment.PassengerNumber == passengerNumber)
                    {
                        if (paxSegment.LiftStatus != LiftStatus.Default)
                            return true;
                        break;
                    }
                }
            }
            return false;
        }

        public static bool IsPaxOnBoard(this Segment segment, short passengerNumber)
        {
            if (segment.PaxSegments != null && segment.PaxSegments.Length > 0)
            {
                for (int index = 0; index < segment.PaxSegments.Length; ++index)
                {
                    PaxSegment paxSegment = segment.PaxSegments[index];
                    if ((int)paxSegment.PassengerNumber == (int)passengerNumber)
                    {
                        if (paxSegment.LiftStatus == LiftStatus.Boarded)
                            return true;
                        break;
                    }
                }
            }
            return false;
        }

        public static DateTime GetUTCDeptDateTime(this Segment segment)
        {
            if (segment.Legs != null && segment.Legs.Length > 0 && segment.Legs[0].LegInfo != null)
                return segment.Legs[0].GetUTCDeptDateTime();
            return segment.STD;
        }

        public static DateTime GetUTCArrvDateTime(this Segment segment)
        {
            if (segment.Legs != null && segment.Legs.Length > 0)
                return segment.Legs[segment.Legs.Length - 1].GetUTCArrvDateTime();
            return segment.STA;
        }

        public static TimeSpan GetTravelTime(this Segment segment)
        {
            if (segment.Legs != null && segment.Legs.Length > 0)
            {
                DateTime utcDeptDateTime = segment.GetUTCDeptDateTime();
                DateTime utcArrvDateTime = segment.GetUTCArrvDateTime();
                if (utcDeptDateTime != default(DateTime) && utcArrvDateTime != default(DateTime))
                    return utcArrvDateTime.Subtract(utcDeptDateTime);
            }
            return TimeSpan.Zero;
        }

        public static TimeSpan GetTimeBeforeDeparture(this Segment segment)
        {
            if (segment.Legs != null && segment.Legs.Length > 0)
            {
                return segment.GetUTCDeptDateTime().Subtract(DateTime.UtcNow);
            }
            return TimeSpan.Zero;
        }

        public static string GetFareClassOfService(this Segment segment)
        {
            if (segment.Fares.Length <= 0)
                return string.Empty;
            return segment.Fares[0].FareClassOfService;
        }

        public static string GetClassOfService(this Segment segment)
        {
            if (segment.Fares.Length <= 0)
                return string.Empty;
            return segment.Fares[0].ClassOfService;
        }

        public static string GetOriginalClassOfService(this Segment segment)
        {
            if (segment.Fares.Length <= 0)
                return string.Empty;
            return segment.Fares[0].OriginalClassOfService;
        }

        public static string GetXrefClassOfService(this Segment segment)
        {
            if (segment.Fares.Length <= 0)
                return string.Empty;
            return segment.Fares[0].XrefClassOfService;
        }

        public static string GetBookingSourceClassOfService(this Segment segment)
        {
            if (segment.Fares.Length <= 0)
                return string.Empty;
            return segment.Fares[0].GetBookingSourceClassOfService();
        }

        public static string GetOperatingClassOfService(this Segment segment)
        {
            if (segment.Fares.Length <= 0)
                return string.Empty;
            return segment.Fares[0].GetOperatingClassOfService();
        }

        public static string GetClassType(this Segment segment)
        {
            if (segment.Fares.Length <= 0)
                return string.Empty;
            return segment.Fares[0].ClassType;
        }

        //public static ClassStatus GetClassStatus(this Segment segment)
        //{
        //    if (segment.Fares.Length > 0 && segment.Fares[0] as AvailableFare != null)
        //        return ((AvailableFare)segment.Fares[0]).Status;
        //    return ClassStatus.AVSOpen;
        //}

        public static string GetProductClass(this Segment segment)
        {
            if (segment.Fares.Length <= 0)
                return string.Empty;
            return segment.Fares[0].ProductClass;
        }

        public static string GetTravelClassCode(this Segment segment)
        {
            if (segment.Fares.Length <= 0)
                return " ";
            return segment.Fares[0].TravelClassCode;
        }

        public static void GetLegNumbers(this Segment segment, string departureStation, string arrivalStation, out int startNum, out int endNum)
        {
            startNum = 0;
            endNum = 0;
            for (int index = 0; index < segment.Legs.Length; ++index)
            {
                Leg leg = segment.Legs[index];
                if (leg.DepartureStation == departureStation)
                    startNum = index + 1;
                if (startNum > 0 && leg.ArrivalStation == arrivalStation)
                {
                    endNum = index + 1;
                    break;
                }
            }
        }

        public static bool IsUpgraded(this Segment segment)
        {
            if (segment.Fares.Length > 0 && segment.Fares[0].ClassOfService != segment.Fares[0].FareClassOfService)
                return segment.Fares[0].FareClassOfService != string.Empty;
            return false;
        }
        #endregion

        #region Leg helpers
        public static DateTime GetUTCDeptDateTime(this Leg leg)
        {
            if (leg.LegInfo == null)
                return default(DateTime);
            DateTime dateTime = leg.STD.AddMinutes(leg.LegInfo.DeptLTV * -1);
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, DateTimeKind.Utc);
        }

        public static DateTime GetUTCArrvDateTime(this Leg leg)
        {
            DateTime dateTime = leg.LegInfo != null ? leg.STA.AddMinutes(leg.LegInfo.ArrvLTV * -1) : leg.STA;
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, DateTimeKind.Utc);
        }

        public static TimeSpan GetTimeBeforeDeparture(this Leg leg)
        {
            if (leg.LegInfo != null)
                return leg.GetUTCDeptDateTime().Subtract(DateTime.UtcNow);
            return TimeSpan.Zero;
        }
        #endregion

        #region Fare helpers

        public static string GetBookingSourceClassOfService(this Fare fare)
        {
            string empty = string.Empty;
            if (!fare.XrefClassOfService.Contains("/"))
                return fare.XrefClassOfService;
            return fare.XrefClassOfService.Split('/')[0].Trim();
        }

        public static string GetOperatingClassOfService(this Fare fare)
        {
            string empty = string.Empty;
            if (!fare.XrefClassOfService.Contains("/"))
                return string.Empty;
            string[] strArray = fare.XrefClassOfService.Split('/');
            int index = strArray.Length - 1;
            return strArray[index].Trim();
        }

        #endregion
    }
}





