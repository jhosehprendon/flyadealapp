import moment from 'moment';
import ajv from 'ajv';
import SchemaValidator, { Keyword } from './SchemaValidator';
import { Validator } from './Validators';
import { Country, DocType } from 'newskies-services/lib/Contracts/Contracts';
import { PaxTypeConfig } from '../Contracts';
import _ from 'underscore';

const schemaTemplate: object = {
    required: ['person', 'agent'],
    properties: {
        person: {
            type: 'object',
            required: ['cultureCode', 'dob', 'nationality', 'name', 'phone', 'travelDocument', 'residentCountry', 'city'],
            properties: {
                cultureCode: { enum: {} },
                dob: { 'fd-moment': { minimum: undefined, maximum: undefined }, not: { type: 'null' } },
                nationality: { enum: {} },
                name: {
                    type: 'object',
                    required: ['firstName', 'lastName', 'title'],
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
                    }
                },
                phone: {
                    type: 'object',
                    required: ['countryCode', 'number'],
                    properties: {
                        countryCode: { enum: {} },
                        number: { type: 'number', minimum: 10, maximum: 999999999999999 }
                    }
                },
                travelDocument: {
                    type: 'object',
                    required: ['docTypeCode', 'docIssuingCountry', 'docNumber', 'expirationDate'],
                    properties: {
                        docNumber: { type: 'string', minLength: 5, maxLength: 20 },
                        docTypeCode: { enum: [] },
                        docIssuingCountry: { enum: [] },
                        expirationDate: { 'fd-moment': { minimum: undefined, maximum: undefined }, not: { type: 'null' } },
                    }
                },
                city: {
                    type: 'string',
                    pattern: '^[a-zA-Z\\s]*$',
                    maxLength: 30,
                    minLength: 2
                },
                residentCountry: { enum: {} },
            }
        },
        agent: {
            type: 'object',
            required: [],
            properties: {}
        }
    }
};

export interface MemberOptions {
    adultConfig: PaxTypeConfig
    countries: Country[];
    cultures: string[];
    docTypes: DocType[];
    passwordRequired: boolean;
    loginNameRequired: boolean;
    travelDocRequired: boolean;
}

export default class MemberValidator implements Validator {

    private _validator: SchemaValidator;
    private _errors: ajv.ErrorObject[];

    constructor(options: MemberOptions) {
        const schema = JSON.parse(JSON.stringify(schemaTemplate));
        schema.properties.person.properties.name.properties.title.enum = [ ...options.adultConfig.titles ];
        schema.properties.person.properties.nationality.enum = options.countries.map(c => c.countryCode);
        schema.properties.person.properties.cultureCode.enum = options.cultures;
        schema.properties.person.properties.phone.properties.countryCode.enum = Object.keys(_.groupBy(options.countries.map(c => c.phoneCode), code => code));
        schema.properties.person.properties.travelDocument.properties.docTypeCode.enum = options.docTypes.map(t => t.docTypeCode);
        schema.properties.person.properties.travelDocument.properties.docIssuingCountry.enum = options.countries.map(c => c.countryCode);
        schema.properties.person.properties.residentCountry.enum = options.countries.map(c => c.countryCode);

        schema.properties.person.properties.dob['fd-moment'].minimum = moment().add(-options.adultConfig.maxAgeYears, 'y');
        schema.properties.person.properties.dob['fd-moment'].maximum = moment().add(-options.adultConfig.minAgeYears, 'y'); 

        // Conditionals for validating password and login name if required
        const agentReqs = [];
        const agentProps: any = {};
        if (options.passwordRequired) {
            agentReqs.push('password', 'passwordConfirm');
            agentProps['password'] = { type: 'string', pattern: '^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$\-_%\^&\*])[^~,\.]*$', minLength: 8, maxLength: 16 };
            agentProps['passwordConfirm'] = { const: { '$data': '1/password' } };
        }
        if (options.loginNameRequired) {
            agentReqs.push('loginName', 'loginNameConfirm');
            agentProps['loginName'] = { type: 'string', pattern: '^[A-Za-z0-9&\'._% +-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,24}$', not: { type: 'null' } };
            agentProps['loginNameConfirm'] = { const: { '$data': '1/loginName' } };
        }
        schema.properties.agent.required = agentReqs;
        schema.properties.agent.properties = agentProps;

        // Don't validate travel doc if not required
        if (options.travelDocRequired === false) {
            schema.properties.person.required = _.without(schema.properties.person.required, 'travelDocument');
            schema.properties.person.properties.travelDocument = {};
        }
        
        this._validator = new SchemaValidator({ schema });
    }

    get errors(): ajv.ErrorObject[] {
        return this._errors;
    }

    async validate(data: any): Promise<boolean> {
        // ignore email case in validation
        data.agent.loginName = data.agent.loginName.toLowerCase();
        data.agent.loginNameConfirm = data.agent.loginNameConfirm ? data.agent.loginNameConfirm.toLowerCase() : '';
        data.agent.passwordConfirm = data.agent.passwordConfirm || '';

        const valid = await this._validator.validate(data);
        this._errors = valid ? [] : this._validator.errors;
        return valid;
    }
}