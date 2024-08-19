import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { systemConstantsSlice } from '@/store/slices/systemConstants';
import { act, fillInput, findByText, renderAsync, RenderOptions } from '@/utils/test-utils';

import {
  defaultFormLeasePayment,
  defaultFormLeasePeriod,
  FormLeasePayment,
  FormLeasePeriod,
} from '../../models';
import { isActualGstEligible as isActualGstEligibleOriginal } from '../../PeriodPaymentsContainer';
import PaymentForm, { IPaymentFormProps } from './PaymentForm';
import { ApiGen_Concepts_LeasePeriod } from '@/models/api/generated/ApiGen_Concepts_LeasePeriod';

const isActualGstEligible = vi.mocked(isActualGstEligibleOriginal);
const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const onSave = vi.fn();
const submitForm = vi.fn();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
  [systemConstantsSlice.name]: { systemConstants: [{ name: 'GST', value: '5.0' }] },
};
vi.mock('../../PeriodPaymentsContainer', () => ({
  isActualGstEligible: vi.fn(),
}));

describe('PaymentForm component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<IPaymentFormProps> & {
        initialValues?: FormLeasePayment;
        periods?: ApiGen_Concepts_LeasePeriod[];
      } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <Formik initialValues={renderOptions.initialValues ?? {}} onSubmit={noop}>
        <PaymentForm
          periods={renderOptions.periods ?? []}
          initialValues={renderOptions.initialValues}
          onSave={onSave}
          formikRef={{ current: { submitForm } } as any}
        />
      </Formik>,
      {
        ...renderOptions,
        store: renderOptions.store ?? storeState,
        history,
      },
    );

    return {
      component,
    };
  };

  beforeEach(() => {
    mockAxios.resetHistory();
    isActualGstEligible.mockReset();
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

  it('Non-variable payments do not display payment category', async () => {
    isActualGstEligible.mockReturnValue(true);
    const {
      component: { container, queryByLabelText },
    } = await setup({
      initialValues: { ...defaultFormLeasePayment, leasePeriodId: 1 },
      periods: [{ ...FormLeasePeriod.toApi(defaultFormLeasePeriod), isVariable: false, id: 1 }],
    });

    const category = queryByLabelText('Payment category:');
    expect(category).toBeNull();
  });

  it('Variable payments do not display payment category', async () => {
    isActualGstEligible.mockReturnValue(true);
    const {
      component: { container, queryByLabelText },
    } = await setup({
      initialValues: { ...defaultFormLeasePayment, leasePeriodId: 1 },
      periods: [{ ...FormLeasePeriod.toApi(defaultFormLeasePeriod), isVariable: true, id: 1 }],
    });

    const category = queryByLabelText('Payment category:');
    expect(category).toBeVisible();
  });

  it('Entering a total amount results in a calculated gst amount and pre tax amount', async () => {
    isActualGstEligible.mockReturnValue(true);
    const {
      component: { container, findByLabelText },
    } = await setup({ initialValues: { ...defaultFormLeasePayment, leasePeriodId: 1 } });

    await act(async () => {
      await fillInput(container, 'amountTotal', '1050');
    });
    const amountPreTax = await findByLabelText('Amount (before tax)');
    const amountGst = await findByLabelText('GST ($)');
    expect(amountPreTax).toHaveValue('$1,000.00');
    expect(amountGst).toHaveValue('$50.00');
  });

  it('Entering a total amount results and a mismatched gst amount results in an error', async () => {
    isActualGstEligible.mockReturnValue(true);
    const {
      component: { container, findByText, findByDisplayValue },
    } = await setup({ initialValues: { ...defaultFormLeasePayment, leasePeriodId: 1 } });

    await act(async () => {
      await fillInput(container, 'amountGst', '50');
    });
    await findByDisplayValue('$50.00');
    const warningMessage = await findByText(
      'Expected payment amount and GST amount must sum to the total received',
    );
    expect(warningMessage).toBeVisible();
  });

  it('Entering a total amount results in a calculated pre tax amount when gst is not enabled', async () => {
    isActualGstEligible.mockReturnValue(false);
    const {
      component: { container, findByLabelText },
    } = await setup({ initialValues: { ...defaultFormLeasePayment, leasePeriodId: 1 } });

    await act(async () => {
      await fillInput(container, 'amountTotal', '1000');
    });
    const amountPreTax = await findByLabelText('Amount (before tax)');
    expect(amountPreTax).toHaveValue('$1,000.00');
  });

  it('Entering a total amount results in a calculated pre tax amount when gst constant is not available', async () => {
    isActualGstEligible.mockReturnValue(true);
    const {
      component: { container, findByLabelText },
    } = await setup({
      store: {
        ...storeState,
        systemConstant: {},
        initialValues: { ...defaultFormLeasePayment, leasePeriodId: 1 },
      },
    });

    await act(async () => {
      await fillInput(container, 'amountTotal', '1000');
    });
    const amountPreTax = await findByLabelText('Amount (before tax)');
    expect(amountPreTax).toHaveValue('$1,000.00');
  });
});
