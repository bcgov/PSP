import { act, fireEvent, render, screen, waitFor } from '@testing-library/react';
import { useLayerQuery } from 'components/maps/leaflet/LayerPopup';
import { ADMINISTRATIVE_AREA_CODE_SET_NAME, PROVINCE_CODE_SET_NAME } from 'constants/API';
import { createMemoryHistory } from 'history';
import { useApiLtsa } from 'hooks/pims-api/useApiLtsa';
import { IPimsAPI, useApi } from 'hooks/useApi';
import { enableFetchMocks } from 'jest-fetch-mock';
import { mockParcelLayerResponse } from 'mocks/filterDataMock';
import { Route } from 'react-router-dom';
import VisibilitySensor from 'react-visibility-sensor';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import leafletMouseSlice from 'store/slices/leafletMouse/LeafletMouseSlice';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { saveParcelLayerData } from 'store/slices/parcelLayerData/parcelLayerDataSlice';
import { propertiesSlice } from 'store/slices/properties';
import { store } from 'store/store';
import { fillInput } from 'utils/test-utils';
import TestCommonWrapper from 'utils/TestCommonWrapper';

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
const getTitleSummaries = jest.fn();
(useApiLtsa as jest.Mock).mockReturnValue({
  getParcelInfo,
  getTitleSummaries,
});

const findOneWhereContains = jest.fn();
const findByPid = jest.fn();
jest.mock('components/maps/leaflet/LayerPopup');
(useLayerQuery as jest.Mock).mockReturnValue({
  findOneWhereContains,
  findByPid,
});

const mockStore = configureMockStore([thunk]);
const defaultStore = mockStore({
  [leafletMouseSlice.name]: {
    mapClickEvent: {
      originalEvent: { timeStamp: 1 },
      latlng: { lat: 1, lng: 2 },
    },
  },
  [lookupCodesSlice.name]: {
    lookupCodes: [
      { name: 'BC', code: 'BC', id: '1', isDisabled: false, type: PROVINCE_CODE_SET_NAME },
      {
        name: 'Victoria',
        id: '1',
        isDisabled: false,
        type: ADMINISTRATIVE_AREA_CODE_SET_NAME,
      },
    ],
  },
  [propertiesSlice.name]: { parcels: [], draftParcels: [] },
});

const renderContainer = ({ store }: any) =>
  render(
    <TestCommonWrapper history={history} store={store ?? defaultStore} organizations={[1 as any]}>
      <Route path="/mapview/:id?">
        <MotiInventoryContainer />
      </Route>
    </TestCommonWrapper>,
  );

