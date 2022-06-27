import { screen } from '@testing-library/react';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { NoteType } from 'models/api/Note';
import { render, RenderOptions, waitFor } from 'utils/test-utils';

import { INoteResultProps, NoteListView } from './NoteListView';

describe('Note List View', () => {
  const mockAxios = new MockAdapter(axios);
  const setup = (renderOptions?: RenderOptions & INoteResultProps) => {
    // render component under test
    const component = render(<NoteListView type={NoteType.File} />, {
      ...renderOptions,
    });

    return {
      ...component,
    };
  };

  beforeEach(() => {
    mockAxios.reset();
  });

  it('renders as expected', async () => {
    mockAxios.onGet(new RegExp(`notes/${NoteType.File}/*`)).reply(200, {});
    const { asFragment } = setup();
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });
  it('should call the API Endpoint with given type', async () => {
    mockAxios.onGet(new RegExp(`notes/${NoteType.File}`)).reply(200, {});
    setup({
      type: NoteType.File,
    });
    await waitFor(() => {
      expect(mockAxios.history.get).toHaveLength(1);
      expect(mockAxios.history.get[0].url).toBe(`/notes/${NoteType.File}`);
    });
  });
  it('should have the Notes header in the component', async () => {
    mockAxios.onGet(new RegExp(`notes/${NoteType.File}`)).reply(200, {});
    setup({ type: NoteType.File });
    expect(screen.getByText(`Notes`)).toBeInTheDocument();
  });
});
