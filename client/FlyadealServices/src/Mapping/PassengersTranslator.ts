import _ from 'underscore';
import { Contracts as NskContracts, Utils as NskUtils } from 'newskies-services';
import Utils from "../Services/Utils";
import moment from "moment";
import { Passenger, PaxFee } from "../Contracts";
import TripTranslator from "./TripTranslator";

const DATE_FORMAT = 'YYYY-MM-DDT00:00:00';

export default class PassengersTranslator {

    static translateFlyadealPassengers(passengers: Passenger[]): NskContracts.Passenger[] {
        const infants = passengers.filter(p => p.paxType === 'INFT');
        const nonInfants = passengers.filter(p => p.paxType !== 'INFT');
        const result = nonInfants.map(pax => {
            const infant = infants.find(i => i.passengerNumber === pax.passengerNumber);
            const paxInfant: NskContracts.PassengerInfant = infant ? {
                dob: moment(infant.dateOfBirth).locale('en').format(DATE_FORMAT),
                gender: !NskUtils.enumsEqual(infant.gender, NskContracts.Gender.Unmapped, NskContracts.Gender) ? infant.gender : NskContracts.Gender.Male,
                nationality: infant.nationality,
                residentCountry: infant.residentCountry,
                names: [{
                    firstName: infant.firstName,
                    lastName: infant.lastName,
                    title: infant.title,
                    middleName: infant.middleName,
                    suffix: ''
                }],
                paxType: '',
            } : undefined;
            const passenger: NskContracts.Passenger = {
                passengerNumber: pax.passengerNumber,
                paxDiscountCode: pax.discountCode,
                names: [{
                    firstName: pax.firstName,
                    lastName: pax.lastName,
                    title: pax.title,
                    middleName: pax.middleName,
                    suffix: ''
                }],
                infant: paxInfant,
                passengerFees: [],
                passengerInfo: {
                    nationality: pax.nationality,
                    residentCountry: pax.residentCountry,
                    gender: pax.gender,
                },
                // passengerAddresses: PassengerAddress[];
                passengerTravelDocuments: [{
                    docTypeCode: pax.travelDocument.docTypeCode,
                    issuedByCode: pax.travelDocument.docIssuingCountry,
                    docSuffix: pax.travelDocument.docSuffix,
                    docNumber: pax.travelDocument.docNumber,
                    dob: moment(pax.dateOfBirth).locale('en').format(DATE_FORMAT),
                    gender: pax.gender,
                    nationality: pax.nationality,
                    expirationDate: pax.travelDocument.expirationDate ? moment(pax.travelDocument.expirationDate).locale('en').format(DATE_FORMAT) : '',
                    names: [
                        { title: pax.title, firstName: pax.firstName, middleName: pax.middleName, lastName: pax.lastName, suffix: '' }
                    ],
                    birthCountry: pax.travelDocument.birthCountry,
                    issuedDate: pax.travelDocument.issuedDate ? moment(pax.travelDocument.issuedDate).locale('en').format(DATE_FORMAT) : '',
                }],
                // passengerBags: PassengerBag[];
                // passengerID: number;
                //passengerTypeInfos: PassengerTypeInfo[];
                //passengerInfos: PassengerInfo[];
                //passengerInfants: PassengerInfant[];
                //pseudoPassenger: boolean;
                passengerTypeInfo: {
                    dob: moment(pax.dateOfBirth).locale('en').format(DATE_FORMAT),
                    paxType: pax.paxType
                }
            };
            if (infant) {
                passenger.passengerTravelDocuments.push({
                    docTypeCode: infant.travelDocument.docTypeCode,
                    issuedByCode: infant.travelDocument.docIssuingCountry,
                    docSuffix: 'I',
                    docNumber: infant.travelDocument.docNumber,
                    dob: moment(infant.dateOfBirth).locale('en').format(DATE_FORMAT),
                    // HACK - Infant document is stored in the adult's travel document collection.
                    // FD Newskies doesn't allow to have multiple travel documents for the passenger with different Genders
                    // Therefore assigning adult's gender to infant travel document
                    gender: passenger.passengerInfo.gender,//infant.gender,
                    nationality: infant.nationality,
                    expirationDate: infant.travelDocument.expirationDate ? moment(infant.travelDocument.expirationDate).locale('en').format(DATE_FORMAT) : '',
                    names: [
                        { title: infant.title, firstName: infant.firstName, middleName: infant.middleName, lastName: infant.lastName, suffix: '' }
                    ],
                    birthCountry: infant.travelDocument.birthCountry,
                    issuedDate: infant.travelDocument.issuedDate ? moment(infant.travelDocument.issuedDate).locale('en').format(DATE_FORMAT) : '',
                });
            }
            return passenger;
        });
        return result;
    }

