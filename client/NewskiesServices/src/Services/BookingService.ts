import AjaxClient from "../Ajax/AjaxClient";
import {
    SellJourneyByKeyRequestData, ChangeFlightsRequest, SellResponse,
    Passenger, GetPassengersResponse, UpdatePassengersRequestData,
    BookingUpdateResponseData, ErrorType, GetBookingContactsResponse,
    BookingContact, Booking, BookingState, UpdateContactsRequestData, UpdateContactsResponse,
    PriceItineraryResponse, GetSSRAvailabilityForBookingResponse, SSRAvailabilityForBookingResponse,
    SeatAvailabilityRequestData, SeatAvailabilityRequest, GetSeatAvailabilityResponse,
    SeatAvailabilityResponse, AssignSeatRequest, AssignSeatsResponse, SellSSRRequest, SSRRequestData,
    CancelSSRRequest, CancelResponse, AddPaymentToBookingRequestData, AddPaymentToBookingResponse,
    RemovePaymentFromBookingRequest, RemovePaymentFromBookingResponse,
    CommitRequest, CommitResponse, GetPostCommitResultsResponse, RetrieveBookingRequest, RetrieveBookingResponse,
    GetBarCodedBoardingPassesRequest, GetBarCodedBoardingPassesResponse, ApplyPromotionRequestData,
    ApplyPromotionResponse, SellFeeRequestData, CancelFeeRequestData

} from "../Contracts/Contracts";
import Utils from "./Utils";

export default class BookingService {
    private _ajaxClient: AjaxClient;

    constructor(ajaxClient: AjaxClient) {
        this._ajaxClient = ajaxClient;
    }

    async clearStateBooking() {
        try {
            await this._ajaxClient.post('api/booking/clearstate');
        } catch (e) {
            throw e;
        }
    }

    async get(): Promise<Booking> {
        try {
            const response = await this._ajaxClient.get('api/booking');
            return response.data as Booking;
        } catch (e) {
            throw e;
        }
    }

    async sellFlights(request: SellJourneyByKeyRequestData): Promise<SellResponse> {
        try {
            const response = await this._ajaxClient.post('api/flights', request);
            const sellResponse = response.data as SellResponse;
            if (sellResponse && sellResponse.bookingUpdateResponseData && sellResponse.bookingUpdateResponseData.error) {
                throw Utils.createNewskiesError(ErrorType.InternalServer, sellResponse.bookingUpdateResponseData.error);
            }
            return sellResponse;
        } catch (e) {
            throw e;
        }
    }
    async changeFlights(request: ChangeFlightsRequest): Promise<SellResponse> {
        try {
            const response = await this._ajaxClient.post('api/flights/change', request);
            const changeResponse = response.data as SellResponse;
            if (changeResponse && changeResponse.bookingUpdateResponseData && changeResponse.bookingUpdateResponseData.error) {
                throw Utils.createNewskiesError(ErrorType.InternalServer, changeResponse.bookingUpdateResponseData.error);
            }
            return changeResponse;
        } catch (e) {
            throw e;
        }
    }

    async getPassengers(): Promise<Passenger[]> {
        try {
            const response = await this._ajaxClient.get('api/passengers');
            const paxResponse = response.data as GetPassengersResponse;
            return paxResponse.passengers;
        } catch (e) {
            throw e;
        }
    }

    async updatePassengers(passengers: Passenger[]) {
        try {
            const request: UpdatePassengersRequestData = {
                passengers
            };
            const response = await this._ajaxClient.post('api/passengers', request);
            const updateResponse = response.data as BookingUpdateResponseData;
            if (updateResponse.error) {
                throw Utils.createNewskiesError(ErrorType.InternalServer, updateResponse.error);
            }
        } catch (e) {
            throw e;
        }
    }

    async getBookingContacts(): Promise<BookingContact[]> {
        try {
            const response = await this._ajaxClient.get('api/contacts');
            const contactsResponse = response.data as GetBookingContactsResponse;
            return contactsResponse.bookingContacts;
        } catch (e) {
            throw e;
        }
    }

    async updateBookingContacts(contacts: BookingContact[]): Promise<UpdateContactsResponse> {
        try {
            const request: UpdateContactsRequestData = {
                bookingContactList: contacts
            };
            const response = await this._ajaxClient.post('api/contacts', request);
            const updateResponse = response.data as UpdateContactsResponse;
            if (updateResponse && updateResponse.bookingUpdateResponseData && updateResponse.bookingUpdateResponseData.error) {
                throw Utils.createNewskiesError(ErrorType.InternalServer, updateResponse.bookingUpdateResponseData.error);
            }
            return updateResponse;
        } catch (e) {
            throw e;
        }
    }

    async getPriceItinerary(request: SellJourneyByKeyRequestData): Promise<PriceItineraryResponse> {
        try {
            const response = await this._ajaxClient.get('api/flights/priceitinerary?' + Utils.urlSerialize(request));
            const priceResponse = response.data as PriceItineraryResponse;
            return priceResponse;
        } catch (e) {
            throw e;
        }
    }

    async getBookingSsrAvailability(): Promise<SSRAvailabilityForBookingResponse> {
        try {
            const response = await this._ajaxClient.get('api/ssrs');
            const ssrResponse = response.data as GetSSRAvailabilityForBookingResponse;
            return ssrResponse.ssrAvailabilityForBookingResponse;
        } catch (e) {
            throw e;
        }
    }

