using Navitaire.WebServices.DataContracts.Booking;
using Newskies.WebApi.Constants;
using System.Collections.Generic;
using dto = Newskies.WebApi.Contracts;

namespace Newskies.WebApi.Mapping
{
    internal static class ConverterHelpers
    {
        internal static PaxPriceType[] ConvertPaxTypeCounts(dto.PaxTypeCount[] paxTypeCounts)
        {
            var list = new List<PaxPriceType>();
            foreach (var item in paxTypeCounts)
                if (item.PaxTypeCode != Global.INFANT_CODE)
                    list.Add(new PaxPriceType { PaxCount = item.PaxCount, PaxType = item.PaxTypeCode });
            return list.ToArray();
        }
    }
}
