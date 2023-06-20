import { Api_SecurityDeposit, Api_SecurityDepositReturn } from '@/models/api/SecurityDeposit';

export const getMockDeposits = (): Api_SecurityDeposit[] => [
  {
    id: 1,
    leaseId: 1,
    depositType: { id: 'PET' },
    description: 'Pet deposit collected for one cat and one medium size dog.',
    amountPaid: 500.0,
    depositDate: '2021-09-15T00:00:00',
    rowVersion: 0,
    depositReturns: getMockDepositReturns(),
    contactHolder: { id: 'P1', person: { firstName: 'test person' } },
    otherTypeDescription: null,
  },
  {
    id: 7,
    leaseId: 1,
    depositType: { id: 'SECURITY' },
    description:
      'Lorem ipsum dolor sit amet, consectetur adipiscing elit. \r\n\r\nInteger nec odio.',
    amountPaid: 2000.0,
    depositDate: '2019-03-01T00:00:00',
    rowVersion: 0,
    depositReturns: [],
    contactHolder: { id: 'O1', organization: { name: 'test organization' } },
    otherTypeDescription: null,
  },
];

export const getMockDepositReturns = (): Api_SecurityDepositReturn[] => [
  {
    id: 1,
    interestPaid: 1,
    parentDepositId: 7,
    terminationDate: '2022-02-01',
    claimsAgainst: 1234.0,
    returnAmount: 123.0,
    returnDate: '2022-02-16',
    rowVersion: 1,
    contactHolder: { id: 'P1' },
  },
];
