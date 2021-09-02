import { useKeycloak } from '@react-keycloak/web';
import { cleanup, fireEvent, render, waitFor } from '@testing-library/react';
import axios from 'axios';
import * as API from 'constants/API';
import { usePropertyNames } from 'features/properties/common/slices/usePropertyNames';
import { createMemoryHistory } from 'history';
import * as MOCK from 'mocks/filterDataMock';
import React from 'react';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import filterSlice from 'store/slices/filter/filterSlice';
import { ILookupCode, lookupCodesSlice } from 'store/slices/lookupCodes';
import { TenantProvider } from 'tenants';
import { fillInput } from 'utils/test-utils';

import propertyNameSlice from '../common/slices/propertyNameSlice';
import { PropertyFilter } from '.';
import { IPropertyFilter } from './IPropertyFilter';

const onFilterChange = jest.fn<void, [IPropertyFilter]>();
//prevent web calls from being made during tests.
jest.mock('axios');
jest.mock('@react-keycloak/web');
jest.mock('features/properties/common/slices/usePropertyNames');

const fetchPropertyNames = jest.fn(() => Promise.resolve(['test']));
(usePropertyNames as any).mockImplementation(() => ({
  fetchPropertyNames,
}));

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
    { id: 1, name: 'organizationVal', isDisabled: false, type: API.ORGANIZATION_CODE_SET_NAME },
    { id: 2, name: 'disabledOrganization', isDisabled: true, type: API.ORGANIZATION_CODE_SET_NAME },
    { id: 1, name: 'roleVal', isDisabled: false, type: API.ROLE_CODE_SET_NAME },
    { id: 2, name: 'disabledRole', isDisabled: true, type: API.ROLE_CODE_SET_NAME },
    {
      id: 1,
      name: 'Core Operational',
      isDisabled: false,
      type: API.PROPERTY_CLASSIFICATION_CODE_SET_NAME,
    },
    {
      id: 2,
      name: 'Core Strategic',
      isDisabled: false,
      type: API.PROPERTY_CLASSIFICATION_CODE_SET_NAME,
    },
    {
      id: 5,
      name: 'Disposed',
      isDisabled: false,
      type: API.PROPERTY_CLASSIFICATION_CODE_SET_NAME,
    },
    {
      id: 6,
      name: 'Demolished',
      isDisabled: false,
      type: API.PROPERTY_CLASSIFICATION_CODE_SET_NAME,
    },
    {
      id: 7,
      name: 'Subdivided',
      isDisabled: false,
      type: API.PROPERTY_CLASSIFICATION_CODE_SET_NAME,
    },
  ] as ILookupCode[],
};

const getStore = (filter: any) =>
  mockStore({
    [filterSlice.name]: filter,
    [propertyNameSlice.name]: ['test'],
    [lookupCodesSlice.name]: lCodes,
  });

const defaultFilter: IPropertyFilter = {
  searchBy: 'pid',
  pid: '',
  address: '',
  pin: '',
  location: '',
};

const getUiElement = (filter: IPropertyFilter, showAllOrganizationSelect = true) => (
  <TenantProvider>
    <Provider store={getStore(filter)}>
      <Router history={history}>
        <PropertyFilter
          defaultFilter={filter}
          organizationLookupCodes={MOCK.mockOrganizationLookups}
          adminAreaLookupCodes={MOCK.mockAdministrativeAreaLookups}
          onChange={onFilterChange}
          showAllOrganizationSelect={showAllOrganizationSelect}
        />
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
      pid: '',
      address: '',
      pin: '',
      location: '',
      searchBy: 'pid',
    });
  });

  it('loads filter values if provided', () => {
    const providedFilter: IPropertyFilter = {
      pid: 'mockPid',
      searchBy: 'address',
      address: 'mockaddress',
      pin: '',
      location: '',
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
      pid: '',
      address: '',
      pin: '',
      location: '',
      searchBy: 'pid',
    });
  });
});
