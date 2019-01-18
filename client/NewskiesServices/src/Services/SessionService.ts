import AjaxClient from '../Ajax/AjaxClient';
import { LogonRequestData, SessionInfo, LogonResponse } from "../Contracts/Contracts";

export default class SessionService {

    private readonly _ajaxClient: AjaxClient;

    constructor(ajax: AjaxClient) {
        this._ajaxClient = ajax;
    }

    async create() {
        try {
            const response = await this._ajaxClient.post('api/session');
        } catch (e) {
            throw e;
        }
    }

    async get() {
        try {
            const response = await this._ajaxClient.get('api/session');
            return response.data as SessionInfo;
        } catch (e) {
            throw e;
        }
    }

    async logon(data: LogonRequestData) {
        try {
            const response = await this._ajaxClient.post('api/agent/logon', data);
            return response.data as LogonResponse;
        } catch (e) {
            throw e;
        }
    }

    async logoff() {
        try {
            const response = await this._ajaxClient.post('api/agent/logoff');
        } catch (e) {
            throw e;
        }
    }
}