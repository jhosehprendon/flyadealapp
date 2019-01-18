import AjaxClient from "../Ajax/AjaxClient";
import { GetOrganizationRequestData, GetOrganizationResponse, CommitAgencyRequest, CommitAgencyResponse } from "../Contracts/Contracts";
import Utils from "./Utils";

export default class AgencyService {
    private _ajaxClient: AjaxClient;

    constructor(ajaxClient: AjaxClient) {
        this._ajaxClient = ajaxClient;
    }

    async get(requestData: GetOrganizationRequestData): Promise<GetOrganizationResponse> {
        try {
            const response = await this._ajaxClient.get('api/agency?' + Utils.urlSerialize(requestData));
            return response.data as GetOrganizationResponse;
        } catch (e) {
            throw e;
        }
    }

    async commitAgency(request: CommitAgencyRequest): Promise<CommitAgencyResponse> {
        try {
            const response = await this._ajaxClient.post('api/agency', request);
            return response.data as CommitAgencyResponse;
        } catch (e) {
            throw e;
        }
    }
}