    static translateNewskiesPassengers(nskPaxes: NskContracts.Passenger[]): Passenger[] {
        const result = nskPaxes.map(pax => {
            const travelDocument = pax.passengerTravelDocuments && pax.passengerTravelDocuments.length ? pax.passengerTravelDocuments[0] : null;
            const dateOfBirth = moment(pax.passengerTypeInfo.dob);
            const passenger: Passenger = {
                passengerNumber: pax.passengerNumber,
                title: pax.names && pax.names.length ? pax.names[0].title : '',
                firstName: pax.names && pax.names.length ? pax.names[0].firstName : '',
                middleName: pax.names && pax.names.length ? pax.names[0].middleName : '',
                lastName: pax.names && pax.names.length ? pax.names[0].lastName : '',
                dateOfBirth: dateOfBirth.isBefore(moment()) ? dateOfBirth : null,//moment(pax.passengerTypeInfo.dob),
                paxType: pax.passengerTypeInfo.paxType,
                nationality: pax.passengerInfo.nationality,
                discountCode: pax.paxDiscountCode,
                residentCountry: pax.passengerInfo.residentCountry,
                gender: pax.passengerInfo.gender,
                otherFees: PassengersTranslator.createPaxOtherFees(pax),
                travelDocument: {
                    birthCountry: travelDocument ? travelDocument.birthCountry : '',
                    docNumber: travelDocument ? travelDocument.docNumber : '',
                    docTypeCode: travelDocument ? travelDocument.docTypeCode : '',
                    docIssuingCountry: travelDocument ? travelDocument.issuedByCode : '',
                    expirationDate: travelDocument ? moment(travelDocument.expirationDate) : null,
                    nationality: travelDocument ? travelDocument.nationality : '',
                    docSuffix: travelDocument ? travelDocument.docSuffix : '',
                    issuedDate: travelDocument ? moment(travelDocument.issuedDate) : null,
                }
            };
            return passenger;
        });
        const infantPassengers = nskPaxes.map(pax => PassengersTranslator.getInfantPassenger(pax)).filter(infant => infant);
        return [...result, ...infantPassengers];
    }

    // filter booking level passenger fees
    private static createPaxOtherFees(passenger: NskContracts.Passenger): PaxFee[] {
        return passenger && passenger.passengerFees ? passenger.passengerFees
            .filter(fee => NskUtils.enumsEqual(fee.feeApplicationType, NskContracts.FeeApplicationType.None, NskContracts.FeeApplicationType)
                || NskUtils.enumsEqual(fee.feeApplicationType, NskContracts.FeeApplicationType.PNR, NskContracts.FeeApplicationType))
            .map(fee => TripTranslator.translatePaxFee(fee, passenger.passengerNumber)) : [];
    }

    private static getInfantPassenger(pax: NskContracts.Passenger): Passenger {
        const infantFee = pax.passengerFees && pax.passengerFees.some(f => f.feeCode === 'INFT');
        if (!infantFee) {
            return undefined;
        }
        const infantName = pax.infant && pax.infant.names ? pax.infant.names[0] : undefined;
        const infant: Passenger = {
            passengerNumber: pax.passengerNumber,
            title: infantName ? infantName.title : '',
            firstName: infantName ? infantName.firstName : '',
            middleName: infantName ? infantName.middleName : '',
            lastName: infantName ? infantName.lastName : '',
            dateOfBirth: pax.infant ? moment(pax.infant.dob) : null,
            paxType: 'INFT',
            nationality: pax.infant ? pax.infant.nationality : '',
            residentCountry: pax.infant ? pax.infant.residentCountry : '',
            gender: pax.infant ? pax.infant.gender : NskContracts.Gender.Unmapped,
            otherFees: []
            // discountCode: pax.paxDiscountCode
        };
        let infantDoc;
        if (pax.passengerTravelDocuments && pax.infant && pax.infant.names) {
            infantDoc = pax.passengerTravelDocuments.find(doc => doc.names && doc.names.some(name => {
                const title = name && name.title ? name.title.toUpperCase() : '';
                const firstName = name && name.firstName ? name.firstName.toUpperCase() : '';
                const lastName = name && name.lastName ? name.lastName.toUpperCase() : '';
                return infant.title.toUpperCase() === title.toUpperCase() && infant.firstName.toUpperCase() === firstName.toUpperCase() && infant.lastName.toUpperCase() === lastName.toUpperCase()
            }));
        }
        infant.travelDocument = {
            birthCountry: infantDoc ? infantDoc.birthCountry : '',
            docNumber: infantDoc ? infantDoc.docNumber : '',
            docTypeCode: infantDoc ? infantDoc.docTypeCode : '',
            docIssuingCountry: infantDoc ? infantDoc.issuedByCode : '',
            expirationDate: infantDoc ? moment(infantDoc.expirationDate) : null,
            nationality: infantDoc ? infantDoc.nationality : '',
            docSuffix: infantDoc ? infantDoc.docSuffix : '',
            issuedDate: infantDoc ? moment(infantDoc.issuedDate) : null,
        };
        return infant;
    }
}