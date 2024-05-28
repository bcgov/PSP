import userEvent from '@testing-library/user-event';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';

import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, renderAsync, RenderOptions, waitFor } from '@/utils/test-utils';

import { defaultFormLeasePayment } from '../../models';
import { IPaymentModalProps, PaymentModal } from './PaymentModal';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const onSave = vi.fn();
const onCancel = vi.fn();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('PaymentModal component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<IPaymentModalProps> & {
        initialValues?: any;
      } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <PaymentModal onSave={onSave} onCancel={onCancel} displayModal={true} terms={[]} />,
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
      initialValues: { ...defaultFormLeasePayment },
    });

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('submits all filled out fields as expected', async () => {
    const {
      component: { getByText },
    } = await setup({});

    await fillInput(document.body, 'receivedDate', '2020-01-01', 'datepicker');
    await fillInput(document.body, 'leasePaymentMethodType.id', 'CHEQ', 'select');
    await fillInput(document.body, 'amountPreTax', '1150');
    await fillInput(document.body, 'amountGst', '50');
    await fillInput(document.body, 'amountTotal', '1200');
    const saveButton = getByText('Save payment');
    await act(async () => userEvent.click(saveButton));
    await waitFor(() => expect(onSave).toHaveBeenCalled());
    expect(onSave).toHaveBeenCalledWith({
      ...defaultFormLeasePayment,
      receivedDate: '2020-01-01',
      amountTotal: 1200,
      amountPreTax: 1200,
      amountGst: '',
      leasePaymentMethodType: {
        id: 'CHEQ',
        displayOrder: null,
        description: null,
        isDisabled: false,
      },
    });
  });

  it('calls onCancel when cancel button clicked', async () => {
    const {
      component: { getByText },
    } = await setup({});
    const cancelButton = getByText('Cancel');
    await act(async () => userEvent.click(cancelButton));
    expect(onCancel).toHaveBeenCalled();
  });
});
