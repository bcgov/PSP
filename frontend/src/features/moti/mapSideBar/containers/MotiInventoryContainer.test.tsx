import {
  act,
  fireEvent,
  render,
  screen,
  wait,
  waitForElementToBeRemoved,
} from '@testing-library/react';
import { useLayerQuery } from 'components/maps/leaflet/LayerPopup';
import { createMemoryHistory } from 'history';
import { useApiLtsa } from 'hooks/pims-api/useApiLtsa';
import { PimsAPI, useApi } from 'hooks/useApi';
import { enableFetchMocks } from 'jest-fetch-mock';
import { Route } from 'react-router-dom';
import VisibilitySensor from 'react-visibility-sensor';
import leafletMouseSlice from 'store/slices/leafletMouse/LeafletMouseSlice';
import TestCommonWrapper from 'utils/TestCommonWrapper';
import { fillInput } from 'utils/testUtils';

import MotiInventoryContainer from './MotiInventoryContainer';

enableFetchMocks();

jest.mock(
  'react-visibility-sensor',
  (): typeof VisibilitySensor => ({ children, partialVisibility, ...rest }: any) => (
    <div {...rest}>{typeof children === 'function' ? children({ isVisible: true }) : children}</div>
  ),
);

const history = createMemoryHistory({
  getUserConfirmation: (message, callback) => {
    callback(true);
  },
});

jest.mock('hooks/pims-api/useApiLtsa');
const getParcelInfo = jest.fn();
(useApiLtsa as jest.Mock).mockReturnValue({
  getParcelInfo,
});

const findOneWhereContains = jest.fn();
const findByPid = jest.fn();
jest.mock('components/maps/leaflet/LayerPopup');
(useLayerQuery as jest.Mock).mockReturnValue({
  findOneWhereContains,
  findByPid,
});

const renderContainer = ({ store }: any) =>
  render(
    <TestCommonWrapper history={history} store={store}>
      <Route path="/mapview/:id?">
        <MotiInventoryContainer />
      </Route>
    </TestCommonWrapper>,
  );

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
const searchAddress = jest.fn();
jest.mock('hooks/useApi');
((useApi as unknown) as jest.Mock<Partial<PimsAPI>>).mockReturnValue({
  searchAddress,
});

