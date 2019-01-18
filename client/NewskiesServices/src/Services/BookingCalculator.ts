import * as NskContracts from "../Contracts/Contracts";
import Utils from './Utils';

export default class BookingCalculator {

    private static _chargeTypeMathByResult: ChargeTypeMathResult = {};

    static calculate(calculationResult: NskContracts.CalculationResult, booking: NskContracts.Booking): NskContracts.BookingCalculatorResultResponse {
        if (!booking) {
            return { amount: 0, points: 0 };
        }
        let calculatorResultResponse1: NskContracts.BookingCalculatorResultResponse = { amount: 0, points: 0 };
        let calculatorResultResponse2: NskContracts.BookingCalculatorResultResponse = { amount: 0, points: 0 };
        let calculatorResultResponse3: NskContracts.BookingCalculatorResultResponse = { amount: 0, points: 0 };
        // let calculatorResultResponse4: NskContracts.BookingCalculatorResultResponse = { amount: 0, points: 0 };
        // let calculatorResultResponse5: NskContracts.BookingCalculatorResultResponse = { amount: 0, points: 0 };
        if (BookingCalculator.isPaymentRange(calculationResult) || BookingCalculator.isDueRange(calculationResult) || BookingCalculator.isEMDRange(calculationResult)) {
            const calculatorResultResponse6: NskContracts.BookingCalculatorResultResponse = BookingCalculator.calculatePayments(calculationResult, booking.payments);
            calculatorResultResponse1 = calculatorResultResponse6;
            if (BookingCalculator.isDueRange(calculationResult) || BookingCalculator.isEMDRange(calculationResult)) {
                //if (booking.BookingComponents != null) {
                //    for (int index = 0; index < booking.BookingComponents.Count; ++index)
                //    {
                //        NskContracts.BookingCalculatorResultResponse calculatorResultResponse7 = BookingCalculator.Calculate(calculationResult, booking.BookingComponents[index]);
                //        calculatorResultResponse2.amount += calculatorResultResponse7.amount;
                //        calculatorResultResponse2.points += calculatorResultResponse7.points;
                //    }
                //}
                if (booking.passengers != null) {
                    for (let index = 0; index < booking.passengers.length; ++index) {
                        const calculatorResultResponse7: NskContracts.BookingCalculatorResultResponse = BookingCalculator.calculateJourneysPassenger(NskContracts.CalculationResult.Total, booking.journeys, booking.passengers[index]/*, selectors*/);
                        calculatorResultResponse2.amount += calculatorResultResponse7.amount;
                        calculatorResultResponse2.points += calculatorResultResponse7.points;
                        const calculatorResultResponse8: NskContracts.BookingCalculatorResultResponse = BookingCalculator.calculateJourneysPassenger(NskContracts.CalculationResult.TotalEMDFee, booking.journeys, booking.passengers[index]/*, selectors*/);
                        calculatorResultResponse3.amount += calculatorResultResponse8.amount;
                        calculatorResultResponse3.points += calculatorResultResponse8.points;
                    }
                }
                if (BookingCalculator.isDueRange(calculationResult)) {
                    calculatorResultResponse1.amount = calculatorResultResponse2.amount - calculatorResultResponse6.amount - calculatorResultResponse3.amount;
                    calculatorResultResponse1.points = calculatorResultResponse2.points - calculatorResultResponse6.points - calculatorResultResponse3.points;
                }
                if (BookingCalculator.isEMDRange(calculationResult)) {
                    calculatorResultResponse1.amount = calculatorResultResponse3.amount;
                    calculatorResultResponse1.points = calculatorResultResponse3.points;
                }
            }
        } else {
            //if (booking.BookingComponents != null) {
            //    for (int index = 0; index < booking.BookingComponents.Count; ++index)
            //    {
            //        NskContracts.BookingCalculatorResultResponse calculatorResultResponse6 = BookingCalculator.Calculate(calculationResult, booking.BookingComponents[index]);
            //        calculatorResultResponse1.Amount += calculatorResultResponse6.Amount;
            //        calculatorResultResponse1.Points += calculatorResultResponse6.Points;
            //    }
            //}
            if (booking.passengers != null) {
                for (let index = 0; index < booking.passengers.length; ++index) {
                    const calculatorResultResponse6: NskContracts.BookingCalculatorResultResponse = BookingCalculator.calculateJourneysPassenger(calculationResult, booking.journeys, booking.passengers[index]/*, selectors*/);
                    calculatorResultResponse1.amount += calculatorResultResponse6.amount;
                    calculatorResultResponse1.points += calculatorResultResponse6.points;
                }
            }
        }
        return calculatorResultResponse1;
    }

