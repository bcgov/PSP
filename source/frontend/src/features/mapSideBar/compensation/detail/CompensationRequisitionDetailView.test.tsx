import { createMemoryHistory } from 'history';

import Claims from '@/constants/claims';
import { Roles } from '@/constants/index';
import {
  getMockApiAcquisitionFileOwnerOrganization,
  getMockApiAcquisitionFileOwnerPerson,
  mockAcquisitionFileResponse,
  mockApiAcquisitionFileTeamOrganization,
  mockApiAcquisitionFileTeamPerson,
} from '@/mocks/acquisitionFiles.mock';
import {
  emptyCompensationFinancial,
  getMockApiCompensationWithProperty,
  getMockApiDefaultCompensation,
  getMockCompensationPropertiesReq,
  getMockCompReqPayee,
} from '@/mocks/compensations.mock';
import {
  getMockApiInterestHolderOrganization,
  getMockApiInterestHolderPerson,
} from '@/mocks/interestHolders.mock';
import { ApiGen_CodeTypes_AcquisitionStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_AcquisitionStatusTypes';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_CompReqPayee } from '@/models/api/generated/ApiGen_Concepts_CompReqPayee';
import { toTypeCodeNullable } from '@/utils/formUtils';
import { act, render, RenderOptions, userEvent, waitForEffects } from '@/utils/test-utils';

import CompensationRequisitionDetailView, {
  CompensationRequisitionDetailViewProps,
} from './CompensationRequisitionDetailView';

const setEditMode = vi.fn();
const onGenerate = vi.fn();

const history = createMemoryHistory();

describe('Compensation Detail View Component', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<CompensationRequisitionDetailViewProps> },
  ) => {
    // render component under test
    const component = render(
      <CompensationRequisitionDetailView
        fileType={renderOptions?.props?.fileType ?? ApiGen_CodeTypes_FileTypes.Acquisition}
        file={renderOptions?.props?.file ?? mockAcquisitionFileResponse()}
        compensation={renderOptions?.props?.compensation ?? getMockApiDefaultCompensation()}
        compensationProperties={
          renderOptions?.props?.compensationProperties ?? getMockCompensationPropertiesReq()
        }
        loading={renderOptions.props?.loading ?? false}
        setEditMode={setEditMode}
        clientConstant={renderOptions.props?.clientConstant ?? '034'}
        onGenerate={onGenerate}
        compensationLeaseStakeHolders={[]}
        compensationPayees={renderOptions?.props?.compensationPayees ?? []}
      />,
      {
        ...renderOptions,
        history: history,
        claims: renderOptions?.claims ?? [Claims.COMPENSATION_REQUISITION_VIEW],
      },
    );

    await waitForEffects();

    return {
      ...component,
    };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup({});
    expect(asFragment()).toMatchSnapshot();
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
    const acquisition = mockAcquisitionFileResponse();
    const mockFinalCompensation = getMockApiDefaultCompensation();

    const { queryByTitle } = await setup({
      claims: [Claims.COMPENSATION_REQUISITION_VIEW],
      props: {
        file: {
          ...acquisition,
          fileStatusTypeCode: toTypeCodeNullable(ApiGen_CodeTypes_AcquisitionStatusTypes.ACTIVE),
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
    const acquisition = mockAcquisitionFileResponse();
    const mockFinalCompensation = getMockApiDefaultCompensation();
    const { queryByTitle, getByTestId } = await setup({
      claims: [Claims.COMPENSATION_REQUISITION_EDIT],
      props: {
        file: {
          ...acquisition,
          fileStatusTypeCode: toTypeCodeNullable(ApiGen_CodeTypes_AcquisitionStatusTypes.COMPLT),
        },
        compensation: { ...mockFinalCompensation, isDraft: false },
      },
    });

    const editButton = queryByTitle('Edit compensation requisition');
    expect(editButton).not.toBeInTheDocument();
    const warningIcon = getByTestId(`tooltip-icon-1-compensation-cannot-edit-tooltip`);
    expect(warningIcon).toBeVisible();
  });

  it('User does not have the option to Edit Compensation when the file is in "FINAL" status', async () => {
    const acquisition = mockAcquisitionFileResponse();
    const mockFinalCompensation = getMockApiDefaultCompensation();
    const { queryByTitle, getByTestId } = await setup({
      claims: [Claims.COMPENSATION_REQUISITION_EDIT],
      props: {
        file: {
          ...acquisition,
          fileStatusTypeCode: toTypeCodeNullable(ApiGen_CodeTypes_AcquisitionStatusTypes.DRAFT),
        },
        compensation: { ...mockFinalCompensation, isDraft: false },
        isFileFinalStatus: true,
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

  it('displays the acquisition files properties selected', async () => {
    const { findByText } = await setup({
      claims: [Claims.COMPENSATION_REQUISITION_EDIT],
      props: {
        compensation: getMockApiCompensationWithProperty(),
        compensationProperties: getMockCompensationPropertiesReq(),
        file: mockAcquisitionFileResponse(),
      },
    });

    expect(await findByText(/Property Test Name 1/)).toBeVisible();
    expect(await findByText(/Property Test Name 2/)).toBeVisible();
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

  it('displays the Product information', async () => {
    const mockCompensation = getMockApiDefaultCompensation();
    const { queryByTestId } = await setup({
      claims: [Claims.COMPENSATION_REQUISITION_VIEW],
      props: {
        compensation: mockCompensation,
      },
    });

    expect(queryByTestId('file-product')).toHaveTextContent('00048');
  });

  it.each([
    [
      'OWNERS',
      [
        {
          ...getMockCompReqPayee(1),
          compensationRequisitionId: 11,
          acquisitionOwner: getMockApiAcquisitionFileOwnerPerson(),
        },
        {
          ...getMockCompReqPayee(2),
          compensationRequisitionId: 11,
          acquisitionOwner: getMockApiAcquisitionFileOwnerOrganization(),
        },
      ],
      ['JOHH DOE', 'FORTIS BC'],
    ],
    [
      'INTEREST HOLDERS',
      [
        {
          ...getMockCompReqPayee(1),
          compensationRequisitionId: 11,
          interestHolderId: 14,
          interestHolder: getMockApiInterestHolderPerson(14),
        },
        {
          ...getMockCompReqPayee(2),
          interestHolderId: 15,
          interestHolder: getMockApiInterestHolderOrganization(15),
        },
      ],
      ['Chester Tester', 'FORTIS BC'],
    ],
    [
      'ACQUITISION TEAM',
      [
        {
          ...getMockCompReqPayee(1),
          acquisitionFileTeamId: 11,
          acquisitionFileTeam: mockApiAcquisitionFileTeamPerson(11),
        },
        {
          ...getMockCompReqPayee(2),
          acquisitionFileTeamId: 12,
          acquisitionFileTeam: mockApiAcquisitionFileTeamOrganization(12),
        },
      ],
      ['first last', 'ABC Inc'],
    ],
  ])(
    'displays the compensation payees - for %s',
    async (_: string, compReqPayees: ApiGen_Concepts_CompReqPayee[], expectedValues: string[]) => {
      const { findByText } = await setup({
        claims: [Claims.COMPENSATION_REQUISITION_EDIT],
        props: {
          compensationPayees: compReqPayees,
        },
      });

      for (const expected of expectedValues) {
        expect(await findByText(new RegExp(expected, 'i'))).toBeVisible();
      }
    },
  );

  it('displays the compensation payees - for LEGACY PAYEE', async () => {
    const { findByText } = await setup({
      claims: [Claims.COMPENSATION_REQUISITION_EDIT],
      props: {
        compensationPayees: [
          {
            ...getMockCompReqPayee(1),
            legacyPayee: 'Legacy Test Value',
          },
        ],
        compensation: getMockApiDefaultCompensation(),
      },
    });

    expect(await findByText(/Legacy Test Value/i)).toBeVisible();
  });

  it('displays empty string - for NULL PAYEES', async () => {
    const { findByTestId } = await setup({
      claims: [Claims.COMPENSATION_REQUISITION_EDIT],
      props: {
        compensationPayees: [
          {
            ...getMockCompReqPayee(1),
            acquisitionFileTeam: null,
            acquisitionFileTeamId: null,
            interestHolder: null,
            interestHolderId: null,
            acquisitionOwnerId: null,
            acquisitionOwner: null,
          },
        ],
      },
    });

    expect(await findByTestId('comp-req-payees')).toHaveTextContent('');
  });

  it('calls onGenerate when generation button is clicked', async () => {
    const { getByTitle } = await setup({});

    const generateButton = getByTitle(/Download File/i);
    await act(async () => userEvent.click(generateButton));

    expect(onGenerate).toHaveBeenCalled();
  });
});
