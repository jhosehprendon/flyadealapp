import { NewskiesClient, Contracts, SessionTokenHeaderInterceptor, ResourceService, SessionTokenStorage } from 'newskies-services';
import AjaxClientConfig = Contracts.AjaxClientConfig;
import SessionStorage = Contracts.SessionStorage;
import NewskiesClientConfig = Contracts.NewskiesClientConfig;

import Validators from './Validation/Validators';
import ResourceCache from './Services/ResourceCache';
import { FlyadealClientConfig } from "./Contracts";
import CultureService from "./Services/CultureService";
import FlightAvailabilityService from "./Services/FlightAvailabilityService";
import BookingService from "./Services/BookingService";
import SessionBag from "./Services/SessionBag";
import ErrorService from "./Services/ErrorService";
import Utils from './Services/Utils';
import OperationService from "./Services/OperationService";
import MemberService from "./Services/MemberService";
import AgentService from './Services/AgentService';
import AgencyService from './Services/AgencyService';
import AccountService from './Services/AccountService';
import SessionService from './Services/SessionService';

export default class FlyadealClient {

    static createClient(config: FlyadealClientConfig): FlyadealClient {
        return new FlyadealClient(config);
    }

    private _nskClient: NewskiesClient;
    private _config: FlyadealClientConfig;
    private _sessionService: SessionService;
    private _sessionTokenStorage: SessionTokenStorage;
    private _resourceCache: ResourceCache;
    private _cultureService: CultureService;
    private _flightAvailabilityService: FlightAvailabilityService;
    private _bookingService: BookingService;
    private _operationService: OperationService;
    private _memberService: MemberService;
    private _agentService: AgentService;
    private _agencyService: AgencyService;
    private _accountService: AccountService;
    // private _errorService: ErrorService;

    private constructor(config: FlyadealClientConfig) {
        if (!config || !config.sessionStorage || !config.sessionTokenHeader) {
            throw new Error(`Invalid FlyadealClient config`);
        }
        this._config = config;
        this._sessionTokenStorage = new SessionTokenStorage(config.sessionStorage);
        const interceptor = new SessionTokenHeaderInterceptor(this._sessionTokenStorage, config.sessionTokenHeader);
        const nskClientConfig: NewskiesClientConfig = {
            ajax: {
                baseUrl: config.baseUrl,
                requestInterceptors: [interceptor],
                responseInterceptors: [interceptor]
            }
        };
        const sessionBag = new SessionBag(config.sessionStorage);
        this._nskClient = new NewskiesClient(nskClientConfig);
        this._sessionService = new SessionService(this._nskClient.sessionService, sessionBag);
        this._resourceCache = new ResourceCache(this._nskClient.resourceService, config);
        this._cultureService = new CultureService(config.localStorage, this._resourceCache, config.defaultCulture);
        this._flightAvailabilityService = new FlightAvailabilityService(this._nskClient.flightAvailabilityService, sessionBag, this._resourceCache);
        this._bookingService = new BookingService(this._nskClient.bookingService, sessionBag, this._resourceCache);
        this._operationService = new OperationService(sessionBag, this._bookingService, this._nskClient.checkinService);
        this._memberService = new MemberService(sessionBag, this._sessionService, this._nskClient.agentService, this._resourceCache);
        this._agentService = new AgentService(sessionBag, this._sessionService, this._nskClient.agentService, this._resourceCache);
        this._agencyService = new AgencyService(sessionBag, this._sessionService, this._nskClient.agencyService, this._resourceCache);
        this._accountService = new AccountService(this._nskClient.accountService);
    }

    async registerRequestInterceptor(interceptor: Contracts.AjaxRequestInterceptor) {
        await this._nskClient.registerRequestInterceptor(interceptor);
    }

    async registerResponseInterceptor(interceptor: Contracts.AjaxResponseInterceptor) {
        await this._nskClient.registerResponseInterceptor(interceptor);
    }

    get validators(): any {
        return Validators;
    }

    get utils() {
        return Utils;
    }

    get sessionTokenStorage(): SessionTokenStorage {
        return this._sessionTokenStorage;
    }

    get errorService(): ErrorService {
        return ErrorService;
    }

    get sessionService(): SessionService {
        return this._sessionService;
    }

    get resourceCache(): ResourceCache {
        return this._resourceCache;
    }

    get cultureService(): CultureService {
        return this._cultureService;
    }

    get flightAvailabilityService(): FlightAvailabilityService {
        return this._flightAvailabilityService;
    }

    get bookingService(): BookingService {
        return this._bookingService;
    }

    get operationService(): OperationService {
        return this._operationService;
    }

    get memberService(): MemberService {
        return this._memberService;
    }

    get agentService(): AgentService {
        return this._agentService;
    }

    get agencyService(): AgencyService {
        return this._agencyService;
    }

    get accountService(): AccountService {
        return this._accountService;
    }
}