using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class GetOrganizationRequestData
    {
        [DataMember, Required, ValidationInterceptor]
        public string OrganizationCode { get; set; }

        //[DataMember, ValidationInterceptor]
        //public bool GetDetails { get; set; }
    }
}
