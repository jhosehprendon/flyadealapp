import moment from 'moment';
import { Contracts as NskContracts, Utils as NskUtils } from 'newskies-services';
import { FlightType } from "../Contracts";

const creditCardPaymentMethods = ['M1', 'V1'];
const sadadOfflinePaymentMethod = 'SD';
const voucherPaymentMethod = 'VO';

const agentRoleCodes = ['TMST', 'TAGT'];
const corpAgentRoleCodes = ['COMA', 'COAG'];

export default class Utils {

    static delay<T>(msec: number, value?: T) {
        return new Promise<T>((resolve) => setTimeout(() => resolve(value), msec));
    }

    static isCreditCardPaymentMethod(code: string): boolean {
        return creditCardPaymentMethods.some(c => c === code);
    }

    static isSadadOfflinePaymentMethod(code: string): boolean {
        return sadadOfflinePaymentMethod === code;
    }

    static isVoucherPaymentMethod(code: string): boolean {
        return voucherPaymentMethod === code;
    }

    static createPaxTypeCounts(adults: number, children?: number, infants?: number) {
        const paxTypeCounts: NskContracts.PaxTypeCount[] = [];
        paxTypeCounts.push({
            paxTypeCode: 'ADT',
            paxCount: adults
        });
        if (children) {
            paxTypeCounts.push({
                paxTypeCode: 'CHD',
                paxCount: children
            });
        }
        if (infants) {
            paxTypeCounts.push({
                paxTypeCode: 'INFT',
                paxCount: infants
            });
        }
        return paxTypeCounts;
    }

    static getSegmentProductClass(segment: NskContracts.Segment): string {
        return segment && segment.fares && segment.fares.length ?
            segment.fares[0].productClass : '';
    }

    static getJourneyProductClass(journey: NskContracts.Journey): string {
        return journey && journey.segments && journey.segments.length ?
            Utils.getSegmentProductClass(journey.segments[0]) : '';
    }

    static getJourneyLegs(journey: NskContracts.Journey): NskContracts.Leg[] {
        return journey && journey.segments && journey.segments.length ?
            journey.segments.mapMany(s => s.legs) : [];
    }

    static getJourneyStd(journey: NskContracts.Journey): moment.Moment {
        return journey && journey.segments && journey.segments.length ? moment(journey.segments[0].std) : null;
    }

    static getJourneySta(journey: NskContracts.Journey): moment.Moment {
        return journey && journey.segments && journey.segments.length ? moment(journey.segments[journey.segments.length - 1].sta) : null;
    }

    static getJourneyDepartureStation(journey: NskContracts.Journey): string {
        return journey && journey.segments && journey.segments.length ? journey.segments[0].departureStation : null;
    }

    static getJourneyArrivalStation(journey: NskContracts.Journey): string {
        return journey && journey.segments && journey.segments.length ? journey.segments[journey.segments.length - 1].arrivalStation : null;
    }

    static getJourneyFlightDesignator(journey: NskContracts.Journey): string {
        const segments = journey && journey.segments && journey.segments.length ? journey.segments : [];
        let result = '';
        segments.forEach((seg, index) => {
            if (!index) {
                result = seg.flightDesignator.carrierCode + seg.flightDesignator.flightNumber;
                return;
            }
            result = result + '/' + seg.flightDesignator.carrierCode + seg.flightDesignator.flightNumber;
        });
        return result;
    }

    static getTravelTimeMins(journey: NskContracts.Journey): number {
        const utcDeptDateTime: moment.Moment = this.getJourneyUTCDeptDateTime(journey);
        const utcArrvDateTime: moment.Moment = this.getJourneyUTCArrvDateTime(journey);
        if (utcDeptDateTime != moment.min() && utcArrvDateTime != moment.min()) {
            return utcArrvDateTime.diff(utcDeptDateTime, 'minutes');
        }
        return 0;
    }

    static getJourneyUTCDeptDateTime(journey: NskContracts.Journey): moment.Moment {
        if (journey.segments && journey.segments.length) {
            return this.getSegmentUTCDeptDateTime(journey.segments[0]);
        }
        return moment.min();
    }

    static getSegmentUTCDeptDateTime(segment: NskContracts.Segment): moment.Moment {
        if (segment.legs && segment.legs.length && segment.legs[0].legInfo) {
            return this.getLegUTCDeptDateTime(segment.legs[0]);
        }
        return moment(segment.std);
    }

    static getLegUTCDeptDateTime(leg: NskContracts.Leg): moment.Moment {
        if (!leg || !leg.legInfo) {
            return moment.min();
        }
        const dateTime: moment.Moment = moment(leg.std).add(leg.legInfo.deptLTV * -1, 'minutes');
        return dateTime;
    }

