import { FormikProps } from 'formik';
import { createRef } from 'react';

import { useProjectTypeahead } from '@/hooks/useProjectTypeahead';
import {
  mockAcquisitionFileOwnersResponse,
  mockAcquisitionFileResponse,
} from '@/mocks/acquisitionFiles.mock';
import {
  emptyCompensationFinancial,
  getMockApiDefaultCompensation,
} from '@/mocks/compensations.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_Concepts_AcquisitionFileOwner } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileOwner';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  fakeText,
  fireEvent,
  render,
  RenderOptions,
  screen,
  userEvent,
  waitFor,
} from '@/utils/test-utils';

import { PayeeOption } from '../../../models/PayeeOptionModel';
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
  '',
);

const getPayeeOptions = (owners: ApiGen_Concepts_AcquisitionFileOwner[]): PayeeOption[] => {
  const options: PayeeOption[] = [];

  const ownersOptions: PayeeOption[] = owners.map(x => PayeeOption.createOwner(x));
  options.push(...ownersOptions);

  return options;
};

jest.mock('@/hooks/useProjectTypeahead');
const mockUseProjectTypeahead = useProjectTypeahead as jest.MockedFunction<
  typeof useProjectTypeahead
>;

const handleTypeaheadSearch = jest.fn();
const setShowAltProjectError = jest.fn();

