import { useKeycloak } from '@react-keycloak/web';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Feature, FeatureCollection, Point } from 'geojson';
import { createMemoryHistory } from 'history';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import {
  IMapStateMachineContext,
  useMapStateMachine,
} from '@/components/common/mapFSM/MapStateMachineContext';
import {
  FeatureSelected,
  emptyPimsBoundaryFeatureCollection,
  emptyPimsLocationFeatureCollection,
  emptyPmbcFeatureCollection,
} from '@/components/common/mapFSM/models';
import {
  Claims,
  PropertyAreaUnitTypes,
  PropertyClassificationTypes,
  PropertyDataSourceTypes,
  PropertyStatusTypes,
  PropertyTenureTypes,
} from '@/constants/index';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import {
  EmptyPropertyLocation,
  PIMS_Property_Location_View,
} from '@/models/layers/pimsPropertyLocationView';
import leafletMouseSlice from '@/store/slices/leafletMouse/LeafletMouseSlice';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  RenderOptions,
  act,
  cleanup,
  mockKeycloak,
  render,
  screen,
  userEvent,
  waitFor,
} from '@/utils/test-utils';

import { useApiProperties } from '@/hooks/pims-api/useApiProperties';
import { ApiGen_Base_Page } from '@/models/api/generated/ApiGen_Base_Page';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import MapContainer from './MapContainer';

const mockAxios = new MockAdapter(axios);

vi.mock('@/components/maps/leaflet/LayerPopup/components/LayerPopupContent');
vi.mock('@/features/advancedFilterBar/AdvancedFilterBar');
vi.mock('@/hooks/pims-api/useApiProperties');
vi.mock('@/hooks/useLtsa');
vi.mock('@/hooks/repositories/useComposedProperties');
vi.mock('@/hooks/repositories/usePropertyAssociations');
vi.mock('@/hooks/repositories/mapLayer/useParcelMapLayer');
vi.mock('@/hooks/repositories/mapLayer/useAdminBoundaryMapLayer');
vi.mock('@/hooks/repositories/mapLayer/usePimsPropertyLayer');
vi.mock('@/hooks/repositories/mapLayer/useLegalAdminBoundariesMapLayer');
vi.mock('@/hooks/repositories/mapLayer/useIndianReserveBandMapLayer');

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

vi.mocked(useApiProperties).mockReturnValue({
  getPropertiesViewPagedApi: vi
    .fn()
    .mockResolvedValue({ data: {} as ApiGen_Base_Page<ApiGen_Concepts_Property> }),
  getMatchingPropertiesApi: vi.fn(),
  getPropertyAssociationsApi: vi.fn(),
  exportPropertiesApi: vi.fn(),
  getPropertiesApi: vi.fn(),
  getPropertyConceptWithIdApi: vi.fn(),
  getPropertyConceptWithPidApi: vi.fn(),
  putPropertyConceptApi: vi.fn(),
  getPropertyConceptWithPinApi: vi.fn(),
});

const mockStore = configureMockStore([thunk]);

interface ParcelSeed {
  id: number;
  latitude: number;
  longitude: number;
  propertyId?: number;
  pid?: number;
}

export const largeMockParcels: ParcelSeed[] = [
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
];

export const distantMockParcels: ParcelSeed[] = [
  { id: 1, latitude: 53.917061, longitude: -122.749672, propertyId: 1 },
  { id: 2, latitude: 54.917062, longitude: -123.749692, propertyId: 2 },
  { id: 3, latitude: 55.917063, longitude: -124.749682, propertyId: 3 },
  { id: 4, latitude: 56.917064, longitude: -125.749672, propertyId: 4 },
];

