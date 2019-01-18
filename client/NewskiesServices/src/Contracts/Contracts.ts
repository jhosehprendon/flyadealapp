import { AxiosResponse, AxiosRequestConfig } from 'axios';

export type AjaxRequestInterceptorCallback = (value: AxiosRequestConfig) => Promise<AxiosRequestConfig> | AxiosRequestConfig;
export type AjaxResponseInterceptorCallback = (value: AxiosResponse) => Promise<AxiosResponse> | AxiosResponse;

export interface AjaxRequestInterceptor {
    requestId?: number;
    getRequestCallback: () => AjaxRequestInterceptorCallback;
    getRequestErrorCallback?: () => (error: any) => any;
}

export interface AjaxResponseInterceptor {
    responseId?: number;
    getResponseCallback: () => AjaxResponseInterceptorCallback;
    getResponseErrorCallback?: () => (error: any) => any;
}

export interface AjaxClientConfig {
    baseUrl?: string;
    requestInterceptors?: AjaxRequestInterceptor[];
    responseInterceptors?: AjaxResponseInterceptor[];
}

export enum ErrorType {
    UnhandledError = 1,
    ServerUnavaliable = 2,
    ConfigurationError = 3,
    RemovePaymentError = 9,
    AddPaymentError = 10,
    BookingCommitError = 11,
    BadData = 400,
    Unauthorized = 401,
    Forbidden = 403,
    InternalServer = 500
}

export interface NewskiesError extends Error {
    errorType: ErrorType,
    payload?: any
}

export interface SessionStorage {
    //readonly length: number;
    clear(): void;
    getItem(key: string): string | null;
    //key(index: number): string | null;
    removeItem(key: string): void;
    setItem(key: string, data: string): void;
    //[key: string]: any;
    //[index: number]: string;
}

export interface AllResourcesResponse {
    cultureList: Culture[];
    stationList: Station[];
    marketList: Market[];
    currencyList: Currency[];
    paxTypeList: PaxType[];
    feeList: Fee[];
    countryList: Country[];
    titleList: Title[];
    docTypeList: DocType[];
    ssrList: SSR[];
    paymentMethodList: PaymentMethod[];
}

export interface NewskiesClientConfig {
    ajax?: AjaxClientConfig;
}

export interface GetFeeListResponse {
    feeList: Fee[];
}

export interface Fee {
    allowed: boolean;
    feeCode: string;
    inActive: boolean;
    name: string;
    description: string;
    displayCode: string;
    feeType: FeeType;
}

export interface GetSSRListResponse {
    ssrList: SSR[];
}

export interface SSR {
    allowed: boolean;
    boardingZone: number;
    feeCode: string;
    inActive: boolean;
    limitPerPassenger: number;
    name: string;
    ruleSetName: string;
    seatMapCode: string;
    // seatRestriction: SeatRestriction;
    ssrCode: string;
    ssrNestCode: string;
    ssrType: SSRType;
    traceQueueCode: string;
    unitValue: number;
}

export enum SSRType {
    Standard = 0,
    Infant = 1,
    Meal = 2,
    BaggageAllowance = 3,
    TravelLineMeal = 4,
    Unmapped = -1
}

export interface Station {
    allowed: boolean;
    cityCode: string;
    countryCode: string;
    cultureCode: string;
    currencyCode: string;
    inActive: boolean;
    latitude: string;
    longitude: string;
    mACCode: string;
    name: string;
    provinceStateCode: string;
    shortName: string;
    stationClass: string;
    stationCode: string;
    timeZoneCode: string;
}

export interface GetStationListResponse {
    stationList: Station[];
}

export interface Market {
    locationCode: string;
    travelLocationCode: string;
    inActive: boolean;
    locationType: MarketLocationType;
    travelLocationType: MarketLocationType;
    includesTaxesAndFees: Directionality;
}

export enum Directionality {
    None = 0,
    To = 1,
    From = 2,
    Between = 3,
    Unmapped = -1
}

export enum MarketLocationType {
    Undefined = 0,
    Station = 1,
    MAC = 3,
    Unmapped = -1
}

export interface GetMarketListResponse {
    marketList: Market[];
}

export interface Culture {
    name: string;
    code: string;
}

export interface GetCultureListResponse {
    cultureList: Culture[];
}

export interface Currency {
    name: string;
    code: string;
    symbol: string;
}

export interface GetCountryListResponse {
    countryList: Country[];
}

export interface Country {
    countryCode: string;
    hasProvinceStates: boolean;
    name: string;
    inActive: boolean;
    countryCode3C: string;
    defaultCurrencyCode: string;
    phoneCode: string;
}

export interface GetTitleListResponse {
    titleList: Title[];
}

export interface Title {
    // allowed: boolean;
    titleKey: string;
    description: string;
    gender: Gender;
    weightCategory: WeightCategory;
}

export enum WeightCategory {
    Male = 0,
    Female = 1,
    Child = 2,
    Unmapped = -1
}

export interface GetDocTypeListResponse {
    docTypeList: DocType[];
}

export interface DocType {
    docTypeCode: string;
    typeName: string;
    inActive: boolean;
}

export interface GetCurrencyListResponse {
    currencyList: Currency[];
}

export interface PaxType {
    name: string;
    code: string;
}

export interface GetPaxTypeListResponse {
    paxTypeList: PaxType[];
}

export interface TripAvailabilityResponse {
    schedules: JourneyDateMarket[][];
    fares: Fare[];
    ssrPriceList: SSRPrice[];
}

export interface SSRPrice {
    ssrCode: string;
    priceVariationList: PriceVariation[];
}

export interface PriceVariation {
    basePrice: number;
    taxTotal: number;
    paxType: string;
    programLevel: string;
}

export interface JourneyDateMarket {
    departureDate: string;
    departureStation: string;
    arrivalStation: string;
    journeys: Journey[];
}

export interface Journey {
    segments: Segment[];
    journeySellKey: string;
}

export interface Segment {
    actionStatusCode: string;
    arrivalStation: string;
    cabinOfService: string;
    departureStation: string;
    segmentType: string;
    sta: string;
    std: string;
    international: boolean;
    flightDesignator: FlightDesignator;
    fares: Fare[];
    legs: Leg[];
    paxSeats: PaxSeat[];
    paxSSRs: PaxSSR[];
    paxSegments: PaxSegment[];
    availableFares?: AvailableFare2[];
}

