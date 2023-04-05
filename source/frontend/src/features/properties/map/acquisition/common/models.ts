import { fromApiPerson, IContactSearchResult } from 'interfaces';
import { isEmpty } from 'lodash';
import { Api_AcquisitionFileOwner, Api_AcquisitionFilePerson } from 'models/api/AcquisitionFile';
import { Api_Address } from 'models/api/Address';
import { NumberFieldValue } from 'typings/NumberFieldValue';
import { fromTypeCode, stringToNull, toTypeCode } from 'utils/formUtils';

export interface WithAcquisitionTeam {
  team: AcquisitionTeamFormModel[];
}

export interface WithAcquisitionOwners {
  owners: AcquisitionOwnerFormModel[];
}

export class AcquisitionTeamFormModel {
  contact?: IContactSearchResult;
  contactTypeCode: string;

  constructor(contactTypeCode: string, contact?: IContactSearchResult) {
    this.contactTypeCode = contactTypeCode;
    this.contact = contact;
  }

  toApi(): Api_AcquisitionFilePerson {
    return {
      personId: this.contact?.personId || 0,
      person: { id: this.contact?.personId || 0 },
      personProfileType: toTypeCode(this.contactTypeCode),
      personProfileTypeCode: this.contactTypeCode,
    };
  }

  static fromApi(model: Api_AcquisitionFilePerson): AcquisitionTeamFormModel {
    const newForm = new AcquisitionTeamFormModel(
      fromTypeCode(model.personProfileType) || '',
      model.person !== undefined ? fromApiPerson(model.person) : undefined,
    );
    return newForm;
  }
}

export class AcquisitionOwnerFormModel {
  id?: number;
  rowVersion?: number;
  acquisitionFileId?: number;
  lastNameOrCorp1?: string;
  lastNameOrCorp2?: string;
  givenName?: string;
  incorporationNumber?: string;
  address?: OwnerAddressFormModel;

  isEmpty(): boolean {
    return (
      this.lastNameOrCorp1?.trim() === '' &&
      this.lastNameOrCorp2?.trim() === '' &&
      this.givenName?.trim() === '' &&
      this.incorporationNumber?.trim() === ''
    );
  }

  toApi(): Api_AcquisitionFileOwner {
    return {
      id: this.id,
      rowVersion: this.rowVersion,
      acquisitionFileId: this.acquisitionFileId,
      lastNameOrCorp1: this.lastNameOrCorp1,
      lastNameOrCorp2: this.lastNameOrCorp2,
      givenName: this.givenName,
      incorporationNumber: this.incorporationNumber,
      address: OwnerAddressFormModel.toApi(this.address),
    };
  }

  static fromApi(model: Api_AcquisitionFileOwner): AcquisitionOwnerFormModel {
    const newForm = new AcquisitionOwnerFormModel();
    newForm.id = model.id;
    newForm.rowVersion = model.rowVersion;
    newForm.acquisitionFileId = model.acquisitionFileId;
    newForm.lastNameOrCorp1 = model.lastNameOrCorp1;
    newForm.lastNameOrCorp2 = model.lastNameOrCorp2;
    newForm.givenName = model.givenName;
    newForm.incorporationNumber = model.incorporationNumber;
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

  static toApi(model: OwnerAddressFormModel | undefined): Api_Address | undefined {
    if (
      !model ||
      (isEmpty(model.streetAddress1) && isEmpty(model.municipality) && isEmpty(model.postal))
    ) {
      return undefined;
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