export const createPimsFeatures = (
  parcelSeed: ParcelSeed[],
): FeatureCollection<Point, PIMS_Property_Location_View> => {
  return {
    type: 'FeatureCollection',
    features: parcelSeed.map<Feature<Point, PIMS_Property_Location_View>>(x => {
      return {
        type: 'Feature',
        id: `PIMS_PROPERTY_LOCATION_VW.fid--${x.id}`,
        geometry: { type: 'Point', coordinates: [x.longitude, x.latitude] },
        properties: {
          ...EmptyPropertyLocation,
          PROPERTY_ID: x.propertyId ?? null,
          PID: x.pid ?? null,
          IS_OWNED: true,
        },
      };
    }),
  };
};

// This mocks the parcels of land a user can see - render a cluster and a marker
const smallMockParcels: ParcelSeed[] = [
  { id: 1, latitude: 54.917061, longitude: -122.749672 },
  { id: 3, latitude: 54.918162, longitude: -122.749772 },
];

// This mocks the parcels of land a user can see - render a cluster and a marker
const mockParcels: ParcelSeed[] = [
  { id: 1, latitude: 55.917161, longitude: -122.749612, pid: 7771 },
  { id: 2, latitude: 55.917262, longitude: -122.749622, pid: 7772 },
  { id: 3, latitude: 55.917363, longitude: -122.749732, pid: 7773 },
];

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

