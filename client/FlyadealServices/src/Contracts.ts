import { Contracts as NskContracts } from 'newskies-services';
import { Moment } from "moment/moment";

export interface FlyadealClientConfig {
    sessionStorage: NskContracts.SessionStorage,
    localStorage: LocalStorage,
    sessionTokenHeader: string,
    baseUrl: string,
    defaultCulture?: string;
    resourcesSettings: ResourcesSettings;
}

export interface ResourcesSettings {
    paxTypes: PaxTypeConfig[];
}

export interface PaxTypeConfig {
    paxType: string;
    titles: string[];
    minAgeYears: number;
    maxAgeYears: number;
}

export interface LocalStorage extends NskContracts.SessionStorage { };

export interface TripAvailabilityRequest {
    adults: number;
    children: number;
    infants: number;
    //assign automatically based on business rules
    // currencyCode: string;
    paxResidentCountry: string;
    flights: AvailabilityRequest[];
}

export interface AvailabilityRequest {
    departureStation: string;
    arrivalStation: string;
    beginDate: string;
    endDate: string;
    promotionCode?: string;
}

export interface TripAvailabilityResponse {
    trips: Trip[];
    currencyCode: string;
}

export interface Trip {
    departureStation: string;
    arrivalStation: string;
    //includeTaxesAndFees: boolean;
    flightDates: FlightDate[];
    // FromSellKey = fromSellKey,
    // FlyAheadStatus = flyAheadOfferStatus
}

export interface FlightDate {
    flightCount: number;
    departureDate: string;
    flights: Flight[];
}

export interface Flight {
    sellKey: string;
    origin: string;
    destination: string;
    std: string;
    sta: string;
    carrierCode: string;
    flightNumber: string;
    stops: number;
    isInternational: boolean;
    cabinOfService: string;
    hasSumOfSectorFares: boolean;
    travelTimeMins: number;
    legs: NskContracts.Leg[];
    fares: AvailabilityFare[];
    journeyFlightType: FlightType;
    paxFees?: PaxFee[];
    paxSsrs?: PaxSsr[];
}

export interface PaxSsr {
    ssrCode: string,
    ssrNumber: number;
    passengerNumber: number;
    feeCode: string;
    note: string;
}

export interface PaxFee {
    // actionStatusCode: string;
    feeCode: string;
    feeDetail: string;
    feeNumber: number;
    feeType: NskContracts.FeeType;
    // feeOverride: boolean;
    flightReference: string;
    note: string;
    ssrCode?: string;
    ssrNumber?: number;
    // paymentNumber: number;
    // replace
    amount: number;
    feeTax: number;
    feeDiscount: number;
    // serviceCharges: NskContracts.BookingServiceCharge[];
    // createdDate: string;
    // isProtected: boolean;
    feeApplicationType: NskContracts.FeeApplicationType;

    passengerNumber: number;
}

export interface AvailabilityFare {
    sellKey: string;
    carrierCode: string;
    classOfService: string;
    classType: string;
    fareApplicationType: NskContracts.FareApplicationType;
    fareBasisCode: string;
    fareClassOfService: string;
    fareSequence: number;
    fareStatus: NskContracts.FareStatus;
    inboundOutbound: NskContracts.InboundOutbound;
    isAllotmentMarketFare: boolean;
    originalClassOfService: string;
    productClass: string;
    travelClassCode: string;
    xrefClassOfService: string;
    paxFares: AvailabilityPaxFare[];
    isSumOfSector: boolean;
    availableCount?: number;
}

export enum FlightType {
    None = 0,
    NonStop = 1,
    Through = 2,
    Direct = 3,
    Connect = 4,
    All = 7,
}

export interface AvailabilityPaxFare {
    paxType: string;
    paxDiscountCode: string;
    fareDiscountCode: string;
    ticketFareBasisCode: string;
    // below calculated properties
    totalFare: number;
    discountedFare: number;
    publishedFare: number;
    revenueFare: number;
    isDiscount: boolean;
    fees?: FareFee[];
}

export interface FareFee {
    amount: number;
    code: string;
}

export interface PriceItinerary {
    currencyCode: string;
    flights: Flight[];
    bookingSum: NskContracts.BookingSum;
}

export interface Booking {
    recordLocator: string;
    currencyCode: string;
    // paxCount: number;
    // bookingID: number;
    bookingInfo: NskContracts.BookingInfo;
    pos: NskContracts.PointOfSale;
    sourcePOS: NskContracts.PointOfSale;
    bookingHold: NskContracts.BookingHold;
    bookingSum: NskContracts.BookingSum;
    passengers: Passenger[];
    journeys: NskContracts.Journey[];
    bookingComments: NskContracts.BookingComment[];
    bookingContacts: BookingContact[];
    payments: NskContracts.Payment[];
    state: NskContracts.BookingState;
}

