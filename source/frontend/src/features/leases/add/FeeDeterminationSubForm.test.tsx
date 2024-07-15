import { act, getByText, render, RenderOptions } from '@testing-library/react';
import { getDefaultFormLease, LeaseFormModel } from '../models';
import { AddLeaseYupSchema } from './AddLeaseYupSchema';
import { createMemoryHistory } from 'history';
import FeeDeterminationSubForm, { IFeeDeterminationSubFormProps } from './FeeDeterminationSubForm';
import { Formik, FormikProps } from 'formik';
import React from 'react';
import { noop } from 'lodash';
import { fillInput, renderAsync } from '@/utils/test-utils';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { mockLookups } from '@/mocks/lookups.mock';
import { Simulate } from 'react-dom/test-utils';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('LeaseFeeDeterminationSubForm component', () => {
  const setup = async (
    renderOptions: RenderOptions & Partial<IFeeDeterminationSubFormProps> = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <Formik onSubmit={noop} initialValues={getDefaultFormLease()}>
        {formikProps => <FeeDeterminationSubForm formikProps={formikProps} />}
      </Formik>,
      {
        ...renderOptions,
        claims: [],
        store: storeState,
        history,
      },
    );

    return {
      component,
    };
  };
  it('renders as expected', async () => {
    const { component } = await setup({});
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('displays expected Nominal fee', async () => {
    const {
      component: { container },
    } = await setup({});

    let suggestedFeeField = await container.querySelector("span[data-testid='suggestedFee']");

    expect(suggestedFeeField).toHaveTextContent('Unknown');

    await act(async () => {
      await fillInput(container, 'isPublicBenefit', 'true', 'select');
      await fillInput(container, 'isFinancialGain', 'false', 'select');
    });

    expect(suggestedFeeField).toHaveTextContent('$1 - Nominal');
  });

  it('displays expected LAF fee', async () => {
    const {
      component: { container },
    } = await setup({});

    let suggestedFeeField = await container.querySelector("span[data-testid='suggestedFee']");

    expect(suggestedFeeField).toHaveTextContent('Unknown');

    await act(async () => {
      await fillInput(container, 'isPublicBenefit', 'true', 'select');
      await fillInput(container, 'isFinancialGain', 'true', 'select');
    });

    expect(suggestedFeeField).toHaveTextContent('Licence Administration Fee (LAF) *');
  });

  it('displays expected FMV fee', async () => {
    const {
      component: { container },
    } = await setup({});

    let suggestedFeeField = await container.querySelector("span[data-testid='suggestedFee']");

    expect(suggestedFeeField).toHaveTextContent('Unknown');

    await act(async () => {
      await fillInput(container, 'isPublicBenefit', 'false', 'select');
      await fillInput(container, 'isFinancialGain', 'true', 'select');
    });

    expect(suggestedFeeField).toHaveTextContent(
      'Fair Market Value (FMV) - (Licence Administration Fee Minimum)',
    );
  });

  it('displays expected LAF fee', async () => {
    const {
      component: { container },
    } = await setup({});

    let suggestedFeeField = await container.querySelector("span[data-testid='suggestedFee']");

    expect(suggestedFeeField).toHaveTextContent('Unknown');

    await act(async () => {
      await fillInput(container, 'isPublicBenefit', 'true', 'select');
      await fillInput(container, 'isFinancialGain', 'true', 'select');
    });

    expect(suggestedFeeField).toHaveTextContent('Licence Administration Fee (LAF) *');
  });

  it('displays expected LAF fee', async () => {
    const {
      component: { container },
    } = await setup({});

    let suggestedFeeField = await container.querySelector("span[data-testid='suggestedFee']");

    expect(suggestedFeeField).toHaveTextContent('Unknown');

    await act(async () => {
      await fillInput(container, 'isPublicBenefit', 'true', 'select');
      await fillInput(container, 'isFinancialGain', 'true', 'select');
    });

    expect(suggestedFeeField).toHaveTextContent('Licence Administration Fee (LAF) *');
  });
});
