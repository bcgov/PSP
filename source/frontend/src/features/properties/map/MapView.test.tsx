import { useKeycloak } from '@react-keycloak/web';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { useLayerQuery } from 'components/maps/leaflet/LayerPopup';
import { createPoints } from 'components/maps/leaflet/mapUtils';
import {
  Claims,
  PropertyAreaUnitTypes,
  PropertyClassificationTypes,
  PropertyDataSourceTypes,
  PropertyStatusTypes,
  PropertyTenureTypes,
} from 'constants/index';
import { createMemoryHistory } from 'history';
import { useApiProperties } from 'hooks/pims-api';
import { useComposedProperties } from 'hooks/useComposedProperties';
import { IProperty } from 'interfaces';
import { Api_Property } from 'models/api/Property';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import leafletMouseSlice from 'store/slices/leafletMouse/LeafletMouseSlice';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { act, cleanup, render, RenderOptions, screen, userEvent, waitFor } from 'utils/test-utils';
import { mockKeycloak } from 'utils/test-utils';

import { useMapProperties } from './hooks/useMapProperties';
import MapView from './MapView';

const mockAxios = new MockAdapter(axios);
jest.mock('@react-keycloak/web');
jest.mock('./hooks/useMapProperties');
jest.mock('components/maps/leaflet/LayerPopup');
jest.mock('hooks/useComposedProperties');
jest.mock('hooks/usePropertyAssociations');
jest.mock('hooks/pims-api');
jest.mock('hooks/useLtsa');

const mockStore = configureMockStore([thunk]);

(useComposedProperties as any).mockImplementation(() => ({
  composedLoading: false,
  ltsaWrapper: { execute: jest.fn(), loading: false },
  apiWrapper: { execute: jest.fn(), loading: false },
  propertyAssociationWrapper: { execute: jest.fn(), loading: false },
}));

const largeMockParcels = [
  { id: 1, latitude: 53.917061, longitude: -122.749672 },
  { id: 2, latitude: 53.917062, longitude: -122.749692 },
  { id: 3, latitude: 53.917063, longitude: -122.749682 },
  { id: 4, latitude: 53.917064, longitude: -122.749672 },
  { id: 5, latitude: 53.917065, longitude: -122.749662 },
  { id: 6, latitude: 53.917066, longitude: -122.749652 },
  { id: 7, latitude: 53.917067, longitude: -122.749642 },
  { id: 8, latitude: 53.917068, longitude: -122.749632 },
  { id: 9, latitude: 53.917069, longitude: -122.749622 },
  { id: 10, latitude: 53.917071, longitude: -122.749612 },
  { id: 11, latitude: 53.918172, longitude: -122.749772 },
] as IProperty[];

// This mocks the parcels of land a user can see - render a cluster and a marker
const smallMockParcels = [
  { id: 1, latitude: 54.917061, longitude: -122.749672 },
  { id: 3, latitude: 54.918162, longitude: -122.749772 },
] as IProperty[];

// This mocks the parcels of land a user can see - render a cluster and a marker
const mockParcels = [
  { id: 1, latitude: 55.917161, longitude: -122.749612, pid: '7771' },
  { id: 2, latitude: 55.917262, longitude: -122.749622, pid: '7772' },
  { id: 3, latitude: 55.917363, longitude: -122.749732, pid: '7773' },
] as IProperty[];

const useLayerQueryMock = {
  findOneWhereContains: jest.fn(),
  findMetadataByLocation: jest.fn(),
};
(useLayerQuery as jest.Mock).mockReturnValue(useLayerQueryMock);

// This will spoof the active parcel (the one that will populate the popup details)
const mockDetails = {
  propertyDetail: {
    id: 1,
    pid: '000-000-001',
    pin: '',
    statusId: PropertyStatusTypes.UnderAdmin,
    dataSourceId: PropertyDataSourceTypes.PAIMS,
    dataSourceEffectiveDate: new Date(),
    classificationId: PropertyClassificationTypes.CoreStrategic,
    classification: 'Core Operational',
    tenureId: PropertyTenureTypes.HighwayRoad,
    name: 'test name',
    description: 'test',
    encumbranceReason: '',
    zoning: '',
    zoningPotential: '',
    latitude: 48,
    longitude: 123,
    isSensitive: false,
    evaluations: [
      {
        evaluatedOn: '2019',
        key: 1,
        value: 100000,
      },
    ],
    regionId: 1,
    districtId: 1,
    address: {
      streetAddress1: '1234 mock Street',
      streetAddress2: 'N/A',
      municipality: '',
      provinceId: 1,
      postal: 'V1V1V1',
    },
    areaUnitId: PropertyAreaUnitTypes.Hectare,
    landArea: 0,
    landLegalDescription: 'test',
  },
};

