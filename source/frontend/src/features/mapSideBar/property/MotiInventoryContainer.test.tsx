import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';

import { IMapStateMachineContext } from '@/components/common/mapFSM/MapStateMachineContext';
import { Claims } from '@/constants/claims';
import { getMockCrownTenuresLayerResponse } from '@/mocks/crownTenuresLayerResponse.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { getMockLtsaResponse } from '@/mocks/ltsa.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, cleanup, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import MotiInventoryContainer, { IMotiInventoryContainerProps } from './MotiInventoryContainer';
import { mockFAParcelLayerResponse } from '@/mocks/faParcelLayerResponse.mock';

const mockAxios = new MockAdapter(axios);
const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onClose = vi.fn();

describe('MotiInventoryContainer component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & IMotiInventoryContainerProps) => {
    const utils = render(
      <MotiInventoryContainer
        onClose={renderOptions.onClose}
        pid={renderOptions.pid}
        id={renderOptions.id}
        location={renderOptions.location}
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

  beforeEach(() => {
    // BC Assessment api
    mockAxios
      .onGet(new RegExp('https://delivery.apps.gov.bc.ca/ext/sgw/geo.bca*'))
      .reply(200, { features: [{ properties: { FOLIO_ID: 1, ROLL_NUMBER: 1 } }] });

    // Parcel Map layer
    mockAxios
      .onGet(
        new RegExp(
          'https://openmaps.gov.bc.ca/geo/pub/WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_FA_SVW/ows*',
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

    // LTSA api
    mockAxios.onPost(new RegExp('/tools/ltsa/all*')).reply(200, getMockLtsaResponse());

    // Crown land layer
    mockAxios
      .onGet(
        new RegExp('https://openmaps.gov.bc.ca/geo/pub/WHSE_TANTALIS.TA_CROWN_TENURES_SVW/wfs*'),
      )
      .reply(200, getMockCrownTenuresLayerResponse());

    // PIMS properties api
    mockAxios.onGet(new RegExp('/properties/\\d+/historicalNumbers')).reply(200, []);
    mockAxios.onGet(new RegExp('/properties/*')).reply(200, { id: 1, pid: 9212434 });

    // PIMS geoserver api
    mockAxios.onGet(new RegExp('/ogs-internal/*')).reply(200, {});

    // catch all
    mockAxios.onPost().reply(200, {});
  });

  afterEach(() => {
    mockAxios.reset();
    vi.clearAllMocks();
    cleanup();
    history.replace('');
  });

  it('requests LTSA data by pid', async () => {
    const { queryByTestId } = setup({
      id: undefined,
      pid: '9212434',
      onClose,
    });

    await waitFor(() => {
      expect(queryByTestId('filter-backdrop-loading')).toBeNull();
    });

    expect(mockAxios.history.post.length).toBeGreaterThanOrEqual(1);
    expect(mockAxios.history.post.some(x => x.url.includes('/tools/ltsa/all'))).toBe(true);
  });

  it('requests BC Assessment data by pid', async () => {
    const { queryByTestId } = setup({
      id: undefined,
      pid: '9212434',
      onClose,
    });

    await waitFor(() => {
      expect(queryByTestId('filter-backdrop-loading')).toBeNull();
    });

    expect(mockAxios.history.get.length).toBeGreaterThanOrEqual(1);
    expect(
      mockAxios.history.get.some(x =>
        x.url.includes('https://delivery.apps.gov.bc.ca/ext/sgw/geo.bca'),
      ),
    ).toBe(true);
  });

  it('shows the crown tab when property has a TANTALIS record', async () => {
    const testMockMachine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      isSelecting: true,
      selectingComponentId: undefined,
      mapLocationFeatureDataset: {
        location: { lng: -120.69195885, lat: 50.25163372 },
        fileLocation: null,
        pimsFeatures: null,
        parcelFeatures: mockFAParcelLayerResponse.features as any,
        regionFeature: null,
        districtFeature: null,
        municipalityFeatures: null,
        highwayFeatures: null,
        selectingComponentId: null,
        crownLandLeasesFeatures: null,
        crownLandLicensesFeatures: null,
        crownLandTenuresFeatures: null,
        crownLandInventoryFeatures: null,
        crownLandInclusionsFeatures: null,
      },
    };

    const { findByText, queryByTestId } = setup({
      id: undefined,
      location: { lng: -120.69195885, lat: 50.25163372 },
      onClose,
      mockMapMachine: testMockMachine,
    });

    await waitFor(() => {
      expect(queryByTestId('filter-backdrop-loading')).toBeNull();
    });

    expect(await findByText(/Crown Land Tenures/i)).toBeInTheDocument();
    expect(mockAxios.history.get.length).toBeGreaterThanOrEqual(1);
    expect(
      mockAxios.history.get.some(x =>
        x.url.includes('https://openmaps.gov.bc.ca/geo/pub/WHSE_TANTALIS.TA_CROWN_TENURES_SVW'),
      ),
    ).toBe(true);
  });

  it('shows the property information tab for inventory properties', async () => {
    const { findByText, queryByTestId } = setup({
      id: 1,
      onClose,
    });

    await waitFor(() => {
      expect(queryByTestId('filter-backdrop-loading')).toBeNull();
    });

    expect(await findByText(/property attributes/i)).toBeInTheDocument();
    expect(mockAxios.history.get.length).toBeGreaterThanOrEqual(1);
    expect(mockAxios.history.get[0].url).toContain(`/properties/1`);
  });

  it('hides the property information tab for non-inventory properties', async () => {
    history.push('/mapview/sidebar/non-inventory-property/pid/9212434');
    // non-inventory properties will not attempt to contact the backend.
    const error = {
      isAxiosError: true,
      response: { status: 404 },
    };
    mockAxios.onGet(new RegExp('/properties/*')).reply(404, error);

    const { queryByText, getByText, queryAllByTestId } = setup({
      pid: '9212434',
      onClose,
    });

    await waitFor(() => {
      expect(queryAllByTestId('filter-backdrop-loading')).toHaveLength(0);
    });

    expect(queryByText(/property attributes/i)).toBeNull();
    expect(getByText('Title')).toHaveClass('active');
  });

  it('should close the form when Close button is clicked', async () => {
    const { getByTitle, findByText } = setup({
      id: 1,
      onClose,
    });

    await act(async () => {});
    expect(await findByText('Property Information')).toBeVisible();
    const closeButton = getByTitle('close');
    await act(async () => userEvent.click(closeButton));

    expect(onClose).toHaveBeenCalled();
  });
});
