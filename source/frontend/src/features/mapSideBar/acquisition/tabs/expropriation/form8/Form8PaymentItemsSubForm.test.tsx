import { Formik } from 'formik';
import { noop } from 'lodash';

import Claims from '@/constants/claims';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { systemConstantsSlice } from '@/store/slices/systemConstants';
import {
  act,
  fillInput,
  fireEvent,
  renderAsync,
  RenderOptions,
  userEvent,
} from '@/utils/test-utils';

import Form8PaymentItemsSubForm, {
  IForm8PaymentItemsSubFormProps,
} from './Form8PaymentItemsSubForm';
import { Form8FormModel } from './models/Form8FormModel';
import { Form8FormModelYupSchema } from './models/Form8FormYupSchema';

const defaultForm8 = new Form8FormModel(null, 1);

describe('FinancialActivitiesSubForm  component', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IForm8PaymentItemsSubFormProps> },
  ) => {
    // render component under test
    const utils = await renderAsync(
      <Formik
        onSubmit={noop}
        initialValues={defaultForm8}
        validationSchema={Form8FormModelYupSchema}
      >
        {formikProps => (
          <Form8PaymentItemsSubForm
            form8Id={renderOptions?.props?.form8Id ?? null}
            formikProps={formikProps}
            gstConstantPercentage={renderOptions?.props?.gstConstantPercentage ?? 0.05}
          />
        )}
      </Formik>,
      {
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
          [systemConstantsSlice.name]: { systemConstants: [{ name: 'GST', value: '5.0' }] },
        },
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [Claims.ACQUISITION_EDIT],
        ...renderOptions,
      },
    );

    return {
      ...utils,
      // Finding elements
      getItemTypeSelect: (index = 0) =>
        utils.container.querySelector(
          `select[name="paymentItems[${index}].paymentItemTypeCode"]`,
        ) as HTMLInputElement,
      getGstRequiredSelect: (index = 0) =>
        utils.container.querySelector(
          `select[name="paymentItems[${index}].isGstRequired"]`,
        ) as HTMLInputElement,
      getPreTaxAmountTextbox: (index = 0) =>
        utils.container.querySelector(
          `input[name="paymentItems[${index}].pretaxAmount"]`,
        ) as HTMLInputElement,
      getTaxAmountTextbox: (index = 0) =>
        utils.container.querySelector(
          `input[name="paymentItems[${index}].taxAmount"]`,
        ) as HTMLInputElement,
      getTotalAmountTextbox: (index = 0) =>
        utils.container.querySelector(
          `input[name="paymentItems[${index}].totalAmount"]`,
        ) as HTMLInputElement,
    };
  };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it(`renders 'Add Payment Item' link`, async () => {
    const { getByTestId } = await setup({});
    expect(getByTestId('add-payment-item')).toBeVisible();
  });

  it(`renders new Payment Item with default values`, async () => {
    const {
      getByTestId,
      getItemTypeSelect,
      getGstRequiredSelect,
      getPreTaxAmountTextbox,
      getTaxAmountTextbox,
      getTotalAmountTextbox,
    } = await setup({});
    const addRow = getByTestId('add-payment-item');
    await act(async () => {
      userEvent.click(addRow);
    });

    expect(getByTestId('paymentItems[0].delete-button')).toBeVisible();

    expect(getItemTypeSelect(0)).toHaveValue('');
    expect(getGstRequiredSelect(0)).toHaveValue('false');

    expect(getPreTaxAmountTextbox(0)).toBeVisible();
    expect(getPreTaxAmountTextbox(0)).toHaveValue('$0.00');

    expect(getTaxAmountTextbox(0)).not.toBeInTheDocument();

    expect(getTotalAmountTextbox(0)).toBeVisible();
    expect(getTotalAmountTextbox(0)).toHaveValue('$0.00');
  });

  it(`renders the REMOVE 'Payment Item' link`, async () => {
    const { getByTestId } = await setup({});
    const addRow = getByTestId('add-payment-item');
    await act(async () => {
      userEvent.click(addRow);
    });
    expect(getByTestId('paymentItems[0].delete-button')).toBeVisible();
  });

  it(`removes the payment item upon user confirmation`, async () => {
    const { getByTestId, getByText, getByTitle, queryByTestId } = await setup({});

    await act(async () => userEvent.click(getByTestId('add-payment-item')));
    await act(async () => userEvent.click(getByTestId('paymentItems[0].delete-button')));

    expect(getByText(/Do you wish to remove this payment item/i)).toBeVisible();
    await act(async () => userEvent.click(getByTitle('ok-modal')));

    expect(queryByTestId(`paymentItems[0]`)).not.toBeInTheDocument();
  });

  it(`keeps the payment item upon user cancel`, async () => {
    const { getByTestId, getByText, getByTitle, queryByTestId } = await setup({});

    await act(async () => userEvent.click(getByTestId('add-payment-item')));
    await act(async () => userEvent.click(getByTestId('paymentItems[0].delete-button')));

    expect(getByText(/Do you wish to remove this payment item/i)).toBeVisible();
    await act(async () => userEvent.click(getByTitle('cancel-modal')));

    expect(queryByTestId(`paymentItems[0]`)).toBeInTheDocument();
  });

  it(`calculates the total amount when adding pretax amount`, async () => {
    const { getByTestId, getPreTaxAmountTextbox, getTaxAmountTextbox, getTotalAmountTextbox } =
      await setup({});
    await act(async () => userEvent.click(getByTestId('add-payment-item')));

    // Pre-tax
    expect(getTotalAmountTextbox(0)).toBeVisible();
    expect(getTotalAmountTextbox(0)).toHaveValue('$0.00');

    // Tax
    expect(getTaxAmountTextbox(0)).not.toBeInTheDocument();

    await act(async () => {
      fireEvent.change(getPreTaxAmountTextbox(0), { target: { value: '$100.00' } });
    });

    // Total
    expect(getPreTaxAmountTextbox(0)).toHaveValue('$100.00');
    expect(getTotalAmountTextbox(0)).toHaveValue('$100.00');
  });

  it(`calculates the GST amount when adding pretax and tax amount`, async () => {
    const {
      container,
      getByTestId,
      getPreTaxAmountTextbox,
      getTaxAmountTextbox,
      getTotalAmountTextbox,
    } = await setup({});
    await act(async () => userEvent.click(getByTestId('add-payment-item')));

    // Pre-tax
    expect(getTotalAmountTextbox(0)).toBeVisible();
    expect(getTotalAmountTextbox(0)).toHaveValue('$0.00');

    // Tax
    expect(getTaxAmountTextbox(0)).not.toBeInTheDocument();

    await act(async () => {
      fireEvent.change(getPreTaxAmountTextbox(0), { target: { value: '$100.00' } });
    });

    await act(() => fillInput(container, 'paymentItems[0].isGstRequired', 'true', 'select'));

    expect(getTaxAmountTextbox(0)).toBeInTheDocument();
    expect(getPreTaxAmountTextbox(0)).toHaveValue('$100.00');
    expect(getTaxAmountTextbox(0)).toHaveValue('$5.00');
    expect(getTotalAmountTextbox(0)).toHaveValue('$105.00');
  });

  it(`re-calculates the TOTAL amount when removing GST`, async () => {
    const {
      container,
      getByTestId,
      getPreTaxAmountTextbox,
      getTaxAmountTextbox,
      getTotalAmountTextbox,
    } = await setup({});
    await act(async () => userEvent.click(getByTestId('add-payment-item')));

    // Pre-tax
    expect(getTotalAmountTextbox(0)).toBeVisible();
    expect(getTotalAmountTextbox(0)).toHaveValue('$0.00');

    // Tax
    expect(getTaxAmountTextbox(0)).not.toBeInTheDocument();

    await act(async () => {
      fireEvent.change(getPreTaxAmountTextbox(0), { target: { value: '$100.00' } });
    });

    await act(() => fillInput(container, 'paymentItems[0].isGstRequired', 'true', 'select'));

    expect(getTaxAmountTextbox(0)).toBeInTheDocument();
    expect(getPreTaxAmountTextbox(0)).toHaveValue('$100.00');
    expect(getTaxAmountTextbox(0)).toHaveValue('$5.00');
    expect(getTotalAmountTextbox(0)).toHaveValue('$105.00');

    await act(() => fillInput(container, 'paymentItems[0].isGstRequired', 'false', 'select'));

    expect(getTaxAmountTextbox(0)).not.toBeInTheDocument();
    expect(getPreTaxAmountTextbox(0)).toHaveValue('$100.00');
    expect(getTotalAmountTextbox(0)).toHaveValue('$100.00');
  });

  it(`Overrides the GST amount if needed`, async () => {
    const {
      container,
      getByTestId,
      getPreTaxAmountTextbox,
      getTaxAmountTextbox,
      getTotalAmountTextbox,
    } = await setup({});
    await act(async () => userEvent.click(getByTestId('add-payment-item')));

    // Pre-tax
    expect(getTotalAmountTextbox(0)).toBeVisible();
    expect(getTotalAmountTextbox(0)).toHaveValue('$0.00');

    // Tax
    expect(getTaxAmountTextbox(0)).not.toBeInTheDocument();

    await act(async () => {
      fireEvent.change(getPreTaxAmountTextbox(0), { target: { value: '$100.00' } });
    });

    await act(() => fillInput(container, 'paymentItems[0].isGstRequired', 'true', 'select'));

    expect(getTaxAmountTextbox(0)).toBeInTheDocument();
    expect(getPreTaxAmountTextbox(0)).toHaveValue('$100.00');
    expect(getTaxAmountTextbox(0)).toHaveValue('$5.00');
    expect(getTotalAmountTextbox(0)).toHaveValue('$105.00');

    await act(async () => {
      fireEvent.change(getTaxAmountTextbox(0), { target: { value: '$10.00' } });
    });

    expect(getPreTaxAmountTextbox(0)).toHaveValue('$100.00');
    expect(getTaxAmountTextbox(0)).toHaveValue('$10.00');
    expect(getTotalAmountTextbox(0)).toHaveValue('$110.00');
  });

  it(`can have more than one payment item`, async () => {
    const {
      getByTestId,
      getItemTypeSelect,
      getGstRequiredSelect,
      getPreTaxAmountTextbox,
      getTaxAmountTextbox,
      getTotalAmountTextbox,
    } = await setup({});

    await act(async () => userEvent.click(getByTestId('add-payment-item')));
    await act(async () => userEvent.click(getByTestId('add-payment-item')));

    expect(getByTestId('paymentItems[0].delete-button')).toBeVisible();

    expect(getItemTypeSelect(1)).toHaveValue('');
    expect(getGstRequiredSelect(1)).toHaveValue('false');

    expect(getPreTaxAmountTextbox(1)).toBeVisible();
    expect(getPreTaxAmountTextbox(1)).toHaveValue('$0.00');

    expect(getTaxAmountTextbox(1)).not.toBeInTheDocument();

    expect(getTotalAmountTextbox(1)).toBeVisible();
    expect(getTotalAmountTextbox(1)).toHaveValue('$0.00');
  });
});
