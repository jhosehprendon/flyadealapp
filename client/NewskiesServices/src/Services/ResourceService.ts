import AjaxClient from '../Ajax/AjaxClient';
import { AxiosResponse } from 'axios';
import * as Contracts from '../Contracts/Contracts'; 

export default class ResourceService {

    private static COUNTRY_PHONE_CODES = [{ "countryCode": "AF", "phoneCode": "93" }, { "countryCode": "AL", "phoneCode": "355" }, { "countryCode": "DZ", "phoneCode": "213" }, { "countryCode": "DZ", "phoneCode": "213" }, { "countryCode": "AS", "phoneCode": "1-684" }, { "countryCode": "AD", "phoneCode": "376" }, { "countryCode": "AO", "phoneCode": "244" }, { "countryCode": "AI", "phoneCode": "1-264" }, { "countryCode": "AQ", "phoneCode": "672" }, { "countryCode": "AG", "phoneCode": "1-268" }, { "countryCode": "AR", "phoneCode": "54" }, { "countryCode": "AM", "phoneCode": "374" }, { "countryCode": "AW", "phoneCode": "297" }, { "countryCode": "AU", "phoneCode": "61" }, { "countryCode": "AT", "phoneCode": "43" }, { "countryCode": "AZ", "phoneCode": "994" }, { "countryCode": "BS", "phoneCode": "1-242" }, { "countryCode": "BH", "phoneCode": "973" }, { "countryCode": "BD", "phoneCode": "880" }, { "countryCode": "BB", "phoneCode": "1-246" }, { "countryCode": "BY", "phoneCode": "375" }, { "countryCode": "BE", "phoneCode": "32" }, { "countryCode": "BZ", "phoneCode": "501" }, { "countryCode": "BJ", "phoneCode": "229" }, { "countryCode": "BM", "phoneCode": "1-441" }, { "countryCode": "BT", "phoneCode": "975" }, { "countryCode": "BO", "phoneCode": "591" }, { "countryCode": "BQ", "phoneCode": "599" }, { "countryCode": "BA", "phoneCode": "387" }, { "countryCode": "BW", "phoneCode": "267" }, { "countryCode": "BR", "phoneCode": "55" }, { "countryCode": "VG", "phoneCode": "1 284" }, { "countryCode": "BN", "phoneCode": "673" }, { "countryCode": "BG", "phoneCode": "359" }, { "countryCode": "BF", "phoneCode": "226" }, { "countryCode": "BI", "phoneCode": "257" }, { "countryCode": "KH", "phoneCode": "855" }, { "countryCode": "CM", "phoneCode": "237" }, { "countryCode": "CA", "phoneCode": "1" }, { "countryCode": "CV", "phoneCode": "238" }, { "countryCode": "KY", "phoneCode": "1-345" }, { "countryCode": "CF", "phoneCode": "236" }, { "countryCode": "TD", "phoneCode": "235" }, { "countryCode": "CL", "phoneCode": "56" }, { "countryCode": "CN", "phoneCode": "86" }, { "countryCode": "CC", "phoneCode": "11" }, { "countryCode": "CO", "phoneCode": "57" }, { "countryCode": "KM", "phoneCode": "269" }, { "countryCode": "CG", "phoneCode": "242" }, { "countryCode": "CD", "phoneCode": "243" }, { "countryCode": "CK", "phoneCode": "682" }, { "countryCode": "CR", "phoneCode": "506" }, { "countryCode": "HR", "phoneCode": "385" }, { "countryCode": "CU", "phoneCode": "53" }, { "countryCode": "CY", "phoneCode": "357" }, { "countryCode": "CZ", "phoneCode": "420" }, { "countryCode": "DK", "phoneCode": "45" }, { "countryCode": "DJ", "phoneCode": "253" }, { "countryCode": "DM", "phoneCode": "1-767" }, { "countryCode": "DO", "phoneCode": "1-809" }, { "countryCode": "EC", "phoneCode": "593" }, { "countryCode": "EG", "phoneCode": "20" }, { "countryCode": "SV", "phoneCode": "503" }, { "countryCode": "GQ", "phoneCode": "240" }, { "countryCode": "ER", "phoneCode": "291" }, { "countryCode": "EE", "phoneCode": "372" }, { "countryCode": "ET", "phoneCode": "251" }, { "countryCode": "FK", "phoneCode": "500" }, { "countryCode": "FO", "phoneCode": "298" }, { "countryCode": "FJ", "phoneCode": "679" }, { "countryCode": "FI", "phoneCode": "358" }, { "countryCode": "FR", "phoneCode": "33" }, { "countryCode": "PF", "phoneCode": "689" }, { "countryCode": "GF", "phoneCode": "594" }, { "countryCode": "GA", "phoneCode": "241" }, { "countryCode": "GM", "phoneCode": "220" }, { "countryCode": "GE", "phoneCode": "995" }, { "countryCode": "DE", "phoneCode": "49" }, { "countryCode": "GH", "phoneCode": "233" }, { "countryCode": "GI", "phoneCode": "350" }, { "countryCode": "GR", "phoneCode": "30" }, { "countryCode": "GL", "phoneCode": "299" }, { "countryCode": "GD", "phoneCode": "1-473" }, { "countryCode": "GP", "phoneCode": "590" }, { "countryCode": "GU", "phoneCode": "1-671" }, { "countryCode": "GT", "phoneCode": "502" }, { "countryCode": "GN", "phoneCode": "224" }, { "countryCode": "GW", "phoneCode": "245" }, { "countryCode": "GY", "phoneCode": "592" }, { "countryCode": "HT", "phoneCode": "509" }, { "countryCode": "HN", "phoneCode": "504" }, { "countryCode": "HK", "phoneCode": "852" }, { "countryCode": "HU", "phoneCode": "36" }, { "countryCode": "IS", "phoneCode": "354" }, { "countryCode": "IN", "phoneCode": "91" }, { "countryCode": "ID", "phoneCode": "62" }, { "countryCode": "IR", "phoneCode": "98" }, { "countryCode": "IQ", "phoneCode": "964" }, { "countryCode": "IE", "phoneCode": "353" }, { "countryCode": "IL", "phoneCode": "972" }, { "countryCode": "IT", "phoneCode": "39" }, { "countryCode": "CI", "phoneCode": "225" }, { "countryCode": "JM", "phoneCode": "1-876" }, { "countryCode": "JP", "phoneCode": "81" }, { "countryCode": "JO", "phoneCode": "962" }, { "countryCode": "KZ", "phoneCode": "7" }, { "countryCode": "KE", "phoneCode": "254" }, { "countryCode": "KI", "phoneCode": "686" }, { "countryCode": "KW", "phoneCode": "965" }, { "countryCode": "KG", "phoneCode": "996" }, { "countryCode": "LA", "phoneCode": "856" }, { "countryCode": "LV", "phoneCode": "371" }, { "countryCode": "LB", "phoneCode": "961" }, { "countryCode": "LS", "phoneCode": "266" }, { "countryCode": "LR", "phoneCode": "231" }, { "countryCode": "LY", "phoneCode": "218" }, { "countryCode": "LI", "phoneCode": "423" }, { "countryCode": "LT", "phoneCode": "370" }, { "countryCode": "LU", "phoneCode": "352" }, { "countryCode": "MO", "phoneCode": "853" }, { "countryCode": "MK", "phoneCode": "389" }, { "countryCode": "MG", "phoneCode": "261" }, { "countryCode": "MW", "phoneCode": "265" }, { "countryCode": "MY", "phoneCode": "60" }, { "countryCode": "MV", "phoneCode": "960" }, { "countryCode": "ML", "phoneCode": "223" }, { "countryCode": "MT", "phoneCode": "356" }, { "countryCode": "MH", "phoneCode": "692" }, { "countryCode": "MQ", "phoneCode": "596" }, { "countryCode": "MR", "phoneCode": "222" }, { "countryCode": "MU", "phoneCode": "230" }, { "countryCode": "YT", "phoneCode": "269" }, { "countryCode": "MX", "phoneCode": "52" }, { "countryCode": "FM", "phoneCode": "691" }, { "countryCode": "MD", "phoneCode": "373" }, { "countryCode": "MC", "phoneCode": "377" }, { "countryCode": "MN", "phoneCode": "976" }, { "countryCode": "ME", "phoneCode": "382" }, { "countryCode": "MS", "phoneCode": "1-664" }, { "countryCode": "MA", "phoneCode": "212" }, { "countryCode": "MZ", "phoneCode": "258" }, { "countryCode": "MM", "phoneCode": "95" }, { "countryCode": "NA", "phoneCode": "264" }, { "countryCode": "NR", "phoneCode": "674" }, { "countryCode": "NP", "phoneCode": "977" }, { "countryCode": "NL", "phoneCode": "31" }, { "countryCode": "AN", "phoneCode": "599" }, { "countryCode": "NC", "phoneCode": "687" }, { "countryCode": "NZ", "phoneCode": "64" }, { "countryCode": "NI", "phoneCode": "505" }, { "countryCode": "NE", "phoneCode": "227" }, { "countryCode": "NG", "phoneCode": "234" }, { "countryCode": "NU", "phoneCode": "683" }, { "countryCode": "NF", "phoneCode": "672" }, { "countryCode": "MP", "phoneCode": "1-670" }, { "countryCode": "KP", "phoneCode": "850" }, { "countryCode": "NO", "phoneCode": "47" }, { "countryCode": "OM", "phoneCode": "968" }, { "countryCode": "PK", "phoneCode": "92" }, { "countryCode": "PW", "phoneCode": "680" }, { "countryCode": "PS", "phoneCode": "970" }, { "countryCode": "PA", "phoneCode": "507" }, { "countryCode": "PG", "phoneCode": "675" }, { "countryCode": "PY", "phoneCode": "595" }, { "countryCode": "PE", "phoneCode": "51" }, { "countryCode": "PH", "phoneCode": "63" }, { "countryCode": "PN", "phoneCode": "870" }, { "countryCode": "PL", "phoneCode": "48" }, { "countryCode": "PT", "phoneCode": "351" }, { "countryCode": "PR", "phoneCode": "1" }, { "countryCode": "QA", "phoneCode": "974" }, { "countryCode": "RE", "phoneCode": "262" }, { "countryCode": "RO", "phoneCode": "40" }, { "countryCode": "RU", "phoneCode": "7" }, { "countryCode": "RW", "phoneCode": "250" }, { "countryCode": "BL", "phoneCode": "590" }, { "countryCode": "WS", "phoneCode": "685" }, { "countryCode": "SM", "phoneCode": "378" }, { "countryCode": "ST", "phoneCode": "239" }, { "countryCode": "SA", "phoneCode": "966" }, { "countryCode": "SN", "phoneCode": "221" }, { "countryCode": "RS", "phoneCode": "381" }, { "countryCode": "SC", "phoneCode": "248" }, { "countryCode": "SL", "phoneCode": "232" }, { "countryCode": "SG", "phoneCode": "65" }, { "countryCode": "SX", "phoneCode": "599" }, { "countryCode": "SK", "phoneCode": "421" }, { "countryCode": "SI", "phoneCode": "386" }, { "countryCode": "SB", "phoneCode": "677" }, { "countryCode": "SO", "phoneCode": "252" }, { "countryCode": "ZA", "phoneCode": "27" }, { "countryCode": "KR", "phoneCode": "82" }, { "countryCode": "ES", "phoneCode": "34" }, { "countryCode": "LK", "phoneCode": "94" }, { "countryCode": "SH", "phoneCode": "290" }, { "countryCode": "KN", "phoneCode": "1-869" }, { "countryCode": "LC", "phoneCode": "1-758" }, { "countryCode": "MF", "phoneCode": "1 599" }, { "countryCode": "PM", "phoneCode": "508" }, { "countryCode": "VC", "phoneCode": "1-784" }, { "countryCode": "SD", "phoneCode": "249" }, { "countryCode": "SR", "phoneCode": "597" }, { "countryCode": "SZ", "phoneCode": "268" }, { "countryCode": "SE", "phoneCode": "46" }, { "countryCode": "CH", "phoneCode": "41" }, { "countryCode": "SY", "phoneCode": "963" }, { "countryCode": "TW", "phoneCode": "886" }, { "countryCode": "TJ", "phoneCode": "992" }, { "countryCode": "TZ", "phoneCode": "255" }, { "countryCode": "TH", "phoneCode": "66" }, { "countryCode": "TL", "phoneCode": "670" }, { "countryCode": "TG", "phoneCode": "228" }, { "countryCode": "TK", "phoneCode": "690" }, { "countryCode": "TO", "phoneCode": "676" }, { "countryCode": "TT", "phoneCode": "1-868" }, { "countryCode": "TN", "phoneCode": "216" }, { "countryCode": "TR", "phoneCode": "90" }, { "countryCode": "TM", "phoneCode": "993" }, { "countryCode": "TC", "phoneCode": "1-649" }, { "countryCode": "TV", "phoneCode": "688" }, { "countryCode": "AE", "phoneCode": "971" }, { "countryCode": "UG", "phoneCode": "256" }, { "countryCode": "GB", "phoneCode": "44" }, { "countryCode": "UA", "phoneCode": "380" }, { "countryCode": "UY", "phoneCode": "598" }, { "countryCode": "US", "phoneCode": "1" }, { "countryCode": "UZ", "phoneCode": "998" }, { "countryCode": "VU", "phoneCode": "678" }, { "countryCode": "VA", "phoneCode": "379" }, { "countryCode": "VE", "phoneCode": "58" }, { "countryCode": "VN", "phoneCode": "84" }, { "countryCode": "VI", "phoneCode": "1-340" }, { "countryCode": "WF", "phoneCode": "681" }, { "countryCode": "YE", "phoneCode": "967" }, { "countryCode": "ZM", "phoneCode": "260" }, { "countryCode": "ZW", "phoneCode": "263" }, { "countryCode": "VG", "phoneCode": "1-284" }, { "countryCode": "TL", "phoneCode": "670" }];

