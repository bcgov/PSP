import { getApiPersonOrOrgMailingAddress } from '@/features/contacts/contactUtils';

import { Api_Organization } from '../api/Organization';
import { Api_GenerateAddress } from './GenerateAddress';
export class Api_GenerateOrganization {
  name: string;
  alias: string;
  incorporation_number: string;
  full_name_string: string;
  address: Api_GenerateAddress | null;

  constructor(organization: Api_Organization | null | undefined) {
    this.name = organization?.name ?? '';
    this.alias = organization?.alias ?? '';
    this.incorporation_number = organization?.incorporationNumber ?? '';
    this.address = organization
      ? new Api_GenerateAddress(getApiPersonOrOrgMailingAddress(organization) ?? null)
      : null;
    this.full_name_string = `${this.name} (Inc. No. ${this.incorporation_number ?? ''})`;
  }
}
