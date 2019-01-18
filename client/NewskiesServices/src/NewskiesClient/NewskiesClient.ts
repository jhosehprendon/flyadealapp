import AjaxClient from '../Ajax/AjaxClient';
import ResourceService from '../Services/ResourceService';
import SessionService from '../Services/SessionService';
import FlightAvailabilityService from '../Services/FlightAvailabilityService';
import BookingService from '../Services/BookingService';
import CheckinService from '../Services/CheckinService';
import AgentService from '../Services/AgentService';
import AgencyService from '../Services/AgencyService';
import AccountService from '../Services/AccountService';
import * as Contracts from '../Contracts/Contracts';

export default class NewskiesClient {

    private readonly _ajaxClient: AjaxClient;
    private readonly _resourceService: ResourceService;
    private readonly _sessionService: SessionService;
    private readonly _flightAvailabilityService: FlightAvailabilityService;
    private readonly _bookingService: BookingService;
    private readonly _checkinService: CheckinService;
    private readonly _agentService: AgentService;
    private readonly _agencyService: AgencyService;
    private readonly _accountService: AccountService;

    constructor(config: Contracts.NewskiesClientConfig) {
        this._ajaxClient = new AjaxClient(config.ajax);
        this._resourceService = new ResourceService(this._ajaxClient);
        this._sessionService = new SessionService(this._ajaxClient);
        this._flightAvailabilityService = new FlightAvailabilityService(this._ajaxClient);
        this._bookingService = new BookingService(this._ajaxClient);
        this._checkinService = new CheckinService(this._ajaxClient);
        this._agentService = new AgentService(this._ajaxClient);
        this._agencyService = new AgencyService(this._ajaxClient);
        this._accountService = new AccountService(this._ajaxClient);
    }

    async registerRequestInterceptor(interceptor: Contracts.AjaxRequestInterceptor) {
        this._ajaxClient.registerRequestInterceptor(interceptor);
    }

    async registerResponseInterceptor(interceptor: Contracts.AjaxResponseInterceptor) {
        this._ajaxClient.registerResponseInterceptor(interceptor);
    }

    get resourceService() {
        return this._resourceService;
    }

    get sessionService() {
        return this._sessionService;
    }

    get flightAvailabilityService() {
        return this._flightAvailabilityService;
    }

    get bookingService() {
        return this._bookingService;
    }

    get checkinService() {
        return this._checkinService;
    }

    get agentService() {
        return this._agentService;
    }

    get agencyService() {
        return this._agencyService;
    }

    get accountService() {
        return this._accountService;
    }
}