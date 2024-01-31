import { getApiPersonOrOrgMailingAddress } from '@/features/contacts/contactUtils';
import { ApiGen_Concepts_InterestHolder } from '@/models/api/generated/ApiGen_Concepts_InterestHolder';
import { formatNames } from '@/utils/personUtils';

import { Api_GenerateAddress } from '../GenerateAddress';
import { Api_GeneratePerson } from '../GeneratePerson';

export class Api_GenerateInterestHolder {
  is_organization: boolean;
  full_name_string: string;
  address: Api_GenerateAddress | null;
  primary_contact: Api_GeneratePerson | null;

  constructor(interestHolder: ApiGen_Concepts_InterestHolder | null) {
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
    this.primary_contact =
      this.is_organization && interestHolder?.primaryContact
        ? new Api_GeneratePerson(interestHolder?.primaryContact)
        : null;
  }
}