describe('MotiInventoryContainer functionality', () => {
  beforeEach(() => {
    fetchMock.mockResponse(JSON.stringify({ status: 200, body: {} }));
    history.push('');
  });
  afterAll(() => {
    jest.restoreAllMocks();
  });
  it('displays as expected', () => {
    history.push('/mapview?sidebar=true');
    const { container } = renderContainer({});
    expect(container.firstChild).toMatchSnapshot();
  });
  it('does not display by default', () => {
    const { container } = renderContainer({});
    const sidebar = container.querySelector('.map-side-drawer');
    expect(sidebar).toBeNull();
  });

  it('displays when sidebar query param is true', () => {
    history.push('/mapview?sidebar=true');
    const { getByText } = renderContainer({});
    expect(getByText('Add Titled Property to Inventory')).toBeInTheDocument();
  });
  it('does not display when close icon clicked', async () => {
    history.push('/mapview?sidebar=true');
    const { getByTitle, getByText } = renderContainer({});
    const closeTitleButton = getByTitle('close');
    fireEvent.click(closeTitleButton);
    waitForElementToBeRemoved(() => getByText('Add Titled Property to Inventory'));
  });

  describe('search functionality', () => {
    beforeEach(() => {
      history.push('/mapview?sidebar=true&sidebarContext=addBareLand');
      jest.clearAllMocks();
    });
    it('searches by pid', async () => {
      const { container, getByTestId, findByTestId } = renderContainer({});

      await findByTestId('pid-search-button');
      await fillInput(container, 'searchPid', '123-456-789');
      fireEvent.click(getByTestId('pid-search-button'));
      await wait(() => {
        expect(findByPid).toHaveBeenCalledWith('123-456-789');
        expect(getParcelInfo).toHaveBeenCalledWith('123-456-789');
      });
    });

    it('does not search when no pid is provided', async () => {
      const { findByTestId } = renderContainer({});

      const searchButton = await findByTestId('pid-search-button');
      fireEvent.click(searchButton);
      await wait(() => {
        expect(findByPid).not.toHaveBeenCalled();
        expect(getParcelInfo).not.toHaveBeenCalled();
      });
    });

    it('displays a warning when the pid cannot be found in ltsa', async () => {
      getParcelInfo.mockRejectedValueOnce({ response: { status: 404 } });
      const { getByTestId, container, findByText, findByTestId } = renderContainer({});

      await findByTestId('pid-search-button');
      await fillInput(container, 'searchPid', '123-456-789');
      fireEvent.click(getByTestId('pid-search-button'));
      const toast = await findByText(
        `PID: 123-456-789 not found in Land Title Direct Search Service.`,
      );
      expect(toast).toBeInTheDocument();
    });

    it('displays an error when the ltsa request fails', async () => {
      getParcelInfo.mockRejectedValueOnce({ response: { status: 500 } });
      const { getByTestId, container, findByText, findByTestId } = renderContainer({});

      await findByTestId('pid-search-button');
      await fillInput(container, 'searchPid', '123-456-789');
      fireEvent.click(getByTestId('pid-search-button'));
      const toast = await findByText(
        `Failed to load parcel info from Land Title Direct Search Service.`,
      );
      expect(toast).toBeInTheDocument();
    });

    it('displays an error when the parcel layer request fails', async () => {
      findByPid.mockRejectedValueOnce({});
      const { getByTestId, container, findByText, findByTestId } = renderContainer({});

      await findByTestId('pid-search-button');
      await fillInput(container, 'searchPid', '123-456-789');
      fireEvent.click(getByTestId('pid-search-button'));
      const toast = await findByText('Failed to load parcel info from parcel layer');
      expect(toast).toBeInTheDocument();
    });

    it('does not search when no pid is provided', async () => {
      const { findByTestId } = renderContainer({});

      const searchButton = await findByTestId('pid-search-button');
      fireEvent.click(searchButton);
      await wait(() => {
        expect(findByPid).not.toHaveBeenCalled();
        expect(getParcelInfo).not.toHaveBeenCalled();
      });
    });
    it('searches by address', async () => {
      findOneWhereContains.mockResolvedValueOnce({
        features: [{ properties: { PID: '987-654-321' } }],
      });
      searchAddress.mockResolvedValueOnce([geocoderResponse]);

      const { container, getByTestId, findByText, findByTestId } = renderContainer({});

      await findByTestId('pid-search-button');
      await fillInput(container, 'searchAddress', '12345 fake');
      const suggestion = await findByText('12345 fake st.');
      expect(suggestion).toBeTruthy();

      act(() => {
        suggestion.click();
      });
      await wait(() => {
        const addressSearchButton = getByTestId('address-search-button');
        expect(addressSearchButton).not.toHaveAttribute('disabled');
        addressSearchButton.click();
      });

      await wait(() => {
        expect(findOneWhereContains).toHaveBeenCalledWith({ lat: 1, lng: 2 });
        expect(getParcelInfo).toHaveBeenCalledWith('987-654-321');
      });
    });

    it('displays an error when searching by address if geocoder does not return a lat/lng', async () => {
      searchAddress.mockResolvedValueOnce([
        { ...geocoderResponse, latitude: undefined, longitude: undefined },
      ]);
      const { container, getByTestId, findByText, findByTestId } = renderContainer({});

      await findByTestId('pid-search-button');
      await fillInput(container, 'searchAddress', '12345 fake');
      const suggestion = await findByText('12345 fake st.');
      expect(suggestion).toBeTruthy();

      act(() => {
        suggestion.click();
      });
      await wait(() => {
        const addressSearchButton = getByTestId('address-search-button');
        expect(addressSearchButton).not.toHaveAttribute('disabled');
        addressSearchButton.click();
      });

      const toast = await findByText(
        'Unable to perform search, property missing latitude/longitude',
      );
      expect(toast).toBeInTheDocument();
    });

    it('displays an error when searching by address if the parcel layer returns an error', async () => {
      findOneWhereContains.mockRejectedValueOnce({});
      searchAddress.mockResolvedValueOnce([geocoderResponse]);
      const { container, getByTestId, findByText, findByTestId } = renderContainer({});

      await findByTestId('pid-search-button');
      await fillInput(container, 'searchAddress', '12345 fake');
      const suggestion = await findByText('12345 fake st.');
      expect(suggestion).toBeTruthy();

      act(() => {
        suggestion.click();
      });
      await wait(() => {
        const addressSearchButton = getByTestId('address-search-button');
        expect(addressSearchButton).not.toHaveAttribute('disabled');
        addressSearchButton.click();
      });

      const toast = await findByText('Failed to load parcel info from parcel layer');
      expect(toast).toBeInTheDocument();
    });
  });
  describe('move marker functionality', () => {
    beforeEach(() => {
      history.push('/mapview?sidebar=true&sidebarContext=addBareLand');
      jest.clearAllMocks();
    });
    it('queries for data as expected when the marker is placed on the map', async () => {
      findOneWhereContains.mockResolvedValueOnce({
        features: [{ properties: { PID: '987-654-321' } }],
      });

      renderContainer({
        store: {
          [leafletMouseSlice.name]: {
            mapClickEvent: {
              originalEvent: { timeStamp: 1 },
              latlng: { lat: 1, lng: 2 },
            },
          },
        },
      });

      const landSearchMarker = await screen.findByTestId('land-search-marker');
      fireEvent.click(landSearchMarker);

      await wait(() => {
        expect(findOneWhereContains).toHaveBeenCalledWith({ lat: 1, lng: 2 });
        expect(getParcelInfo).toHaveBeenCalledWith('987-654-321');
      });
    });
  });
});
