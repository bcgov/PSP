import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';
import { cleanup, render, RenderOptions, waitFor } from 'utils/test-utils';

import MapSideBarContainer from './MapSideBarContainer';

// Need to mock this library for unit tests
jest.mock('react-visibility-sensor', () => {
  return jest.fn().mockImplementation(({ children }) => {
    if (children instanceof Function) {
      return children({ isVisible: true });
    }
    return children;
  });
});

describe('MapSideBarContainer component', () => {
  const mockAxios = new MockAdapter(axios);
  const history = createMemoryHistory();

  // render component under test
  const setup = (renderOptions: RenderOptions = {}) => {
    const renderResult = render(<MapSideBarContainer />, { ...renderOptions, history });
    return { ...renderResult };
  };

  beforeEach(() => {
    mockAxios.reset();
    jest.restoreAllMocks();
    cleanup();
    history.replace('');
  });

  it('requests ltsa data by pid', async () => {
    history.replace('mapview?pid=9212434&searchBy=pinOrPid&sidebar=true');
    mockAxios.onPost().reply(200, {});
    mockAxios.onGet().reply(200, {});
    setup({});
    await waitFor(() => {
      expect(mockAxios.history.post).toHaveLength(1);
      expect(mockAxios.history.post[0].url).toBe(`/tools/ltsa/all?pid=009-212-434`);
    });
  });

  it('shows the property information tab for inventory properties', async () => {
    history.replace('mapview?pid=9212434&searchBy=pinOrPid&sidebar=true');
    mockAxios.onPost().reply(200, {});
    mockAxios.onGet(new RegExp('properties/*')).reply(200, {});
    mockAxios.onGet(new RegExp('ogs-internal/*')).reply(200, {});
    const { findByText } = setup({});
    await waitFor(() => {
      expect(mockAxios.history.get.length).toBeGreaterThanOrEqual(1);
      expect(mockAxios.history.get[0].url).toBe(`/properties/009-212-434`);
    });
    expect(await findByText('Property attributes')).toBeInTheDocument();
  });

  it('hides the property information tab for non-inventory properties', async () => {
    history.replace('mapview?pid=9212434&searchBy=pinOrPid&sidebar=true');
    mockAxios.onPost().reply(200, {});
    // non-inventory properties return a "not-found" error from API
    const error = {
      isAxiosError: true,
      response: { status: 404 },
    };
    mockAxios.onGet(new RegExp('/properties/*')).reply(404, error);
    mockAxios.onGet(new RegExp('ogs-internal/*')).reply(200, {});
    const { queryByText } = setup({});
    await waitFor(() => {
      expect(mockAxios.history.get.length).toBeGreaterThanOrEqual(1);
      expect(mockAxios.history.get[0].url).toBe(`/properties/009-212-434`);
    });
    expect(queryByText('Property attributes')).toBeNull();
  });
});
