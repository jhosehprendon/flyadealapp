import _ from 'underscore';
import { BookingCalculator, Contracts as NskContracts, Utils as NskUtils } from 'newskies-services';
import Utils from "../Services/Utils";
import { Trip, FlightDate, Flight, AvailabilityFare, AvailabilityPaxFare, PaxFee, PaxSsr, TripAvailabilityResponse } from "../Contracts";

export default class TripTranslator {

    static translateTripAvailabilityResponse(response: NskContracts.TripAvailabilityResponse): TripAvailabilityResponse {
        if (!response || !response.schedules) {
            return { trips: [], currencyCode: null };
        }
        const trips = response.schedules.map(sch => this.translateTrip(sch, response.fares));
        return { trips, currencyCode: null };
    }

    static translateTrip(journeyDateMarkets: NskContracts.JourneyDateMarket[], fares: NskContracts.Fare[]): Trip {
        if (!journeyDateMarkets || !journeyDateMarkets.length) {
            return {
                departureStation: null,
                arrivalStation: null,
                flightDates: []
            };
        }
        const journeyDateMarket = journeyDateMarkets[0];
        const trip: Trip = {
            departureStation: journeyDateMarket.departureStation,
            arrivalStation: journeyDateMarket.arrivalStation,
            flightDates: this.translateFlightDates(journeyDateMarkets, fares)
        };
        return trip;
    }

    static translateFlightDates(journeyDateMarkets: NskContracts.JourneyDateMarket[], fares: NskContracts.Fare[]): FlightDate[] {
        if (!journeyDateMarkets) {
            return null;
        }
        const flightDates: FlightDate[] = [];
        for (const journeyDateMarket of journeyDateMarkets) {
            const flightDate: FlightDate = {
                flightCount: journeyDateMarket.journeys.length,
                departureDate: journeyDateMarket.departureDate,
                flights: this.translateFlights(journeyDateMarket.journeys, fares)
            };
            flightDates.push(flightDate);
        }
        return flightDates;
    }

    static translateFlights(journeys: NskContracts.Journey[], fares?: NskContracts.Fare[], passengers?: NskContracts.Passenger[]): Flight[] {
        const flights: Flight[] = [];
        for (const journey of journeys) {
            const firstSegment = journey.segments[0];
            const lastSegment = journey.segments[journey.segments.length - 1];
            const flightFares = this.getJourneyFares(journey, fares);
            const flight: Flight = {
                sellKey: journey.journeySellKey,
                stops: journey.segments.length - 1,
                origin: firstSegment.departureStation,
                destination: lastSegment.arrivalStation,
                std: firstSegment.std,
                sta: lastSegment.sta,
                carrierCode: firstSegment.flightDesignator.carrierCode,
                flightNumber: firstSegment.flightDesignator.flightNumber,
                isInternational: journey.segments.some(s => s.international),
                cabinOfService: firstSegment.cabinOfService,
                hasSumOfSectorFares: flightFares.some(f => !NskUtils.enumsEqual(f.fareApplicationType, NskContracts.FareApplicationType.Route, NskContracts.FareApplicationType)),
                travelTimeMins: Utils.getTravelTimeMins(journey),
                journeyFlightType: Utils.getJourneyType(journey),
                legs: this.getLegs(journey),
                fares: []
            };
            if (passengers && passengers.length) {
                flight.paxFees = this.translateJourneyPaxFees(journey, passengers);
                flight.paxSsrs = this.translateJourneySsrs(journey);
            }
            this.translateFares(journey, flight, fares);
            flights.push(flight);
        }
        return flights;
    }

    static translateJourneyPaxFees(journey: NskContracts.Journey, passengers: NskContracts.Passenger[]) {
        let resultFees: PaxFee[] = [];
        for (const passenger of passengers) {
            const paxFees: PaxFee[] = passenger.passengerFees
                .filter(fee => {
                    if (NskUtils.enumsEqual(fee.feeApplicationType, NskContracts.FeeApplicationType.Segment, NskContracts.FeeApplicationType)) {
                        return journey.segments.some(s => fee.flightReference.indexOf(`${s.flightDesignator.carrierCode}${s.flightDesignator.flightNumber} ${s.departureStation}${s.arrivalStation}`) > -1);
                    }
                    if (NskUtils.enumsEqual(fee.feeApplicationType, NskContracts.FeeApplicationType.Leg, NskContracts.FeeApplicationType)) {
                        return journey.segments.some(s => s.legs.some(leg => fee.flightReference.indexOf(`${leg.flightDesignator.carrierCode}${leg.flightDesignator.flightNumber} ${leg.departureStation}${leg.arrivalStation}`) > -1));
                    }
                    return false;
                })
                .map(pf => this.translatePaxFee(pf, passenger.passengerNumber));
            resultFees = [...resultFees, ...paxFees];
        }
        return resultFees;
    }

