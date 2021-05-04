import React from 'react';
import { Router, Route } from 'react-router-dom';
import { createMemoryHistory } from 'history';
import { render, cleanup, wait } from '@testing-library/react';
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
import { useLayerQuery, handleParcelDataLayerResponse } from 'components/maps/leaflet/LayerPopup';

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
    // fn.cancel = jest.fn();
    // fn.flush = jest.fn();
    return fn as ((...args: any[]) => any) & _.Cancelable;
  }),
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
(useLayerQuery as jest.Mock).mockReturnValue({
  findOneWhereContains: async () => featureResponse,
  findByPid: async () => featureResponse,
});

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
    </Provider>,
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
        expect(suggestion).toBeVisible();

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
        expect(suggestion).toBeVisible();

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
        expect(suggestion).toBeVisible();

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
        expect(suggestion).toBeVisible();

        suggestion.click();
        const addressSearchButton = getByTestId('address-search-button');
        addressSearchButton.click();
      });
      expect(findOneWhereContains).toHaveBeenCalled();
    });
  });
});
