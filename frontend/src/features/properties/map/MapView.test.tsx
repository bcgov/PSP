import { useKeycloak } from '@react-keycloak/web';
import { act, cleanup, fireEvent, render, screen, wait } from '@testing-library/react';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { useLayerQuery } from 'components/maps/leaflet/LayerPopup';
import { createPoints } from 'components/maps/leaflet/mapUtils';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';
import { usePropertyNames } from 'features/properties/common/slices/usePropertyNames';
import { createMemoryHistory } from 'history';
import { PimsAPI, useApi } from 'hooks/useApi';
import { IParcel, IProperty } from 'interfaces';
import { noop } from 'lodash';
import React from 'react';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import leafletMouseSlice from 'store/slices/leafletMouse/LeafletMouseSlice';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { IPropertyDetail, propertiesSlice, useProperties } from 'store/slices/properties';
import { TenantProvider } from 'tenants';
import TestCommonWrapper from 'utils/TestCommonWrapper';

import MapView from './MapView';

const mockAxios = new MockAdapter(axios);
jest.mock('@react-keycloak/web');
Enzyme.configure({ adapter: new Adapter() });
const mockStore = configureMockStore([thunk]);
jest.mock('hooks/useApi');
jest.mock('components/maps/leaflet/LayerPopup');
jest.mock('features/properties/common/slices/usePropertyNames');
jest.mock('store/slices/properties/useProperties');

const fetchPropertyNames = jest.fn(() => () => Promise.resolve(['test']));
(usePropertyNames as any).mockImplementation(() => ({
  fetchPropertyNames,
}));

(useProperties as any).mockImplementation(() => ({
  deleteParcel: jest.fn(),
  deleteBuilding: jest.fn(),
  updateParcel: jest.fn(),
  createParcel: jest.fn(),
  fetchPropertyDetail: jest.fn(),
  fetchBuildingDetail: jest.fn(),
  fetchParcelDetail: jest.fn(),
  fetchParcelsDetail: jest.fn(),
  fetchParcels: jest.fn(),
}));

const largeMockParcels = [
  { id: 1, latitude: 53.917065, longitude: -122.749672, propertyTypeId: 1 },
  { id: 2, latitude: 53.917065, longitude: -122.749672, propertyTypeId: 1 },
  { id: 3, latitude: 53.917065, longitude: -122.749672, propertyTypeId: 1 },
  { id: 4, latitude: 53.917065, longitude: -122.749672, propertyTypeId: 1 },
  { id: 5, latitude: 53.917065, longitude: -122.749672, propertyTypeId: 1 },
  { id: 6, latitude: 53.917065, longitude: -122.749672, propertyTypeId: 1 },
  { id: 7, latitude: 53.917065, longitude: -122.749672, propertyTypeId: 1 },
  { id: 8, latitude: 53.917065, longitude: -122.749672, propertyTypeId: 1 },
  { id: 9, latitude: 53.917065, longitude: -122.749672, propertyTypeId: 1 },
  { id: 10, latitude: 53.917065, longitude: -122.749672, propertyTypeId: 1 },
  { id: 11, latitude: 53.918165, longitude: -122.749772, propertyTypeId: 0 },
] as IProperty[];

// This mocks the parcels of land a user can see - render a cluster and a marker
const smallMockParcels = [
  { id: 1, latitude: 53.917065, longitude: -122.749672, propertyTypeId: 1 },
  { id: 3, latitude: 53.918165, longitude: -122.749772, propertyTypeId: 0 },
] as IProperty[];

// This mocks the parcels of land a user can see - render a cluster and a marker
const mockParcels = [
  { id: 1, latitude: 53.917065, longitude: -122.749672, propertyTypeId: 1 },
  { id: 2, latitude: 53.917065, longitude: -122.749672, propertyTypeId: 1 },
  { id: 3, latitude: 53.918165, longitude: -122.749772, propertyTypeId: 0 },
] as IProperty[];

let findOneWhereContains = jest.fn();

(useLayerQuery as jest.Mock).mockReturnValue({
  findOneWhereContains: findOneWhereContains,
});

