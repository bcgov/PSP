import { AddressTypes } from '@/constants';
import { ContactMethodTypes } from '@/constants/contactMethodType';
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
  id: id,
  name: name,
  isDisabled: false,
  alias: null,
  comment: null,
  incorporationNumber: null,
  contactMethods: [
    {
      id: 1,
      contactMethodType: toTypeCodeNullable(ContactMethodTypes.WorkPhone),
      value: '222-333-4444',
      rowVersion: null,
    },
    {
      id: 2,
      contactMethodType: toTypeCodeNullable(ContactMethodTypes.WorkMobile),
      value: '555-666-7777',

      rowVersion: null,
    },
  ],
  organizationPersons: [
    {
      personId: 3,
      person: null,
      organizationId: id,
      rowVersion: 1,
    },
  ],
  organizationAddresses: [
    {
      addressUsageType: toTypeCodeNullable(AddressTypes.Mailing),
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
  };
};
