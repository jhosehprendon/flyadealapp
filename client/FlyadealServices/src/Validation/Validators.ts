import ajv from 'ajv';
import PassengerValidator from './PassengerValidator';
import BookingContactValidator from "./BookingContactValidator";
import SchemaValidator from "./SchemaValidator";
import MemberValidator from './MemberValidator';
import AgentValidator from './AgentValidator';

export interface Validator {
    validate(data: any): Promise<boolean>;
    errors: ajv.ErrorObject[];
}

export default {
    SchemaValidator,
    PassengerValidator,
    BookingContactValidator,
    MemberValidator,
    AgentValidator
}