// This will spoof the active parcel (the one that will populate the popup details)
const mockDetails: IPropertyDetail = {
  propertyTypeId: 0,
  parcelDetail: {
    id: 1,
    name: 'test name',
    pid: '000-000-000',
    pin: '',
    encumbranceReason: '',
    assessedBuilding: 0,
    assessedLand: 0,
    projectNumbers: [],
    classificationId: 0,
    zoning: '',
    zoningPotential: '',
    agencyId: 0,
    latitude: 48,
    longitude: 123,
    classification: 'Core Operational',
    description: 'test',
    isSensitive: false,
    parcels: [],
    evaluations: [
      {
        date: '2019',
        key: '',
        value: 100000,
      },
    ],
    fiscals: [],
    address: {
      line1: '1234 mock Street',
      line2: 'N/A',
      administrativeArea: '',
      provinceId: 'BC',
      postal: 'V1V1V1',
    },
    landArea: '',
    landLegalDescription: 'test',
    buildings: [],
    agency: 'FIN',
  },
};

const store = mockStore({
  [lookupCodesSlice.name]: { lookupCodes: [] },
  [propertiesSlice.name]: { parcelDetail: mockDetails, draftParcels: [], parcels: mockParcels },
  [leafletMouseSlice.name]: { parcelDetail: mockDetails },
});

