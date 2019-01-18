import { Contracts as NskContracts } from 'newskies-services';
import { SSRSegment, AvailablePaxSSR, PaxSSRPrice } from "../Contracts";
import TripTranslator from "./TripTranslator";

export default class SsrAvailabilityTranslator {

    static translate(nskSegments: NskContracts.SSRSegment[]): SSRSegment[] {
        if (!nskSegments || !nskSegments.length) {
            return [];
        }
        return nskSegments.map(s => this.translateSegment(s)).filter(s => s);
    }

    private static translateSegment(nskSegment: NskContracts.SSRSegment): SSRSegment {
        if (!nskSegment) {
            return null;
        }
        return {
            legKey: nskSegment.legKey,
            availablePaxSSRList: nskSegment.availablePaxSSRList ? nskSegment.availablePaxSSRList.map(ssr => this.translateAvailablePaxSsr(ssr)).filter(ssr=>ssr) : []
        };
    }

    private static translateAvailablePaxSsr(nskAailPaxSsr: NskContracts.AvailablePaxSSR): AvailablePaxSSR {
        if (!nskAailPaxSsr) {
            return null;
        }
        return {
            ssrCode: nskAailPaxSsr.ssrCode,
            inventoryControlled: nskAailPaxSsr.inventoryControlled,
            seatDependent: nskAailPaxSsr.seatDependent,
            // nonSeatDependent: nskAailPaxSsr,
            available: nskAailPaxSsr.available,
            passengerNumberList: nskAailPaxSsr.passengerNumberList,
            paxSSRPriceList: nskAailPaxSsr.paxSSRPriceList ? nskAailPaxSsr.paxSSRPriceList.map(p => this.translatePaxSsrPrice(p, nskAailPaxSsr.ssrCode)).filter(p => p) : [],
            ssrLegList: nskAailPaxSsr.ssrLegList,
        }
    }

    private static translatePaxSsrPrice(nskPaxSsrPrice: NskContracts.PaxSSRPrice, ssrCode: string): PaxSSRPrice {
        if (!nskPaxSsrPrice) {
            return null;
        }
        const paxFee = TripTranslator.translatePaxFee(nskPaxSsrPrice.paxFee);
        paxFee.ssrCode = ssrCode;
        return {
            passengerNumberList: nskPaxSsrPrice.passengerNumberList,
            paxFee
        };
    }
}