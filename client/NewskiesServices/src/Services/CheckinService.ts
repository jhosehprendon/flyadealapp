import AjaxClient from "../Ajax/AjaxClient";
import {
    CheckInPassengersRequestData, CheckInMultiplePassengerRequest,
    CheckInPassengersResponse, CheckInPassengersResponseData, CheckInMultiplePassengerResponse, CheckInPaxResponse//, CheckInError
} from "../Contracts/Contracts";
import Utils from "./Utils";

export default class CheckinService {
    private _ajaxClient: AjaxClient;

    constructor(ajaxClient: AjaxClient) {
        this._ajaxClient = ajaxClient;
    }

    async checkinPassengers(requestData: CheckInPassengersRequestData): Promise<CheckInPassengersResponseData> {
        try {
            const response = await this._ajaxClient.post('api/checkin', requestData);
            const checkinPaxesResponseData = response.data as CheckInPassengersResponseData;
            return checkinPaxesResponseData;
        } catch (e) {
            throw e;
        }
    }   
}