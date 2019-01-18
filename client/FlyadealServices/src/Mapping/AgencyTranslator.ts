import { Contracts as NskContracts, Utils as NskUtils } from 'newskies-services';
import { Phone, Member, Person, Agent, Agency, Organization } from "../Contracts";
import MemberTranslator from './MemberTranslator';
import moment from 'moment';

const DATE_FORMAT = 'YYYY-MM-DDT00:00:00';
const DEFAULT_ROLE_CODE = 'WWMB';

export default class AgencyTranslator {
    static translateFlyadealAgency(agency: Agency): NskContracts.CommitAgencyRequest {
        const org = agency.organization;
        const agt = agency.agent;
        const organization: NskContracts.Organization = {
            organizationCode: org.organizationCode,
            organizationType: org.organizationType,
            organizationName: org.organizationName,
            status: NskContracts.OrganizationStatus.Default,
            address: {
                addressLine1: org.address.addressLine1,
                addressLine2: org.address.addressLine2,
                city: org.address.city,
                provinceState: org.address.provinceState,
                countryCode: org.address.countryCode,
                postalCode: org.address.postalCode
            },
            url: org.url,
            phone: org.phone.countryCode + ' ' + org.phone.number,
            emailAddress: org.emailAddress,
            cultureCode: org.cultureCode,
            currencyCode: org.currencyCode,
            contactName: {
                title: org.contactName.title,
                firstName: org.contactName.firstName,
                lastName: org.contactName.lastName,
            },
            contactPhone: org.contactPhone.countryCode + ' ' + org.contactPhone.number
        };
        const dob = moment(agt.person.dob).locale('en').format(DATE_FORMAT);
        const travelDoc = agt.person.travelDocument;
        const person: NskContracts.Person = {
            personID: agt.person.personID,
            personType: agt.person.personType,
            cultureCode: agt.person.cultureCode,
            dob,
            gender: agt.person.gender,
            customerNumber: agt.person.customerNumber,
            emailAddress: agt.agent.loginName,
            nationality: agt.person.nationality,
            name: {
                title: agt.person.name.title,
                firstName: agt.person.name.firstName,
                lastName: agt.person.name.lastName
            },
            mobilePhone: agt.person.phone.countryCode + ' ' + agt.person.phone.number,
            travelDocs: null,
            residentCountry: agt.person.residentCountry,
            city: agt.person.city
        };
        if(travelDoc) {
            person.travelDocs = [{
                docTypeCode: travelDoc.docTypeCode,
                issuedByCode: travelDoc.docIssuingCountry,
                docSuffix: travelDoc.docSuffix,
                docNumber: travelDoc.docNumber,
                dob: dob,
                gender: agt.person.gender,
                nationality: agt.person.nationality,
                expirationDate: travelDoc.expirationDate ? moment(travelDoc.expirationDate).locale('en').format(DATE_FORMAT) : '',
                names: [{ firstName: agt.person.name.firstName, middleName: '', lastName: agt.person.name.lastName, suffix: '', title: agt.person.name.title }],
                birthCountry: travelDoc.birthCountry || '',
                issuedDate: travelDoc.issuedDate ? moment(travelDoc.issuedDate).locale('en').format(DATE_FORMAT) : ''
            }];
        }
        const agent: NskContracts.Agent = {
            agentID: agt.agent.agentID,
            agentIdentifier: agt.agent.agentIdentifier,
            loginName: agt.agent.loginName,
            status: agt.agent.status,
            password: agt.agent.password,
            authenticationType: agt.agent.authenticationType,
            personID: agt.agent.personID,
            departmentCode: agt.agent.departmentCode,
            locationCode: agt.agent.locationCode,
            forcePasswordReset: agt.agent.forcePasswordReset,
            locked: agt.agent.locked,
            agentRoles: [{
                roleCode: DEFAULT_ROLE_CODE
            }]
        };
        const commitAgentRequestData: NskContracts.CommitAgentRequestData = {
            person,
            agent
        };
        const request: NskContracts.CommitAgencyRequest = {
            organization,
            commitAgentRequestData
        };
        return request as NskContracts.CommitAgencyRequest;
    }

    static translateNewskiesAgency(agency: NskContracts.CommitAgencyResponse, countries: NskContracts.Country[]): Agency {
        const org: Organization = {
            organizationCode: agency.organization.organizationCode,
            organizationType: agency.organization.organizationType,
            organizationName: agency.organization.organizationName,
            status: agency.organization.status,
            address: agency.organization.address,
            url: agency.organization.url,
            phone: this.createPhone(agency.organization.phone, countries),
            emailAddress: agency.organization.emailAddress,
            cultureCode: agency.organization.cultureCode,
            currencyCode: agency.organization.currencyCode,
            contactName: agency.organization.contactName,
            contactPhone: this.createPhone(agency.organization.contactPhone, countries)
        };
        const agent = MemberTranslator.translateNewskiesMember(agency.commitAgentResponseData, countries);
        return {
            organization: org,
            agent: agent
        } as Agency;
    }

    private static createPhone(nskPhone: string, countries: NskContracts.Country[]): Phone {
        if (!nskPhone) {
            return {
                countryCode: '',
                number: null
            };
        }
        const parts = nskPhone.split(' ');
        if (parts.length < 2) {
            return {
                countryCode: '',
                number: null
            };
        }
        const country = countries.find(c => c.phoneCode === parts[0]);
        if (!country || !/\d+/.test(parts[1])) {
            return {
                countryCode: '',
                number: null
            };
        }
        return {
            countryCode: parts[0],
            number: Number(parts[1])
        }
    }
}