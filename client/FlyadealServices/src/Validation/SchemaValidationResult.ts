import ajv from 'ajv';

export class SchemaValidationResult {
    private _errors: Array<ajv.ErrorObject>;

    constructor(errors?: Array<ajv.ErrorObject>) {
        this._errors = errors;
    }

    isValid(path: string | RegExp) {
        if (!path) {
            return !this._errors || this._errors.length;
        }
        const regex = path as RegExp;
        return !this._errors || !this._errors.some(e => {
            if (e.keyword === 'required') {
                const reqParams = e.params as ajv.RequiredParams;
                if (!reqParams || !reqParams.missingProperty) {
                    return false;
                }
                const match = `${e.keyword}${e.dataPath}.${reqParams.missingProperty}`;
                //return regex.test ? regex.test(match) : match.indexOf(path.toString()) > -1;
                return regex.test ? regex.test(match) : match.endsWith(path.toString());
            }
            const match = `${e.keyword}${e.dataPath}`;
            //return regex.test ? regex.test(match) : match.indexOf(path.toString()) > -1;
            return regex.test ? regex.test(match) : match.endsWith(path.toString());
        });
    }
}