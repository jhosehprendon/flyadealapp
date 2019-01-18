import { Contracts as NskContracts, BookingService as NskBookingService } from 'newskies-services';
import { TripAvailabilityRequest, PriceItinerary, Trip, TripAvailabilityResponse, CheckInSelection  } from "../Contracts";

export default class SessionBag {
    
    private _sessionStorage: NskContracts.SessionStorage;

    constructor(sessionStorage: NskContracts.SessionStorage) {
        this._sessionStorage = sessionStorage;
    }

    get journeySellKeys(): NskContracts.SellKeyList[] {
        const strResult = this._sessionStorage.getItem('journeySellKeys');
        if (!strResult) {
            return undefined;
        }
        try {
            return JSON.parse(strResult) as NskContracts.SellKeyList[];
        } catch (e) {
            this._sessionStorage.removeItem('journeySellKeys');
        }
        return undefined;
    }

    set journeySellKeys(value: NskContracts.SellKeyList[]) {
        if (value === undefined) {
            this._sessionStorage.removeItem('journeySellKeys');
            return;
        }
        this._sessionStorage.setItem('journeySellKeys', JSON.stringify(value));
    }

    get flightSearchRequest(): TripAvailabilityRequest {
        const strResult = this._sessionStorage.getItem('flightSearchRequest');
        if (!strResult) {
            return undefined;
        }
        try {
            return JSON.parse(strResult) as TripAvailabilityRequest;
        } catch (e) {
            this._sessionStorage.removeItem('flightSearchRequest');
        }
        return undefined;
    }

    set flightSearchRequest(value: TripAvailabilityRequest) {
        if (value === undefined) {
            this._sessionStorage.removeItem('flightSearchRequest');
            return;
        }
        this._sessionStorage.setItem('flightSearchRequest', JSON.stringify(value));
    }

    get availabilityResponse(): TripAvailabilityResponse {
        const strResult = this._sessionStorage.getItem('availabilityResponse');
        if (!strResult) {
            return undefined;
        }
        try {
            return JSON.parse(strResult) as TripAvailabilityResponse;
        } catch (e) {
            this._sessionStorage.removeItem('availabilityResponse');
        }
        return undefined;
    }

    set availabilityResponse(value: TripAvailabilityResponse) {
        if (value === undefined) {
            this._sessionStorage.removeItem('availabilityResponse');
            return;
        }
        this._sessionStorage.setItem('availabilityResponse', JSON.stringify(value));
    }

    get availabilityLowFareTrips(): NskContracts.LowFareTripAvailabilityResponse {
        const strResult = this._sessionStorage.getItem('availabilityLowFareTrips');
        if (!strResult) {
            return undefined;
        }
        try {
            return JSON.parse(strResult) as NskContracts.LowFareTripAvailabilityResponse;
        } catch (e) {
            this._sessionStorage.removeItem('availabilityLowFareTrips');
        }
        return undefined;
    }

    set availabilityLowFareTrips(value: NskContracts.LowFareTripAvailabilityResponse) {
        if (value === undefined) {
            this._sessionStorage.removeItem('availabilityLowFareTrips');
            return;
        }
        this._sessionStorage.setItem('availabilityLowFareTrips', JSON.stringify(value));
    }

    get priceItinerary(): PriceItinerary {
        const strResult = this._sessionStorage.getItem('priceItinerary');
        if (!strResult) {
            return undefined;
        }
        try {
            return JSON.parse(strResult) as PriceItinerary;
        } catch (e) {
            this._sessionStorage.removeItem('priceItinerary');
        }
        return undefined;
    }

    set priceItinerary(value: PriceItinerary) {
        if (value === undefined) {
            this._sessionStorage.removeItem('priceItinerary');
            return;
        }
        this._sessionStorage.setItem('priceItinerary', JSON.stringify(value));
    }

    get seatsNewlyAutoAssigned(): boolean {
        const strResult = this._sessionStorage.getItem('seatsNewlyAutoAssigned');
        return !strResult || strResult === 'false' ? false : true;
    }

    set seatsNewlyAutoAssigned(value: boolean) {
        if (value) {
            this._sessionStorage.setItem('seatsNewlyAutoAssigned', JSON.stringify(value));
        } else {
            this._sessionStorage.removeItem('seatsNewlyAutoAssigned');
        }
    }

    get checkInSelection(): CheckInSelection {
        const strResult = this._sessionStorage.getItem('checkInSelection');
        if (!strResult) {
            return undefined;
        }
        try {
            return JSON.parse(strResult) as CheckInSelection;
        } catch (e) {
            this._sessionStorage.removeItem('checkInSelection');
        }
        return undefined;
    }

    set checkInSelection(value: CheckInSelection) {
        if (value === undefined) {
            this._sessionStorage.removeItem('checkInSelection');
            return;
        }
        this._sessionStorage.setItem('checkInSelection', JSON.stringify(value));
    }

    get isManage(): boolean {
        const strResult = this._sessionStorage.getItem('isManage');
        if (!strResult) {
            return false;
        }
        return true;
    }

    set isManage(value: boolean) {
        if (value) {
            this._sessionStorage.setItem('isManage', 'Y');
            return;
        }
        this._sessionStorage.removeItem('isManage');
    }

    get manageJourneys(): number[] {
        const strResult = this._sessionStorage.getItem('manageJourneys');
        if (!strResult) {
            return undefined;
        }
        try {
            return JSON.parse(strResult) as number[];
        } catch (e) {
            this._sessionStorage.removeItem('manageJourneys');
        }
        return undefined;
    }

    set manageJourneys(value: number[]) {
        if (value === undefined) {
            this._sessionStorage.removeItem('manageJourneys');
            return;
        }
        this._sessionStorage.setItem('manageJourneys', JSON.stringify(value));
    }

