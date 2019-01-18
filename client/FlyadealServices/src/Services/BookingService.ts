import { Contracts as NskContracts, BookingService as NskBookingService, Utils as NskUtils, BookingCalculator } from 'newskies-services';
import FlightAvailabilityService from "./FlightAvailabilityService";
import SessionBag from "./SessionBag";
import Utils from "./Utils";
import { Passenger, BookingContact, Booking, PriceItinerary, SSRSegment, SeatMap, RetrievedBooking, BoardingPass } from "../Contracts";
import PassengersTranslator from "../Mapping/PassengersTranslator";
import BookingContactsTranslator from "../Mapping/BookingContactsTranslator";
import ResourceCache from "./ResourceCache";
import TripTranslator from "../Mapping/TripTranslator";
import SsrAvailabilityTranslator from "../Mapping/SsrAvailabilityTranslator";
import SeatMapResponseTranslator from "../Mapping/SeatMapResponseTranslator";
import moment from 'moment';

export default class BookingService {

    private _nskService: NskBookingService;
    private _sessionBag: SessionBag;
    private _resourceCache: ResourceCache;
    private _nskBooking: NskContracts.Booking;

    constructor(nskService: NskBookingService, sessionBag: SessionBag, resourceCache: ResourceCache) {
        this._nskService = nskService;
        this._sessionBag = sessionBag;
        this._resourceCache = resourceCache;
    }

    async clear() {
        this._nskBooking = null;
    }

    async clearStateBooking() {
        try {
            await this._nskService.clearStateBooking();
            this._sessionBag.seatsNewlyAutoAssigned = false;
        } catch (e) {
            throw e;
        }
    }

    async get(): Promise<Booking> {
        try {
            if (!this._nskBooking) {
                this._nskBooking = await this._nskService.get();
            }
            const countries = await this._resourceCache.getCountries();
            const nskBooking = this._nskBooking;
            const contact = nskBooking.bookingContacts && nskBooking.bookingContacts.length ? BookingContactsTranslator.translateNewskiesBookingContact(nskBooking.bookingContacts[0], countries) : null;
            const passengers = PassengersTranslator.translateNewskiesPassengers(nskBooking.passengers);
            const booking: Booking = {
                passengers,
                bookingContacts: contact ? [contact] : [],
                recordLocator: nskBooking.recordLocator,
                currencyCode: nskBooking.currencyCode,
                bookingInfo: nskBooking.bookingInfo,
                pos: nskBooking.pos,
                sourcePOS: nskBooking.sourcePOS,
                bookingHold: nskBooking.bookingHold,
                bookingSum: nskBooking.bookingSum,
                journeys: nskBooking.journeys,
                bookingComments: nskBooking.bookingComments,
                payments: nskBooking.payments,
                state: nskBooking.state
            };
            return booking;
        } catch (error) {
            throw error;
        }
    }

    async sellFlights(journeySellKeys: NskContracts.SellKeyList[]): Promise<NskContracts.SellResponse> {
        try {
            const searchRequest = this._sessionBag.flightSearchRequest;
            const origin = searchRequest.flights.length ? searchRequest.flights[0].departureStation : null;
            const currencyCode = await this._resourceCache.getStationCurrency(origin);
            const request: NskContracts.SellJourneyByKeyRequestData = {
                currencyCode,
                journeySellKeys,
                paxTypeCounts: Utils.createPaxTypeCounts(searchRequest.adults, searchRequest.children, searchRequest.infants),
                typeOfSale: {
                    promotionCode: this.promotionCode || '',
                    paxResidentCountry: searchRequest.paxResidentCountry || '',
                    fareTypes: null
                }
            };
            const response = await this._nskService.sellFlights(request);
            this._sessionBag.journeySellKeys = journeySellKeys;
            this._sessionBag.seatsNewlyAutoAssigned = true;
            await this.applyPromotion();
            await this.clear();
            // await this.get();
            return response;
        } catch (error) {
            throw error;
        }
    }

    async changeFlights(journeySellKeys: NskContracts.SellKeyList[]): Promise<NskContracts.SellResponse> {
        try {
            const request: NskContracts.ChangeFlightsRequest = {
                journeySellKeys
            };
            const response = await this._nskService.changeFlights(request);
            this._sessionBag.seatsNewlyAutoAssigned = true;
            await this.clear();
            return response;
        } catch (error) {
            throw error;
        }
    }

