import { ContactMethodTypes } from '@/constants/contactMethodType';
import { getApiPersonOrOrgMailingAddress } from '@/features/contacts/contactUtils';
import { phoneFormatter } from '@/utils/formUtils';
import { formatNames } from '@/utils/personUtils';

import { ApiGen_Concepts_Person } from '../api/generated/ApiGen_Concepts_Person';
import { Api_GenerateAddress } from './GenerateAddress';

export class Api_GeneratePerson {
  given_name: string;
  middle_names: string;
  last_name: string;
  email: string;
  organizations: string;
  full_name_string: string;
  address: Api_GenerateAddress | null;
  phone: string;

  constructor(person: ApiGen_Concepts_Person | null | undefined) {
    this.given_name = person?.firstName ?? '';
    this.middle_names = person?.middleNames ?? '';
    this.last_name = person?.surname ?? '';
    const workEmail =
      person?.contactMethods?.filter(
        p => p.contactMethodType?.id === ContactMethodTypes.WorkEmail,
      ) ?? [];
    const personalEmail =
      person?.contactMethods?.filter(
        p => p.contactMethodType?.id === ContactMethodTypes.PersonalEmail,
      ) ?? [];
    this.email =
      workEmail?.length > 0
        ? workEmail.map(p => p.value).join(', ')
        : personalEmail?.length > 0
        ? personalEmail.map(p => p.value).join(', ')
        : '';
    const workPhone =
      person?.contactMethods?.filter(
        p => p.contactMethodType?.id === ContactMethodTypes.WorkPhone,
      ) ?? [];
    const personalPhone =
      person?.contactMethods?.filter(
        p => p.contactMethodType?.id === ContactMethodTypes.PersonalPhone,
      ) ?? [];
    this.phone =
      workPhone?.length > 0
        ? workPhone.map(p => phoneFormatter(p.value)).join(', ')
        : personalPhone?.length > 0
        ? personalPhone.map(p => phoneFormatter(p.value)).join(', ')
        : '';
    this.organizations =
      person?.personOrganizations?.map(o => o.organization?.name).join(', ') ?? '';
    this.address = person
      ? new Api_GenerateAddress(getApiPersonOrOrgMailingAddress(person) ?? null)
      : null;
    this.full_name_string = formatNames([this.given_name, this.middle_names, this.last_name]);
  }
}
