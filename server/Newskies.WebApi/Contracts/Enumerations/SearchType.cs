using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum SearchType
    {
        [EnumMember]
        Contains = 2,

        [EnumMember]
        EndsWith = 1,

        [EnumMember]
        ExactMatch = 3,

        [EnumMember]
        StartsWith = 0
    }
}
