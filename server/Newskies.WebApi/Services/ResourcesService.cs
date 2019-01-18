using System;
using System.Threading.Tasks;
using AutoMapper;
using Newskies.UtilitiesManager;
using dto = Newskies.WebApi.Contracts;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;
using Newskies.WebApi.Constants;
using Microsoft.Extensions.Options;
using Newskies.WebApi.Configuration;
using System.ServiceModel;
using System.Linq;
using System.Collections.Generic;

namespace Newskies.WebApi.Services
{
    public interface IResourcesService
    {
        Task<dto.GetStationListResponse> GetStationList(string cultureCode = null);
        Task<dto.GetMarketListResponse> GetMarketList();
        Task<dto.GetCultureListResponse> GetCultureList();
        Task<dto.GetCurrencyListResponse> GetCurrencyList();
        Task<dto.GetPaxTypeListResponse> GetPaxTypeList();
        Task<dto.GetSSRListResponse> GetSSRList(string cultureCode = null);
        Task<dto.GetFeeListResponse> GetFeeList(string cultureCode = null);
        Task<dto.GetDocTypeListResponse> GetDocTypeList(string cultureCode = null);
        Task<dto.GetTitleListResponse> GetTitleList(string cultureCode = null);
        Task<dto.GetCountryListResponse> GetCountryList(string cultureCode = null);
        Task<dto.GetPaymentMethodsListResponse> GetPaymentMethodsList(string cultureCode = null);
        Task<dto.StationsMarkets> GetStationsMarketsSimplfied(string cultureCode = null);
    }

    public class ResourcesService : ServiceBase, IResourcesService
    {
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly ISessionBagService _sessionBag;
        private readonly IUtilitiesManager _client;
        private readonly IUserSessionService _userSessionService;
        private readonly AppSettings _airlineSettings; 
        private static readonly SemaphoreSlim _semaphoreSlimStationList = new SemaphoreSlim(1, 1);
        private static readonly SemaphoreSlim _semaphoreSlimMarketList = new SemaphoreSlim(1, 1);
        private static readonly SemaphoreSlim _semaphoreSlimSSRList = new SemaphoreSlim(1, 1);
        private static readonly SemaphoreSlim _semaphoreSlimFeeList = new SemaphoreSlim(1, 1);
        private static readonly SemaphoreSlim _semaphoreSlimTravelDocTypeList = new SemaphoreSlim(1, 1);
        private static readonly SemaphoreSlim _semaphoreSlimTitleList = new SemaphoreSlim(1, 1);
        private static readonly SemaphoreSlim _semaphoreSlimCountryList = new SemaphoreSlim(1, 1);
        private static readonly SemaphoreSlim _semaphoreSlimPaymentMethodsList = new SemaphoreSlim(1, 1);

