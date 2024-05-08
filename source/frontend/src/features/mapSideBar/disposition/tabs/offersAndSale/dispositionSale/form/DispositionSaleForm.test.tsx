import { Formik, FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import Claims from '@/constants/claims';
import { DispositionSaleFormModel } from '@/features/mapSideBar/disposition/models/DispositionSaleFormModel';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes/lookupCodesSlice';
import { systemConstantsSlice } from '@/store/slices/systemConstants';
import {
  act,
  fillInput,
  fireEvent,
  renderAsync,
  RenderOptions,
  userEvent,
  waitFor,
  waitForEffects,
} from '@/utils/test-utils';

import DispositionSaleForm, { IDispositionSaleFormProps } from './DispositionSaleForm';
import { DispositionSaleFormYupSchema } from './DispositionSaleFormYupSchema';
import { createRef } from 'react';

const history = createMemoryHistory();

const defaultInitialValues = new DispositionSaleFormModel(null, 1, 0);

describe('DispositionSaleForm  component', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IDispositionSaleFormProps> },
  ) => {
    // render component under
    const ref = createRef<FormikProps<DispositionSaleFormModel>>();
    const utils = await renderAsync(
      <Formik<DispositionSaleFormModel>
        enableReinitialize
        onSubmit={noop}
        initialValues={defaultInitialValues}
        validationSchema={DispositionSaleFormYupSchema}
        innerRef={ref}
      >
        {formikProps => <DispositionSaleForm dispositionSaleId={null} />}
      </Formik>,
      {
        ...renderOptions,
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [Claims.DISPOSITION_EDIT],
        history: history,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
          [systemConstantsSlice.name]: { systemConstants: [{ name: 'GST', value: '5.0' }] },
        },
      },
    );

    return {
      ...utils,
      // Finding elements
      getFinalSaleAmountTextbox: () =>
        utils.container.querySelector(`input[name="finalSaleAmount"]`) as HTMLInputElement,
      getGSTCollectedAmountTextbox: () =>
        utils.container.querySelector(`input[name="gstCollectedAmount"]`) as HTMLInputElement,
      getRealtorCommissionAmountTextbox: () =>
        utils.container.querySelector(`input[name="realtorCommissionAmount"]`) as HTMLInputElement,
      getTotalCostSaleAmountTextbox: () =>
        utils.container.querySelector(`input[name="totalCostAmount"]`) as HTMLInputElement,
      getNetBookAmountTextbox: () =>
        utils.container.querySelector(`input[name="netBookAmount"]`) as HTMLInputElement,
      getNetProceedsBeforeSPPAmountTextbox: () =>
        utils.container.querySelector(
          `input[name="netProceedsBeforeSppAmount"]`,
        ) as HTMLInputElement,
      getSPPAmountTextbox: () =>
        utils.container.querySelector(`input[name="sppAmount"]`) as HTMLInputElement,
      getNetProceedsAfterSPPAmountTextbox: () =>
        utils.container.querySelector(
          `input[name="netProceedsAfterSppAmount"]`,
        ) as HTMLInputElement,
    };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup({
      props: {
        dispositionSaleId: null,
      },
    });

    await act(async () => {});
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it(`renders 'Add Purchaser' link`, async () => {
    const { getByTestId } = await setup({});
    await act(async () => {});
    expect(getByTestId('add-purchaser-button')).toBeVisible();
  });

  it(`renders 'Remove team member' link`, async () => {
    const { getByTestId } = await setup({ props: { dispositionSaleId: null } });
    const addRow = getByTestId('add-purchaser-button');

    await act(async () => userEvent.click(addRow));
    expect(getByTestId('dispositionPurchasers.0.remove-button')).toBeVisible();
  });

  it(`displays a confirmation popup before purchaser is removed`, async () => {
    const { getByTestId, getByText } = await setup({
      props: { dispositionSaleId: null },
    });
    const addRow = getByTestId('add-purchaser-button');

    await act(async () => userEvent.click(addRow));
    await act(async () => userEvent.click(getByTestId('dispositionPurchasers.0.remove-button')));

    expect(getByText(/Do you wish to remove this purchaser/i)).toBeVisible();
  });

  it(`removes the purchaser upon user confirmation`, async () => {
    const { getByTestId, getByText, getByTitle, queryByTestId } = await setup({
      props: { dispositionSaleId: null },
    });

    const addRow = getByTestId('add-purchaser-button');
    await act(async () => userEvent.click(addRow));
    await act(async () => userEvent.click(getByTestId('dispositionPurchasers.0.remove-button')));

    expect(getByText(/Do you wish to remove this purchaser/i)).toBeVisible();

    await act(async () => userEvent.click(getByTitle('ok-modal')));
    expect(queryByTestId('purchaserRow[0]')).toBeNull();
  });

  it(`does not remove the owner when confirmation popup is cancelled`, async () => {
    const { getByTestId, getByText, getByTitle } = await setup({
      props: { dispositionSaleId: null },
    });

    const addRow = getByTestId('add-purchaser-button');

    await act(async () => userEvent.click(addRow));
    await act(async () => userEvent.click(getByTestId('dispositionPurchasers.0.remove-button')));

    expect(getByText(/Do you wish to remove this purchaser/i)).toBeVisible();

    await act(async () => userEvent.click(getByTitle('cancel-modal')));
    expect(getByTestId('purchaserRow[0]')).toBeInTheDocument();
  });

  it('Calculates the GST collected amount over the "Final Sale Amount" when the GST is required is set to "Yes"', async () => {
    const { container, getFinalSaleAmountTextbox, getGSTCollectedAmountTextbox } = await setup({
      props: { dispositionSaleId: null },
    });

    expect(getFinalSaleAmountTextbox()).toBeVisible();
    expect(getFinalSaleAmountTextbox()).toHaveValue('');
    expect(getGSTCollectedAmountTextbox()).toBeNull();

    await act(async () => {
      fireEvent.change(getFinalSaleAmountTextbox(), { target: { value: '$1,050,000.00' } });
    });
    waitForEffects();

    await act(async () => {
      fillInput(container, 'isGstRequired', 'true', 'select');
    });
    waitForEffects();

    expect(getGSTCollectedAmountTextbox()).toBeVisible();
    expect(getGSTCollectedAmountTextbox()).toHaveValue('$50,000.00');
  });

  it('Calculates the GST and displays warning when the GST is required is set to "NO"', async () => {
    const {
      container,
      getFinalSaleAmountTextbox,
      getGSTCollectedAmountTextbox,
      getByTitle,
      findByText,
    } = await setup({
      props: { dispositionSaleId: null },
    });

    expect(getFinalSaleAmountTextbox()).toBeVisible();
    expect(getFinalSaleAmountTextbox()).toHaveValue('');
    expect(getGSTCollectedAmountTextbox()).toBeNull();

    await act(async () => {
      fireEvent.change(getFinalSaleAmountTextbox(), { target: { value: '$1,050,000.00' } });
    });
    waitForEffects();

    await act(async () => {
      fillInput(container, 'isGstRequired', 'true', 'select');
    });
    waitForEffects();

    expect(getGSTCollectedAmountTextbox()).toBeVisible();
    expect(getGSTCollectedAmountTextbox()).toHaveValue('$50,000.00');

    await act(async () => {
      fillInput(container, 'isGstRequired', 'false', 'select');
    });
    waitForEffects();

    expect(
      await findByText(/The GST, if provided, will be cleared. Do you wish to proceed/i),
    ).toBeVisible();

    await act(async () => userEvent.click(getByTitle('ok-modal')));
    expect(getGSTCollectedAmountTextbox()).toBeNull();
  });

  it('Calculates the Net Proceeds without GST', async () => {
    const {
      getFinalSaleAmountTextbox,
      getRealtorCommissionAmountTextbox,
      getTotalCostSaleAmountTextbox,
      getNetBookAmountTextbox,
      getGSTCollectedAmountTextbox,
      getNetProceedsBeforeSPPAmountTextbox,
      getSPPAmountTextbox,
      getNetProceedsAfterSPPAmountTextbox,
    } = await setup({
      props: { dispositionSaleId: null },
    });

    expect(getFinalSaleAmountTextbox()).toBeVisible();
    expect(getFinalSaleAmountTextbox()).toHaveValue('');
    expect(getGSTCollectedAmountTextbox()).toBeNull();

    await act(async () => {
      fireEvent.change(getFinalSaleAmountTextbox(), { target: { value: '$10,000.00' } });
    });
    fireEvent.blur(getFinalSaleAmountTextbox());
    await waitForEffects();

    await act(async () => {
      fireEvent.change(getRealtorCommissionAmountTextbox(), { target: { value: '$100.00' } });
    });
    fireEvent.blur(getRealtorCommissionAmountTextbox());
    await waitForEffects();

    await act(async () => {
      fireEvent.change(getTotalCostSaleAmountTextbox(), { target: { value: '$100.00' } });
    });
    fireEvent.blur(getTotalCostSaleAmountTextbox());
    await waitForEffects();

    await act(async () => {
      fireEvent.change(getNetBookAmountTextbox(), { target: { value: '$300.00' } });
    });
    fireEvent.blur(getNetBookAmountTextbox());
    await waitForEffects();

    expect(getNetProceedsBeforeSPPAmountTextbox()).toHaveValue('$9,500.00');
    expect(getNetProceedsAfterSPPAmountTextbox()).toHaveValue('$9,500.00');

    await act(async () => {
      fireEvent.change(getSPPAmountTextbox(), { target: { value: '$500.00' } });
    });
    fireEvent.blur(getSPPAmountTextbox());
    await waitForEffects();

    expect(getNetProceedsBeforeSPPAmountTextbox()).toHaveValue('$9,500.00');
    expect(getNetProceedsAfterSPPAmountTextbox()).toHaveValue('$9,000.00');
  });

  it('Calculates the Net Proceeds WITH GST', async () => {
    const {
      container,
      getFinalSaleAmountTextbox,
      getRealtorCommissionAmountTextbox,
      getTotalCostSaleAmountTextbox,
      getNetBookAmountTextbox,
      getGSTCollectedAmountTextbox,
      getNetProceedsBeforeSPPAmountTextbox,
      getSPPAmountTextbox,
      getNetProceedsAfterSPPAmountTextbox,
    } = await setup({
      props: { dispositionSaleId: null },
    });

    expect(getFinalSaleAmountTextbox()).toBeVisible();
    expect(getFinalSaleAmountTextbox()).toHaveValue('');
    expect(getGSTCollectedAmountTextbox()).toBeNull();

    await act(async () => {
      fireEvent.change(getFinalSaleAmountTextbox(), { target: { value: '$10,500.00' } });
    });
    fireEvent.blur(getFinalSaleAmountTextbox());
    await waitForEffects();

    await act(async () => {
      fireEvent.change(getRealtorCommissionAmountTextbox(), { target: { value: '$100.00' } });
    });
    fireEvent.blur(getRealtorCommissionAmountTextbox());
    await waitForEffects();

    await act(async () => {
      fireEvent.change(getTotalCostSaleAmountTextbox(), { target: { value: '$100.00' } });
    });
    fireEvent.blur(getTotalCostSaleAmountTextbox());
    await waitForEffects();

    await act(async () => {
      fireEvent.change(getNetBookAmountTextbox(), { target: { value: '$300.00' } });
    });
    fireEvent.blur(getNetBookAmountTextbox());
    await waitForEffects();

    expect(getNetProceedsBeforeSPPAmountTextbox()).toHaveValue('$10,000.00');
    expect(getNetProceedsAfterSPPAmountTextbox()).toHaveValue('$10,000.00');

    await act(async () => {
      fireEvent.change(getSPPAmountTextbox(), { target: { value: '$500.00' } });
    });
    fireEvent.blur(getSPPAmountTextbox());
    await waitForEffects();

    expect(getNetProceedsBeforeSPPAmountTextbox()).toHaveValue('$10,000.00');
    expect(getNetProceedsAfterSPPAmountTextbox()).toHaveValue('$9,500.00');

    await act(async () => {
      fillInput(container, 'isGstRequired', 'true', 'select');
    });
    fireEvent.blur(getGSTCollectedAmountTextbox());
    await waitForEffects();

    expect(getGSTCollectedAmountTextbox()).toBeVisible();
    expect(getGSTCollectedAmountTextbox()).toHaveValue('$500.00');

    expect(getNetProceedsBeforeSPPAmountTextbox()).toHaveValue('$9,500.00');
    expect(getNetProceedsAfterSPPAmountTextbox()).toHaveValue('$9,000.00');
  });

  it('Calculates the Net Proceeds WITH Negative Values', async () => {
    const {
      getFinalSaleAmountTextbox,
      getRealtorCommissionAmountTextbox,
      getGSTCollectedAmountTextbox,
      getNetProceedsBeforeSPPAmountTextbox,
      getNetProceedsAfterSPPAmountTextbox,
    } = await setup({
      props: { dispositionSaleId: null },
    });

    expect(getFinalSaleAmountTextbox()).toBeVisible();
    expect(getFinalSaleAmountTextbox()).toHaveValue('');
    expect(getGSTCollectedAmountTextbox()).toBeNull();

    await act(async () => {
      fireEvent.change(getFinalSaleAmountTextbox(), { target: { value: '$1,000.00' } });
    });
    fireEvent.blur(getFinalSaleAmountTextbox());
    await waitForEffects();

    await act(async () => {
      fireEvent.change(getRealtorCommissionAmountTextbox(), { target: { value: '$1,500.00' } });
    });
    fireEvent.blur(getRealtorCommissionAmountTextbox());
    await waitForEffects();

    expect(getNetProceedsBeforeSPPAmountTextbox()).toHaveValue('-$500.00');
    expect(getNetProceedsAfterSPPAmountTextbox()).toHaveValue('-$500.00');
  });

  it('Displays Warning when removing the GST is required', async () => {
    const {
      container,
      getFinalSaleAmountTextbox,
      getRealtorCommissionAmountTextbox,
      getTotalCostSaleAmountTextbox,
      getNetBookAmountTextbox,
      getGSTCollectedAmountTextbox,
      getNetProceedsBeforeSPPAmountTextbox,
      getSPPAmountTextbox,
      getNetProceedsAfterSPPAmountTextbox,
      getByText,
      getByTitle,
    } = await setup({
      props: { dispositionSaleId: null },
    });

    expect(getFinalSaleAmountTextbox()).toBeVisible();
    expect(getFinalSaleAmountTextbox()).toHaveValue('');
    expect(getGSTCollectedAmountTextbox()).toBeNull();

    await act(async () => {
      fireEvent.change(getFinalSaleAmountTextbox(), { target: { value: '10500' } });
    });

    await act(async () => {
      fillInput(container, 'isGstRequired', 'true', 'select');
    });
    await waitForEffects();

    expect(getGSTCollectedAmountTextbox()).toBeVisible();
    expect(getGSTCollectedAmountTextbox()).toHaveValue('$500.00');

    await act(async () => {
      fireEvent.change(getRealtorCommissionAmountTextbox(), { target: { value: '$100.00' } });
      fireEvent.change(getTotalCostSaleAmountTextbox(), { target: { value: '$100.00' } });
    });
    await waitForEffects();

    await act(async () => {
      fireEvent.change(getNetBookAmountTextbox(), { target: { value: '$300.00' } });
    });
    fireEvent.blur(getNetBookAmountTextbox());
    await waitForEffects();

    expect(getNetProceedsBeforeSPPAmountTextbox()).toHaveValue('$9,500.00');
    expect(getNetProceedsAfterSPPAmountTextbox()).toHaveValue('$9,500.00');

    await act(async () => {
      fireEvent.change(getSPPAmountTextbox(), { target: { value: '$500.00' } });
    });
    fireEvent.blur(getSPPAmountTextbox());

    expect(getNetProceedsBeforeSPPAmountTextbox()).toHaveValue('$9,500.00');
    expect(getNetProceedsAfterSPPAmountTextbox()).toHaveValue('$9,000.00');

    await act(async () => {
      fillInput(container, 'isGstRequired', 'false', 'select');
    });
    await waitForEffects();
    expect(
      getByText(/The GST, if provided, will be cleared. Do you wish to proceed/i),
    ).toBeVisible();
    await act(async () => userEvent.click(getByTitle('ok-modal')));

    expect(getGSTCollectedAmountTextbox()).toBeNull();
    expect(getFinalSaleAmountTextbox()).toHaveValue('$10,500.00');
    expect(getNetProceedsBeforeSPPAmountTextbox()).toHaveValue('$10,000.00');
    expect(getNetProceedsAfterSPPAmountTextbox()).toHaveValue('$9,500.00');
  });
});