    static calculateFare(calculationResult: NskContracts.CalculationResult, fare: NskContracts.Fare): NskContracts.BookingCalculatorResultResponse {
        let calculatorResultResponse1: NskContracts.BookingCalculatorResultResponse = { amount: 0, points: 0 };
        // let calculatorResultResponse2: NskContracts.BookingCalculatorResultResponse = { amount: 0, points: 0 };
        if (!fare)
            return calculatorResultResponse1;
        for (let index: number = 0; index < fare.paxFares.length; ++index) {
            let calculatorResultResponse3: NskContracts.BookingCalculatorResultResponse = BookingCalculator.calculatePaxFare(calculationResult, fare.paxFares[index]);
            calculatorResultResponse1.amount += calculatorResultResponse3.amount;
            calculatorResultResponse1.points += calculatorResultResponse3.points;
        }
        return calculatorResultResponse1;
    }

    static calculateJourneysPassenger(calculationResult: NskContracts.CalculationResult, journeyServices: NskContracts.Journey[], passenger: NskContracts.Passenger/*, ...selectors: BookingCalculator.SelectServiceCharge[]*/): NskContracts.BookingCalculatorResultResponse {
        let calculatorResultResponse1: NskContracts.BookingCalculatorResultResponse = { amount: 0, points: 0 };
        // let calculatorResultResponse2: NskContracts.BookingCalculatorResultResponse = { amount: 0, points: 0 };
        if (!journeyServices || !passenger)
            return calculatorResultResponse1;
        if (BookingCalculator.isFareRange(calculationResult)) {
            for (let index: number = 0; index < journeyServices.length; ++index) {
                let calculatorResultResponse3: NskContracts.BookingCalculatorResultResponse = BookingCalculator.calculateJourneyPaxTypeDiscount(calculationResult, journeyServices[index], passenger.passengerTypeInfo.paxType, passenger.paxDiscountCode);
                calculatorResultResponse1.amount += calculatorResultResponse3.amount;
                calculatorResultResponse1.points += calculatorResultResponse3.points;
                if (passenger.infant && passenger.infant.paxType) {
                    let calculatorResultResponse4: NskContracts.BookingCalculatorResultResponse = BookingCalculator.calculateJourneyPaxTypeDiscount(calculationResult, journeyServices[index], passenger.infant.paxType, '');
                    calculatorResultResponse1.amount += calculatorResultResponse4.amount;
                    calculatorResultResponse1.points += calculatorResultResponse4.points;
                }
            }
        }
        if (BookingCalculator.isPassengerFeeRange(calculationResult)) {
            let calculatorResultResponse3: NskContracts.BookingCalculatorResultResponse = BookingCalculator.calculatePassenger(calculationResult, passenger/*, selectors*/);
            calculatorResultResponse1.amount += calculatorResultResponse3.amount;
            calculatorResultResponse1.points += calculatorResultResponse3.points;
        }
        return calculatorResultResponse1;
    }

    static calculatePassenger(calculationResult: NskContracts.CalculationResult, passenger: NskContracts.Passenger/*, ...selectors: BookingCalculator.SelectServiceCharge[]*/): NskContracts.BookingCalculatorResultResponse {
        if (!passenger) {
            return { amount: 0, points: 0 };
        }
        return BookingCalculator.calculatePassengerFees(calculationResult, passenger.passengerFees, NskContracts.FeeType.All/*, selectors*/);
    }

    static calculatePassengerFees(calculationResult: NskContracts.CalculationResult, passengerFees: NskContracts.PassengerFee[], feeType: NskContracts.FeeType/*, ...selectors: BookingCalculator.SelectServiceCharge[]*/): NskContracts.BookingCalculatorResultResponse {
        let calculatorResultResponse1: NskContracts.BookingCalculatorResultResponse = { amount: 0, points: 0 };
        // let calculatorResultResponse2: NskContracts.BookingCalculatorResultResponse = { amount: 0, points: 0 };
        for (let index: number = 0; index < passengerFees.length; ++index) {
            let passengerFee: NskContracts.PassengerFee = passengerFees[index];
            if (passengerFee.feeType == feeType || feeType == NskContracts.FeeType.All) {
                let calculatorResultResponse3: NskContracts.BookingCalculatorResultResponse = BookingCalculator.calculatePassengerFee(calculationResult, passengerFee/*, selectors*/);
                calculatorResultResponse1.amount += calculatorResultResponse3.amount;
                calculatorResultResponse1.points += calculatorResultResponse3.points;
            }
        }
        return calculatorResultResponse1;
    }

