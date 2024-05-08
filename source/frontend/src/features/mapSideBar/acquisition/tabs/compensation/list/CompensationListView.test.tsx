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
import { act, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import CompensationListView, { ICompensationListViewProps } from './CompensationListView';
import { ApiGen_CodeTypes_AcquisitionStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_AcquisitionStatusTypes';

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
        acquisitionFile={renderOptions?.acquisitionFile ?? { ...mockAcquisitionfile }}
        compensations={renderOptions?.compensations ?? []}
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

  it('displays the calculated total for the entire file excluding drafts', async () => {
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
    const { getAllByText } = setup({ compensations: mockList });

    await waitFor(() => {
      expect(getAllByText('$600.55')[0]).toBeVisible();
    });
  });

  it('displays the calculated total per compensation', async () => {
    const { getAllByText } = setup({
      compensations: getMockApiCompensationList(),
    });

    await waitFor(() => {
      expect(getAllByText('$35.00')[1]).toBeVisible();
    });
  });

  it('can click the delete action on a given row', async () => {
    const compensations = getMockApiCompensationList();
    const { findAllByTitle } = setup({
      acquisitionFile: {
        ...mockAcquisitionFileResponse(),
        fileStatusTypeCode: toTypeCodeNullable(ApiGen_CodeTypes_AcquisitionStatusTypes.ACTIVE),
      },
      compensations: compensations,
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
      acquisitionFile: {
        ...mockAcquisitionFileResponse(),
        fileStatusTypeCode: toTypeCode(ApiGen_CodeTypes_AcquisitionStatusTypes.ACTIVE),
      },
      compensations: compensations,
      claims: [Claims.COMPENSATION_REQUISITION_DELETE],
    });

    const icon = queryByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');
    expect(icon).toBeVisible();
  });

  it('does not display warning icon if compensation in final state and user is admin', async () => {
    const compensations = getMockApiCompensationList();
    const { queryByTestId } = setup({
      acquisitionFile: {
        ...mockAcquisitionFileResponse(),
        fileStatusTypeCode: toTypeCode(ApiGen_CodeTypes_AcquisitionStatusTypes.ACTIVE),
      },
      compensations: compensations,
      claims: [Claims.COMPENSATION_REQUISITION_DELETE],
      roles: [Roles.SYSTEM_ADMINISTRATOR],
    });

    const icon = queryByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');
    expect(icon).toBeNull();
  });

  it('delete action hidden if delete claim missing', async () => {
    const { queryByTitle } = setup({
      compensations: getMockApiCompensationList(),
      claims: [],
    });

    const deleteButton = queryByTitle('Delete Compensation');
    await waitFor(() => {
      expect(deleteButton).toBeNull();
    });
  });

  it('can click the Add Compensation button', async () => {
    const { getByText } = setup({
      compensations: getMockApiCompensationList(),
      claims: [Claims.COMPENSATION_REQUISITION_VIEW, Claims.COMPENSATION_REQUISITION_ADD],
    });

    const addButton = getByText('Add a Requisition');
    expect(addButton).toBeVisible();
    await act(async () => userEvent.click(addButton));
    await waitFor(() => {
      expect(onAddCompensationRequisition).toHaveBeenCalled();
    });
  });
});
