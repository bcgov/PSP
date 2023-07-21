import { getApiPersonOrOrgMailingAddress } from '@/features/contacts/contactUtils';
import { Api_InterestHolder } from '@/models/api/InterestHolder';
import { formatNames } from '@/utils/personUtils';

import { Api_GenerateAddress } from '../GenerateAddress';

export class Api_GenerateExpropriationInterestHolder {
  is_organization: boolean;
  full_name_string: string;
  address: Api_GenerateAddress | null;
  org_primary_contact: string;

  constructor(interestHolder: Api_InterestHolder | null) {
    this.is_organization = !!interestHolder?.organization ?? false;
    this.full_name_string = this.is_organization
      ? `${interestHolder?.organization?.name ?? ''} (Inc. No. ${
          interestHolder?.organization?.incorporationNumber ?? ''
        })`
      : formatNames([
          interestHolder?.person?.firstName,
          interestHolder?.person?.middleNames,
          interestHolder?.person?.surname,
        ]);
    this.address = interestHolder?.person
      ? new Api_GenerateAddress(getApiPersonOrOrgMailingAddress(interestHolder.person) ?? null)
      : interestHolder?.organization
      ? new Api_GenerateAddress(
          getApiPersonOrOrgMailingAddress(interestHolder.organization) ?? null,
        )
      : null;
    this.org_primary_contact = ''; // TODO
  }
}
