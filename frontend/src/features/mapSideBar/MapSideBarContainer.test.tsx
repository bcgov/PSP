import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';
import { cleanup, render, RenderOptions, waitFor } from 'utils/test-utils';

import MapSideBarContainer from './MapSideBarContainer';
describe('MapSideBarContainer component', () => {
  const mockAxios = new MockAdapter(axios);
  const history = createMemoryHistory();
  const setup = (renderOptions: RenderOptions = {}) => {
    // render component under test
    const component = render(<MapSideBarContainer />, {
      ...renderOptions,
      history,
    });

    return {
      component,
    };
  };

  it('requests ltsa data by pid', async () => {
    history.push('mapview?pid=9212434&searchBy=pinOrPid&sidebar=true');
    mockAxios.onPost().reply(200, {});
    mockAxios.onGet().reply(200, {});
    setup({});
    await waitFor(() => {
      expect(mockAxios.history.post).toHaveLength(1);
      expect(mockAxios.history.post[0].url).toBe(`/tools/ltsa/all?pid=009-212-434`);
    });
  });
  afterEach(() => {
    mockAxios.reset();
    jest.restoreAllMocks();
    cleanup();
    history.push('');
  });
});
