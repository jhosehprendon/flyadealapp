import { Contracts as NskContracts, Utils as NskUtils, AgentService as NskAgentService } from 'newskies-services';
import SessionService from './SessionService';
import SessionBag from "./SessionBag";
import Contracts, { Member } from "../Contracts";
import MemberTranslator from '../Mapping/MemberTranslator';
import ResourceCache from './ResourceCache';

export default class MemberService {

    private _sessionBag: SessionBag;
    private _sessionService: SessionService;
    private _agentService: NskAgentService;
    private _resourceCache: ResourceCache;
    private _memberRoleCode = 'WWMB';

    constructor(sessionBag: SessionBag, sessionService: SessionService, agentService: NskAgentService, resourceCache: ResourceCache) {
        this._sessionBag = sessionBag;
        this._sessionService = sessionService;
        this._agentService = agentService;
        this._resourceCache = resourceCache;
    }

    async clear() {
        this._sessionBag.agentData = undefined;
        this._sessionBag.mustChangePasswordUsername = undefined;
    }

    async get(): Promise<Member> {
        try {
            const countries = await this._resourceCache.getCountries();
            if (await this.isMember() && !this._sessionBag.agentData) {
                const response = await this._agentService.get();
                this._sessionBag.agentData = response;
            }
            const result = MemberTranslator.translateNewskiesMember(this._sessionBag.agentData, countries);
            return result;
        } catch (e) {
            throw e;
        }
    }

    async register(memberRequestData: Member): Promise < NskContracts.CommitAgentResponseData > {
        try {
            const commitAgentRequest = MemberTranslator.translateMember(memberRequestData);
            const nskMember = await this._agentService.addAgent(commitAgentRequest as NskContracts.CommitAgentRequestData);
            return nskMember.commitAgentResData;
        } catch (error) {
            throw error;
        }
    }

    //async updatePerson(person: NskContracts.Person) {
    async updatePerson(memberRequestData: Member) {
        try {
            const commitAgentRequest = MemberTranslator.translateMember(memberRequestData);
            const response = await this._agentService.updatePerson(commitAgentRequest.person);
            await this.clear();
            return response;
        } catch (error) {
            throw error;
        }
    }

    async setPassword(password: string) {
        try {
            const passwordRequest: NskContracts.PasswordSetRequest = {
                newPassword: password
            };
            const response = await this._agentService.setPassword(passwordRequest);
            await this.clear();
            return response;
        } catch (error) {
            throw error;
        }
    }

    async resetPassword(loginName: string, newPassword: string) {
        try {
            const passwordRequest: NskContracts.PasswordResetRequest = {
                loginName: loginName,
                newPassword: newPassword
            }
            const response = await this._agentService.resetPassword(passwordRequest);
            await this.clear();
            return response;
        } catch (error) {
            throw error; 
        }
    }

    async login(loginName: string, password: string): Promise<NskContracts.LogonResponse> {
        try {
            const logonRequestData: NskContracts.LogonRequestData = {
                domainCode: 'WW2',
                agentName: loginName,
                password: password,
                roleCode: this._memberRoleCode
            };
            const response = await this._sessionService.logon(logonRequestData);
            return response as NskContracts.LogonResponse;
        } catch (error) {
            throw error;
        }
    }

    async logout() {
        try {
            await this._sessionService.logoff();
            await this.clear();
        } catch (error) {
            throw error;
        }
    }

    async getBookings(): Promise<NskContracts.FindBookingResponseData> {
        try {
            const response = await this._agentService.getBookings();
            return response;
        } catch (e) {
            throw e;
        }
    }

    async isMember(): Promise<boolean> {
        const userSessionInfo = this._sessionBag.userSessionInfo;
        if (userSessionInfo && userSessionInfo.roleCode === this._memberRoleCode) {
            return true;
        }
        return false;
    }
}