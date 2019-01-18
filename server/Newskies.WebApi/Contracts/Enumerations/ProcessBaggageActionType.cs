using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum ProcessBaggageActionType
    {
        [EnumMember]
        Add = 1,

        [EnumMember]
        CheckIn = 2,

        [EnumMember]
        None = 0,

        [EnumMember]
        Print = 4
    }
}
