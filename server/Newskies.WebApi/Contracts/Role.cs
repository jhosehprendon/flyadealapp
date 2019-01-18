using System.Runtime.Serialization; using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class Role
    {
        [DataMember, Required, ValidationInterceptor]
        public string RoleCode { get; set; }

        //[DataMember, ValidationInterceptor]
        public string RoleName { get; set; }

        //[DataMember, ValidationInterceptor]
        public string ParentRoleCode { get; set; }
    }
}
