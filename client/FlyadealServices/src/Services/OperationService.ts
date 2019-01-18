import { Contracts as NskContracts, BookingService as NskBookingService, Utils as NskUtils, BookingCalculator, CheckinService as NskCheckinService} from 'newskies-services';
import SessionBag from "./SessionBag";
import BookingService from "./BookingService";
import Contracts from "../Contracts";
import OperationTranslator from "../Mapping/OperationTranslator";

export default class OperationService {
    
    private _sessionBag: SessionBag;
    private _bookingService: BookingService;
    private _checkinService: NskCheckinService;

    constructor(sessionBag: SessionBag, bookingService: BookingService, checkinService: NskCheckinService) {
        this._sessionBag = sessionBag;
        this._bookingService = bookingService;
        this._checkinService = checkinService;
    }

    async clear() {
        this._sessionBag.checkInSelection = undefined;
    }

    async addPaxSegmentToCheckIn(journeyIndex: number, segmentIndex: number, passengerNumber: number) {
        try {
            await this.validateCheckInJourneyPaxSegment(journeyIndex, segmentIndex, passengerNumber);
            let checkInSelection: Contracts.CheckInSelection = this._sessionBag.checkInSelection;
            if (!checkInSelection) {
                checkInSelection = { paxSegments: [] };
            }
            const added = checkInSelection.paxSegments.find(c => c.journeyIndex === journeyIndex && c.segmentIndex === segmentIndex && c.passengerNumber === passengerNumber);
            if (added) {
                return;
            }
            checkInSelection.paxSegments.push({
                journeyIndex,
                segmentIndex,
                passengerNumber
            });
            this._sessionBag.checkInSelection = checkInSelection;
        } catch (error) {
            throw error;
        }
    }

    async removePaxSegmentToCheckIn(journeyIndex: number, segmentIndex: number, passengerNumber: number) {
        try {
            await this.validateCheckInJourneyPaxSegment(journeyIndex, segmentIndex, passengerNumber);
            let checkInSelection: Contracts.CheckInSelection = this._sessionBag.checkInSelection;
            if (!checkInSelection) {
                checkInSelection = { paxSegments: [] };
            }
            checkInSelection.paxSegments = checkInSelection.paxSegments.filter(c => c.journeyIndex !== journeyIndex || c.segmentIndex !== segmentIndex || c.passengerNumber !== passengerNumber);
            this._sessionBag.checkInSelection = checkInSelection;
        } catch (error) {
            throw error;
        }
    }

    isSelectedToCheckIn(journeyIndex: number, segmentIndex: number, passengerNumber: number): boolean {
        const checkInSelection: Contracts.CheckInSelection = this._sessionBag.checkInSelection;
        return checkInSelection && checkInSelection.paxSegments && checkInSelection.paxSegments.some(c => c.journeyIndex === journeyIndex && c.segmentIndex === segmentIndex && c.passengerNumber === passengerNumber);
    }

    async getCheckInSelection(bypassValidation: boolean = false): Promise<Contracts.CheckInSelection> {
        const selection = this._sessionBag.checkInSelection;
        if (bypassValidation) {
            return selection;
        }
        if (selection && selection.paxSegments) {
            for (const paxSeg of selection.paxSegments) {
                await this.validateCheckInJourneyPaxSegment(paxSeg.journeyIndex, paxSeg.segmentIndex, paxSeg.passengerNumber);
            }
        }
        return selection;
    }

    async checkinPassengers(): Promise<NskContracts.CheckInPassengersResponseData> {
        const selection = await this.getCheckInSelection();
        const checkInPaxesRequestData: NskContracts.CheckInPassengersRequestData = {
            checkInMultiplePassengerRequestList: OperationTranslator.translatePaxSegmentsCheckIn(selection.paxSegments)
        };
        return await this._checkinService.checkinPassengers(checkInPaxesRequestData);
    }

