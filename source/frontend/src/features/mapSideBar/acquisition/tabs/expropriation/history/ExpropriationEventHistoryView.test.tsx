import { Claims } from '@/constants';
import { getMockExpropriationEvent } from '@/mocks/expropriationEvents.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import ExpropriationEventHistoryView, {
  IExpropriationEventHistoryViewProps,
} from './ExpropriationEventHistoryView';

describe('ExpropriationEventHistoryView', () => {
  const setup = (
    renderOptions: RenderOptions & { props?: Partial<IExpropriationEventHistoryViewProps> } = {},
  ) => {
    const defaultProps: IExpropriationEventHistoryViewProps = {
      isLoading: false,
      expropriationEvents: [],
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

    const dateColumnCells = document.querySelectorAll(
      '.table .tbody .tr-wrapper .tr .td:first-child',
    );

    return {
      ...rendered,
      tableRows,
      dateColumnCells,
    };
  };

  it('renders as expected', async () => {
    const { asFragment } = setup();
    expect(screen.getByText(/Expropriation Date History/i)).toBeVisible();
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays a message when no matching records found', async () => {
    const { tableRows } = setup({ props: { expropriationEvents: [] } });
    expect(tableRows.length).toBe(0);
    expect(await screen.findByText(/No matching expropriations found/i)).toBeVisible();
  });

  it('displays expropriation row buttons (update, delete), when the user has permissions', async () => {
    setup({
      props: { expropriationEvents: [getMockExpropriationEvent()] },
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
        expropriationEvents: [getMockExpropriationEvent()],
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
        expropriationEvents: [getMockExpropriationEvent()],
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
        expropriationEvents: [getMockExpropriationEvent()],
        onDelete,
      },
      claims: [Claims.ACQUISITION_EDIT],
    });

    const deleteButton = await screen.getByTestId('delete-expropriation-event-0');
    expect(deleteButton).toBeVisible();

    await act(async () => userEvent.click(deleteButton));

    expect(onDelete).toHaveBeenCalled();
  });

  it('displays the expropriation event history unsorted by default', async () => {
    const { dateColumnCells } = setup({
      props: {
        expropriationEvents: [
          {
            ...getMockExpropriationEvent(),
            eventDate: '2025-01-10',
          },
          {
            ...getMockExpropriationEvent(),
            eventDate: '2001-03-30',
          },
          {
            ...getMockExpropriationEvent(),
            eventDate: '2014-12-31',
          },
        ],
      },
      claims: [Claims.ACQUISITION_EDIT],
    });

    expect(dateColumnCells.length).toBe(3);
    expect(dateColumnCells[0]).toHaveTextContent('Jan 10, 2025');
    expect(dateColumnCells[1]).toHaveTextContent('Mar 30, 2001');
    expect(dateColumnCells[2]).toHaveTextContent('Dec 31, 2014');
  });

  it('sorts the expropriation event history by date', async () => {
    const { dateColumnCells } = setup({
      props: {
        expropriationEvents: [
          {
            ...getMockExpropriationEvent(),
            eventDate: '2025-01-10',
          },
          {
            ...getMockExpropriationEvent(),
            eventDate: '2001-03-30',
          },
          {
            ...getMockExpropriationEvent(),
            eventDate: '2014-12-31',
          },
        ],
      },
      claims: [Claims.ACQUISITION_EDIT],
    });

    await act(async () => {
      userEvent.click(screen.getByTestId('sort-column-eventDate'));
    });

    // column should be sorted in ascending order
    expect(dateColumnCells.length).toBe(3);
    expect(dateColumnCells[0]).toHaveTextContent('Mar 30, 2001');
    expect(dateColumnCells[1]).toHaveTextContent('Dec 31, 2014');
    expect(dateColumnCells[2]).toHaveTextContent('Jan 10, 2025');
  });
});