    private readonly _ajaxClient: AjaxClient;

    constructor(ajax: AjaxClient) {
        this._ajaxClient = ajax;
    }

    async getAllResources(culture: string): Promise<Contracts.AllResourcesResponse> {
        try {
            const axiosResponse: AxiosResponse = await this._ajaxClient.get(`api/resources?culture=${culture}`);
            const response: Contracts.AllResourcesResponse = (axiosResponse ? axiosResponse.data : undefined);
            if (response && response.countryList) {
                ResourceService.populateCountryPhoneCodes(response.countryList);
            }
            return response;
        } catch (e) {
            throw e;
        }
    }

    async getMarkets(): Promise<Contracts.Market[]> {
        try {
            const response: AxiosResponse = await this._ajaxClient.get('api/resources/markets');
            return response && response.data ? response.data.marketList : undefined;
        } catch (e) {
            throw e;
        }
    }

    async getStations(culture: string): Promise<Contracts.Station[]> {
        try {
            const response: AxiosResponse = await this._ajaxClient.get(`api/resources/stations?culture=${culture}`);
            return response && response.data ? response.data.stationList : undefined;
        } catch (e) {
            throw e;
        }
    }

    async getFees(culture: string): Promise<Contracts.Fee[]> {
        try {
            const response: AxiosResponse = await this._ajaxClient.get(`api/resources/fees?culture=${culture}`);
            return response && response.data ? response.data.feeList : undefined;
        } catch (e) {
            throw e;
        }
    }