    get boardingPassJourney(): number {
        const strResult = this._sessionStorage.getItem('boardingPassJourney');
        if (!strResult) {
            return undefined;
        }
        try {
            return JSON.parse(strResult) as number;
        } catch (e) {
            this._sessionStorage.removeItem('boardingPassJourney');
        }
    }

    set boardingPassJourney(value: number) {
        if (value === undefined) {
            this._sessionStorage.removeItem('boardingPassJourney');
            return;
        }
        this._sessionStorage.setItem('boardingPassJourney', JSON.stringify(value));
    }

    get boardingPassPassenger(): number {
        const strResult = this._sessionStorage.getItem('boardingPassPassenger');
        if (!strResult) {
            return undefined;
        }
        try {
            return JSON.parse(strResult) as number;
        } catch (e) {
            this._sessionStorage.removeItem('boardingPassPassenger');
        }
    }

    set boardingPassPassenger(value: number) {
        if (value === undefined) {
            this._sessionStorage.removeItem('boardingPassPassenger');
            return;
        }
        this._sessionStorage.setItem('boardingPassPassenger', JSON.stringify(value));
    }

    get userSessionInfo(): NskContracts.SessionInfo {
        const strResult = this._sessionStorage.getItem('userSessionInfo');
        if (!strResult) {
            return undefined;
        }
        try {
            return JSON.parse(strResult) as NskContracts.SessionInfo;
        } catch (e) {
            this._sessionStorage.removeItem('userSessionInfo');
        }
    }

    set userSessionInfo(info: NskContracts.SessionInfo) {
        if (!info) {
            this._sessionStorage.removeItem('userSessionInfo');
            return;
        }
        this._sessionStorage.setItem('userSessionInfo', JSON.stringify(info));
    }

    get agentData(): NskContracts.CommitAgentResponseData {
        const strResult = this._sessionStorage.getItem('sessionAgentData');
        if (!strResult) {
            return undefined;
        }
        try {
            return JSON.parse(strResult) as NskContracts.CommitAgentResponseData;
        } catch (e) {
            this._sessionStorage.removeItem('sessionAgentData');
        }
    }

    set agentData(info: NskContracts.CommitAgentResponseData) {
        if (!info) {
            this._sessionStorage.removeItem('sessionAgentData');
            return;
        }
        this._sessionStorage.setItem('sessionAgentData', JSON.stringify(info));
    }

    get promotionCode(): string {
        const strResult = this._sessionStorage.getItem('promotionCode');
        if (!strResult) {
            return undefined;
        }
        try {
            return JSON.parse(strResult);
        } catch (e) {
            this._sessionStorage.removeItem('promotionCode');
        }
    }

    set promotionCode(value: string) {
        if (!value) {
            this._sessionStorage.removeItem('promotionCode');
            return;
        }
        this._sessionStorage.setItem('promotionCode', JSON.stringify(value));
    }

    get editAgentId(): string {
        const strResult = this._sessionStorage.getItem('editAgentId');
        if (!strResult) {
            return undefined;
        }
        try {
            return JSON.parse(strResult);
        } catch (e) {
            this._sessionStorage.removeItem('editAgentId');
        }
    }

    set editAgentId(value: string) {
        if (!value) {
            this._sessionStorage.removeItem('editAgentId');
            return;
        }
        this._sessionStorage.setItem('editAgentId', JSON.stringify(value));
    }

    get mustChangePasswordUsername(): string {
        const strResult = this._sessionStorage.getItem('mustChangePasswordUsername');
        if (!strResult) {
            return undefined;
        }
        try {
            return JSON.parse(strResult);
        } catch (e) {
            this._sessionStorage.removeItem('mustChangePasswordUsername');
        }
    }

    set mustChangePasswordUsername(value: string) {
        if (!value) {
            this._sessionStorage.removeItem('mustChangePasswordUsername');
            return;
        }
        this._sessionStorage.setItem('mustChangePasswordUsername', JSON.stringify(value));
    }

    get agentListData(): NskContracts.FindAgentResponseData {
        const strResult = this._sessionStorage.getItem('sessionAgentListData');
        if (!strResult) {
            return undefined;
        }
        try {
            return JSON.parse(strResult) as NskContracts.FindAgentResponseData;
        } catch (e) {
            this._sessionStorage.removeItem('sessionAgentListData');
        }
    }

    set agentListData(data: NskContracts.FindAgentResponseData) {
        if (!data) {
            this._sessionStorage.removeItem('sessionAgentListData');
            return;
        }
        this._sessionStorage.setItem('sessionAgentListData', JSON.stringify(data));
    }

    get currentRefreshCount(): Number {
        const currentCounter = this._sessionStorage.getItem("_confirmationRefreshCount");
        if (!currentCounter) {
            return 0;
        }
        try {
            return JSON.parse(currentCounter) as number;
        } catch (e) {
            this._sessionStorage.removeItem("_confirmationRefreshCount");
            return 0;
        }
    }

    set currentRefreshCount(value: Number) {
        if (value === undefined || value === null) {
            this._sessionStorage.removeItem("_confirmationRefreshCount");
            return;
        }
        this._sessionStorage.setItem("_confirmationRefreshCount", JSON.stringify(value));
    }

    get triggerPurchaseEvent(): boolean {
        const purchaseEvent = this._sessionStorage.getItem("_triggerPurchaseEvent");
        return !(!purchaseEvent || purchaseEvent === "false");
    }

    set triggerPurchaseEvent(value: boolean) {
        if (value) {
            this._sessionStorage.setItem("_triggerPurchaseEvent", JSON.stringify(value));
            return;
        }
        this._sessionStorage.removeItem("_triggerPurchaseEvent");
    }




}