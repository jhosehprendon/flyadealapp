using System.Runtime.Serialization;

namespace Newskies.WebApi.Contracts.Enumerations
{
    [DataContract]
    public enum DOW
    {
        [EnumMember]
        None = 0,

        [EnumMember]
        Monday = 1,

        [EnumMember]
        Tuesday = 2,

        [EnumMember]
        Wednesday = 3,

        [EnumMember]
        Thursday = 4,

        [EnumMember]
        Friday = 5,

        [EnumMember]
        WeekDay = 6,

        [EnumMember]
        Saturday = 7,

        [EnumMember]
        Sunday = 8,

        [EnumMember]
        WeekEnd = 9,

        [EnumMember]
        Daily = 10,

        [EnumMember]
        Unmapped = -1,
    }
}
