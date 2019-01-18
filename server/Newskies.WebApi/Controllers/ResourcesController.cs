using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newskies.WebApi.Services;
using Newskies.WebApi.Filters;
using System;
using Newskies.WebApi.Contracts;

namespace Newskies.WebApi.Controllers
{
    [Authorization, ValidateModel, ActionFilterInterceptor, ApiExceptionFilter]
    [Route("api/[controller]")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ResourcesController : Controller
    {
        private readonly IResourcesService _resourcesService;

        public ResourcesController(IResourcesService resourcesService)
        {
            _resourcesService = resourcesService ?? throw new ArgumentNullException(nameof(resourcesService));
        }

        // GET: api/Resources
        [HttpGet]
        public async Task<IActionResult> Get(string culture = null)
        {
            var stationListResponse = await _resourcesService.GetStationList(culture);
            var marketListResponse = await _resourcesService.GetMarketList();
            var cultureListResponse = await _resourcesService.GetCultureList();
            var currencyListResponse = await _resourcesService.GetCurrencyList();
            var paxTypeListResponse = await _resourcesService.GetPaxTypeList();
            var travelDocTypeListResponse = await _resourcesService.GetDocTypeList(culture);
            var feeListResponse = await _resourcesService.GetFeeList(culture);
            var ssrList = await _resourcesService.GetSSRList(culture);
            var titleList = await _resourcesService.GetTitleList(culture);
            var countryList = await _resourcesService.GetCountryList(culture);
            var paymentMethods = await _resourcesService.GetPaymentMethodsList(culture);
            var resources = new AllResourcesResponse
            {
                StationList = stationListResponse.StationList,
                MarketList = marketListResponse.MarketList,
                CultureList = cultureListResponse.CultureList,
                CurrencyList = currencyListResponse.CurrencyList,
                PaxTypeList = paxTypeListResponse.PaxTypeList,
                FeeList = feeListResponse.FeeList,
                DocTypeList = travelDocTypeListResponse.DocTypeList,
                SSRList = ssrList.SSRList,
                TitleList = titleList.TitleList,
                CountryList = countryList.CountryList,
                PaymentMethodList = paymentMethods.PaymentMethodList,
            };
            return new OkObjectResult(resources);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Stations(string culture = null)
        {
            return new OkObjectResult(await _resourcesService.GetStationList(culture));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Markets()
        {
            return new OkObjectResult(await _resourcesService.GetMarketList());
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Cultures()
        {
            return new OkObjectResult(await _resourcesService.GetCultureList());
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Currencies()
        {
            return new OkObjectResult(await _resourcesService.GetCurrencyList());
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> PaxTypes()
        {
            return new OkObjectResult(await _resourcesService.GetPaxTypeList());
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> SSRs(string culture = null)
        {
            return new OkObjectResult(await _resourcesService.GetSSRList(culture));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Fees(string culture = null)
        {
            return new OkObjectResult(await _resourcesService.GetFeeList(culture));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> DocumentTypes(string culture = null)
        {
            return new OkObjectResult(await _resourcesService.GetDocTypeList(culture));
        }

        // [HttpGet("[action]"), RequireSessionBooking]
        //public async Task<IActionResult> DocumentTypesForBooking(string culture = null)
        //{
        //        return new OkObjectResult(await _resourcesService.GetDocTypeList(culture));
        //}

        [HttpGet("[action]")]
        public async Task<IActionResult> Titles(string culture = null)
        {
            return new OkObjectResult(await _resourcesService.GetTitleList(culture));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Countries(string culture = null)
        {
            return new OkObjectResult(await _resourcesService.GetCountryList(culture));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> PaymentMethodsTypes(string culture = null)
        {
            return new OkObjectResult(await _resourcesService.GetPaymentMethodsList(culture));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> StationsMarkets(string culture = null)
        {
            return new OkObjectResult(await _resourcesService.GetStationsMarketsSimplfied(culture));
        }
    }
}