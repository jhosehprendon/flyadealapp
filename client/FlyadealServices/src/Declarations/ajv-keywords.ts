declare module "ajv-keywords" {

    import Ajv from 'ajv';

    export default function extendAjv(ajv: Ajv.Ajv): void;
};