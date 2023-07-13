import { ContactMethodTypes } from '@/constants/contactMethodType';
import { getApiPersonOrOrgMailingAddress } from '@/features/contacts/contactUtils';
import { phoneFormatter } from '@/utils/formUtils';

import { Api_Organization } from '../api/Organization';
import { Api_GenerateAddress } from './GenerateAddress';
export class Api_GenerateOrganization {
  name: string;
  alias: string;
  incorporation_number: string;
  email: string;
  phone: string;
  full_name_string: string;
  address: Api_GenerateAddress | null;

  constructor(organization: Api_Organization | null | undefined) {
    this.name = organization?.name ?? '';
    this.alias = organization?.alias ?? '';
    this.incorporation_number = organization?.incorporationNumber ?? '';
    const workEmail =
      organization?.contactMethods?.filter(
        o => o.contactMethodType?.id === ContactMethodTypes.WorkEmail,
      ) ?? [];
    const personalEmail =
      organization?.contactMethods?.filter(
        o => o.contactMethodType?.id === ContactMethodTypes.PersonalEmail,
      ) ?? [];
    this.email =
      workEmail?.length > 0
        ? workEmail.map(p => p.value).join(', ')
        : personalEmail?.length > 0
        ? personalEmail.map(p => p.value).join(', ')
        : '';
    const workPhone =
      organization?.contactMethods?.filter(
        o => o.contactMethodType?.id === ContactMethodTypes.WorkPhone,
      ) ?? [];
    const personalPhone =
      organization?.contactMethods?.filter(
        o => o.contactMethodType?.id === ContactMethodTypes.PersonalPhone,
      ) ?? [];
    this.phone =
      workPhone?.length > 0
        ? workPhone.map(p => phoneFormatter(p.value)).join(', ')
        : personalPhone?.length > 0
        ? personalPhone.map(p => phoneFormatter(p.value)).join(', ')
        : '';
    this.address = organization
      ? new Api_GenerateAddress(getApiPersonOrOrgMailingAddress(organization) ?? null)
      : null;
    this.full_name_string = `${this.name} (Inc. No. ${this.incorporation_number ?? ''})`;
  }
}
