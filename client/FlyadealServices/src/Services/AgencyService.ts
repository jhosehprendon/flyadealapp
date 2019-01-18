import { Contracts as NskContracts, Utils as NskUtils, AgencyService as NskAgencyService } from 'newskies-services';
import { Agency, Agent, Organization, Member } from '../Contracts';
import SessionService from './SessionService';
import SessionBag from "./SessionBag";
import ResourceCache from './ResourceCache';
import AgencyTranslator from '../Mapping/AgencyTranslator';

export default class AgencyService {
    private _sessionBag: SessionBag;
    private _sessionService: SessionService;
    private _nskService: NskAgencyService;
    private _resourceCache: ResourceCache;

    constructor(sessionBag: SessionBag, sessionService: SessionService, agencyService: NskAgencyService, resourceCache: ResourceCache) {
        this._sessionBag = sessionBag;
        this._sessionService = sessionService;
        this._nskService = agencyService;
        this._resourceCache = resourceCache;
    }

    async get(organizationCode: string, getDetails: boolean): Promise<NskContracts.Organization> {
        try {
            const request: NskContracts.GetOrganizationRequestData = {
                organizationCode,
                getDetails
            };
            const response = await this._nskService.get(request);
            return response.organization as NskContracts.Organization;
        } catch (e) {
            throw e;
        }
    }

    async commitAgency(organization: Organization, agent: Member): Promise<Agency> {
        try {
            const countries = await this._resourceCache.getCountries();
            const agency: Agency = {
                organization,
                agent
            };
            const request = AgencyTranslator.translateFlyadealAgency(agency);
            const response = AgencyTranslator.translateNewskiesAgency(await this._nskService.commitAgency(request), countries);
            return response as Agency;
        } catch (e) {
            throw e;
        }
    }
}