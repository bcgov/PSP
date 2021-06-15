import { useKeycloak } from '@react-keycloak/web';
import { cleanup, fireEvent, render, wait } from '@testing-library/react';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';
import { usePropertyNames } from 'features/properties/common/slices/usePropertyNames';
import { PropertyFilter } from 'features/properties/filter';
import { createMemoryHistory } from 'history';
import { PimsAPI, useApi } from 'hooks/useApi';
import { IParcel, IProperty } from 'interfaces';
import { Map as LeafletMap } from 'leaflet';
import React, { createRef } from 'react';
import { Map as ReactLeafletMap, MapProps as LeafletMapProps, Marker } from 'react-leaflet';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import leafletMouseSlice from 'store/slices/leafletMouse/LeafletMouseSlice';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { IPropertyDetail, propertiesSlice } from 'store/slices/properties';
import { TenantProvider } from 'tenants';
import TestCommonWrapper from 'utils/TestCommonWrapper';

import Map from './Map';
import { createPoints } from './mapUtils';
import SelectedPropertyMarker from './SelectedPropertyMarker/SelectedPropertyMarker';

const mockAxios = new MockAdapter(axios);
jest.mock('@react-keycloak/web');
Enzyme.configure({ adapter: new Adapter() });
const mockStore = configureMockStore([thunk]);
jest.mock('hooks/useApi');

jest.mock('features/properties/common/slices/usePropertyNames');
const fetchPropertyNames = jest.fn(() => () => Promise.resolve(['test']));
(usePropertyNames as any).mockImplementation(() => ({
  fetchPropertyNames,
}));

// This mocks the parcels of land a user can see - should be able to see 2 markers
const mockParcels = [
  { id: 1, latitude: 48.455059, longitude: -123.496452, propertyTypeId: 1 },
  { id: 2, latitude: 53.917065, longitude: -122.749672, propertyTypeId: 0 },
] as IProperty[];
((useApi as unknown) as jest.Mock<Partial<PimsAPI>>).mockReturnValue({
  loadProperties: jest.fn(async () => {
    return createPoints(mockParcels);
  }),
  getParcel: async () => {
    return {} as IParcel;
  },
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
  [propertiesSlice.name]: { parcelDetail: mockDetails, draftParcels: [] },
  [leafletMouseSlice.name]: { parcelDetail: mockDetails },
});

// To check for alert message
const emptyDetails = null;
const noParcels = [] as IProperty[];

let history = createMemoryHistory();
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
      url:
        'https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}',
      attribution:
        'Tiles &copy; Esri &mdash; Source: Esri, DigitalGlobe, GeoEye, Earthstar Geographics, CNES/Airbus DS, USDA, USGS, AeroGRID, IGN, and the GIS User Community',
      thumbnail: '/satellite.jpg',
    },
  ],
};

