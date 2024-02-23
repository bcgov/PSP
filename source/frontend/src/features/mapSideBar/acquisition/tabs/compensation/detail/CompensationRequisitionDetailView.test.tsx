import { createMemoryHistory } from 'history';

import { AcquisitionStatus } from '@/constants/acquisitionFileStatus';
import Claims from '@/constants/claims';
import { Roles } from '@/constants/index';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import {
  emptyCompensationFinancial,
  getMockApiDefaultCompensation,
} from '@/mocks/compensations.mock';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { toTypeCodeNullable } from '@/utils/formUtils';
import { act, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import CompensationRequisitionDetailView, {
  CompensationRequisitionDetailViewProps,
} from './CompensationRequisitionDetailView';

const setEditMode = jest.fn();

const history = createMemoryHistory();
jest.mock('@react-keycloak/web');

describe('Compensation Detail View Component', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<CompensationRequisitionDetailViewProps> },
  ) => {
    // render component under test
    const component = render(
      <CompensationRequisitionDetailView
        acquisitionFile={renderOptions?.props?.acquisitionFile ?? mockAcquisitionFileResponse()}
        compensation={renderOptions?.props?.compensation ?? getMockApiDefaultCompensation()}
        loading={renderOptions.props?.loading ?? false}
        setEditMode={setEditMode}
        clientConstant={renderOptions.props?.clientConstant ?? '034'}
        onGenerate={jest.fn()}
        compensationContactPerson={undefined}
        compensationContactOrganization={undefined}
      />,
      {
        ...renderOptions,
        history: history,
        claims: renderOptions?.claims ?? [Claims.COMPENSATION_REQUISITION_VIEW],
      },
    );

    return {
      ...component,
    };
  };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup({});
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('Displays the Compensation Requisition Header Information with Draft Status', async () => {
    const mockCompensation = getMockApiDefaultCompensation();
    const { queryByTestId } = await setup({
      claims: [Claims.COMPENSATION_REQUISITION_VIEW],
      props: { compensation: { ...mockCompensation, isDraft: true } },
    });

    const editButton = queryByTestId('compensation-client');
    expect(editButton).toHaveTextContent('034');

    const compensationNumber = queryByTestId('compensation-number');
    expect(compensationNumber).toHaveTextContent('Draft');

    const headerPreTaxAmount = queryByTestId('header-pretax-amount');
    expect(headerPreTaxAmount).toHaveTextContent('$0.00');

    const headerTaxAmount = queryByTestId('header-tax-amount');
    expect(headerTaxAmount).toHaveTextContent('$0.00');

    const headerTotalAmount = queryByTestId('header-total-amount');
    expect(headerTotalAmount).toHaveTextContent('$0.00');
  });

  it('Displays the Compensation Requisition Header Information with Final Status', async () => {
    const mockCompensation = getMockApiDefaultCompensation();
    const { queryByTestId } = await setup({
      claims: [Claims.COMPENSATION_REQUISITION_VIEW],
      props: {
        compensation: {
          ...mockCompensation,
          isDraft: false,
          id: 1,
          financials: [
            {
              ...emptyCompensationFinancial,
              pretaxAmount: 30000,
              taxAmount: 1500,
              totalAmount: 31500,
            },
          ],
        },
      },
    });

    const editButton = queryByTestId('compensation-client');
    expect(editButton).toHaveTextContent('034');

    const compensationNumber = queryByTestId('compensation-number');
    expect(compensationNumber).toHaveTextContent('1');

    const headerPreTaxAmount = queryByTestId('header-pretax-amount');
    expect(headerPreTaxAmount).toHaveTextContent('$30,000.00');

    const headerTaxAmount = queryByTestId('header-tax-amount');
    expect(headerTaxAmount).toHaveTextContent('$1,500.00');

    const headerTotalAmount = queryByTestId('header-total-amount');
    expect(headerTotalAmount).toHaveTextContent('$31,500.00');
  });

  it('Edit Compensation Button not displayed without claims when is in "Draft" status', async () => {
    const acquistionFile = mockAcquisitionFileResponse();
    const mockFinalCompensation = getMockApiDefaultCompensation();

    const { queryByTitle } = await setup({
      claims: [Claims.COMPENSATION_REQUISITION_VIEW],
      props: {
        acquisitionFile: {
          ...acquistionFile,
          fileStatusTypeCode: toTypeCodeNullable(AcquisitionStatus.Active),
        },
        compensation: { ...mockFinalCompensation, isDraft: true },
      },
    });

    const editButton = queryByTitle('Edit compensation requisition');
    expect(editButton).not.toBeInTheDocument();
  });

  it('Can click on the Edit Compensation Button when is in "Draft" status', async () => {
    const mockCompensation = getMockApiDefaultCompensation();

    const { getByTitle } = await setup({
      claims: [Claims.COMPENSATION_REQUISITION_EDIT],
      props: {
        compensation: { ...mockCompensation, isDraft: true },
      },
    });

    const editButton = getByTitle('Edit compensation requisition');
    expect(editButton).toBeVisible();

    await act(async () => userEvent.click(editButton));

    expect(setEditMode).toHaveBeenCalled();
  });

  it('User does not have the option to Edit Compensation when is in "FINAL" status', async () => {
    const acquistionFile = mockAcquisitionFileResponse();
    const mockFinalCompensation = getMockApiDefaultCompensation();
    const { queryByTitle, getByTestId } = await setup({
      claims: [Claims.COMPENSATION_REQUISITION_EDIT],
      props: {
        acquisitionFile: {
          ...acquistionFile,
          fileStatusTypeCode: toTypeCodeNullable(AcquisitionStatus.Complete),
        },
        compensation: { ...mockFinalCompensation, isDraft: false },
      },
    });

    const editButton = queryByTitle('Edit compensation requisition');
    expect(editButton).not.toBeInTheDocument();
    const warningIcon = getByTestId(`tooltip-icon-1-compensation-cannot-edit-tooltip`);
    expect(warningIcon).toBeVisible();
  });

  it('Admin user should be able to Edit Compensation when is in "FINAL" status', async () => {
    const mockFinalCompensation = getMockApiDefaultCompensation();
    const { queryByTitle } = await setup({
      roles: [Roles.SYSTEM_ADMINISTRATOR],
      props: { compensation: { ...mockFinalCompensation, isDraft: false } },
    });

    const editButton = queryByTitle('Edit compensation requisition');
    expect(editButton).toBeInTheDocument();
  });

  it('displays the compensation finalized date', async () => {
    const mockFinalCompensation: ApiGen_Concepts_CompensationRequisition = {
      ...getMockApiDefaultCompensation(),
      isDraft: false,
      finalizedDate: '2024-06-12T18:00:00',
    };
    const { getByTestId } = await setup({
      roles: [Roles.SYSTEM_ADMINISTRATOR],
      props: { compensation: mockFinalCompensation },
    });

    const compensationFinalizedDate = getByTestId('compensation-finalized-date');
    expect(compensationFinalizedDate).toHaveTextContent('Jun 12, 2024');
  });

  it('Displays the Advanced Payment Served Date', async () => {
    const mockCompensation = getMockApiDefaultCompensation();
    const { queryByTestId } = await setup({
      claims: [Claims.COMPENSATION_REQUISITION_VIEW],
      props: {
        compensation: {
          ...mockCompensation,
          isDraft: true,
          advancedPaymentServedDate: '2023-09-18T00:00:00',
        },
      },
    });

    const advancedPaymntServedDate = queryByTestId('advanced-payment-served-date');
    expect(advancedPaymntServedDate).toHaveTextContent('Sep 18, 2023');
  });
});