    async getSsrs(culture: string): Promise<Contracts.SSR[]> {
        try {
            const response: AxiosResponse = await this._ajaxClient.get(`api/resources/ssrs?culture=${culture}`);
            return response && response.data ? response.data.ssrList : undefined;
        } catch (e) {
            throw e;
        }
    }

    async getCultures(): Promise<Contracts.Culture[]> {
        try {
            const response: AxiosResponse = await this._ajaxClient.get(`api/resources/cultures`);
            return response && response.data ? response.data.cultureList : undefined;
        } catch (e) {
            throw e;
        }
    }

    async getCurrencies(): Promise<Contracts.Currency[]> {
        try {
            const response: AxiosResponse = await this._ajaxClient.get(`api/resources/currencies`);
            return response && response.data ? response.data.currencyList : undefined;
        } catch (e) {
            throw e;
        }
    }

    async getPaxTypes(culture: string): Promise<Contracts.PaxType[]> {
        try {
            const response: AxiosResponse = await this._ajaxClient.get(`api/resources/paxtypes?culture=${culture}`);
            return response && response.data ? response.data.paxTypeList : undefined;
        } catch (e) {
            throw e;
        }
    }

    async getCountries(culture: string): Promise<Contracts.Country[]> {
        try {
            const response: AxiosResponse = await this._ajaxClient.get(`api/resources/countries?culture=${culture}`);
            const countries = response && response.data ? response.data.countryList : undefined;
            ResourceService.populateCountryPhoneCodes(countries);
            return countries;
        } catch (e) {
            throw e;
        }
    }

