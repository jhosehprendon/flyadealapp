import { Contracts as NskContracts, SessionService as NskSessionService } from 'newskies-services';
import SessionBag from './SessionBag';

const anonRoleCode = 'WWW2';

export default class SessionService {
    private _sessionService: NskSessionService;
    private _sessionBag: SessionBag;

    constructor(sessionService: NskSessionService, sessionBag: SessionBag) {
        this._sessionService = sessionService;
        this._sessionBag = sessionBag;
    }

    async create() {
        try {
            const response = await this._sessionService.create();
        } catch (e) {
            throw e;
        }
    }

    async get(): Promise<NskContracts.SessionInfo> {
        try {
            const sessionInfo = await this._sessionService.get();
            this._sessionBag.userSessionInfo = sessionInfo;
            return sessionInfo as NskContracts.SessionInfo;
        } catch (e) {
            throw e;
        }
    }

    async logon(data: NskContracts.LogonRequestData): Promise<NskContracts.LogonResponse> {
        try {
            const response = await this._sessionService.logon(data);
            if (response) {
                await this.get();
            }
            return response as NskContracts.LogonResponse;
        } catch (e) {
            throw e;
        }
    }

    async logoff() {
        try {
            const response = await this._sessionService.logoff();
        } catch (e) {
            throw e;
        }
    }

    async isLoggedIn(): Promise<boolean> {
        try {
            const sessionInfo = await this._sessionService.get();
            return sessionInfo.roleCode !== anonRoleCode;
        } catch (e) {
            throw e;
        }
    }
}