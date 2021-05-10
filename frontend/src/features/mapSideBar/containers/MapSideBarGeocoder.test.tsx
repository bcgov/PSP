import React from 'react';
import { Router, Route } from 'react-router-dom';
import { createMemoryHistory } from 'history';
import { render, cleanup, wait, screen, act } from '@testing-library/react';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { Provider } from 'react-redux';

import { ToastContainer } from 'react-toastify';
import MapSideBarContainer from './MapSideBarContainer';
import { noop } from 'lodash';
import * as reducerTypes from 'constants/reducerTypes';
import * as actionTypes from 'constants/actionTypes';
import { IParcel } from 'actions/parcelsActions';
import { mockDetails } from 'mocks/filterDataMock';
import VisibilitySensor from 'react-visibility-sensor';
import { useKeycloak } from '@react-keycloak/web';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Claims } from 'constants/claims';
import { fireEvent } from '@testing-library/dom';
import { useApi, PimsAPI, IGeocoderResponse, IGeocoderPidsResponse } from 'hooks/useApi';
import * as API from 'constants/API';
import * as _ from 'lodash';
import { useLayerQuery } from 'components/maps/leaflet/LayerPopup';
import { TenantProvider, defaultTenant } from 'tenants';

jest.mock(
  'react-visibility-sensor',
  (): typeof VisibilitySensor => ({ children, partialVisibility, ...rest }: any) => (
    <div {...rest}>{typeof children === 'function' ? children({ isVisible: true }) : children}</div>
  ),
);
const mockAxios = new MockAdapter(axios);

jest.mock('@react-keycloak/web');
jest.spyOn(_, 'debounce').mockImplementation(
  jest.fn((fn: any) => {
    return fn as (...args: any[]) => any;
  }) as any,
);
jest.spyOn(global, 'fetch').mockImplementation(
  () =>
    Promise.resolve({
      json: () => Promise.resolve(JSON.stringify(defaultTenant)),
    }) as Promise<Response>,
);

const mockStore = configureMockStore([thunk]);
const getStore = (parcelDetail?: IParcel) =>
  mockStore({
    [reducerTypes.NETWORK]: {
      [actionTypes.GET_PARCEL_DETAIL]: {
        status: 200,
      },
    },
    [reducerTypes.PARCEL]: {
      parcelDetail: {
        parcelDetail: parcelDetail,
        propertyTypeId: 0,
      },
      parcels: [],
      draftParcels: [],
    },
    [reducerTypes.LOOKUP_CODE]: {
      lookupCodes: [
        { name: 'BC', code: 'BC', id: '1', isDisabled: false, type: API.PROVINCE_CODE_SET_NAME },
        {
          name: 'Victoria',
          id: '1',
          isDisabled: false,
          type: API.AMINISTRATIVE_AREA_CODE_SET_NAME,
        },
      ],
    },
    [reducerTypes.PARCEL]: { parcels: [], draftParcels: [] },
  });

const history = createMemoryHistory({
  getUserConfirmation: (message, callback) => {
    callback(true);
  },
});

const geocoderResponse = {
  siteId: '1',
  fullAddress: '12345 fake st.',
  address1: '4321 fake st.',
  administrativeArea: 'Victoria',
  provinceCode: 'BC',
  latitude: 1,
  longitude: 2,
  score: 100,
};

const featureResponse = {
  features: [
    {
      type: 'Feature',
      geometry: { type: 'Point', coordinates: [1, 2] },
      properties: { PID: '123-456-789' },
    },
  ],
  type: 'FeatureCollection',
};
jest.mock('components/maps/leaflet/LayerPopup');
const handleParcelDataLayerResponse = jest.fn();

jest.mock('hooks/useApi');
((useApi as unknown) as jest.Mock<Partial<PimsAPI>>).mockReturnValue({
  searchAddress: async () => {
    return [geocoderResponse] as IGeocoderResponse[];
  },
  getSitePids: async () => {
    return {
      siteId: '1',
      pids: ['123-456-789'],
    } as IGeocoderPidsResponse;
  },
});

const renderContainer = ({ store }: any) =>
  render(
    <TenantProvider>
      <Provider store={store ?? getStore()}>
        <Router history={history}>
          <ToastContainer
            autoClose={5000}
            hideProgressBar
            newestOnTop={false}
            closeOnClick={false}
            rtl={false}
            pauseOnFocusLoss={false}
          />
          <Route path="/mapView/:id?">
            <MapSideBarContainer refreshParcels={noop} properties={[]} />
          </Route>
        </Router>
      </Provider>
    </TenantProvider>,
  );

