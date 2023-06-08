import { fromApiOrganization, fromApiPerson, IContactSearchResult } from 'interfaces';
import { isEmpty } from 'lodash';
import {
  Api_AcquisitionFileOwner,
  Api_AcquisitionFilePerson,
  Api_AcquisitionFileRepresentative,
  Api_AcquisitionFileSolicitor,
} from 'models/api/AcquisitionFile';
import { Api_Address } from 'models/api/Address';
import { NumberFieldValue } from 'typings/NumberFieldValue';
import { fromTypeCode, stringToBoolean, stringToNull, toTypeCode } from 'utils/formUtils';

export interface WithAcquisitionTeam {
  team: AcquisitionTeamFormModel[];
}

export interface WithAcquisitionOwners {
  owners: AcquisitionOwnerFormModel[];
}

export class AcquisitionTeamFormModel {
  id?: number;
  rowVersion?: number;
  contact?: IContactSearchResult;
  contactTypeCode: string;

  constructor(contactTypeCode: string, id?: number, contact?: IContactSearchResult) {
    this.id = id;
    this.contactTypeCode = contactTypeCode;
    this.contact = contact;
  }

  toApi(acquisitionFileId: number): Api_AcquisitionFilePerson {
    return {
      id: this.id,
      rowVersion: this.rowVersion,
      acquisitionFileId: acquisitionFileId,
      personId: this.contact?.personId || 0,
      person: { id: this.contact?.personId || 0 },
      personProfileType: toTypeCode(this.contactTypeCode),
      personProfileTypeCode: this.contactTypeCode,
    };
  }

  static fromApi(model: Api_AcquisitionFilePerson): AcquisitionTeamFormModel {
    const newForm = new AcquisitionTeamFormModel(
      fromTypeCode(model.personProfileType) || '',
      model.id ?? 0,
      model.person !== undefined ? fromApiPerson(model.person) : undefined,
    );

    newForm.rowVersion = model.rowVersion;
    //newForm.acquisitionFileId = model.acquisitionFileId;

    return newForm;
  }
}

export class AcquisitionSolicitorFormModel {
  contact: IContactSearchResult | null;
  id: number | null = null;
  acquisitionFileId: number | null = null;
  isDisabled: boolean | null = null;
  rowVersion: number | null = null;

  constructor(contact: IContactSearchResult | null) {
    this.contact = contact;
  }

  toApi(): Api_AcquisitionFileSolicitor {
    return {
      id: this.id ?? null,
      personId: this.contact?.personId ?? null,
      person: null,
      organizationId: !this.contact?.personId ? this.contact?.organizationId ?? null : null,
      organization: null,
      isDisabled: this.isDisabled,
      rowVersion: this.rowVersion ?? undefined,
      acquisitionFileId: this.acquisitionFileId ?? null,
    };
  }

  static fromApi(model: Api_AcquisitionFileSolicitor): AcquisitionSolicitorFormModel {
    const newForm = new AcquisitionSolicitorFormModel(
      model.person !== null
        ? fromApiPerson(model.person) ?? null
        : model.organization
        ? fromApiOrganization(model.organization)
        : null,
    );
    newForm.id = model.id;
    newForm.isDisabled = model.isDisabled;
    newForm.rowVersion = model.rowVersion ?? null;
    newForm.acquisitionFileId = model.acquisitionFileId ?? null;
    return newForm;
  }
}

export class AcquisitionRepresentativeFormModel {
  contact: IContactSearchResult | null;
  id: number | null = null;
  acquisitionFileId: number | null = null;
  comment: string | '' = '';
  isDisabled: boolean | null = null;
  rowVersion: number | null = null;

  constructor(contact: IContactSearchResult | null) {
    this.contact = contact;
  }

  toApi(): Api_AcquisitionFileRepresentative {
    return {
      personId: this.contact?.personId ?? null,
      id: this.id ?? null,
      person: null,
      comment: this.comment.trim() === '' ? null : this.comment.trim(),
      isDisabled: this.isDisabled,
      rowVersion: this.rowVersion ?? undefined,
      acquisitionFileId: this.acquisitionFileId ?? null,
    };
  }

  static fromApi(model: Api_AcquisitionFileRepresentative): AcquisitionRepresentativeFormModel {
    const newForm = new AcquisitionRepresentativeFormModel(
      model.person !== null ? fromApiPerson(model.person) ?? null : null,
    );
    newForm.id = model.id;
    newForm.isDisabled = model.isDisabled;
    newForm.comment = model.comment || '';
    newForm.rowVersion = model.rowVersion ?? null;
    newForm.acquisitionFileId = model.acquisitionFileId ?? null;
    return newForm;
  }
}

export class AcquisitionOwnerFormModel {
  id?: number;
  rowVersion?: number;
  acquisitionFileId?: number;
  isPrimaryContact: string = 'false';
  isOrganization: string = 'false';
  lastNameAndCorpName: string | '' = '';
  otherName: string | '' = '';
  givenName: string | '' = '';
  incorporationNumber: string | '' = '';
  registrationNumber: string | '' = '';
  contactEmailAddress: string | '' = '';
  contactPhoneNumber: string | '' = '';
  address?: OwnerAddressFormModel;