    async getPriceItinerary(journeySellKeys?: NskContracts.SellKeyList[]): Promise<PriceItinerary> {
        try {
            if (!journeySellKeys) {
                await this.get();
                return {
                    bookingSum: this._nskBooking.bookingSum,
                    currencyCode: this._nskBooking.currencyCode,
                    flights: TripTranslator.translateFlights(this._nskBooking.journeys, null, this._nskBooking.passengers)
                };
            }
            const searchRequest = this._sessionBag.flightSearchRequest;
            const origin = searchRequest.flights.length ? searchRequest.flights[0].departureStation : null;
            const currencyCode = await this._resourceCache.getStationCurrency(origin);
            const request: NskContracts.SellJourneyByKeyRequestData = {
                currencyCode,
                journeySellKeys,
                paxTypeCounts: Utils.createPaxTypeCounts(searchRequest.adults, searchRequest.children, searchRequest.infants),
                typeOfSale: {
                    promotionCode: this.promotionCode || '',
                    paxResidentCountry: searchRequest.paxResidentCountry || '',
                    fareTypes: null
                }
            };
            const priceResponse = await this._nskService.getPriceItinerary(request);
            const priceItinerary = {
                bookingSum: priceResponse.bookingSum,
                currencyCode: priceResponse.currencyCode,
                flights: TripTranslator.translateFlights(priceResponse.journeys, null, priceResponse.passengers)
            };
            this._sessionBag.journeySellKeys = journeySellKeys;
            this._sessionBag.priceItinerary = priceItinerary;
            return priceItinerary;
        } catch (error) {
            throw error;
        }
    }

    async getPassengers(): Promise<Passenger[]> {
        try {
            const booking = await this.get();
            return booking.passengers;
            // const nskPassengers = await this._nskService.getPassengers();
            // return PassengersTranslator.translateNewskiesPassengers(nskPassengers);
        } catch (error) {
            throw error;
        }
    }

    async updatePassengers(passengers: Passenger[]) {
        try {
            const nskPassengers = PassengersTranslator.translateFlyadealPassengers(passengers);
            await this._nskService.updatePassengers(nskPassengers);
            await this.clear();
            // await this.get();
        } catch (error) {
            throw error;
        }
    }

    async getBookingContact(): Promise<BookingContact> {
        try {
            const booking = await this.get();
            // const contacts = await this._nskService.getBookingContacts();
            // const countries = await this._resourceCache.getCountries();
            return booking.bookingContacts && booking.bookingContacts.length ? booking.bookingContacts[0] : null;
        } catch (error) {
            throw error;
        }
    }

    async updateBookingContact(contact: BookingContact) {
        try {
            const nskContact = BookingContactsTranslator.translateFlyadealBookingContact(contact);
            await this._nskService.updateBookingContacts([nskContact]);
            await this.clear();
            // await this.get();
        } catch (error) {
            throw error;
        }
    }

    async getBookingSsrAvailability(): Promise<SSRSegment[]> {
        try {
            const response = await this._nskService.getBookingSsrAvailability();
            if (!response || !response.ssrSegmentList) {
                return [];
            }
            const segments: SSRSegment[] = SsrAvailabilityTranslator.translate(response.ssrSegmentList);
            return segments;
        } catch (error) {
            throw error;
        }
    }

    async getBookingSeatsAvailability(requestData: NskContracts.SeatAvailabilityRequestData): Promise<SeatMap> {
        try {
            const response = await this._nskService.getBookingSeatsAvailability(requestData);
            const booking = await this.get();
            return SeatMapResponseTranslator.translate(requestData, response, booking);
        } catch (e) {
            throw e;
        }
    }

    async assignSeat(assignSeatData: NskContracts.AssignSeatData): Promise<NskContracts.AssignSeatsResponse> {
        try {
            const request: NskContracts.AssignSeatRequest = {
                assignSeatData
            };
            const response = await this._nskService.assignSeat(request);
            this._sessionBag.seatsNewlyAutoAssigned = false;
            await this.clear();
            return response;
        } catch (e) {
            throw e;
        }
    }

    async removeSeat(assignSeatData: NskContracts.AssignSeatData): Promise<NskContracts.AssignSeatsResponse> {
        try {
            const request: NskContracts.AssignSeatRequest = {
                assignSeatData
            };
            const response = await this._nskService.removeSeat(request);
            this._sessionBag.seatsNewlyAutoAssigned = false;
            await this.clear();
            return response;
        } catch (e) {
            throw e;
        }
    }

    async sellSsr(ssrRequestData: NskContracts.SSRRequestData): Promise<NskContracts.SellResponse> {
        try {
            const request: NskContracts.SellSSRRequest = {
                ssrRequestData
            };
            const response = await this._nskService.sellSsr(request);
            await this.clear();
            return response;
        } catch (e) {
            throw e;
        }
    }