    static translatePaxFee(fee: NskContracts.PassengerFee, passengerNumber: number = -1): PaxFee {
        const paxFee: PaxFee = _.omit(fee, 'feeOverride', 'serviceCharges', 'createdDate', 'isProtected', 'actionStatusCode', 'paymentNumber');
        paxFee.amount = BookingCalculator.calculatePassengerFee(NskContracts.CalculationResult.TotalPassengerFee, fee).amount;
        paxFee.feeTax = BookingCalculator.calculatePassengerFee(NskContracts.CalculationResult.TotalPassengerFeeTax, fee).amount;
        paxFee.feeDiscount = BookingCalculator.calculatePassengerFee(NskContracts.CalculationResult.PassengerFeeDiscount, fee).amount;
        paxFee.passengerNumber = passengerNumber;
        return paxFee;
    }

    private static translateJourneySsrs(journey: NskContracts.Journey) {
        return journey.segments.mapMany(s => s.paxSSRs).map(ssr => {
            const paxSsr: PaxSsr = {
                ssrCode: ssr.ssrCode,
                ssrNumber: ssr.ssrNumber,
                feeCode: ssr.feeCode,
                passengerNumber: ssr.passengerNumber,
                note: ssr.note
            };
            return paxSsr;
        });
        //let resultSsrs: PaxSsr[] = [];
        //for (const segment of journey.segments) {
        //    const paxSsrs: PaxSsr[] = segment.paxSSRs.map(ssr => {
        //        const paxSsr: PaxSsr = {
        //            ssrCode: ssr.ssrCode,
        //            ssrNumber: ssr.ssrNumber,
        //            feeCode: ssr.feeCode,
        //            passengerNumber: ssr.passengerNumber,
        //            note: ssr.note
        //        };
        //        return paxSsr;
        //    });
        //    resultSsrs = [...resultSsrs, ...paxSsrs];
        //}
        //return resultSsrs;
    }

    private static getSegmentFares(segment: NskContracts.Segment, allFares?: NskContracts.Fare[]): NskContracts.Fare[] {
        // if segment.fares already populated (when we translate price summary),
        // then return segment.fares
        if (!allFares && segment.fares && segment.fares.length) {
            return segment.fares;
        }
        const indexes = segment.availableFares.map(s => s.fareIndex);
        const distinctIndexes = indexes.reduce((x, y) => x.includes(y) ? x : [...x, y], []);
        return allFares.filter(f => distinctIndexes.some(i => i === allFares.indexOf(f)));
    }

    private static getJourneyFares(journey: NskContracts.Journey, allFares?: NskContracts.Fare[]): NskContracts.Fare[] {
        if (!allFares) {
            return journey.segments.mapMany(seg => seg.fares);
            // return journey.segments.map(s => s.fares.map(f => f)).reduce((fares, fares1) => fares.concat(fares1));
        }
        const allIndexes = journey.segments.mapMany(s => s.availableFares.map(af => af.fareIndex));
        // const allIndexes = journey.segments.map(s => s.availableFares.map(af => af.fareIndex)).reduce((arr1, arr2) => arr1.concat(arr2));
        const distinctIndexes = allIndexes.reduce((x, y) => x.includes(y) ? x : [...x, y], []);
        return allFares.filter(f => distinctIndexes.some(i => i === allFares.indexOf(f)));
    }

    static getLegs(journey: NskContracts.Journey): NskContracts.Leg[] {
        const legs: NskContracts.Leg[] = [];
        if (journey && journey.segments && journey.segments.length) {
            journey.segments.forEach(segment => {
                segment.legs.forEach(leg => legs.push(leg));
            });
        }
        return legs;
    }

