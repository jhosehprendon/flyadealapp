import { Container } from 'wkcore';
import _ from 'underscore';
import moment from 'moment';

const schemaRequired = ['flightType', 'originCode', 'destinationCode', 'adults', 'children', 'infants', 'departureDate'];
const schemaProperties = {
    'flightType': {
        enum: ['round', 'oneway']
    },
    'adults': {
        'type': 'number',
        'maximum': 9
    },
    'children': { 'type': 'number' },
    'infants': { 'type': 'number', 'maximum': { '$data': '1/adults' } },
    'originCode': {
        type: 'string',
        maxLength: 3,
        minLength: 3
    },
    'destinationCode': {
        type: 'string',
        maxLength: 3,
        minLength: 3
    },
    'departureDate': { not: { type: 'null' } },
    'arriveDate': { }
};
const schema = {
    required: schemaRequired,
    properties: schemaProperties,
    select: { $data: '0/flightType' },
    selectCases: {
        oneway: {
            required: schemaRequired,
            properties: schemaProperties,
        },
        round: {
            required: [...schemaRequired, ...['arriveDate']],
            properties: { ...schemaProperties, arriveDate: { not: { type: 'null' } } },
        }
    }
};

export default class FlightSearchBase extends Container {

    async init({ config, ctx }) {
        await super.init({ config, ctx });
        this.options = {
            flightType: 'round',
            originCode: '',
            destinationCode: '',
            adultsArray: [...Array(9).keys()].map(i => i + 1),
            childrenArray: [...Array(10).keys()],
            infantsArray: [...Array(10).keys()],
            adults: 1,
            children: 0,
            infants: 0,
            departureDate: moment().startOf('day'),
            arriveDate: moment().add(1, 'days').startOf('day'),
            originLocationSelect: [],
            destinationLocationSelect: [],
            showValidationErrors: false
        };
        this.ctx.inventory.set('FlightSearch', this);
        this.createValidator();
    }

    async destroy() {
        delete this.ctx.inventory.FlightSearch;
        super.destroy(...arguments);
    }

    createValidator() {
        const sdk = this.ctx.inventory.get('dataService');
        this.validator = new sdk.validators.SchemaValidator({ schema });
    }

    async update({ config, ctx }) {
        await super.update({ config, ctx });
        await this.populateOptions();
    }

    async populateOptions() {
        const sdk = this.ctx.inventory.get('dataService');
        const culture = await sdk.cultureService.getCurrentCulture();
        this.options.originCode = this.options.originCode ? this.options.originCode : '';
        this.options.originLocationSelect = await this.getOriginLocations();
        this.options.destinationLocationSelect = await this.getDestinationLocations(this.options.originCode);
        this.options.destinationCode = this.options.destinationCode 
                && this.options.destinationLocationSelect.some(d => d.value === this.options.destinationCode) ? this.options.destinationCode : '';
        this.options.departureDate = this.options.departureDate.startOf('day').locale(culture.toLowerCase());
        this.options.arriveDate = this.options.arriveDate.startOf('day').locale(culture.toLowerCase());
    }

    async getOriginLocations() {
        const sdk = this.ctx.inventory.get('dataService');
        const culture = await sdk.cultureService.getCurrentCulture();
        const stations = await sdk.resourceCache.getStations(culture);
        const markets = await sdk.resourceCache.getMarkets();
        return stations.filter(station => markets.some(m => m.locationCode === station.stationCode))
            .map(station => { return { value: station.stationCode, name: station.shortName }; });
    }

    async getDestinationLocations(originCode) {
        const sdk = this.ctx.inventory.get('dataService');
        const culture = await sdk.cultureService.getCurrentCulture();
        const stations = await sdk.resourceCache.getStations(culture);
        const markets = await sdk.resourceCache.getMarkets();
        const selectedMarkets = originCode ?
            _.filter(markets, m => m.locationCode === originCode && _.any(stations, st => st.stationCode === m.travelLocationCode)) : [];
        return selectedMarkets.map(m => {
            const station = _.find(stations, s => m.travelLocationCode === s.stationCode);
            return { value: station.stationCode, name: station.shortName };
        });
    }

    isOneWay() {
        return this.options.flightType === 'oneway';
    }

    createSearchRequest() {
        const request = {
            adults: this.options.adults,
            children: this.options.children,
            infants: this.options.infants,
            paxResidentCountry: 'SA',
            flights: [{
                departureStation: this.options.originCode,
                arrivalStation: this.options.destinationCode,
                beginDate: moment(this.options.departureDate).locale('en').format('YYYY-MM-DD'),
                endDate: moment(this.options.departureDate).locale('en').format('YYYY-MM-DD')
            }]
        };
        if (!this.isOneWay()) {
            request.flights.push({
                departureStation: this.options.destinationCode,
                arrivalStation: this.options.originCode,
                beginDate: moment(this.options.arriveDate).locale('en').format('YYYY-MM-DD'),
                endDate: moment(this.options.arriveDate).locale('en').format('YYYY-MM-DD')
            });
        }
        return request;
    }

    createSearchLowFareRequest(fullFareRequest) {
        const request = fullFareRequest || this.createSearchRequest();
        const lowFareRequest = { ...request, flights: [] };
        const days = Math.floor(Number(this.properties.lowFareSearchDayRange) / 2);

        request.flights.forEach(f => {
            const beginDaysDiff = moment(f.beginDate, 'YYYY-MM-DD').diff(moment().startOf('day'), 'days');
            const letfDaysDelta = beginDaysDiff < days ? beginDaysDiff : days;
            const beginSearchDate = moment(f.beginDate, 'YYYY-MM-DD').add(-letfDaysDelta, 'd');
            const endSearchDate = moment(beginSearchDate).add(Number(this.properties.lowFareSearchDayRange) - 1, 'd');
            const flight = {
                ...f,
                beginDate: beginSearchDate.locale('en').format('YYYY-MM-DD'), // moment(f.beginDate, 'YYYY-MM-DD').locale('en').add(-days, 'd').format('YYYY-MM-DD'),
                endDate: endSearchDate.locale('en').format('YYYY-MM-DD') //moment(f.endDate, 'YYYY-MM-DD').locale('en').add(days, 'd').format('YYYY-MM-DD')
            };
            lowFareRequest.flights.push(flight);
        });
        return lowFareRequest;
    }

    async searchLowFareFlights(request) {
        const lowFareRequest = request || this.createSearchLowFareRequest();
        const sdk = this.ctx.inventory.get('dataService');
        return await sdk.flightAvailabilityService.findFlightsLowFares(lowFareRequest);
    }

    async searchFullFareFlights() {
        const request = this.createSearchRequest();
        const sdk = this.ctx.inventory.get('dataService');
        return await sdk.flightAvailabilityService.findFlights(request);
    }

    async searchFlights() {
        const request = this.createSearchRequest();
        const lowFareRequest = this.createSearchLowFareRequest(request);
        const sdk = this.ctx.inventory.get('dataService');
        await sdk.flightAvailabilityService.findFlights(request);
        await sdk.flightAvailabilityService.findFlightsLowFares(lowFareRequest);
    }
}