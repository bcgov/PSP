import { ApiGen_Concepts_LeaseTenant } from '@/models/api/generated/ApiGen_Concepts_LeaseTenant';

import { Api_GenerateOrganization } from '../GenerateOrganization';
import { Api_GeneratePerson } from '../GeneratePerson';

export class Api_GenerateTenant {
  person: Api_GeneratePerson | null;
  organization: Api_GenerateOrganization | null;
  primary_contact: Api_GeneratePerson | null;
  name: string;
  address_string: string;
  email: string;
  constructor(leaseTenant: ApiGen_Concepts_LeaseTenant) {
    this.person = leaseTenant.person ? new Api_GeneratePerson(leaseTenant.person) : null;
    this.organization = leaseTenant.organization
      ? new Api_GenerateOrganization(leaseTenant.organization)
      : null;
    this.name = this.person
      ? this.person.full_name_string
      : this.organization?.full_name_string ?? '';
    this.address_string = this.person
      ? this.person.address?.address_string_multiline_compressed ?? ''
      : this.organization?.address?.address_string_multiline_compressed ?? '';
    this.primary_contact =
      !leaseTenant.person && leaseTenant.primaryContact
        ? new Api_GeneratePerson(leaseTenant.primaryContact)
        : null;
    this.email = this.person
      ? this.person?.email ?? ''
      : this.primary_contact?.email
      ? this.primary_contact?.email ?? ''
      : this.organization?.email ?? '';
  }
}
