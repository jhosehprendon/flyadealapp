﻿using System;
using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class Leg
    {
        [DataMember, ValidationInterceptor]
        public string ArrivalStation { get; set; }

        [DataMember, ValidationInterceptor]
        public string DepartureStation { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime STA { get; set; }

        [DataMember, ValidationInterceptor]
        public DateTime STD { get; set; }

        [DataMember, ValidationInterceptor]
        public FlightDesignator FlightDesignator { get; set; }

        [DataMember, ValidationInterceptor]
        public LegInfo LegInfo { get; set; }
    }
}