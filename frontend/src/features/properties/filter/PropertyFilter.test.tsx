import { useKeycloak } from '@react-keycloak/web';
import { cleanup, fireEvent, render, waitFor } from '@testing-library/react';
import axios from 'axios';
import * as API from 'constants/API';
import { createMemoryHistory } from 'history';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import filterSlice from 'store/slices/filter/filterSlice';
import { ILookupCode, lookupCodesSlice } from 'store/slices/lookupCodes';
import { TenantProvider } from 'tenants';
import { fillInput } from 'utils/test-utils';

import { PropertyFilter } from '.';
import { IPropertyFilter } from './IPropertyFilter';

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

const defaultFilter: IPropertyFilter = {
  searchBy: 'pinOrPid',
  pinOrPid: '',
  address: '',
};

const getUiElement = (filter: IPropertyFilter, showAllOrganizationSelect = true) => (
  <TenantProvider>
    <Provider store={getStore(filter)}>
      <Router history={history}>
        <PropertyFilter defaultFilter={filter} onChange={onFilterChange} />
      </Router>
    </Provider>
  </TenantProvider>
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
    const { container } = render(getUiElement(defaultFilter));
    expect(container.firstChild).toMatchSnapshot();
  });

  it('submits correct values', async () => {
    // Arrange
    mockKeycloak(['admin-properties']);

    const { container } = render(getUiElement(defaultFilter));
    const submit = container.querySelector('button[type="submit"]');

    // Act
    // Enter values on the form fields, then click the Search button
    await waitFor(() => fillInput(container, 'address', 'Victoria'));

    await waitFor(() => {
      fireEvent.click(submit!);
    });

    // Assert
    expect(onFilterChange).toBeCalledWith({
      pinOrPid: '',
      address: '',
      searchBy: 'pinOrPid',
    });
  });

  it('loads filter values if provided', () => {
    const providedFilter: IPropertyFilter = {
      pinOrPid: 'mockPid',
      searchBy: 'address',
      address: 'mockaddress',
    };
    const { getByText } = render(getUiElement(providedFilter));
    expect(getByText('Address')).toBeVisible();
  });

  it('resets values when reset button is clicked', async () => {
    const { container, getByTestId } = render(getUiElement(defaultFilter));

    // Act
    // Enter values on the form fields, then click the Search button
    await waitFor(() => fillInput(container, 'address', 'Victoria'));
    await waitFor(() => {
      fireEvent.click(getByTestId('reset-button'));
    });
    expect(onFilterChange).toBeCalledWith<[IPropertyFilter]>({
      pinOrPid: '',
      address: '',
      searchBy: 'pinOrPid',
    });
  });
});
