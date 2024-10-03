import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';

import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  fillInput,
  render,
  RenderOptions,
  act,
  userEvent,
  selectOptions,
  getByDisplayValue,
} from '@/utils/test-utils';

import { FormLeasePeriod, defaultFormLeasePeriod } from '../../models';
import PeriodForm, { IPeriodFormProps } from './PeriodForm';
import { createRef } from 'react';

const history = createMemoryHistory();
const onSave = vi.fn();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('PeriodForm component', () => {
  const setup = (renderOptions: RenderOptions & Partial<IPeriodFormProps> = {}) => {
    const formikRef = createRef<FormikProps<FormLeasePeriod>>();
    // render component under test
    const utils = render(
      <PeriodForm
        onSave={onSave}
        formikRef={formikRef}
        initialValues={renderOptions?.initialValues ?? { ...defaultFormLeasePeriod }}
        lease={{} as any}
        gstConstant={{ name: 'gstDecimal', value: '5' }}
      />,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return {
      ...utils,
      getFormikRef: () => formikRef,
    };
  };

  it('renders as expected', async () => {
    const { asFragment } = setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders with data as expected', async () => {
    const { asFragment } = setup({ initialValues: { ...defaultFormLeasePeriod } });
    expect(asFragment()).toMatchSnapshot();
  });

  it('does not display variable period options if fixed period selected', async () => {
    const { queryByText, getByTestId } = setup({});

    const variableButton = getByTestId('radio-isvariable-predetermined');
    await act(async () => {
      userEvent.click(variableButton);
    });

    expect(queryByText('Add Base Rent')).toBeNull();
    expect(queryByText('Add Additional Rent')).toBeNull();
    expect(queryByText('Add Variable Rent')).toBeNull();
  });

  it('displays variable period options if variable period selected', async () => {
    const { getByText, getByTestId } = setup({});

    const variableButton = getByTestId('radio-isvariable-variable');
    await act(async () => {
      userEvent.click(variableButton);
    });

    expect(getByText('Add Base Rent')).toBeVisible();
    expect(getByText('Add Additional Rent')).toBeVisible();
    expect(getByText('Add Variable Rent')).toBeVisible();
  });

  it('displays variable period gst when manual gst does not equal calculated gst.', async () => {
    const { getByDisplayValue, getByTestId } = setup({
      initialValues: {
        ...defaultFormLeasePeriod,
        isGstEligible: true,
        paymentAmount: 1,
        variableRentPaymentAmount: 1,
        additionalRentPaymentAmount: 1,
        isVariableRentGstEligible: true,
        isAdditionalRentGstEligible: true,
        variableRentGstAmount: 100,
        gstAmount: 200,
        additionalRentGstAmount: 300,
      },
    });

    const variableButton = getByTestId('radio-isvariable-variable');
    await act(async () => {
      userEvent.click(variableButton);
    });

    expect(getByDisplayValue('$100.00')).toBeVisible();
    expect(getByDisplayValue('$200.00')).toBeVisible();
    expect(getByDisplayValue('$300.00')).toBeVisible();
  });

  it('Does not allow variability to be modified when editing', async () => {
    const { getByTestId } = setup({
      initialValues: { ...defaultFormLeasePeriod, id: 1 },
    });

    const variableButton = getByTestId('radio-isvariable-variable');
    await act(async () => {
      userEvent.click(variableButton);
    });

    expect(variableButton).toBeDisabled();
  });

  it('displays expected fields when period duration of fixed is selected', async () => {
    const { getByText } = setup({});

    await act(async () => {
      selectOptions('isFlexible', 'false');
    });

    expect(getByText('Start date:')).toBeVisible();
    expect(getByText('End date:')).toBeVisible();
  });

  it('displays expected fields when period duration of fixed is selected', async () => {
    const { getByText } = setup({});

    await act(async () => {
      selectOptions('isFlexible', 'true');
    });

    expect(getByText('Start date:')).toBeVisible();
    expect(getByText('End date (Anticipated):')).toBeVisible();
  });

  it('displays expected fields when period duration of flexible is selected', async () => {
    const { getByText, getByTestId } = setup({});

    const variableButton = getByTestId('radio-isvariable-variable');
    await act(async () => {
      userEvent.click(variableButton);
    });

    expect(getByText('Add Base Rent')).toBeVisible();
    expect(getByText('Add Additional Rent')).toBeVisible();
    expect(getByText('Add Variable Rent')).toBeVisible();
  });

  it('validates that the end date must be after the start date', async () => {
    const { container, findByText } = setup({});

    await act(async () => {
      fillInput(container, 'startDate', '2020-01-02', 'datepicker');
    });
    await act(async () => {
      fillInput(container, 'expiryDate', '2020-01-01', 'datepicker');
    });

    const error = await findByText('Expiry Date must be after Start Date');
    expect(error).toBeVisible();
  });

  it('validates that the start date is required', async () => {
    const { container, findByDisplayValue } = setup({});

    await act(async () => {
      fillInput(container, 'startDate', '2020-01-02', 'datepicker');
    });

    const input = await findByDisplayValue('Jan 02, 2020');
    expect(input).toHaveAttribute('required');
  });

  it(`validates that the default period status is 'Not Exercised'`, async () => {
    const { findByDisplayValue } = setup({});

    const periodStatus = await findByDisplayValue('Not Exercised');
    expect(periodStatus).toBeVisible();
  });

  it('automatically populates the gst amount field when payment amount entered', async () => {
    const { container, findByDisplayValue } = setup({
      initialValues: { ...defaultFormLeasePeriod, isGstEligible: true },
    });

    await act(async () => {
      await fillInput(container, 'paymentAmount', '1000');
    });
    const gstAmount = await findByDisplayValue('$50.00');
    expect(gstAmount).toBeVisible();
  });

  it('automatically populates the gst amount field when payment amount entered for variable terms', async () => {
    const { container, findByDisplayValue } = setup({
      initialValues: { ...defaultFormLeasePeriod, isGstEligible: true },
    });

    await act(async () => {
      await fillInput(container, 'paymentAmount', '1000');
      await fillInput(container, 'isVariable', 'true', 'select');
    });
    const gstAmount = await findByDisplayValue('$50.00');
    expect(gstAmount).toBeVisible();
  });

  it('automatically populates the gst amount field when additional amount entered for variable terms', async () => {
    const { container, findByDisplayValue } = setup({
      initialValues: {
        ...defaultFormLeasePeriod,
        isAdditionalRentGstEligible: true,
        isVariable: 'true',
      },
    });

    await act(async () => {
      await fillInput(container, 'additionalRentPaymentAmount', '2000');
      await fillInput(container, 'isAdditionalRentGstEligible', 'true', 'select');
    });
    const gstAmount = await findByDisplayValue('$100.00');
    expect(gstAmount).toBeVisible();
  });

  it('automatically populates the gst amount field when variable amount entered for variable terms', async () => {
    const { container, findByDisplayValue } = setup({
      initialValues: {
        ...defaultFormLeasePeriod,
        isVariableRentGstEligible: true,
        isVariable: 'true',
      },
    });

    await act(async () => {
      await fillInput(container, 'variableRentPaymentAmount', '3000');
      await fillInput(container, 'isVariableRentGstEligible', 'true', 'select');
    });
    const gstAmount = await findByDisplayValue('$150.00');
    expect(gstAmount).toBeVisible();
  });

  it('calls onSave when form is submitted', async () => {
    const { container, getFormikRef } = setup({});
    const formikRef = getFormikRef();

    await act(async () => {
      fillInput(container, 'startDate', '2020-01-02', 'datepicker');
      fillInput(container, 'expiryDate', '2020-01-02', 'datepicker');
    });
    await act(() => formikRef.current.submitForm());

    expect(onSave).toHaveBeenCalled();
  });
});
