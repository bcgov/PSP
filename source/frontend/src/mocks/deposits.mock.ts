import { ApiGen_Concepts_SecurityDeposit } from '@/models/api/generated/ApiGen_Concepts_SecurityDeposit';
import { ApiGen_Concepts_SecurityDepositReturn } from '@/models/api/generated/ApiGen_Concepts_SecurityDepositReturn';

import { getMockPerson } from './contacts.mock';
import { getMockOrganization } from './organization.mock';

export const getMockDeposits = (): ApiGen_Concepts_SecurityDeposit[] => [
  {
    id: 1,
    leaseId: 1,
    depositType: { id: 'PET', description: null, displayOrder: null, isDisabled: false },
    description: 'Pet deposit collected for one cat and one medium size dog.',
    amountPaid: 500.0,
    depositDateOnly: '2021-09-15T00:00:00',
    rowVersion: 0,
    depositReturns: getMockDepositReturns(),
    contactHolder: {
      id: 'P1',
      person: {
        ...getMockPerson({ firstName: '', id: 1, surname: '' }),
        firstName: 'test person',
      },
      organization: null,
    },
    otherTypeDescription: null,
  },
  {
    id: 7,
    leaseId: 1,
    depositType: { id: 'SECURITY', description: null, displayOrder: null, isDisabled: false },
    description:
      'Lorem ipsum dolor sit amet, consectetur adipiscing elit. \r\n\r\nInteger nec odio.',
    amountPaid: 2000.0,
    depositDateOnly: '2019-03-01T00:00:00',
    rowVersion: 0,
    depositReturns: [],
    contactHolder: {
      id: 'O1',
      organization: { ...getMockOrganization(), name: 'test organization' },
      person: null,
    },
    otherTypeDescription: null,
  },
];

export const getMockDepositReturns = (): ApiGen_Concepts_SecurityDepositReturn[] => [
  {
    id: 1,
    interestPaid: 1,
    parentDepositId: 7,
    terminationDate: '2022-02-01',
    claimsAgainst: 1234.0,
    returnAmount: 123.0,
    returnDate: '2022-02-16',
    rowVersion: 1,
    contactHolder: { id: 'P1', organization: null, person: null },
    depositType: null,
  },
];
