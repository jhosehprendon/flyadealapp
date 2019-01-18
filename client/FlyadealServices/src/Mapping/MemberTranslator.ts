import { Contracts as NskContracts, Utils as NskUtils } from 'newskies-services';
import Contracts, { Phone, Member, Person, Agent } from "../Contracts";
import moment from 'moment';

const DATE_FORMAT = 'YYYY-MM-DDT00:00:00';
const DEFAULT_ROLE_CODE = 'WWMB';

export default class MemberTranslator {
    static translateMember(member: Member): NskContracts.CommitAgentRequestData {
        const travelDoc = member.person.travelDocument;
        const dob = moment(member.person.dob).locale('en').format(DATE_FORMAT);
        const person: NskContracts.Person = {
            personID: member.person.personID,
            personType: member.person.personType,
            cultureCode: member.person.cultureCode,
            dob: dob,
            gender: member.person.gender,
            customerNumber: member.person.customerNumber,
            emailAddress: member.agent.loginName,
            nationality: member.person.nationality,
            name: {
                title: member.person.name.title,
                firstName: member.person.name.firstName,
                lastName: member.person.name.lastName
            },
            mobilePhone: member.person.phone.countryCode + ' ' + member.person.phone.number,
            travelDocs: travelDoc ? [{
                docTypeCode: travelDoc.docTypeCode,
                issuedByCode: travelDoc.docIssuingCountry,
                docSuffix: travelDoc.docSuffix,
                docNumber: travelDoc.docNumber,
                dob: dob,
                gender: member.person.gender,
                nationality: member.person.nationality,
                expirationDate: travelDoc.expirationDate ? moment(travelDoc.expirationDate).locale('en').format(DATE_FORMAT) : '',
                names: [{ firstName: member.person.name.firstName, middleName: '', lastName: member.person.name.lastName, suffix: '', title: member.person.name.title }],
                birthCountry: travelDoc.birthCountry || '',
                issuedDate: travelDoc.issuedDate ? moment(travelDoc.issuedDate).locale('en').format(DATE_FORMAT) : ''
            }] : [],
            residentCountry: member.person.residentCountry,
            city: member.person.city
        };
        const agent: NskContracts.Agent = {
            agentID: member.agent.agentID,
            agentIdentifier: member.agent.agentIdentifier,
            loginName: member.agent.loginName,
            status: member.agent.status,
            password: member.agent.password,
            authenticationType: member.agent.authenticationType,
            personID: member.agent.personID,
            departmentCode: member.agent.departmentCode,
            locationCode: member.agent.locationCode,
            forcePasswordReset: member.agent.forcePasswordReset,
            locked: member.agent.locked,
            agentRoles: [{
                roleCode: DEFAULT_ROLE_CODE
            }]
        };
        const requestData: NskContracts.CommitAgentRequestData = {
            person: person,
            agent: agent
        };

        return requestData;
    }

    static translateNewskiesMember(agentResponse: NskContracts.CommitAgentResponseData, countries: NskContracts.Country[]): Member {
        const person = agentResponse.person;
        const agent = agentResponse.agent;
        const travelDocument = person.travelDocs && person.travelDocs.length > 0 ? person.travelDocs[0] : null;
        const member: Member = {
            person: {
                personID: person.personID,
                personType: person.personType,
                cultureCode: person.cultureCode || '',
                dob: moment(person.dob) || moment(),
                gender: person.gender,
                customerNumber: person.customerNumber || '',
                emailAddress: person.emailAddress || '',
                nationality: person.nationality || '',
                name: {
                    title: person.name.title || '',
                    firstName: person.name.firstName || '',
                    lastName: person.name.lastName || ''
                },
                phone: this.createPhone(person.mobilePhone, countries),
                travelDocument: {
                    birthCountry: travelDocument ? travelDocument.birthCountry : '',
                    docNumber: travelDocument ? travelDocument.docNumber : '',
                    docTypeCode: travelDocument ? travelDocument.docTypeCode : '',
                    docIssuingCountry: travelDocument ? travelDocument.issuedByCode : '',
                    issuedDate: travelDocument ? travelDocument.issuedDate ?
                        moment(travelDocument.issuedDate) : undefined : undefined,
                    expirationDate: travelDocument ? travelDocument.expirationDate ?
                        moment(travelDocument.expirationDate) : undefined : undefined,
                    nationality: travelDocument ? travelDocument.nationality : '',
                    docSuffix: travelDocument ? travelDocument.docSuffix : ''
                },
                residentCountry: person.residentCountry || '',
                city: person.city || ''
            },
            agent: {
                agentID: agent.agentID,
                agentIdentifier: agent.agentIdentifier,
                loginName: agent.loginName || '',
                status: agent.status,
                password: agent.password,
                authenticationType: agent.authenticationType,
                personID: agent.personID,
                departmentCode: agent.departmentCode || '',
                locationCode: agent.locationCode || '',
                forcePasswordReset: agent.forcePasswordReset,
                locked: agent.locked,
                agentRoles: agent.agentRoles
            }
        }
        return member;
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