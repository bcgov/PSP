import { Formik } from 'formik';
import {
  defaultFormLease,
  IFormLease,
  ILeaseSecurityDeposit,
  ILeaseSecurityDepositReturn,
} from 'interfaces';
import { noop } from 'lodash';
import { render, RenderOptions, RenderResult } from 'utils/test-utils';

import DepositsContainer from './DepositsContainer';

const mockDeposits: ILeaseSecurityDeposit[] = [
  {
    id: 1,
    depositType: { id: 'PET' },
    description: 'Pet deposit collected for one cat and one medium size dog.',
    amountPaid: 500.0,
    depositDate: '2021-09-15T00:00:00',
    rowVersion: 0,
  },
  {
    id: 2,
    depositType: { id: 'SECURITY' },
    description:
      'Lorem ipsum dolor sit amet, consectetur adipiscing elit. \r\n\r\nInteger nec odio.',
    amountPaid: 2000.0,
    depositDate: '2019-03-01T00:00:00',
    rowVersion: 0,
  },
];

const mockDepositReturns: ILeaseSecurityDepositReturn[] = [
  {
    id: 1,
    securityDepositTypeId: 'PET',
    securityDepositType: 'Pet deposit',
    terminationDate: '2020-11-15T00:00:00',
    depositTotal: 2084.0,
    claimsAgainst: 200.0,
    returnAmount: 200.0,
    returnDate: '2021-03-25T00:00:00',
    chequeNumber: '20-12780',
    payeeName: 'John Smith',
    payeeAddress: '1020 Skid Row',
  },
];

const setup = (renderOptions: RenderOptions & { lease?: IFormLease } = {}): RenderResult => {
  // render component under test
  const result = render(
    <Formik onSubmit={noop} initialValues={renderOptions.lease ?? defaultFormLease}>
      <DepositsContainer />
    </Formik>,
    {
      ...renderOptions,
    },
  );
  return { ...result };
};

describe('Lease Deposits', () => {
  beforeEach(() => {
    Date.now = jest.fn().mockReturnValue(new Date('2020-10-15T18:33:37.000Z'));
  });
  afterAll(() => {
    jest.restoreAllMocks();
  });
  it('renders as expected', () => {
    const result = setup({
      lease: {
        ...defaultFormLease,
        returnNotes: 'Tenant no longer has a dog, deposit returned, less fee for carpet cleaning',
        securityDeposits: mockDeposits,
        securityDepositReturns: mockDepositReturns,
      },
    });
    expect(result.asFragment()).toMatchSnapshot();
  });
});
