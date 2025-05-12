import { IContactSearchResult } from '@/interfaces';
import { ApiGen_CodeTypes_AddressUsageTypes } from '@/models/api/generated/ApiGen_CodeTypes_AddressUsageTypes';
import { ApiGen_Concepts_Address } from '@/models/api/generated/ApiGen_Concepts_Address';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { ApiGen_Concepts_PersonOrganization } from '@/models/api/generated/ApiGen_Concepts_PersonOrganization';
import { isValidId } from '@/utils';
import { formatNames } from '@/utils/personUtils';

export function getApiPersonOrOrgMailingAddress(
  contact: ApiGen_Concepts_Person | ApiGen_Concepts_Organization,
): ApiGen_Concepts_Address | null {
  if (!contact) {
    return null;
  }

  return (
    (contact as ApiGen_Concepts_Person).personAddresses?.find(
      addr =>
        addr?.addressUsageType?.id === ApiGen_CodeTypes_AddressUsageTypes.MAILING && addr.address,
    )?.address ??
    (contact as ApiGen_Concepts_Organization).organizationAddresses?.find(
      addr => addr?.addressUsageType?.id === ApiGen_CodeTypes_AddressUsageTypes.MAILING,
    )?.address ??
    null
  );
}

export const getDefaultContact = (organization?: {
  organizationPersons: ApiGen_Concepts_PersonOrganization[] | null;
}): ApiGen_Concepts_Person | null => {
  if (organization?.organizationPersons?.length === 1) {
    return organization.organizationPersons[0].person;
  }
  return null;
};

export const getPrimaryContact = (
  primaryContactId: number,
  organization?: {
    organizationPersons?: ApiGen_Concepts_PersonOrganization[];
  },
): ApiGen_Concepts_Person | null => {
  return (
    organization?.organizationPersons?.find(op => op.personId === primaryContactId)?.person ?? null
  );
};

export function formatContactSearchResult(contact: IContactSearchResult, defaultText = ''): string {
  let text = defaultText;
  if (isValidId(contact?.personId)) {
    text = formatNames([contact.firstName, contact.middleNames, contact.surname]);
  } else if (isValidId(contact?.organizationId)) {
    text = contact.organizationName || '';
  }
  return text;
}