    async cancelSsr(ssrRequestData: NskContracts.SSRRequestData): Promise<NskContracts.CancelResponse> {
        try {
            const request: NskContracts.CancelSSRRequest = {
                ssrRequestData
            };
            const response = await this._nskService.cancelSsr(request);
            await this.clear();
            return response;
        } catch (e) {
            throw e;
        }
    }

    async sellFee(feeRequestData: NskContracts.SellFeeRequestData): Promise<NskContracts.SellResponse> {
        try {
            const response = await this._nskService.sellFee(feeRequestData);
            await this.clear();
            return response;
        } catch (e) {
            throw e;
        }
    }

    async cancelFee(feeRequestData: NskContracts.CancelFeeRequestData): Promise<NskContracts.CancelResponse> {
        try {
            const response = await this._nskService.cancelFee(feeRequestData);
            await this.clear();
            return response;
        } catch (e) {
            throw e;
        }
    }

    async hasAutoAssignedSeats(): Promise<boolean> {
        return this._sessionBag.seatsNewlyAutoAssigned;
    }

    async addPayment(data: NskContracts.AddPaymentToBookingRequestData): Promise<NskContracts.AddPaymentToBookingResponseData> {
        try {
            const response = await this._nskService.addPayment(data);
            await this.clear();
            if (!response || !response.bookingPaymentResponse || !response.bookingPaymentResponse.validationPayment
                || !response.bookingPaymentResponse.validationPayment.payment) {
                throw NskUtils.createNewskiesError(NskContracts.ErrorType.AddPaymentError, new Error(`Error adding payment to the booking`));
            }
            const payment = response.bookingPaymentResponse.validationPayment.payment;
            if (response.bookingPaymentResponse.validationPayment.paymentValidationErrors
                && response.bookingPaymentResponse.validationPayment.paymentValidationErrors.length) {
                const error = response.bookingPaymentResponse.validationPayment.paymentValidationErrors[0];
                throw NskUtils.createNewskiesError(NskContracts.ErrorType.AddPaymentError, new Error(`[${error.errorType}] ${error.errorDescription}`));
            }
            if (Utils.isCreditCardPaymentMethod(payment.paymentMethodCode)) {
                if (!NskUtils.enumsEqual(payment.authorizationStatus, NskContracts.AuthorizationStatus.Pending, NskContracts.AuthorizationStatus) ||
                    !NskUtils.enumsEqual(payment.status, NskContracts.BookingPaymentStatus.Received, NskContracts.BookingPaymentStatus)) {
                    throw NskUtils.createNewskiesError(NskContracts.ErrorType.AddPaymentError, new Error(`Error adding payment to the booking`));
                }
            }
            // TODO: SADAD and all other payment validations
            if (!Utils.isVoucherPaymentMethod(payment.paymentMethodCode)) {
                // assume all non-voucher payments always pay the full balance
                const booking = await this.get();
                if (booking.bookingSum.balanceDue) {
                    throw NskUtils.createNewskiesError(NskContracts.ErrorType.AddPaymentError, new Error(`Error adding payment to the booking. Balance due is ${booking.bookingSum.balanceDue}`));
                }
            }
            return response.bookingPaymentResponse;
        } catch (e) {
            throw e;
        }
    }

    async removePayment(paymentNumber: number) {
        try {
            const request: NskContracts.RemovePaymentFromBookingRequest = {
                paymentNumber
            };
            const response = await this._nskService.removePayment(request);
            const error = response.bookingUpdateResponseData.error;
            if (error) {
                throw NskUtils.createNewskiesError(NskContracts.ErrorType.RemovePaymentError, new Error(`[${error.name}] ${error.message}`));
            }
            return response.bookingUpdateResponseData.success;
        } catch (e) {
            throw e;
        }
    }

    async commit(): Promise<NskContracts.CommitResponse> {
        try {
            const response = await this._nskService.commit();
            await this.clear();
            if (!response || !response.bookingUpdateResponseData || !response.bookingUpdateResponseData.success) {
                const message = response.bookingUpdateResponseData.error ? `${response.bookingUpdateResponseData.error.name}:${response.bookingUpdateResponseData.error.message}` : '';
                throw NskUtils.createNewskiesError(NskContracts.ErrorType.BookingCommitError, new Error(`Error committing booking.${message}`));
            }
            return response;
        } catch (e) {
            throw e;
        }
    }

