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
import { accessRequestsSlice } from 'store/slices/accessRequests';

import { RowActions } from './RowActions';

const mockStore = configureMockStore([thunk]);
const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);

const getItems = (disabled?: boolean) => [
  {
    id: '1',
    businessIdentifier: 'testername1',
    firstName: 'testUserFirst1',
    lastName: 'testUserLast1',
    isDisabled: !!disabled,
    position: 'tester position',
    organizations: [{ id: '1', name: 'HLTH' }],
    roles: [{ id: '1', name: 'admin' }],
    lastLogin: '2020-10-14T17:45:39.7381599',
  },
];

const getStore = (disabled?: boolean) =>
  mockStore({
    [accessRequestsSlice.name]: {
      pagedAccessRequests: {
        pageIndex: 0,
        total: 1,
        quantity: 1,
        items: getItems(disabled),
      },
      filter: {},
      rowsPerPage: 10,
    },
  });
const props = { data: getItems(), row: { original: { id: '1', isDisabled: false } } };
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
    cleanup();
  });
  it('enable button', async () => {
    const tempProps = { ...props };
    tempProps.row.original.isDisabled = true;
    const { container, getByText } = testRender(getStore(), tempProps);
    mockAxios.onPut().reply(200);
    fireEvent.click(container);
    const approveButton = getByText('Approve');
    fireEvent.click(approveButton);
    await waitFor(() => {
      expect(mockAxios.history.put).toHaveLength(1);
      expect(mockAxios.history.put[0].url).toBe('/keycloak/access/requests');
    });
  });
  it('disable button', async () => {
    const tempProps = { ...props };
    tempProps.row.original.isDisabled = false;
    const { container, getByText } = testRender(getStore(), props);
    mockAxios.onPut().reply(200);
    fireEvent.click(container);
    const holdButton = getByText('Hold');
    fireEvent.click(holdButton);
    await waitFor(() => {
      expect(mockAxios.history.put).toHaveLength(1);
      expect(mockAxios.history.put[0].url).toBe('/keycloak/access/requests');
    });
  });
  it('open button', async () => {
    const { container, getByText } = testRender(getStore(), props);
    mockAxios.onGet().reply(200);
    fireEvent.click(container);
    const declineButton = getByText('Decline');
    fireEvent.click(declineButton);
    await waitFor(() => {
      expect(mockAxios.history.put).toHaveLength(1);
      expect(mockAxios.history.put[0].url).toBe('/keycloak/access/requests');
    });
  });
});
