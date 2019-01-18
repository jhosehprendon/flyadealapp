using Newskies.WebApi.Contracts;
using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Flyadeal.Interceptors.Helpers
{
    public static class ActionsHelper
    {
        internal static void PopulateCheckInInformation(this Booking booking)
        {
            if (booking == null)
            {
                return;
            }
            var segments = booking.Journeys.SelectMany(j => j.Segments);
            foreach (var segment in segments)
            {
                foreach (var paxSegment in segment.PaxSegments)
                {
                    paxSegment.CheckInStatus = GetPaxSegmentCheckInStatus(paxSegment.PassengerNumber, segment.DepartureStation, segment.ArrivalStation, booking);
                }
            }
        }

        internal static PaxSegmentCheckInStatus? GetPaxSegmentCheckInStatus(short paxNumber, string segmentOrigin, string segmentDestination, Booking booking)
        {
            if (string.IsNullOrEmpty(booking.RecordLocator) || booking.BookingInfo.BookingStatus != BookingStatus.Confirmed)
            {
                return PaxSegmentCheckInStatus.BookingNotComplete;
            }
            var segment = booking.Journeys.SelectMany(j => j.Segments).FirstOrDefault(s => s.DepartureStation == segmentOrigin && s.ArrivalStation == segmentDestination);
            if (segment == null)
            {
                return null;
            }
            //if (Constants.CheckInAllowedStations.Length > 0 && Constants.CheckInAllowedStations.All(code => !code.Equals(segment.DepartureStation, StringComparison.OrdinalIgnoreCase)))
            //{
            //    return PaxSegmentCheckInStatus.RestrictedByAirport;
            //}
            var paxSegment = segment.PaxSegments.FirstOrDefault(ps => ps.PassengerNumber == paxNumber);
            if (paxSegment == null)
            {
                return null;
            }
            if (paxSegment.LiftStatus != Newskies.WebApi.Contracts.Enumerations.LiftStatus.Default)
            {
                return PaxSegmentCheckInStatus.AlreadyCheckedIn;
            }
            var utcDepTime = segment.GetUTCDeptDateTime();
            if (utcDepTime > DateTime.UtcNow && utcDepTime - DateTime.UtcNow < Constants.CheckInMinimumPriorToDeparture)
            {
                return PaxSegmentCheckInStatus.TooCloseToDeparture;
            }
            if (utcDepTime < DateTime.UtcNow)
            {
                return PaxSegmentCheckInStatus.FlightHasAlreadyDeparted;
            }
            if (utcDepTime - Constants.CheckInMaxumumPriorToDeparture > DateTime.UtcNow)
            {
                return PaxSegmentCheckInStatus.CheckInNotYetOpen;
            }
            return PaxSegmentCheckInStatus.Allowed;
        }

        internal static void HideSensitivePaymentInformation(this Booking booking)
        {
            if (booking.Payments != null)
            {
                booking.Payments.ToList().ForEach(p => 
                {
                    p.PaymentFields = null;
                    p.Expiration = default(DateTime);
                    p.AccountName = null;
                });
            }
        }

        internal static string GetPaymentFieldValue(this Payment payment, string fieldName)
        {
            var field = payment.PaymentFields.ToList().Find(
                        f => f.FieldName.Equals(fieldName, StringComparison.OrdinalIgnoreCase));
            return field != null ? field.FieldValue : null;
        }

        internal static PaymentField[] FilterPaymentFields(this Payment payment, List<string> fieldNames)
        {
            var fields = payment.PaymentFields.ToList().FindAll(f => fieldNames.Contains(f.FieldName));
            return fields.ToArray();
        }
    }
}
