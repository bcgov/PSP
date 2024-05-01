import userEvent from '@testing-library/user-event';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';

import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, renderAsync, RenderOptions, waitFor } from '@/utils/test-utils';

import { FormLeaseDeposit } from '../../models/FormLeaseDeposit';
import ReceivedDepositModal, { IReceivedDepositModalProps } from './ReceivedDepositModal';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const onSave = vi.fn();
const onCancel = vi.fn();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('ReceivedDepositModal component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<IReceivedDepositModalProps> & {
        initialValues?: any;
      } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <ReceivedDepositModal
        onSave={onSave}
        onCancel={onCancel}
        display={true}
        initialValues={FormLeaseDeposit.createEmpty(1)}
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
      initialValues: FormLeaseDeposit.createEmpty(1),
    });

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('submits all filled out fields as expected', async () => {
    const {
      component: { getByText },
    } = await setup({});

    await fillInput(document.body, 'depositTypeCode', 'SECURITY', 'select');
    await fillInput(document.body, 'description', 'Test description', 'textarea');
    await fillInput(document.body, 'amountPaid', '1235');
    await fillInput(document.body, 'depositDate', '2020-01-02', 'datepicker');
    await fillInput(document.body, 'contactHolder.id', 'p1');
    const saveButton = getByText('Yes');
    await act(async () => userEvent.click(saveButton));
    await waitFor(() => expect(onSave).toHaveBeenCalled());
    expect(onSave).toHaveBeenCalledWith({
      amountPaid: 1235,
      depositDate: '2020-01-02',
      depositTypeCode: 'SECURITY',
      description: 'Test description',
      id: undefined,
      leaseId: 1,
      otherTypeDescription: '',
      contactHolder: { id: 'p1' },
      rowVersion: 0,
    });
  });

  it('calls onCancel when cancel button clicked', async () => {
    const {
      component: { getByText },
    } = await setup({});
    const cancelButton = getByText('No');
    await act(async () => userEvent.click(cancelButton));
    expect(onCancel).toHaveBeenCalled();
  });
});
