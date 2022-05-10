import { useKeycloak } from '@react-keycloak/web';
import { cleanup } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Claims } from 'constants/claims';
import { Formik } from 'formik';
import { defaultFormLease, IFormLease } from 'interfaces';
import { noop } from 'lodash';
import { Api_SecurityDeposit, Api_SecurityDepositReturn } from 'models/api/SecurityDeposit';
import {
  fillInput,
  render,
  RenderOptions,
  RenderResult,
  userEvent,
  waitFor,
} from 'utils/test-utils';

import DepositsContainer from './DepositsContainer';

const mockDeposits: Api_SecurityDeposit[] = [
  {
    id: 1,
    depositType: { id: 'PET' },
    description: 'Pet deposit collected for one cat and one medium size dog.',
    amountPaid: 500.0,
    depositDate: '2021-09-15T00:00:00',
    rowVersion: 0,
    depositReturns: [],
  },
  {
    id: 7,
    depositType: { id: 'SECURITY' },
    description:
      'Lorem ipsum dolor sit amet, consectetur adipiscing elit. \r\n\r\nInteger nec odio.',
    amountPaid: 2000.0,
    depositDate: '2019-03-01T00:00:00',
    rowVersion: 0,
    depositReturns: [],
  },
];

const mockDepositReturns: Api_SecurityDepositReturn[] = [
  {
    id: 1,
    interestPaid: 1,
    parentDepositId: 7,
    terminationDate: '2022-02-01',
    claimsAgainst: 1234.0,
    returnAmount: 123.0,
    returnDate: '2022-02-16',
    rowVersion: 1,
    interestPaid: 0,
  },
];

const mockAxios = new MockAdapter(axios);
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
  afterEach(() => {
    mockAxios.reset();
    cleanup();
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

  it('saves deposit notes', async () => {
    mockAxios.onPost().reply(200, {});
    const { getByText, getByTestId, container } = setup({
      lease: {
        ...defaultFormLease,
        returnNotes: 'Tenant no longer has a dog, deposit returned, less fee for carpet cleaning',
        securityDeposits: mockDeposits,
        securityDepositReturns: mockDepositReturns,
      },
      claims: [Claims.LEASE_EDIT],
    });
    const editButton = getByTestId('edit-notes');
    userEvent.click(editButton);
    await fillInput(container, 'returnNotes', 'test note', 'textarea');
    const saveButton = getByText('Save');
    userEvent.click(saveButton);
    await waitFor(() => {
      expect(mockAxios.history.put).toHaveLength(1);
    });
  });

  it('cancels an edited deposit note', async () => {
    const { getByText, getByTestId, container } = await setup({
      lease: {
        ...defaultFormLease,
        returnNotes: 'Tenant no longer has a dog, deposit returned, less fee for carpet cleaning',
        securityDeposits: mockDeposits,
        securityDepositReturns: mockDepositReturns,
      },
      claims: [Claims.LEASE_EDIT],
    });
    const editButton = getByTestId('edit-notes');
    userEvent.click(editButton);
    const noteField = await fillInput(container, 'returnNotes', 'test note', 'textarea');
    const cancelButton = getByText('Cancel');
    userEvent.click(cancelButton);
    await waitFor(() => {
      expect(noteField.input).toHaveValue('');
    });
  });
});
