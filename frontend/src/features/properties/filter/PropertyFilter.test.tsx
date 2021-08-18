import { useKeycloak } from '@react-keycloak/web';
import { cleanup, fireEvent, render, waitFor } from '@testing-library/react';
import axios from 'axios';
import * as API from 'constants/API';
import { Classifications } from 'constants/classifications';
import { usePropertyNames } from 'features/properties/common/slices/usePropertyNames';
import { createMemoryHistory } from 'history';
import * as MOCK from 'mocks/filterDataMock';
import React from 'react';
import { act } from 'react-dom/test-utils';
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
  searchBy: 'name',
  pid: '',
  address: '',
  administrativeArea: '',
  propertyType: '',
  organizations: '',
  classificationId: '',
  minLotSize: '',
  maxLotSize: '',
  rentableArea: '',
  name: '',
};

const getUiElement = (filter: IPropertyFilter, showAllOrganizationSelect = true) => (
  <TenantProvider>
    <Provider store={getStore(filter)}>
      <Router history={history}>
        <PropertyFilter
          defaultFilter={filter}
          organizationLookupCodes={MOCK.ORGANIZATIONS}
          adminAreaLookupCodes={MOCK.ADMINISTRATIVEAREAS}
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

  it('renders only my organizations if showAllOrganizations not set', () => {
    mockKeycloak(['property-view']);
    // Capture any changes
    const { getByPlaceholderText } = render(getUiElement(defaultFilter, false));
    expect(getByPlaceholderText('My Organizations')).toBeVisible();
  });

  it('submits correct values', async () => {
    // Arrange
    mockKeycloak(['admin-properties']);

    const { container } = render(getUiElement(defaultFilter));
    const classificationId = container.querySelector('select[name="classificationId"]');
    const submit = container.querySelector('button[type="submit"]');

    // Act
    // Enter values on the form fields, then click the Search button
    await waitFor(() => fillInput(container, 'administrativeArea', 'Victoria', 'typeahead'));

    await waitFor(() => {
      fireEvent.change(classificationId!, {
        target: {
          value: `${Classifications.CoreOperational}`,
        },
      });
    });

    await waitFor(() => {
      fireEvent.click(submit!);
    });

    // Assert
    expect(onFilterChange).toBeCalledWith<[IPropertyFilter]>({
      pid: '',
      address: '',
      administrativeArea: 'Victoria',
      organizations: '',
      classificationId: `${Classifications.CoreOperational}`,
      minLotSize: '',
      maxLotSize: '',
      name: '',
      searchBy: 'name',
      propertyType: '',
      rentableArea: '',
    });
  });

  it('loads filter values if provided', () => {
    const providedFilter: IPropertyFilter = {
      pid: 'mockPid',
      searchBy: 'address',
      address: 'mockaddress',
      administrativeArea: 'mockAdministrativeArea',
      organizations: '2',
      classificationId: `${Classifications.CoreStrategic}`,
      minLotSize: '10',
      maxLotSize: '20',
      rentableArea: '0',
    };
    const { getByText } = render(getUiElement(providedFilter));
    expect(getByText('Address')).toBeVisible();
    expect(getByText('Core Operational')).toBeVisible();
  });

  it('loads filter values if array based organizations is provided', () => {
    const providedFilter: IPropertyFilter = {
      pid: 'mockPid',
      searchBy: 'address',
      address: 'mockaddress',
      administrativeArea: 'mockAdministrativeArea',
      organizations: ['2'] as any,
      classificationId: `${Classifications.CoreStrategic}`,
      minLotSize: '10',
      maxLotSize: '20',
      rentableArea: '0',
    };
    const { getByDisplayValue } = render(getUiElement(providedFilter));
    expect(getByDisplayValue('HTLH')).toBeVisible();
  });

  it('loads filter values if empty organizations array is provided', () => {
    const providedFilter: IPropertyFilter = {
      pid: 'mockPid',
      searchBy: 'address',
      address: 'mockaddress',
      administrativeArea: 'mockAdministrativeArea',
      organizations: [] as any,
      classificationId: `${Classifications.CoreStrategic}`,
      minLotSize: '10',
      maxLotSize: '20',
      rentableArea: '0',
    };
    const { container } = render(getUiElement(providedFilter));
    const organizations = container.querySelector('input[name="organizations"]');
    expect(organizations).toHaveValue('');
  });

  it('resets values when reset button is clicked', async () => {
    const { container, getByTestId } = render(getUiElement(defaultFilter));
    const classificationId = container.querySelector('select[name="classificationId"]');

    // Act
    // Enter values on the form fields, then click the Search button
    await waitFor(() => fillInput(container, 'administrativeArea', 'Victoria', 'typeahead'));

    await waitFor(() => {
      fireEvent.change(classificationId!, {
        target: {
          value: '1',
        },
      });
    });
    await waitFor(() => {
      fireEvent.click(getByTestId('reset-button'));
    });
    expect(onFilterChange).toBeCalledWith<[IPropertyFilter]>({
      pid: '',
      address: '',
      administrativeArea: '',
      organizations: '',
      classificationId: '',
      minLotSize: '',
      maxLotSize: '',
      name: '',
      searchBy: 'name',
      propertyType: '',
      rentableArea: '',
    });
  });

  it('searches for property names', async () => {
    const { container } = render(getUiElement({ ...defaultFilter, includeAllProperties: true }));
    const nameField = container.querySelector('input[id="name-field"]');
    fireEvent.change(nameField!, {
      target: {
        value: 'test',
      },
    });
    await waitFor(() => {
      expect(fetchPropertyNames).toHaveBeenCalled();
    });
  });

  it('disables the property name and organizations fields when All Government is selected', async () => {
    await act(async () => {
      const { container, getByPlaceholderText } = render(
        getUiElement({ ...defaultFilter, includeAllProperties: true }),
      );
      expect(getByPlaceholderText('Property name')).toBeDisabled();
      expect(container.querySelector('input[name="organizations"]')).toBeDisabled();
    });
  });

  it('enables the property name and organizations fields when My Organizations is selected', async () => {
    await act(async () => {
      const { container, getByPlaceholderText } = render(
        getUiElement({ ...defaultFilter, includeAllProperties: false }),
      );
      expect(getByPlaceholderText('Property name')).not.toBeDisabled();
      expect(container.querySelector('input[name="organizations"]')).not.toBeDisabled();
    });
  });
});
