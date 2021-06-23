import { useKeycloak } from '@react-keycloak/web';
import { fireEvent } from '@testing-library/dom';
import { act, cleanup, render, screen, wait } from '@testing-library/react';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { useLayerQuery } from 'components/maps/leaflet/LayerPopup';
import * as actionTypes from 'constants/actionTypes';
import * as API from 'constants/API';
import { Claims } from 'constants/claims';
import { createMemoryHistory } from 'history';
import { IGeocoderPidsResponse, IGeocoderResponse, PimsAPI, useApi } from 'hooks/useApi';
import { IParcel } from 'interfaces';
import * as _ from 'lodash';
import { mockDetails } from 'mocks/filterDataMock';
import { Route } from 'react-router-dom';
import VisibilitySensor from 'react-visibility-sensor';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { networkSlice } from 'store/slices/network/networkSlice';
import { propertiesSlice } from 'store/slices/properties';
import { defaultTenant } from 'tenants';
import TestCommonWrapper from 'utils/TestCommonWrapper';

import PimsInventoryContainer from './PimsInventoryContainer';

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
    [networkSlice.name]: {
      [actionTypes.GET_PARCEL_DETAIL]: {
        status: 200,
      },
    },
    [propertiesSlice.name]: {
      parcelDetail: {
        parcelDetail: parcelDetail,
        propertyTypeId: 0,
      },
      parcels: [],
      draftParcels: [],
    },
    [lookupCodesSlice.name]: {
      lookupCodes: [
        { name: 'BC', code: 'BC', id: '1', isDisabled: false, type: API.PROVINCE_CODE_SET_NAME },
        {
          name: 'Victoria',
          id: '1',
          isDisabled: false,
          type: API.ADMINISTRATIVE_AREA_CODE_SET_NAME,
        },
      ],
    },
    [propertiesSlice.name]: { parcels: [], draftParcels: [] },
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
    <TestCommonWrapper store={store ?? getStore()} history={history}>
      <Route path="/mapView/:id?">
        <PimsInventoryContainer />
      </Route>
    </TestCommonWrapper>,
  );

describe('PimsInventoryContainer Geocoder functionality', () => {
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

describe('PimsInventoryContainer PID/PIN search functionality', () => {
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
