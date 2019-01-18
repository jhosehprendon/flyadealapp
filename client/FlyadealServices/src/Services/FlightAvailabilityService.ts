import { Contracts as NskContracts, FlightAvailabilityService as NskFlightAvailabilityService } from 'newskies-services';
import { TripAvailabilityRequest, TripAvailabilityResponse, PriceItinerary, Trip } from '../Contracts';
import Utils from './Utils';
import SessionBag from "./SessionBag";
import ResourceCache from "./ResourceCache";
import TripTranslator from "../Mapping/TripTranslator";

export default class FlightAvailabilityService {

    private _nskService: NskFlightAvailabilityService;
    private _sessionBag: SessionBag;
    private _resourceCache: ResourceCache;

    constructor(nskService: NskFlightAvailabilityService, sessionBag: SessionBag, resourceCache: ResourceCache) {
        this._nskService = nskService;
        this._sessionBag = sessionBag;
        this._resourceCache = resourceCache;
    }

    async findFlights(request: TripAvailabilityRequest): Promise<TripAvailabilityResponse> {
        const paxTypeCounts: NskContracts.PaxTypeCount[] = Utils.createPaxTypeCounts(request.adults, request.children, request.infants);
        const origin = request.flights.length ? request.flights[0].departureStation : null;
        const currencyCode = await this._resourceCache.getStationCurrency(origin);
        const nskAvailRequests = request.flights.map(f => {
            const nskAvailReq: NskContracts.AvailabilityRequest = {
                departureStation: f.departureStation,
                arrivalStation: f.arrivalStation,
                beginDate: f.beginDate,
                endDate: f.endDate,
                paxTypeCounts: paxTypeCounts,
                currencyCode,// request.currencyCode,
                paxResidentCountry: request.paxResidentCountry,
                promotionCode: f.promotionCode
            }
            return nskAvailReq;
        });
        const nskRequest: NskContracts.TripAvailabilityRequest = {
            availabilityRequests: nskAvailRequests
        };
        try {
            const response = TripTranslator.translateTripAvailabilityResponse(await this._nskService.findFlights(nskRequest));
            response.currencyCode = currencyCode;
            // populate unavailable flight dates
            response.trips.forEach((trip, index) => {
                trip.departureStation = trip.departureStation || request.flights[index].departureStation;
                trip.arrivalStation = trip.arrivalStation || request.flights[index].arrivalStation;
                if (!trip.flightDates.length) {
                    trip.flightDates.push({
                        flightCount: 0,
                        departureDate: request.flights[index].beginDate,
                        flights: []
                    });
                }
            });
            this._sessionBag.journeySellKeys = [];
            this._sessionBag.flightSearchRequest = request;
            this._sessionBag.availabilityResponse = response;
            this._sessionBag.priceItinerary = undefined;
            return response;
        } catch (error) {
            throw error;
        }
    }

    async findFlightsLowFares(request: TripAvailabilityRequest): Promise<NskContracts.LowFareTripAvailabilityResponse> {
        const paxTypeCounts: NskContracts.PaxTypeCount[] = Utils.createPaxTypeCounts(request.adults, request.children, request.infants);
        const origin = request.flights.length ? request.flights[0].departureStation : null;
        const currencyCode = await this._resourceCache.getStationCurrency(origin);
        const nskAvailRequests = request.flights.map(f => {
            const nskAvailReq: NskContracts.LowFareAvailabilityRequest = {
                departureStation: f.departureStation,
                arrivalStation: f.arrivalStation,
                beginDate: f.beginDate,
                endDate: f.endDate
            }
            return nskAvailReq;
        });
        const nskRequest: NskContracts.LowFareTripAvailabilityRequest = {
            currencyCode: currencyCode,
            paxResidentCountry: request.paxResidentCountry,
            paxTypeCounts: paxTypeCounts,
            lowFareAvailabilityRequestList: nskAvailRequests,
            promotionCode: request.flights[0].promotionCode
        };
        try {
            const nskResponse = await this._nskService.findFlightsLowFares(nskRequest);
            this._sessionBag.availabilityLowFareTrips = nskResponse;
            return nskResponse;
        } catch (error) {
            throw error;
        }
    }

    get flightSearchRequest(): TripAvailabilityRequest {
        return this._sessionBag.flightSearchRequest;
    }

    set flightSearchRequest(value: TripAvailabilityRequest) {
        this._sessionBag.flightSearchRequest = value;
    }

    get availabilityResponse(): TripAvailabilityResponse {
        return this._sessionBag.availabilityResponse;
    }

    set availabilityResponse(value: TripAvailabilityResponse) {
        this._sessionBag.availabilityResponse = value;
    }

    get availabilityLowFareTrips(): NskContracts.LowFareTripAvailabilityResponse {
        return this._sessionBag.availabilityLowFareTrips;
    }

    set availabilityLowFareTrips(value: NskContracts.LowFareTripAvailabilityResponse) {
        this._sessionBag.availabilityLowFareTrips = value;
    }
}