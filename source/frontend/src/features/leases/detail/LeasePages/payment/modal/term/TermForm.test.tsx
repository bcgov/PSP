import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';

import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { fillInput, render, RenderOptions, act } from '@/utils/test-utils';

import { FormLeaseTerm, defaultFormLeaseTerm } from '../../models';
import TermForm, { ITermFormProps } from './TermForm';
import { createRef } from 'react';

const history = createMemoryHistory();
const onSave = vi.fn();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('TermForm component', () => {
  const setup = (renderOptions: RenderOptions & Partial<ITermFormProps> = {}) => {
    const formikRef = createRef<FormikProps<FormLeaseTerm>>();
    // render component under test
    const utils = render(
      <TermForm
        onSave={onSave}
        formikRef={formikRef}
        initialValues={renderOptions?.initialValues ?? { ...defaultFormLeaseTerm }}
        lease={{} as any}
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
    const { asFragment } = setup({ initialValues: { ...defaultFormLeaseTerm } });
    expect(asFragment()).toMatchSnapshot();
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

  it(`validates that the default term status is 'Not Exercised'`, async () => {
    const { findByDisplayValue } = setup({});

    const termStatus = await findByDisplayValue('Not Exercised');
    expect(termStatus).toBeVisible();
  });

  it('calls onSave when form is submitted', async () => {
    const { container, getFormikRef } = setup({});
    const formikRef = getFormikRef();

    await act(async () => {
      fillInput(container, 'startDate', '2020-01-02', 'datepicker');
    });
    await act(() => formikRef.current.submitForm());

    expect(onSave).toHaveBeenCalled();
  });
});
