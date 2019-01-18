import { NewskiesClient, Contracts, SessionTokenHeaderInterceptor, SessionService, ResourceService } from 'newskies-services';
import { FlyadealClientConfig, PaxTypeConfig } from "../Contracts";

interface Cache<T> {
    [K: string]: T;
}

export default class ResourceCache {

    private _config: FlyadealClientConfig;
    private _resourceService: ResourceService;
    private _markets: Contracts.Market[];
    private _docTypes: Cache<Contracts.DocType[]> = {};
    private _stations: Cache<Contracts.Station[]> = {};
    private _countries: Cache<Contracts.Country[]> = {};
    private _titles: Cache<Contracts.Title[]> = {};
    private _cultures: Contracts.Culture[];
    private _currencies: Contracts.Currency[];
    private _paxTypes: Contracts.PaxType[];
    private _fees: Cache<Contracts.Fee[]> = {};
    private _ssrs: Cache<Contracts.SSR[]> = {};
    private _paymentMethods: Cache<Contracts.PaymentMethod[]> = {};

    constructor(resourceService: ResourceService, config: FlyadealClientConfig) {
        this._resourceService = resourceService;
        this._config = config;
    }

    async getAllResources(culture: string = this._config.defaultCulture): Promise<Contracts.AllResourcesResponse> {
        try {
            if (this._paxTypes && this._markets && this._currencies
                && this._cultures && this._stations && this._stations[culture]
                && this._fees && this._fees[culture] && this._countries && this._countries[culture]
                && this._titles && this._titles[culture]
                && this._docTypes && this._docTypes[culture]
                && this._ssrs && this._ssrs[culture]
                && this._paymentMethods && this._paymentMethods[culture]) {
                return {
                    cultureList: this._cultures,
                    stationList: this._stations[culture],
                    marketList: this._markets,
                    currencyList: this._currencies,
                    paxTypeList: this._paxTypes,
                    feeList: this._fees[culture],
                    countryList: this._countries[culture],
                    titleList: this._titles[culture],
                    docTypeList: this._docTypes[culture],
                    ssrList: this._ssrs[culture],
                    paymentMethodList: this._paymentMethods[culture]
                };
            }
            const resources = await this._resourceService.getAllResources(culture);
            this._cultures = resources.cultureList;
            this._stations[culture] = resources.stationList;
            this._countries[culture] = resources.countryList;
            this._markets = resources.marketList;
            this._currencies = resources.currencyList;
            this._paxTypes = resources.paxTypeList;
            this._titles[culture] = resources.titleList;
            this._fees[culture] = resources.feeList;
            this._docTypes[culture] = resources.docTypeList;
            this._ssrs[culture] = resources.ssrList;
            this._paymentMethods[culture] = resources.paymentMethodList;
            return resources;
        } catch (error) {
            throw error;
        }
    }

    async clear() {
        this._markets = null;
        this._docTypes = {};
        this._stations = {};
        this._countries = {};
        this._titles = {};
        this._cultures = null;
        this._currencies = null;
        this._paxTypes = null;
        this._fees = {};
        this._ssrs = {};
        this._paymentMethods = {};
    }

    async clearDocTypes() {
        this._docTypes = {};
    }

    async clearPaymentMethods() {
        this._paymentMethods = {};
    }

    async getMarkets(): Promise<Contracts.Market[]>{
        try {
            if (this._markets) {
                return this._markets;
            }
            const markets = await this._resourceService.getMarkets();
            this._markets = markets;
            return markets;
        } catch (error) {
            throw error;
        }
    }

    async getStations(culture: string = this._config.defaultCulture): Promise<Contracts.Station[]> {
        try {
            const cached = this._stations[culture];
            if (cached) {
                return cached;
            }
            const stations = await this._resourceService.getStations(culture);
            this._stations[culture] = stations;
            return stations;
        } catch (error) {
            throw error;
        }
    }

    async getStationCurrency(stationCode: string, culture: string = this._config.defaultCulture): Promise<string> {
        try {
            const stations = await this.getStations(culture);
            const station = stations.find(s => s.stationCode === stationCode);
            return station ? station.currencyCode : 'SAR';
        } catch (error) {
            throw error;
        }
    }

