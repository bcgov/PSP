import { useKeycloak } from '@react-keycloak/web';
import { AccessRequestStatus } from 'constants/accessStatus';
import * as actionTypes from 'constants/actionTypes';
import * as API from 'constants/API';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';
import React from 'react';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import { create, ReactTestInstance } from 'react-test-renderer';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { accessRequestsSlice } from 'store/slices/accessRequests';
import { ILookupCode, lookupCodesSlice } from 'store/slices/lookupCodes';
import { networkSlice } from 'store/slices/network/networkSlice';
import { ThemeProvider } from 'styled-components';
import { TenantConsumer, TenantProvider } from 'tenants';

import ManageAccessRequests from './ManageAccessRequests';

jest.mock('@react-keycloak/web');
(useKeycloak as jest.Mock).mockReturnValue({
  keycloak: {
    userInfo: {
      organizations: [1],
      roles: [],
    },
    subject: 'test',
  },
});

const accessRequests = {
  page: 1,
  total: 2,
  quantity: 2,
  items: [
    {
      id: 1,
      status: AccessRequestStatus.Received,
      roles: [{ id: '1', name: 'role1' }],
      organizations: [{ id: '1', name: 'organization 1' }],
      user: {
        id: 'userid',
        displayName: 'testUser',
        firstName: 'firstName',
        surname: 'surname',
        email: 'user@email.com',
        position: 'position 1',
      },
    },
  ],
};

const history = createMemoryHistory();
history.push('admin');
const mockStore = configureMockStore([thunk]);

const lCodes = {
  lookupCodes: [
    { name: 'roleVal', id: 1, isDisabled: false, type: API.ROLE_TYPES },
  ] as ILookupCode[],
};

jest.mock('react-router-dom', () => ({
  ...jest.requireActual('react-router-dom'), // use actual for all non-hook parts
  useRouteMatch: () => ({ url: '/admin', path: '/admin' }),
}));

const successStore = mockStore({
  [accessRequestsSlice.name]: {
    pageSize: 10,
    filter: { searchText: '', role: '', organization: '' },
    pagedAccessRequests: accessRequests,
  },
  [networkSlice.name]: {
    [actionTypes.GET_REQUEST_ACCESS]: {
      isFetching: false,
    },
  },
  [lookupCodesSlice.name]: lCodes,
});

const componentRender = (store: any) => {
  process.env.REACT_APP_TENANT = 'MOTI';
  let component = create(
    <TenantProvider>
      <TenantConsumer>
        {({ tenant }) => (
          <Formik initialValues={{}} onSubmit={noop}>
            <Router history={history}>
              <Provider store={store}>
                <ThemeProvider theme={{ tenant, css: {} }}>
                  <ManageAccessRequests />
                </ThemeProvider>
              </Provider>
            </Router>
          </Formik>
        )}
      </TenantConsumer>
    </TenantProvider>,
  );
  return component;
};

describe('Manage access requests', () => {
  afterEach(() => jest.restoreAllMocks());
  it('Snapshot matches', () => {
    const component = componentRender(successStore);
    expect(component.toJSON()).toMatchSnapshot();
  });

  it('Table row count is 1', () => {
    const component = componentRender(successStore);
    const instance = component.root;
    const table = (instance as ReactTestInstance).findByProps({ name: 'accessRequestsTable' });
    expect(table.props.data.length).toBe(1);
  });

  it('On Hold menu button is disabled', () => {
    const component = componentRender(successStore);
    const instance = component.root;
    const holdMenuItem = (instance as ReactTestInstance).findByProps({ label: 'Hold' });
    expect(holdMenuItem.props.disabled).toBeTruthy();
  });

  it('On Approve menu button is enabled', () => {
    const component = componentRender(successStore);
    const instance = component.root;
    const holdMenuItem = (instance as ReactTestInstance).findByProps({ label: 'Approve' });
    expect(holdMenuItem.props.disabled).toBeFalsy();
  });

  it('On Decline menu button is enabled', () => {
    const component = componentRender(successStore);
    const instance = component.root;
    const holdMenuItem = (instance as ReactTestInstance).findByProps({ label: 'Decline' });
    expect(holdMenuItem.props.disabled).toBeFalsy();
  });
});