        public ResourcesService(IMemoryCache cache, IMapper mapper, ISessionBagService sessionBag, IUtilitiesManager client,
            IUserSessionService userSessionService, IOptions<AppSettings> appSettings,
            IOptions<AppSettings> airlineSettings) : base(appSettings)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache)); ;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _sessionBag = sessionBag ?? throw new ArgumentNullException(nameof(sessionBag));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _userSessionService = userSessionService ?? throw new ArgumentNullException(nameof(userSessionService));
            _airlineSettings = airlineSettings != null && airlineSettings.Value != null ? airlineSettings.Value : 
                throw new ArgumentNullException(nameof(airlineSettings));
        }

        public async Task<dto.GetStationListResponse> GetStationList(string cultureCode = null)
        {
            var culture = !string.IsNullOrEmpty(cultureCode) ? cultureCode : await _sessionBag.CultureCode();
            var signature = !string.IsNullOrEmpty(await _sessionBag.Signature())
                    ? await _sessionBag.Signature()
                    : await _userSessionService.GetAnonymousSharedSignature();
            var stationListCacheKey = string.Format("stationList_{0}_{1}", culture, await _sessionBag.RoleCode());

            var cachedStationList = _cache.Get<dto.GetStationListResponse>(stationListCacheKey);
            if (cachedStationList != null)
                return cachedStationList;
            await _semaphoreSlimStationList.WaitAsync();
            try
            {
                cachedStationList = _cache.Get<dto.GetStationListResponse>(stationListCacheKey);
                if (cachedStationList != null)
                    return cachedStationList;
                var stationsResp = await _client.GetStationListAsync(new GetStationListRequest
                {
                    ContractVersion = _navApiContractVer,
                    MessageContractVersion = _navMsgContractVer,
                    Signature = signature,
                    EnableExceptionStackTrace = false,
                    GetStationListRequestData = new GetStationListRequestData { CultureCode = culture }
                });
                //_navApiContractVer, false,
                //_navMsgContractVer, signature, new GetStationListRequestData { CultureCode = culture });
                return _cache.Set(stationListCacheKey, _mapper.Map<dto.GetStationListResponse>(stationsResp),
                    _newskiesSettings.ResourcesCachePeriod);
            }
            finally
            {
                _semaphoreSlimStationList.Release();
            }
        }

        public async Task<dto.GetMarketListResponse> GetMarketList()
        {
            var signature = !string.IsNullOrEmpty(await _sessionBag.Signature())
                        ? await _sessionBag.Signature()
                        : await _userSessionService.GetAnonymousSharedSignature();
            var marketListCacheKey = string.Format("marketList_{0}", await _sessionBag.RoleCode());
            var cachedMarketList = _cache.Get<dto.GetMarketListResponse>(marketListCacheKey);
            if (cachedMarketList != null)
                return cachedMarketList;
            await _semaphoreSlimMarketList.WaitAsync();
            try
            {
                cachedMarketList = _cache.Get<dto.GetMarketListResponse>(marketListCacheKey);
                if (cachedMarketList != null)
                    return cachedMarketList;
                var getMarketListResp = await _client.GetMarketListAsync(new GetMarketListRequest
                {
                    ContractVersion = _navApiContractVer,
                    MessageContractVersion = _navMsgContractVer,
                    Signature = signature,
                    EnableExceptionStackTrace = false
                });
                //_navApiContractVer, false,
                //_navMsgContractVer, signature);
                return _cache.Set(marketListCacheKey, _mapper.Map<dto.GetMarketListResponse>(getMarketListResp),
                    _newskiesSettings.ResourcesCachePeriod);
            }
            finally
            {
                _semaphoreSlimMarketList.Release();
            }
        }

        public async Task<dto.GetCultureListResponse> GetCultureList()
        {
            var result = new dto.GetCultureListResponse
            {
                CultureList = _airlineSettings.Cultures
            };
            return await Task.FromResult(result);
        }

        public async Task<dto.GetCurrencyListResponse> GetCurrencyList()
        {
            var result = new dto.GetCurrencyListResponse
            {
                CurrencyList = _airlineSettings.Currencies
            };
            return await Task.FromResult(result);
        }

        public async Task<dto.GetPaxTypeListResponse> GetPaxTypeList()
        {
            var result = new dto.GetPaxTypeListResponse
            {
                PaxTypeList = new[]
                {
                    new dto.PaxType {Code = Global.ADULT_CODE, Name = "Adult"},
                    new dto.PaxType {Code = Global.CHILD_CODE, Name = "Child"},
                    new dto.PaxType {Code = Global.INFANT_CODE, Name = "Infant"}
                }
            };
            return await Task.FromResult(result);
        }

        public async Task<dto.GetSSRListResponse> GetSSRList(string cultureCode = null)
        {
            var culture = !string.IsNullOrEmpty(cultureCode) ? cultureCode : await _sessionBag.CultureCode();
            var signature = !string.IsNullOrEmpty(await _sessionBag.Signature())
                        ? await _sessionBag.Signature()
                        : await _userSessionService.GetAnonymousSharedSignature();
            var ssrListCacheKey = string.Format("ssrList_{0}_{1}", culture, await _sessionBag.RoleCode());
            var cachedSSRList = _cache.Get<dto.GetSSRListResponse>(ssrListCacheKey);
            if (cachedSSRList != null)
                return cachedSSRList;
            await _semaphoreSlimSSRList.WaitAsync();
            try
            {
                cachedSSRList = _cache.Get<dto.GetSSRListResponse>(ssrListCacheKey);
                if (cachedSSRList != null)
                    return cachedSSRList;
                var getSSRListResp = await _client.GetSSRListAsync(new GetSSRListRequest
                {
                    ContractVersion = _navApiContractVer,
                    MessageContractVersion = _navMsgContractVer,
                    Signature = signature,
                    EnableExceptionStackTrace = false,
                    GetSSRListRequestData = new GetSSRListRequestData { CultureCode = culture }
                });
                //_navApiContractVer, false,
                //_navMsgContractVer, signature, new GetSSRListRequestData { CultureCode = culture });
                return _cache.Set(ssrListCacheKey, _mapper.Map<dto.GetSSRListResponse>(getSSRListResp),
                    _newskiesSettings.ResourcesCachePeriod);
            }
            finally
            {
                _semaphoreSlimSSRList.Release();
            }
        }

        public async Task<dto.GetFeeListResponse> GetFeeList(string cultureCode = null)
        {
            var culture = !string.IsNullOrEmpty(cultureCode) ? cultureCode : await _sessionBag.CultureCode();
            var signature = !string.IsNullOrEmpty(await _sessionBag.Signature())
                        ? await _sessionBag.Signature()
                        : await _userSessionService.GetAnonymousSharedSignature();
            var feeListCacheKey = string.Format("feeList_{0}_{1}", culture, _sessionBag.RoleCode());
            var cachedFeeList = _cache.Get<dto.GetFeeListResponse>(feeListCacheKey);
            if (cachedFeeList != null)
                return cachedFeeList;
            await _semaphoreSlimFeeList.WaitAsync();
            try
            {
                cachedFeeList = _cache.Get<dto.GetFeeListResponse>(feeListCacheKey);
                if (cachedFeeList != null)
                    return cachedFeeList;
                var getFeeListResp = await _client.GetFeeListAsync(new GetFeeListRequest
                {
                    ContractVersion = _navApiContractVer,
                    MessageContractVersion = _navMsgContractVer,
                    Signature = signature,
                    EnableExceptionStackTrace = false,
                    GetFeeListRequestData = new GetFeeListRequestData { CultureCode = culture }
                });
                //_navApiContractVer, false,
                //_navMsgContractVer, signature, new GetFeeListRequestData { CultureCode = culture });
                return _cache.Set(feeListCacheKey, _mapper.Map<dto.GetFeeListResponse>(getFeeListResp),
                    _newskiesSettings.ResourcesCachePeriod);
            }
            finally
            {
                _semaphoreSlimFeeList.Release();
            }
        }

        public async Task<dto.GetDocTypeListResponse> GetDocTypeList(string cultureCode = null)//, string station1 = "", string station2 = "")
        {
            var culture = !string.IsNullOrEmpty(cultureCode) ? cultureCode : await _sessionBag.CultureCode();
            var signature = !string.IsNullOrEmpty(await _sessionBag.Signature())
                        ? await _sessionBag.Signature()
                        : await _userSessionService.GetAnonymousSharedSignature();
            var travelDocTypeListCacheKey = string.Format("travelDocTypeList_{0}_{1}", culture, _sessionBag.RoleCode());
            var cachedTravelDocTypeList = _cache.Get<dto.GetDocTypeListResponse>(travelDocTypeListCacheKey);
            if (cachedTravelDocTypeList != null)
                return cachedTravelDocTypeList;
            await _semaphoreSlimTravelDocTypeList.WaitAsync();
            try
            {
                cachedTravelDocTypeList = _cache.Get<dto.GetDocTypeListResponse>(travelDocTypeListCacheKey);
                if (cachedTravelDocTypeList != null)
                    return cachedTravelDocTypeList;
                var getTravelDocTypeListResp = await _client.GetDocTypeListAsync(new GetDocTypeListRequest
                {
                    ContractVersion = _navApiContractVer,
                    MessageContractVersion = _navMsgContractVer,
                    Signature = signature,
                    EnableExceptionStackTrace = false,
                    GetDocTypeListRequestData = new GetDocTypeListRequestData { CultureCode = culture }
                });
                //_navApiContractVer, false,
                //_navMsgContractVer, signature, new GetDocTypeListRequestData { CultureCode = culture });
                return _cache.Set(travelDocTypeListCacheKey, _mapper.Map<dto.GetDocTypeListResponse>(getTravelDocTypeListResp),
                    _newskiesSettings.ResourcesCachePeriod);
            }
            finally
            {
                _semaphoreSlimTravelDocTypeList.Release();
            }
        }

        public async Task<dto.GetTitleListResponse> GetTitleList(string cultureCode = null)
        {
            var culture = !string.IsNullOrEmpty(cultureCode) ? cultureCode : await _sessionBag.CultureCode();
            var signature = !string.IsNullOrEmpty(await _sessionBag.Signature())
                        ? await _sessionBag.Signature()
                        : await _userSessionService.GetAnonymousSharedSignature();
            var titleListCacheKey = string.Format("titleList_{0}_{1}", culture, await _sessionBag.RoleCode());
            var cachedTitleList = _cache.Get<dto.GetTitleListResponse>(titleListCacheKey);
            if (cachedTitleList != null)
                return cachedTitleList;
            await _semaphoreSlimTitleList.WaitAsync();
            try
            {
                cachedTitleList = _cache.Get<dto.GetTitleListResponse>(titleListCacheKey);
                if (cachedTitleList != null)
                    return cachedTitleList;
                var getTitleListResp = await _client.GetTitleListAsync(new GetTitleListRequest
                {
                    ContractVersion = _navApiContractVer,
                    MessageContractVersion = _navMsgContractVer,
                    Signature = signature,
                    EnableExceptionStackTrace = false,
                    GetTitleListRequestData = new GetTitleListRequestData { CultureCode = culture }
                });
                //_navApiContractVer, false,
                //_navMsgContractVer, signature, new GetTitleListRequestData { CultureCode = culture });
                return _cache.Set(titleListCacheKey,
                    _mapper.Map<dto.GetTitleListResponse>(getTitleListResp), _newskiesSettings.ResourcesCachePeriod);
            }
            finally
            {
                _semaphoreSlimTitleList.Release();
            }
        }

        public async Task<dto.GetCountryListResponse> GetCountryList(string cultureCode = null)
        {
            var culture = !string.IsNullOrEmpty(cultureCode) ? cultureCode : await _sessionBag.CultureCode();
            var signature = !string.IsNullOrEmpty(await _sessionBag.Signature())
                        ? await _sessionBag.Signature()
                        : await _userSessionService.GetAnonymousSharedSignature();
            var CountryListCacheKey = string.Format("CountryList_{0}_{1}", culture, await _sessionBag.RoleCode());
            var cachedCountryList = _cache.Get<dto.GetCountryListResponse>(CountryListCacheKey);
            if (cachedCountryList != null)
                return cachedCountryList;
            await _semaphoreSlimCountryList.WaitAsync();
            try
            {
                cachedCountryList = _cache.Get<dto.GetCountryListResponse>(CountryListCacheKey);
                if (cachedCountryList != null)
                    return cachedCountryList;
                var getCountryListResp = await _client.GetCountryListAsync(new GetCountryListRequest
                {
                    ContractVersion = _navApiContractVer,
                    MessageContractVersion = _navMsgContractVer,
                    Signature = signature,
                    EnableExceptionStackTrace = false,
                    GetCountryListRequestData = new GetCountryListRequestData { CultureCode = culture }
                });
                //_navApiContractVer, false,
                //_navMsgContractVer, signature, new GetCountryListRequestData { CultureCode = culture });
                return _cache.Set(CountryListCacheKey,
                    _mapper.Map<dto.GetCountryListResponse>(getCountryListResp), _newskiesSettings.ResourcesCachePeriod);
            }
            finally
            {
                _semaphoreSlimCountryList.Release();
            }
        }

        public async Task<dto.GetPaymentMethodsListResponse> GetPaymentMethodsList(string cultureCode = null)
        {
            var culture = !string.IsNullOrEmpty(cultureCode) ? cultureCode : await _sessionBag.CultureCode();
            var signature = !string.IsNullOrEmpty(await _sessionBag.Signature())
                        ? await _sessionBag.Signature()
                        : await _userSessionService.GetAnonymousSharedSignature();
            var paymentMethodListCacheKey = string.Format("paymentMethodList_{0}_{1}", culture, await _sessionBag.RoleCode());
            var cachedPaymentMethodsList = _cache.Get<dto.GetPaymentMethodsListResponse>(paymentMethodListCacheKey);
            if (cachedPaymentMethodsList != null)
                return cachedPaymentMethodsList;
            await _semaphoreSlimPaymentMethodsList.WaitAsync();
            try
            {
                cachedPaymentMethodsList = _cache.Get<dto.GetPaymentMethodsListResponse>(paymentMethodListCacheKey);
                if (cachedPaymentMethodsList != null)
                    return cachedPaymentMethodsList;
                var getPaymentMethodsListResp = await _client.GetPaymentMethodsListAsync(new GetPaymentMethodsListRequest
                {
                    ContractVersion = _navApiContractVer,
                    MessageContractVersion = _navMsgContractVer,
                    Signature = signature,
                    EnableExceptionStackTrace = false,
                    GetPaymentMethodsListRequestData = new GetPaymentMethodsListRequestData { CultureCode = culture }
                });
                //_navApiContractVer, false,
                //_navMsgContractVer, signature, new GetPaymentMethodsListRequestData { CultureCode = culture });
                return _cache.Set(paymentMethodListCacheKey,
                    _mapper.Map<dto.GetPaymentMethodsListResponse>(getPaymentMethodsListResp), _newskiesSettings.ResourcesCachePeriod);
            }
            finally
            {
                _semaphoreSlimPaymentMethodsList.Release();
            }
        }

        public async Task<dto.StationsMarkets> GetStationsMarketsSimplfied(string cultureCode = null)
        {
            var stationsResponse = await GetStationList(cultureCode);
            var marketsResponse = await GetMarketList();

            var stations = new List<dto.StationSimplified>();
            stationsResponse.StationList.ToList().ForEach(s => stations.Add(new dto.StationSimplified
            {
                Code = s.StationCode,
                Country = s.CountryCode,
                Latitude = s.Latitude,
                Longitude = s.Longitude,
                Name = s.ShortName,
                Currency = s.CurrencyCode
            }));

            var marketsDic = new Dictionary<string, List<string>>();
            foreach (var market in marketsResponse.MarketList.ToList().FindAll(p => !p.InActive))
                if (marketsDic.ContainsKey(market.LocationCode))
                    marketsDic[market.LocationCode].Add(market.TravelLocationCode);
                else
                    marketsDic.Add(market.LocationCode, new List<string> { market.TravelLocationCode });
            var markets = new List<dto.MarketSimplified>();
            foreach (var key in marketsDic.Keys)
                markets.Add(new dto.MarketSimplified { Key = key, Value = marketsDic[key].ToArray() });

            return new dto.StationsMarkets { StationsList = stations.ToArray(), MarketsList = markets.ToArray() };
        }
    }
}