const store = mockStore({
  [lookupCodesSlice.name]: { lookupCodes: [] },
  [leafletMouseSlice.name]: { propertyDetail: mockDetails },
});

let history = createMemoryHistory();

describe('MapView', () => {
  const onMarkerClick = jest.fn();
  const onMarkerPopupClosed = jest.fn();

  const setup = async (renderOptions: RenderOptions = {}) => {
    const utils = render(
      <MapView showParcelBoundaries={true} onMarkerPopupClosed={onMarkerPopupClosed} />,
      {
        store,
        history,
        ...renderOptions,
      },
    );
    await act(async () => {}); // Wait for async mount actions to settle

    return { ...utils };
  };

  beforeEach(() => {
    useLayerQueryMock.findMetadataByLocation.mockResolvedValue({});
    (useKeycloak as jest.Mock).mockReturnValue({
      keycloak: {
        userInfo: {
          roles: ['property-edit', 'property-view'],
          organizations: [0],
        },
      },
    });
    (useMapProperties as unknown as jest.Mock<Partial<typeof useMapProperties>>).mockReturnValue({
      loadProperties: {
        execute: jest.fn().mockResolvedValue({
          features: createPoints(mockParcels),
          type: 'FeatureCollection',
          bbox: undefined,
        }),
      },
    });
    (useApiProperties as unknown as jest.Mock<Partial<typeof useApiProperties>>).mockReturnValue({
      getProperty: async () => {
        return {} as IProperty;
      },
      getPropertyConceptWithId: async () => {
        return {} as Api_Property;
      },
    });
    delete (window as any).ResizeObserver;
    window.ResizeObserver = jest.fn().mockImplementation(() => ({
      observe: jest.fn(),
      unobserve: jest.fn(),
      disconnect: jest.fn(),
    }));
    onMarkerClick.mockClear();
    jest.clearAllMocks();
    mockAxios.reset();
    mockAxios.onGet('/basemaps.json').reply(200, {
      basemaps: [
        {
          name: 'BC Roads',
          urls: [
            'https://maps.gov.bc.ca/arcgis/rest/services/province/roads_wm/MapServer/tile/{z}/{y}/{x}',
          ],
          attribution: '&copy; Government of British Columbia, DataBC, GeoBC',
          thumbnail: '/streets.jpg',
        },
        {
          name: 'Satellite',
          urls: [
            'https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}',
            'https://server.arcgisonline.com/ArcGIS/rest/services/Reference/World_Transportation/MapServer/tile/{z}/{y}/{x}',
          ],
          attribution:
            'Tiles &copy; Esri &mdash; Source: Esri, DigitalGlobe, GeoEye, Earthstar Geographics, CNES/Airbus DS, USDA, USGS, AeroGRID, IGN, and the GIS User Community',
          thumbnail: '/satellite.jpg',
        },
      ],
    });
    mockAxios.onAny().reply(200, {});
    history = createMemoryHistory();
    history.push('/mapview');
  });

  afterEach(cleanup);

  it('Renders the map', async () => {
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
    expect(document.querySelector('.leaflet-container')).toBeVisible();
  });

  it('Can toggle the base map', async () => {
    await setup();
    // find basemap toggle button
    const basemapToggle = await screen.findByAltText(/Map Thumbnail/i);
    expect(basemapToggle).toBeInTheDocument();
    expect(basemapToggle).toHaveAttribute('src', '/satellite.jpg');
    // click it
    await act(() => userEvent.click(basemapToggle));
    // expect image to change
    expect(basemapToggle).toHaveAttribute('src', '/streets.jpg');
  });

  it('Renders markers when provided', async () => {
    await setup();
    expect(document.querySelector('.leaflet-marker-icon')).toBeVisible();
  });

  it('the map can zoom in until no clusters are visible', async () => {
    (useMapProperties as unknown as jest.Mock<Partial<typeof useMapProperties>>).mockReturnValue({
      loadProperties: {
        execute: jest.fn().mockResolvedValue({
          features: createPoints(smallMockParcels),
          type: 'FeatureCollection',
          bbox: undefined,
        }),
      },
    });
    (useApiProperties as unknown as jest.Mock<Partial<typeof useApiProperties>>).mockReturnValue({
      getParcel: async () => {
        return {} as IProperty;
      },
    });

    const { container } = await setup();

    // click the zoom-in button 10 times
    const zoomIn = container.querySelector('.leaflet-control-zoom-in');
    for (let i = 1; i <= 10; i++) {
      await act(() => userEvent.click(zoomIn!));
    }

    const cluster = container.querySelector('.leaflet-marker-icon.marker-cluster');
    expect(cluster).toBeNull();
  });

  it('the map can handle features with invalid geometry', async () => {
    (useMapProperties as unknown as jest.Mock<Partial<typeof useMapProperties>>).mockReturnValue({
      loadProperties: {
        execute: jest.fn().mockResolvedValue({
          features: createPoints(smallMockParcels).map(feature => ({
            ...feature,
            geometry: null,
          })),
          type: 'FeatureCollection',
          bbox: undefined,
        }),
      },
    });
    (useApiProperties as unknown as jest.Mock<Partial<typeof useApiProperties>>).mockReturnValue({
      getParcel: async () => {
        return {} as IProperty;
      },
    });

    const { findByText } = await setup();
    expect(await findByText('No search results found')).toBeVisible();
  });

  it('the map can zoom out until the markers are clustered', async () => {
    const { container } = await setup();

    // click the zoom-out button 10 times
    const zoomOut = container.querySelector('.leaflet-control-zoom-out');
    for (let i = 1; i <= 10; i++) {
      await act(() => userEvent.click(zoomOut!));
    }

    const cluster = container.querySelector('.leaflet-marker-icon.marker-cluster');
    expect(cluster).toBeVisible();
  });

  it('clusters can be clicked to zoom and spiderfy', async () => {
    const { container } = await setup();

    // click the zoom-out button 10 times
    const zoomOut = container.querySelector('.leaflet-control-zoom-out');
    for (let i = 1; i <= 10; i++) {
      await act(() => userEvent.click(zoomOut!));
    }

    const cluster = container.querySelector('.leaflet-marker-icon.marker-cluster');
    expect(cluster).toBeVisible();
    await act(() => userEvent.click(cluster!));

    const polyline = container.querySelector('.leaflet-pane.leaflet-overlay-pane svg g');
    expect(polyline).toBeVisible();
  });

  it('the map can be clicked', async () => {
    useLayerQueryMock.findOneWhereContains.mockResolvedValue({
      features: [
        {
          type: 'Feature',
          geometry: { type: 'Point', coordinates: [-1.133005, 52.629835] },
          properties: {},
        },
      ],
    });

    const { container } = await setup();

    const map = container.querySelector('.leaflet-container');
    expect(map).toBeVisible();
    await act(() => userEvent.click(map!));

    expect(useLayerQueryMock.findOneWhereContains).toHaveBeenLastCalledWith({
      lat: 52.81604319154934,
      lng: -124.67285156250001,
    });
  });

  it('clusters can be clicked to zoom and spiderfy large clusters', async () => {
    (useApiProperties as unknown as jest.Mock<Partial<typeof useApiProperties>>).mockReturnValue({
      getProperty: async () => {
        return {} as IProperty;
      },
    });
    (useMapProperties as unknown as jest.Mock<Partial<typeof useMapProperties>>).mockReturnValue({
      loadProperties: {
        execute: jest.fn().mockResolvedValue({
          features: createPoints(largeMockParcels),
          type: 'FeatureCollection',
          bbox: undefined,
        }),
      },
    });

    const { container } = await setup();

    const cluster = container.querySelector('.leaflet-marker-icon.marker-cluster');
    expect(cluster).toBeVisible();
    await waitFor(() => {
      act(() => userEvent.click(cluster!));
      const polyline = container.querySelector('.leaflet-pane.leaflet-overlay-pane svg g');
      expect(polyline).toBeVisible();
    });
  });

  it('Rendered markers can be clicked normally', async () => {
    await setup();
    // click on clustered markers to expand into single markers
    const cluster = document.querySelector('.leaflet-marker-icon.marker-cluster');
    await act(() => userEvent.click(cluster!));
    // click on single marker
    const marker = document.querySelector('img.leaflet-marker-icon');
    await act(() => userEvent.click(marker!));
    // verify property information slide-out is shown
    const text = await screen.findByText('Property Information');
    expect(text).toBeVisible();
  });

  it('Rendered markers can be clicked and displayed with permissions', async () => {
    mockKeycloak({ claims: [Claims.ADMIN_PROPERTIES] });
    await setup();
    // click on clustered markers to expand into single markers
    const cluster = document.querySelector('.leaflet-marker-icon.marker-cluster');
    await act(() => userEvent.click(cluster!));
    // click on single marker
    const marker = document.querySelector('img.leaflet-marker-icon');
    await act(() => userEvent.click(marker!));
    // verify property information slide-out is shown
    const text = await screen.findByText('Property Information');
    expect(text).toBeVisible();
  });

  it('Rendered markers can be clicked which hides the filter', async () => {
    await setup();
    // click on clustered markers to expand into single markers
    const cluster = document.querySelector('.leaflet-marker-icon.marker-cluster');
    await act(() => userEvent.click(cluster!));
    // click on single marker
    const marker = document.querySelector('img.leaflet-marker-icon');
    await act(() => userEvent.click(marker!));
    // verify property information slide-out is shown
    const text = await screen.findByText('Property Information');
    expect(text).toBeVisible();
    // when showing the property information slide-out, the map filter bar is hidden
    const label = await screen.queryByText('Search:');
    expect(label).toBeNull();
  });
});
