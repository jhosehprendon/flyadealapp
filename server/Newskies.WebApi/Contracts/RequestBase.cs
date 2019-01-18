using Newskies.WebApi.Validation;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class RequestBase
    {
        //public bool ActiveOnlyField;

        //[DataMember, ValidationInterceptor]
        public short PageSize { get; set; }

        [DataMember, ValidationInterceptor]
        public long LastID { get; set; }

        //private string LastCodeField;

        //private string CultureCodeField;

        //private bool GetTotalCountField;
    }
}