    async pollStatus(): Promise<NskContracts.GetPostCommitResultsResponse> {
        try {
            let response = await this._nskService.pollStatus(true);
            let count = 0;
            while (response && response.shouldContinuePolling && count < response.maxQueryCount) {
                await Utils.delay(response.repeatQueryIntervalSecs * 1000);
                count++;
                response = await this._nskService.pollStatus(false);
            }
            await this.clear();
            return response;
        } catch (e) {
            throw e;
        }
    }

    async retrieveBooking(request: NskContracts.RetrieveBookingRequest): Promise<Booking> {
        try {
            this._nskBooking = await this._nskService.retrieveBooking(request);
            return await this.get();
        } catch (error) {
            throw error;
        }
    }

    async resendItinerary() {
        try {
            await this._nskService.resendItinerary();
        } catch (error) {
            throw error;
        }
    }

    get journeySellKeys(): NskContracts.SellKeyList[] {
        return this._sessionBag.journeySellKeys;
    }

    set journeySellKeys(value: NskContracts.SellKeyList[]) {
        this._sessionBag.journeySellKeys = value;
    }

    get priceItinerary(): PriceItinerary {
        return this._sessionBag.priceItinerary;
    }

    set priceItinerary(value: PriceItinerary) {
        this._sessionBag.priceItinerary = value;
    }

    getPaxFeePrice(passengerNumber: number, predicate: (fee: NskContracts.PassengerFee) => boolean, departureStation: string, arrivalStation: string): number {
        if (!this._nskBooking) {
            throw new Error('Unable to calculate Fee price. Booking needs to be retrieved first using get() method');
        }
        const passenger = this._nskBooking.passengers.find(p => p.passengerNumber === passengerNumber);
        if (!passenger) {
            throw new Error(`Unable to calculate Fee price. Passenger with ${passengerNumber} passenger number not found`);
        }
        const fee = passenger.passengerFees.find(fee => predicate(fee) && fee.flightReference.indexOf(`${departureStation}${arrivalStation}`) > -1);
        if (!fee) {
            return null;
        }
        const response = BookingCalculator.calculatePassengerFee(NskContracts.CalculationResult.TotalPassengerFee, fee);
        return response.amount;
    }

    getPaxSsrPrice(passengerNumber: number, predicate: (ssr: NskContracts.PaxSSR) => boolean, departureStation: string, arrivalStation: string): number {
        if (!this._nskBooking) {
            throw new Error('Unable to calculate Fee price. Booking needs to be retrieved first using get() method');
        }
        const segment = this._nskBooking.journeys.mapMany(j => j.segments).find(s => s.departureStation === departureStation && s.arrivalStation === arrivalStation);
        if (!segment) {
            throw new Error(`Unable to calculate Fee price. ${departureStation}-${arrivalStation} segment not found`);
        }
        const ssr = segment.paxSSRs.find(ssr => predicate(ssr) && ssr.passengerNumber === passengerNumber);
        if (!ssr) {
            return null;
        }
        const feePrice = this.getPaxFeePrice(passengerNumber, fee => fee.ssrNumber === ssr.ssrNumber && fee.ssrCode === ssr.ssrCode, departureStation, arrivalStation);
        return feePrice ? feePrice : 0;
    }

    getPaxSeat(passengerNumber: number, departureStation: string, arrivalStation: string): NskContracts.PaxSeat {
        if (!this._nskBooking) {
            throw new Error('Unable to get passenger seat. Booking needs to be retrieved first using get() method');
        }
        const segment = this._nskBooking.journeys.mapMany(j => j.segments).find(s => s.departureStation === departureStation && s.arrivalStation === arrivalStation);
        if (!segment) {
            throw new Error(`Unable to get passenger seat. ${departureStation}-${arrivalStation} segment not found`);
        }
        const seat = segment.paxSeats.find(seat => seat.passengerNumber === passengerNumber);
        return seat;
    }

    async manageBooking() {
        this._sessionBag.isManage = true;
    }

    async isManageBooking(): Promise<boolean> {
        return this._sessionBag.isManage;
    }

    async setJourneysToManage(journeyIndexes: number[]) {
        this._sessionBag.manageJourneys = journeyIndexes;
    }

    async getJourneysToManage(): Promise<number[]> {
        return this._sessionBag.manageJourneys;
    }