    static calculatePassengerFee(calculationResult: NskContracts.CalculationResult, passengerFee: NskContracts.PassengerFee/*, ...selectors: BookingCalculator.SelectServiceCharge[]*/): NskContracts.BookingCalculatorResultResponse {
        if (calculationResult == NskContracts.CalculationResult.TotalNewPassengerFee) {
            if (!passengerFee)
                return { amount: 0, points: 0 };
            //if (passengerFee.ActionStatusCode != "KK")
            //    return new NskContracts.BookingCalculatorResultResponse();
            const serviceCharges: NskContracts.BookingServiceCharge[] = passengerFee.serviceCharges;
            //if (selectors.length != 0 && serviceCharges != null)
            //    serviceCharges = BookingCalculator.selectServiceCharges(passengerFee, selectors);
            return BookingCalculator.calculateServiceCharges(calculationResult, serviceCharges);
        }
        if (!passengerFee)
            return { amount: 0, points: 0 };
        //if (passengerFee.ActionStatusCode == "XX")
        //    return new NskContracts.BookingCalculatorResultResponse();
        const serviceCharges1: NskContracts.BookingServiceCharge[] = passengerFee.serviceCharges;
        //if (selectors.length != 0 && serviceCharges1 != null)
        //    serviceCharges1 = BookingCalculator.selectServiceCharges(passengerFee, selectors);
        return BookingCalculator.calculateServiceCharges(calculationResult, serviceCharges1);
    }

    static calculateJourneyPaxTypeDiscount(calculationResult: NskContracts.CalculationResult, journey: NskContracts.Journey, paxType: string, paxDiscountCode: string): NskContracts.BookingCalculatorResultResponse {
        let calculatorResultResponse1: NskContracts.BookingCalculatorResultResponse = { amount: 0, points: 0 };
        // let calculatorResultResponse2: NskContracts.BookingCalculatorResultResponse = { amount: 0, points: 0 };
        if (!journey || !journey.segments || !journey.segments.length || BookingCalculator.isPassive(journey))
            return calculatorResultResponse1;
        for (let index1: number = 0; index1 < journey.segments.length; ++index1) {
            let segment: NskContracts.Segment = journey.segments[index1];
            if (segment.fares && segment.fares.length) {
                let fare: NskContracts.Fare = segment.fares[0];
                if (fare.paxFares != null) {
                    let paxFare1: NskContracts.PaxFare = null;
                    let paxFare2: NskContracts.PaxFare = {
                        paxType: '',
                        paxDiscountCode: '',
                        fareDiscountCode: '',
                        serviceCharges: [],
                        ticketFareBasisCode: ''
                    };
                    for (let index2: number = 0; index2 < fare.paxFares.length; ++index2) {
                        let paxFare3: NskContracts.PaxFare = fare.paxFares[index2];
                        if (paxFare3.paxType == paxType && paxFare3.paxDiscountCode == paxDiscountCode)
                            paxFare1 = paxFare3;
                        if (paxFare3.paxType == "ADT")
                            paxFare2 = paxFare3;
                    }
                    if (!paxFare1)
                        paxFare1 = paxFare2;
                    let calculatorResultResponse3: NskContracts.BookingCalculatorResultResponse = BookingCalculator.calculatePaxFare(calculationResult, paxFare1);
                    calculatorResultResponse1.amount += calculatorResultResponse3.amount;
                    calculatorResultResponse1.points += calculatorResultResponse3.points;
                }
            }
        }
        return calculatorResultResponse1;
    }

    static calculatePaxFare(calculationResult: NskContracts.CalculationResult, paxFare: NskContracts.PaxFare): NskContracts.BookingCalculatorResultResponse {
        if (!paxFare)
            return { amount: 0, points: 0 };
        return BookingCalculator.calculateServiceCharges(calculationResult, paxFare.serviceCharges);
    }

    static isPassive(journey: NskContracts.Journey): boolean {
        return journey.segments != null && journey.segments.length && journey.segments[0].segmentType === "P";
    }

    static calculatePayments(calculationResult: NskContracts.CalculationResult, payments: NskContracts.Payment[]): NskContracts.BookingCalculatorResultResponse {
        const calculatorResultResponse1: NskContracts.BookingCalculatorResultResponse = { amount: 0, points: 0 };
        if (!payments)
            return calculatorResultResponse1;
        for (let index: number = 0; index < payments.length; ++index) {
            const calculatorResultResponse3: NskContracts.BookingCalculatorResultResponse = BookingCalculator.calculatePayment(calculationResult, payments[index]);
            calculatorResultResponse1.amount += calculatorResultResponse3.amount;
            calculatorResultResponse1.points += calculatorResultResponse3.points;
        }
        return calculatorResultResponse1;
    }

