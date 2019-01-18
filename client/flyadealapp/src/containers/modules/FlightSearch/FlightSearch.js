import moment from 'moment';
import { Logger } from 'wklib';
import geolocation from 'geolocation';
import FlightSearchBase from '../../containers/FlightSearchBase/FlightSearchBase';

const logger = new Logger('FlightSearch');

export default class FlightSearch extends FlightSearchBase {

    async init({ config, ctx }) {
        await super.init({ config, ctx });
        await this.populateLastSearch();
    }

    async populateLastSearch() {
        const sdk = this.ctx.inventory.get('dataService');
        const lastSearchRequest = sdk.flightAvailabilityService.flightSearchRequest;
        if (!lastSearchRequest || !lastSearchRequest.flights || !lastSearchRequest.flights.length) {
            await this.selectNearestStation();
            await super.populateOptions();
            return;
        }
        this.options = {
            flightType: lastSearchRequest.flights.length > 1 ? 'round' : 'oneway',
            originCode: lastSearchRequest.flights[0].departureStation,
            destinationCode: lastSearchRequest.flights[0].arrivalStation,
            adultsArray: [...Array(9).keys()].map(i => i + 1),
            childrenArray: [...Array(10).keys()],
            infantsArray: [...Array(10).keys()],
            adults: lastSearchRequest.adults,
            children: lastSearchRequest.children,
            infants: lastSearchRequest.infants,
            departureDate: moment(lastSearchRequest.flights[0].beginDate).startOf('day'),
            arriveDate: moment(lastSearchRequest.flights[lastSearchRequest.flights.length - 1].endDate).startOf('day'),
            originLocationSelect: await super.getOriginLocations(),
            destinationLocationSelect: await super.getDestinationLocations(lastSearchRequest.flights[0].departureStation),
        };
    }

    async update({ config, ctx }) {
        await super.update({ config, ctx });
        this.options.showValidationErrors && await this.validator.validate(this.options);
    }

    async selectNearestStation() {
        const sdk = this.ctx.inventory.get('dataService');
        const culture = await sdk.cultureService.getCurrentCulture();
        const stations = await sdk.resourceCache.getStations(culture);
        try {
            const stationCode = await this.getNearestStation(stations);
            this.selectOrigin(stationCode, false);
        } catch(err) {
            logger.warn(`Unable to retrieve current location: ${err.message}`);
            this.selectOrigin(stations[0].stationCode, false);
        }
    }

    getNearestStation(stations) {
        return new Promise((resolve, reject)=>{
            geolocation.getCurrentPosition((err, position) => {
                if (err) {
                    return reject(err);
                }
                let stationsInfo = [];
                stations.forEach(s => {
                    stationsInfo.push(this.getStationDistanceKm(position.coords.latitude, position.coords.longitude, parseFloat(s.latitude), parseFloat(s.longitude), s.stationCode));
                });
                if (!stationsInfo) {
                    return reject(new Error('No station info found'));
                }
                stationsInfo.sort((a, b) => { return a.distanceKm - b.distanceKm });
                return resolve(stationsInfo[0].stnCode);
            });
        });
    }

    getStationDistanceKm(lat1, lon1, lat2, lon2, stnCode) {
        const p = 0.017453292519943295;
        const c = Math.cos;
        const a = 0.5 - c((lat2 - lat1) * p) / 2 +
            c(lat1 * p) * c(lat2 * p) *
            (1 - c((lon2 - lon1) * p)) / 2;

        return { distanceKm: 12742 * Math.asin(Math.sqrt(a)), stnCode: stnCode };
    }

    setFlightType(code) {
        this.options.flightType = code;
        if (this.isOneWay() && !this.options.departureDate.isSame(this.options.arriveDate, 'day')) {
            this.options.arriveDate = this.options.departureDate.startOf('day');
        }
        if (!this.isOneWay() && this.options.departureDate.isSame(this.options.arriveDate, 'day')) {
            this.options.arriveDate = moment(this.options.departureDate).add(1, 'd').startOf('day');
        }
        this.ctx.emit('update');
    }

    async setFlightDate(depart, arrive, update=true) {
        const sdk = this.ctx.inventory.get('dataService');
        const culture = await sdk.cultureService.getCurrentCulture();
        this.options.departureDate = depart.startOf('day').locale(culture.toLowerCase());
        this.options.arriveDate = arrive.startOf('day').locale(culture.toLowerCase());
        return update && this.ctx.emit('update');
    }

    selectOrigin(code, update=true) {
        this.options.originCode = code;
        return update && this.ctx.emit('update');
    }

    selectDestination(code, update=true) {
        this.options.destinationCode = code;
        return update && this.ctx.emit('update');
    }

    addPass(type, update=true) {
        if (type === 'adt') {
            if (this.options.adults < 9 && !this.totalPass()) {
                this.options.adults = this.options.adults + 1;
            }
        } else if (type === 'chd') {
            if (this.options.children < 9 && !this.totalPass()) {
                this.options.children = this.options.children + 1;
            }
        } else if (type === 'int') {
            if (this.options.infants < 9 && this.options.infants < this.options.adults) {
                this.options.infants = this.options.infants + 1;
            }
        }
        return update && this.ctx.emit('update');
    }

    minPass(type, update=true) {
        if (type === 'adt') {
            if (this.options.adults > 1) {
                this.options.adults = this.options.adults - 1;
            }
            if (this.options.infants > this.options.adults) {
	            this.options.infants = this.options.adults;
            }
        } else if (type === 'chd') {
            if (this.options.children > 0) {
                this.options.children = this.options.children - 1;
            }
        } else if (type === 'int') {
            if (this.options.infants > 0) {
                this.options.infants = this.options.infants - 1;
            }
        }
        return update && this.ctx.emit('update');
    }
    
    totalPass() {
	    return (this.options.adults + this.options.children) >= 9;
    }

    async onValidate() {
        const valid = await this.validator.validate(this.options);
        this.options.showValidationErrors = true;
        return valid;
    }

    showValidationError(path) {
        return this.options.showValidationErrors && !this.options.validationResult.isValid(path);
    }
}