import AjaxClient from '../Ajax/AjaxClient';
import { Account } from '../Contracts/Contracts';
import Utils from './Utils';

export default class AccountService {
    private _ajaxClient: AjaxClient;

    constructor(ajaxClient: AjaxClient) {
        this._ajaxClient = ajaxClient;
    }

    async get(): Promise<Account> {
        try {
            const response = await this._ajaxClient.get('api/account');
            return response.data as Account;
        } catch (e) {
            throw e;
        }
    }
}