  isEmpty(): boolean {
    if (this.isOrganization === 'true') {
      return (
        this.lastNameAndCorpName.trim() === '' &&
        this.otherName.trim() === '' &&
        this.incorporationNumber.trim() === '' &&
        this.registrationNumber.trim() === ''
      );
    } else {
      return (
        this.givenName.trim() === '' &&
        this.lastNameAndCorpName.trim() === '' &&
        this.otherName.trim() === ''
      );
    }
  }

  toApi(): Api_AcquisitionFileOwner {
    return {
      id: this.id,
      rowVersion: this.rowVersion,
      acquisitionFileId: this.acquisitionFileId,
      isPrimaryContact: stringToBoolean(this.isPrimaryContact),
      isOrganization: this.isOrganization === 'true' ? true : false,
      lastNameAndCorpName:
        this.lastNameAndCorpName.trim() === '' ? null : this.lastNameAndCorpName.trim(),
      otherName: this.otherName.trim() === '' ? null : this.otherName.trim(),
      givenName:
        this.isOrganization === 'true'
          ? null
          : this.givenName.trim() === ''
          ? null
          : this.givenName.trim(),
      incorporationNumber:
        this.isOrganization === 'true'
          ? this.incorporationNumber.trim() === ''
            ? null
            : this.incorporationNumber.trim()
          : null,
      registrationNumber:
        this.isOrganization === 'true'
          ? this.registrationNumber.trim() === ''
            ? null
            : this.registrationNumber.trim()
          : null,
      contactEmailAddr:
        this.contactEmailAddress.trim() === '' ? null : this.contactEmailAddress.trim(),
      contactPhoneNum:
        this.contactPhoneNumber.trim() === '' ? null : this.contactPhoneNumber.trim(),
      address: OwnerAddressFormModel.toApi(this.address),
    };
  }

  static fromApi(model: Api_AcquisitionFileOwner): AcquisitionOwnerFormModel {
    const newForm = new AcquisitionOwnerFormModel();
    newForm.id = model.id;
    newForm.rowVersion = model.rowVersion;
    newForm.acquisitionFileId = model.acquisitionFileId;
    newForm.isPrimaryContact = model.isPrimaryContact ? 'true' : '';
    newForm.isOrganization = model.isOrganization ? 'true' : 'false';
    newForm.lastNameAndCorpName = model.lastNameAndCorpName || '';
    newForm.otherName = model.otherName || '';
    newForm.givenName = model.givenName || '';
    newForm.incorporationNumber = model.incorporationNumber || '';
    newForm.registrationNumber = model.registrationNumber || '';
    newForm.contactEmailAddress = model.contactEmailAddr || '';
    newForm.contactPhoneNumber = model.contactPhoneNum || '';
    newForm.address = model.address ? OwnerAddressFormModel.fromApi(model.address!) : undefined;

    return newForm;
  }
}

export class OwnerAddressFormModel {
  id?: number;
  rowVersion?: number;
  streetAddress1?: string;
  streetAddress2?: string;
  streetAddress3?: string;
  municipality?: string;
  postal?: string;
  provinceId?: NumberFieldValue;
  countryId?: NumberFieldValue;
  countryOther?: string;

  static addressLines(apiAddress: OwnerAddressFormModel | undefined): number {
    if (!apiAddress) {
      return 1;
    }

    if (apiAddress.streetAddress3?.trim() !== '') {
      return 3;
    }

    if (apiAddress.streetAddress2?.trim() !== '') {
      return 2;
    }

    return 1;
  }

  static fromApi(apiAddress: Api_Address): OwnerAddressFormModel {
    const model = new OwnerAddressFormModel();
    model.id = apiAddress.id;
    model.rowVersion = apiAddress.rowVersion;
    model.streetAddress1 = apiAddress.streetAddress1;
    model.streetAddress2 = apiAddress.streetAddress2;
    model.streetAddress3 = apiAddress.streetAddress3;
    model.municipality = apiAddress.municipality;
    model.postal = apiAddress.postal;
    model.provinceId = apiAddress.provinceStateId;
    model.countryId = apiAddress.countryId;

    return model;
  }

  static toApi(model: OwnerAddressFormModel | undefined): Api_Address | null {
    if (
      !model ||
      (isEmpty(model.streetAddress1) && isEmpty(model.municipality) && isEmpty(model.postal))
    ) {
      return null;
    }

    return {
      id: model.id,
      rowVersion: model.rowVersion,
      streetAddress1: model.streetAddress1,
      streetAddress2: stringToNull(model.streetAddress2),
      streetAddress3: stringToNull(model.streetAddress3),
      municipality: model.municipality,
      postal: model.postal,
      provinceStateId: isEmpty(model.provinceId) ? undefined : Number(model.provinceId),
      province: isEmpty(model.provinceId) ? undefined : toTypeCode(Number(model.provinceId)),
      countryId: Number(model.countryId),
      country: toTypeCode(Number(model.countryId)),
      countryOther: model.countryOther,
    };
  }
}