describe('MapProperties View', () => {
  (useKeycloak as jest.Mock).mockReturnValue({
    keycloak: {
      userInfo: {
        agencies: [0],
      },
    },
  });
  beforeEach(() => {
    process.env.REACT_APP_TENANT = 'MOTI';
    mockAxios.reset();
    jest.clearAllMocks();
    mockAxios.onGet('/basemaps.json').reply(200, baseMapLayers);
    mockAxios.onAny().reply(200);
    cleanup();
    history = createMemoryHistory();
    delete (window as any).ResizeObserver;
    window.ResizeObserver = jest.fn().mockImplementation(() => ({
      observe: jest.fn(),
      unobserve: jest.fn(),
      disconnect: jest.fn(),
    }));
  });

  afterEach(() => {
    delete process.env.REACT_APP_TENANT;
    window.ResizeObserver = ResizeObserver;
    jest.restoreAllMocks();
  });

  const getMap = (
    mapRef: React.RefObject<ReactLeafletMap<LeafletMapProps, LeafletMap>>,
    properties: IProperty[],
    selectedProperty: any,
    disableMapFilterBar: boolean = false,
  ) => {
    return (
      <TestCommonWrapper store={store} history={history}>
        <Map
          lat={48.43}
          lng={-123.37}
          zoom={14}
          properties={properties}
          selectedProperty={selectedProperty}
          agencies={[]}
          lotSizes={[]}
          onMarkerClick={jest.fn()}
          mapRef={mapRef}
          administrativeAreas={[]}
          disableMapFilterBar={disableMapFilterBar}
        />
      </TestCommonWrapper>
    );
  };

  it('Opens the slide out when clicked', async () => {
    const mapRef = createRef<ReactLeafletMap<LeafletMapProps, LeafletMap>>();
    const component = mount(getMap(mapRef, mockParcels, mockDetails, true));
    await wait(() => expect(mapRef.current).toBeDefined(), { timeout: 500 });
    const infoButton = component.find('#slideOutInfoButton').first();
    infoButton.simulate('click');
    const emptySlideOut = component.find('p#emptySlideOut');
    expect(emptySlideOut.text()).toEqual('Click a pin to view the property details');
  });

  it('opens the layers control when clicked', async () => {
    const mapRef = createRef<ReactLeafletMap<LeafletMapProps, LeafletMap>>();
    const component = mount(getMap(mapRef, mockParcels, mockDetails, true));
    await wait(() => expect(mapRef.current).toBeDefined(), { timeout: 500 });
    let layersControlButton = component.find('#layersControlButton').first();
    layersControlButton.simulate('click');
    layersControlButton = component.find('#layersControlButton').first();
    expect(layersControlButton.hasClass('open')).toBeTruthy();
  });

  it('Renders the map with the filter bar disabled', async () => {
    const mapRef = createRef<ReactLeafletMap<LeafletMapProps, LeafletMap>>();
    const component = mount(getMap(mapRef, mockParcels, mockDetails, true));
    await wait(() => expect(mapRef.current).toBeDefined(), { timeout: 500 });
    const propertyFilter = component.find(PropertyFilter);
    expect(propertyFilter).toEqual({});
  });

  it('Renders the marker in correct position', async () => {
    const mapRef = createRef<ReactLeafletMap<LeafletMapProps, LeafletMap>>();
    const component = mount(getMap(mapRef, mockParcels, mockDetails));
    await wait(() => expect(mapRef.current).toBeDefined(), { timeout: 500 });
    const selectedMarkers = component.find(SelectedPropertyMarker);
    expect(selectedMarkers.length).toEqual(1);
    const markerProps = selectedMarkers.first().props();
    expect(markerProps.position).toEqual([48, 123]);
  });

  it('Should render 0 markers when there are no parcels', async () => {
    const mapRef = createRef<ReactLeafletMap<LeafletMapProps, LeafletMap>>();
    const component = mount(getMap(mapRef, noParcels, emptyDetails));
    await wait(() => expect(mapRef.current).toBeDefined(), { timeout: 500 });
    const marker = component.find(Marker);
    expect(marker.length).toBe(0);
    const selectedMarker = component.find(SelectedPropertyMarker);
    expect(selectedMarker.length).toBe(0);
  });

  it('Renders the properties as cluster and on selected property', async () => {
    const mapRef = createRef<ReactLeafletMap<LeafletMapProps, LeafletMap>>();

    const component = mount(getMap(mapRef, mockParcels, mockDetails));

    await wait(() => expect(mapRef.current).toBeDefined(), { timeout: 500 });
    const marker = component.find(Marker);
    const selectedMarker = component.find(SelectedPropertyMarker).first();
    expect(selectedMarker).toBeDefined();
    await wait(
      () => {
        expect(marker.length).toBe(1);
      },
      { timeout: 500 },
    );
  });

  it('by default makes the expected calls to load map data', async () => {
    const mapRef = createRef<ReactLeafletMap<LeafletMapProps, LeafletMap>>();

    mount(getMap(mapRef, noParcels, emptyDetails));

    const { loadProperties } = useApi();
    const bbox = (loadProperties as jest.Mock).mock.calls.map(call => call[0].bbox);
    const expectedBbox = [
      '-146.25,-135,55.77657301866769,61.60639637138628',
      '-146.25,-135,48.922499263758255,55.77657301866769',
      '-146.25,-135,40.97989806962013,48.922499263758255',
      '-135,-123.75,55.77657301866769,61.60639637138628',
      '-135,-123.75,48.922499263758255,55.77657301866769',
      '-135,-123.75,40.97989806962013,48.922499263758255',
      '-123.75,-112.5,55.77657301866769,61.60639637138628',
      '-123.75,-112.5,48.922499263758255,55.77657301866769',
      '-123.75,-112.5,40.97989806962013,48.922499263758255',
    ]; //given our map dimensions and center point, this array should never change.
    await wait(() => expect(loadProperties).toHaveBeenCalledTimes(9), { timeout: 500 });
    expect(bbox).toEqual(expectedBbox);
  });

  it('makes the correct calls to load map data when filter updated.', async () => {
    const mapRef = createRef<ReactLeafletMap<LeafletMapProps, LeafletMap>>();

    const { container } = render(getMap(mapRef, noParcels, emptyDetails));
    const nameInput = container.querySelector('#name-field');
    fireEvent.change(nameInput!, {
      target: {
        value: 'testname',
      },
    });
    fireEvent.blur(nameInput!);
    const searchButton = container.querySelector('#search-button');
    fireEvent.click(searchButton!);

    const { loadProperties } = useApi();
    await wait(() => expect(loadProperties).toHaveBeenCalledTimes(18), { timeout: 500 });
    expect((loadProperties as jest.Mock).mock.calls[9][0].name).toBe('testname');
  });

  it('filter will fire everytime the search button is clicked', async () => {
    const mapRef = createRef<ReactLeafletMap<LeafletMapProps, LeafletMap>>();

    const { container } = render(getMap(mapRef, noParcels, emptyDetails));
    const searchButton = container.querySelector('#search-button');
    fireEvent.click(searchButton!);

    const { loadProperties } = useApi();
    await wait(() => expect(loadProperties).toHaveBeenCalledTimes(18), { timeout: 500 });
  });

  it('makes the correct calls to load the map data when the reset filter is clicked', async () => {
    const mapRef = createRef<ReactLeafletMap<LeafletMapProps, LeafletMap>>();

    const { container } = render(getMap(mapRef, noParcels, emptyDetails));
    const { loadProperties } = useApi();

    await wait(() => expect(loadProperties).toHaveBeenCalledTimes(9), { timeout: 500 });

    const nameInput = container.querySelector('#name-field');
    fireEvent.change(nameInput!, {
      target: {
        value: 'testname',
      },
    });
    fireEvent.blur(nameInput!);

    const searchButton = container.querySelector('#search-button');
    fireEvent.click(searchButton!);
    await wait(() => expect(loadProperties).toHaveBeenCalledTimes(18), { timeout: 500 });
    const resetButton = container.querySelector('#reset-button');
    fireEvent.click(resetButton!);
    await wait(() => expect(loadProperties).toHaveBeenCalledTimes(27), { timeout: 500 });

    expect((loadProperties as jest.Mock).mock.calls[18][0].name).toBe('');
  });
});
