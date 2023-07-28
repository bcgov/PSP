import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';

import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { systemConstantsSlice } from '@/store/slices/systemConstants';
import { fillInput, renderAsync, RenderOptions } from '@/utils/test-utils';

import { defaultFormLeasePayment } from '../../models';
import { isActualGstEligible as isActualGstEligibleOriginal } from '../../TermPaymentsContainer';
import PaymentForm, { IPaymentFormProps } from './PaymentForm';

const isActualGstEligible = isActualGstEligibleOriginal as jest.Mock;
const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const onSave = jest.fn();
const submitForm = jest.fn();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
  [systemConstantsSlice.name]: { systemConstants: [{ name: 'GST', value: '5.0' }] },
};
jest.mock('../../TermPaymentsContainer', () => ({
  isActualGstEligible: jest.fn(),
}));

describe('PaymentForm component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<IPaymentFormProps> & {
        initialValues?: any;
      } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <Formik initialValues={renderOptions.initialValues ?? {}} onSubmit={noop}>
        <PaymentForm
          terms={[]}
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
  it('Entering a total amount results in a calculated gst amount and pre tax amount', async () => {
    isActualGstEligible.mockReturnValue(true);
    const {
      component: { container, findByLabelText },
    } = await setup({ initialValues: { ...defaultFormLeasePayment, leaseTermId: 1 } });

    await fillInput(container, 'amountTotal', '1050');
    const amountPreTax = await findByLabelText('Expected payment ($)');
    const amountGst = await findByLabelText('GST ($)');
    expect(amountPreTax).toHaveValue('$1,000.00');
    expect(amountGst).toHaveValue('$50.00');
  });

  it('Entering a total amount results in a calculated pre tax amount when gst is not enabled', async () => {
    isActualGstEligible.mockReturnValue(false);
    const {
      component: { container, findByLabelText },
    } = await setup({ initialValues: { ...defaultFormLeasePayment, leaseTermId: 1 } });

    await fillInput(container, 'amountTotal', '1000');
    const amountPreTax = await findByLabelText('Expected payment ($)');
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
        initialValues: { ...defaultFormLeasePayment, leaseTermId: 1 },
      },
    });

    await fillInput(container, 'amountTotal', '1000');
    const amountPreTax = await findByLabelText('Expected payment ($)');
    expect(amountPreTax).toHaveValue('$1,000.00');
  });
});