export interface PaxSeat {
    passengerNumber: number;
    arrivalStation: string;
    departureStation: string;
    unitDesignator: string;
    compartmentDesignator: string;
    penalty: number;
    paxSeatInfo: PaxSeatInfo;
}

export interface PaxSeatInfo {
    seatSet: number;
    deck: number;
    properties: KeyValuePair<string, string>[];
}

export interface KeyValuePair<K, V> {
    key: K;
    value: V;
}

export interface AvailableFare2 {
    availableCount: number;
    status: ClassStatus;
    fareIndex: number;
    serviceBundleSetCode: string;
    serviceBundleOfferIndexes: number[];
}

export enum ClassStatus {
    Active = 0,
    AVSClosed = 4,
    AVSOnRequest = 3,
    AVSOpen = 2,
    InActive = 1
}

export interface FlightDesignator {
    carrierCode: string;
    flightNumber: string;
    opSuffix: string;
}

export interface Fare {
    classOfService: string;
    classType: string;
    ruleTariff: string;
    carrierCode: string;
    ruleNumber: string;
    fareBasisCode: string;
    fareSequence: number;
    fareClassOfService: string;
    fareStatus: FareStatus;
    fareApplicationType: FareApplicationType;
    originalClassOfService: string;
    xrefClassOfService: string;
    paxFares: PaxFare[];
    productClass: string;
    isAllotmentMarketFare: boolean;
    travelClassCode: string;
    fareSellKey: string;
    inboundOutbound: InboundOutbound;
    fareLink: number;
    fareDesignator: FareDesignator;
}

export enum FareStatus {
    Default = 0,
    SameDayStandBy = 1,
    FareOverrideConfirming = 2,
    FareOverrideConfirmed = 3,
    Unmapped = -1
}

export enum FareApplicationType {
    Route = 0,
    Sector = 1,
    Governing = 2,
    Unmapped = -1
}

export interface PaxFare {
    paxType: string;
    paxDiscountCode: string;
    fareDiscountCode: string;
    serviceCharges: BookingServiceCharge[];
    ticketFareBasisCode: string;
}

export interface BookingServiceCharge {
    chargeType: ChargeType;
    collectType: CollectType;
    chargeCode: string;
    ticketCode: string;
    currencyCode: string;
    amount: number;
    chargeDetail: string;
    foreignCurrencyCode: string;
    foreignAmount: number;
}

export enum ChargeType {
    FarePrice = 0,
    Discount = 1,
    IncludedTravelFee = 2,
    IncludedTax = 3,
    TravelFee = 4,
    Tax = 5,
    ServiceCharge = 6,
    PromotionDiscount = 7,
    ConnectionAdjustmentAmount = 8,
    AddOnServicePrice = 9,
    IncludedAddOnServiceFee = 10,
    AddOnServiceFee = 11,
    Calculated = 12,
    Note = 13,
    AddOnServiceMarkup = 14,
    FareSurcharge = 15,
    Loyalty = 16,
    FarePoints = 17,
    DiscountPoints = 18,
    AddOnServiceCancelFee = 19,
    Unmapped = -1
}

export enum CollectType {
    SellerChargeable = 0,
    ExternalChargeable = 1,
    SellerNonChargeable = 2,
    ExternalNonChargeable = 3,
    ExternalChargeableImmediate = 4,
    Unmapped = -1
}

export enum InboundOutbound {

    None = 0,
    Inbound = 1,
    Outbound = 2,
    Both = 3,
    RoundFrom = 4,
    RoundTo = 5,
    Unmapped = -1
}

export interface FareDesignator {
    fareTypeIndicator: string;
    cityCode: string;
    travelCityCode: string;
    ruleFareTypeCode: string;
    baseFareFareClassCode: string;
    dowType: string;
    seasonType: string;
    routingNumber: number;
    oneWayRoundTrip: string;
    openJawAllowed: boolean;
    tripDirection: string;
}

export interface Leg {
    arrivalStation: string;
    departureStation: string;
    sta: string;
    std: string;
    flightDesignator: FlightDesignator;
    legInfo: LegInfo;
}

export interface LegInfo {
    equipmentType: string;
    equipmentTypeSuffix: string;
    arrivalTerminal: string;
    codeShareIndicator: string;
    departureTerminal: string;
    eTicket: boolean;
    paxSTA: string;
    paxSTD: string;
    scheduleServiceType: string;
    operatingFlightNumber: string;
    operatedByText: string;
    operatingCarrier: string;
    operatingOpSuffix: string;
    subjectToGovtApproval: boolean;
    arrvLTV: number;
    deptLTV: number;
}

export interface PaxSSR {
    actionStatusCode: string;
    arrivalStation: string;
    departureStation: string;
    passengerNumber: number;
    ssrCode: string;
    ssrNumber: number;
    ssrDetail: string;
    feeCode: string;
    note: string;
    ssrValue: number;
    isInServiceBundle: boolean;
}

export interface PaxSegment {
    boardingSequence: string;
    createdDate: string;
    overBookIndicator: string;
    passengerNumber: number;
    priorityDate: string;
    tripType: TripType;
    timeChanged: boolean;
    verifiedTravelDocs: string;
    modifiedDate: string;
    activityDate: string;
    baggageAllowanceWeight: number;
    serviceBundleCode: string;
    checkInStatus?: PaxSegmentCheckInStatus;
    liftStatus: LiftStatus;
}

export enum TripType {
    None = 0,
    OneWay = 1,
    RoundTrip = 2,
    HalfRound = 3,
    OpenJaw = 4,
    CircleTrip = 5,
    All = 6,
    Unmapped = -1
}

export interface TripAvailabilityRequest {
    availabilityRequests: AvailabilityRequest[];
}

export interface AvailabilityRequest {
    departureStation: string;
    arrivalStation: string;
    beginDate: string;
    endDate: string;
    promotionCode?: string;
    paxTypeCounts: PaxTypeCount[];
    currencyCode: string;
    paxResidentCountry: string;
}