    async allJourneysCannotBeChanged(booking: NskContracts.Booking): Promise<boolean> {
        const journeysNotYetFlown = booking.journeys.filter(j => moment(Utils.getJourneyStd(j)) > moment());
        const changeableJourney = journeysNotYetFlown.find(j => j.segments.find(s => s.paxSegments && s.paxSegments.find(ps =>
            !NskUtils.enumsEqual(ps.checkInStatus, NskContracts.PaxSegmentCheckInStatus.FlightHasAlreadyDeparted, NskContracts.PaxSegmentCheckInStatus)
            && !NskUtils.enumsEqual(ps.checkInStatus, NskContracts.PaxSegmentCheckInStatus.TooCloseToDeparture, NskContracts.PaxSegmentCheckInStatus)
            && !NskUtils.enumsEqual(ps.checkInStatus, NskContracts.PaxSegmentCheckInStatus.AlreadyCheckedIn, NskContracts.PaxSegmentCheckInStatus)
        ) !== undefined) !== undefined);
        return changeableJourney === undefined;
    }

    async isJourneyChangeable(journey: NskContracts.Journey): Promise<boolean> {
        if (moment(Utils.getJourneyStd(journey)) < moment()) {
            return false;
        }
        const isChangeable = journey.segments.find(s => s.paxSegments && s.paxSegments.find(ps =>
            !NskUtils.enumsEqual(ps.checkInStatus, NskContracts.PaxSegmentCheckInStatus.FlightHasAlreadyDeparted, NskContracts.PaxSegmentCheckInStatus)
            && !NskUtils.enumsEqual(ps.checkInStatus, NskContracts.PaxSegmentCheckInStatus.TooCloseToDeparture, NskContracts.PaxSegmentCheckInStatus)
            && !NskUtils.enumsEqual(ps.checkInStatus, NskContracts.PaxSegmentCheckInStatus.AlreadyCheckedIn, NskContracts.PaxSegmentCheckInStatus)
        ) !== undefined) !== undefined;
        return isChangeable;
    }

    async setJourneyIndexBoardingPass(journeyIndex: number) {
        this._sessionBag.boardingPassJourney = journeyIndex;
    }

    async getJourneyIndexBoardingPass(): Promise<number> {
        return this._sessionBag.boardingPassJourney;
    }

    async setPassengerIndexBoardingPass(passengerIndex: number) {
        this._sessionBag.boardingPassPassenger = passengerIndex;
    }

    async getPassengerIndexBoardingPass(): Promise<number> {
        return this._sessionBag.boardingPassPassenger;
    }

    async clearManageBooking() {
        this._sessionBag.manageJourneys = undefined;
        this._sessionBag.isManage = undefined;
    }

    async getBarcodedBoardingPass(requestData: NskContracts.BoardingPassRequest): Promise<BoardingPass> {
        try {
            requestData = {
                barCodeType: requestData.barCodeType || NskContracts.BarCodeType.Default,
                journeyIndex: requestData.journeyIndex || 0,
                segmentIndex: requestData.segmentIndex || 0,
                legIndex: requestData.legIndex || 0,
                paxNumber: requestData.paxNumber || 0,
                barcodeHeight: requestData.barcodeHeight || 300,
                barcodeWidth: requestData.barcodeWidth || 500
            };
            const request: NskContracts.GetBarCodedBoardingPassesRequest = {
                boardingPassRequest: requestData
            }
            const response = await this._nskService.getBarcodedBoardingPass(request);
            return response as BoardingPass;
        } catch (e) {
            throw e;
        }
    }

    async applyPromotion(code?: string): Promise<NskContracts.BookingUpdateResponseData> {
        try {
            if (code) {
                const response = await this._nskService.applyPromotion(code);
                return response.bookingUpdateResponseData as NskContracts.BookingUpdateResponseData;
            }
            const cached = this.promotionCode;
            const response = cached ? await this._nskService.applyPromotion(cached) : null;
            if (!response) {
                return;
            }
            return response.bookingUpdateResponseData as NskContracts.BookingUpdateResponseData;
        } catch (e) {
            throw e;
        }
    }

    set promotionCode(code: string) {
        this._sessionBag.promotionCode = code;
    }

    get promotionCode(): string {
        return this._sessionBag.promotionCode;
    }

    async getConfirmationRefreshCount(): Promise<Number> {
        return this._sessionBag.currentRefreshCount;
    }

    async setConfirmationRefreshCount(value: Number) {
        this._sessionBag.currentRefreshCount = value;
    }

    async setTriggerPurchaseEvent() {
        this._sessionBag.triggerPurchaseEvent = true;
    }

    async setTriggerNoPurchaseEvent() {
        this._sessionBag.triggerPurchaseEvent = false;
    }

    async hasTriggeredPurchaseEvent() {
        return this._sessionBag.triggerPurchaseEvent;
    }

}