    static getJourneyUTCArrvDateTime(journey: NskContracts.Journey): moment.Moment {
        if (journey.segments && journey.segments.length) {
            return this.getSegmentUTCArrvDateTime(journey.segments[journey.segments.length - 1]);
        }
        return moment.min();
    }

    static getSegmentUTCArrvDateTime(segment: NskContracts.Segment): moment.Moment {
        if (segment.legs && segment.legs.length && segment.legs[segment.legs.length - 1].legInfo) {
            return this.getLegUTCArrvDateTime(segment.legs[segment.legs.length - 1]);
        }
        return moment(segment.std);
    }

    static getLegUTCArrvDateTime(leg: NskContracts.Leg): moment.Moment {
        if (!leg || !leg.legInfo) {
            return moment.min();
        }
        const dateTime: moment.Moment = moment(leg.sta).add(leg.legInfo.arrvLTV * -1, 'minutes');
        return dateTime;
    }

    static getJourneyType(journey: NskContracts.Journey): FlightType {
        if (journey && journey.segments && journey.segments.length) {
            if (journey.segments.length > 1)
                return FlightType.Connect;
            if (journey.segments.length == 1)
                return this.getNumberOfStops(journey.segments[0]) <= 0 ? FlightType.NonStop : FlightType.Through;
        }
        return FlightType.None;
    }

    static getNumberOfStops(segment: NskContracts.Segment): number {
        if (segment && segment.legs && segment.legs.length) {
            return segment.legs.length - 1;
        }
        return 0;
    }

    static isGoverningFare(fare: NskContracts.Fare) {
        return NskUtils.enumsEqual(fare.fareApplicationType, NskContracts.FareApplicationType.Route, NskContracts.FareApplicationType) || NskUtils.enumsEqual(fare.fareApplicationType, NskContracts.FareApplicationType.Governing, NskContracts.FareApplicationType);
    }

    static isInternational(booking: NskContracts.Booking) {
        return NskUtils.isInternational(booking);
    }

    static lastPaymentApproved(booking: NskContracts.Booking) {
        return booking && booking.payments && booking.payments.length &&
            (NskUtils.enumsEqual(booking.payments[booking.payments.length - 1].status, NskContracts.BookingPaymentStatus.Approved, NskContracts.BookingPaymentStatus) &&
                NskUtils.enumsEqual(booking.payments[booking.payments.length - 1].authorizationStatus, NskContracts.AuthorizationStatus.Approved, NskContracts.AuthorizationStatus))
            ||
            (booking.payments[booking.payments.length - 1].paymentMethodCode === sadadOfflinePaymentMethod &&
                NskUtils.enumsEqual(booking.payments[booking.payments.length - 1].status, NskContracts.BookingPaymentStatus.PendingCustomerAction, NskContracts.BookingPaymentStatus) &&
                NskUtils.enumsEqual(booking.payments[booking.payments.length - 1].authorizationStatus, NskContracts.AuthorizationStatus.Pending, NskContracts.AuthorizationStatus));
        
    }

    static lastPaymentDeclined(booking: NskContracts.Booking) {
        return booking && booking.payments && booking.payments.length &&
            (NskUtils.enumsEqual(booking.payments[booking.payments.length - 1].status, NskContracts.BookingPaymentStatus.Declined, NskContracts.BookingPaymentStatus));
    }

    static getJourneyBaggageSsrs(journey: NskContracts.Journey): NskContracts.PaxSSR[] {
        if (!journey || !journey.segments) {
            return [];
        }
        return journey.segments.mapMany(s => s.paxSSRs).filter(ssr => Utils.isBaggageSsr(ssr));
    }

    static getJourneyMealSsrs(journey: NskContracts.Journey): NskContracts.PaxSSR[] {
        if (!journey || !journey.segments) {
            return [];
        }
        return journey.segments.mapMany(s => s.paxSSRs).filter(ssr => Utils.isMealSsr(ssr));
    }

    static isBaggageSsr(ssr: NskContracts.PaxSSR): boolean {
        return ssr && ssr.ssrCode && (ssr.ssrCode.startsWith('XP') || ssr.ssrCode.startsWith('F'));
    }

    static isMealSsr(ssr: NskContracts.PaxSSR): boolean {
        return ssr && ssr.ssrCode && ssr.ssrCode.endsWith('ML');
    }

    static isAgent(code: string): boolean {
        return code && agentRoleCodes.some(c => c === code);
    }

    static isCorpAgent(code: string): boolean {
        return code && corpAgentRoleCodes.some(c => c === code);
    }

    static getCorpAgentRoleCodes(): string[] {
        return corpAgentRoleCodes;
    }

    static getAgentRoleCodes(): string[] {
        return agentRoleCodes;
    }
}