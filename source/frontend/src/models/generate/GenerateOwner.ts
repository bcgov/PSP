import { Api_AcquisitionFileOwner } from 'models/api/AcquisitionFile';

import { GenerateAddress } from './GenerateAddress';
export class GenerateOwner {
  given_name: string;
  last_name_or_corp_name: string;
  other_name: string;
  incorporation_number: string;
  registration_number: string;
  address: GenerateAddress;
  is_corporation: boolean;
  owner_string: string;
  email: string;
  is_primary_contact: boolean;

  constructor(owner: Api_AcquisitionFileOwner | null) {
    this.given_name = owner?.givenName ?? '';
    this.last_name_or_corp_name = owner?.lastNameAndCorpName ?? '';
    this.other_name = owner?.otherName ?? '';
    this.incorporation_number = owner?.incorporationNumber ?? '';
    this.registration_number = owner?.registrationNumber ?? '';
    this.address = new GenerateAddress(owner?.address ?? null);
    this.is_corporation = owner?.isOrganization ?? false;
    this.email = owner?.contactEmailAddr ?? '';
    this.is_primary_contact = this.is_primary_contact = owner?.isPrimaryContact ?? false;
    this.owner_string = this.is_corporation
      ? `${this.last_name_or_corp_name}, Inc. No. ${this.incorporation_number} (OR Reg. No. ${this.registration_number})`
      : [this.given_name, this.last_name_or_corp_name, this.other_name].filter(x => !!x).join(' ');
  }
}