    static calculateJourney(calculationResult: NskContracts.CalculationResult, journey: NskContracts.Journey): NskContracts.BookingCalculatorResultResponse {
        const calculatorResultResponse1: NskContracts.BookingCalculatorResultResponse = { amount: 0, points: 0 };
        if (!journey || !journey.segments || (!journey.segments.length || BookingCalculator.isPassive(journey)))
            return calculatorResultResponse1;
        for (let index: number = 0; index < journey.segments.length; ++index) {
            let segment: NskContracts.Segment = journey.segments[index];
            if (segment.fares != null && segment.fares.length != 0) {
                let fare: NskContracts.Fare = segment.fares[0];
                if (fare.paxFares != null) {
                    let paxFare: NskContracts.PaxFare = {
                        paxType: '',
                        paxDiscountCode: '',
                        fareDiscountCode: '',
                        serviceCharges: [],
                        ticketFareBasisCode: ''
                    };
                    if (fare.paxFares.length > 0)
                        paxFare = fare.paxFares[0];
                    let calculatorResultResponse3: NskContracts.BookingCalculatorResultResponse = BookingCalculator.calculatePaxFare(calculationResult, paxFare);
                    calculatorResultResponse1.amount += calculatorResultResponse3.amount;
                    calculatorResultResponse1.points += calculatorResultResponse3.points;
                }
            }
        }
        return calculatorResultResponse1;
    }

    static calculateFarePaxTypeDiscount(calculationResult: NskContracts.CalculationResult, fare: NskContracts.Fare, paxType: string, paxDiscountCode: string): NskContracts.BookingCalculatorResultResponse {
        const calculatorResultResponse1: NskContracts.BookingCalculatorResultResponse = { amount: 0, points: 0 };
        if (!fare) {
            return calculatorResultResponse1;
        }
        let paxFare1: NskContracts.PaxFare = null;
        let paxFare2: NskContracts.PaxFare = null;
        for (let index = 0; index < fare.paxFares.length; index++) {
            const paxFare3 = fare.paxFares[index];
            if (paxFare3.paxType === paxType && paxFare3.paxDiscountCode === paxDiscountCode) {
                paxFare1 = paxFare3;
            }
            if (paxFare3.paxType == "ADT") {
                paxFare2 = paxFare3;
            }
        }
        if (!paxFare1) {
            paxFare1 = paxFare2;
        }
        if (paxFare1) {
            return BookingCalculator.calculatePaxFare(calculationResult, paxFare1);
        }
        return calculatorResultResponse1;
    }

    private static isFareRange(calculationResult: NskContracts.CalculationResult): boolean {
        return calculationResult <= (NskContracts.CalculationResult.IncludedTravelTax | NskContracts.CalculationResult.ETicketFare) || BookingCalculator.isTotalRange(calculationResult);
    }

    private static isAOSFeeRange(calculationResult: NskContracts.CalculationResult): boolean {
        return calculationResult >= NskContracts.CalculationResult.PublishedAOSFare && calculationResult <= <NskContracts.CalculationResult>59 || BookingCalculator.isTotalRange(calculationResult);
    }
    private static isPassengerFeeRange(calculationResult: NskContracts.CalculationResult): boolean {
        return calculationResult >= NskContracts.CalculationResult.PublishedPassengerFee && calculationResult <= <NskContracts.CalculationResult>39 || BookingCalculator.isTotalRange(calculationResult);
    }
    private static isPaymentRange(calculationResult: NskContracts.CalculationResult): boolean {
        return calculationResult >= NskContracts.CalculationResult.NominalPayment && calculationResult <= <NskContracts.CalculationResult>89;
    }
    private static isDueRange(calculationResult: NskContracts.CalculationResult): boolean {
        return calculationResult >= NskContracts.CalculationResult.NominalBalanceDue && calculationResult <= <NskContracts.CalculationResult>99;
    }
    private static isTotalRange(calculationResult: NskContracts.CalculationResult): boolean {
        return calculationResult >= NskContracts.CalculationResult.Total && calculationResult <= NskContracts.CalculationResult.TotalToCollect;
    }
    private static isEMDRange(calculationResult: NskContracts.CalculationResult): boolean {
        return calculationResult >= NskContracts.CalculationResult.TotalEMDFee && calculationResult <= NskContracts.CalculationResult.TotalEMDFeeIssued;
    }

