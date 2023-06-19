import { FormikProps } from 'formik';
import { createRef } from 'react';

import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { getMockApiCompensation, getMockApiDefaultCompensation } from '@/mocks/compensations.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  fakeText,
  fillInput,
  fireEvent,
  render,
  RenderOptions,
  userEvent,
  waitFor,
} from '@/utils/test-utils';

import { CompensationRequisitionFormModel } from './models';
import UpdateCompensationRequisitionForm, {
  CompensationRequisitionFormProps,
} from './UpdateCompensationRequisitionForm';

const currentGstPercent = 0.05;
const onSave = jest.fn();
const onCancel = jest.fn();

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const defauiltApiCompensation = getMockApiDefaultCompensation();
const defaultCompensation = new CompensationRequisitionFormModel(
  defauiltApiCompensation.id,
  defauiltApiCompensation.acquisitionFileId,
);

describe('Compensation Requisition UpdateForm component', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<CompensationRequisitionFormProps> },
  ) => {
    const formikRef = createRef<FormikProps<CompensationRequisitionFormModel>>();
    const utils = render(
      <UpdateCompensationRequisitionForm
        {...renderOptions.props}
        onSave={onSave}
        onCancel={onCancel}
        payeeOptions={[]}
        initialValues={renderOptions.props?.initialValues ?? defaultCompensation}
        financialActivityOptions={[]}
        chartOfAccountsOptions={[]}
        responsiblityCentreOptions={[]}
        yearlyFinancialOptions={[]}
        gstConstant={currentGstPercent ?? 0.05}
        acquisitionFile={renderOptions.props?.acquisitionFile ?? mockAcquisitionFileResponse()}
        isLoading={renderOptions.props?.isLoading ?? false}
      />,
      {
        ...renderOptions,
        store: storeState,
      },
    );

    return {
      ...utils,
      formikRef,
      getStatusDropDown: () =>
        utils.container.querySelector(`select[name="status"]`) as HTMLInputElement,
      getPayeeGSTNumber: () =>
        utils.container.querySelector(`input[name="payees.0.gstNumber"]`) as HTMLInputElement,
      getPayeePaymentInTrust: () =>
        utils.container.querySelector(
          `input[name="payees.0.isPaymentInTrust"]`,
        ) as HTMLInputElement,
      getPayeePreTaxAmount: () =>
        utils.container.querySelector(`input[name="payees.0.pretaxAmount"]`) as HTMLInputElement,
      getPayeeTaxAmount: () =>
        utils.container.querySelector(`input[name="payees.0.taxAmount"]`) as HTMLInputElement,
      getPayeeTotalAmount: () =>
        utils.container.querySelector(`input[name="payees.0.totalAmount"]`) as HTMLInputElement,
      getSpecialInstructionsTextbox: () =>
        utils.container.querySelector(`textarea[name="specialInstruction"]`) as HTMLInputElement,
      getDetailedRemarksTextbox: () =>
        utils.container.querySelector(`textarea[name="detailedRemarks"]`) as HTMLInputElement,
    };
  };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays a loading spinner when loading', async () => {
    const { getByTestId } = await setup({ props: { isLoading: true } });
    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
  });

  it('should validate character limits', async () => {
    const { findByText, getByText, getSpecialInstructionsTextbox, getDetailedRemarksTextbox } =
      await setup({
        props: { initialValues: defaultCompensation },
      });

    await act(async () => {
      await waitFor(() => userEvent.paste(getSpecialInstructionsTextbox(), fakeText(2001)));
      await waitFor(() => userEvent.paste(getDetailedRemarksTextbox(), fakeText(2001)));
    });

    const saveButton = getByText('Save');
    userEvent.click(saveButton);

    expect(await findByText(/Special instructions must be at most 2000 characters/i)).toBeVisible();
    expect(await findByText(/Detailed remarks must be at most 2000 characters/i)).toBeVisible();
  });

  it('should display the payee information', async () => {
    const compensationWithPayeeInformation = CompensationRequisitionFormModel.fromApi(
      getMockApiCompensation(),
    );
    const {
      getPayeePreTaxAmount,
      getPayeeTaxAmount,
      getPayeeTotalAmount,
      getPayeeGSTNumber,
      getPayeePaymentInTrust,
    } = await setup({ props: { initialValues: compensationWithPayeeInformation } });

    expect(getPayeePaymentInTrust()).toBeChecked();
    expect(getPayeeGSTNumber()).toHaveValue('9999');
    expect(getPayeePreTaxAmount()).toHaveValue('$30,000.00');
    expect(getPayeeTaxAmount()).toHaveValue('$1,500.00');
    expect(getPayeeTotalAmount()).toHaveValue('$31,500.00');
  });

  it('should display confirmation modal when changing the status to "FINAL"', async () => {
    const { findByText, getByTitle, getStatusDropDown } = await setup({
      props: { initialValues: defaultCompensation },
    });

    await act(async () => {
      fireEvent.change(getStatusDropDown(), { target: { value: 'final' } });
    });

    expect(
      await findByText(/You have selected to change the status from DRAFT to FINAL./i),
    ).toBeVisible();

    await act(async () => userEvent.click(getByTitle('ok-modal')));

    expect(getStatusDropDown()).toHaveValue('final');
  });

  it('should return status to Draft when confirmation modal cancel', async () => {
    const { container, findByText, getByTitle, getStatusDropDown } = await setup({
      props: { initialValues: defaultCompensation },
    });

    await act(async () => {
      await fillInput(container, 'status', 'final', 'select');
    });

    expect(
      await findByText(/You have selected to change the status from DRAFT to FINAL./i),
    ).toBeVisible();

    await act(async () => userEvent.click(getByTitle('cancel-modal')));
    expect(getStatusDropDown()).toHaveValue('draft');
  });

  it('should validate extra fields when changing to final status', async () => {
    const { getStatusDropDown, findByText, getByText, getByTitle } = await setup({
      props: { initialValues: defaultCompensation },
    });

    await act(async () => {
      fireEvent.change(getStatusDropDown(), { target: { value: 'final' } });
    });

    expect(
      await findByText(/You have selected to change the status from DRAFT to FINAL./i),
    ).toBeVisible();

    await act(async () => userEvent.click(getByTitle('ok-modal')));

    const saveButton = getByText('Save');
    userEvent.click(saveButton);

    expect(await findByText(/Fiscal year is required/i)).toBeVisible();
  });
});
