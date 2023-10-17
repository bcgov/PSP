import { getApiPersonOrOrgMailingAddress } from '@/features/contacts/contactUtils';
import { Api_AcquisitionFileOwner } from '@/models/api/AcquisitionFile';
import { phoneFormatter } from '@/utils/formUtils';

import { Api_Organization } from '../api/Organization';
import { Api_Person } from '../api/Person';
import { Api_GenerateAddress } from './GenerateAddress';

export class Api_GenerateOwner {
  given_name: string;
  last_name_or_corp_name: string;
  other_name: string;
  formatted_other_name: string;
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
    this.formatted_other_name = owner?.otherName ? `(${owner?.otherName})` : '';
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
      : [this.given_name, this.last_name_or_corp_name, this.formatted_other_name]
          .filter(x => !!x)
          .join(' ');
  }

  static fromApiPerson(model: Api_Person): Api_GenerateOwner {
    let generateOwner = new Api_GenerateOwner(null);
    generateOwner.given_name = model.firstName ?? '';
    generateOwner.last_name_or_corp_name = model.surname ?? '';
    generateOwner.other_name = model.middleNames ?? '';
    generateOwner.incorporation_number = '';
    generateOwner.registration_number = '';
    generateOwner.address = new Api_GenerateAddress(getApiPersonOrOrgMailingAddress(model) ?? null);
    generateOwner.is_corporation = false;
    generateOwner.owner_string = [
      generateOwner.given_name,
      generateOwner.last_name_or_corp_name,
      generateOwner.other_name,
    ]
      .filter(x => !!x)
      .join(' ');

    return generateOwner;
  }

  static fromApiOrganization(model: Api_Organization): Api_GenerateOwner {
    let generateOwner = new Api_GenerateOwner(null);

    generateOwner.given_name = '';
    generateOwner.last_name_or_corp_name = model.name ?? '';
    generateOwner.other_name = model.alias ?? '';
    generateOwner.incorporation_number = model.incorporationNumber ?? '';
    generateOwner.registration_number = '';
    generateOwner.address = new Api_GenerateAddress(getApiPersonOrOrgMailingAddress(model) ?? null);
    generateOwner.is_corporation = true;
    generateOwner.owner_string = generateOwner.incorporation_number
      ? `${generateOwner.last_name_or_corp_name}, Inc. No. ${generateOwner.incorporation_number}`
      : `${generateOwner.last_name_or_corp_name}`;

    return generateOwner;
  }
}
