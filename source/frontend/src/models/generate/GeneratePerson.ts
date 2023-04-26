import { getApiPersonOrOrgMailingAddress } from 'features/contacts/contactUtils';
import { Api_Person } from 'models/api/Person';

import { GenerateAddress } from './GenerateAddress';
export class GeneratePerson {
  given_name: string;
  middle_names: string;
  last_name: string;
  email: string;
  organizations: string;
  full_name_string: string;
  address: GenerateAddress | null;

  constructor(person: Api_Person | null | undefined) {
    this.given_name = person?.firstName ?? '';
    this.middle_names = person?.middleNames ?? '';
    this.last_name = person?.surname ?? '';
    const workEmail =
      person?.contactMethods?.filter(p => p.contactMethodType?.id === 'WORKEMAIL') ?? [];
    const personalEmail =
      person?.contactMethods?.filter(p => p.contactMethodType?.id === 'PERSEMAIL') ?? [];
    this.email =
      workEmail?.length > 0
        ? workEmail.map(p => p.value).join(', ')
        : personalEmail?.length > 0
        ? personalEmail.map(p => p.value).join(', ')
        : '';
    this.organizations =
      person?.personOrganizations?.map(o => o.organization?.name).join(', ') ?? '';
    this.address = person
      ? new GenerateAddress(getApiPersonOrOrgMailingAddress(person) ?? null)
      : null;
    this.full_name_string = [this.given_name, this.middle_names, this.last_name]
      .filter(x => !!x)
      .join(' ');
  }
}
