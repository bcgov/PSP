import axios from 'axios';
import { createMemoryHistory } from 'history';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import * as API from '@/constants/API';
import filterSlice from '@/store/slices/filter/filterSlice';
import { ILookupCode, lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, cleanup, fillInput, fireEvent, render, screen, waitFor } from '@/utils/test-utils';
import TestCommonWrapper from '@/utils/TestCommonWrapper';

import { IGeocoderResponse } from '@/hooks/pims-api/interfaces/IGeocoder';
import { useGeocoderRepository } from '@/hooks/useGeocoderRepository';
import { PropertyFilter } from '.';
import { defaultPropertyFilter, IPropertyFilter } from './IPropertyFilter';
import { Claims } from '@/constants';

const onFilterChange = vi.fn();
//prevent web calls from being made during tests.
vi.mock('axios');

vi.mock('@/hooks/useGeocoderRepository');
const mockApiGetSitePidsApi = vi.fn();
const searchAddressApi = vi.fn();
vi.mocked(useGeocoderRepository).mockReturnValue({
  getSitePids: mockApiGetSitePidsApi,
  isLoadingSitePids: false,
  searchAddress: searchAddressApi,
  isLoadingSearchAddress: false,
  getNearestToPoint: function (lng: number, lat: number): Promise<IGeocoderResponse> {
    throw new Error('Function not implemented.');
  },
  isLoadingNearestToPoint: false,
});

const mockedAxios = vi.mocked(axios);
const mockStore = configureMockStore([thunk]);
let history = createMemoryHistory();

const lCodes = {
  lookupCodes: [
    { id: 1, name: 'roleVal', isDisabled: false, type: API.ROLE_TYPES },
    { id: 2, name: 'disabledRole', isDisabled: true, type: API.ROLE_TYPES },
    {
      id: 1,
      name: 'Core Operational',
      isDisabled: false,
      type: API.PROPERTY_CLASSIFICATION_TYPES,
    },
    {
      id: 2,
      name: 'Core Strategic',
      isDisabled: false,
      type: API.PROPERTY_CLASSIFICATION_TYPES,
    },
    {
      id: 5,
      name: 'Disposed',
      isDisabled: false,
      type: API.PROPERTY_CLASSIFICATION_TYPES,
    },
    {
      id: 6,
      name: 'Demolished',
      isDisabled: false,
      type: API.PROPERTY_CLASSIFICATION_TYPES,
    },
    {
      id: 7,
      name: 'Subdivided',
      isDisabled: false,
      type: API.PROPERTY_CLASSIFICATION_TYPES,
    },
  ] as ILookupCode[],
};

const getStore = (filter: any) =>
  mockStore({
    [filterSlice.name]: filter,
    [lookupCodesSlice.name]: lCodes,
  });

const getUiElement = (filter: IPropertyFilter, useGeocoder = true) => (
  <TestCommonWrapper store={getStore(filter)} history={history}>
    <PropertyFilter
      propertyFilter={filter}
      useGeocoder={useGeocoder}
      defaultFilter={filter}
      onChange={onFilterChange}
    />
  </TestCommonWrapper>
);

