import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Claims } from 'constants/claims';
import { createMemoryHistory } from 'history';
import { mockLookups } from 'mocks/mockLookups';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { cleanup, render, RenderOptions, waitFor } from 'utils/test-utils';

import MotiInventoryContainer, { IMotiInventoryContainerProps } from './MotiInventoryContainer';

// mock keycloak auth library
jest.mock('@react-keycloak/web');

const onClose = jest.fn();
const onZoom = jest.fn();

// Need to mock this library for unit tests
jest.mock('react-visibility-sensor', () => {
  return jest.fn().mockImplementation(({ children }) => {
    if (children instanceof Function) {
      return children({ isVisible: true });
    }
    return children;
  });
});

describe('MotiInventoryContainer component', () => {
  const mockAxios = new MockAdapter(axios);
  const history = createMemoryHistory();
  const storeState = {
    [lookupCodesSlice.name]: { lookupCodes: mockLookups },
  };

  const setup = (renderOptions: RenderOptions & IMotiInventoryContainerProps) => {
    // render component under test
    const utils = render(
      <MotiInventoryContainer
        onClose={renderOptions.onClose}
        pid={renderOptions.pid}
        onZoom={renderOptions.onZoom}
        id={renderOptions.id}
      />,
      {
        ...renderOptions,
        history,
        store: { ...renderOptions.store, ...storeState },
        claims: [Claims.PROPERTY_VIEW],
      },
    );

    return { ...utils };
  };

  afterEach(() => {
    mockAxios.reset();
    jest.restoreAllMocks();
    cleanup();
    history.replace('');
  });

  it('requests ltsa data by pid', async () => {
    mockAxios
      .onGet(new RegExp('https://delivery.apps.gov.bc.ca/ext/sgw/geo.bca*'))
      .reply(200, { features: [{ properties: { FOLIO_ID: 1, ROLL_NUMBER: 1 } }] });
    mockAxios.onPost().reply(200, {});
    mockAxios.onGet().reply(200, { pid: 9212434 });

    setup({
      pid: '9212434',
      id: 1,
      onClose,
      onZoom,
    });

    await waitFor(() => {
      expect(mockAxios.history.post).toHaveLength(1);
      expect(mockAxios.history.post[0].url).toBe(`/tools/ltsa/all?pid=009-212-434`);
    });
  });

  it('shows the property information tab for inventory properties', async () => {
    mockAxios.onPost().reply(200, {});
    mockAxios.onGet(new RegExp('/properties/*')).reply(200, { id: 9212434 });
    mockAxios.onGet(new RegExp('/ogs-internal/*')).reply(200, {});

    const { findByText, queryByTestId } = setup({
      id: 9212434,
      onClose,
      onZoom,
    });

    await waitFor(() => {
      expect(mockAxios.history.get.length).toBeGreaterThanOrEqual(1);
      expect(mockAxios.history.get[0].url).toBe(`/properties/9212434`);
    });
    await waitFor(() => {
      expect(queryByTestId('filter-backdrop-loading')).toBeNull();
    });
    expect(await findByText(/property attributes/i)).toBeInTheDocument();
  });

  it('hides the property information tab for non-inventory properties', async () => {
    mockAxios.onPost().reply(200, {});
    // non-inventory properties will not attempt to contact the backend.
    const error = {
      isAxiosError: true,
      response: { status: 404 },
    };
    mockAxios.onGet(new RegExp('/properties/*')).reply(404, error);
    mockAxios.onGet(new RegExp('ogs-internal/*')).reply(200, {});
    mockAxios
      .onGet(new RegExp('https://delivery.apps.gov.bc.ca/ext/sgw/geo.bca*'))
      .reply(200, { features: [{ properties: { FOLIO_ID: 1, ROLL_NUMBER: 1 } }] });

    const { queryByText, getByText, queryAllByTestId } = setup({
      pid: '9212434',
      onClose,
      onZoom,
    });

    expect(queryByText(/property attributes/i)).toBeNull();
    expect(getByText('Title')).toHaveClass('active');
    await waitFor(() => {
      expect(queryAllByTestId('filter-backdrop-loading')).toHaveLength(0);
    });
  });
});
