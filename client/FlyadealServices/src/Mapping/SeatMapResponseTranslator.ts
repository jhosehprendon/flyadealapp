import _ from 'underscore';
import { Contracts as NskContracts, Utils as NskUtils } from 'newskies-services';
import { SeatMap, SeatGroup, Booking, PassengerSeatInfo, PassengerPriceGroup } from "../Contracts";
import TripTranslator from "./TripTranslator";

export default class SeatMapResponseTranslator {

    private static SEAT_GROUPS = [
        { number: 1, name: 'Front row' },
        { number: 2, name: 'Priority' },
        { number: 3, name: 'Speedy exit' },
        { number: 4, name: 'Extra legroom' },
        { number: 5, name: 'Ladies only' },
        { number: 6, name: 'Standard' }
    ];

    static translate(requestData: NskContracts.SeatAvailabilityRequestData, nskResponse: NskContracts.SeatAvailabilityResponse, booking: Booking): SeatMap {
        if (!nskResponse) {
            return null;
        }
        const seatMap: SeatMap = {
            origin: nskResponse.equipmentInfos[0].departureStation,
            destination: nskResponse.equipmentInfos[0].arrivalStation,
            passengerSeatInfos: this.createPassengerSeatInfos(requestData, nskResponse.seatGroupPassengerFees, booking),
            seatGroups: this.createSeatGroups(nskResponse.equipmentInfos[0].compartments[0].seats)
        };
        return seatMap;
    }

    private static createPassengerSeatInfos(requestData: NskContracts.SeatAvailabilityRequestData, seatGroupFees: NskContracts.SeatGroupPassengerFee[], booking: Booking): PassengerSeatInfo[] {
        const segment = booking.journeys[requestData.journeyIndex].segments[requestData.segmentIndex];
        return booking.passengers.filter(pax => pax.paxType !== 'INFT').map(pax => {
            const paxSeat = segment.paxSeats ? segment.paxSeats.find(ps => ps.passengerNumber === pax.passengerNumber) : undefined;
            const paxSeatInfo: PassengerSeatInfo = {
                passengerNumber: pax.passengerNumber,
                selectedSeat: paxSeat ? paxSeat.unitDesignator : undefined,
                passengerPriceGroups: undefined,
                is12To15: pax.dateOfBirth ? this.isPax12To15(pax.dateOfBirth.toDate(), new Date(segment.std)) : false
            };
            const paxSeatGroupFees = seatGroupFees.filter(f => f.passengerNumber === pax.passengerNumber);
            paxSeatInfo.passengerPriceGroups = paxSeatGroupFees.map(f => {
                const paxfee = TripTranslator.translatePaxFee(f.passengerFee, pax.passengerNumber);
                return {
                    type: f.seatGroup,
                    price: paxfee.amount
                };
            });
            return paxSeatInfo;
        });
    }

    private static createSeatGroups(seats: NskContracts.SeatInfo[]): SeatGroup[] {
        const seatGroups: SeatGroup[] = [];
        const sortedSeats = _.sortBy(seats, seat => parseInt(seat.seatDesignator, 16));
        sortedSeats.forEach((seat, seatIndex) => {
            let typeGroup: SeatGroup = undefined;
            if (!seatGroups.length || seatGroups[seatGroups.length - 1].type !== seat.seatGroup) {
                const group = SeatMapResponseTranslator.SEAT_GROUPS.find(sg => sg.number === seat.seatGroup);
                typeGroup = {
                    type: seat.seatGroup,
                    seatGroupName: group ? group.name : '',
                    // deck: 1,
                    priceGroup: seat.seatGroup,
                    seats: []
                };
                seatGroups.push(typeGroup);
            } else {
                typeGroup = seatGroups[seatGroups.length - 1];
            }
            typeGroup.seats.push({
                number: seat.seatDesignator,
                available: NskUtils.enumsEqual(seat.seatAvailability, NskContracts.SeatAvailability.Open, NskContracts.SeatAvailability), // seat.seatAvailability === 'Open',
                status: seat.seatAvailability,
                infantAllowed: seat.propertyList.some(p => p.typeCode === 'INFANT' && p.value === 'True'),
                window: seat.propertyList.some(p => p.typeCode === 'WINDOW' && p.value === 'True'),
                femaleOnly: seat.propertyList.some(p => p.typeCode === 'FEMONLY' && p.value === 'True'),
                exitRow: seat.propertyList.some(p => p.typeCode === 'EXITROW' && p.value === 'True'),
                childAllowed: !seat.propertyList.some(p => p.typeCode === 'NCHILD' && p.value === 'True') && seat.seatGroup !== 1,
                youngAdultAllowed: seat.seatGroup !== 1 && seat.seatGroup !== 4
            });
        });
        return seatGroups;
    }

    private static isPax12To15(dob: Date, departDate: Date): boolean {
        const age = departDate.getFullYear() - dob.getFullYear();
        dob.setFullYear(dob.getFullYear() + age);
        const ageYears = departDate.getTime() < dob.getTime() ? age - 1 : age;
        return ageYears >= 12 && ageYears <= 15;
    }

}