describe('MapSideBarContainer Geocoder functionality', () => {
  // clear mocks before each test
  beforeEach(() => {
    (useKeycloak as jest.Mock).mockReturnValue({
      keycloak: {
        userInfo: {
          agencies: [1],
          roles: [Claims.PROPERTY_EDIT],
        },
        subject: 'test',
      },
    });
    (useLayerQuery as jest.Mock).mockReturnValue({
      findOneWhereContains: async () => featureResponse,
      findByPid: async () => featureResponse,
      findByPin: async () => featureResponse,
      handleParcelDataLayerResponse,
    });
    mockAxios.onAny().reply(200, {});
  });
  afterEach(() => {
    history.push({ search: '' });
    cleanup();
    jest.clearAllMocks();
  });
  describe('land(parcel) functionality', () => {
    it('performs search using geocoder', async () => {
      history.push('/mapview/?sidebar=true&sidebarContext=addBareLand&disabled=true');
      const parcel = { ...mockDetails[0], projectNumbers: ['SPP-10000'] };
      mockAxios.onGet().reply(200, parcel);
      const { getByText, getByTestId, container } = renderContainer({});
      const searchAddress = container.querySelector(`input[name="data.searchAddress"]`);
      await wait(async () => {
        fireEvent.change(searchAddress!, {
          target: {
            value: '123 fake st.',
          },
        });

        const suggestion = getByText('12345 fake st.');
        expect(suggestion).toBeTruthy();

        suggestion.click();
        const addressSearchButton = getByTestId('address-search-button');
        addressSearchButton.click();
      });
      expect(handleParcelDataLayerResponse).toHaveBeenCalledTimes(1);
    });
    it('performs searches using geocoder, and handles a response with no siteid populates fields', async () => {
      ((useApi as unknown) as jest.Mock<Partial<PimsAPI>>).mockReturnValue({
        searchAddress: async () => {
          return [
            {
              ...geocoderResponse,
              siteId: '',
            },
          ] as IGeocoderResponse[];
        },
        getSitePids: async () => {
          return {
            siteId: '1',
            pids: ['123-456-789'],
          } as IGeocoderPidsResponse;
        },
      });

      history.push('/mapview/?sidebar=true&sidebarContext=addBareLand&disabled=true');
      const parcel = { ...mockDetails[0], projectNumbers: ['SPP-10000'] };
      mockAxios.onGet().reply(200, parcel);
      const { getByText, getByTestId, container } = renderContainer({});
      const searchAddress = container.querySelector(`input[name="data.searchAddress"]`);
      await wait(async () => {
        fireEvent.change(searchAddress!, {
          target: {
            value: '123 fake st.',
          },
        });

        const suggestion = getByText('12345 fake st.');
        expect(suggestion).toBeTruthy();

        suggestion.click();
        const addressSearchButton = getByTestId('address-search-button');
        addressSearchButton.click();
      });
      expect(handleParcelDataLayerResponse).toHaveBeenCalledTimes(1);
    });

    it('performs searches using geocoder, and handles a response with no siteid and the layer has a PID', async () => {
      ((useApi as unknown) as jest.Mock<Partial<PimsAPI>>).mockReturnValue({
        searchAddress: async () => {
          return [
            {
              ...geocoderResponse,
              siteId: '',
            },
          ] as IGeocoderResponse[];
        },
        getSitePids: async () => {
          return {
            siteId: '1',
            pids: ['123-456-789'],
          } as IGeocoderPidsResponse;
        },
      });

      history.push('/mapview/?sidebar=true&sidebarContext=addBareLand&disabled=true');
      const parcel = { ...mockDetails[0], projectNumbers: ['SPP-10000'] };
      mockAxios.onGet().reply(200, parcel);
      const { getByText, getByTestId, container } = renderContainer({});
      const searchAddress = container.querySelector(`input[name="data.searchAddress"]`);
      await wait(async () => {
        fireEvent.change(searchAddress!, {
          target: {
            value: '123 fake st.',
          },
        });

        const suggestion = getByText('12345 fake st.');
        expect(suggestion).toBeTruthy();

        suggestion.click();
        const addressSearchButton = getByTestId('address-search-button');
        addressSearchButton.click();
      });
      expect(handleParcelDataLayerResponse).toHaveBeenCalledTimes(1);
    });

    it('performs searches using geocoder, and handles a response with no siteid and the layer and calls findOneWhereContains', async () => {
      ((useApi as unknown) as jest.Mock<Partial<PimsAPI>>).mockReturnValue({
        searchAddress: async () => {
          return [
            {
              ...geocoderResponse,
              siteId: '',
            },
          ] as IGeocoderResponse[];
        },
        getSitePids: async () => {
          return {
            siteId: '1',
            pids: ['123-456-789'],
          } as IGeocoderPidsResponse;
        },
      });
      const findOneWhereContains = jest.fn(async () => ({
        features: [],
        type: 'FeatureCollection',
      }));
      (useLayerQuery as jest.Mock).mockReturnValue({
        findOneWhereContains,
        findByPid: async () => featureResponse,
      });

      history.push('/mapview/?sidebar=true&sidebarContext=addBareLand&disabled=true');
      const parcel = { ...mockDetails[0], projectNumbers: ['SPP-10000'] };
      mockAxios.onGet().reply(200, parcel);
      const { getByText, getByTestId, container } = renderContainer({});
      const searchAddress = container.querySelector(`input[name="data.searchAddress"]`);
      await wait(async () => {
        fireEvent.change(searchAddress!, {
          target: {
            value: '123 fake st.',
          },
        });

        const suggestion = getByText('12345 fake st.');
        expect(suggestion).toBeTruthy();

        suggestion.click();
        const addressSearchButton = getByTestId('address-search-button');
        addressSearchButton.click();
      });
      expect(findOneWhereContains).toHaveBeenCalled();
    });
  });
});

