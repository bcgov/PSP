import { Claims } from '@/constants';
import { getMockExpropriationEvent } from '@/mocks/expropriationEvents.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import ExpropriationEventHistoryView, {
  IExpropriationEventHistoryViewProps,
} from './ExpropriationEventHistoryView';
import { ExpropriationEventRow } from './models';

describe('ExpropriationEventHistoryView', () => {
  const setup = (
    renderOptions: RenderOptions & { props?: Partial<IExpropriationEventHistoryViewProps> } = {},
  ) => {
    const defaultProps: IExpropriationEventHistoryViewProps = {
      isLoading: false,
      eventRows: [],
      sort: {},
      setSort: vi.fn(),
      onAdd: vi.fn(),
      onUpdate: vi.fn(),
      onDelete: vi.fn(),
    };

    const rendered = render(
      <ExpropriationEventHistoryView {...defaultProps} {...renderOptions.props} />,
      {
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        ...renderOptions,
      },
    );

    const tableRows = document.querySelectorAll('.table .tbody .tr-wrapper');

    return {
      ...rendered,
      tableRows,
    };
  };

  it('renders as expected', async () => {
    const { asFragment } = setup();
    expect(screen.getByText(/Expropriation Date History/i)).toBeVisible();
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays a message when no matching records found', async () => {
    const { tableRows } = setup({ props: { eventRows: [] } });
    expect(tableRows.length).toBe(0);
    expect(await screen.findByText(/No matching expropriations found/i)).toBeVisible();
  });

  it('displays expropriation row buttons (update, delete), when the user has permissions', async () => {
    setup({
      props: { eventRows: [ExpropriationEventRow.fromApi(getMockExpropriationEvent())] },
      claims: [Claims.ACQUISITION_EDIT],
    });

    const editButton = await screen.getByTestId('edit-expropriation-event-0');
    const deleteButton = await screen.getByTestId('delete-expropriation-event-0');

    expect(editButton).toBeVisible();
    expect(deleteButton).toBeVisible();
  });

  it('calls onAdd when add button is clicked', async () => {
    const onAdd = vi.fn();
    setup({
      props: {
        eventRows: [ExpropriationEventRow.fromApi(getMockExpropriationEvent())],
        onAdd,
      },
      claims: [Claims.ACQUISITION_EDIT],
    });

    const addButton = await screen.getByTestId('add-expropriation-event');
    expect(addButton).toBeVisible();

    await act(async () => userEvent.click(addButton));

    expect(onAdd).toHaveBeenCalled();
  });

  it('calls onUpdate when edit button is clicked', async () => {
    const onUpdate = vi.fn();
    setup({
      props: {
        eventRows: [ExpropriationEventRow.fromApi(getMockExpropriationEvent())],
        onUpdate,
      },
      claims: [Claims.ACQUISITION_EDIT],
    });

    const editButton = await screen.getByTestId('edit-expropriation-event-0');
    expect(editButton).toBeVisible();

    await act(async () => userEvent.click(editButton));

    expect(onUpdate).toHaveBeenCalled();
  });

  it('calls onDelete when trashcan button is clicked', async () => {
    const onDelete = vi.fn();
    setup({
      props: {
        eventRows: [ExpropriationEventRow.fromApi(getMockExpropriationEvent())],
        onDelete,
      },
      claims: [Claims.ACQUISITION_EDIT],
    });

    const deleteButton = await screen.getByTestId('delete-expropriation-event-0');
    expect(deleteButton).toBeVisible();

    await act(async () => userEvent.click(deleteButton));

    expect(onDelete).toHaveBeenCalled();
  });
});
