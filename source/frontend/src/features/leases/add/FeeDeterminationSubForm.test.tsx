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
import { SuggestedFeeCode } from '../leaseUtils';

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
      component: { container, getByText },
    } = await setup({});

    let suggestedFeeField = await container.querySelector("span[data-testid='suggestedFee']");

    expect(suggestedFeeField).toHaveTextContent('Unknown');

    await act(async () => {
      await fillInput(container, 'isPublicBenefit', 'true', 'select');
      await fillInput(container, 'isFinancialGain', 'false', 'select');
    });

    expect(suggestedFeeField).toHaveTextContent('$1 - Nominal');
    expect(
      getByText('No or nominal fee determinations should include justification in the', {
        exact: false,
      }),
    ).toBeVisible();
  });

  it('displays expected LAF fee', async () => {
    const {
      component: { container, getByText },
    } = await setup({});

    let suggestedFeeField = await container.querySelector("span[data-testid='suggestedFee']");

    expect(suggestedFeeField).toHaveTextContent('Unknown');

    await act(async () => {
      await fillInput(container, 'isPublicBenefit', 'true', 'select');
      await fillInput(container, 'isFinancialGain', 'true', 'select');
    });

    expect(suggestedFeeField).toHaveTextContent('Licence Administration Fee (LAF) *');
    expect(
      getByText('License administration fees are charged when there is either: a financial', {
        exact: false,
      }),
    ).toBeVisible();
  });

  it('displays expected FMV fee', async () => {
    const {
      component: { container, getByText },
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
    expect(
      getByText('Fair market value fee determination should include the square footage rate', {
        exact: false,
      }),
    ).toBeVisible();
  });

  it('displays expected any fee', async () => {
    const {
      component: { container, getByText },
    } = await setup({});

    let suggestedFeeField = await container.querySelector("span[data-testid='suggestedFee']");

    expect(suggestedFeeField).toHaveTextContent('Unknown');

    await act(async () => {
      await fillInput(container, 'isPublicBenefit', 'false', 'select');
      await fillInput(container, 'isFinancialGain', 'false', 'select');
    });

    expect(suggestedFeeField).toHaveTextContent(SuggestedFeeCode.ANY);
    expect(
      getByText('No or nominal fee determinations should include justification', { exact: false }),
    ).toBeVisible();
    expect(
      getByText('Fair market value fee determination should include the square footage rate', {
        exact: false,
      }),
    ).toBeVisible();
    expect(
      getByText('License administration fees are charged when there is either: a financial', {
        exact: false,
      }),
    ).toBeVisible();
  });
});
