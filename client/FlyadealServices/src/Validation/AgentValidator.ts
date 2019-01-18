import moment from 'moment';
import ajv from 'ajv';
import SchemaValidator, { Keyword } from './SchemaValidator';
import { Validator } from './Validators';
import { Country, DocType } from 'newskies-services/lib/Contracts/Contracts';
import { PaxTypeConfig } from '../Contracts';
import _ from 'underscore';

const schemaTemplate: object = {
    required: ['agency', 'member'],
    properties: {
        agency: {
            required: ['organizationCode', 'organizationName', 'address', 'url', 'phone', 'emailAddress', 'emailAddressConfirm', 'cultureCode', 'contactName', 'contactPhone'],
            properties: {
                organizationCode: { type: 'string', pattern: '^[0-9]{7,10}$', not: { type: 'null' } },
                organizationName: { type: 'string', minLength: 1, maxLength: 100 },
                address: {
                    type: 'object',
                    required: ['addressLine1', /*'addressLine2',*/ 'city', /*'postalCode',*/ 'countryCode'],
                    properties: {
                        addressLine1: { type: 'string', minLength: 1, maxLength: 100 },
                        addressLine2: { type: 'string', minLength: 0, maxLength: 100 },
                        city: { type: 'string', minLength: 1, maxLength: 100 },
                        postalCode: { type: 'string', minLength: 0, maxLength: 100 },
                        countryCode: { enum: {} } 
                    }
                },
                url: { type: 'string', minLength: 2, maxLength: 200 },
                phone: {
                    type: 'object',
                    required: ['countryCode', 'number'],
                    properties: {
                        countryCode: { enum: {} },
                        number: { type: 'number', minimum: 10, maximum: 999999999999999 }
                    }
                },
                emailAddress: { type: 'string', pattern: '^[A-Za-z0-9&\'._% +-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,24}$', not: { type: 'null' } },
                emailAddressConfirm: { const: { '$data': '1/emailAddress' } },
                cultureCode: { enum: {} },
                contactName: {
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
                        }
                    }
                },
                contactPhone: {
                    type: 'object',
                    required: ['countryCode', 'number'],
                    properties: {
                        countryCode: { enum: {} },
                        number: { type: 'number', minimum: 10, maximum: 999999999999999 }
                    }
                }
            }
        },
        member: {
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
                                }
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
        }
    }
}

export interface AgentValidatorOptions {
    adultConfig: PaxTypeConfig
    countries: Country[];
    cultures: string[];
    docTypes: DocType[];
    passwordRequired: boolean;
    loginNameRequired: boolean;
    travelDocRequired: boolean;
    isCorporate: boolean;
}

export default class AgentValidator implements Validator {
    private _validator: SchemaValidator;
    private _errors: ajv.ErrorObject[];

    constructor(options: AgentValidatorOptions) {
        const schema = JSON.parse(JSON.stringify(schemaTemplate));
        // Apply enum options
        schema.properties.agency.properties.address.properties.countryCode.enum = options.countries.map(c => c.countryCode);
        schema.properties.agency.properties.phone.properties.countryCode.enum = Object.keys(_.groupBy(options.countries.map(c => c.phoneCode), code => code));
        schema.properties.agency.properties.cultureCode.enum = options.cultures;
        schema.properties.agency.properties.contactName.properties.title.enum = [...options.adultConfig.titles];
        schema.properties.agency.properties.contactPhone.properties.countryCode.enum = Object.keys(_.groupBy(options.countries.map(c => c.phoneCode), code => code));
        schema.properties.member.properties.person.properties.cultureCode.enum = options.cultures;
        schema.properties.member.properties.person.properties.nationality.enum = options.countries.map(c => c.countryCode);
        schema.properties.member.properties.person.properties.name.properties.title.enum = [...options.adultConfig.titles];
        schema.properties.member.properties.person.properties.phone.properties.countryCode.enum = Object.keys(_.groupBy(options.countries.map(c => c.phoneCode), code => code));
        schema.properties.member.properties.person.properties.travelDocument.properties.docTypeCode.enum = options.docTypes.map(t => t.docTypeCode);
        schema.properties.member.properties.person.properties.travelDocument.properties.docIssuingCountry.enum = options.countries.map(c => c.countryCode);
        schema.properties.member.properties.person.properties.residentCountry.enum = options.countries.map(c => c.countryCode);
        // Date of birth range
        schema.properties.member.properties.person.properties.dob['fd-moment'].minimum = moment().add(-options.adultConfig.maxAgeYears, 'y');
        schema.properties.member.properties.person.properties.dob['fd-moment'].maximum = moment().add(-options.adultConfig.minAgeYears, 'y'); 
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
        schema.properties.member.properties.agent.required = agentReqs;
        schema.properties.member.properties.agent.properties = agentProps;

        // Don't validate travel doc if not required
        if (options.travelDocRequired === false) {
            schema.properties.member.properties.person.required = _.without(schema.properties.member.properties.person.required, 'travelDocument');
            schema.properties.member.properties.person.properties.travelDocument = {};
        }

        // Alternate schema for Corporate Organization Codes
        if (options.isCorporate) {
            //schema.properties.agency.properties.organizationCode = { type: 'string', pattern: '^[0-9]{7,10}[a-zA-Z]{1}$', not: { type: 'null' } };
            schema.properties.agency.properties.organizationCode = { type: 'string' }; // The Org Code for Corp Agents will be auto-generated on server side now.
        }

        this._validator = new SchemaValidator({ schema });
    }

    get errors(): ajv.ErrorObject[] {
        return this._errors;
    }

    async validate(data: any): Promise<boolean> {
        // ignore email case in validation
        data.member.agent.loginName = data.member.agent.loginName.toLowerCase();
        data.member.agent.loginNameConfirm = data.member.agent.loginNameConfirm ? data.member.agent.loginNameConfirm.toLowerCase() : '';
        data.member.agent.passwordConfirm = data.member.agent.passwordConfirm || '';

        const valid = await this._validator.validate(data);
        this._errors = valid ? [] : this._validator.errors;
        return valid;
    }
}