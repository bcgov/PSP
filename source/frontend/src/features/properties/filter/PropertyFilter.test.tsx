import { useKeycloak } from '@react-keycloak/web';
import axios, { AxiosResponse } from 'axios';
import { createMemoryHistory } from 'history';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import * as API from '@/constants/API';
import { useApiGeocoder } from '@/hooks/pims-api/useApiGeocoder';
import { ApiGen_Base_Page } from '@/models/api/generated/ApiGen_Base_Page';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import filterSlice from '@/store/slices/filter/filterSlice';
import { ILookupCode, lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  cleanup,
  fireEvent,
  getByDisplayValue,
  render,
  waitFor,
  screen,
} from '@/utils/test-utils';
import { fillInput } from '@/utils/test-utils';
import TestCommonWrapper from '@/utils/TestCommonWrapper';

import { PropertyFilter } from '.';
import { defaultPropertyFilter, IPropertyFilter } from './IPropertyFilter';
import { IGeocoderPidsResponse } from '@/hooks/pims-api/interfaces/IGeocoder';

const onFilterChange = vi.fn();
//prevent web calls from being made during tests.
vi.mock('axios');

vi.mock('@/hooks/pims-api/useApiGeocoder');

const mockApiGetSitePidsApi = vi.fn();
vi.mocked(useApiGeocoder).mockReturnValue({
  getSitePidsApi: mockApiGetSitePidsApi as (
    siteId: string,
  ) => Promise<AxiosResponse<IGeocoderPidsResponse, any>>,
} as unknown as ReturnType<typeof useApiGeocoder>);

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

const getUiElement = (
  filter: IPropertyFilter,
  showAllOrganizationSelect = true,
  useGeocoder = true,
) => (
  <TestCommonWrapper store={getStore(filter)} history={history}>
    <PropertyFilter useGeocoder={useGeocoder} defaultFilter={filter} onChange={onFilterChange} />
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
    //mockKeycloak(['property-view']);
    // Capture any changes
    const { container } = render(getUiElement(defaultPropertyFilter), {
      claims: ['property-view'],
    });
    expect(container.firstChild).toMatchSnapshot();
  });

  it('does not submit if there is no pid/pin for address', async () => {
    // Arrange

    const { container } = render(getUiElement({ ...defaultPropertyFilter, searchBy: 'address' }), {
      claims: ['admin-properties'],
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

  it('submits if address set and useGeocoder false', async () => {
    // Arrange

    const { container } = await render(
      getUiElement({ ...defaultPropertyFilter, searchBy: 'address' }, true, false),
      { claims: ['admin-properties'] },
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
    expect(onFilterChange).toBeCalledWith<[IPropertyFilter]>({
      pinOrPid: '',
      planNumber: '',
      address: '',
      searchBy: 'pinOrPid',
      page: undefined,
      quantity: undefined,
      latitude: '',
      longitude: '',
      ownership: 'isCoreInventory,isPropertyOfInterest,isOtherInterest',
    });
  });
});