    private static calculatePayment(calculationResult: NskContracts.CalculationResult, payment: NskContracts.Payment): NskContracts.BookingCalculatorResultResponse {
        const calculatorResultResponse: NskContracts.BookingCalculatorResultResponse = { amount: 0, points: 0 };
        if (!payment || payment.status == NskContracts.BookingPaymentStatus.Declined) {
            return calculatorResultResponse;
        }
        if (payment.paymentMethodType == NskContracts.PaymentMethodType.Loyalty) {
            calculatorResultResponse.points = payment.paymentAmount;
        }
        else {
            calculatorResultResponse.amount = payment.paymentAmount;
        }
        switch (calculationResult) {
            case NskContracts.CalculationResult.NominalPayment:
            case NskContracts.CalculationResult.NominalBalanceDue:
                return calculatorResultResponse;
            case NskContracts.CalculationResult.TotalPayment:
            case NskContracts.CalculationResult.BalanceDue:
                return calculatorResultResponse;
            case NskContracts.CalculationResult.AuthorizedPayment:
            case NskContracts.CalculationResult.AuthorizedBalanceDue:
                if (payment.status == NskContracts.BookingPaymentStatus.Approved || payment.paymentAmount < 0)
                    return calculatorResultResponse;
                break;
            case NskContracts.CalculationResult.NewPayment:
                if (payment.status == NskContracts.BookingPaymentStatus.New)
                    return calculatorResultResponse;
                break;
            case NskContracts.CalculationResult.ApprovedPayment:
                if (payment.status == NskContracts.BookingPaymentStatus.Approved)
                    return calculatorResultResponse;
                break;
            case NskContracts.CalculationResult.DepositPaymentNonDeclined:
                if (payment.deposit)
                    return calculatorResultResponse;
                break;
            case NskContracts.CalculationResult.TotalDepositPaymentAmount:
                if (payment.deposit)
                    return calculatorResultResponse;
                break;
            case NskContracts.CalculationResult.TotalNonDepositPaymentAmount:
                if (!payment.deposit)
                    return calculatorResultResponse;
                break;
        }
        return { amount: 0, points: 0 };
    }

    static calculateServiceCharges(calculationResult: NskContracts.CalculationResult, serviceCharges: NskContracts.BookingServiceCharge[]): NskContracts.BookingCalculatorResultResponse {
        let calculatorResultResponse: NskContracts.BookingCalculatorResultResponse = { amount: 0, points: 0 };
        if (serviceCharges != null && serviceCharges.length > 0) {
            const chargeTypeMath: ChargeTypeMath[] = BookingCalculator.getChargeTypeMath(calculationResult);
            if (chargeTypeMath.length > 0) {
                calculatorResultResponse = BookingCalculator.filterAndSummarizeAmount(chargeTypeMath, serviceCharges);
            }
        }
        return calculatorResultResponse;
    }

    private static calculateForeignServiceCharges(calculationResult: NskContracts.CalculationResult, serviceCharges: NskContracts.BookingServiceCharge[]): NskContracts.BookingCalculatorResultResponse {
        let calculatorResultResponse: NskContracts.BookingCalculatorResultResponse = { amount: 0, points: 0 };
        if (serviceCharges != null && serviceCharges.length > 0) {
            const chargeTypeMath: ChargeTypeMath[] = BookingCalculator.getChargeTypeMath(calculationResult);
            if (chargeTypeMath.length > 0) {
                calculatorResultResponse = BookingCalculator.filterAndSummarizeForeignAmount(chargeTypeMath, serviceCharges);
            }
        }
        return calculatorResultResponse;
    }

