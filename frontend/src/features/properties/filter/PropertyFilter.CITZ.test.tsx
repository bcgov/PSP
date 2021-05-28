import React from 'react';
import { render, wait, fireEvent, cleanup, act, screen } from '@testing-library/react';
import { PropertyFilter } from './';
import * as MOCK from 'mocks/filterDataMock';
import axios from 'axios';
import { useKeycloak } from '@react-keycloak/web';
import { createMemoryHistory } from 'history';
import { IPropertyFilter } from './IPropertyFilter';
import { Router } from 'react-router-dom';
import { Provider } from 'react-redux';
import thunk from 'redux-thunk';
import configureMockStore from 'redux-mock-store';
import { usePropertyNames } from 'features/properties/common/slices/usePropertyNames';
import * as API from 'constants/API';
import { fillInput } from 'utils/testUtils';
import { ILookupCode, lookupCodesSlice } from 'store/slices/lookupCodes';
import filterSlice from 'store/slices/filter/filterSlice';
import propertyNameSlice from '../common/slices/propertyNameSlice';
import { TenantProvider } from 'tenants';

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
        agencies: ['1'],
      },
    },
  });
};
const mockStore = configureMockStore([thunk]);
let history = createMemoryHistory();

const lCodes = {
  lookupCodes: [
    { name: 'agencyVal', id: '1', isDisabled: false, type: API.AGENCY_CODE_SET_NAME },
    { name: 'disabledAgency', id: '2', isDisabled: true, type: API.AGENCY_CODE_SET_NAME },
    { name: 'roleVal', id: '1', isDisabled: false, type: API.ROLE_CODE_SET_NAME },
    { name: 'disabledRole', id: '2', isDisabled: true, type: API.ROLE_CODE_SET_NAME },
    {
      name: 'Core Operational',
      id: '0',
      isDisabled: false,
      type: API.PROPERTY_CLASSIFICATION_CODE_SET_NAME,
    },
    {
      name: 'Core Strategic',
      id: '1',
      isDisabled: false,
      type: API.PROPERTY_CLASSIFICATION_CODE_SET_NAME,
    },
    {
      name: 'Surplus Active',
      id: '2',
      isDisabled: false,
      type: API.PROPERTY_CLASSIFICATION_CODE_SET_NAME,
    },
    {
      name: 'Surplus Encumbered',
      id: '3',
      isDisabled: false,
      type: API.PROPERTY_CLASSIFICATION_CODE_SET_NAME,
    },
    {
      name: 'Disposed',
      id: '4',
      isDisabled: false,
      type: API.PROPERTY_CLASSIFICATION_CODE_SET_NAME,
    },
    {
      name: 'Demolished',
      id: '5',
      isDisabled: false,
      type: API.PROPERTY_CLASSIFICATION_CODE_SET_NAME,
    },
    {
      name: 'Subdivided',
      id: '6',
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
  projectNumber: '',
  agencies: '',
  classificationId: '',
  minLotSize: '',
  maxLotSize: '',
  rentableArea: '',
  name: '',
};

const getUiElement = (filter: IPropertyFilter, showAllAgencySelect = true) => (
  <TenantProvider>
    <Provider store={getStore(filter)}>
      <Router history={history}>
        <PropertyFilter
          defaultFilter={filter}
          agencyLookupCodes={MOCK.AGENCIES}
          adminAreaLookupCodes={MOCK.ADMINISTRATIVEAREAS}
          onChange={onFilterChange}
          showAllAgencySelect={showAllAgencySelect}
        />
      </Router>
    </Provider>
  </TenantProvider>
);

describe('[ CITZ ] MapFilterBar', () => {
  mockedAxios.get.mockImplementationOnce(() => Promise.resolve({}));

  beforeEach(() => {
    process.env.REACT_APP_TENANT = 'CITZ';
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

  it('renders only my agencies if showAllAgencies not set', () => {
    mockKeycloak(['property-view']);
    // Capture any changes
    const { getByPlaceholderText } = render(getUiElement(defaultFilter, false));
    expect(getByPlaceholderText('My Agencies')).toBeVisible();
  });

  it('submits correct values', async () => {
    // Arrange
    mockKeycloak(['admin-properties']);

    const { container } = render(getUiElement(defaultFilter));
    const classificationId = container.querySelector('select[name="classificationId"]');
    const submit = container.querySelector('button[type="submit"]');

    // Act
    // Enter values on the form fields, then click the Search button
    await fillInput(container, 'administrativeArea', 'Victoria', 'typeahead');

    await wait(() => {
      fireEvent.change(classificationId!, {
        target: {
          value: '0',
        },
      });
    });

    await wait(() => {
      fireEvent.click(submit!);
    });

    // Assert
    expect(onFilterChange).toBeCalledWith<[IPropertyFilter]>({
      pid: '',
      address: '',
      administrativeArea: 'Victoria',
      projectNumber: '',
      agencies: '',
      classificationId: '0',
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
      projectNumber: '',
      agencies: '2',
      classificationId: '0',
      minLotSize: '10',
      maxLotSize: '20',
      inSurplusPropertyProgram: true,
      rentableArea: '0',
    };
    const { getByText } = render(getUiElement(providedFilter));
    expect(getByText('Address')).toBeVisible();
    expect(getByText('Core Operational')).toBeVisible();
  });

  it('loads filter values if array based agencies is provided', () => {
    const providedFilter: IPropertyFilter = {
      pid: 'mockPid',
      searchBy: 'address',
      address: 'mockaddress',
      administrativeArea: 'mockAdministrativeArea',
      projectNumber: '',
      agencies: ['2'] as any,
      classificationId: '0',
      minLotSize: '10',
      maxLotSize: '20',
      inSurplusPropertyProgram: true,
      rentableArea: '0',
    };
    const { getByDisplayValue } = render(getUiElement(providedFilter));
    expect(getByDisplayValue('HTLH')).toBeVisible();
  });

  it('loads filter values if empty agencies array is provided', () => {
    const providedFilter: IPropertyFilter = {
      pid: 'mockPid',
      searchBy: 'address',
      address: 'mockaddress',
      administrativeArea: 'mockAdministrativeArea',
      projectNumber: '',
      agencies: [] as any,
      classificationId: '0',
      minLotSize: '10',
      maxLotSize: '20',
      inSurplusPropertyProgram: true,
      rentableArea: '0',
    };
    const { container } = render(getUiElement(providedFilter));
    const agencies = container.querySelector('input[name="agencies"]');
    expect(agencies).toHaveValue('');
  });

  it('resets values when reset button is clicked', async () => {
    const { container, getByTestId } = render(getUiElement(defaultFilter));
    const classificationId = container.querySelector('select[name="classificationId"]');

    // Act
    // Enter values on the form fields, then click the Search button
    await fillInput(container, 'administrativeArea', 'Victoria', 'typeahead');

    await wait(() => {
      fireEvent.change(classificationId!, {
        target: {
          value: '1',
        },
      });
    });
    fireEvent.click(getByTestId('reset-button'));
    expect(onFilterChange).toBeCalledWith<[IPropertyFilter]>({
      pid: '',
      address: '',
      administrativeArea: '',
      projectNumber: '',
      agencies: '',
      classificationId: '',
      minLotSize: '',
      maxLotSize: '',
      name: '',
      searchBy: 'name',
      propertyType: '',
      rentableArea: '',
    });
  });

  it('displays the surplus properties window if clicked', async () => {
    const { getByText } = render(getUiElement(defaultFilter));
    const findMorePropertiesButton = getByText('Surplus Properties');
    act(() => {
      fireEvent.click(findMorePropertiesButton);
    });
    expect(await screen.findByText('Find available surplus properties')).toBeVisible();
  });

  it('hides the surplus properties window if closed', async () => {
    const { getByText } = render(getUiElement(defaultFilter));
    const findMorePropertiesButton = getByText('Surplus Properties');
    act(() => {
      fireEvent.click(findMorePropertiesButton);
    });
    await wait(() => {
      expect(screen.getByText('Find available surplus properties')).toBeVisible();
    });
    const closeFindMoreProperties = screen.getAllByTestId('close-button')[1];
    act(() => {
      fireEvent.click(closeFindMoreProperties);
    });
    await wait(() => {
      expect(screen.queryByText('Find available surplus properties')).toBeNull();
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
    await wait(() => {
      expect(fetchPropertyNames).toHaveBeenCalled();
    });
  });

  it('disables the property name and agencies fields when All Government is selected', () => {
    const { container, getByPlaceholderText } = render(
      getUiElement({ ...defaultFilter, includeAllProperties: true }),
    );
    expect(getByPlaceholderText('Property name')).toBeDisabled();
    expect(container.querySelector('input[name="agencies"]')).toBeDisabled();
  });

  it('enables the property name and agencies fields when My Agencies is selected', () => {
    const { container, getByPlaceholderText } = render(
      getUiElement({ ...defaultFilter, includeAllProperties: false }),
    );
    expect(getByPlaceholderText('Property name')).not.toBeDisabled();
    expect(container.querySelector('input[name="agencies"]')).not.toBeDisabled();
  });
});
