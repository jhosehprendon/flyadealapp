using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Newskies.WebApi.Validation;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Constants;
using System.Linq;

namespace Flyadeal.Interceptors.Validation
{
    public class PaxTypeCountInterceptor : IValidationInterceptor
    {
        private readonly int _maxAdults;
        private readonly int _maxChildren;
        private readonly int _maxTotal;

        public PaxTypeCountInterceptor() : this(9, 8, 9) { }

        public PaxTypeCountInterceptor(int maxAdults = 9, int maxChildren = 8, int maxTotal = 9)
        {
            _maxAdults = maxAdults;
            _maxChildren = maxChildren;
            _maxTotal = maxTotal;
        }

        public ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var paxTypeCounts = (PaxTypeCount[])value;
            if (paxTypeCounts == null || paxTypeCounts.Length == 0)
            {
                return new ValidationResult("Missing pax counts.");
            }
            var paxTypeList = new List<string> { Global.ADULT_CODE, Global.CHILD_CODE, Global.INFANT_CODE };
            foreach (var pc in paxTypeCounts)
            {
                if (string.IsNullOrEmpty(pc.PaxTypeCode) || !paxTypeList.Contains(pc.PaxTypeCode.ToUpper()))
                {
                    return new ValidationResult(string.Format("Invalid pax type code: {0}",
                        !string.IsNullOrEmpty(pc.PaxTypeCode) ? pc.PaxTypeCode : "NULL"));
                }
            }
            var errorMsg = string.Empty;
            var memberNames = new List<string>();
            var adultCount = Array.Find(paxTypeCounts, p => p.PaxTypeCode == Global.ADULT_CODE);
            var childCount = Array.Find(paxTypeCounts, p => p.PaxTypeCode == Global.CHILD_CODE);
            var infantCount = Array.Find(paxTypeCounts, p => p.PaxTypeCode == Global.INFANT_CODE);
            var totalPaxCount = Array.FindAll(paxTypeCounts, p => p.PaxTypeCode != Global.INFANT_CODE).Sum(p => p.PaxCount);
            if (adultCount == null || adultCount.PaxCount < 1 || adultCount.PaxCount > 10)
            {
                errorMsg += string.Format("Adult count must be between 1 and {0}. ", _maxAdults);
            }
            if (childCount != null && childCount.PaxCount > 10)
            {
                errorMsg += string.Format("Child count must be between 0 and {0}. ", _maxChildren);
            }
            if (infantCount != null && infantCount.PaxCount > adultCount.PaxCount)
            {
                errorMsg += "Infant count must not exceed Adult count. ";
            }
            if (totalPaxCount > _maxTotal)
            {
                errorMsg += string.Format("Total passenger count must not exceed {0}. ", _maxTotal);
            }
            if (!string.IsNullOrEmpty(errorMsg))
                return new ValidationResult(errorMsg.Trim());
            return ValidationResult.Success;
        }
    }
}
