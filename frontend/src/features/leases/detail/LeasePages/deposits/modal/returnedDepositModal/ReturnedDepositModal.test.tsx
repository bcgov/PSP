import userEvent from '@testing-library/user-event';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';
import { mockLookups } from 'mocks/mockLookups';
import { Api_SecurityDeposit, Api_SecurityDepositReturn } from 'models/api/SecurityDeposit';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { fillInput, renderAsync, RenderOptions, waitFor } from 'utils/test-utils';

import { FormLeaseDepositReturn } from '../../models/FormLeaseDepositReturn';
import ReturnedDepositModal, { IReturnedDepositModalProps } from './ReturnedDepositModal';

const mockDeposit: Api_SecurityDeposit = {
  id: 7,
  description: 'Test deposit 1',
  amountPaid: 1234.0,
  depositDate: '2022-02-09',
  depositType: {
    id: 'PET',
    description: 'Pet deposit',
    isDisabled: false,
  },
  depositReturns: [],
  rowVersion: 1,
};
const mockReturnedDeposit: Api_SecurityDepositReturn = {
  parentDepositId: 7,
  returnAmount: 123,
  interestPaid: 1,
  contactHolder: {
    id: 'P6',
    person: {
      id: 6,
      isDisabled: false,
      surname: 'User',
      firstName: 'Admin',
      middleNames: '',
      personOrganizations: [],
      personAddresses: [],
      contactMethods: [],
      rowVersion: 1,
    },
  },
  rowVersion: 0,
};

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const onSave = jest.fn();
const onCancel = jest.fn();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('ReturnedDepositModal component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<IReturnedDepositModalProps> & {
        initialValues?: any;
      } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <ReturnedDepositModal
        onSave={onSave}
        onCancel={onCancel}
        display={true}
        initialValues={FormLeaseDepositReturn.createEmpty(mockDeposit)}
      />,
      {
        ...renderOptions,
        history,
        store: storeState,
      },
    );

    return {
      component,
    };
  };

  beforeEach(() => {
    mockAxios.resetHistory();
    onSave.mockReset();
    onCancel.mockReset();
  });
  it('renders as expected', async () => {
    const { component } = await setup({});

    expect(component.asFragment()).toMatchSnapshot();
  });
  it('renders with data as expected', async () => {
    const { component } = await setup({
      initialValues: FormLeaseDepositReturn.createEmpty(mockDeposit),
    });

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('submits all filled out fields as expected', async () => {
    const {
      component: { getByText },
    } = await setup({
      initialValues: FormLeaseDepositReturn.createFromModel(mockReturnedDeposit, mockDeposit),
    });

    await fillInput(document.body, 'terminationDate', '2020-01-01', 'datepicker');
    await fillInput(document.body, 'returnDate', '2020-01-02', 'datepicker');
    await fillInput(document.body, 'claimsAgainst', '1,000.00');
    await fillInput(document.body, 'returnAmount', '2,000.00');
    await fillInput(document.body, 'interestPaid', '1.00');
    await fillInput(document.body, 'contactHolder.id', 'p1');

    const saveButton = getByText('Save');
    userEvent.click(saveButton);
    await waitFor(() => expect(onSave).toHaveBeenCalled());
    expect(onSave).toHaveBeenCalledWith({
      claimsAgainst: 1000,
      depositTypeCode: 'PET',
      depositTypeDescription: 'Pet deposit',
      id: undefined,
      parentDepositAmount: 1234,
      parentDepositId: 7,
      parentDepositOtherDescription: '',
      returnAmount: 2000,
      interestPaid: 1,
      returnDate: '2020-01-02',
      rowVersion: 0,
      terminationDate: '2020-01-01',
      contactHolder: { id: 'p1' },
    });
  });

  it('calls onCancel when cancel button clicked', async () => {
    const {
      component: { getByText },
    } = await setup({});
    const cancelButton = getByText('Cancel');
    userEvent.click(cancelButton);
    expect(onCancel).toHaveBeenCalled();
  });
});
