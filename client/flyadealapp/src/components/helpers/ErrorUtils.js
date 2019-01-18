export default class ErrorUtils {

    static async handleApiError({ ctx, error }) {
        const sdk = ctx.inventory.get('dataService');
        const router = ctx.inventory.get('router');
        const alertModal = ctx.inventory.get('alertModal');
        if (sdk.errorService.sessionExpiredError(error)) {
            return await alertModal.show({
                title: 'Session Expired',
                message: 'Your session has expired, please start from the beggining.',
                closeButton: true,
                closeCallback: async () => {
                    // if sessionExpired transition state is the current state
                    if (router.activeState && router.activeState.transitions && router.activeState.name === router.activeState.transitions.sessionExpired) {
                        await sdk.sessionService.create();
                    }
                    return router.sessionExpired(error) && ErrorUtils.clearSessionAvailabilityData({ ctx });
                }
            });
        }
        const badData = sdk.errorService.badDataError(error);
        if (badData) {
            let message;
            if (badData.messages && badData.messages.length) {
                badData.messages.forEach((m, i) => message = (i === 0 ? `${m}` : `${message}\n\r${m}`));
            }
            return await alertModal.show({
                title: 'Invalid Request',
                message,
                closeButton: true,
                closeCallback: () => router.badDataError(badData)
            });
        }
        const internalServerError = sdk.errorService.internalServerError(error);
        if (internalServerError) {
            const message = internalServerError && internalServerError.messages && internalServerError.messages.length ? internalServerError.messages[0] : 'Internal Server Error, please try again later.';
            return await alertModal.show({
                title: 'Error',
                message,
                closeButton: true,
                closeCallback: () => router.internalServerError(internalServerError)
            });
        }
        if (sdk.errorService.serverUnavailableError(error)) {
            return await alertModal.show({
                title: 'Error',
                message: 'Server is unavailable, please try again later.',
                closeButton: true,
                closeCallback: () => router.serverUnavailableError(error)
            });
        }
    }

    static clearSessionAvailabilityData({ ctx }) {
        const sdk = ctx.inventory.get('dataService');
        sdk.flightAvailabilityService.availabilityResponse = undefined;
        sdk.flightAvailabilityService.availabilityLowFareTrips = undefined;
        sdk.bookingService.priceItinerary = undefined;
    }
}