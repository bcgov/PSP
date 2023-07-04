import { getApiPersonOrOrgMailingAddress } from '@/features/contacts/contactUtils';
import { IEditableOrganization } from '@/interfaces/editable-contact';
import {
  Api_AcquisitionFileOwner,
  Api_AcquisitionFileRepresentative,
} from '@/models/api/AcquisitionFile';
import { phoneFormatter } from '@/utils/formUtils';

import { Api_InterestHolder } from '../api/InterestHolder';
import { Api_Person } from '../api/Person';
import { Api_GenerateAddress } from './GenerateAddress';

export class Api_GenerateOwner {
  given_name: string;
  last_name_or_corp_name: string;
  other_name: string;
  incorporation_number: string;
  registration_number: string;
  address: Api_GenerateAddress;
  is_corporation: boolean;
  owner_string: string;
  email: string;
  phone: string;
  is_primary_contact: boolean;

  constructor(owner: Api_AcquisitionFileOwner | null) {
    this.given_name = owner?.givenName ?? '';
    this.last_name_or_corp_name = owner?.lastNameAndCorpName ?? '';
    this.other_name = owner?.otherName ?? '';
    this.incorporation_number = owner?.incorporationNumber ?? '';
    this.registration_number = owner?.registrationNumber ?? '';
    this.address = new Api_GenerateAddress(owner?.address ?? null);
    this.is_corporation = owner?.isOrganization ?? false;
    this.email = owner?.contactEmailAddr ?? '';
    this.phone =
      owner?.contactPhoneNum !== undefined && owner?.contactPhoneNum !== null
        ? phoneFormatter(owner.contactPhoneNum)
        : '';
    this.is_primary_contact = this.is_primary_contact = owner?.isPrimaryContact ?? false;
    this.owner_string = this.is_corporation
      ? `${this.last_name_or_corp_name}, Inc. No. ${this.incorporation_number} (OR Reg. No. ${this.registration_number})`
      : [this.given_name, this.last_name_or_corp_name, this.other_name].filter(x => !!x).join(' ');
  }

  static fromInterestHolder(model: Api_InterestHolder): Api_GenerateOwner {
    let generateOwner = new Api_GenerateOwner(null);

    return generateOwner;
  }

  static fromApiPerson(model: Api_Person): Api_GenerateOwner {
    let generateOwner = new Api_GenerateOwner(null);

    const apiAddress = getApiPersonOrOrgMailingAddress(model);
    generateOwner.address = new Api_GenerateAddress(apiAddress ?? null);

    return generateOwner;
  }

  static fromOrganization(model: IEditableOrganization): Api_GenerateOwner {
    let generateOwner = new Api_GenerateOwner(null);

    const apiAddress = getApiPersonOrOrgMailingAddress(model);
    generateOwner.address = new Api_GenerateAddress(apiAddress ?? null);

    return generateOwner;
  }

  static fromOwnerRepresentative(model: Api_AcquisitionFileRepresentative): Api_GenerateOwner {
    let generateOwner = new Api_GenerateOwner(null);

    const apiAddress = getApiPersonOrOrgMailingAddress(model.person!);
    generateOwner.address = new Api_GenerateAddress(apiAddress ?? null);

    return generateOwner;
  }
}
