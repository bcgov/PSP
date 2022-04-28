import { useKeycloak } from '@react-keycloak/web';
import { cleanup, fireEvent, render, waitFor } from '@testing-library/react';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import * as actionTypes from 'constants/actionTypes';
import * as API from 'constants/API';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';
import moment from 'moment-timezone';
import React from 'react';
import { act } from 'react-dom/test-utils';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { ILookupCode, lookupCodesSlice } from 'store/slices/lookupCodes';
import { networkSlice } from 'store/slices/network/networkSlice';
import { usersSlice } from 'store/slices/users';
import { ThemeProvider } from 'styled-components';
import { fillInput } from 'utils/test-utils';

import { ManageUsers } from './ManageUsers';

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
history.push('admin');
const mockStore = configureMockStore([thunk]);

const lCodes = {
  lookupCodes: [
    { id: 1, name: 'roleVal', code: '', isDisabled: false, type: API.ROLE_TYPES },
    { id: 2, name: 'disabledRole', code: '', isDisabled: true, type: API.ROLE_TYPES },
  ] as ILookupCode[],
};
const mockAxios = new MockAdapter(axios);

jest.mock('react-router-dom', () => ({
  ...jest.requireActual('react-router-dom'), // use actual for all non-hook parts
  useRouteMatch: () => ({ url: '/admin', path: '/admin' }),
}));
const getStore = (includeDate?: boolean) =>
  mockStore({
    [usersSlice.name]: {
      pagedUsers: {
        pageIndex: 0,
        total: 2,
        quantity: 2,
        items: [
          {
            id: 1,
            businessIdentifier: 'testername1',
            firstName: 'testUserFirst1',
            surname: 'testUserLast1',
            isDisabled: false,
            position: 'tester position',
            organizations: [{ id: 1, name: 'HLTH' }],
            roles: [{ id: 1, name: 'admin' }],
            lastLogin: includeDate ? '2020-10-14T17:45:39.7381599' : null,
          },
          {
            id: 2,
            businessIdentifier: 'testername2',
            firstName: 'testUser',
            surname: 'testUser',
            isDisabled: true,
            position: 'tester',
            organizations: [{ id: 1, name: 'HLTH' }],
            roles: [{ id: 1, name: 'admin' }],
            lastLogin: includeDate ? '2020-10-14T17:46:39.7381599' : null,
          },
        ],
      },
      filter: { firstName: '' },
      rowsPerPage: 10,
    },
    [lookupCodesSlice.name]: lCodes,
    [networkSlice.name]: {
      [actionTypes.GET_USERS]: {
        isFetching: false,
      },
    },
  });

describe('Manage Users Component', () => {
  beforeEach(() => {
    mockAxios.resetHistory();
  });

  afterEach(() => {
    cleanup();
  });
  const testRender = (store: any) =>
    render(
      <Formik initialValues={{}} onSubmit={noop}>
        <Provider store={store}>
          <Router history={history}>
            <ThemeProvider theme={{ tenant: {}, css: {} }}>
              <ManageUsers />
            </ThemeProvider>
          </Router>
        </Provider>
      </Formik>,
    );

  it('Snapshot matches', () => {
    const { container } = testRender(getStore());
    expect(container.firstChild).toMatchSnapshot();
  });

  it('Table row count is 2', () => {
    const { container } = testRender(getStore());
    const rows = container.querySelectorAll('.tbody .tr');
    expect(rows.length).toBe(2);
  });

  it('displays enabled roles', () => {
    const { queryByText } = testRender(getStore());
    expect(queryByText('roleVal')).toBeVisible();
  });

  it('Does not display disabled roles', () => {
    const { queryByText } = testRender(getStore());
    expect(queryByText('disabledRole')).toBeNull();
  });

  it('Displays the correct last login time', () => {
    const dateTime = moment
      .utc('2020-10-14T17:45:39.7381599')
      .local()
      .format('YYYY-MM-DD hh:mm a');
    const { getByText } = testRender(getStore(true));
    expect(getByText(dateTime)).toBeVisible();
  });

  it('downloads data when excel icon clicked', async () => {
    const { getByTestId } = testRender(getStore());
    const excelIcon = getByTestId('excel-icon');
    mockAxios.onGet().reply(200);

    act(() => {
      fireEvent.click(excelIcon);
    });
    await waitFor(() => {
      expect(mockAxios.history.get.length).toBe(1);
    });
  });

  it('can search for users', async () => {
    const { container } = testRender(getStore());
    await fillInput(container, 'firstName', 'testUserFirst1');
    const searchButton = container.querySelector('#search-button');
    mockAxios.onPost().reply(200);
    act(() => {
      fireEvent.click(searchButton!);
    });
    await waitFor(() => {
      expect(mockAxios.history.post.length).toBe(1);
    });
  });
});