    private static filterAndSummarizeAmount(math: ChargeTypeMath[], serviceCharges: NskContracts.BookingServiceCharge[]): NskContracts.BookingCalculatorResultResponse {
        const calculatorResultResponse: NskContracts.BookingCalculatorResultResponse = { amount: 0, points: 0 };
        for (let index1: number = 0; index1 < serviceCharges.length; ++index1) {
            let serviceCharge: NskContracts.BookingServiceCharge = serviceCharges[index1];
            // assume it's always not for delete'
            // if (serviceCharge.ActionStatusCode != "XX") {
            for (let index2: number = 0; index2 < math.length; ++index2) {
                let chargeTypeMath: ChargeTypeMath = math[index2];
                if (Utils.enumsEqual(chargeTypeMath.chargeType, serviceCharge.chargeType, NskContracts.ChargeType)
                    && !Utils.enumsEqual(chargeTypeMath.chargeType, NskContracts.ChargeType.FarePoints, NskContracts.ChargeType)
                    && !Utils.enumsEqual(chargeTypeMath.chargeType, NskContracts.ChargeType.DiscountPoints, NskContracts.ChargeType)
                    /*chargeTypeMath.chargeType, serviceCharge.chargeType && chargeTypeMath.chargeType != NskContracts.ChargeType.FarePoints && chargeTypeMath.chargeType != NskContracts.ChargeType.DiscountPoints*/) {
                    calculatorResultResponse.amount += serviceCharge.amount * chargeTypeMath.multiply;
                    break;
                }
                if (Utils.enumsEqual(chargeTypeMath.chargeType, serviceCharge.chargeType, NskContracts.ChargeType)
                    && (Utils.enumsEqual(chargeTypeMath.chargeType, NskContracts.ChargeType.FarePoints, NskContracts.ChargeType) || Utils.enumsEqual(chargeTypeMath.chargeType, NskContracts.ChargeType.DiscountPoints, NskContracts.ChargeType))
                    /*chargeTypeMath.chargeType == serviceCharge.chargeType && (chargeTypeMath.chargeType == NskContracts.ChargeType.FarePoints || chargeTypeMath.chargeType == NskContracts.ChargeType.DiscountPoints)*/) {
                    calculatorResultResponse.points += serviceCharge.amount * chargeTypeMath.multiply;
                    break;
                }
            }
            // }
        }
        return calculatorResultResponse;
    }

    private static filterAndSummarizeForeignAmount(math: ChargeTypeMath[], serviceCharges: NskContracts.BookingServiceCharge[]): NskContracts.BookingCalculatorResultResponse {
        const calculatorResultResponse: NskContracts.BookingCalculatorResultResponse = { amount: 0, points: 0 };
        let str: string = null;
        for (let index1: number = 0; index1 < serviceCharges.length; ++index1) {
            const serviceCharge: NskContracts.BookingServiceCharge = serviceCharges[index1];
            for (let index2: number = 0; index2 < math.length; ++index2) {
                let chargeTypeMath: ChargeTypeMath = math[index2];
                if (Utils.enumsEqual(chargeTypeMath.chargeType, serviceCharge.chargeType, NskContracts.ChargeType) && Utils.enumsEqual(serviceCharge.collectType, NskContracts.CollectType.SellerChargeable, NskContracts.CollectType)
                    /*chargeTypeMath.chargeType == serviceCharge.chargeType && serviceCharge.ActionStatusCode != "XX" && serviceCharge.collectType == CollectType.SellerChargeable*//*CollectType.Immediate*/) {
                    calculatorResultResponse.amount += serviceCharge.foreignAmount * chargeTypeMath.multiply;
                    if (str != null && serviceCharge.foreignCurrencyCode != str) {
                        throw new Error("Not all foreign amounts are in the same currency");
                    }
                    str = serviceCharge.foreignCurrencyCode;
                    break;
                }
            }
        }
        return calculatorResultResponse;
    }

    private static getChargeTypeMath(calculationResult: NskContracts.CalculationResult): ChargeTypeMath[] {
        let chargeTypeMathList: ChargeTypeMath[] = BookingCalculator._chargeTypeMathByResult[calculationResult];
        if (!chargeTypeMathList) {
            chargeTypeMathList = BookingCalculator.buildChargeTypeMath(calculationResult);
            BookingCalculator._chargeTypeMathByResult[calculationResult] = chargeTypeMathList;
        }
        return chargeTypeMathList;
    }