let history = createMemoryHistory();
describe('MapView', () => {
  const onMarkerClick = jest.fn();
  (useKeycloak as jest.Mock).mockReturnValue({
    keycloak: {
      userInfo: {
        agencies: [0],
      },
    },
  });
  beforeEach(() => {
    ((useApi as unknown) as jest.Mock<Partial<PimsAPI>>).mockReturnValue({
      loadProperties: jest.fn(async () => {
        return createPoints(mockParcels);
      }),
      getParcel: async () => {
        return {} as IParcel;
      },
    });
    delete (window as any).ResizeObserver;
    window.ResizeObserver = jest.fn().mockImplementation(() => ({
      observe: jest.fn(),
      unobserve: jest.fn(),
      disconnect: jest.fn(),
    }));
    onMarkerClick.mockClear();
    mockAxios.reset();
    jest.clearAllMocks();
    mockAxios.onAny().reply(200);
    cleanup();
    history = createMemoryHistory();
  });

  const getMap = () => {
    process.env.REACT_APP_TENANT = 'MOTI';
    return (
      <TestCommonWrapper store={store} history={history}>
        <MapView
          disableMapFilterBar={false}
          disabled={false}
          showParcelBoundaries={true}
          onMarkerPopupClosed={noop}
        />
      </TestCommonWrapper>
    );
  };

  it('Renders the map', async () => {
    await wait(() => {
      const { container } = render(getMap());
      expect(container.firstChild).toMatchSnapshot();
      expect(container.querySelector('.leaflet-container')).toBeVisible();
    });
  });

  it('Renders markers when provided', async () => {
    let map: any;
    await act(async () => {
      map = render(getMap());
    });
    expect(map.container.querySelector('.leaflet-marker-icon')).toBeVisible();
  });
  it('Rendered markers can be clicked', async () => {
    let map: any;
    await act(async () => {
      map = render(getMap());
    });
    const cluster = map.container.querySelector('.leaflet-marker-icon');
    fireEvent.click(cluster!);
    const marker = map.container.querySelector('img.leaflet-marker-icon');
    fireEvent.click(marker!);
    const text = await screen.findByText('Property Info');
    expect(text).toBeVisible();
  });

  it('the map can zoom in until no clusters are visible', async () => {
    ((useApi as unknown) as jest.Mock<Partial<PimsAPI>>).mockReturnValue({
      loadProperties: jest.fn(async () => {
        return createPoints(smallMockParcels);
      }),
      getParcel: async () => {
        return {} as IParcel;
      },
    });

    const { container } = render(getMap());

    await wait(() => {
      const icon = container.querySelector('.leaflet-control-zoom-in');
      fireEvent.click(icon!);
      const cluster = container.querySelector('.leaflet-marker-icon.marker-cluster');
      expect(cluster).toBeNull();
    });
  });

  it('the map can zoom out until the markers are clustered', async () => {
    const { container } = render(getMap());
    await wait(() => {
      const icon = container.querySelector('.leaflet-control-zoom-out');
      fireEvent.click(icon!);
      const cluster = container.querySelector('.leaflet-marker-icon.marker-cluster');
      expect(cluster).toBeVisible();
    });
  });

  it('clusters can be clicked to zoom and spiderfy', async () => {
    const { container } = render(getMap());
    await wait(() => {
      const icon = container.querySelector('.leaflet-control-zoom-out');
      fireEvent.click(icon!);
      const cluster = container.querySelector('.leaflet-marker-icon.marker-cluster');
      expect(cluster).toBeVisible();
      fireEvent.click(cluster!);
    });
    await wait(() => {
      const cluster = container.querySelector('.leaflet-marker-icon.marker-cluster');
      expect(cluster).toBeVisible();
      fireEvent.click(cluster!);
      const polyline = container.querySelector('.leaflet-pane.leaflet-overlay-pane svg g');
      expect(polyline).toBeVisible();
    });
  });

  it('the map can be clicked', async () => {
    await wait(() => {
      findOneWhereContains.mockResolvedValue({
        features: [
          { type: 'Feature', geometry: { type: 'Point', coordinates: [-1.133005, 52.629835] } },
        ],
      });
      const { container } = render(getMap());
      const map = container.querySelector('.leaflet-container');
      expect(map).toBeVisible();
      fireEvent.click(map!);
    });
    expect(findOneWhereContains).toHaveBeenLastCalledWith({
      lat: 54.97761367069628,
      lng: -129.37500000000003,
    });
  });

  it('When the map is clicked, the resulting popup can be closed', async () => {
    findOneWhereContains.mockResolvedValue({
      features: [
        {
          type: 'Feature',
          geometry: { type: 'Point', coordinates: [-1.133005, 52.629835] },
          properties: {
            propertyTypeId: 0,
          },
        },
      ],
    });
    const { container } = render(getMap());
    await wait(() => {
      const map = container.querySelector('.leaflet-container');
      expect(map).toBeVisible();
      fireEvent.click(map!);
    });
    expect(findOneWhereContains).toHaveBeenLastCalledWith({
      lat: 54.97761367069628,
      lng: -129.37500000000003,
    });
    const closeButton = container.querySelector('.leaflet-popup-close-button');
    fireEvent.click(closeButton!);
    const layerPopup = container.querySelector('.leaflet-popup');
    expect(layerPopup).not.toBeInTheDocument();
  });

  it('the map cannot be clicked if not interactive', async () => {
    await wait(() => {
      findOneWhereContains.mockResolvedValue({
        features: [
          { type: 'Feature', geometry: { type: 'Point', coordinates: [-1.133005, 52.629835] } },
        ],
      });
      const { container } = render(getMap());
      const map = container.querySelector('.leaflet-container');
      expect(map).toBeVisible();
      fireEvent.click(map!);
    });
    expect(findOneWhereContains).toHaveBeenLastCalledWith({
      lat: 54.97761367069628,
      lng: -129.37500000000003,
    });
  });

  it('clusters can be clicked to zoom and spiderfy large clusters', async () => {
    ((useApi as unknown) as jest.Mock<Partial<PimsAPI>>).mockReturnValue({
      loadProperties: jest.fn(async () => {
        return createPoints(largeMockParcels);
      }),
      getParcel: async () => {
        return {} as IParcel;
      },
    });
    const { container } = render(getMap());
    await wait(() => {
      const icon = container.querySelector('.leaflet-control-zoom-out');
      fireEvent.click(icon!);
      const cluster = container.querySelector('.leaflet-marker-icon.marker-cluster');
      expect(cluster).toBeVisible();
      fireEvent.click(cluster!);
    });
    await wait(() => {
      const cluster = container.querySelector('.leaflet-marker-icon.marker-cluster');
      expect(cluster).toBeVisible();
      fireEvent.click(cluster!);
      const polyline = container.querySelector('.leaflet-pane.leaflet-overlay-pane svg g');
      expect(polyline).toBeVisible();
    });
  });
});
