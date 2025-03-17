import axios from 'axios';
import { createMemoryHistory } from 'history';

import { Claims } from '@/constants';
import { IGeocoderResponse } from '@/hooks/pims-api/interfaces/IGeocoder';
import { useGeocoderRepository } from '@/hooks/useGeocoderRepository';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  cleanup,
  fillInput,
  getByName,
  render,
  RenderOptions,
  screen,
  userEvent,
} from '@/utils/test-utils';

import { PropertyFilter } from '.';
import { defaultPropertyFilter, IPropertyFilter } from './IPropertyFilter';
import { IPropertyFilterProps } from './PropertyFilter';
import { SearchToggleOption } from './PropertySearchToggle';

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
let history = createMemoryHistory();

describe('MapFilterBar', () => {
  vi.mocked(mockedAxios.get).mockImplementationOnce(() => Promise.resolve({}));

  const setup = (
    renderOptions: RenderOptions & {
      props?: Partial<IPropertyFilterProps>;
    } = {},
  ) => {
    const utils = render(
      <PropertyFilter
        propertyFilter={renderOptions?.props?.propertyFilter ?? defaultPropertyFilter}
        useGeocoder={renderOptions?.props?.useGeocoder ?? true}
        defaultFilter={defaultPropertyFilter}
        onChange={onFilterChange}
        toggle={renderOptions?.props?.toggle ?? SearchToggleOption.Map}
      />,
      {
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [Claims.PROPERTY_VIEW],
        history,
        ...renderOptions,
      },
    );

    const searchButton = screen.getByTitle('search');
    const resetButton = screen.getByTitle('reset-button');
    const searchByDropdown = getByName('searchBy') as HTMLSelectElement;
    return { ...utils, searchButton, resetButton, searchByDropdown };
  };

  beforeEach(() => {
    import.meta.env.VITE_TENANT = 'MOTI';
  });

  afterEach(() => {
    delete import.meta.env.VITE_TENANT;
    cleanup();
  });

  it('renders correctly', () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('disables the search button if there are no pid, pin or address', async () => {
    const { searchButton } = setup({
      props: {
        propertyFilter: { ...defaultPropertyFilter, searchBy: 'address' },
      },
    });

    expect(searchButton).toBeDisabled();
  });

  it('disables the search button if there is an invalid pid', async () => {
    const { searchButton } = setup({
      props: {
        propertyFilter: { ...defaultPropertyFilter, searchBy: 'pid' },
      },
    });

    expect(searchButton).toBeDisabled();
  });

  it('disables the search button if there is an invalid pin', async () => {
    const { searchButton } = setup({
      props: {
        propertyFilter: { ...defaultPropertyFilter, searchBy: 'pin' },
      },
    });

    expect(searchButton).toBeDisabled();
  });

  it('shows search by PID option', async () => {
    setup({
      props: {
        propertyFilter: { ...defaultPropertyFilter, searchBy: 'pid' },
      },
    });

    let pid = screen.getByPlaceholderText(/Enter a PID/i);
    expect(pid).toBeVisible();

    await act(async () => {
      userEvent.paste(pid, 'aaa');
    });

    expect(screen.getByPlaceholderText(/Enter a PID/i)).toBeVisible();
  });

  it('shows search by PIN option', async () => {
    setup({
      props: {
        propertyFilter: { ...defaultPropertyFilter, searchBy: 'pin' },
      },
    });

    let pin = screen.getByPlaceholderText(/Enter a PIN/i);
    expect(pin).toBeVisible();

    await act(async () => {
      userEvent.paste(pin, 'aaa');
    });

    expect(screen.getByPlaceholderText(/Enter a PIN/i)).toBeVisible();
  });

  it('shows search by Plan Number option', async () => {
    setup({
      props: {
        propertyFilter: { ...defaultPropertyFilter, searchBy: 'planNumber' },
      },
    });

    expect(screen.getByPlaceholderText(/Enter a plan number/i)).toBeVisible();
  });

  it('shows search by Historical File Number option', async () => {
    setup({
      props: {
        propertyFilter: { ...defaultPropertyFilter, searchBy: 'historical' },
      },
    });

    expect(screen.getByPlaceholderText('Enter a historical file#', { exact: false })).toBeVisible();
  });

  it('clears the form when changing the search by option', async () => {
    const { searchByDropdown } = setup({
      props: {
        propertyFilter: { ...defaultPropertyFilter, searchBy: 'pid' },
      },
    });

    let pid = screen.getByPlaceholderText(/Enter a PID/i);
    expect(pid).toBeVisible();

    await act(async () => {
      userEvent.paste(pid, '9999');
    });

    await act(async () => {
      userEvent.selectOptions(searchByDropdown, 'address');
    });

    pid = screen.queryByPlaceholderText(/Enter a PID/i);
    expect(pid).toBeNull();

    await act(async () => {
      userEvent.selectOptions(searchByDropdown, 'pid');
    });

    pid = screen.getByPlaceholderText(/Enter a PID/i);
    expect(pid).toBeVisible();
    expect(pid).toHaveValue('');
  });

  it('submits the form if there is pid/pin for address when using the geocoder', async () => {
    // Arrange
    const { container, searchButton } = setup({
      props: {
        propertyFilter: { ...defaultPropertyFilter, searchBy: 'address' },
      },
    });
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
    await act(async () => {
      fillInput(container, 'address', 'Victoria');
    });

    const addressSuggestion = await screen.findByText('510 Catherine St', { exact: false });

    await act(async () => {
      userEvent.click(addressSuggestion!);
    });

    await act(async () => {
      userEvent.click(searchButton);
    });

    // Assert
    expect(onFilterChange).toHaveBeenCalled();
  });

  it('submits the form if there is pid/pin for address, and handles multiple pids in the geocoder response.', async () => {
    // Arrange
    const { container, searchButton } = setup({
      props: {
        propertyFilter: { ...defaultPropertyFilter, searchBy: 'address' },
      },
    });
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
    await act(async () => {
      fillInput(container, 'address', 'Victoria');
    });

    const addressButton = await screen.findByText('510 Catherine St', { exact: false });

    await act(async () => {
      userEvent.click(addressButton!);
    });

    expect(
      await screen.findByText('Warning, multiple PIDs found for this address', { exact: false }),
    ).toBeVisible();

    await act(async () => {
      userEvent.click(searchButton);
    });

    // Assert
    expect(onFilterChange).toHaveBeenCalled();
  });

  it('submits if the address is set and useGeocoder is false', async () => {
    // Arrange
    const { container, searchButton } = setup({
      props: {
        propertyFilter: { ...defaultPropertyFilter, searchBy: 'address' },
        useGeocoder: false,
      },
    });

    // Act
    // Enter values on the form fields, then click the Search button
    await act(async () => {
      fillInput(container, 'address', 'Victoria');
    });

    expect(await screen.findByDisplayValue('Victoria')).toBeVisible();
    await act(async () => {
      userEvent.click(searchButton);
    });

    // Assert
    expect(onFilterChange).toHaveBeenCalled();
  });

  it('resets the form values when reset button is clicked', async () => {
    const { container, resetButton } = setup({
      props: {
        propertyFilter: { ...defaultPropertyFilter, searchBy: 'address' },
      },
    });

    // Act
    // Enter values on the form fields, then click the Search button
    await act(async () => {
      fillInput(container, 'address', 'Victoria');
    });
    await act(async () => {
      userEvent.click(resetButton);
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

  it('searches by PID', async () => {
    const { container, searchButton } = setup({
      props: {
        propertyFilter: { ...defaultPropertyFilter, searchBy: 'pid' },
      },
    });

    // Enter values on the form fields, then click the Search button
    await act(async () => {
      fillInput(container, 'pid', '123');
    });
    await act(async () => {
      userEvent.click(searchButton);
    });

    expect(onFilterChange).toHaveBeenCalledWith<[IPropertyFilter]>({
      pid: '123',
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

  it('searches by PIN', async () => {
    const { container, searchButton } = setup({
      props: {
        propertyFilter: { ...defaultPropertyFilter, searchBy: 'pin' },
      },
    });

    // Enter values on the form fields, then click the Search button
    await act(async () => {
      fillInput(container, 'pin', '999999');
    });
    await act(async () => {
      userEvent.click(searchButton);
    });

    expect(onFilterChange).toHaveBeenCalledWith<[IPropertyFilter]>({
      pid: '',
      pin: '999999',
      planNumber: '',
      address: '',
      searchBy: 'pin',
      page: undefined,
      quantity: undefined,
      latitude: '',
      longitude: '',
      historical: '',
      ownership: 'isCoreInventory,isPropertyOfInterest,isOtherInterest',
    });
  });

  it.each([
    ['The map is the active page', SearchToggleOption.Map, 'list-view', '/properties/list'],
    ['Property List View is the active page', SearchToggleOption.List, 'map-view', '/mapview'],
  ])(
    'navigates between the map and the property list view when toggle button is clicked - %s',
    async (
      _: string,
      toggleValue: SearchToggleOption,
      buttonTitle: string,
      expectedRoute: string,
    ) => {
      setup({ props: { toggle: toggleValue } });

      const iconButton = screen.getByTitle(buttonTitle);
      expect(iconButton).toBeVisible();

      await act(async () => {
        userEvent.click(iconButton);
      });

      expect(history.location.pathname).toBe(expectedRoute);
    },
  );
});