export interface PaxTypeCount {
    paxTypeCode: string;
    paxCount: number;
}

export interface SellJourneyByKeyRequestData {
    journeySellKeys: SellKeyList[];
    currencyCode: string;
    paxTypeCounts: PaxTypeCount[];
    typeOfSale: TypeOfSale;
}

export interface TypeOfSale {
    paxResidentCountry: string;
    promotionCode: string;
    fareTypes: string[];
}

export interface SellKeyList {
    journeySellKey: string;
    fareSellKey: string;
}

export interface ChangeFlightsRequest {
    journeySellKeys: SellKeyList[]
}

export interface SellResponse {
    bookingUpdateResponseData: BookingUpdateResponseData;
}

export interface CancelResponse {
    bookingUpdateResponseData: BookingUpdateResponseData;
}

export interface BookingUpdateResponseData {
    success: Success;
    warning: Warning;
    error: Error;
}

export interface Success {
    recordLocatorField: string;
    pnrAmount: BookingSum;
}

export interface BookingSum {
    balanceDue: number;
    authorizedBalanceDue: number;
    segmentCount: number;
    passiveSegmentCount: number;
    totalCost: number;
    pointsBalanceDue: number;
    totalPointCost: number;
    alternateCurrencyCode: string;
    alternateCurrencyBalanceDue: number;
}

export interface Warning {
    warningText: string;
}

export interface LowFareTripAvailabilityRequest {
    currencyCode: string;
    paxResidentCountry: string;
    lowFareAvailabilityRequestList: LowFareAvailabilityRequest[];
    paxTypeCounts: PaxTypeCount[];
    promotionCode: string;
}

export interface LowFareAvailabilityRequest {
    departureStation: string;
    arrivalStation: string;
    beginDate: string;
    endDate: string;
}

export interface LowFareTripAvailabilityResponse {
    lowFareAvailabilityResponseList: LowFareAvailabilityResponse[];
}

export interface LowFareAvailabilityResponse {
    dateMarketLowFareList: DateMarketLowFare[];
}

export interface DateMarketLowFare {
    departureCity: string;
    arrivalCity: string;
    fareAmount: number;
    taxesAndFeesAmount: number;
    departureDate: string;
    expireUTC: string;
    includesTaxesAndFees: boolean;
    carrierCode: string;
    statusCode: string;
    farePointAmount: number;
    availableCount: number;
    dateFlightLowFareList: DateFlightLowFare[];
    dateMarketSegmentList: DateMarketSegment[];
    currencyCode: string;
}

export interface DateMarketSegment {
    departureCity: string;
    arrivalCity: string;
    departureDate: string;
    carrierCode: string;
    dateMarketSegmentType: string;
}

export interface DateFlightLowFare {
    fareAmount: number;
    taxesAndFeesAmount: number;
    sta: string;
    std: string;
    farePointAmount: number;
    availableCount: number;
    dateFlightLegList: DateFlightLeg[];
    dateFlightFaresList: DateFlightFares[];
    productClass: string;
}

export interface DateFlightLeg {
    departureStation: string;
    arrivalStation: string;
    sta: string;
    std: string;
    carrierCode: string;
    flightNumber: string;
    operatingCarrier: string;
    equipmentType: string;
}

export interface DateFlightFares {
    nightsStay: number;
    dateFlightFareList: DateFlightFare[];
}

export interface DateFlightFare {
    fareAmount: number;
    taxesAndFeesAmount: number;
    farePointAmount: number;
    availableCount: number;
    bookingClassList: string[];
    isRouteAU: boolean;
    routeAdjustment: number;
    productClass: string;
    paxType: string;
    paxDiscountCode: string;
    dateFlightPaxFareList: DateFlightPaxFare[];
}

export interface DateFlightPaxFare {
    fareAmount: number;
    taxesAndFeesAmount: number;
    farePointAmount: number;
    paxType: string;
    paxDiscountCode: string;
}

export interface Booking {
    recordLocator: string;
    currencyCode: string;
    paxCount: number;
    bookingID: number;
    bookingInfo: BookingInfo;
    pos: PointOfSale;
    sourcePOS: PointOfSale;
    bookingHold: BookingHold;
    bookingSum: BookingSum;
    passengers: Passenger[];
    journeys: Journey[];
    bookingComments: BookingComment[];
    bookingContacts: BookingContact[];
    payments: Payment[];
    state: BookingState;
}


export enum BookingState {
    New = 0,
    Clean = 1,
    Modified = 2,
    Deleted = 3,
    Confirmed = 4,
    Unmapped = -1
}

export interface PriceItineraryResponse {
    currencyCode: string;
    journeys: Journey[];
    passengers: Passenger[];
    bookingSum: BookingSum;
}

