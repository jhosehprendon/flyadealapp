import AjaxClient from "../Ajax/AjaxClient";
import {
    TripAvailabilityRequest, TripAvailabilityResponse, SellJourneyByKeyRequestData,
    SellResponse, LowFareTripAvailabilityRequest, LowFareTripAvailabilityResponse,
    Booking
} from "../Contracts/Contracts";
import Utils from "./Utils";

export default class FlightAvailabilityService {
    private _ajaxClient: AjaxClient;

    constructor(ajaxClient: AjaxClient) {
        this._ajaxClient = ajaxClient;
    }

    async findFlights(request: TripAvailabilityRequest): Promise<TripAvailabilityResponse> {
        try {
            const urlQuery = Utils.urlSerialize(request);
            const response = await this._ajaxClient.get('api/flights?' + urlQuery);
            const tripAvailabilityResponse = response.data as TripAvailabilityResponse;
            return tripAvailabilityResponse;
            //if (!tripAvailabilityResponse) {
            //    return null;
            //}
            //const trips = TripTranslator.translateTripAvailabilityResponse(tripAvailabilityResponse);
            //const emptyTrips = trips.filter(t => !t.arrivalStation || !t.departureStation);
            //emptyTrips.forEach(trip => {
            //    trips.indexOf(trip);
            //    const availRequest = request.availabilityRequests[trips.indexOf(trip)];
            //    trip.departureStation = availRequest.departureStation;
            //    trip.arrivalStation = availRequest.arrivalStation;
            //});
            //return trips;
        } catch (e) {
            throw e;
        }
    }

    async findFlightsLowFares(request: LowFareTripAvailabilityRequest): Promise<LowFareTripAvailabilityResponse> {
        try {
            const urlQuery = Utils.urlSerialize(request);
            const response = await this._ajaxClient.get('api/flights/lowfares?' + urlQuery);
            return response.data as LowFareTripAvailabilityResponse;
        } catch (e) {
            throw e;
        }
    }
}