import { Claims } from '@/constants';
import { getMockExpropriationEvent } from '@/mocks/expropriationEvents.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import { ExpropriationEventRow } from '../models';
import {
  ExpropriationEventResults,
  IExpropriationEventResultsProps,
} from './ExpropriationEventResults';

describe('ExpropriationEventResults', () => {
  const setup = (
    renderOptions: RenderOptions & { props?: Partial<IExpropriationEventResultsProps> } = {},
  ) => {
    const defaultProps: IExpropriationEventResultsProps = {
      loading: false,
      results: [],
      sort: {},
      setSort: vi.fn(),
      onUpdate: vi.fn(),
      onDelete: vi.fn(),
    };

    const rendered = render(
      <ExpropriationEventResults {...defaultProps} {...renderOptions.props} />,
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
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays a message when no matching records found', async () => {
    const { tableRows } = setup({ props: { results: [] } });
    expect(tableRows.length).toBe(0);
    expect(await screen.findByText(/No matching expropriations found/i)).toBeVisible();
  });

  it('displays the expropriation row buttons (update, delete) when the user has appropriate permissions', async () => {
    setup({
      props: { results: [ExpropriationEventRow.fromApi(getMockExpropriationEvent())] },
      claims: [Claims.ACQUISITION_EDIT],
    });

    const editButton = await screen.getByTestId('edit-expropriation-event-0');
    const deleteButton = await screen.getByTestId('delete-expropriation-event-0');

    expect(editButton).toBeVisible();
    expect(deleteButton).toBeVisible();
  });

  it('hides the expropriation row buttons (update, delete) when the user has insufficient permissions', async () => {
    setup({
      props: { results: [ExpropriationEventRow.fromApi(getMockExpropriationEvent())] },
      claims: [Claims.ACQUISITION_VIEW],
    });

    const editButton = await screen.queryByTestId('edit-expropriation-event-0');
    const deleteButton = await screen.queryByTestId('delete-expropriation-event-0');

    expect(editButton).toBeNull();
    expect(deleteButton).toBeNull();
  });

  it('calls onUpdate when edit button is clicked', async () => {
    const onUpdate = vi.fn();
    setup({
      props: {
        results: [ExpropriationEventRow.fromApi(getMockExpropriationEvent())],
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
        results: [ExpropriationEventRow.fromApi(getMockExpropriationEvent())],
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
        results: [
          ExpropriationEventRow.fromApi({
            ...getMockExpropriationEvent(),
            eventDate: '2025-01-10',
          }),
          ExpropriationEventRow.fromApi({
            ...getMockExpropriationEvent(),
            eventDate: '2001-03-30',
          }),
          ExpropriationEventRow.fromApi({
            ...getMockExpropriationEvent(),
            eventDate: '2014-12-31',
          }),
        ],
      },
      claims: [Claims.ACQUISITION_EDIT],
    });

    expect(dateColumnCells.length).toBe(3);
    expect(dateColumnCells[0]).toHaveTextContent('Jan 10, 2025');
    expect(dateColumnCells[1]).toHaveTextContent('Mar 30, 2001');
    expect(dateColumnCells[2]).toHaveTextContent('Dec 31, 2014');
  });

  it('calls setSort when sort icons are clicked', async () => {
    const setSort = vi.fn();
    const { dateColumnCells } = setup({
      props: {
        results: [ExpropriationEventRow.fromApi(getMockExpropriationEvent())],
        setSort,
      },
      claims: [Claims.ACQUISITION_EDIT],
    });

    await act(async () => {
      userEvent.click(screen.getByTestId('sort-column-eventDate'));
    });

    // column should be sorted in ascending order
    expect(setSort).toHaveBeenCalledWith({ eventDate: 'asc' });
  });
});
