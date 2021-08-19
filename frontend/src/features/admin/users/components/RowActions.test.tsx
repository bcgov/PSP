import { fireEvent, render, waitFor } from '@testing-library/react';
import { cleanup } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';
import React from 'react';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { usersSlice } from 'store/slices/users';

import { RowActions } from './RowActions';

const mockStore = configureMockStore([thunk]);
const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);

const getItems = (disabled?: boolean) => [
  {
    id: 1,
    key: '03c92a6b-686e-4015-84b3-e726905473f4',
    username: 'testername1',
    firstName: 'testUserFirst1',
    lastName: 'testUserLast1',
    isDisabled: !!disabled,
    position: 'tester position',
    organizations: [{ id: 1, name: 'HLTH' }],
    roles: [{ id: 1, name: 'admin' }],
    lastLogin: '2020-10-14T17:45:39.7381599',
  },
];

const getStore = (disabled?: boolean) =>
  mockStore({
    [usersSlice.name]: {
      pagedUsers: {
        pageIndex: 0,
        total: 1,
        quantity: 1,
        items: getItems(disabled),
      },
      filter: {},
      rowsPerPage: 10,
    },
  });
const props = { data: getItems(), row: { original: { id: 1, isDisabled: false } } };
const testRender = (store: any, props: any) =>
  render(
    <Provider store={store}>
      <Router history={history}>
        <RowActions {...{ ...(props as any) }} />
      </Router>
    </Provider>,
  );

describe('rowAction functions', () => {
  beforeEach(() => {
    mockAxios.resetHistory();
  });
  afterEach(() => {
    jest.clearAllMocks();
    cleanup();
  });
  it('enable button', async () => {
    const tempProps = { ...props };
    tempProps.row.original.isDisabled = true;
    const { container, getByText } = testRender(getStore(), tempProps);
    mockAxios.onPut().reply(200);
    fireEvent.click(container);
    const enableButton = getByText('Enable');
    fireEvent.click(enableButton);
    await waitFor(() => {
      expect(mockAxios.history.put).toHaveLength(1);
      expect(mockAxios.history.put[0].url).toBe(
        '/keycloak/users/03c92a6b-686e-4015-84b3-e726905473f4',
      );
    });
  });
  it('disable button', async () => {
    const tempProps = { ...props };
    tempProps.row.original.isDisabled = false;
    const { container, getByText } = testRender(getStore(), props);
    mockAxios.onPut().reply(200);
    fireEvent.click(container);
    const disableButton = getByText('Disable');
    fireEvent.click(disableButton);
    await waitFor(() => {
      expect(mockAxios.history.put).toHaveLength(1);
      expect(mockAxios.history.put[0].url).toBe(
        '/keycloak/users/03c92a6b-686e-4015-84b3-e726905473f4',
      );
    });
  });
  it('open button', async () => {
    const { container, getByText } = testRender(getStore(), props);
    mockAxios.onGet().reply(200, { data: getItems()[0] });
    fireEvent.click(container);
    const openButton = getByText('Open');
    fireEvent.click(openButton);
    await waitFor(() => {
      expect(history.location.pathname).toBe('/admin/user/1');
    });
  });
});
