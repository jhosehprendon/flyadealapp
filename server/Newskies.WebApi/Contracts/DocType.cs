using System.Runtime.Serialization; using Newskies.WebApi.Validation;

namespace Newskies.WebApi.Contracts
{
    [DataContract]
    public class DocType
    {
        [DataMember, ValidationInterceptor]
        public string DocTypeCode { get; set; }

        [DataMember, ValidationInterceptor]
        public string TypeName { get; set; }

        //[DataMember, ValidationInterceptor]
        //public schemas.navitaire.com.WebServices.DataContracts.Common.Enumerations.DocumentType DocumentType { get; set; }

        //[DataMember, ValidationInterceptor]
        //public schemas.navitaire.com.WebServices.DataContracts.Common.Enumerations.DocRequiredPropertyType DocRequiredPropertyType { get; set; }

        [DataMember, ValidationInterceptor]
        public bool InActive { get; set; }
    }
}
