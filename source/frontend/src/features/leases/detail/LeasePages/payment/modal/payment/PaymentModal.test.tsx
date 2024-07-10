import userEvent from '@testing-library/user-event';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';

import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  fillInput,
  renderAsync,
  RenderOptions,
  waitFor,
  screen,
  getByName,
} from '@/utils/test-utils';

import { FormLeasePeriod, defaultFormLeasePayment, defaultFormLeasePeriod } from '../../models';
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
      <PaymentModal
        onSave={onSave}
        onCancel={onCancel}
        displayModal={true}
        periods={renderOptions.periods ?? []}
        initialValues={renderOptions.initialValues ?? {}}
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
      initialValues: { ...defaultFormLeasePayment },
    });

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('submits all filled out fields as expected', async () => {
    const { component } = await setup({
      initialValues: { ...defaultFormLeasePayment, leasePeriodId: 1 },
      periods: [{ ...FormLeasePeriod.toApi(defaultFormLeasePeriod), id: 1, isVariable: true }],
    });
    const { getByText } = component;

    await fillInput(document.body, 'receivedDate', '2020-01-01', 'datepicker');
    await act(async () =>
      userEvent.selectOptions(
        getByName('leasePaymentMethodType.id'),
        screen.getByRole('option', { name: 'Cheque' }),
      ),
    );
    await act(async () =>
      userEvent.selectOptions(
        getByName('leasePaymentCategoryTypeCode.id'),
        screen.getByRole('option', { name: 'Base Rent' }),
      ),
    );
    await fillInput(document.body, 'amountTotal', '1200');
    const saveButton = getByText('Save payment');
    await act(async () => userEvent.click(saveButton));
    await waitFor(() => expect(onSave).toHaveBeenCalled());
    expect(onSave).toHaveBeenCalledWith({
      receivedDate: '2020-01-01',
      leasePeriodId: 1,
      id: 0,
      note: '',
      amountPst: '',
      amountTotal: 1200,
      amountPreTax: 1200,
      amountGst: '',
      leasePaymentCategoryTypeCode: {
        id: 'BASE',
        description: '',
        displayOrder: null,
        isDisabled: false,
      },
      leasePaymentMethodType: {
        id: 'CHEQ',
        isDisabled: false,
        description: '',
        displayOrder: null,
      },
      leasePaymentStatusTypeCode: {
        description: '',
        displayOrder: null,
        id: '',
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