    private static buildChargeTypeMath(calculationResult: NskContracts.CalculationResult): ChargeTypeMath[] {
        const chargeTypeMathList: ChargeTypeMath[] = [];
        switch (calculationResult) {
            case NskContracts.CalculationResult.PublishedFare:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.FarePrice));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.FarePoints));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.ConnectionAdjustmentAmount));
                break;
            case NskContracts.CalculationResult.FareDiscount:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.Discount));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.PromotionDiscount));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.DiscountPoints));
                break;
            case NskContracts.CalculationResult.DiscountedFare:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.FarePrice));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.FarePoints));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.ConnectionAdjustmentAmount));
                chargeTypeMathList.push(new ChargeTypeMath(-1, NskContracts.ChargeType.Discount));
                chargeTypeMathList.push(new ChargeTypeMath(-1, NskContracts.ChargeType.PromotionDiscount));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.DiscountPoints));
                break;
            case NskContracts.CalculationResult.IncludedTravelTax:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.IncludedTax));
                break;
            case NskContracts.CalculationResult.IncludedTravelFee:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.IncludedTravelFee));
                break;
            case NskContracts.CalculationResult.RevenueFare:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.FarePrice));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.FarePoints));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.ConnectionAdjustmentAmount));
                chargeTypeMathList.push(new ChargeTypeMath(-1, NskContracts.ChargeType.Discount));
                chargeTypeMathList.push(new ChargeTypeMath(-1, NskContracts.ChargeType.PromotionDiscount));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.DiscountPoints));
                chargeTypeMathList.push(new ChargeTypeMath(-1, NskContracts.ChargeType.IncludedTax));
                chargeTypeMathList.push(new ChargeTypeMath(-1, NskContracts.ChargeType.IncludedTravelFee));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.FareSurcharge));
                break;
            case NskContracts.CalculationResult.AddedTravelTax:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.Tax));
                break;
            case NskContracts.CalculationResult.AddedTravelFee:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.TravelFee));
                break;
            case NskContracts.CalculationResult.TotalTravelTax:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.IncludedTax));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.Tax));
                break;
            case NskContracts.CalculationResult.TotalTravelFee:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.IncludedTravelFee));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.TravelFee));
                break;
            case NskContracts.CalculationResult.TotalTravelFeeAndTax:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.IncludedTax));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.IncludedTravelFee));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.Tax));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.TravelFee));
                break;
            case NskContracts.CalculationResult.TotalFare:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.FarePrice));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.FarePoints));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.ConnectionAdjustmentAmount));
                chargeTypeMathList.push(new ChargeTypeMath(-1, NskContracts.ChargeType.Discount));
                chargeTypeMathList.push(new ChargeTypeMath(-1, NskContracts.ChargeType.PromotionDiscount));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.DiscountPoints));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.Tax));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.TravelFee));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.FareSurcharge));
                break;
            case NskContracts.CalculationResult.AddedTravelTaxAndTravelFee:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.Tax));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.TravelFee));
                break;
            case NskContracts.CalculationResult.TotalFareSurcharge:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.FareSurcharge));
                break;
            case NskContracts.CalculationResult.FarePoints:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.FarePoints));
                break;
            case NskContracts.CalculationResult.RevenueFareWithoutSurcharges:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.FarePrice));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.ConnectionAdjustmentAmount));
                chargeTypeMathList.push(new ChargeTypeMath(-1, NskContracts.ChargeType.Discount));
                chargeTypeMathList.push(new ChargeTypeMath(-1, NskContracts.ChargeType.PromotionDiscount));
                chargeTypeMathList.push(new ChargeTypeMath(-1, NskContracts.ChargeType.IncludedTax));
                chargeTypeMathList.push(new ChargeTypeMath(-1, NskContracts.ChargeType.IncludedTravelFee));
                break;
            case NskContracts.CalculationResult.ETicketFare:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.FarePrice));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.ConnectionAdjustmentAmount));
                chargeTypeMathList.push(new ChargeTypeMath(-1, NskContracts.ChargeType.Discount));
                chargeTypeMathList.push(new ChargeTypeMath(-1, NskContracts.ChargeType.PromotionDiscount));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.Tax));
                break;
            case NskContracts.CalculationResult.DiscountedFareWithSurcharges:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.FarePrice));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.FarePoints));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.ConnectionAdjustmentAmount));
                chargeTypeMathList.push(new ChargeTypeMath(-1, NskContracts.ChargeType.Discount));
                chargeTypeMathList.push(new ChargeTypeMath(-1, NskContracts.ChargeType.PromotionDiscount));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.DiscountPoints));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.FareSurcharge));
                break;
            case NskContracts.CalculationResult.PublishedPassengerFee:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.ServiceCharge));
                break;
            case NskContracts.CalculationResult.PassengerFeeDiscount:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.Discount));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.PromotionDiscount));
                break;
            case NskContracts.CalculationResult.DiscountedPassengerFee:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.ServiceCharge));
                chargeTypeMathList.push(new ChargeTypeMath(-1, NskContracts.ChargeType.Discount));
                chargeTypeMathList.push(new ChargeTypeMath(-1, NskContracts.ChargeType.PromotionDiscount));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.DiscountPoints));
                break;
            case NskContracts.CalculationResult.PassengerFeeIncludedTax:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.IncludedTax));
                break;
            case NskContracts.CalculationResult.RevenuePassengerFee:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.ServiceCharge));
                chargeTypeMathList.push(new ChargeTypeMath(-1, NskContracts.ChargeType.Discount));
                chargeTypeMathList.push(new ChargeTypeMath(-1, NskContracts.ChargeType.PromotionDiscount));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.DiscountPoints));
                chargeTypeMathList.push(new ChargeTypeMath(-1, NskContracts.ChargeType.IncludedTax));
                break;
            case NskContracts.CalculationResult.AddedPassengerFeeTax:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.Tax));
                break;
            case NskContracts.CalculationResult.TotalPassengerFeeTax:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.IncludedTax));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.Tax));
                break;
            case NskContracts.CalculationResult.TotalPassengerFee:
            case NskContracts.CalculationResult.TotalNewPassengerFee:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.ServiceCharge));
                chargeTypeMathList.push(new ChargeTypeMath(-1, NskContracts.ChargeType.Discount));
                chargeTypeMathList.push(new ChargeTypeMath(-1, NskContracts.ChargeType.PromotionDiscount));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.DiscountPoints));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.Tax));
                break;
            case NskContracts.CalculationResult.ConnectionAdjustmentAmount:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.ConnectionAdjustmentAmount));
                break;
            case NskContracts.CalculationResult.DiscountPoints:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.DiscountPoints));
                break;
            case NskContracts.CalculationResult.PublishedAOSFare:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.AddOnServicePrice));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.AddOnServiceMarkup));
                break;
            case NskContracts.CalculationResult.IncludedAOSFee:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.IncludedAddOnServiceFee));
                break;
            case NskContracts.CalculationResult.IncludedAOSTax:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.IncludedTax));
                break;
            case NskContracts.CalculationResult.RevenueAOSFare:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.AddOnServicePrice));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.AddOnServiceMarkup));
                chargeTypeMathList.push(new ChargeTypeMath(-1, NskContracts.ChargeType.IncludedTax));
                chargeTypeMathList.push(new ChargeTypeMath(-1, NskContracts.ChargeType.IncludedAddOnServiceFee));
                break;
            case NskContracts.CalculationResult.AddedAOSTax:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.Tax));
                break;
            case NskContracts.CalculationResult.TotalAOSTax:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.Tax));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.IncludedTax));
                break;
            case NskContracts.CalculationResult.TotalAOSFee:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.IncludedAddOnServiceFee));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.AddOnServiceFee));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.AddOnServiceCancelFee));
                break;
            case NskContracts.CalculationResult.TotalAOSFeeAndTax:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.IncludedAddOnServiceFee));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.AddOnServiceFee));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.Tax));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.IncludedTax));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.AddOnServiceCancelFee));
                break;
            case NskContracts.CalculationResult.TotalAOSFare:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.AddOnServicePrice));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.AddOnServiceMarkup));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.Tax));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.AddOnServiceFee));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.AddOnServiceCancelFee));
                break;
            case NskContracts.CalculationResult.TotalAOSHeldAmmount:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.AddOnServicePrice));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.AddOnServiceMarkup));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.Tax));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.AddOnServiceFee));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.AddOnServiceCancelFee));
                break;
            case NskContracts.CalculationResult.TotalAOSChargedAmount:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.AddOnServicePrice));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.AddOnServiceMarkup));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.Tax));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.AddOnServiceFee));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.AddOnServiceCancelFee));
                break;
            case NskContracts.CalculationResult.Total:
            case NskContracts.CalculationResult.TotalCharged:
            case NskContracts.CalculationResult.TotalToCollect:
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.FarePrice));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.FarePoints));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.AddOnServicePrice));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.AddOnServiceMarkup));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.ConnectionAdjustmentAmount));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.ServiceCharge));
                chargeTypeMathList.push(new ChargeTypeMath(-1, NskContracts.ChargeType.Discount));
                chargeTypeMathList.push(new ChargeTypeMath(-1, NskContracts.ChargeType.PromotionDiscount));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.DiscountPoints));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.TravelFee));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.AddOnServiceFee));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.AddOnServiceCancelFee));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.Tax));
                chargeTypeMathList.push(new ChargeTypeMath(1, NskContracts.ChargeType.FareSurcharge));
                break;
        }
        return chargeTypeMathList;
    }
}

interface ChargeTypeMathResult {
    [key: number]: ChargeTypeMath[];
}

class ChargeTypeMath {

    private _multiply: number;
    private _chargeType: NskContracts.ChargeType;

    constructor(m: number, ch: NskContracts.ChargeType) {
        this._multiply = m;
        this._chargeType = ch;
    }

    get multiply() {
        return this._multiply;
    }

    get chargeType() {
        return this._chargeType;
    }
}