    static translateFares(journey: NskContracts.Journey, flight: Flight, fares?: NskContracts.Fare[]) {
        if (!journey || !journey.segments || !journey.segments.length) {
            return;
        }
        const segment = journey.segments[0];
        segment.fares = this.getSegmentFares(segment, fares);
        for (let fareIndex = 0; fareIndex < segment.fares.length; fareIndex++) {
            const list = journey.segments.map(s => s.fares[fareIndex]);
            const fare = !list.some(f => Utils.isGoverningFare(f)) ? segment.fares[fareIndex] : list.find(f => Utils.isGoverningFare(f));
            const availFare: AvailabilityFare = {
                sellKey: fare.fareSellKey,
                carrierCode: fare.carrierCode,
                classOfService: fare.classOfService,
                classType: fare.classType,
                fareApplicationType: fare.fareApplicationType,
                fareBasisCode: fare.fareBasisCode,
                fareClassOfService: fare.fareClassOfService,
                fareSequence: fare.fareSequence,
                fareStatus: fare.fareStatus,
                inboundOutbound: fare.inboundOutbound,
                isAllotmentMarketFare: fare.isAllotmentMarketFare,
                originalClassOfService: fare.originalClassOfService,
                productClass: fare.productClass,
                travelClassCode: fare.travelClassCode,
                xrefClassOfService: fare.xrefClassOfService,
                isSumOfSector: false,
                paxFares: []
            };
            if (segment.availableFares && segment.availableFares.length > fareIndex) {
                availFare.availableCount = segment.availableFares[fareIndex].availableCount;
            }
            if (!NskUtils.enumsEqual(fare.fareApplicationType, NskContracts.FareApplicationType.Route, NskContracts.FareApplicationType)) {
                this.translateFareBySumOfSector(journey, fareIndex, availFare, flight);
            } else {
                this.translateFareByRoute(journey, availFare, flight, fare);
            }
        }
    }

    static translateFareByRoute(journey: NskContracts.Journey, availFare: AvailabilityFare, flight: Flight, fare: NskContracts.Fare): void {
        if (!journey || !availFare || !flight || !fare) {
            return;
        }
        this.translatePassengerFareByRoute(availFare, fare);
        flight.fares.push(availFare);
    }

    static translatePassengerFareByRoute(availFare: AvailabilityFare, fare: NskContracts.Fare): void {
        if (!availFare || !fare) {
            return;
        }
        fare.paxFares.forEach(paxFare => {
            const revenuePaxFareResult = BookingCalculator.calculateFarePaxTypeDiscount(NskContracts.CalculationResult.RevenueFare, fare, paxFare.paxType, paxFare.paxDiscountCode);
            const publishedPaxFareResult = BookingCalculator.calculateFarePaxTypeDiscount(NskContracts.CalculationResult.PublishedFare, fare, paxFare.paxType, paxFare.paxDiscountCode);
            const totalPaxFareResult = BookingCalculator.calculateFarePaxTypeDiscount(NskContracts.CalculationResult.Total, fare, paxFare.paxType, paxFare.paxDiscountCode);
            const discountedPaxFareResult = BookingCalculator.calculateFarePaxTypeDiscount(NskContracts.CalculationResult.DiscountedFare, fare, paxFare.paxType, paxFare.paxDiscountCode);
            const passengerFare: AvailabilityPaxFare = {
                paxType: paxFare.paxType,
                paxDiscountCode: paxFare.paxDiscountCode,
                fareDiscountCode: paxFare.fareDiscountCode,
                ticketFareBasisCode: paxFare.ticketFareBasisCode,
                totalFare: totalPaxFareResult.amount,
                discountedFare: discountedPaxFareResult.amount,
                publishedFare: publishedPaxFareResult.amount,
                revenueFare: revenuePaxFareResult.amount,
                isDiscount: false,
            };
            if (passengerFare.totalFare < passengerFare.publishedFare) {
                passengerFare.isDiscount = true;
            }
            const totalTravelFeesAndTaxesResult = BookingCalculator.calculateFarePaxTypeDiscount(NskContracts.CalculationResult.TotalTravelFeeAndTax, fare, paxFare.paxType, paxFare.paxDiscountCode);
            if (totalTravelFeesAndTaxesResult && totalTravelFeesAndTaxesResult.amount) {
                passengerFare.fees = [];
                paxFare.serviceCharges.forEach(sc => {
                    const chargeFeeAndTaxResult = BookingCalculator.calculateServiceCharges(NskContracts.CalculationResult.TotalTravelFeeAndTax, [sc]);
                    if (!chargeFeeAndTaxResult || !chargeFeeAndTaxResult.amount) {
                        return;
                    }
                    passengerFare.fees.push({
                        amount: chargeFeeAndTaxResult.amount,
                        code: sc.chargeCode
                    });
                });
            }
            availFare.paxFares.push(passengerFare);
        });
    }

