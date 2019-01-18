using Microsoft.Extensions.Options;
using Newskies.WebApi.Configuration;
using System;

namespace Newskies.WebApi.Services
{
    public class ServiceBase
    {
        protected readonly NewskiesSettings _newskiesSettings;
        protected readonly ServiceEndpoints _navApiEndpoints;
        protected readonly int _navApiContractVer;
        protected readonly string _navMsgContractVer;

        public ServiceBase(IOptions<AppSettings> appSettings)
        {
            _newskiesSettings = appSettings != null && appSettings.Value != null && appSettings.Value.NewskiesSettings != null ?
                appSettings.Value.NewskiesSettings : throw new ArgumentNullException(nameof(appSettings.Value.NewskiesSettings));
            _navApiEndpoints = _newskiesSettings.ServiceEndpoints;
            _navApiContractVer = _newskiesSettings.ApiContractVersion;
            _navMsgContractVer = _newskiesSettings.MsgContractVersion;
        }        
    }
}
