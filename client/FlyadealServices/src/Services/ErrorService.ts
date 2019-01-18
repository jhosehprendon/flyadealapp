import { Contracts as NskContracts, Utils } from "newskies-services";

export default class ErrorService {

    static sessionExpiredError(error: Error) {
        const newskiesError = error as NskContracts.NewskiesError;
        if (newskiesError) {
            return Utils.enumsEqual(newskiesError.errorType, NskContracts.ErrorType.Unauthorized, NskContracts.ErrorType);
        }
        return false;
    }

    static badDataError(error: Error) {
        const newskiesError = error as NskContracts.NewskiesError;
        if (newskiesError && Utils.enumsEqual(newskiesError.errorType, NskContracts.ErrorType.BadData, NskContracts.ErrorType)) {
            return newskiesError.payload ? newskiesError.payload : [];
        }
        return undefined;
    }

    static internalServerError(error: Error) {
        const newskiesError = error as NskContracts.NewskiesError;
        if (newskiesError && Utils.enumsEqual(newskiesError.errorType, NskContracts.ErrorType.InternalServer, NskContracts.ErrorType)) {
            return newskiesError.payload ? newskiesError.payload : {};
        }
        return undefined;
    }

    static serverUnavailableError(error: Error) {
        const newskiesError = error as NskContracts.NewskiesError;
        if (newskiesError) {
            return Utils.enumsEqual(newskiesError.errorType, NskContracts.ErrorType.ServerUnavaliable, NskContracts.ErrorType);
        }
        return false;
    }

    static addPaymentError(error: Error) {
        const newskiesError = error as NskContracts.NewskiesError;
        if (newskiesError) {
            return Utils.enumsEqual(newskiesError.errorType, NskContracts.ErrorType.AddPaymentError, NskContracts.ErrorType);
        }
        return false;
    }

    static bookingCommitError(error: Error) {
        const newskiesError = error as NskContracts.NewskiesError;
        if (newskiesError) {
            return Utils.enumsEqual(newskiesError.errorType, NskContracts.ErrorType.BookingCommitError, NskContracts.ErrorType);
        }
        return false;
    }

    static unauthorizedError(error: Error) {
        const newskiesError = error as NskContracts.NewskiesError;
        if (newskiesError) {
            return Utils.enumsEqual(newskiesError.errorType, NskContracts.ErrorType.Unauthorized, NskContracts.ErrorType);
        }
        return false;
    }
}