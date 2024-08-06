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
} from '@/utils/test-utils';

import { FormLeasePeriod, defaultFormLeasePeriod } from '../../models';
import PeriodForm, { IPeriodFormProps } from './PeriodForm';
import { createRef } from 'react';
import { ISystemConstant } from '@/store/slices/systemConstants';

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
        gstConstant={{ name: 'gstDecimal', value: '0.05' }}
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

  it.skip('renders as expected', async () => {
    const { asFragment } = setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it.skip('renders with data as expected', async () => {
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