describe('Compensation Requisition UpdateForm component', () => {
  const payeeOptions = getPayeeOptions(mockAcquisitionFileOwnersResponse());

  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<CompensationRequisitionFormProps> },
  ) => {
    const formikRef = createRef<FormikProps<CompensationRequisitionFormModel>>();
    const utils = render(
      <UpdateCompensationRequisitionForm
        {...renderOptions.props}
        onSave={onSave}
        onCancel={onCancel}
        payeeOptions={renderOptions.props?.payeeOptions ?? payeeOptions}
        initialValues={renderOptions.props?.initialValues ?? defaultCompensation}
        financialActivityOptions={[]}
        chartOfAccountsOptions={[]}
        responsiblityCentreOptions={[]}
        yearlyFinancialOptions={[]}
        gstConstant={currentGstPercent ?? 0.05}
        acquisitionFile={renderOptions.props?.acquisitionFile ?? mockAcquisitionFileResponse()}
        isLoading={renderOptions.props?.isLoading ?? false}
        showAltProjectError={false}
        setShowAltProjectError={setShowAltProjectError}
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
      getProjectSelector: () => {
        return utils.container.querySelector(
          `input[name="typeahead-alternateProject"]`,
        ) as HTMLInputElement;
      },
      getProjectSelectorItem: (index: number) => {
        return utils.container.querySelector(
          `#typeahead-alternateProject-item-${index}`,
        ) as HTMLElement;
      },
      getAdvancedPaymentServedDate: () => {
        return utils.container.querySelector(
          `input[name="advancedPaymentServedDate"]`,
        ) as HTMLInputElement;
      },
      getPayeeOptionsDropDown: () =>
        utils.container.querySelector(`select[name="payee.payeeKey"]`) as HTMLInputElement,
      getPayeeGSTNumber: () =>
        utils.container.querySelector(`input[name="payee.gstNumber"]`) as HTMLInputElement,
      getPayeePaymentInTrust: () =>
        utils.container.querySelector(`input[name="payee.isPaymentInTrust"]`) as HTMLInputElement,
      getPayeePreTaxAmount: () =>
        utils.container.querySelector(`input[name="payee.pretaxAmount"]`) as HTMLInputElement,
      getPayeeTaxAmount: () =>
        utils.container.querySelector(`input[name="payee.taxAmount"]`) as HTMLInputElement,
      getPayeeTotalAmount: () =>
        utils.container.querySelector(`input[name="payee.totalAmount"]`) as HTMLInputElement,
      getSpecialInstructionsTextbox: () =>
        utils.container.querySelector(`textarea[name="specialInstruction"]`) as HTMLInputElement,
      getDetailedRemarksTextbox: () =>
        utils.container.querySelector(`textarea[name="detailedRemarks"]`) as HTMLInputElement,
    };
  };

  beforeEach(() => {
    mockUseProjectTypeahead.mockReturnValue({
      handleTypeaheadSearch,
      isTypeaheadLoading: false,
      matchedProjects: [
        {
          id: 1,
          text: 'MOCK TEST PROJECT',
        },
        {
          id: 2,
          text: 'ANOTHER MOCK',
        },
      ],
    });
  });

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
    const apiCompensation = getMockApiDefaultCompensation();
    const compensationWithPayeeInformation = CompensationRequisitionFormModel.fromApi({
      ...apiCompensation,
      fiscalYear: '2020',
      acquisitionOwnerId: 1,
      isDraft: true,
      gstNumber: '9999',
      isPaymentInTrust: true,
      financials: [
        {
          ...emptyCompensationFinancial,
          pretaxAmount: 30000,
          taxAmount: 1500,
          totalAmount: 31500,
        },
      ],
    });

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

  it('should NOT display confirmation modal when saving a compensation with Status as "Draft"', async () => {
    const apiCompensation = getMockApiDefaultCompensation();
    const mockCompensation = CompensationRequisitionFormModel.fromApi({
      ...apiCompensation,
      fiscalYear: '2020',
      acquisitionOwnerId: 1,
      isDraft: true,
    });

    const { queryByText, getSpecialInstructionsTextbox } = await setup({
      props: { initialValues: mockCompensation },
    });

    await act(async () => {
      await waitFor(() => userEvent.paste(getSpecialInstructionsTextbox(), 'updated value'));
    });

    const saveButton = screen.getByText('Save');
    await act(async () => userEvent.click(saveButton));

    expect(
      queryByText(/You have selected to change the status from DRAFT to FINAL./i),
    ).not.toBeInTheDocument();
    expect(onSave).toHaveBeenCalled();
  });

  it('should display confirmation modal when saving a compensation with Status as "FINAL"', async () => {
    const apiCompensation = getMockApiDefaultCompensation();
    const mockCompensation = CompensationRequisitionFormModel.fromApi({
      ...apiCompensation,
      fiscalYear: '2020',
      acquisitionOwnerId: 1,
      isDraft: true,
    });

    const { findByText, getStatusDropDown, getByTitle } = await setup({
      props: { initialValues: mockCompensation },
    });

    await act(async () => {
      fireEvent.change(getStatusDropDown(), { target: { value: 'final' } });
    });

    const saveButton = screen.getByText('Save');
    await act(async () => userEvent.click(saveButton));

    expect(onSave).not.toHaveBeenCalled();
    expect(
      await findByText(/You have selected to change the status from DRAFT to FINAL./i),
    ).toBeVisible();

    await act(async () => userEvent.click(getByTitle('cancel-modal')));
    expect(onSave).not.toHaveBeenCalled();
  });

  it('save a compensation with Status as "FINAL" after confirming modal', async () => {
    const apiCompensation = getMockApiDefaultCompensation();
    const mockCompensation = CompensationRequisitionFormModel.fromApi({
      ...apiCompensation,
      fiscalYear: '2020',
      acquisitionOwnerId: 1,
      isDraft: true,
    });
    const { findByText, getStatusDropDown, getByTitle } = await setup({
      props: { initialValues: mockCompensation },
    });

    await act(async () => {
      fireEvent.change(getStatusDropDown(), { target: { value: 'final' } });
    });

    const saveButton = screen.getByText('Save');
    await act(async () => userEvent.click(saveButton));

    expect(onSave).not.toHaveBeenCalled();
    expect(
      await findByText(/You have selected to change the status from DRAFT to FINAL./i),
    ).toBeVisible();

    await act(async () => userEvent.click(getByTitle('ok-modal')));

    expect(onSave).toHaveBeenCalled();
  });

  it('displays the compensation finalized date', async () => {
    const mockCompensation = CompensationRequisitionFormModel.fromApi({
      ...getMockApiDefaultCompensation(),
      isDraft: false,
      finalizedDate: '2024-06-12T00:00:00',
    });
    const { getByTestId } = await setup({
      props: { initialValues: mockCompensation },
    });

    const compensationFinalizedDate = getByTestId('compensation-finalized-date');
    expect(compensationFinalizedDate).toHaveTextContent('Jun 12, 2024');
  });

  it('should display the LEGACY payee information', async () => {
    const apiCompensation = {
      ...getMockApiDefaultCompensation(),
      fiscalYear: '2020',
      isDraft: true,
      gstNumber: '9999',
      isPaymentInTrust: true,
      acquisitionOwnerId: null,
      interestHolderId: null,
      acquisitionFilePersonId: null,
      legacyPayee: 'Stark, Tony',
      financials: [
        {
          ...emptyCompensationFinancial,
          pretaxAmount: 30000,
          taxAmount: 1500,
          totalAmount: 31500,
        },
      ],
    };

    const compensationWithPayeeInformation =
      CompensationRequisitionFormModel.fromApi(apiCompensation);

    const payeesAndLegacyOptions = [
      ...payeeOptions,
      PayeeOption.createLegacyPayee(apiCompensation),
    ];

    const {
      getPayeePreTaxAmount,
      getPayeeTaxAmount,
      getPayeeTotalAmount,
      getPayeeGSTNumber,
      getPayeePaymentInTrust,
      getPayeeOptionsDropDown,
    } = await setup({
      props: {
        initialValues: compensationWithPayeeInformation,
        payeeOptions: payeesAndLegacyOptions,
      },
    });

    expect(getPayeeOptionsDropDown()).toHaveValue('LEGACY_PAYEE-1');
    expect(getPayeePaymentInTrust()).toBeChecked();
    expect(getPayeeGSTNumber()).toHaveValue('9999');
    expect(getPayeePreTaxAmount()).toHaveValue('$30,000.00');
    expect(getPayeeTaxAmount()).toHaveValue('$1,500.00');
    expect(getPayeeTotalAmount()).toHaveValue('$31,500.00');
  });

  it('should validate alternate project same as file project', async () => {
    const acquisitionFile = { ...mockAcquisitionFileResponse(), projectId: 1 };
    const apiCompensation = {
      ...getMockApiDefaultCompensation(),
      isDraft: false,
      finalizedDate: '2024-06-12T18:00:00',
    };
    const mockCompensation = CompensationRequisitionFormModel.fromApi(apiCompensation);

    const { getProjectSelector, getProjectSelectorItem } = await setup({
      props: { initialValues: mockCompensation, acquisitionFile: acquisitionFile },
    });

    await act(async () => userEvent.type(getProjectSelector(), 'MOCK TEST'));
    await act(async () => {
      fireEvent.click(getProjectSelectorItem(0));
    });

    expect(setShowAltProjectError).toHaveBeenCalledWith(true);
  });

  it('displays the compensation advanced payment served date', async () => {
    const mockCompensation = CompensationRequisitionFormModel.fromApi({
      ...getMockApiDefaultCompensation(),
      isDraft: false,
      advancedPaymentServedDate: '2024-09-16T00:00:00',
    });
    const { getAdvancedPaymentServedDate } = await setup({
      props: { initialValues: mockCompensation },
    });

    const inputServedDate = getAdvancedPaymentServedDate();
    expect(inputServedDate).toHaveValue('Sep 16, 2024');
  });
});
