import _ from 'underscore';
import moment from 'moment';
import ajv from 'ajv';
import SchemaValidator from './SchemaValidator';
import { Validator } from './Validators';
import { Country } from 'newskies-services/lib/Contracts/Contracts';

const schema = {
    required: ['name', 'emailAddress', 'homePhone', 'countryCode', 'city', 'emailAddressConfirm'],
    properties: {
        name: {
            type: 'object',
            required: ['title', 'firstName', 'lastName'],
            properties: {
                title: { enum: {} },
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
            }
        },
        emailAddress: { type: 'string', pattern: '^[A-Za-z0-9&\'._% +-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,24}$', not: { type : 'null'} },
        emailAddressConfirm: { const: { '$data': '1/emailAddress' } },
        homePhone: {
            type: 'object',
            required: ['countryCode', 'number'],
            properties: {
                countryCode: { enum: {} },
                number: { type: 'number', minimum: 10, maximum: 999999999999999 }
            }
        },
        city: {
            type: 'string',
            pattern: '^[a-zA-Z\\s]*$',
            maxLength: 30,
            minLength: 2
        },
        countryCode: { enum: {} },
        cultureCode: { enum: {} }
    }
};

export interface BookingContactOptions {
    titles: string[];
    countries: Country[];
    cultures: string[];
}

export default class BookingContactValidator implements Validator {

    private _validator: SchemaValidator;
    private _errors: ajv.ErrorObject[];

    constructor(options: BookingContactOptions) {
        schema.properties.name.properties.title.enum = options.titles;
        schema.properties.countryCode.enum = options.countries.map(c => c.countryCode);
        schema.properties.cultureCode.enum = options.cultures;
        schema.properties.homePhone.properties.countryCode.enum = Object.keys(_.groupBy(options.countries.map(c => c.phoneCode), code => code));
        this._validator = new SchemaValidator({ schema });
    }

    get errors(): ajv.ErrorObject[] {
        return this._errors;
    }

    async validate(data: any): Promise<boolean> {
        // ignore email case in validation
        data.emailAddress = data.emailAddress ? data.emailAddress.toLowerCase() : null;
        data.emailAddressConfirm = data.emailAddressConfirm ? data.emailAddressConfirm.toLowerCase() : null;

        const valid = await this._validator.validate(data);
        this._errors = valid ? [] : this._validator.errors;
        return valid;
    }
}