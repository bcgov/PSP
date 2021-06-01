import { useKeycloak } from '@react-keycloak/web';
import { cleanup, fireEvent, render, wait } from '@testing-library/react';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import * as API from 'constants/API';
import { mount } from 'enzyme';
import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import React from 'react';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { useAccessRequests } from 'store/slices/accessRequests/useAccessRequests';
import { ILookupCode, lookupCodesSlice } from 'store/slices/lookupCodes';
import { IGenericNetworkAction } from 'store/slices/network/interfaces';
import { networkSlice } from 'store/slices/network/networkSlice';
import { TenantProvider } from 'tenants';
import { fillInput } from 'utils/testUtils';

import * as actionTypes from '../../../constants/actionTypes';
import AccessRequestPage from './AccessRequestPage';

jest.mock('@react-keycloak/web');
(useKeycloak as jest.Mock).mockReturnValue({
  keycloak: {
    userInfo: {
      agencies: [1],
      roles: [],
    },
    subject: 'test',
  },
});

jest.mock('store/slices/accessRequests/useAccessRequests');
(useAccessRequests as jest.Mock).mockReturnValue({
  fetchCurrentAccessRequest: jest.fn(),
  addAccessRequest: jest.fn(),
});

Enzyme.configure({ adapter: new Adapter() });

const lCodes = {
  lookupCodes: [
    { name: 'One', id: '1', isDisabled: false, type: 'core operational' },
    { name: 'agencyVal', id: '1', isDisabled: false, type: API.AGENCY_CODE_SET_NAME },
    { name: 'disabledAgency', id: '2', isDisabled: true, type: API.AGENCY_CODE_SET_NAME },
    { name: 'roleVal', id: '1', isDisabled: false, type: API.ROLE_CODE_SET_NAME },
    { name: 'disabledRole', id: '2', isDisabled: true, type: API.ROLE_CODE_SET_NAME },
    {
      name: 'privateRole',
      id: '2',
      isDisabled: false,
      isPublic: false,
      type: API.ROLE_CODE_SET_NAME,
    },
  ] as ILookupCode[],
};

const requestAccess = {
  status: 200,
  isFetching: false,
} as IGenericNetworkAction;

const mockStore = configureMockStore([thunk]);
const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
mockAxios.onAny().reply(200, {});

// Simulating a successful submit
const successStore = mockStore({
  [lookupCodesSlice.name]: lCodes,
  [networkSlice.name]: {
    [actionTypes.ADD_REQUEST_ACCESS]: requestAccess,
  },
});

// Store without status of 200
const store = mockStore({
  [lookupCodesSlice.name]: lCodes,
  [networkSlice.name]: {
    addRequestAccess: requestAccess,
  },
});

// Render component under test
const testRender = () =>
  render(
    <TenantProvider>
      <Provider store={successStore}>
        <Router history={history}>
          <AccessRequestPage />
        </Router>
      </Provider>
    </TenantProvider>,
  );

describe('[ MOTI ] AccessRequestPage', () => {
  beforeEach(() => {
    process.env.REACT_APP_TENANT = 'MOTI';
  });
  afterEach(() => {
    delete process.env.REACT_APP_TENANT;
    cleanup();
  });
  afterAll(() => {
    jest.unmock('store/slices/accessRequests/useAccessRequests');
  });

  // Enzyme tests
  describe('component functionality when requestAccess status is 200 and fetching is false', () => {
    it('initializes form with null for agencies and roles', () => {
      const componentRender = mount(
        <TenantProvider>
          <Provider store={successStore}>
            <Router history={history}>
              <AccessRequestPage />
            </Router>
          </Provider>
        </TenantProvider>,
      );
      expect(
        componentRender
          .find(Formik)
          .first()
          .prop('initialValues'),
      ).toEqual({
        agencies: [],
        showAgency: false,
        agency: undefined,
        id: 0,
        status: 'OnHold',
        note: '',
        role: undefined,
        roles: [],
        rowVersion: undefined,
        user: {
          displayName: undefined,
          email: undefined,
          firstName: undefined,
          id: undefined,
          lastName: undefined,
          position: '',
          username: undefined,
        },
        userId: undefined,
      });
    });

    it('does not show success message by default', () => {
      const component = mount(
        <Provider store={store}>
          <Router history={history}>
            <AccessRequestPage />
          </Router>
        </Provider>,
      );
      expect(component.find('div.alert').isEmpty).toBeTruthy();
    });
  });

  it('renders correctly', () => {
    const { container } = testRender();
    expect(container.firstChild).toMatchSnapshot();
  });

  it('renders dropdown for roles', () => {
    const { container } = testRender();
    const dropdown = container.querySelector(`select[name="role"]`);
    expect(dropdown).toBeVisible();
  });

  it('displays enabled roles', () => {
    const { queryByText } = testRender();
    expect(queryByText('roleVal')).toBeVisible();
  });

  it('does not display disabled roles', () => {
    const { queryByText } = testRender();
    expect(queryByText('disabledRole')).toBeNull();
  });

  it('does not display private roles', () => {
    const { queryByText } = testRender();
    expect(queryByText('privateRole')).toBeNull();
  });

  it('displays a success message upon form submission', async () => {
    const { addAccessRequest } = useAccessRequests();
    const { container, getByText } = testRender();
    await fillInput(container, 'role', '1', 'select');
    await fillInput(container, 'note', 'some notes', 'textarea');
    const submit = getByText('Submit');
    fireEvent.click(submit);
    await wait(() => expect(addAccessRequest).toBeCalled());
    expect(getByText('Your request has been submitted.')).toBeVisible();
  });

  it('displays an error message upon failure to submit', async () => {
    const { addAccessRequest } = useAccessRequests();
    (addAccessRequest as jest.Mock).mockRejectedValueOnce(new Error('network-error'));
    const { container, getByText } = testRender();
    await fillInput(container, 'role', '1', 'select');
    await fillInput(container, 'note', 'some notes', 'textarea');
    const submit = getByText('Submit');
    fireEvent.click(submit);
    await wait(() => expect(addAccessRequest).toBeCalled());
    expect(getByText('Failed to submit your access request.')).toBeVisible();
  });
});
