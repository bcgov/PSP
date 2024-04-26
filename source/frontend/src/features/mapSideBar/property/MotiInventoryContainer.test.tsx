import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';

import { Claims } from '@/constants/claims';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { cleanup, render, RenderOptions, waitFor } from '@/utils/test-utils';

import MotiInventoryContainer, { IMotiInventoryContainerProps } from './MotiInventoryContainer';

const onClose = vi.fn();

// Need to mock this library for unit tests
vi.mock('react-visibility-sensor', () => {
  return {
    default: vi.fn().mockImplementation(({ children }) => {
      if (children instanceof Function) {
        return children({ isVisible: true });
      }
      return children;
    }),
  };
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
        id={renderOptions.id}
      />,
      {
        ...renderOptions,
        useMockAuthentication: true,
        history,
        store: { ...renderOptions.store, ...storeState },
        claims: [Claims.PROPERTY_VIEW],
      },
    );

    return { ...utils };
  };

  afterEach(() => {
    mockAxios.reset();
    vi.clearAllMocks();
    cleanup();
    history.replace('');
  });

  it('requests ltsa data by pid', async () => {
    mockAxios.onGet(new RegExp('https://delivery.apps.gov.bc.ca/ext/sgw/geo.bca*')).reply(200, {
      features: [
        {
          properties: { FOLIO_ID: 1, ROLL_NUMBER: 1 },
        },
      ],
    });
    mockAxios
      .onGet(new RegExp('https://delivery.apps.gov.bc.ca/ext/sgw/geo.bca*'))
      .reply(200, { features: [{ properties: { FOLIO_ID: 1, ROLL_NUMBER: 1 } }] });
    mockAxios
      .onGet(
        new RegExp(
          'https://openmaps.gov.bc.ca/geo/pub/WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_SVW/ows*',
        ),
      )
      .reply(200, {
        features: [
          {
            properties: {},
            geometry: {
              type: 'Polygon',
              coordinates: [
                [
                  [-120.69195885, 50.25163372],
                  [-120.69176022, 50.2588544],
                  [-120.69725103, 50.25889407],
                  [-120.70326422, 50.25893724],
                  [-120.70352697, 50.25172245],
                  [-120.70287648, 50.25171749],
                  [-120.70200152, 50.25171082],
                  [-120.69622707, 50.2516666],
                  [-120.69195885, 50.25163372],
                ],
              ],
            },
          },
        ],
      });
    mockAxios.onPost().reply(200, {});
    mockAxios.onGet().reply(200, { pid: 9212434 });
  });

  it('shows the property information tab for inventory properties', async () => {
    mockAxios.onPost().reply(200, {});
    mockAxios.onGet(new RegExp('/properties/*')).reply(200, { id: 9212434 });
    mockAxios.onGet(new RegExp('/ogs-internal/*')).reply(200, {});

    const { findByText, queryByTestId } = setup({
      id: 9212434,
      onClose,
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
    history.push('/mapview/sidebar/non-inventory-property/9212434');
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
    });

    expect(queryByText(/property attributes/i)).toBeNull();
    expect(getByText('Title')).toHaveClass('active');
    await waitFor(() => {
      expect(queryAllByTestId('filter-backdrop-loading')).toHaveLength(0);
    });
  });
});
