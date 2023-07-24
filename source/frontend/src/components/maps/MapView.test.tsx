import userEvent from '@testing-library/user-event';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { FeatureCollection } from 'geojson';
import React from 'react';

import { Claims } from '@/constants/claims';
import { useApiGeocoder } from '@/hooks/pims-api/useApiGeocoder';
import { useApiProperties } from '@/hooks/pims-api/useApiProperties';
import { IProperty } from '@/interfaces';
import { mockParcel } from '@/mocks/filterData.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { cleanup, deferred, render, RenderOptions, waitFor } from '@/utils/test-utils';

import { createPoints } from './leaflet/Layers/util';
import MapView from './MapView';

const mockAxios = new MockAdapter(axios);

jest.mock('@react-keycloak/web');

// This mocks the parcels of land a user can see - should be able to see 2 markers
const mockParcels = [
  { id: 1, latitude: 53.917065, longitude: -122.749672 },
  { id: 2, latitude: 53.917065, longitude: -122.749672 },
] as IProperty[];

jest.mock('@/hooks/pims-api/useApiGeocoder');
jest.mock('@/hooks/pims-api/useApiProperties');

// This will spoof the active parcel (the one that will populate the popup details)
const mockDetails = {
  propertyDetail: {
    ...mockParcel,
    latitude: 48,
    longitude: -123,
  },
};

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
  selectedProperty: IProperty | null;
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

xdescribe('MapProperties View', () => {
  let mockLoadProperties: jest.Mock<Promise<FeatureCollection>>;
  let mockGetParcel: jest.Mock<Promise<IProperty>>;

  beforeEach(() => {
    mockAxios.reset();
    jest.clearAllMocks();
    mockAxios.onGet('/basemaps.json').reply(200, baseMapLayers);
    mockAxios.onAny().reply(200);
    delete (window as any).ResizeObserver;
    window.ResizeObserver = jest.fn().mockImplementation(() => ({
      observe: jest.fn(),
      unobserve: jest.fn(),
      disconnect: jest.fn(),
    }));

    mockLoadProperties = jest.fn(
      async () =>
        ({
          features: createPoints(mockParcels),
          type: 'FeatureCollection',
          bbox: undefined,
        } as FeatureCollection),
    );
    mockGetParcel = jest.fn(async () => ({} as IProperty));

    (useApiGeocoder as unknown as jest.Mock<Partial<typeof useApiGeocoder>>).mockReturnValue({
      loadProperties: mockLoadProperties,
      getProperty: mockGetParcel,
    });

    (useApiProperties as unknown as jest.Mock<Partial<typeof useApiProperties>>).mockReturnValue({
      getProperty: mockGetParcel,
    });
  });

  afterEach(() => {
    window.ResizeObserver = ResizeObserver;
    jest.restoreAllMocks();
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
    userEvent.click(layersControlButton);
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
    userEvent.type(nameInput, '123-456-789');
    await waitFor(() => userEvent.click(searchButton));
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
    await waitFor(() => userEvent.click(searchButton));
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
    userEvent.type(nameInput, '123-456-789');
    await waitFor(() => userEvent.click(searchButton));
    // check API call params...
    const filter = expect.objectContaining({ PID: '123456789' });
    await waitFor(() => expect(mockLoadProperties).toHaveBeenCalledWith(filter));
    const resetButton = findResetButton();
    await waitFor(() => userEvent.click(resetButton));
    const defaultFilter = expect.objectContaining({ PID: undefined });
    await waitFor(() => expect(mockLoadProperties).toHaveBeenCalledWith(defaultFilter));
  });
});
