import Ajv from 'ajv';
import extendAjv from 'ajv-keywords';
import keywords from './Keywords';
import { SchemaValidationResult } from "./SchemaValidationResult";

export interface SchemaValidatorOptions {
    schema: object;
    name?: string;
    keywords?: Keyword[];
}

export interface Keyword {
    name: string;
    definition: Ajv.KeywordDefinition;
}

export default class SchemaValidator {

    private _ajv: Ajv.Ajv;
    private _name: string;
    private _validateFunc: Ajv.ValidateFunction;
    private _result: SchemaValidationResult;

    constructor(options: SchemaValidatorOptions) {
        this._name = options.name;
        if (!options.schema) throw new Error('Schema is not defined');
        this._ajv = new Ajv({ $data: true, allErrors: true, passContext: true });
        extendAjv(this._ajv);
        options.keywords = [...(options.keywords || []), ...keywords];
        options.keywords.forEach(kw => {
            this._ajv.addKeyword(kw.name, kw.definition);
        });
        this._validateFunc = this._ajv.compile(options.schema);
    }

    get name(): string {
        return this._name;
    }

    get errors(): Ajv.ErrorObject[] {
        return this._validateFunc ? this._validateFunc.errors : [];
    }

    async validate(data: any) {
        const valid = this._validateFunc(data);
        //if (!valid) {
        //    console.log(this.validateFunc.errors)
        //};
        this._result = new SchemaValidationResult(this._validateFunc.errors);
        data && (data.validationResult = this._result);
        return valid;
    }

    isValid(path: string) {
        if (!path) {
            return !this._validateFunc.errors || this._validateFunc.errors.length;
        }
        return !this._result || this._result.isValid(path);
    }
}