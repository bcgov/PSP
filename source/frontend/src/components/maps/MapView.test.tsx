import userEvent from '@testing-library/user-event';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { FeatureCollection } from 'geojson';

import { Claims } from '@/constants/claims';
import { useApiGeocoder } from '@/hooks/pims-api/useApiGeocoder';
import { useApiProperties } from '@/hooks/pims-api/useApiProperties';
import { IProperty } from '@/interfaces';
import { mockApiProperty } from '@/mocks/filterData.mock';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, cleanup, deferred, render, RenderOptions, waitFor } from '@/utils/test-utils';

import MapView from './MapView';
import { PointFeature } from './types';

const mockAxios = new MockAdapter(axios);

// This mocks the parcels of land a user can see - should be able to see 2 markers
const mockParcels = [
  { id: 1, latitude: 53.917065, longitude: -122.749672 },
  { id: 2, latitude: 53.917065, longitude: -122.749672 },
] as IProperty[];

vi.mock('@/hooks/pims-api/useApiGeocoder');
vi.mock('@/hooks/pims-api/useApiProperties');

// This will spoof the active parcel (the one that will populate the popup details)
const mockDetails = {
  propertyDetail: {
    ...mockApiProperty,
    latitude: 48,
    longitude: -123,
  },
};

/**
 * Creates map points (in GeoJSON format) for further clustering by `supercluster`
 * @param properties
 */
export const createPoints = (properties: IProperty[], type: string = 'Point') =>
  properties.map(x => {
    return {
      type: 'Feature',
      properties: {
        ...x,
        cluster: false,
        PROPERTY_ID: x.id,
      },
      geometry: {
        type: type,
        coordinates: [x.longitude, x.latitude],
      },
    } as PointFeature;
  });

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: [] },
};

// To check for alert message
const noParcels = [] as IProperty[];

const baseMapLayers = {
  basemaps: [
    {
      name: 'Map',
      url: 'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png',
      attribution: '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors',
      thumbnail: '/streets.jpg',
    },
    {
      name: 'Satellite',
      url: 'https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}',
      attribution:
        'Tiles &copy; Esri &mdash; Source: Esri, DigitalGlobe, GeoEye, Earthstar Geographics, CNES/Airbus DS, USDA, USGS, AeroGRID, IGN, and the GIS User Community',
      thumbnail: '/satellite.jpg',
    },
  ],
};

interface TestProps {
  properties: IProperty[];
  selectedProperty: ApiGen_Concepts_Property | null;
  disableMapFilterBar: boolean;
  zoom?: number;
  done?: () => void;
  setMap?: (map: L.Map) => void;
  renderOptions?: RenderOptions;
}

function createProps(): TestProps {
  return {
    properties: mockParcels,
    selectedProperty: mockDetails.propertyDetail,
    disableMapFilterBar: false,
    renderOptions: {
      useMockAuthentication: true,
      organizations: [0],
      store: storeState,
      claims: [Claims.PROPERTY_EDIT],
    },
  };
}

// component under test
function Template(props: Omit<TestProps, 'renderOptions'>) {
  return <MapView />;
}

function setup(props: Omit<TestProps, 'done'>) {
  const { renderOptions, ...uiProps } = props;
  // create a promise to wait for the map to be ready (which happens after initial render)
  const { promise, resolve } = deferred();
  const uiToRender = <Template {...uiProps} done={resolve} />;
  const component = render(uiToRender, renderOptions);
  return {
    component,
    ready: promise,
    findSlideOutButton: () => document.querySelector('#slideOutInfoButton') as HTMLElement,
    findLayerListButton: () => document.querySelector('#layersControlButton') as HTMLElement,
    findLayerListContainer: () => document.querySelector('#layersContainer') as HTMLElement,
    findFilterBar: () => document.querySelector('.map-filter-bar') as HTMLElement,
    findPidOrPidField: () => document.querySelector('#input-pinOrPid') as HTMLElement,
    findSearchButton: () => document.querySelector('#search-button') as HTMLElement,
    findResetButton: () => document.querySelector('#reset-button') as HTMLElement,
    findZoomInButton: () => document.querySelector('.leaflet-control-zoom-in') as HTMLElement,
    findZoomOutButton: () => document.querySelector('.leaflet-control-zoom-out') as HTMLElement,
    findMapMarker: () =>
      document.querySelector('.leaflet-marker-icon:not(.marker-cluster)') as HTMLElement,
    findMapCluster: () =>
      document.querySelector('.leaflet-marker-icon.marker-cluster') as HTMLElement,
  };
}