    async getTitles(culture: string): Promise<Contracts.Title[]> {
        try {
            const response: AxiosResponse = await this._ajaxClient.get(`api/resources/titles?culture=${culture}`);
            return response && response.data ? response.data.titleList : undefined;
        } catch (e) {
            throw e;
        }
    }

    async getDocumentTypes(culture: string): Promise<Contracts.DocType[]> {
        try {
            const response: AxiosResponse = await this._ajaxClient.get(`api/resources/documentTypes?culture=${culture}`);
            return response && response.data ? response.data.docTypeList : undefined;
        } catch (e) {
            throw e;
        }
    }

    async getPaymentMethods(culture: string): Promise<Contracts.PaymentMethod[]> {
        try {
            const response: AxiosResponse = await this._ajaxClient.get(`api/resources/paymentMethodsTypes?culture=${culture}`);
            return response && response.data ? response.data.paymentMethodList : undefined;
        } catch (e) {
            throw e;
        }
    }

    private static populateCountryPhoneCodes(countries: Contracts.Country[]) {
        if (countries) {
            countries.forEach((country: Contracts.Country) => {
                const phone = ResourceService.COUNTRY_PHONE_CODES.find(ph => ph.countryCode === country.countryCode);
                phone && (country.phoneCode = phone.phoneCode);
            });
        }
    }
}