    async getFees(culture: string = this._config.defaultCulture): Promise<Contracts.Fee[]> {
        try {
            const cached = this._fees[culture];
            if (cached) {
                return cached;
            }
            const fees = await this._resourceService.getFees(culture);
            this._fees[culture] = fees;
            return fees;
        } catch (error) {
            throw error;
        }
    }

    async getCultures(): Promise<Contracts.Culture[]> {
        try {
            if (this._cultures) {
                return this._cultures;
            }
            const cultures = await this._resourceService.getCultures();
            this._cultures = cultures;
            return cultures;
        } catch (error) {
            throw error;
        }
    }

    async getCurrencies(): Promise<Contracts.Currency[]> {
        try {
            if (this._currencies) {
                return this._currencies;
            }
            const currencies = await this._resourceService.getCurrencies();
            this._currencies = currencies;
            return currencies;
        } catch (error) {
            throw error;
        }
    }

    async getPaxTypes(culture: string = this._config.defaultCulture): Promise<Contracts.PaxType[]> {
        try {
            if (this._paxTypes) {
                return this._paxTypes;
            }
            const paxTypes = await this._resourceService.getPaxTypes(culture);
            this._paxTypes = paxTypes;
            return paxTypes;
        } catch (error) {
            throw error;
        }
    }

    async getTitles(culture: string = this._config.defaultCulture): Promise<Contracts.Title[]> {
        try {
            const cached = this._titles[culture];
            if (cached) {
                return cached;
            }
            const titles = await this._resourceService.getTitles(culture);
            this._titles[culture] = titles;
            return titles;
        } catch (error) {
            throw error;
        }
    }

    async getCountries(culture: string = this._config.defaultCulture): Promise<Contracts.Country[]> {
        try {
            const cached = this._countries[culture];
            if (cached) {
                return cached;
            }
            const countries = await this._resourceService.getCountries(culture);
            this._countries[culture] = countries;
            return countries;
        } catch (error) {
            throw error;
        }
    }

    async getSsrs(culture: string = this._config.defaultCulture): Promise<Contracts.SSR[]> {
        try {
            const cached = this._ssrs[culture];
            if (cached) {
                return cached;
            }
            const ssrs = await this._resourceService.getSsrs(culture);
            this._ssrs[culture] = ssrs;
            return ssrs;
        } catch (error) {
            throw error;
        }
    }

    async getDocumentTypes(culture: string = this._config.defaultCulture): Promise<Contracts.DocType[]> {
        try {
            const cached = this._docTypes[culture];
            if (cached) {
                return cached;
            }
            const docTypes = await this._resourceService.getDocumentTypes(culture);
            this._docTypes[culture] = docTypes;
            return docTypes;
        } catch (error) {
            throw error;
        }
    }

    async getPaymentMethods(culture: string = this._config.defaultCulture): Promise<Contracts.PaymentMethod[]> {
        try {
            const cached = this._paymentMethods[culture];
            if (cached) {
                return cached;
            }
            const methods = await this._resourceService.getPaymentMethods(culture);
            this._paymentMethods[culture] = methods;
            return methods;
        } catch (error) {
            throw error;
        }
    }

    async getPaxTypeConfig(paxTypeCode: string, culture: string = this._config.defaultCulture): Promise<PaxTypeConfig> {
        const paxTypes = await this.getPaxTypes(culture);
        const paxType = paxTypes.find(t => t.code === paxTypeCode);
        if (!paxType) {
            throw <Contracts.NewskiesError>{ errorType: Contracts.ErrorType.BadData, message: `${paxTypeCode} is invalid passenger type` };
        }
        const paxTypeConfig: PaxTypeConfig = this._config.resourcesSettings.paxTypes.find(t => t.paxType === paxTypeCode);
        if (!paxTypeConfig) {
            throw <Contracts.NewskiesError>{ errorType: Contracts.ErrorType.ConfigurationError, message: `${paxTypeCode} is missing in passenger type configuration` };
        }
        return paxTypeConfig;
    }
}