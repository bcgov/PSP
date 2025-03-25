import { isEmpty } from 'lodash';

import { FileTeamFormModel } from '@/features/mapSideBar/shared/models';
import { ApiGen_Concepts_AcquisitionFileOwner } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileOwner';
import { ApiGen_Concepts_Address } from '@/models/api/generated/ApiGen_Concepts_Address';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { NumberFieldValue } from '@/typings/NumberFieldValue';
import { stringToBoolean, stringToNull, toTypeCodeConcept } from '@/utils/formUtils';

export interface WithAcquisitionTeam {
  team: FileTeamFormModel[];
}

export interface WithAcquisitionOwners {
  owners: AcquisitionOwnerFormModel[];
}

export class AcquisitionOwnerFormModel {
  id?: number;
  rowVersion?: number;
  acquisitionFileId?: number;
  isPrimaryContact = 'false';
  isOrganization = 'false';
  lastNameAndCorpName: string | '' = '';
  otherName: string | '' = '';
  givenName: string | '' = '';
  incorporationNumber: string | '' = '';
  registrationNumber: string | '' = '';
  contactEmailAddress: string | '' = '';
  contactPhoneNumber: string | '' = '';
  address?: OwnerAddressFormModel = new OwnerAddressFormModel();

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

  toApi(): ApiGen_Concepts_AcquisitionFileOwner {
    return {
      id: this.id ?? 0,
      acquisitionFileId: this.acquisitionFileId ?? 0,
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
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }

  static fromApi(model: ApiGen_Concepts_AcquisitionFileOwner): AcquisitionOwnerFormModel {
    const newForm = new AcquisitionOwnerFormModel();
    newForm.id = model.id;
    newForm.rowVersion = model.rowVersion ?? undefined;
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
    newForm.address = model.address
      ? OwnerAddressFormModel.fromApi(model.address)
      : new OwnerAddressFormModel();

    return newForm;
  }
}

export class OwnerAddressFormModel {
  id?: number;
  rowVersion?: number;
  streetAddress1?: string = '';
  streetAddress2?: string = '';
  streetAddress3?: string = '';
  municipality?: string = '';
  postal?: string = '';
  provinceId?: NumberFieldValue = '';
  countryId?: NumberFieldValue = 1;
  countryOther?: string = '';

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

  static fromApi(apiAddress: ApiGen_Concepts_Address): OwnerAddressFormModel {
    const model = new OwnerAddressFormModel();
    model.id = apiAddress.id ?? undefined;
    model.rowVersion = apiAddress.rowVersion ?? undefined;
    model.streetAddress1 = apiAddress.streetAddress1 ?? undefined;
    model.streetAddress2 = apiAddress.streetAddress2 ?? undefined;
    model.streetAddress3 = apiAddress.streetAddress3 ?? undefined;
    model.municipality = apiAddress.municipality ?? undefined;
    model.postal = apiAddress.postal ?? undefined;
    model.provinceId = apiAddress.provinceStateId ?? undefined;
    model.countryId = apiAddress.countryId ?? undefined;
    model.countryOther = apiAddress.countryOther ?? undefined;

    return model;
  }

  static toApi(model: OwnerAddressFormModel | undefined): ApiGen_Concepts_Address | null {
    if (
      !model ||
      (isEmpty(model.streetAddress1) && isEmpty(model.municipality) && isEmpty(model.postal))
    ) {
      return null;
    }

    return {
      id: model.id ?? null,
      rowVersion: model.rowVersion ?? null,
      streetAddress1: model.streetAddress1 ?? null,
      streetAddress2: stringToNull(model.streetAddress2),
      streetAddress3: stringToNull(model.streetAddress3),
      municipality: model.municipality ?? null,
      postal: model.postal ?? null,
      provinceStateId: isEmpty(model.provinceId) ? null : Number(model.provinceId),
      province: isEmpty(model.provinceId) ? null : toTypeCodeConcept(Number(model.provinceId)),
      countryId: Number(model.countryId),
      country: toTypeCodeConcept(Number(model.countryId)),
      countryOther: model.countryOther ?? null,
      comment: null,
      district: null,
      latitude: null,
      longitude: null,
      region: null,
      districtCode: null,
      regionCode: null,
    };
  }
}
