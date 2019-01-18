import { Contracts as NskContracts, Utils as NskUtils } from 'newskies-services';
import { PaxSegmentCheckIn } from "../Contracts";

export default class OperationTranslator {

    static translatePaxSegmentsCheckIn(paxSegmentsCheckIn: PaxSegmentCheckIn[]): NskContracts.CheckInMultiplePassengerRequest[] {
        const result: NskContracts.CheckInMultiplePassengerRequest[] = [];
        paxSegmentsCheckIn.forEach(psc => {
            const existingObj = result.find(p => p.journeyIndex === psc.journeyIndex && p.segmentIndex === psc.segmentIndex);
            if (existingObj) {
                existingObj.passengerNumbers.push(psc.passengerNumber);
            }
            else {
                const checkinMultiplePaxReq: NskContracts.CheckInMultiplePassengerRequest = {
                    journeyIndex: psc.journeyIndex,
                    segmentIndex: psc.segmentIndex,
                    passengerNumbers: [psc.passengerNumber]
                };
                result.push(checkinMultiplePaxReq);
            }
        });
        return result;
    }
}