const isPidAvailable = jest.fn();
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
const getNearestAddress = jest.fn();
jest.mock('hooks/useApi');
((useApi as unknown) as jest.Mock<Partial<IPimsAPI>>).mockReturnValue({
  searchAddress,
  isPidAvailable,
  getNearestAddress,
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
    const { asFragment } = renderContainer({});
    expect(asFragment()).toMatchSnapshot();
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
    const { getByTitle, container } = renderContainer({});
    const closeTitleButton = getByTitle('close');
    fireEvent.click(closeTitleButton);
    await waitFor(() => {
      expect(container.querySelector('.map-side-drawer')).toHaveClass('close');
    });
  });

  describe('search functionality', () => {
    beforeEach(() => {
      history.push('/mapview?sidebar=true&sidebarContext=addBareLand');
      isPidAvailable.mockResolvedValue({ available: true });
      jest.clearAllMocks();
    });
    it('searches by pid', async () => {
      findByPid.mockResolvedValueOnce(mockParcelLayerResponse);
      const { container, getByTestId, findByTestId } = renderContainer({});

      await findByTestId('pid-search-button');
      await fillInput(container, 'searchPid', '123-456-789');
      fireEvent.click(getByTestId('pid-search-button'));
      const cityInput = await screen.findByDisplayValue('Victoria');

      expect(findByPid).toHaveBeenCalledWith('123-456-789');
      expect(getParcelInfo).toHaveBeenCalledWith('123-456-789');
      expect(getNearestAddress).toHaveBeenCalledWith({ lat: 48.42500804, lng: -123.339996055 });
      expect(findOneWhereContains).toHaveBeenCalled();
      expect(cityInput).toBeInTheDocument();
    });

    it('returns an error if pid not found', async () => {
      const { container, getByTestId, findByTestId } = renderContainer({});

      await findByTestId('pid-search-button');
      await fillInput(container, 'searchPid', '123-456-789');
      fireEvent.click(getByTestId('pid-search-button'));
      const toast = await screen.findByText(
        'Unable to find parcel identifier (PID) for the searched location. A property must have a PID to be added to PSP, ensure this property has a PID.',
      );
      expect(toast).toBeInTheDocument();
    });

    it('does not search when no pid is provided', async () => {
      const { findByTestId } = renderContainer({});

      const searchButton = await findByTestId('pid-search-button');
      fireEvent.click(searchButton);
      await waitFor(() => {
        expect(findByPid).not.toHaveBeenCalled();
        expect(getParcelInfo).not.toHaveBeenCalled();
        expect(getNearestAddress).not.toHaveBeenCalled();
      });
    });

    it('displays a warning when the pid cannot be found in ltsa', async () => {
      findByPid.mockResolvedValueOnce(mockParcelLayerResponse);
      getParcelInfo.mockRejectedValueOnce({ response: { status: 404 } });
      const { getByTestId, container, findByText, findByTestId } = renderContainer({});

      await findByTestId('pid-search-button');
      await fillInput(container, 'searchPid', '123-456-789');
      fireEvent.click(getByTestId('pid-search-button'));
      const toast = await findByText(`PID: 123-456-789 not found in Title Direct Search Service.`);
      expect(toast).toBeInTheDocument();
    });

    it('displays an error when the ltsa request fails', async () => {
      findByPid.mockResolvedValueOnce(mockParcelLayerResponse);
      getParcelInfo.mockRejectedValueOnce({ response: { status: 500 } });
      const { getByTestId, container, findByText, findByTestId } = renderContainer({});

      await findByTestId('pid-search-button');
      await fillInput(container, 'searchPid', '123-456-789');
      fireEvent.click(getByTestId('pid-search-button'));
      const toast = await findByText(`Request failed from Title Direct Search Service.`);
      expect(toast).toBeInTheDocument();
    });

    it('displays the ltsa error from the API if specified', async () => {
      findByPid.mockResolvedValueOnce(mockParcelLayerResponse);
      getParcelInfo.mockRejectedValueOnce({
        response: { status: 500, data: { details: 'test error' } },
      });
      const { getByTestId, container, findByText, findByTestId } = renderContainer({});

      await findByTestId('pid-search-button');
      await fillInput(container, 'searchPid', '123-456-789');
      fireEvent.click(getByTestId('pid-search-button'));
      const toast = await findByText('test error');
      expect(toast).toBeInTheDocument();
    });

    it('displays a warning when the pid cannot be found in the ltsa title summary service', async () => {
      findByPid.mockResolvedValueOnce(mockParcelLayerResponse);
      getTitleSummaries.mockRejectedValueOnce({ response: { status: 404 } });
      const { getByTestId, container, findByText, findByTestId } = renderContainer({});

      await findByTestId('pid-search-button');
      await fillInput(container, 'searchPid', '123-456-789');
      fireEvent.click(getByTestId('pid-search-button'));
      const toast = await findByText(`PID: 123-456-789 not found in Title Direct Search Service.`);
      expect(toast).toBeInTheDocument();
    });

    it('displays an error when the ltsa title summary request fails', async () => {
      findByPid.mockResolvedValueOnce(mockParcelLayerResponse);
      getTitleSummaries.mockRejectedValueOnce({ response: { status: 500 } });
      const { getByTestId, container, findByText, findByTestId } = renderContainer({});

      await findByTestId('pid-search-button');
      await fillInput(container, 'searchPid', '123-456-789');
      fireEvent.click(getByTestId('pid-search-button'));
      const toast = await findByText('Request failed from Title Direct Search Service.');
      expect(toast).toBeInTheDocument();
    });

    it('displays the ltsa error from the title summary API if specified', async () => {
      findByPid.mockResolvedValueOnce(mockParcelLayerResponse);
      getTitleSummaries.mockRejectedValueOnce({
        response: { status: 500, data: { details: 'test error' } },
      });
      const { getByTestId, container, findByText, findByTestId } = renderContainer({});

      await findByTestId('pid-search-button');
      await fillInput(container, 'searchPid', '123-456-789');
      fireEvent.click(getByTestId('pid-search-button'));
      const toast = await findByText('test error');
      expect(toast).toBeInTheDocument();
    });

    it('displays an error when the parcel layer request fails', async () => {
      findByPid.mockRejectedValueOnce({});
      const { getByTestId, container, findByText, findByTestId } = renderContainer({});

      await findByTestId('pid-search-button');
      await fillInput(container, 'searchPid', '123-456-789');
      fireEvent.click(getByTestId('pid-search-button'));
      const toast = await findByText(
        'Property search failed. Please check your search criteria and try again. If this error persists, contact the Help Desk.',
      );
      expect(toast).toBeInTheDocument();
    });

    it('address button is disabled by default', async () => {
      const { findByTestId } = renderContainer({});

      const searchButton = await findByTestId('address-search-button');
      expect(searchButton).toBeDisabled();
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
      await waitFor(() => {
        const addressSearchButton = getByTestId('address-search-button');
        expect(addressSearchButton).not.toHaveAttribute('disabled');
        addressSearchButton.click();
      });

      await waitFor(() => {
        expect(findOneWhereContains).toHaveBeenCalledWith({ lat: 1, lng: 2 });
        expect(getParcelInfo).toHaveBeenCalledWith('987-654-321');
        expect(getNearestAddress).not.toHaveBeenCalled();
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
      await waitFor(() => {
        const addressSearchButton = getByTestId('address-search-button');
        expect(addressSearchButton).not.toHaveAttribute('disabled');
        addressSearchButton.click();
      });

      const toast = await findByText(
        'Unable to find parcel identifier (PID) for the searched location. A property must have a PID to be added to PSP, ensure this property has a PID.',
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
      await waitFor(() => {
        const addressSearchButton = getByTestId('address-search-button');
        expect(addressSearchButton).not.toHaveAttribute('disabled');
        addressSearchButton.click();
      });

      const toast = await findByText(
        'Property search failed. Please check your search criteria and try again. If this error persists, contact the Help Desk.',
      );
      expect(toast).toBeInTheDocument();
    });
  });
  describe('move marker functionality', () => {
    beforeEach(() => {
      history.push('/mapview?sidebar=true&sidebarContext=addBareLand');
      isPidAvailable.mockResolvedValue({ available: true });
      jest.clearAllMocks();
    });
    it('queries for data as expected when the marker is placed on the map', async () => {
      findOneWhereContains.mockResolvedValueOnce({
        features: [{ properties: { PID: '987-654-321' } }],
      });

      renderContainer({
        store: {
          ...(defaultStore.getState() as any),
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

      await waitFor(() => {
        expect(findOneWhereContains).toHaveBeenCalledWith({ lat: 1, lng: 2 });
        expect(getParcelInfo).toHaveBeenCalledWith('987-654-321');
        expect(getNearestAddress).toHaveBeenCalledWith({ lat: 1, lng: 2 });
      });
    });
  });
  describe('form functionality', () => {
    beforeEach(() => {
      history.push('/mapview?sidebar=true&sidebarContext=addBareLand');
      isPidAvailable.mockResolvedValue({ available: true });
      jest.clearAllMocks();
    });

    it('the cancel and submit buttons are disabled by default', async () => {
      const { findByText } = renderContainer({});

      const cancelButton = (await findByText('Cancel')).closest('button') as HTMLButtonElement;
      const submitButton = (await findByText('Save')).closest('button') as HTMLButtonElement;

      expect(cancelButton).toBeDisabled();
      expect(submitButton).toBeDisabled();
    });

    it('the cancel and submit buttons are enabled when a valid property is found', async () => {
      findByPid.mockResolvedValueOnce(mockParcelLayerResponse);
      const { container, getByTestId, findByTestId, findByText } = renderContainer({});

      await findByTestId('pid-search-button');
      await fillInput(container, 'searchPid', '123-456-789');
      fireEvent.click(getByTestId('pid-search-button'));
      const cancelButton = (await findByText('Cancel')).closest('button') as HTMLButtonElement;
      const submitButton = (await findByText('Save')).closest('button') as HTMLButtonElement;

      expect(cancelButton).not.toBeDisabled();
      expect(submitButton).not.toBeDisabled();
    });

    it('the submit button displays a toast message', async () => {
      findByPid.mockResolvedValueOnce(mockParcelLayerResponse);
      const { container, getByTestId, findByTestId, findByText } = renderContainer({});

      await findByTestId('pid-search-button');
      await fillInput(container, 'searchPid', '123-456-789');
      fireEvent.click(getByTestId('pid-search-button'));
      const submitButton = (await findByText('Save')).closest('button') as HTMLButtonElement;
      expect(submitButton).not.toBeDisabled();
      fireEvent.click(submitButton);

      const message = await findByText('Save Property Placeholder message');
      expect(message).toBeInTheDocument();
    });

    it('the tabs are disabled by default', async () => {
      const { findByText } = renderContainer({});

      const propertyTab = await findByText('Property');

      expect(propertyTab).toHaveClass('disabled');
    });

    it('the tabs are enabled when a valid property is found', async () => {
      findByPid.mockResolvedValueOnce(mockParcelLayerResponse);
      const { container, getByTestId, findByTestId, findByText } = renderContainer({});

      await findByTestId('pid-search-button');
      await fillInput(container, 'searchPid', '123-456-789');
      fireEvent.click(getByTestId('pid-search-button'));
      const propertyTab = await findByText('Property');

      expect(propertyTab).not.toHaveClass('disabled');
    });
  });
  describe('duplicate pid functionality', () => {
    const { open } = window;
    beforeAll(() => {
      delete (window as any).open;
      window.open = jest.fn();
    });

    afterAll(() => {
      window.open = open;
    });

    beforeEach(() => {
      history.push('/mapview?sidebar=true&sidebarContext=addBareLand');
      isPidAvailable.mockResolvedValue({ available: false });
      findByPid.mockResolvedValueOnce(mockParcelLayerResponse);
    });

    it('displays the duplicate pid modal if a duplicate pid is detected.', async () => {
      const { container, getByTestId } = renderContainer({});
      await fillInput(container, 'searchPid', '123-456-789');
      act(() => {
        fireEvent.click(getByTestId('pid-search-button'));
      });
      const modal = await screen.findByText(/The parcel identifier \(PID\) 123-456-789/g);
      expect(modal).toBeInTheDocument();
    });
    it('does not display the modal text if there is no duplicate pid', async () => {
      isPidAvailable.mockResolvedValue({ available: true });
      const { container, getByTestId } = renderContainer({});
      await fillInput(container, 'searchPid', '123-456-789');
      act(() => {
        fireEvent.click(getByTestId('pid-search-button'));
      });
      const modal = await screen.queryByText(/The parcel identifier \(PID\) 123-456-789/g);
      expect(modal).not.toBeInTheDocument();
    });
    it('opens the expected url in a new window when Yes clicked', async () => {
      const { container, getByTestId } = renderContainer({});
      await fillInput(container, 'searchPid', '123-456-789');
      act(() => {
        fireEvent.click(getByTestId('pid-search-button'));
      });
      await screen.findByText(/The parcel identifier \(PID\) 123-456-789/g);
      const yesButton = screen.getByText('Yes');
      act(() => {
        yesButton.click();
      });
      expect(window.open).toHaveBeenCalled();
    });
    it('closes the modal when no is clicked', async () => {
      const { container, getByTestId } = renderContainer({});
      await fillInput(container, 'searchPid', '123-456-789');
      act(() => {
        fireEvent.click(getByTestId('pid-search-button'));
      });
      await screen.findByText(/The parcel identifier \(PID\) 123-456-789/g);
      const noButton = screen.getByText('No');
      act(() => {
        noButton.click();
      });
      expect(window.open).not.toHaveBeenCalled();
    });
  });
  describe('populate search form functionality', () => {
    beforeEach(() => {
      history.push('/mapview?sidebar=true&sidebarContext=addBareLand');
      isPidAvailable.mockResolvedValue({ available: true });
      findByPid.mockResolvedValueOnce(mockParcelLayerResponse);
    });

    it('does not call pid search field with original store value', async () => {
      store.dispatch(
        saveParcelLayerData({ e: {} as any, data: mockParcelLayerResponse.features[0].properties }),
      );
      renderContainer({ store: store });
      await screen.findByText('Search for a Property');

      expect(findByPid).not.toHaveBeenCalledWith('');
    });
    it('populates the pid search field', async () => {
      renderContainer({ store: store });
      await screen.findByText('Search for a Property');
      store.dispatch(
        saveParcelLayerData({ e: {} as any, data: mockParcelLayerResponse.features[0].properties }),
      );
      const pidInput = await screen.findByDisplayValue('123-456-789');
      expect(pidInput).toBeInTheDocument();
    });

    it('calls findByPid with the expected pid', async () => {
      renderContainer({ store: store });
      await screen.findByText('Search for a Property');
      store.dispatch(
        saveParcelLayerData({ e: {} as any, data: mockParcelLayerResponse.features[0].properties }),
      );
      await screen.findByDisplayValue('123-456-789');
      expect(findByPid).toHaveBeenCalledWith('123456789');
    });
  });
});
