import { Contracts as NskContracts, Utils as NskUtils, AgentService as NskAgentService } from 'newskies-services';
import MemberTranslator from '../Mapping/MemberTranslator';
import Contracts, { Member } from '../Contracts';
import SessionService from './SessionService';
import SessionBag from './SessionBag';
import ResourceCache from './ResourceCache';
import Utils from './Utils';

export default class AgentService {
    private _sessionBag: SessionBag;
    private _sessionService: SessionService;
    private _nskService: NskAgentService;
    private _resourceCache: ResourceCache;
    private _masterRoleCodes = ['TMST', 'COMA'];
    private _subRoleCodes = ['TAGT', 'COAG'];
    private _domainCode = 'WW2';

    constructor(sessionBag: SessionBag, sessionService: SessionService, agentService: NskAgentService, resourceCache: ResourceCache) {
        this._sessionBag = sessionBag;
        this._sessionService = sessionService;
        this._nskService = agentService;
        this._resourceCache = resourceCache;
    }

    async get(id?: number): Promise<Member> {
        try {
            const countries = await this._resourceCache.getCountries();
            if ((await this.isAgent() || await this.isCorpAgent()) && (!this._sessionBag.agentData || this._sessionBag.agentData.agent.agentID !== id)) {
                const response = await this._nskService.get(id);
                this._sessionBag.agentData = response;
                
            }
            const result = MemberTranslator.translateNewskiesMember(this._sessionBag.agentData, countries);
            return result as Member;
        } catch (e) {
            throw e;
        }
    }

    async getAgents(requestData: NskContracts.FindAgentRequestData = null): Promise<NskContracts.FindAgentResponseData> {
        try {
            if (!requestData || (!requestData.agentName.value && !requestData.firstName.value && !requestData.lastName && requestData.status === NskContracts.AgentStatus.Active)) {
                if (!this._sessionBag.agentListData) {
                    const response = await this._nskService.getAgents(requestData);
                    this._sessionBag.agentListData = response.findAgentResponseData;
                }
                return this._sessionBag.agentListData;
            }
            const response = await this._nskService.getAgents(requestData);
            return response.findAgentResponseData as NskContracts.FindAgentResponseData;
        } catch (e) {
            throw e;
        }
    }

    async addAgent(requestData: NskContracts.CommitAgentRequestData): Promise<NskContracts.CommitAgentResponseData> {
        try {
            const response = await this._nskService.addAgent(requestData);
            this._sessionBag.agentListData = undefined;
            return response.commitAgentResData as NskContracts.CommitAgentResponseData;
        } catch (e) {
            throw e;
        }
    }

    async updateAgent(agent: NskContracts.Agent): Promise<NskContracts.Agent> {
        try {
            const response = await this._nskService.updateAgent(agent.agentID, agent);
            this._sessionBag.agentListData = undefined;
            return response as NskContracts.Agent;
        } catch (e) {
            throw e;
        }
    }

    //async updatePerson(person: NskContracts.Person): Promise<NskContracts.Person> {
    async updatePerson(memberRequestData: Member) {
        try {
            const commitAgentRequest = MemberTranslator.translateMember(memberRequestData);
            const sessionAgent = await this.get();
            const personId = commitAgentRequest.agent.personID !== sessionAgent.agent.personID ? commitAgentRequest.agent.personID : undefined;
            const response = await this._nskService.updatePerson(commitAgentRequest.person, Number(personId));
            this._sessionBag.agentListData = undefined;
            return response as NskContracts.Person;
        } catch (e) {
            throw e;
        }
    }

    async clear() {
        this._sessionBag.agentData = undefined;
        this._sessionBag.mustChangePasswordUsername = undefined;
        this._sessionBag.agentListData = undefined;
    }

    async login(loginName: string, password: string): Promise<NskContracts.LogonResponse> {
        try {
            await this.clear();
            const logonRequestData: NskContracts.LogonRequestData = {
                domainCode: this._domainCode,
                agentName: loginName,
                password: password,
                roleCode: null
            };
            const response = await this._sessionService.logon(logonRequestData);
            return response as NskContracts.LogonResponse;
        } catch (e) {
            throw e;
        }
    }

    async resetPassword(loginName: string, newPassword: string) {
        try {
            const passwordRequest: NskContracts.PasswordResetRequest = {
                loginName: loginName,
                newPassword: newPassword
            }
            const response = await this._nskService.resetPassword(passwordRequest);
            await this.clear();
            return response;
        } catch (error) {
            throw error;
        }
    }

    async setPasswordAnonymously(loginName: string, password: string, newPassword: string) {
        try {
            const logonRequestData: NskContracts.LogonRequestData = {
                domainCode: this._domainCode,
                agentName: loginName,
                password: password,
                roleCode: null
            };
            const passwordSetRequest: NskContracts.PasswordSetRequest = {
                newPassword: newPassword
            };
            const passwordSetAnonymouslyRequest: NskContracts.PasswordSetAnonymouslyRequest = {
                logonRequestData: logonRequestData,
                passwordSetRequest: passwordSetRequest
            };
            const response = await this._nskService.setPasswordAnonymously(passwordSetAnonymouslyRequest);
            await this.clear();
            return response;
        } catch (error) {
            throw error;
        }
    }

    async isAgent(): Promise<boolean> {
        const userSessionInfo = this._sessionBag.userSessionInfo;
        if (userSessionInfo && Utils.isAgent(userSessionInfo.roleCode)) {
            return true;
        }
        return false;
    }

    async isCorpAgent(): Promise<boolean> {
        const userSessionInfo = this._sessionBag.userSessionInfo;
        if (userSessionInfo && Utils.isCorpAgent(userSessionInfo.roleCode)) {
            return true;
        }
        return false;
    }

    async isMasterAgent(): Promise<boolean> {
        const userSessionInfo = this._sessionBag.userSessionInfo;
        if (userSessionInfo && this._masterRoleCodes.some(c => c === userSessionInfo.roleCode)) {
            return true;
        }
        return false;
    }

    async logoff(): Promise<boolean> {
        try {
            const response = await this._nskService.logoff();
            await this.clear();
            return response;
        } catch (e) {
            throw e;
        }
    }

    async getBookings(requestData?: NskContracts.FindBookingRequestData): Promise<NskContracts.FindBookingResponseData> {
        try {
            const response = await this._nskService.getBookings(requestData);
            return response;
        } catch (e) {
            throw e;
        }
    }

    set editAgentId(value: string) {
        this._sessionBag.editAgentId = value;
    }

    get editAgentId(): string {
        return this._sessionBag.editAgentId;
    } 

    set mustChangePasswordUsername(value: string) {
        this._sessionBag.mustChangePasswordUsername = value;
    }

    get mustChangePasswordUsername(): string {
        return this._sessionBag.mustChangePasswordUsername;
    } 
}