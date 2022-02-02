import { useKeycloak } from '@react-keycloak/web';
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
    id: 7,
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
    parentDepositId: 7,
    depositType: {
      id: 'SECURITY',
      description: 'Security deposit',
      isDisabled: false,
    },
    terminationDate: '2022-02-01',
    claimsAgainst: 1234.0,
    returnAmount: 123.0,
    returnDate: '2022-02-16',
    payeeName: '',
    payeeAddress: '',
    rowVersion: 1,
  },
];

jest.mock('@react-keycloak/web');
(useKeycloak as jest.Mock).mockReturnValue({
  keycloak: {
    userInfo: {
      organizations: [1],
      roles: [],
    },
    subject: 'test',
  },
});

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

describe('DepositsContainer', () => {
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
