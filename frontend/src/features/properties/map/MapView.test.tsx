import React from 'react';
import { createMemoryHistory } from 'history';
import { Router } from 'react-router-dom';
import { IProperty, IParcelDetail, IParcel } from 'actions/parcelsActions';
import Adapter from 'enzyme-adapter-react-16';
import Enzyme from 'enzyme';
import * as reducerTypes from 'constants/reducerTypes';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { wait, fireEvent, render, cleanup, screen } from '@testing-library/react';
import { Provider } from 'react-redux';
import { useKeycloak } from '@react-keycloak/web';
import { useApi, PimsAPI } from 'hooks/useApi';
import { fetchPropertyNames } from 'actionCreators/propertyActionCreator';
import axios from 'axios';

import MockAdapter from 'axios-mock-adapter';
import MapView from './MapView';
import { noop } from 'lodash';
import { createPoints } from 'components/maps/leaflet/mapUtils';
import { useLayerQuery } from 'components/maps/leaflet/LayerPopup';

const mockAxios = new MockAdapter(axios);
jest.mock('@react-keycloak/web');
Enzyme.configure({ adapter: new Adapter() });
const mockStore = configureMockStore([thunk]);
jest.mock('hooks/useApi');
jest.mock('actionCreators/propertyActionCreator');
jest.mock('components/maps/leaflet/LayerPopup');

(fetchPropertyNames as any).mockImplementation(jest.fn(() => () => ['test']));

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
const mockDetails: IParcelDetail = {
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
  [reducerTypes.LOOKUP_CODE]: { lookupCodes: [] },
  [reducerTypes.PARCEL]: { parcelDetail: mockDetails, draftParcels: [], parcels: mockParcels },
  [reducerTypes.LEAFLET_CLICK_EVENT]: { parcelDetail: mockDetails },
});

let history = createMemoryHistory();
describe('MapProperties View', () => {
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
    return (
      <Provider store={store}>
        <Router history={history}>
          <MapView
            disableMapFilterBar={false}
            disabled={false}
            showParcelBoundaries={true}
            onMarkerPopupClosed={noop}
          />
        </Router>
      </Provider>
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
    await wait(() => {
      const { container } = render(getMap());
      expect(container.querySelector('.leaflet-marker-icon')).toBeVisible();
    });
  });
  it('Rendered markers can be clicked', async () => {
    await wait(() => {
      const { container } = render(getMap());
      const icon = container.querySelector('.leaflet-marker-icon');
      expect(icon).toBeVisible();
      fireEvent.click(icon!);
      expect(screen.getByText('Property Info')).toBeVisible();
    });
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
    expect(findOneWhereContains).toHaveBeenLastCalledWith({ lat: 48, lng: 123 });
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
    expect(findOneWhereContains).toHaveBeenLastCalledWith({ lat: 48, lng: 123 });
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
