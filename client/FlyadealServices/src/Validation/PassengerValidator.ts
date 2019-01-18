import moment from 'moment';
import ajv from 'ajv';
import SchemaValidator, { Keyword } from './SchemaValidator';
import { PaxType, DocType, Country } from "newskies-services/lib/Contracts/Contracts";
import { PaxTypeConfig, Passenger } from "../Contracts";
import { Validator } from "./Validators";

const schemaTemplate: object = {
    required: ['dateOfBirth', 'title', 'firstName', 'lastName', 'passengerNumber', 'paxType', 'residentCountry', 'travelDocument'],
    properties: {
        title: { enum: [] },
        firstName: {
            type: 'string',
            pattern: '^[a-zA-Z]*$',
            minLength: 1,
            maxLength: 100
        },
        lastName: {
            type: 'string',
            pattern: '^[a-zA-Z]*$',
            minLength: 1,
            maxLength: 100
        },
        passengerNumber: { type: 'number' },
        paxType: { type: 'string' },
        residentCountry: { enum: [], not: { type: 'null' } },
        dateOfBirth: { 'fd-moment': { minimum: undefined, maximum: null }, not: { type: 'null' } },
        travelDocument: {
            type: 'object',
            required: ['docNumber', 'docTypeCode', 'docIssuingCountry', 'expirationDate'],
            properties: {
                //birthCountry: string,
                docNumber: { type: 'string', minLength: 5, maxLength: 20 },
                docTypeCode: { enum: [] },
                docIssuingCountry: { enum: [] },
                //issuedDate?: Moment,
                expirationDate: { 'fd-moment': { minimum: null, maximum: null }, not: { type: 'null' } },
                // nationality: { enum: [] },
                //docSuffix: string,
            }
        }
    }
};

export interface PassengerValidatorOptions {
    paxTypes: PaxType[];
    docTypes: DocType[];
    countries: Country[];
    adultConfig: PaxTypeConfig;
    childConfig: PaxTypeConfig;
    infantConfig: PaxTypeConfig;
    std: moment.Moment;
    bookingDate: moment.Moment;
    isInternational: Boolean;
}

export default class PassengerValidator implements Validator {

    private _paxTypes: PaxType[];
    private _validators: SchemaValidator[];
    private _docTypes: DocType[];
    private _countries: Country[];
    private _adultConfig: PaxTypeConfig;
    private _childConfig: PaxTypeConfig;
    private _infantConfig: PaxTypeConfig;
    private _std: moment.Moment;
    private _bookingDate: moment.Moment;
    private _isInternational: Boolean;
    private _errors: ajv.ErrorObject[];

    constructor(options: PassengerValidatorOptions) {
        this._paxTypes = options.paxTypes;
        this._docTypes = options.docTypes;
        this._adultConfig = options.adultConfig;
        this._childConfig = options.childConfig;
        this._infantConfig = options.infantConfig;
        this._countries = options.countries;
        this._std = options.std;
        this._bookingDate = options.bookingDate;
        this._isInternational = options.isInternational;
        this._validators = this.createPassengerTypesValidators();
    }

    private createPassengerTypesValidators(): SchemaValidator[] {
        const countryCodes = this._countries.map(c => c.countryCode);
        return this._paxTypes.map(type => {
            const schema = JSON.parse(JSON.stringify(schemaTemplate));
            let config;
            if (type.code === 'CHD') {
                config = this._childConfig;
            } else if (type.code === 'INFT') {
                config = this._infantConfig;
            } else {
                config = this._adultConfig;
            }
            config.maxAgeYears = type.code === 'INFT' ? config.maxAgeYears : config.maxAgeYears += 1;
            schema.properties.residentCountry.enum = countryCodes;
            schema.properties.title.enum = [...config.titles];
            schema.properties.dateOfBirth['fd-moment'].minimum = moment(this._std).add(-config.maxAgeYears, 'y');
            const today = moment();
            if (moment(this._std).add(-config.minAgeYears, 'y') > today) {
                schema.properties.dateOfBirth['fd-moment'].maximum = type.code === 'INFT' ? today.add(-8, 'd') : today;
            } else {
                schema.properties.dateOfBirth['fd-moment'].maximum = moment(this._std).add(-config.minAgeYears, 'y');
            }
            schema.properties.travelDocument.properties.docTypeCode.enum = this._docTypes.map(t => t.docTypeCode);
            schema.properties.travelDocument.properties.docIssuingCountry.enum = countryCodes;
            // schema.properties.travelDocument.properties.nationality.enum = countryCodes;
            this._isInternational ? schema.properties.travelDocument.properties.expirationDate['fd-moment'].minimum = moment(this._std) :
                schema.properties.travelDocument.properties.expirationDate['fd-moment'].minimum = moment(this._bookingDate).add(1, 'day');
            return new SchemaValidator({ schema, name: type.code });
        });
    }

    async validate(passenger: Passenger) {
        const validator = this._validators.find(v => v.name === passenger.paxType);
        if (!validator) {
            throw new Error(`${passenger.paxType} validator is not registered in PassengersValidator`);
        }
        const valid = await validator.validate(passenger);
        this._errors = valid ? [] : validator.errors;
        return valid;
    }

    get errors(): ajv.ErrorObject[] {
        return this._errors;
    }
}