describe.skip('MapProperties View', () => {
  let mockLoadProperties;
  let mockGetParcel;

  beforeEach(() => {
    mockAxios.reset();
    vi.clearAllMocks();
    mockAxios.onGet('/basemaps.json').reply(200, baseMapLayers);
    mockAxios.onAny().reply(200);
    delete (window as any).ResizeObserver;
    window.ResizeObserver = vi.fn().mockImplementation(() => ({
      observe: vi.fn(),
      unobserve: vi.fn(),
      disconnect: vi.fn(),
    }));

    mockLoadProperties = vi.fn(
      async () =>
        ({
          features: createPoints(mockParcels),
          type: 'FeatureCollection',
          bbox: undefined,
        } as FeatureCollection),
    );
    mockGetParcel = vi.fn(async () => ({} as IProperty));

    vi.mocked(useApiGeocoder).mockReturnValue({
      loadProperties: mockLoadProperties,
      getProperty: mockGetParcel,
    } as unknown as ReturnType<typeof useApiGeocoder>);

    vi.mocked(useApiProperties).mockReturnValue({
      getProperty: mockGetParcel,
    } as unknown as ReturnType<typeof useApiProperties>);
  });

  afterEach(() => {
    window.ResizeObserver = ResizeObserver;
    vi.restoreAllMocks();
    cleanup();
  });

  it('renders correctly', async () => {
    const props = createProps();
    const { component, ready } = setup({ ...props, disableMapFilterBar: true });
    await waitFor(() => ready);
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('should open the layer list when clicked', async () => {
    const props = createProps();
    const { ready, findLayerListButton, findLayerListContainer } = setup({
      ...props,
      disableMapFilterBar: true,
    });
    await waitFor(() => ready);
    const layersContainer = findLayerListContainer();
    expect(layersContainer).toBeInTheDocument();
    expect(layersContainer.className).toContain('closed');
    // clicking the button should open the layer list...
    const layersControlButton = findLayerListButton();
    await act(async () => userEvent.click(layersControlButton));
    await waitFor(() => expect(layersContainer.className).not.toContain('closed'));
  });

  it('should render the properties as cluster and on selected property', async () => {
    const props = createProps();
    const { ready, findMapCluster } = setup(props);
    await waitFor(() => ready);
    await waitFor(() => {
      const cluster = findMapCluster();
      expect(cluster).toBeVisible();
    });
  });

  it(`renders the filter bar when disableMapFilterBar is set to "false"`, async () => {
    const props = createProps();
    const { ready, findFilterBar } = setup({ ...props, disableMapFilterBar: false });
    await waitFor(() => ready);
    const propertyFilter = findFilterBar();
    expect(propertyFilter).toBeInTheDocument();
    expect(propertyFilter).toBeVisible();
  });

  it(`doesn't render the filter bar when disableMapFilterBar is set to "true"`, async () => {
    const props = createProps();
    const { ready, findFilterBar } = setup({ ...props, disableMapFilterBar: true });
    await waitFor(() => ready);
    const propertyFilter = findFilterBar();
    expect(propertyFilter).toBeNull();
  });

  it(`should render 0 markers when there are no parcels`, async () => {
    mockLoadProperties.mockResolvedValue({
      features: [],
      type: 'FeatureCollection',
      bbox: undefined,
    });
    const props = createProps();
    const { ready, findMapMarker } = setup({
      ...props,
      properties: noParcels,
      selectedProperty: null,
    });
    await waitFor(() => ready);
    const marker = findMapMarker();
    expect(marker).toBeNull();
  });

  it('should call the API to load map data', async () => {
    const props = createProps();
    const { ready } = setup(props);
    await waitFor(() => ready);
    expect(mockLoadProperties).toHaveBeenCalled();
  });

  it('makes the correct calls to load map data when filter is updated', async () => {
    const props = createProps();
    const {
      ready,
      findPidOrPidField: findPidField,
      findSearchButton,
    } = setup({
      ...props,
      properties: noParcels,
      selectedProperty: null,
    });
    await waitFor(() => ready);
    // type something in the filter bar

    const nameInput = findPidField();
    const searchButton = findSearchButton();
    await act(async () => userEvent.type(nameInput, '123-456-789'));
    await act(async () => userEvent.click(searchButton));
    // check API call params...
    const filter = expect.objectContaining({ PID: '123456789' });
    await waitFor(() => expect(mockLoadProperties).toHaveBeenCalledWith(filter));
  });

  it('should fire the filter every time the search button is clicked', async () => {
    const props = createProps();
    const { ready, findSearchButton } = setup({
      ...props,
      properties: noParcels,
      selectedProperty: null,
    });
    await waitFor(() => ready);
    const searchButton = findSearchButton();
    await act(async () => userEvent.click(searchButton));
    expect(mockLoadProperties).toHaveBeenCalled();
  });

  it('makes the correct calls to load the map data when the reset filter is clicked', async () => {
    const props = createProps();
    const {
      ready,
      findPidOrPidField: findPidField,
      findSearchButton,
      findResetButton,
    } = setup({
      ...props,
      properties: noParcels,
      selectedProperty: null,
    });
    await waitFor(() => ready);
    // type something in the filter bar
    const nameInput = findPidField();
    const searchButton = findSearchButton();
    await act(async () => userEvent.type(nameInput, '123-456-789'));
    await act(async () => userEvent.click(searchButton));
    // check API call params...
    const filter = expect.objectContaining({ PID: '123456789' });
    await waitFor(() => expect(mockLoadProperties).toHaveBeenCalledWith(filter));
    const resetButton = findResetButton();
    await act(async () => userEvent.click(resetButton));
    const defaultFilter = expect.objectContaining({ PID: undefined });
    await waitFor(() => expect(mockLoadProperties).toHaveBeenCalledWith(defaultFilter));
  });
});
