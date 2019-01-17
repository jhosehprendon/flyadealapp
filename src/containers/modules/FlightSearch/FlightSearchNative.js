import React from 'react';
import FlightSearch from './FlightSearch';
import _ from 'underscore';

import ErrorUtils from '../../../components/helpers/ErrorUtils';
import FlightSearchView from './view/FlightSearchView';


export default class FlightSearchNative extends FlightSearch {

    async init({ config, ctx }) {
        await super.init({ config, ctx });
        const sdk = this.ctx.inventory.get('dataService');
        this.options.culture = await sdk.cultureService.getCurrentCulture();
        this.options.stations = await sdk.resourceCache.getStations(this.options.culture);
    }

    async update({ctx, config}) {
        const sdk = this.ctx.inventory.get('dataService');
        this.options.culture = await sdk.cultureService.getCurrentCulture();
        this.options.stations = await sdk.resourceCache.getStations(this.options.culture);
        await super.update({ctx, config});
    }

    async searchFlights() {
        try {
            if (!await this.onValidate()) {
                return await this.handleValidationError() && this.ctx.emit('update');
            }
            await super.searchFlights();
            if (!this.ctx.inventory.router.transitTo('searchFlights')) {
                return this.ctx.emit('update');
            }
        } catch (error) {
            return await ErrorUtils.handleApiError({ ctx: this.ctx, error });
        }
    }

    async handleValidationError() {
        const errors = this.validator.errors;
        if(!errors || !errors.length) {
            return;
        }
        let message = '';
        const uniqueErrors = _.uniq(errors, e => e.dataPath);
        uniqueErrors.forEach(m => {
            if(m.dataPath && m.dataPath.indexOf('destinationCode') > -1) {
                message = `${message.length ? ', ' : ''}Please select your Destination`;
            }
        });
        const alertModal = this.ctx.inventory.get('alertModal');
        return await alertModal.show({
                title: 'Validation error',
                message,
                closeButton: true
            });
    }

    async searchFullFareFlights() {
        try {
            await super.searchFullFareFlights();
            const router = this.ctx.inventory.get('router');
            if (!router.transitTo('fullFareSearchComplete')) {
                this.ctx.emit('update');
            }
        } catch (error) {
            return await ErrorUtils.handleApiError({ ctx: this.ctx, error });
        }
    }

    async searchLowFareFlights(request) {
        try {
            await super.searchLowFareFlights(request);
            const router = this.ctx.inventory.get('router');
            if (!router.transitTo('lowFareSearchComplete')) {
                this.ctx.emit('update');
            }
        } catch (error) {
            return await ErrorUtils.handleApiError({ ctx: this.ctx, error });
        }
    }

    getStationName(stationCode) {
        const station = this.options.stations.find(s => s.stationCode === stationCode);
        return station ? station.shortName : stationCode;
    }

    addAdult = () => this.addPass('adt');

    addChild = () => this.addPass('chd');

    addInfant = () => this.addPass('int');

    removeAdult = () => this.minPass('adt');

    removeChild = () => this.minPass('chd');

    removeInfant = () => this.minPass('int');

    getComponent({ style } = {}) {
        const props = {
            options: { ...this.options},
            properties: this.properties,
            actions: {
                setFlightType: (type) => this.setFlightType(type),
                getStationName: (code) => this.getStationName(code),
                selectOrigin: (code) => this.selectOrigin(code),
                selectDestination: (code) => this.selectDestination(code),
                setFlightDate: async (startDate, endDate) => await this.setFlightDate(startDate, endDate),
                addAdult: this.addAdult,
                addChild: this.addChild,
                addInfant: this.addInfant,
                removeAdult: this.removeAdult,
                removeChild: this.removeChild,
                removeInfant: this.removeInfant,
                searchFlights: async () => await this.searchFlights(),
            },
            style
        };
        return (
            <FlightSearchView {...props} />
        );
    }

}