    async autoAssignSeats() {
        const selection = await this.getCheckInSelection();
        const segmentsCheckingIn = OperationTranslator.translatePaxSegmentsCheckIn(selection.paxSegments);
        const booking = await this._bookingService.get();
        for (let s of segmentsCheckingIn) {
            var legCount = 0;
            for (let l of booking.journeys[s.journeyIndex].segments[s.segmentIndex].legs) {
                const paxSeats = booking.journeys[s.journeyIndex].segments[s.segmentIndex].paxSeats;
                var doAssign = false;
                s.passengerNumbers.forEach(paxNo => {
                    const paxSeat = paxSeats.find(ps => ps.passengerNumber === paxNo && ps.departureStation === l.departureStation && ps.arrivalStation === l.arrivalStation);
                    if (paxSeat === undefined) {
                        doAssign = true;
                    }
                });
                if (doAssign) {
                    const assignSeatData: NskContracts.AssignSeatData = {
                        journeyIndex: s.journeyIndex,
                        segmentIndex: s.segmentIndex,
                        legIndex: legCount,
                        paxNumber: null,
                        compartmentDesignator: "",
                        unitDesignator: ""
                    };
                    await this._bookingService.assignSeat(assignSeatData);
                }
                legCount++;
            }
        }
    }

    async allRequiredSeatsSelected(): Promise<boolean> {
        const checkInSelection = await this.getCheckInSelection();
        if (checkInSelection && checkInSelection.paxSegments) {
            const booking = await this._bookingService.get();
            const allLegs = booking.journeys.mapMany((journey, journeyIndex) => journey.segments.mapMany((segment, segmentIndex) => segment.legs.map((leg, legIndex) => { return { ...leg, journeyIndex, segmentIndex, legIndex }; })));
            const legsCheckIn = allLegs.filter(l => checkInSelection.paxSegments.find(c => c.journeyIndex === l.journeyIndex && c.segmentIndex === l.segmentIndex));
            const numSeatsRequired = legsCheckIn.length * booking.passengers.filter(p => p.paxType !== "INFT").length;
            const allPaxSeats = booking.journeys.mapMany((j, journeyIndex) => j.segments.mapMany((s, segmentIndex) => s.paxSeats.map(paxSeat => { return { ...paxSeat, journeyIndex, segmentIndex }; })));
            const paxSeats = allPaxSeats.filter(seat => checkInSelection.paxSegments.find(checkInSeg => checkInSeg.journeyIndex === seat.journeyIndex && checkInSeg.segmentIndex === seat.segmentIndex));
            const numSeatsAssigned = paxSeats ? paxSeats.length : 0;
            return numSeatsRequired === numSeatsAssigned;
        }
        return false;
    }

    async flownSegmentExists(): Promise<boolean> {
        const booking = await this._bookingService.get();
        const flownPaxSegment = booking.journeys[0].segments.filter(s => s.paxSegments && s.paxSegments.find(
            ps => ps.checkInStatus.toString() === 'FlightHasAlreadyDeparted' || ps.liftStatus.toString() === 'Boarded'));
        return flownPaxSegment.length > 0;
    }

    private async validateCheckInJourneyPaxSegment(journeyIndex: number, segmentIndex: number, passengerNumber: number) {
        const booking = await this._bookingService.get();
        if (!booking || !booking.recordLocator || !booking.journeys) {
            throw NskUtils.createNewskiesError(NskContracts.ErrorType.BadData, new Error(`Error validating pax/segment for check in. Booking is empty or incomplete`));
        }
        if (!booking.journeys[journeyIndex] || !booking.journeys[journeyIndex].segments[segmentIndex]) {
            throw NskUtils.createNewskiesError(NskContracts.ErrorType.BadData, new Error(`Error validating pax/segment for check in. Unable to locate journey/segments with ${journeyIndex}/${segmentIndex} indexes`));
        }
        const passenger = booking.passengers.find(p => p.passengerNumber === passengerNumber);
        if (!passenger) {
            throw NskUtils.createNewskiesError(NskContracts.ErrorType.BadData, new Error(`Error validating pax/segment for check in. Unable to locate passenger with ${passengerNumber} passenger number`));
        }
        const segment = booking.journeys[journeyIndex].segments[segmentIndex];
        const paxSegment = segment.paxSegments.find(ps => ps.passengerNumber === passengerNumber);
        if (!paxSegment) {
            throw NskUtils.createNewskiesError(NskContracts.ErrorType.BadData, new Error(`Error validating pax/segment for check in. Unable to locate PaxSegment for ${passengerNumber} passenger number`));
        }
        if (!NskUtils.enumsEqual(paxSegment.checkInStatus, NskContracts.PaxSegmentCheckInStatus.Allowed, NskContracts.PaxSegmentCheckInStatus)) {
            throw NskUtils.createNewskiesError(NskContracts.ErrorType.BadData, new Error(`Error validating pax/segment for check in. Passenger check-in status is ${paxSegment.checkInStatus}`));
        }
    }
}