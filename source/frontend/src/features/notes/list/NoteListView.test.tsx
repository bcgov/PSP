import { screen } from '@testing-library/react';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import Claims from 'constants/claims';
import { NoteTypes } from 'constants/noteTypes';
import { mockKeycloak, render, RenderOptions, waitFor } from 'utils/test-utils';

import { INoteListViewProps, NoteListView } from './NoteListView';

// mock auth library
jest.mock('@react-keycloak/web');

describe('Note List View', () => {
  const mockAxios = new MockAdapter(axios);
  const setup = (renderOptions?: RenderOptions & INoteListViewProps) => {
    // render component under test
    const component = render(<NoteListView type={NoteTypes.Acquisition_File} entityId={0} />, {
      ...renderOptions,
    });

    return {
      ...component,
    };
  };

  beforeEach(() => {
    mockKeycloak({ claims: [Claims.NOTE_DELETE] });
    mockAxios.reset();
  });

  it('renders as expected', async () => {
    mockAxios.onGet(new RegExp(`notes/${NoteTypes.Acquisition_File}/*`)).reply(200, {});
    const { asFragment } = setup();
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('should call the API Endpoint with given type', async () => {
    mockAxios.onGet(new RegExp(`notes/${NoteTypes.Acquisition_File}/owner/*`)).reply(200, {});
    setup({
      type: NoteTypes.Acquisition_File,
      entityId: 0,
    });
    await waitFor(() => {
      expect(mockAxios.history.get).toHaveLength(1);
      expect(mockAxios.history.get[0].url).toBe(`/notes/${NoteTypes.Acquisition_File}/owner/0`);
    });
  });

  it('should have the Notes header in the component', async () => {
    mockAxios.onGet(new RegExp(`notes/${NoteTypes.Acquisition_File}/owner/*`)).reply(200, {});
    setup({ type: NoteTypes.Acquisition_File, entityId: 0 });
    expect(await screen.findByText(`Notes`)).toBeInTheDocument();
  });
});