describe('MapFilterBar', () => {
  vi.mocked(mockedAxios.get).mockImplementationOnce(() => Promise.resolve({}));

  beforeEach(() => {
    import.meta.env.VITE_TENANT = 'MOTI';
    history = createMemoryHistory();
  });

  afterEach(() => {
    delete import.meta.env.VITE_TENANT;
    cleanup();
  });

  it('renders correctly', () => {
    // Capture any changes
    const { container } = render(getUiElement(defaultPropertyFilter), {
      claims: [Claims.PROPERTY_VIEW],
    });
    expect(container.firstChild).toMatchSnapshot();
  });

  it('does not submit if there is no pid/pin for address', async () => {
    // Arrange

    const { container } = render(getUiElement({ ...defaultPropertyFilter, searchBy: 'address' }), {
      claims: [Claims.ADMIN_PROPERTIES],
    });
    const submit = container.querySelector('button[type="submit"]');

    // Act
    // Enter values on the form fields, then click the Search button
    await waitFor(() => fillInput(container, 'address', 'Victoria'));

    await act(async () => {
      fireEvent.click(submit!);
    });

    // Assert
    expect(onFilterChange).not.toHaveBeenCalled();
  });

  it('does submit if there is pid/pin for address.', async () => {
    // Arrange

    const { container } = render(getUiElement({ ...defaultPropertyFilter, searchBy: 'address' }), {
      claims: [Claims.ADMIN_PROPERTIES],
    });
    const submit = container.querySelector('button[type="submit"]');
    searchAddressApi.mockResolvedValue([
      {
        siteId: '3744cde1-18cd-4c45-9d39-472285295fd3',
        fullAddress: '510 Catherine St, Victoria, BC',
        address1: '510 Catherine St',
        administrativeArea: 'Victoria',
        provinceCode: 'BC',
        latitude: 48.4316251,
        longitude: -123.385134,
        score: 81,
      },
    ]);
    mockApiGetSitePidsApi.mockResolvedValue({
      siteId: '3744cde1-18cd-4c45-9d39-472285295fd3',
      pids: ['000115240'],
    });

    // Act
    // Enter values on the form fields, then click the Search button
    await waitFor(() => fillInput(container, 'address', 'Victoria'));

    const addressButton = await screen.findByText('510 Catherine St', { exact: false });

    await act(async () => {
      fireEvent.click(addressButton!);
    });

    await act(async () => {
      fireEvent.click(submit!);
    });

    // Assert
    expect(onFilterChange).toHaveBeenCalled();
  });

  it('does submit if there is pid/pin for address, and handles multiple pids in the response.', async () => {
    // Arrange

    const { container } = render(getUiElement({ ...defaultPropertyFilter, searchBy: 'address' }), {
      claims: [Claims.ADMIN_PROPERTIES],
    });
    const submit = container.querySelector('button[type="submit"]');
    searchAddressApi.mockResolvedValue([
      {
        siteId: '3744cde1-18cd-4c45-9d39-472285295fd3',
        fullAddress: '510 Catherine St, Victoria, BC',
        address1: '510 Catherine St',
        administrativeArea: 'Victoria',
        provinceCode: 'BC',
        latitude: 48.4316251,
        longitude: -123.385134,
        score: 81,
      },
    ]);
    mockApiGetSitePidsApi.mockResolvedValue({
      siteId: '3744cde1-18cd-4c45-9d39-472285295fd3',
      pids: ['000115240', '000115258'],
    });

    // Act
    // Enter values on the form fields, then click the Search button
    await waitFor(() => fillInput(container, 'address', 'Victoria'));

    const addressButton = await screen.findByText('510 Catherine St', { exact: false });

    await act(async () => {
      fireEvent.click(addressButton!);
    });

    await screen.findAllByText('Warning, multiple PIDs found for this address', {
      exact: false,
    })[0];

    await act(async () => {
      fireEvent.click(submit!);
    });

    // Assert
    expect(onFilterChange).toHaveBeenCalled();
  });

  it('submits if address set and useGeocoder false', async () => {
    // Arrange

    const { container } = await render(
      getUiElement({ ...defaultPropertyFilter, searchBy: 'address' }, false),
      { claims: [Claims.ADMIN_PROPERTIES] },
    );
    const submit = container.querySelector('button[type="submit"]');

    // Act
    // Enter values on the form fields, then click the Search button
    await waitFor(() => fillInput(container, 'address', 'Victoria'));

    await screen.findByDisplayValue('Victoria');
    await act(async () => {
      fireEvent.click(submit!);
    });

    // Assert
    expect(onFilterChange).toHaveBeenCalled();
  });

  it('resets values when reset button is clicked', async () => {
    const { container, getByTestId } = render(getUiElement(defaultPropertyFilter));

    // Act
    // Enter values on the form fields, then click the Search button
    await waitFor(() => fillInput(container, 'address', 'Victoria'));
    await waitFor(() => {
      fireEvent.click(getByTestId('reset-button'));
    });
    expect(onFilterChange).toHaveBeenCalledWith<[IPropertyFilter]>({
      pid: '',
      pin: '',
      planNumber: '',
      address: '',
      searchBy: 'pid',
      page: undefined,
      quantity: undefined,
      latitude: '',
      longitude: '',
      historical: '',
      ownership: 'isCoreInventory,isPropertyOfInterest,isOtherInterest',
    });
  });
});