    static translateFareBySumOfSector(journey: NskContracts.Journey, fareIndex: number, availFare: AvailabilityFare, flight: Flight): void {
        if (!journey) {
            return;
        }
        const fares: NskContracts.Fare[] = journey.segments.map(segment => segment.fares[fareIndex]);
        availFare.sellKey = this.buildSumOfSectorFareSellKey(fares);
        availFare.isSumOfSector = true;
        this.translatePassengerFaresWithSumOfSector(availFare, fares);
        flight.fares.push(availFare);
    }

    static buildSumOfSectorFareSellKey(sosFares: NskContracts.Fare[]): string {
        let key = '';
        if (sosFares && sosFares.length) {
            for (let index: number = 0; index < sosFares.length; ++index) {
                const fare: NskContracts.Fare = sosFares[index];
                key = index == 0 ? fare.fareSellKey : key + '^' + fare.fareSellKey;
            }
        }
        return key;
    }

    static translatePassengerFaresWithSumOfSector(availFare: AvailabilityFare, sosFares: NskContracts.Fare[]): void {
        if (!availFare || !sosFares || !sosFares.length) {
            return;
        }
        const fare: NskContracts.Fare = !sosFares.some(f => Utils.isGoverningFare(f)) ? sosFares[0] : sosFares.find(f => Utils.isGoverningFare(f));
        if (!fare) {
            return;
        }
        fare.paxFares.forEach(paxFare => {
            const revenuePaxFareResult = BookingCalculator.calculateFarePaxTypeDiscount(NskContracts.CalculationResult.RevenueFare, fare, paxFare.paxType, paxFare.paxDiscountCode);
            const publishedPaxFareResult = BookingCalculator.calculateFarePaxTypeDiscount(NskContracts.CalculationResult.PublishedFare, fare, paxFare.paxType, paxFare.paxDiscountCode);
            const totalPaxFareResult = BookingCalculator.calculateFarePaxTypeDiscount(NskContracts.CalculationResult.Total, fare, paxFare.paxType, paxFare.paxDiscountCode);
            const discountedPaxFareResult = BookingCalculator.calculateFarePaxTypeDiscount(NskContracts.CalculationResult.DiscountedFare, fare, paxFare.paxType, paxFare.paxDiscountCode);
            const passengerFare: AvailabilityPaxFare = {
                paxType: paxFare.paxType,
                paxDiscountCode: paxFare.paxDiscountCode,
                fareDiscountCode: paxFare.fareDiscountCode,
                ticketFareBasisCode: paxFare.ticketFareBasisCode,
                totalFare: totalPaxFareResult.amount,
                discountedFare: discountedPaxFareResult.amount,
                publishedFare: publishedPaxFareResult.amount,
                revenueFare: revenuePaxFareResult.amount,
                isDiscount: false
            };

            this.sumUpSumOfSectorAmounts(sosFares, passengerFare);
            if (passengerFare.totalFare < passengerFare.publishedFare) {
                passengerFare.isDiscount = true;
            }
            availFare.paxFares.push(passengerFare);
        });
    }

    static sumUpSumOfSectorAmounts(sosFares: NskContracts.Fare[], passengerFare: AvailabilityPaxFare): void {
        if (!sosFares || !sosFares.length || !passengerFare) {
            return;
        }
        passengerFare.totalFare = 0;
        passengerFare.publishedFare = 0;
        passengerFare.revenueFare = 0;
        //passengerFare.LoyaltyPoints = 0;
        passengerFare.discountedFare = 0;
        sosFares.forEach(sosFare => {
            sosFare.paxFares.forEach(paxFare => {
                if (paxFare.paxType == passengerFare.paxType) {
                    passengerFare.totalFare += BookingCalculator.calculateFarePaxTypeDiscount(NskContracts.CalculationResult.Total, sosFare, paxFare.paxType, paxFare.paxDiscountCode).amount;
                    passengerFare.publishedFare += BookingCalculator.calculateFarePaxTypeDiscount(NskContracts.CalculationResult.PublishedFare, sosFare, paxFare.paxType, paxFare.paxDiscountCode).amount;
                    passengerFare.revenueFare += BookingCalculator.calculateFarePaxTypeDiscount(NskContracts.CalculationResult.RevenueFare, sosFare, paxFare.paxType, paxFare.paxDiscountCode).amount;
                    // passengerFare.LoyaltyPoints += BookingCalculator.Calculate(CalculationResult.FarePoints, paxFare).Points;
                    passengerFare.discountedFare += BookingCalculator.calculateFarePaxTypeDiscount(NskContracts.CalculationResult.DiscountedFare, sosFare, paxFare.paxType, paxFare.paxDiscountCode).amount;
                }
            });
        });
    }
}