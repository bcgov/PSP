import { createMemoryHistory } from 'history';

import Claims from '@/constants/claims';
import Roles from '@/constants/roles';
import {
  emptyCompensationFinancial,
  emptyCompensationRequisition,
  getMockApiCompensationList,
} from '@/mocks/compensations.mock';
import { mockAcquisitionFileResponse, mockLookups } from '@/mocks/index.mock';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { toTypeCode, toTypeCodeNullable } from '@/utils/formUtils';
import {
  act,
  getByTestId,
  queryByTestId,
  render,
  RenderOptions,
  userEvent,
  waitFor,
  waitForEffects,
} from '@/utils/test-utils';

import CompensationListView, { ICompensationListViewProps } from './CompensationListView';
import { ApiGen_CodeTypes_AcquisitionStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_AcquisitionStatusTypes';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const history = createMemoryHistory();

const onDelete = vi.fn();
const onAddCompensationRequisition = vi.fn();
const onUpdateTotalCompensation = vi.fn();

const mockAcquisitionfile = mockAcquisitionFileResponse();

describe('compensation list view', () => {
  const setup = (renderOptions?: RenderOptions & Partial<ICompensationListViewProps>) => {
    // render component under test
    const component = render(
      <CompensationListView
        fileType={renderOptions?.fileType ?? ApiGen_CodeTypes_FileTypes.Acquisition}
        file={renderOptions?.file ?? { ...mockAcquisitionfile }}
        compensationsResults={renderOptions?.compensationsResults ?? []}
        subFilescompensations={renderOptions?.subFilescompensations ?? []}
        isLoading={false}
        onDelete={onDelete}
        onAdd={onAddCompensationRequisition}
        onUpdateTotalCompensation={onUpdateTotalCompensation}
      />,
      {
        ...renderOptions,
        store: storeState,
        history: history,
        claims: renderOptions?.claims ?? [Claims.COMPENSATION_REQUISITION_VIEW],
      },
    );

    return {
      ...component,
    };
  };

  beforeEach(() => {
    vi.restoreAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup({
      claims: [],
    });
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('displays the expected message if no compensations are available', async () => {
    const { getByText } = setup();

    await waitFor(() => {
      expect(getByText('No matching Compensation Requisition(s) found')).toBeVisible();
    });
  });

  it('displays the calculated total for the final compensations', async () => {
    const mockList: ApiGen_Concepts_CompensationRequisition[] = [
      {
        ...emptyCompensationRequisition,
        isDraft: true,
        financials: [{ ...emptyCompensationFinancial, totalAmount: 500 }],
      },
      {
        ...emptyCompensationRequisition,
        isDraft: true,
        financials: [{ ...emptyCompensationFinancial, totalAmount: 100 }],
      },
      {
        ...emptyCompensationRequisition,
        isDraft: true,
        financials: [{ ...emptyCompensationFinancial, totalAmount: 0.55 }],
      },
      {
        ...emptyCompensationRequisition,
        isDraft: false,
        financials: [{ ...emptyCompensationFinancial, totalAmount: 500 }],
      },
    ];
    const { getByTestId } = setup({ compensationsResults: mockList, subFilescompensations: [] });
    waitForEffects();

    await waitFor(() => {
      expect(getByTestId('payment-total-main-file')).toHaveTextContent('$500.00');
      expect(getByTestId('payment-total-subfiles')).toHaveTextContent('$500.00');
    });
  });

  it('displays the calculated total for the final compensations with the subfiles', async () => {
    const mockList: ApiGen_Concepts_CompensationRequisition[] = [
      {
        ...emptyCompensationRequisition,
        isDraft: true,
        financials: [{ ...emptyCompensationFinancial, totalAmount: 500 }],
      },
      {
        ...emptyCompensationRequisition,
        isDraft: true,
        financials: [{ ...emptyCompensationFinancial, totalAmount: 100 }],
      },
      {
        ...emptyCompensationRequisition,
        isDraft: true,
        financials: [{ ...emptyCompensationFinancial, totalAmount: 0.55 }],
      },
      {
        ...emptyCompensationRequisition,
        isDraft: false,
        financials: [{ ...emptyCompensationFinancial, totalAmount: 500 }],
      },
    ];

    const mockSubFileList: ApiGen_Concepts_CompensationRequisition[] = [
      {
        ...emptyCompensationRequisition,
        isDraft: true,
        financials: [{ ...emptyCompensationFinancial, totalAmount: 0.55 }],
      },
      {
        ...emptyCompensationRequisition,
        isDraft: false,
        financials: [{ ...emptyCompensationFinancial, totalAmount: 1000 }],
      },
    ];
    const { getByTestId } = setup({
      compensationsResults: mockList,
      subFilescompensations: mockSubFileList,
    });
    waitForEffects();

    await waitFor(() => {
      expect(getByTestId('payment-total-main-file')).toHaveTextContent('$500.00');
      expect(getByTestId('payment-total-subfiles')).toHaveTextContent('$1,500.00');
    });
  });

  it('Does not display payment total for subfiles on LEASES', async () => {
    const { getByTestId, queryByTestId } = setup({
      fileType: ApiGen_CodeTypes_FileTypes.Lease,
      compensationsResults: [],
      subFilescompensations: null,
    });
    waitForEffects();

    expect(getByTestId('payment-total-main-file')).toHaveTextContent('$0.00');
    expect(queryByTestId('payment-total-subfiles')).not.toBeInTheDocument();
  });

  it('displays the calculated total for the drafts', async () => {
    const mockList: ApiGen_Concepts_CompensationRequisition[] = [
      {
        ...emptyCompensationRequisition,
        isDraft: true,
        financials: [{ ...emptyCompensationFinancial, totalAmount: 500 }],
      },
      {
        ...emptyCompensationRequisition,
        isDraft: true,
        financials: [{ ...emptyCompensationFinancial, totalAmount: 100 }],
      },
      {
        ...emptyCompensationRequisition,
        isDraft: true,
        financials: [{ ...emptyCompensationFinancial, totalAmount: 0.55 }],
      },
      {
        ...emptyCompensationRequisition,
        isDraft: false,
        financials: [{ ...emptyCompensationFinancial, totalAmount: 500 }],
      },
    ];
    const { getByTestId } = setup({ compensationsResults: mockList });

    await waitFor(() => {
      expect(getByTestId('payment-total-drafts')).toHaveTextContent('$600.55');
    });
  });

  it('displays the calculated total per compensation', async () => {
    const { getAllByText } = setup({
      compensationsResults: getMockApiCompensationList(),
    });

    await waitFor(() => {
      expect(getAllByText('$35.00')[1]).toBeVisible();
    });
  });

  it('can click the delete action on a given row', async () => {
    const compensations = getMockApiCompensationList();
    const { findAllByTitle } = setup({
      file: {
        ...mockAcquisitionFileResponse(),
        fileStatusTypeCode: toTypeCodeNullable(ApiGen_CodeTypes_AcquisitionStatusTypes.ACTIVE),
      },
      compensationsResults: compensations,
      claims: [Claims.COMPENSATION_REQUISITION_DELETE],
    });

    const deleteButton = (await findAllByTitle('Delete Compensation'))[0];
    await act(async () => userEvent.click(deleteButton));
    await waitFor(() => {
      expect(onDelete).toHaveBeenCalledWith(compensations[3].id);
    });
  });

  it('displays warning icon if compensation in final state', async () => {
    const compensations = getMockApiCompensationList();
    const { queryByTestId } = setup({
      file: {
        ...mockAcquisitionFileResponse(),
        fileStatusTypeCode: toTypeCode(ApiGen_CodeTypes_AcquisitionStatusTypes.ACTIVE),
      },
      compensationsResults: compensations,
      claims: [Claims.COMPENSATION_REQUISITION_DELETE],
    });

    const icon = queryByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');
    expect(icon).toBeVisible();
  });

  it('does not display warning icon if compensation in final state and user is admin', async () => {
    const compensations = getMockApiCompensationList();
    const { queryByTestId } = setup({
      file: {
        ...mockAcquisitionFileResponse(),
        fileStatusTypeCode: toTypeCode(ApiGen_CodeTypes_AcquisitionStatusTypes.ACTIVE),
      },
      compensationsResults: compensations,
      claims: [Claims.COMPENSATION_REQUISITION_DELETE],
      roles: [Roles.SYSTEM_ADMINISTRATOR],
    });

    const icon = queryByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');
    expect(icon).toBeNull();
  });

  it('delete action hidden if delete claim missing', async () => {
    const { queryByTitle } = setup({
      compensationsResults: getMockApiCompensationList(),
      claims: [],
    });

    const deleteButton = queryByTitle('Delete Compensation');
    await waitFor(() => {
      expect(deleteButton).toBeNull();
    });
  });

  it('can click the Add Compensation button', async () => {
    const { getByText } = setup({
      compensationsResults: getMockApiCompensationList(),
      claims: [Claims.COMPENSATION_REQUISITION_VIEW, Claims.COMPENSATION_REQUISITION_ADD],
    });

    const addButton = getByText('Add Requisition');
    expect(addButton).toBeVisible();
    await act(async () => userEvent.click(addButton));
    await waitFor(() => {
      expect(onAddCompensationRequisition).toHaveBeenCalled();
    });
  });
});