describe('MapSideBarContainer PID/PIN search functionality', () => {
  // clear mocks before each test
  beforeEach(() => {
    (useKeycloak as jest.Mock).mockReturnValue({
      keycloak: {
        userInfo: {
          agencies: [1],
          roles: [Claims.PROPERTY_EDIT],
        },
        subject: 'test',
      },
    });
    (useLayerQuery as jest.Mock).mockReturnValue({
      findOneWhereContains: async () => featureResponse,
      findByPid: async () => featureResponse,
      findByPin: async () => featureResponse,
      handleParcelDataLayerResponse,
    });
    mockAxios.reset();
    mockAxios.onAny().reply(200, {});
  });
  afterEach(() => {
    history.push({ search: '' });
    cleanup();
    jest.clearAllMocks();
  });
  it('searches by pid and calls expected handle function', async () => {
    // clear mocks before each test
    history.push('/mapview/?sidebar=true&sidebarContext=addBareLand&disabled=true');
    const parcel = { ...mockDetails[0], projectNumbers: ['SPP-10000'] };
    mockAxios.onGet().reply(200, parcel);
    const { getByTestId, container } = renderContainer({});
    const searchAddress = container.querySelector(`input[name="data.searchPid"]`);
    act(() => {
      fireEvent.change(searchAddress!, {
        target: {
          value: '123-456-789',
        },
      });
    });
    const addressSearchButton = getByTestId('pid-search-button');
    addressSearchButton.click();
    await wait(async () => expect(handleParcelDataLayerResponse).toHaveBeenCalledTimes(1));
  });

  it('searches by pid and populates form if match found', async () => {
    // clear mocks before each test
    history.push('/mapview/?sidebar=true&sidebarContext=addBareLand&disabled=true');
    const parcel = { ...mockDetails[0], projectNumbers: ['SPP-10000'] };
    mockAxios.onGet().reply(200, [parcel]);
    const { getByTestId, container } = renderContainer({});
    const searchAddress = container.querySelector(`input[name="data.searchPid"]`);
    act(() => {
      fireEvent.change(searchAddress!, {
        target: {
          value: '123-456-789',
        },
      });
    });
    await wait(async () => {
      const addressSearchButton = getByTestId('pid-search-button');
      addressSearchButton.click();
    });
    await wait(() => expect(screen.getByDisplayValue('1234 mock Street')).toBeTruthy());
  });

  it('searches by pin and calls expected handle function', async () => {
    // clear mocks before each test
    history.push('/mapview/?sidebar=true&sidebarContext=addBareLand&disabled=true');
    const parcel = { ...mockDetails[0], projectNumbers: ['SPP-10000'] };
    mockAxios.onGet().reply(200, parcel);
    const { getByTestId, container } = renderContainer({});
    const searchAddress = container.querySelector(`input[name="data.searchPin"]`);
    act(() => {
      fireEvent.change(searchAddress!, {
        target: {
          value: '123456789',
        },
      });
    });
    await wait(async () => {
      const addressSearchButton = getByTestId('pin-search-button');
      addressSearchButton.click();
    });
    await wait(async () => expect(handleParcelDataLayerResponse).toHaveBeenCalledTimes(1));
  });

  it('searches by pin and populates form if match found', async () => {
    // clear mocks before each test
    history.push('/mapview/?sidebar=true&sidebarContext=addBareLand&disabled=true');
    const parcel = { ...mockDetails[0], projectNumbers: ['SPP-10000'] };
    mockAxios.onGet().reply(200, [parcel]);
    const { getByTestId, container } = renderContainer({});
    const searchAddress = container.querySelector(`input[name="data.searchPin"]`);
    act(() => {
      fireEvent.change(searchAddress!, {
        target: {
          value: '123456789',
        },
      });
    });
    await wait(async () => {
      const addressSearchButton = getByTestId('pin-search-button');
      addressSearchButton.click();
    });
    await wait(() => expect(screen.getByDisplayValue('1234 mock Street')).toBeTruthy());
  });
});
