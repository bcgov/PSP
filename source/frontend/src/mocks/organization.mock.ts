import { ContactMethodTypes } from '@/constants/contactMethodType';
import { ApiGen_CodeTypes_AddressUsageTypes } from '@/models/api/generated/ApiGen_CodeTypes_AddressUsageTypes';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { toTypeCodeNullable } from '@/utils/formUtils';

import { getMockApiAddress } from './address.mock';

export const getMockOrganization = ({
  id = 2,
  name = 'Test Organization',
}: {
  id?: number;
  name?: string;
} = {}): ApiGen_Concepts_Organization => ({
  ...getEmptyOrganization(),
  id: id,
  name: name,
  isDisabled: false,
  alias: 'Fake Inc',
  comment: null,
  incorporationNumber: '987',
  contactMethods: [
    {
      id: 1,
      contactMethodType: toTypeCodeNullable(ContactMethodTypes.WorkPhone),
      value: '222-333-4444',
      personId: null,
      organizationId: 0,
      rowVersion: null,
    },
    {
      id: 2,
      contactMethodType: toTypeCodeNullable(ContactMethodTypes.WorkMobile),
      value: '555-666-7777',
      personId: null,
      organizationId: 0,
      rowVersion: null,
    },
  ],
  organizationPersons: [
    {
      id: 1,
      personId: 3,
      person: null,
      organizationId: id,
      organization: null,
      rowVersion: 1,
    },
  ],
  organizationAddresses: [
    {
      addressUsageType: toTypeCodeNullable(ApiGen_CodeTypes_AddressUsageTypes.MAILING),
      address: getMockApiAddress(),
      id: 1,
      organizationId: 1,
      rowVersion: null,
    },
  ],
  rowVersion: 1,
});

export const getEmptyOrganization = (): ApiGen_Concepts_Organization => {
  return {
    id: 0,
    isDisabled: false,
    name: null,
    alias: null,
    incorporationNumber: null,
    organizationPersons: null,
    organizationAddresses: null,
    contactMethods: null,
    comment: null,
    rowVersion: null,
    parentOrganizationId: null,
    regionCode: null,
    districtCode: null,
    organizationTypeCode: null,
    identifierTypeCode: null,
    organizationIdentifier: null,
    website: null,
    parentOrganization: null,
  };
};