describe('MapContainer', () => {
  const setup = async (renderOptions: RenderOptions = {}) => {
    const utils = render(
      <>
        <MapContainer />
      </>,
      {
        store,
        history,
        mockMapMachine: {
          ...mapMachineBaseMock,
          mapFeatureData: {
            pimsLocationFeatures: createPimsFeatures(mockParcels),
            pimsBoundaryFeatures: emptyPimsBoundaryFeatureCollection,
            pmbcFeatures: emptyPmbcFeatureCollection,
          },
        },
        ...renderOptions,
        useMockAuthentication: true,
      },
    );
    await act(async () => {}); // Wait for async mount actions to settle

    return { ...utils };
  };

  beforeEach(() => {
    delete (window as any).ResizeObserver;
    window.ResizeObserver = vi.fn().mockImplementation(() => ({
      observe: vi.fn(),
      unobserve: vi.fn(),
      disconnect: vi.fn(),
    }));
    vi.clearAllMocks();
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
    await act(async () => userEvent.click(basemapToggle));
    // expect image to change
    expect(basemapToggle).toHaveAttribute('src', '/streets.jpg');
  });

  it('Renders markers when provided', async () => {
    await setup();
    expect(document.querySelector('.leaflet-marker-icon')).toBeVisible();
  });

  it('the map can zoom in until no clusters are visible', async () => {
    const { container } = await setup({
      mockMapMachine: {
        ...mapMachineBaseMock,
        mapFeatureData: {
          pimsLocationFeatures: createPimsFeatures(smallMockParcels),
          pimsBoundaryFeatures: emptyPimsBoundaryFeatureCollection,
          pmbcFeatures: emptyPmbcFeatureCollection,
        },
      },
    });

    // click the zoom-in button 10 times
    const zoomIn = container.querySelector('.leaflet-control-zoom-in');
    for (let i = 1; i <= 10; i++) {
      await act(async () => userEvent.click(zoomIn!));
    }

    const cluster = container.querySelector('.leaflet-marker-icon.marker-cluster');
    expect(cluster).toBeNull();
  });

  it('the map can handle features with invalid geometry', async () => {
    const { container } = await setup({
      mockMapMachine: {
        ...mapMachineBaseMock,
        mapFeatureData: {
          pimsLocationFeatures: emptyPimsLocationFeatureCollection,
          pimsBoundaryFeatures: emptyPimsBoundaryFeatureCollection,
          pmbcFeatures: emptyPmbcFeatureCollection,
        },
      },
    });
    const map = container.querySelector('.leaflet-container');
    expect(map).toBeVisible();
  });

  it('the map can zoom out until the markers are clustered', async () => {
    const { container } = await setup();

    // click the zoom-out button 10 times
    const zoomOut = container.querySelector('.leaflet-control-zoom-out');
    for (let i = 1; i <= 10; i++) {
      await act(async () => userEvent.click(zoomOut!));
    }

    const cluster = container.querySelector('.leaflet-marker-icon.marker-cluster');
    expect(cluster).toBeVisible();
  });

  it('clusters can be clicked to zoom and spiderfy', async () => {
    const { container } = await setup();

    const cluster = container.querySelector('.leaflet-marker-icon.marker-cluster');
    expect(cluster).toBeVisible();

    //continue clicking the cluster until it is fully expanded.
    while (container.querySelector('.leaflet-marker-icon.marker-cluster') != null) {
      await act(async () => userEvent.click(cluster!));
    }
  });

  it('the map can be clicked', async () => {
    const { container } = await setup();

    const map = container.querySelector('.leaflet-container');
    expect(map).toBeVisible();
    await act(async () => userEvent.click(map!));

    expect(mapMachineBaseMock.mapClick).toHaveBeenLastCalledWith({
      lat: 52.81604319154934,
      lng: -124.67285156250001,
    });
  });

  it('clusters can be clicked to zoom and spiderfy large clusters', async () => {
    const { container } = await setup({
      mockMapMachine: {
        ...mapMachineBaseMock,
        mapFeatureData: {
          pimsLocationFeatures: createPimsFeatures(largeMockParcels),
          pimsBoundaryFeatures: emptyPimsBoundaryFeatureCollection,
          pmbcFeatures: emptyPmbcFeatureCollection,
        },
      },
    });

    const cluster = container.querySelector('.leaflet-marker-icon.marker-cluster');
    expect(cluster).toBeVisible();
    await waitFor(async () => {
      await act(async () => userEvent.click(cluster!));
      const polyline = container.querySelector('.leaflet-pane.leaflet-overlay-pane svg g');
      expect(polyline).toBeVisible();
    });
  });

  it('Rendered markers can be clicked normally', async () => {
    const pimsFeatures = createPimsFeatures(mockParcels);
    const feature = pimsFeatures.features[2];
    const [longitude, latitude] = feature.geometry.coordinates;

    const expectedFeature: FeatureSelected = {
      clusterId: feature.id?.toString() || '',
      pimsLocationFeature: feature.properties,
      pimsBoundaryFeature: null,
      pmbcFeature: null,
      latlng: { lng: longitude, lat: latitude },
    };
    const testMapMock: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      mapFeatureData: {
        pimsLocationFeatures: pimsFeatures,
        pimsBoundaryFeatures: emptyPimsBoundaryFeatureCollection,
        pmbcFeatures: emptyPmbcFeatureCollection,
      },
    };

    await setup({ mockMapMachine: testMapMock });

    // click on clustered markers to expand into single markers
    const cluster = document.querySelector('.leaflet-marker-icon.marker-cluster');
    await act(async () => userEvent.click(cluster!));
    // click on single marker
    const marker = document.querySelector('img.leaflet-marker-icon');
    await act(async () => userEvent.click(marker!));
    // verify the correct feature got clicked
    expect(testMapMock.mapMarkerClick).toHaveBeenCalledWith(expectedFeature);
  });

  it('Rendered markers can be clicked and displayed with permissions', async () => {
    mockKeycloak({ claims: [Claims.ADMIN_PROPERTIES] });

    const pimsFeatures = createPimsFeatures(mockParcels);
    const feature = pimsFeatures.features[2];
    const [longitude, latitude] = feature.geometry.coordinates;

    const expectedFeature: FeatureSelected = {
      clusterId: feature.id?.toString() || '',
      pimsLocationFeature: feature.properties,
      pimsBoundaryFeature: null,
      pmbcFeature: null,
      latlng: { lng: longitude, lat: latitude },
    };
    const testMapMock: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      mapFeatureData: {
        pimsLocationFeatures: pimsFeatures,
        pimsBoundaryFeatures: emptyPimsBoundaryFeatureCollection,
        pmbcFeatures: emptyPmbcFeatureCollection,
      },
    };

    await setup({ mockMapMachine: testMapMock });
    // click on clustered markers to expand into single markers
    const cluster = document.querySelector('.leaflet-marker-icon.marker-cluster');
    await act(async () => userEvent.click(cluster!));
    // click on single marker
    const marker = document.querySelector('img.leaflet-marker-icon');
    await act(async () => userEvent.click(marker!));
    // verify the correct feature got clicked
    expect(testMapMock.mapMarkerClick).toHaveBeenCalledWith(expectedFeature);
  });

  it.skip('Rendered markers can be clicked which hides the filter', async () => {
    await setup();
    // click on clustered markers to expand into single markers
    const cluster = document.querySelector('.leaflet-marker-icon.marker-cluster');
    await act(async () => userEvent.click(cluster!));
    // click on single marker
    const marker = document.querySelector('img.leaflet-marker-icon');
    await act(async () => userEvent.click(marker!));
    // verify property information slide-out is shown
    const text = await screen.findByText('Property Information');
    expect(text).toBeVisible();
    // when showing the property information slide-out, the map filter bar is hidden
    const label = await screen.queryByText('Search:');
    expect(label).toBeNull();
  });

  it('Rendered distant markers', async () => {
    mockKeycloak({ claims: [Claims.ADMIN_PROPERTIES] });

    const pimsFeatures = createPimsFeatures(distantMockParcels);

    const testMapMock: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      mapFeatureData: {
        pimsLocationFeatures: pimsFeatures,
        pimsBoundaryFeatures: emptyPimsBoundaryFeatureCollection,
        pmbcFeatures: emptyPmbcFeatureCollection,
      },
      isFiltering: false,
    };

    await setup({ mockMapMachine: testMapMock });

    const clusterIcons = document.querySelectorAll('.leaflet-marker-icon.marker-cluster');
    const markerIcons = document.querySelectorAll('img.leaflet-marker-icon');
    expect(clusterIcons.length).toBe(0);
    expect(markerIcons.length).toBe(pimsFeatures.features.length);
  });

  it('Markers can be filtered', async () => {
    mockKeycloak({ claims: [Claims.ADMIN_PROPERTIES] });

    const pimsFeatures = createPimsFeatures(distantMockParcels);
    const feature = pimsFeatures.features[0];
    const [longitude, latitude] = feature.geometry.coordinates;

    const expectedFeature: FeatureSelected = {
      clusterId: feature.id?.toString() || '',
      pimsLocationFeature: feature.properties,
      pimsBoundaryFeature: null,
      pmbcFeature: null,
      latlng: { lng: longitude, lat: latitude },
    };

    const activeIds = [Number(pimsFeatures.features[0].properties.PROPERTY_ID)];
    const testMapMock: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      mapFeatureData: {
        pimsLocationFeatures: pimsFeatures,
        pimsBoundaryFeatures: emptyPimsBoundaryFeatureCollection,
        pmbcFeatures: emptyPmbcFeatureCollection,
      },
      activePimsPropertyIds: activeIds,
      isFiltering: true,
    };

    await setup({ mockMapMachine: testMapMock });

    const clusterIcons = document.querySelectorAll('.leaflet-marker-icon.marker-cluster');
    const markerIcons = document.querySelectorAll('img.leaflet-marker-icon');

    // verify the markers have been filtered
    expect(clusterIcons.length).toBe(0);
    expect(markerIcons.length).toBe(activeIds.length);

    // click on single marker
    const marker = document.querySelector('img.leaflet-marker-icon');
    await act(async () => userEvent.click(marker!));

    // verify the correct feature got clicked
    expect(testMapMock.mapMarkerClick).toHaveBeenCalledWith(expectedFeature);
  });
});
