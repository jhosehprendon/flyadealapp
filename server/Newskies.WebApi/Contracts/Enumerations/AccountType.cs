using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum AccountType
    {
        [EnumMember]
        Default = 0,

        [EnumMember]
        Credit = 1,

        [EnumMember]
        Prepaid = 2,

        [EnumMember]
        Dependent = 3,

        [EnumMember]
        Supplementary = 4,

        [EnumMember]
        Unmapped = -1,
    }
}
