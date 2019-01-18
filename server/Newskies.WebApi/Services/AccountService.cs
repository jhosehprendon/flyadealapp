using AutoMapper;
using Microsoft.Extensions.Options;
using Newskies.AccountManager;
using Newskies.WebApi.Configuration;
using System;
using System.Threading.Tasks;
using dto = Newskies.WebApi.Contracts;

namespace Newskies.WebApi.Services
{
    public interface IAccountService
    {
        Task<dto.Account> GetAccount();
    }

    public class AccountService : ServiceBase, IAccountService
    {
        private readonly ISessionBagService _sessionBag;
        private readonly IAccountManager _accountClient;

        public AccountService(ISessionBagService sessionBag, IAccountManager accountClient, IOptions<AppSettings> appSettings) : base(appSettings)
        {
            _sessionBag = sessionBag ?? throw new ArgumentNullException(nameof(sessionBag));
            _accountClient = accountClient ?? throw new ArgumentNullException(nameof(accountClient));
        }

        public async Task<dto.Account> GetAccount()
        {
            var resp = await _accountClient.GetAccountByReferenceAsync(new GetAccountByReferenceRequest
            {
                ContractVersion = _navApiContractVer,
                MessageContractVersion = _navMsgContractVer,
                EnableExceptionStackTrace = false,
                Signature = await _sessionBag.Signature(),
                GetAccountByReferenceReqData = new GetAccountByReferenceRequestData
                {
                    AccountReference = await _sessionBag.OrganizationCode()
                }
            });
            return  Mapper.Map<dto.Account>(resp.Account);
        }
    }
}
