import AjaxClient from "../Ajax/AjaxClient";
import {
    CommitAgentRequestData, CommitAgentResponse, CommitAgentResponseData, Person, PasswordSetRequest, PasswordResetRequest, PasswordSetAnonymouslyRequest,
    FindBookingResponseData, Agent, LogonRequestData, LogonResponse, FindAgentResponse, FindAgentRequestData, FindBookingRequestData //PassengerTravelDocument, Name, Gender,
} from "../Contracts/Contracts";
import Utils from "./Utils";

export default class AgentService {
    private _ajaxClient: AjaxClient;

    constructor(ajaxClient: AjaxClient) {
        this._ajaxClient = ajaxClient;
    }

    async get(id?: number): Promise<CommitAgentResponseData> {
        try {
            const response = id ? await this._ajaxClient.get('api/agent/' + id) : await this._ajaxClient.get('api/agent');
            const responseData = response.data as CommitAgentResponseData;
            return responseData;
        } catch (e) {
            throw e;
        }
    }

    async getAgents(requestData: FindAgentRequestData): Promise<FindAgentResponse> {
        try {
            const query = Utils.urlSerialize(requestData);
            const response = await this._ajaxClient.get('api/agent/agents?' + query);
            return response.data as FindAgentResponse;
        } catch (e) {
            throw e;
        }
    }

    async updatePerson(person: Person, id?: number): Promise<Person> {
        try {
            const query = id ? '/' + id.toString() : '/';
            const response = await this._ajaxClient.post('api/agent/person' + query, person);
            return response.data as Person;
        } catch (error) {
            throw error;
        }
    }

    async setPassword(requestData: PasswordSetRequest) {
        try {
            const response = await this._ajaxClient.post('api/agent/passwordset', requestData);
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async setPasswordAnonymously(requestData: PasswordSetAnonymouslyRequest) {
        try {
            const response = await this._ajaxClient.post('api/agent/passwordsetanonymously', requestData);
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async resetPassword(requestData: PasswordResetRequest) {
        try {
            const response = await this._ajaxClient.post('api/agent/passwordreset', requestData);
            return response.data;
        } catch (error) {
            throw error;
        }
    }

    async addAgent(requestData: CommitAgentRequestData): Promise<CommitAgentResponse> {
        try {
            const response = await this._ajaxClient.post('api/agent', requestData);
            const responseData = response.data as CommitAgentResponse;
            return responseData;
        } catch (e) {
            throw e;
        }
    }   

    async updateAgent(id: number, agent: Agent): Promise<Agent> {
        try {
            const response = await this._ajaxClient.post('api/agent/' + id, agent);
            return response.data as Agent;
        } catch (e) {
            throw e;
        }
    }

    async getBookings(requestData?: FindBookingRequestData): Promise <FindBookingResponseData> {
        try {
            const query = requestData ? Utils.urlSerialize(requestData) : '';
            const response = await this._ajaxClient.get('api/agent/bookings?' + query);
            const responseData = response.data as FindBookingResponseData;
            return responseData;
        } catch (e) {
            throw e;
        }
    }

    async logon(requestData: LogonRequestData): Promise<LogonResponse> {
        try {
            const response = await this._ajaxClient.post('api/agent/logon');
            return response.data as LogonResponse;
        } catch (e) {
            throw e;
        }
    }

    async logoff(): Promise<boolean> {
        try {
            const response = await this._ajaxClient.post('api/agent/logoff');
            return response.data as boolean;
        } catch (e) {
            throw e;
        }
    }
}