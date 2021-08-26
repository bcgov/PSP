import { useKeycloak } from '@react-keycloak/web';
import { cleanup, render } from '@testing-library/react';
import * as actionTypes from 'constants/actionTypes';
import * as API from 'constants/API';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';
import React from 'react';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { ILookupCode, lookupCodesSlice } from 'store/slices/lookupCodes';
import { networkSlice } from 'store/slices/network/networkSlice';
import { organizationsSlice } from 'store/slices/organizations';

import ManageOrganizations from './ManageOrganizations';

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

const history = createMemoryHistory();
history.push('admin/organizations');
const mockStore = configureMockStore([thunk]);

const lCodes = {
  lookupCodes: [
    { name: 'organizationVal', id: 1, isDisabled: false, type: API.ORGANIZATION_CODE_SET_NAME },
  ] as ILookupCode[],
};

jest.mock('react-router-dom', () => ({
  ...jest.requireActual('react-router-dom'), // use actual for all non-hook parts
  useRouteMatch: () => ({ url: '/admin/organizations', path: '/admin/organizations' }),
}));
const getStore = () =>
  mockStore({
    [organizationsSlice.name]: {
      pagedOrganizations: {
        page: 1,
        pageIndex: 0,
        quantity: 10,
        total: 2,
        items: [
          {
            code: 'BCA',
            id: 41,
            isDisabled: false,
            name: 'BC Assessment',
            sendEmail: false,
            sortOrder: 0,
            type: 'Organization',
          },
          {
            code: 'TEST',
            id: 42,
            isDisabled: false,
            name: 'BC Test',
            sendEmail: true,
            sortOrder: 0,
            type: 'Organization',
            parentId: 12,
          },
        ],
      },
    },
    [lookupCodesSlice.name]: lCodes,
    [networkSlice.name]: {
      [actionTypes.GET_ORGANIZATIONS]: {
        isFetching: false,
      },
    },
  });

describe('Manage Organizations Component', () => {
  beforeAll(() => {
    const { getComputedStyle } = window;
    window.getComputedStyle = elt => getComputedStyle(elt);
  });
  afterEach(() => {
    cleanup();
  });

  const testRender = (store: any) =>
    render(
      <Formik initialValues={{}} onSubmit={noop}>
        <Provider store={store}>
          <Router history={history}>
            <ManageOrganizations />
          </Router>
        </Provider>
      </Formik>,
    );

  it('Snapshot matches', () => {
    const { container } = testRender(getStore());
    expect(container.firstChild).toMatchSnapshot();
  });

  it('displays correct organization labels', () => {
    const { queryByText } = testRender(getStore());
    expect(queryByText('BC Assessment')).toBeVisible();
    expect(queryByText('BC Test')).toBeVisible();
  });

  it('displays appropriate codes', () => {
    const { queryByText } = testRender(getStore());
    expect(queryByText('BCA')).toBeVisible();
    expect(queryByText('TEST')).toBeVisible();
  });

  it('displays appropriate cols', () => {
    const { queryByText } = testRender(getStore());
    expect(queryByText('Organization name')).toBeVisible();
    expect(queryByText('Short name')).toBeVisible();
    expect(queryByText('Description')).toBeVisible();
    expect(queryByText('Parent Organization')).toBeVisible();
  });
});
