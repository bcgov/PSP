import { AddressTypes } from '@/constants';
import { ContactMethodTypes } from '@/constants/contactMethodType';
import { Api_Organization } from '@/models/api/Organization';

import { getMockApiAddress } from './address.mock';

export const getMockOrganization = ({
  id = 2,
  name = 'Test Organization',
}: {
  id?: number;
  name?: string;
} = {}): Api_Organization => ({
  id: id,
  name: name,
  isDisabled: false,
  contactMethods: [
    { contactMethodType: { id: ContactMethodTypes.WorkPhone }, value: '222-333-4444' },
    { contactMethodType: { id: ContactMethodTypes.WorkMobile }, value: '555-666-7777' },
  ],
  organizationPersons: [
    {
      personId: 3,
      organizationId: id,
      isDisabled: false,
      rowVersion: 1,
    },
  ],
  organizationAddresses: [
    { addressUsageType: { id: AddressTypes.Mailing }, address: getMockApiAddress() },
  ],
  rowVersion: 1,
});
