using Newskies.WebApi.Validation;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts
{
    public class GetOrganizationResponse
    {
        public Organization Organization { get; set; }
    }
}
