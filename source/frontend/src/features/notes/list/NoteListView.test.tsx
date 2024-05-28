import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import Claims from '@/constants/claims';
import { NoteTypes } from '@/constants/noteTypes';
import { mockNotesResponse } from '@/mocks/noteResponses.mock';
import {
  mockKeycloak,
  render,
  RenderOptions,
  screen,
  waitFor,
  waitForElementToBeRemoved,
  within,
} from '@/utils/test-utils';

import { INoteListViewProps, NoteListView } from './NoteListView';

// mock auth library

describe('Note List View', () => {
  const mockAxios = new MockAdapter(axios);
  const setup = (renderOptions?: RenderOptions & INoteListViewProps) => {
    // render component under test
    const component = render(<NoteListView type={NoteTypes.Acquisition_File} entityId={0} />, {
      ...renderOptions,
    });

    const getTableRows = () => {
      return component.container.querySelectorAll<HTMLDivElement>('.table .tbody .tr-wrapper');
    };

    return {
      ...component,
      getTableRows,
      getTableCell: (row = 0, col = 0) => {
        const rows = getTableRows();
        const cells = within(rows[row]).getAllByRole('cell');
        if (col < cells.length) {
          return cells[col];
        }
        return null;
      },
    };
  };

  beforeEach(() => {
    mockKeycloak({ claims: [Claims.NOTE_DELETE] });
    mockAxios.reset();
  });

  it('renders as expected', async () => {
    mockAxios.onGet(new RegExp(`notes/${NoteTypes.Acquisition_File}/*`)).reply(200, {});
    const { asFragment, getByTitle } = setup();
    await waitForElementToBeRemoved(getByTitle('table-loading'));
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('should call the API Endpoint with given type', async () => {
    mockAxios.onGet(new RegExp(`notes/${NoteTypes.Acquisition_File}/*`)).reply(200, {});
    const { getByTitle } = setup({
      type: NoteTypes.Acquisition_File,
      entityId: 0,
    });
    await waitForElementToBeRemoved(getByTitle('table-loading'));
    await waitFor(() => {
      expect(mockAxios.history.get).toHaveLength(1);
      expect(mockAxios.history.get[0].url).toBe(`/notes/${NoteTypes.Acquisition_File}/0`);
    });
  });

  it('should have the Notes header in the component', async () => {
    mockAxios.onGet(new RegExp(`notes/${NoteTypes.Acquisition_File}/*`)).reply(200, {});
    const { getByTitle } = setup({ type: NoteTypes.Acquisition_File, entityId: 0 });
    await waitForElementToBeRemoved(getByTitle('table-loading'));
    expect(await screen.findByText(`Notes`)).toBeInTheDocument();
  });

  it('should display notes by default in descending order of created date', async () => {
    mockAxios
      .onGet(new RegExp(`notes/${NoteTypes.Acquisition_File}/*`))
      .reply(200, mockNotesResponse());

    const { getTableRows, getTableCell, getByTitle } = setup({
      type: NoteTypes.Acquisition_File,
      entityId: 0,
    });
    await waitForElementToBeRemoved(getByTitle('table-loading'));

    expect(getTableRows()).toHaveLength(4);
    expect(getTableCell(0, 0)).toHaveTextContent('Note 4');
    expect(getTableCell(1, 0)).toHaveTextContent('Note 3');
    expect(getTableCell(2, 0)).toHaveTextContent('Note 2');
    expect(getTableCell(3, 0)).toHaveTextContent('Note 1');
  });
});