export interface BookingContact {
    typeCode: string;
    names: BookingName[];
    emailAddress: string;
    homePhone: string;
    workPhone: string;
    otherPhone: string;
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

export interface BookingComment {
    commentType: CommentType;
    commentText: string;
    pointOfSale: PointOfSale;
    createdDate: string;
    sendToBookingSource: boolean;
}

export enum CommentType {
    Default = 0,
    Itinerary = 1,
    Manifest = 2,
    Alert = 3,
    Archive = 4,
    Unmapped = -1
}

export interface UpdateContactsRequestData {
    bookingContactList: BookingContact[];
}

export interface UpdateContactsResponse {
    bookingUpdateResponseData: BookingUpdateResponseData;
}

export interface GetBookingContactsResponse {
    bookingContacts: BookingContact[];
}

export interface UpdatePassengersRequestData {
    passengers: Passenger[];
}

export interface GetPassengersResponse {
    passengers: Passenger[];
}

export interface Passenger {
    // customerNumber?: string;
    passengerNumber: number;
    // familyNumber?: number;
    paxDiscountCode: string;
    names: BookingName[];
    infant: PassengerInfant;
    passengerInfo: PassengerInfo;
    passengerFees: PassengerFee[];
    // passengerAddresses?: PassengerAddress[];
    passengerTravelDocuments: PassengerTravelDocument[];
    // passengerBags?: PassengerBag[];
    // passengerID?: number;
    // passengerTypeInfos?: PassengerTypeInfo[];
    // passengerInfos?: PassengerInfo[];
    // passengerInfants: PassengerInfant[];
    // pseudoPassenger?: boolean;
    passengerTypeInfo: PassengerTypeInfo;
}

export interface PassengerTypeInfo {
    dob: string;
    paxType: string;
}

export interface PassengerBag {
    baggageID: number;
    oSTag: string;
    oSTagDate: string;
    stationCode: string;
    weight: number;
    weightType: WeightType;
    taggedToStation: string;
    taggedToFlightNumber: string;
    lRTIndicator: boolean;
    baggageType: string;
    taggedToCarrierCode: string;
}

export enum WeightType {
    Default = 0,
    Pounds = 1,
    Kilograms = 2,
    Unmapped = -1
}

export enum Gender {
    Male = 0,
    Female = 1,
    Unmapped = -1
}

export interface PassengerTravelDocument {
    docTypeCode: string;
    issuedByCode: string;
    docSuffix: string;
    docNumber: string;
    dob: string;
    gender: Gender;
    nationality: string;
    expirationDate: string;
    names: BookingName[];
    birthCountry: string;
    issuedDate: string;
}

export interface PassengerAddress {
    typeCode: string;
    stationCode: string;
    companyName: string;
    addressLine1: string;
    addressLine2: string;
    addressLine3: string;
    city: string;
    provinceState: string;
    postalCode: string;
    countryCode: string;
    phone: string;
    emailAddress: string;
    cultureCode: string;
    refusedContact: boolean;
}

export interface PassengerFee {
    actionStatusCode: string;
    feeCode: string;
    feeDetail: string;
    feeNumber: number;
    feeType: FeeType;
    feeOverride: boolean;
    flightReference: string;
    note: string;
    ssrCode: string;
    ssrNumber: number;
    paymentNumber: number;
    serviceCharges: BookingServiceCharge[];
    createdDate: string;
    isProtected: boolean;
    feeApplicationType: FeeApplicationType;
}

export enum FeeApplicationType {
    Journey = 2,
    Leg = 4,
    None = 0,
    PNR = 1,
    Segment = 3,
    Unmapped = -1
}

export enum FeeType {
    All = 0,
    Tax = 1,
    TravelFee = 2,
    ServiceFee = 3,
    PaymentFee = 4,
    PenaltyFee = 5,
    SSRFee = 6,
    NonFlightServiceFee = 7,
    UpgradeFee = 8,
    SeatFee = 9,
    BaseFare = 10,
    SpoilageFee = 11,
    NameChangeFee = 12,
    ConvenienceFee = 13,
    BaggageFee = 14,
    FareSurcharge = 15,
    PromotionDiscount = 16,
    ServiceBundle = 17,
    Unmapped = -1
}

export interface PassengerInfo {
    balanceDue?: number;
    gender: Gender;
    nationality: string;
    residentCountry: string;
    totalCost?: number;
}

export interface PassengerInfant {
    dob: string;
    gender: Gender;
    nationality: string;
    residentCountry: string;
    names: BookingName[];
    paxType: string;
}

export interface BookingName {
    firstName: string;
    middleName: string;
    lastName: string;
    suffix: string;
    title: string;
}

export interface BookingHold {
    holdDateTime: string;
}

export interface PointOfSale {
    agentCode: string;
    organizationCode: string;
    domainCode: string;
    locationCode: string;
}

export interface BookingInfo {
    bookingStatus: BookingStatus;
    bookingType: string;
    channelType: ChannelType;
    createdDate: string;
    expiredDate: string;
    modifiedDate: string;
    priceStatus: PriceStatus;
    changeAllowed: boolean;
    createdAgentID: number;
    modifiedAgentID: number;
    bookingDate: string;
    owningCarrierCode: string;
    paidStatus: PaidStatus;
    activityDate: string;
}

export enum BookingStatus {
    Default = 0,
    Hold = 1,
    Confirmed = 2,
    Closed = 3,
    HoldCanceled = 4,
    PendingArchive = 5,
    Archived = 6,
    Unmapped = -1
}

export enum ChannelType {
    API = 4,
    Default = 0,
    Direct = 1,
    GDS = 3,
    Web = 2
}

export enum PriceStatus {
    Invalid = 0,
    Override = 1,
    Valid = 3
}

export enum PaidStatus {
    OverPaid = 2,
    PaidInFull = 1,
    UnderPaid = 0
}

export interface Payment {
    referenceType: PaymentReferenceType;
    referenceID: number;
    paymentMethodType: PaymentMethodType;
    paymentMethodCode: string;
    currencyCode: string;
    paymentAmount: number;
    collectedCurrencyCode: string;
    collectedAmount: number;
    quotedCurrencyCode: string;
    quotedAmount: number;
    status: BookingPaymentStatus;
    accountNumber: string;
    accountNumberID: number;
    expiration: string;
    authorizationCode: string;
    authorizationStatus: AuthorizationStatus;
    parentPaymentID: number;
    transferred: boolean;
    reconcilliationID: number;
    fundedDate: string;
    installments: number;
    paymentText: string;
    channelType: ChannelType;
    paymentNumber: number;
    accountName: string;
    sourcePointOfSale: PointOfSale;
    pointOfSale: PointOfSale;
    paymentID: number;
    deposit: boolean;
    accountID: number;
    password: string;
    accountTransactionCode: string;
    voucherID: number;
    voucherTransactionID: number;
    overrideVoucherRestrictions: boolean;
    overrideAmount: boolean;
    recordLocator: string;
    paymentAddedToState: boolean;
    threeDSecure: ThreeDSecure;
    paymentFields: PaymentField[];
    createdDate: string;
    createdAgentID: number;
    modifiedDate: string;
    modifiedAgentID: number;
    binRange: number;
    approvalDate: string;
    bookingID: number;
}

export interface PaymentField {
    fieldName: string;
    fieldValue: string;
}

export interface ThreeDSecure {
    browserUserAgent: string;
    browserAccept: string;
    remoteIpAddress: string;
    termUrl: string;
    proxyVia: string;
    validationTDSApplicable: boolean;
    validationTDSPaReq: string;
    validationTDSAcsUrl: string;
    validationTDSPaRes: string;
    validationTDSSuccessful: boolean;
    validationTDSAuthResult: string;
    validationTDSCavv: string;
    validationTDSCavvAlgorithm: string;
    validationTDSEci: string;
    validationTDSXid: string;
}

export enum AuthorizationStatus {
    Unknown = 0,
    Acknowledged = 1,
    Pending = 2,
    InProcess = 3,
    Approved = 4,
    Declined = 5,
    Referral = 6,
    PickUpCard = 7,
    HotCard = 8,
    Voided = 9,
    Retrieval = 10,
    ChargedBack = 11,
    Error = 12,
    ValidationFailed = 13,
    Address = 14,
    VerificationCode = 15,
    FraudPrevention = 16,
    Unmapped = -1
}

export enum PaymentReferenceType {
    Booking = 1,
    Session = 2,
    Unmapped = -1
}

export enum PaymentMethodType {
    PrePaid = 0,
    ExternalAccount = 1,
    AgencyAccount = 2,
    CustomerAccount = 3,
    Voucher = 4,
    Loyalty = 5,
    Unmapped = -1
}

export enum BookingPaymentStatus {
    New = 0,
    Received = 1,
    Pending = 2,
    Approved = 3,
    Declined = 4,
    Unknown = 5,
    PendingCustomerAction = 6,
    Unmapped = -1
}

export enum CalculationResult {    PublishedFare = 0,    FareDiscount = 1,    DiscountedFare = 2,    IncludedTravelTax = 3,    IncludedTravelFee = 4,    RevenueFare = 5,    AddedTravelTax = 6,    AddedTravelFee = 7,    TotalTravelTax = 8,    TotalTravelFee = 9,    TotalTravelFeeAndTax = 10,    TotalFare = 11,    AddedTravelTaxAndTravelFee = 12,    TotalFareSurcharge = 13,    FarePoints = 14,    RevenueFareWithoutSurcharges = 15,    ETicketFare = 16,    DiscountedFareWithSurcharges = 17,    PublishedPassengerFee = 20,    PassengerFeeDiscount = 22,    DiscountedPassengerFee = 23,    PassengerFeeIncludedTax = 24,    RevenuePassengerFee = 25,    AddedPassengerFeeTax = 26,    TotalPassengerFeeTax = 27,    TotalPassengerFee = 28,    ConnectionAdjustmentAmount = 29,    TotalNewPassengerFee = 30,    DiscountPoints = 31,    PublishedAOSFare = 40,    IncludedAOSFee = 41,    IncludedAOSTax = 42,    RevenueAOSFare = 43,    AddedAOSFee = 44,    AddedAOSTax = 45,    TotalAOSTax = 46,    TotalAOSFee = 47,    TotalAOSFeeAndTax = 48,    TotalAOSFare = 49,    TotalAOSHeldAmmount = 50,    TotalAOSChargedAmount = 51,    NominalPayment = 80,    TotalPayment = 81,    AuthorizedPayment = 82,    NewPayment = 83,    ApprovedPayment = 84,    DepositPaymentNonDeclined = 85,    TotalDepositPaymentAmount = 86,    TotalNonDepositPaymentAmount = 87,    NominalBalanceDue = 90,    BalanceDue = 91,    AuthorizedBalanceDue = 92,    Total = 100,    TotalCharged = 101,    TotalToCollect = 102,    TotalEMDFee = 103,    TotalNewEMDFee = 104,    TotalEMDFeeIssued = 105
}

export interface BookingCalculatorResultResponse {
    amount: number;
    points: number;
}

export interface LogonRequestData {
    domainCode: string;
    agentName: string;
    password: string;
    roleCode: string;
}

export interface LogonResponse {
    mustChangePassword: boolean;
}

export interface GetSSRAvailabilityForBookingResponse {
    ssrAvailabilityForBookingResponse: SSRAvailabilityForBookingResponse;
}

export interface SSRAvailabilityForBookingResponse {
    ssrSegmentList: SSRSegment[];
}

export interface SSRSegment {
    legKey: LegKey;
    availablePaxSSRList: AvailablePaxSSR[];
}

export interface LegKey {
    departureDate: string;
    departureStation: string;
    arrivalStation: string;
}

export interface AvailablePaxSSR {
    ssrCode: string;
    inventoryControlled: boolean;
    nonInventoryControlled: boolean;
    seatDependent: boolean;
    nonSeatDependent: boolean;
    available: number;
    passengerNumberList: number[];
    paxSSRPriceList: PaxSSRPrice[];
    ssrLegList: SSRLeg[];
}

export interface PaxSSRPrice {
    paxFee: PassengerFee;
    passengerNumberList: number[];
}

export interface SSRLeg {
    legKey: LegKey;
    available: number;
}

export interface SeatAvailabilityRequest {
    seatAvailabilityRequestData: SeatAvailabilityRequestData;
}

export interface SeatAvailabilityRequestData {
    journeyIndex: number;
    segmentIndex: number;
    legIndex: number;
}

export interface GetSeatAvailabilityResponse {
    seatAvailabilityResponse: SeatAvailabilityResponse;
}

export interface SeatAvailabilityResponse {
    equipmentInfos: EquipmentInfo[];
    seatGroupPassengerFees: SeatGroupPassengerFee[];
    legs: SeatAvailabilityLeg[];
    propertyTypeCodesLookup: EquipmentPropertyTypeCodesLookup;
}

export interface EquipmentPropertyTypeCodesLookup {
    booleanPropertyTypes: EquipmentProperty[];
    numericPropertyTypeCodes: string[];
    ssrCodes: string[];
    timestamp: string;
}

export interface SeatGroupPassengerFee {
    seatGroup: number;
    passengerFee: PassengerFee;
    passengerNumber: number;
}

export interface SeatAvailabilityLeg {
    arrivalStation: string;
    departureStation: string;
    sta: string;
    std: string;
    flightDesignator: FlightDesignator;
    aircraftType: string;
    aircraftTypeSuffix: string;
    equipmentIndex: number;
}

export interface EquipmentInfo {
    arrivalStation: string;
    departureStation: string;
    equipmentType: string;
    equipmentTypeSuffix: string;
    availableUnits: number;
    compartments: CompartmentInfo[];
    propertyList: EquipmentProperty[];
    propertyBits: number[];
    propertyInts: number[];
    propertyBitsInUse: number[];
    propertyIntsInUse: number[];
    ssrBitsInUse: number[];
    marketingCode: string;
    name: string;
}

export interface CompartmentInfo {
    compartmentDesignator: string;
    deck: number;
    length: number;
    width: number;
    availableUnits: number;
    orientation: number;
    sequence: number;
    seats: SeatInfo[];
    propertyList: EquipmentProperty[];
    propertyBits: number[];
    propertyInts: number[];
    propertyTimestamp: string;
}

export interface SeatInfo {
    assignable: boolean;
    cabotageLevel: number;
    carAvailableUnits: number;
    compartmentDesignator: string;
    seatSet: number;
    criterionWeight: number;
    seatSetAvailableUnits: number;
    ssrSeatMapCode: string;
    seatAngle: number;
    seatAvailability: SeatAvailability;
    seatDesignator: string;
    seatType: string;
    x: number;
    y: number;
    propertyList: EquipmentProperty[];
    ssrPermissions: string[];
    ssrPermissionBits: number[];
    propertyBits: number[];
    propertyInts: number[];
    travelClassCode: string;
    propertyTimestamp: string;
    seatGroup: number;
    zone: number;
    height: number;
    width: number;
    priority: number;
    text: string;
    odPenalty: number;
    terminalDisplayCharacter: string;
    premiumSeatIndicator: boolean;
}

export enum SeatAvailability {
    Blocked = 2,
    Broken = 10,
    CheckedIn = 7,
    FleetBlocked = 8,
    HeldForAnotherSession = 3,
    HeldForThisSession = 4,
    Missing = 6,
    Open = 5,
    Reserved = 1,
    ReservedForPNR = 11,
    Restricted = 9,
    SoftBlocked = 12,
    Unavailable = 13,
    Unknown = 0
}

export interface EquipmentProperty {
    typeCode: string;
    value: string;
}

export interface AssignSeatRequest {
    assignSeatData: AssignSeatData;
}

export interface AssignSeatData {
    journeyIndex: number;
    segmentIndex: number;
    legIndex: number;
    paxNumber: number;
    compartmentDesignator: string;
    unitDesignator: string;
}

export interface AssignSeatsResponse {
    bookingUpdateResponseData: BookingUpdateResponseData;
    assignedSeatInfo: AssignedSeatInfo;
}

export interface AssignedSeatInfo {
    journeyList: AssignedSeatJourney[];
}

export interface AssignedSeatJourney {
    segments: AssignSeatSegment[];
}

export interface AssignSeatSegment {
    arrivalStation: string;
    departureStation: string;
    sta: Date;
    std: Date;
    cabinOfService: string;
    flightDesignator: FlightDesignator;
    paxSeats: PaxSeat[];
}

export interface SellSSRRequest {
    ssrRequestData: SSRRequestData;
}

export interface CancelSSRRequest {
    ssrRequestData: SSRRequestData;
}

export interface SSRRequestData {
    ssrCode: string;
    paxNumber: number;
    ssrCount: number;
    journeyIndex: number;
    segmentIndex: number;
    legIndex: number;
    note: string;
}

export interface SellFeeRequestData {
    feeCode: string;
    passengerNumber: number;
    note: string;
}

export interface CancelFeeRequestData {
    feeCode: string;
    passengerNumber: number;
}

export interface GetPaymentMethodsListResponse {
    paymentMethodList: PaymentMethod[];
}

export interface PaymentMethod {
    allowed: boolean;
    allowDeposit: boolean;
    allowZeroAmount: boolean;
    commissionable: boolean;
    dCCType: DCCType;
    disallowPartialRefund: boolean;
    feeCode: string;
    inActive: boolean;
    maxInstallments: number;
    name: string;
    paymentMethodCode: string;
    paymentMethodType: PaymentMethodType;
    paymentRefundType: PaymentRefundType;
    proportionalRefund: boolean;
    refundableByAgent: boolean;
    // refundCurrencyControl: schemas.navitaire.com.WebServices.DataContracts.Common.Enumerations.RefundCurrencyControl;
    restrictionHours: number;
    systemControlled: boolean;
    validationRequired: boolean;
    // paymentMethodFields: Newskies.UtilitiesManager.PaymentMethodField[];
}

export enum DCCType {
    None = 0,
    ZeroRate = 1,
    FullAmount = 2
}

export enum PaymentRefundType {
    NotAllowed = 0,
    LineItemLevel = 1,
    AccountLevel = 2,
    BookingLevel = 3,
    Unmapped = -1
}

export interface AddPaymentToBookingRequestData {
    quotedCurrencyCode: string;
    quotedAmount: number;
    paymentMethodCode: string;
    accountNumber: string;
    expiration: string;
    accountHolderName: string;
    cvvCode: string;
    threeDSecureRequest: ThreeDSecureRequest;
}

export interface ThreeDSecureRequest {
    browserUserAgent: string;
    browserAccept: string;
    remoteIpAddress: string;
    termUrl: string;
    proxyVia: string;
}

export interface AddPaymentToBookingResponse {
    bookingPaymentResponse: AddPaymentToBookingResponseData;
}

export interface AddPaymentToBookingResponseData {
    validationPayment: ValidationPayment;
}

export interface RemovePaymentFromBookingRequest {
    paymentNumber: number;
}

export interface RemovePaymentFromBookingResponse {
    bookingUpdateResponseData: BookingUpdateResponseData
}

export interface ValidationPayment {
    payment: Payment;
    paymentValidationErrors: PaymentValidationError[];
}

export interface PaymentValidationError {
    errorType: PaymentValidationErrorType;
    errorDescription: string;
    attributeName: string;
}

export enum PaymentValidationErrorType {
    Unknown = 0,
    Other = 1,
    AccountNumber = 2,
    Amount = 3,
    ExpirationDate = 4,
    RestrictionHours = 5,
    MissingAccountNumber = 6,
    MissingExpirationDate = 7,
    PaymentSystemUnavailable = 8,
    MissingParentPaymentID = 9,
    InProcessPaymentChanged = 10,
    InvalidNumberOfInstallments = 11,
    CreditShellCommentRequired = 12,
    NoQuotedCurrencyProvided = 13,
    NoBaseCurrencyForBooking = 14,
    QuotedCurrencyDoesNotMatchBaseCurrency = 15,
    QuotedRefundAmountNotLessThanZero = 16,
    QuotedPaymentAmountIsLessThanZero = 17,
    RoleCodeNotFound = 18,
    UnknownOrInactivePaymentMethod = 19,
    DepositPaymentsNotAllowedForPaymentMethod = 20,
    UnableToRetrieveRoleCodeSettings = 21,
    DepositPaymentsNotAllowedForRole = 22,
    PaymentMethodNotAllowedForRole = 23,
    InvalidAccountNumberLength = 24,
    PaymentTextIsRequired = 25,
    InvalidPaymentTextLength = 26,
    InvalidMiscPaymentFieldLength = 27,
    MiscPaymentFieldRequired = 28,
    BookingCurrencyIsInvalidForSkyPay = 29,
    SkyPayExceptionThrown = 30,
    InvalidAccountNumberForPaymentMethod = 31,
    InvalidELVTransaction = 32,
    BlackListedCard = 33,
    InvalidPaymentAddress = 34,
    InvalidSecurityCode = 35,
    InvalidCurrencyCode = 36,
    InvalidAmount = 37,
    PossibleFraud = 38,
    InvalidCustomerAccount = 39,
    AccountHolderIsNotAnAgency = 40,
    InvalidStartDate = 41,
    InvalidInitialPaymentStatus = 42,
    PaymentCurrencyMustMatchBookingCurrency = 43,
    CollectedAmountMustMatchPaymentAmount = 44,
    RefundsNotAllowedUsingThisPaymentMethod = 45,
    CreditShellAmountGreaterThanOrEqualToZero = 46,
    CreditFileAmountLessThanOrEqualToZero = 47,
    InvalidPrepaidApprovalCodeLength = 48,
    AccountNumberFailedModulousCheck = 49,
    NoExternalRatesAvailable = 50,
    ExternalCurrencyConversion = 51,
    InvalidVoucher = 52,
    StoredCardSecurityViolation = 53,
    AccountNumberDecryptionFailure = 54,
    DirectCurrencyConversionIssueOnRefund = 55,
    Unmapped = -1
}

export interface CommitRequest {
    flag: boolean
}

export interface CommitResponse {
    bookingUpdateResponseData: BookingUpdateResponseData;
}

export interface GetPostCommitResultsResponse {
    maxQueryCount: number;
    repeatQueryIntervalSecs: number;
    shouldContinuePolling: boolean;
    sessionPaymentQueryCount: number;
    paymentQueryLastCall: string;
    redirectPaymentURL: string;
    redirectMethod: string;
    redirectParams: PaymentField[];
    bookingDelta: Booking;
}

export interface RetrieveBookingRequest {
    recordLocator: string;
    lastName: string;
}

export interface RetrieveBookingResponse {
    booking: Booking;
}

export enum PaxSegmentCheckInStatus {
    BookingNotComplete = 0,
    CheckInNotYetOpen = 1,
    TooCloseToDeparture = 2,
    AlreadyCheckedIn = 3,
    FlightHasAlreadyDeparted = 4,
    PaxHasInfant = 5,
    RestrictedByAirport = 6,
    Allowed = 7
}

export interface BookingRetrieveInfo {
    hasBalanceDue: boolean;
    allPaxSegmentsCheckedIn: boolean;
    paxJourneySegmentInfos: PaxJourneySegmentInfo[]
}

export interface PaxJourneySegmentInfo {
    journeyIndex: number;
    segmentIndex: number;
    checkInStatus: CheckInStatus;
    paxLiftStatus: PaxLiftStatus[]

}

export interface PaxLiftStatus {
    passengerNumber: number;
    liftStatus: LiftStatus
}

export enum CheckInStatus {
    NotYetOpen = 0,
    Open = 1,
    Closed = 2,
    Unmapped = -1,
}

export enum LiftStatus {
    Default = 0,
    CheckedIn = 1,
    Boarded = 2,
    NoShow = 3,
    Unmapped = -1
}

export interface CheckInPassengersRequestData {
    checkInMultiplePassengerRequestList: CheckInMultiplePassengerRequest[]
}

export interface CheckInMultiplePassengerRequest {
    journeyIndex: number;
    segmentIndex: number;
    passengerNumbers: number[]
}

export interface CheckInPassengersResponse {
    checkInPassengersResponseData: CheckInPassengersResponseData
}

export interface CheckInPassengersResponseData {
    checkInMultiplePassengerResponseList: CheckInMultiplePassengerResponse[]
}

export interface CheckInMultiplePassengerResponse {
    checkInPaxResponseList: CheckInPaxResponse[]
}

export interface CheckInPaxResponse {
    errorList: CheckInError[]
}

export interface CheckInError {
    errorMessage: string;
    typeId: number
}

export interface Name {
    title: string;
    firstName: string;
    middleName?: string;
    lastName: string
}

export interface CommitAgentRequestData {
    person: Person;
    agent: Agent;
}

export enum PersonType {
    None = 0,
    Customer = 1,
    Agent = 2,
    Unmapped = -1
}

export interface Person {
    personID: number;
    personType: PersonType;
    cultureCode: string;
    dob: string;
    gender: Gender;
    customerNumber: string;
    emailAddress: string;
    nationality: string;
    name: Name;
    mobilePhone: string;
    travelDocs: PassengerTravelDocument[]; // see if this works
    residentCountry: string;
    city: string;
}

export interface AgentIdentifier {
    organizationCode: string;
    domainCode: string;
    agentName: string;
}

export enum AgentStatus {
    Active = 1,
    Default = 0,
    Pending = 4,
    Suspended = 3,
    Terminated = 2
}

export enum AuthenticationType {
    None = 0,
    Password = 1,
    Unmapped = -1
}

export interface Agent {
    agentID: number;
    agentIdentifier: AgentIdentifier;
    loginName: string;
    status: AgentStatus;
    password: string;
    authenticationType: AuthenticationType;
    personID: number;
    departmentCode: string;
    locationCode: string;
    forcePasswordReset: boolean;
    locked: boolean;
    agentRoles: AgentRole[];
}

export interface AgentRole {
    roleCode: string;
}

export interface CommitAgentResponse {
    commitAgentResData: CommitAgentResponseData 
}

export interface CommitAgentResponseData {
    person: Person;
    agent: Agent;
}

export interface SessionInfo {
    agentName: string;
    roleCode: string;
    organizationCode: string;
}

export interface PasswordSetRequest {
    newPassword: string;
}

export interface PasswordSetAnonymouslyRequest {
    logonRequestData: LogonRequestData
    passwordSetRequest: PasswordSetRequest
}

export interface PasswordResetRequest {
    loginName: string; 
    newPassword: string;
}

export interface FindBookingData {
    flightDate: string;
    fromCity: string;
    toCity: string;
    recordLocator: string;
    bookingId: number;
    passengerId: number;
    bookingStatus: BookingStatus;
    flightNumber: string;
    sourceAgentCode: string;
    editable: boolean;
    name: BookingName;
    expiredDate: string;
}

export interface FindBookingResponseData {
    records: number;
    endingId: number;
    findBookingDataList: FindBookingData[];
}

export enum BarCodeType {
    Default = 0,
    Airport = 1,
    M2D = 2,
    S2D = 3,
    Type1_1D = 4,
    Type2_1D = 5,
    Type3_1D = 6,
    Type4_1D = 7,
    BothType1 = 8,
    BothType2 = 9,
    BothType3 = 10,
    BothType4 = 11,
    Type3_1D_Plus_Space = 12,
    Type3_1D_Date = 13,
    BothType3_Plus_Space = 14,
    BothType3_Date = 15,
    Type5_1D = 16,
    BothType5 = 17,
    Unmapped = -1
}

export interface BoardingPassRequest {
    barCodeType?: BarCodeType;
    journeyIndex: number;
    segmentIndex?: number;
    legIndex?: number;
    paxNumber?: number;
    barcodeWidth?: number;
    barcodeHeight?: number;
}

export interface GetBarCodedBoardingPassesRequest {
    boardingPassRequest: BoardingPassRequest
}

export interface Barcode {
    barCodeData: string;
    barCodeImageBase64: string;
    barCodeType: string;
}

export interface InventoryLegKey {
    arrivalStation: string;
    carrierCode: string;
    departureDate: string;
    departureStation: string;
    flightNumber: string;
}

export interface BoardingPassSegment {
    arrivalStation: string;
    departureStation: string;
    departureTime: string;
    departureGate: string;
    boardingTime: string;
    inventoryLegKey: InventoryLegKey;
    legs: BoardingPassLeg[];
}

export interface BoardingPassLeg {
    boardingZone: number;
    boardingSequence: string;
    seatRow: string;
    seatColumn: string;
}

export interface BarcodedBoardingPass {
    barCode: Barcode;
    name: BookingName;
    recordLocator: string;
    segments: BoardingPassSegment[];
}

export interface GetBarCodedBoardingPassesResponse {
    barcodeHeight: number;
    barcodeWidth: number;
    barCodedBoardingPasses: BarcodedBoardingPass[]
}

export enum OrganizationType {
    Default = 0,
    Master = 1,
    Carrier = 2,
    TravelAgency = 3,
    ThirdParty = 4,
    Unmapped = -1
}

export enum OrganizationStatus {
    Default = 0,
    Active = 1,
    Cancelled = 2,
    Pending = 3,
    Unmapped = -1
}

export interface Address {
    addressLine1: string;
    addressLine2: string;
    city: string;
    provinceState: string;
    postalCode: string;
    countryCode: string;
}

export interface Organization {
    organizationCode: string;
    organizationType: OrganizationType;
    organizationName: string;
    status: OrganizationStatus;
    address: Address;
    url: string;
    phone: string;
    emailAddress: string;
    cultureCode: string;
    currencyCode: string;
    contactName: Name;
    contactPhone: string;
}

export interface CommitAgencyRequest {
    organization: Organization;
    commitAgentRequestData: CommitAgentRequestData;
}

export interface CommitAgencyResponse {
    organization: Organization;
    commitAgentResponseData: CommitAgentResponseData;
}

export interface ApplyPromotionRequestData {
    promotionCode: string;
}

export interface ApplyPromotionResponse {
    bookingUpdateResponseData: BookingUpdateResponseData;
}

export interface GetOrganizationRequestData {
    organizationCode: string;
    getDetails?: boolean;
}

export interface GetOrganizationResponse {
    organization: Organization;
}

export interface FindAgentItem {
    agentID: number;
    status: AgentStatus;
    agentIdentifier: AgentIdentifier;
    allowed: boolean;
}

export interface ResponseBase {
    pageSize: number;
    lastID: number;
    //totalCount: number;
    //cultureCode: string;
}

export interface FindAgentResponseData extends ResponseBase {
    findAgentList: FindAgentItem[];
}

export interface FindAgentResponse {
    findAgentResponseData: FindAgentResponseData;
}

export enum AccountType {
    Default = 0,
    Credit = 1,
    Prepaid = 2,
    Dependent = 3,
    Supplementary = 4,
    Unmapped = -1
}

export enum AccountStatus {
    Default = 0,
    Open = 1,
    Closed = 2,
    AgencyInactive = 3,
    Unmapped = -1
}

export interface Account {
    accountType: AccountType;
    accountStatus: AccountStatus;
    accountReference: string;
    currencyCode: string;
    foreignCurrencyCode: string;
    foreignAmount: number;
    availableCredits: number;
    spoiledCurrencyCode: string;
    spoiledForeignAmount: number;
}

export enum SearchType {
    Contains = 2,
    EndsWith = 1,
    ExactMatch = 3,
    StartsWith= 0
}

export interface SearchString {
    value: string;
    searchType: SearchType
}

export interface RequestBase {
    pageSize: number;
    lastID: number;
}

export interface FindAgentRequestData extends RequestBase {
    organizationCode: string;
    domainCode: string;
    agentName: SearchString;
    lastName: string;
    firstName: SearchString;
    status: AgentStatus;
    roleCode: string;
}

export interface FindBookingRequestData {
    recordLocator?: string;
    departDate?: string;
    destination?: string;
    agentId?: number;
    lastID: number;
    pageSize: number;
}
