import { useKeycloak } from '@react-keycloak/web';
import axios from 'axios';
import { createMemoryHistory } from 'history';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import * as API from '@/constants/API';
import filterSlice from '@/store/slices/filter/filterSlice';
import { ILookupCode, lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, cleanup, fireEvent, render, waitFor } from '@/utils/test-utils';
import { fillInput } from '@/utils/test-utils';
import TestCommonWrapper from '@/utils/TestCommonWrapper';

import { PropertyFilter } from '.';
import { defaultPropertyFilter, IPropertyFilter } from './IPropertyFilter';

const onFilterChange = jest.fn<void, [IPropertyFilter]>();
//prevent web calls from being made during tests.
jest.mock('axios');
jest.mock('@react-keycloak/web');

const mockedAxios = axios as jest.Mocked<typeof axios>;
const mockKeycloak = (claims: string[]) => {
  (useKeycloak as jest.Mock).mockReturnValue({
    keycloak: {
      subject: 'test',
      userInfo: {
        roles: claims,
        organizations: ['1'],
      },
    },
  });
};
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

const getUiElement = (filter: IPropertyFilter, showAllOrganizationSelect = true) => (
  <TestCommonWrapper store={getStore(filter)} history={history}>
    <PropertyFilter useGeocoder={true} defaultFilter={filter} onChange={onFilterChange} />
  </TestCommonWrapper>
);

describe('MapFilterBar', () => {
  mockedAxios.get.mockImplementationOnce(() => Promise.resolve({}));

  beforeEach(() => {
    process.env.REACT_APP_TENANT = 'MOTI';
    history = createMemoryHistory();
    mockKeycloak([]);
  });

  afterEach(() => {
    delete process.env.REACT_APP_TENANT;
    cleanup();
  });

  it('renders correctly', () => {
    mockKeycloak(['property-view']);
    // Capture any changes
    const { container } = render(getUiElement(defaultPropertyFilter));
    expect(container.firstChild).toMatchSnapshot();
  });

  it('submits correct values', async () => {
    // Arrange
    mockKeycloak(['admin-properties']);

    const { container } = render(getUiElement(defaultPropertyFilter));
    const submit = container.querySelector('button[type="submit"]');

    // Act
    // Enter values on the form fields, then click the Search button
    await waitFor(() => fillInput(container, 'address', 'Victoria'));

    await act(async () => {
      fireEvent.click(submit!);
    });

    // Assert
    expect(onFilterChange).toBeCalledWith({
      address: '',
      latitude: '',
      longitude: '',
      page: undefined,
      pinOrPid: '',
      planNumber: '',
      quantity: undefined,
      searchBy: 'pinOrPid',
    });
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
    });
  });
});
