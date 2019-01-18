import { Contracts as NskContracts, AccountService as NskAccountService } from 'newskies-services';

export default class AccountService {
    private _nskService: NskAccountService;

    constructor(nskService: NskAccountService) {
        this._nskService = nskService;
    }

    async get(): Promise<NskContracts.Account> {
        try {
            const response = await this._nskService.get();
            return response as NskContracts.Account;
        } catch (e) {
            throw e;
        }
    }
}