    async getBookingSeatsAvailability(requestData: SeatAvailabilityRequestData): Promise<SeatAvailabilityResponse> {
        try {
            const request: SeatAvailabilityRequest = {
                seatAvailabilityRequestData: requestData
            };
            const response = await this._ajaxClient.get('api/seats?' + Utils.urlSerialize(request));
            const seatResponse = response.data as GetSeatAvailabilityResponse;
            Utils.populateEquipmentProperties(seatResponse.seatAvailabilityResponse);
            return seatResponse.seatAvailabilityResponse;
        } catch (e) {
            throw e;
        }
    }

    async assignSeat(request: AssignSeatRequest): Promise<AssignSeatsResponse> {
        try {
            const axiosResponse = await this._ajaxClient.post('api/seats', request);
            const response: AssignSeatsResponse = <AssignSeatsResponse>axiosResponse.data;
            return response;
        } catch (e) {
            throw e;
        }
    }

    async removeSeat(request: AssignSeatRequest): Promise<AssignSeatsResponse> {
        try {
            const axiosResponse = await this._ajaxClient.delete('api/seats?' + Utils.urlSerialize(request));
            const response: AssignSeatsResponse = <AssignSeatsResponse>axiosResponse.data;
            return response;
        } catch (e) {
            throw e;
        }
    }

    async sellSsr(request: SellSSRRequest): Promise<SellResponse> {
        try {
            const axiosResponse = await this._ajaxClient.post('api/ssrs', request);
            const response: SellResponse = <SellResponse>axiosResponse.data;
            return response;
        } catch (e) {
            throw e;
        }
    }

    async cancelSsr(request: CancelSSRRequest): Promise<CancelResponse> {
        try {
            const axiosResponse = await this._ajaxClient.delete('api/ssrs?' + Utils.urlSerialize(request));
            const response: CancelResponse = <CancelResponse>axiosResponse.data;
            return response;
        } catch (e) {
            throw e;
        }
    }

    async sellFee(request: SellFeeRequestData): Promise<SellResponse> {
        try {
            const axiosResponse = await this._ajaxClient.post('api/fee', request);
            const response: SellResponse = <SellResponse>axiosResponse.data;
            return response;
        } catch (e) {
            throw e;
        }
    }

    async cancelFee(request: CancelFeeRequestData): Promise<CancelResponse> {
        try {
            const axiosResponse = await this._ajaxClient.delete('api/fee?' + Utils.urlSerialize(request));
            const response: CancelResponse = <CancelResponse>axiosResponse.data;
            return response;
        } catch (e) {
            throw e;
        }
    }

    async addPayment(request: AddPaymentToBookingRequestData): Promise<AddPaymentToBookingResponse> {
        try {
            const axiosResponse = await this._ajaxClient.post('api/payment', request);
            const response: AddPaymentToBookingResponse = <AddPaymentToBookingResponse>axiosResponse.data;
            return response;
        } catch (e) {
            throw e;
        }
    }

    async removePayment(request: RemovePaymentFromBookingRequest): Promise<RemovePaymentFromBookingResponse> {
        try {
            const axiosResponse = await this._ajaxClient.delete('api/payment?' + Utils.urlSerialize(request));
            const removePaymentResponse: RemovePaymentFromBookingResponse = <RemovePaymentFromBookingResponse>axiosResponse.data;
            if (removePaymentResponse && removePaymentResponse.bookingUpdateResponseData && removePaymentResponse.bookingUpdateResponseData.error) {
                throw Utils.createNewskiesError(ErrorType.InternalServer, removePaymentResponse.bookingUpdateResponseData.error);
            }
            return removePaymentResponse;
        } catch (e) {
            throw e;
        }
    }

    async commit(): Promise<CommitResponse> {
        try {
            const request: CommitRequest = { flag: false };
            const axiosResponse = await this._ajaxClient.post('api/booking', request);
            const response: CommitResponse = <CommitResponse>axiosResponse.data;
            return response;
        } catch (e) {
            throw e;
        }
    }

    async pollStatus(resetCounter: Boolean): Promise<GetPostCommitResultsResponse> {
        try {
            const axiosResponse = await this._ajaxClient.get('api/booking/polling' + (resetCounter ? '?resetCounter=true' : ''));
            const response: GetPostCommitResultsResponse = <GetPostCommitResultsResponse>axiosResponse.data;
            return response;
        } catch (e) {
            throw e;
        }
    }

    async retrieveBooking(request: RetrieveBookingRequest): Promise<Booking> {
        try {
            const clonedRequest = { ...request };
            delete clonedRequest.recordLocator;
            const urlQuery = Utils.urlSerialize(clonedRequest);
            const response = await this._ajaxClient.get(`api/booking/${request.recordLocator}?${urlQuery}`);
            const bookingResponse = response.data as RetrieveBookingResponse;
            return bookingResponse.booking;
        } catch (e) {
            throw e;
        }
    }

    async resendItinerary() {
        try {
            await this._ajaxClient.get('api/booking/itinerary');
        } catch (e) {
            throw e;
        }
    }

    async getBarcodedBoardingPass(request: GetBarCodedBoardingPassesRequest): Promise<GetBarCodedBoardingPassesResponse> {
        try {
            const response = await this._ajaxClient.post('api/checkin/GetBarCodedBoardingPasses', request);
            return response.data as GetBarCodedBoardingPassesResponse;
        } catch (e) {
            throw e;
        }
    }

    async applyPromotion(code: string): Promise<ApplyPromotionResponse> {
        try {
            const request: ApplyPromotionRequestData = {
                promotionCode: code,
            };
            const response = await this._ajaxClient.post('api/booking/ApplyPromotion', request);
            return response.data as ApplyPromotionResponse;
        } catch (e) {
            throw e;
        }
    }
}