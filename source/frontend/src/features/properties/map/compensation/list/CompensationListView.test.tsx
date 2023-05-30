import Claims from 'constants/claims';
import { createMemoryHistory } from 'history';
import { getMockApiCompensationList } from 'mocks/compensations.mock';
import { mockLookups } from 'mocks/index.mock';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent, waitFor } from 'utils/test-utils';

import CompensationListView, { ICompensationListViewProps } from './CompensationListView';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const history = createMemoryHistory();
jest.mock('@react-keycloak/web');

const onDelete = jest.fn();
const onAddCompensationRequisition = jest.fn();

describe('compensation list view', () => {
  const setup = (renderOptions?: RenderOptions & Partial<ICompensationListViewProps>) => {
    // render component under test
    const component = render(
      <CompensationListView
        compensations={renderOptions?.compensations ?? []}
        onDelete={onDelete}
        onAdd={onAddCompensationRequisition}
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
    jest.restoreAllMocks();
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
    const { getAllByText } = setup({
      compensations: [
        {
          id: 1,
          acquisitionFileId: 1,
          isDraft: true,
          fiscalYear: null,
          yearlyFinancialId: null,
          yearlyFinancial: null,
          chartOfAccountsId: null,
          chartOfAccounts: null,
          responsibilityId: null,
          responsibility: null,
          agreementDate: null,
          expropriationNoticeServedDate: null,
          expropriationVestingDate: null,
          generationDate: null,
          specialInstruction: null,
          detailedRemarks: null,
          isDisabled: null,
          financials: [
            {
              id: 1,
              financialActivityCodeId: 1,
              financialActivityCode: null,
              compensationId: 1,
              pretaxAmount: null,
              isGstRequired: false,
              taxAmount: null,
              totalAmount: 3,
              isDisabled: null,
              rowVersion: 1,
            },
            {
              id: 2,
              compensationId: 1,
              financialActivityCodeId: 1,
              financialActivityCode: null,
              pretaxAmount: null,
              isGstRequired: false,
              taxAmount: null,
              totalAmount: 10,
              isDisabled: null,
              rowVersion: 1,
            },
          ],
        },
        {
          id: 2,
          acquisitionFileId: 1,
          isDraft: false,
          fiscalYear: null,
          yearlyFinancialId: null,
          yearlyFinancial: null,
          chartOfAccountsId: null,
          chartOfAccounts: null,
          responsibilityId: null,
          responsibility: null,
          agreementDate: null,
          expropriationNoticeServedDate: null,
          expropriationVestingDate: null,
          generationDate: null,
          specialInstruction: null,
          detailedRemarks: null,
          isDisabled: null,
          financials: [
            {
              id: 3,
              financialActivityCodeId: 1,
              financialActivityCode: null,
              compensationId: 1,
              pretaxAmount: null,
              isGstRequired: false,
              taxAmount: null,
              totalAmount: 100,
              isDisabled: null,
              rowVersion: 1,
            },
            {
              id: 4,
              financialActivityCodeId: 1,
              financialActivityCode: null,
              compensationId: 1,
              pretaxAmount: null,
              isGstRequired: false,
              taxAmount: null,
              totalAmount: 500.55,
              isDisabled: null,
              rowVersion: 1,
            },
          ],
        },
      ],
    });

    await waitFor(() => {
      expect(getAllByText('$600.55')[1]).toBeVisible();
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
    const { findAllByTitle } = setup({
      compensations: getMockApiCompensationList(),
      claims: [Claims.COMPENSATION_REQUISITION_DELETE],
    });

    const deleteButton = (await findAllByTitle('Delete Compensation'))[0];
    act(() => userEvent.click(deleteButton));
    await waitFor(() => {
      expect(onDelete).toHaveBeenCalledWith(4);
    });
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
    act(() => userEvent.click(addButton));
    await waitFor(() => {
      expect(onAddCompensationRequisition).toHaveBeenCalled();
    });
  });
});
