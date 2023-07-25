import userEvent from '@testing-library/user-event';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';

import { getMockDepositReturns, getMockDeposits } from '@/mocks/deposits.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { fillInput, renderAsync, RenderOptions, waitFor } from '@/utils/test-utils';

import { FormLeaseDepositReturn } from '../../models/FormLeaseDepositReturn';
import ReturnedDepositModal, { IReturnedDepositModalProps } from './ReturnedDepositModal';

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
        initialValues={FormLeaseDepositReturn.createEmpty(getMockDeposits()[0])}
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
      initialValues: FormLeaseDepositReturn.createEmpty(getMockDeposits()[0]),
    });

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('submits all filled out fields as expected', async () => {
    const {
      component: { getByText },
    } = await setup({
      initialValues: FormLeaseDepositReturn.fromApi(
        getMockDepositReturns()[0],
        getMockDeposits()[0],
      ),
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
      depositTypeDescription: '',
      id: undefined,
      parentDepositAmount: 500,
      parentDepositId: 1,
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
