import _ from 'underscore';
import { Contracts as NskContracts, Utils as NskUtils } from 'newskies-services';
import Utils from "../Services/Utils";
import moment from "moment/moment";
import { BookingContact, Phone } from "../Contracts";

export default class BookingContactsTranslator {

    static translateFlyadealBookingContact(contact: BookingContact): NskContracts.BookingContact {
        const result: NskContracts.BookingContact = {
            typeCode: contact.typeCode,
            names: [contact.name],
            emailAddress: contact.emailAddress || '',
            homePhone: contact.homePhone ? contact.homePhone.countryCode + ' ' + contact.homePhone.number : '',
            workPhone: contact.workPhone ? contact.workPhone.countryCode + ' ' + contact.workPhone.number : '',
            otherPhone: contact.otherPhone ? contact.otherPhone.countryCode + ' ' + contact.otherPhone.number : '',
            fax: contact.fax || '',
            companyName: contact.companyName || '',
            addressLine1: contact.addressLine1 || '',
            addressLine2: contact.addressLine2 || '',
            addressLine3: contact.addressLine3 || '',
            city: contact.city || '',
            provinceState: contact.provinceState || '',
            postalCode: contact.postalCode || '',
            countryCode: contact.countryCode || '',
            cultureCode: contact.cultureCode || '',
            customerNumber: contact.customerNumber || '',
            sourceOrganization: contact.sourceOrganization || '',
        };
        return result;
    }

    static translateNewskiesBookingContact(nskContact: NskContracts.BookingContact, countries: NskContracts.Country[]): BookingContact {
        const result: BookingContact = {
            typeCode: nskContact.typeCode,
            name: nskContact.names && nskContact.names.length ? nskContact.names[0] : null,
            emailAddress: nskContact.emailAddress,
            homePhone: this.createPhone(nskContact.homePhone, countries),
            workPhone: this.createPhone(nskContact.workPhone, countries),
            otherPhone: this.createPhone(nskContact.otherPhone, countries),
            fax: nskContact.fax,
            companyName: nskContact.companyName,
            addressLine1: nskContact.addressLine1,
            addressLine2: nskContact.addressLine2,
            addressLine3: nskContact.addressLine3,
            city: nskContact.city,
            provinceState: nskContact.provinceState,
            postalCode: nskContact.postalCode,
            countryCode: nskContact.countryCode,
            cultureCode: nskContact.cultureCode,
            customerNumber: nskContact.customerNumber,
            sourceOrganization: nskContact.sourceOrganization,
        };
        return result;
    }

    private static createPhone(nskPhone: string, countries: NskContracts.Country[]): Phone {
        if (!nskPhone) {
            return null;
        }
        const parts = nskPhone.split(' ');
        if (parts.length < 2) {
            return null;
        }
        const country = countries.find(c => c.phoneCode === parts[0]);
        if (!country || !/\d+/.test(parts[1])) {
            return null;
        }
        return {
            countryCode: parts[0],
            number: Number(parts[1])
        }
    }
}