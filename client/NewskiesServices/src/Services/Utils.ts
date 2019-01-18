import { AxiosError } from "axios";
import { Journey, Segment, Leg, Fare, FareApplicationType, NewskiesError, ErrorType, SeatAvailabilityResponse, Booking } from "../Contracts/Contracts";

export default class Utils {

    static urlSerialize(obj: any, prefix?: string): string {
        let result = []
        let prop: any;
        for (prop in obj) {
            if (obj.hasOwnProperty(prop)) {
                let k = prefix ? prefix + (!isNaN(prop) ? "[" + prop + "]" : "." + prop) : prop, v = obj[prop];
                result.push((v !== null && typeof v === "object") ?
                    Utils.urlSerialize(v, k) :
                    k + "=" + v);
            }
        }
        return result.join("&");
    }

    static enumsEqual(e1: number | string, e2: number | string, enumObject: any): boolean {
        const type1 = typeof e1;
        const type2 = typeof e2;
        if (type1 === type2) {
            return e1 === e2;
        }
        if ((type1 === null || type1 === 'undefined') && type2 != null && type2 != 'undefined') {
            return false;
        }
        if ((type2 === null || type2 === 'undefined') && type1 != null && type1 != 'undefined') {
            return false;
        }
        if (type1 === 'number' && type2 === 'string') {
            return enumObject[e1] === e2;
        }
        if (type2 === 'number' && type1 === 'string') {
            return enumObject[e2] === e1;
        }
        throw new Error('Invalid Enum values passed for comparision');
    }

    static getNewskiesError(error: Error): NewskiesError {
        const axiosError = error as AxiosError;
        if (axiosError) {
            if (!axiosError.response) {
                return { ...<any>axiosError, errorType: ErrorType.ServerUnavaliable };
            }
            return { ...<any>axiosError, errorType: axiosError.response.status, payload: axiosError.response.data };
        }
        return { ...error, errorType: ErrorType.UnhandledError };
    }

    static createNewskiesError(errorType: ErrorType, error: Error = new Error()): NewskiesError {
        return { ...error, errorType };
    }

    static populateEquipmentProperties(response: SeatAvailabilityResponse) {
        if (!response.propertyTypeCodesLookup || !response.propertyTypeCodesLookup.booleanPropertyTypes || !response.equipmentInfos) {
            return;
        }
        const booleanPropsLookup = response.propertyTypeCodesLookup.booleanPropertyTypes;
        response.equipmentInfos.forEach(info => {
            if (!info.compartments) {
                return;
            }
            info.compartments.forEach(comp => {
                if (!comp.seats) {
                    return;
                }
                comp.seats.forEach(seat => {
                    seat.propertyList = seat.propertyList || [];
                    let bitsStr = '';
                    seat.propertyBits.forEach(num => {
                        bitsStr = `${(num).toString(2).padStart(32, '0')}${bitsStr}`;
                    });
                    const bits = bitsStr.split('').reverse().map(Number);
                    bits.forEach((bit, ind) => {
                        if (!bit) {
                            return;
                        }
                        seat.propertyList.push(booleanPropsLookup[ind]);
                    });
                });
            });
        });
    }

    static isInternational(booking: Booking) {
        return booking.journeys.some(j => j.segments.some(s => s.international == true));
    }
}