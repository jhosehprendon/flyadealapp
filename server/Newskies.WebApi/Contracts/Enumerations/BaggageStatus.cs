using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum BaggageStatus 
    {
        [EnumMember]
        Added = 3,
        [EnumMember]
        AddedPrinted = 4,
        [EnumMember]
        Checked = 1,
        [EnumMember]
        Default = 0,
        [EnumMember]
        Removed = 2
    }
}