export interface Passenger {
    dateOfBirth?: Moment,
    discountCode?: string,
    firstName: string,
    middleName: string,
    //frequentFlyer: {
    //    carrierCode: XY,
    //    docNumber: 8877665544,
    //    docTypeCode: OAFF
    //},
    lastName: string,
    passengerNumber: number,
    paxType: string,
    nationality: string,
    title: string,
    residentCountry: string,
    gender: NskContracts.Gender,
    travelDocument?: PassengerTravelDocument,
    // fees that are not related to any journey/segment/leg
    otherFees?: PaxFee[]
}

export interface PassengerTravelDocument {
    birthCountry: string,
    docNumber: string,
    docTypeCode: string,
    docIssuingCountry: string,
    issuedDate?: Moment,
    expirationDate?: Moment,
    nationality: string,
    docSuffix: string,
}

export interface BookingContact {
    typeCode: string;
    name: NskContracts.BookingName;
    emailAddress: string;
    homePhone: Phone;
    workPhone: Phone;
    otherPhone: Phone;
    fax: string;
    companyName: string;
    addressLine1: string;
    addressLine2: string;
    addressLine3: string;
    city: string;
    provinceState: string;
    postalCode: string;
    countryCode: string;
    cultureCode: string;
    customerNumber: string;
    sourceOrganization: string;
}

export interface Phone {
    countryCode: string;
    number: number;
}

export interface SSRSegment {
    legKey: NskContracts.LegKey;
    availablePaxSSRList: AvailablePaxSSR[];
}

export interface AvailablePaxSSR {
    ssrCode: string;
    inventoryControlled: boolean;
    // nonInventoryControlled: boolean;
    seatDependent: boolean;
    // nonSeatDependent: boolean;
    available: number;
    passengerNumberList: number[];
    paxSSRPriceList: PaxSSRPrice[];
    ssrLegList: NskContracts.SSRLeg[];
}

export interface PaxSSRPrice {
    paxFee: PaxFee;
    passengerNumberList: number[];
}

//export interface SeatAvailabilityResponse {
//    seatMaps: SeatMap[];
//}

export interface SeatMap {
    origin: string;
    destination: string;
    passengerSeatInfos: PassengerSeatInfo[];
    seatGroups: SeatGroup[];
}

export interface PassengerSeatInfo {
    passengerNumber: number;
    selectedSeat?: string;
    is12To15: boolean;
    passengerPriceGroups: PassengerPriceGroup[];
}

export interface PassengerPriceGroup {
    type: number;
    price: number;
}

export interface SeatGroup {
    type: number;
    seatGroupName: string;
    priceGroup: number;
    seats: Seat[];
}

export interface Seat {
    number: string;
    available: boolean;
    status: NskContracts.SeatAvailability;
    infantAllowed: boolean;
    window: boolean;
    femaleOnly: boolean;
    exitRow: boolean;
    childAllowed: boolean;
    youngAdultAllowed: boolean;
}

export interface RetrievedBooking {
    journeys: NskContracts.Journey[]
    passengers: NskContracts.Passenger[]
    bookingRetrieveInfo: NskContracts.BookingRetrieveInfo
}

export interface CheckInSelection {
    paxSegments: PaxSegmentCheckIn[];
}

export interface PaxSegmentCheckIn {
    journeyIndex: number;
    segmentIndex: number;
    passengerNumber: number;
}

export interface Person {
    personID: number;
    personType: NskContracts.PersonType;
    cultureCode: string;
    dob: Moment;
    gender: NskContracts.Gender;
    customerNumber: string;
    emailAddress: string;
    nationality: string;
    name: NskContracts.Name;
    phone: Phone;
    travelDocument?: PassengerTravelDocument;
    residentCountry: string;
    city: string;
}

export interface Agent {
    agentID: number;
    agentIdentifier: NskContracts.AgentIdentifier;
    loginName: string;
    status: NskContracts.AgentStatus;
    password: string;
    authenticationType: NskContracts.AuthenticationType;
    personID: number;
    departmentCode: string;
    locationCode: string;
    forcePasswordReset: boolean;
    locked: boolean;
    agentRoles: NskContracts.AgentRole[];
}

export interface Member {
    person: Person;
    agent: Agent;
}

export interface Organization {
    organizationCode: string;
    organizationType: NskContracts.OrganizationType;
    organizationName: string;
    status: NskContracts.OrganizationStatus;
    address: NskContracts.Address;
    url: string;
    phone: Phone;
    emailAddress: string;
    cultureCode: string;
    currencyCode: string;
    contactName: NskContracts.Name;
    contactPhone: Phone;
}

export interface Agency {
    organization: Organization;
    agent: Member;
}

export interface BoardingPass {
    barcodeHeight: number;
    barcodeWidth: number;
    barCodedBoardingPasses: NskContracts.BarcodedBoardingPass[];
}
