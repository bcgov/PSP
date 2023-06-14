import { fireEvent, render, waitFor } from '@testing-library/react';
import { cleanup } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import { FormAccessRequest } from '@/features/admin/access-request/models';
import { mockApiAccessRequest } from '@/mocks/filterData.mock';

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
const props = {
  data: getItems(),
  row: { original: new FormAccessRequest(mockApiAccessRequest) },
  refresh: noop,
};
const testRender = (props: any) =>
  render(
    <Provider store={mockStore()}>
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
  it('approve button', async () => {
    const { container, getByText } = testRender(props);
    mockAxios.onPut().reply(200, mockApiAccessRequest);
    fireEvent.click(container);
    const approveButton = getByText('Approve');
    fireEvent.click(approveButton);
    await waitFor(() => {
      expect(mockAxios.history.put).toHaveLength(1);
      expect(mockAxios.history.put[0].url).toBe('/keycloak/access/requests');
    });
  });
  it('hold button', async () => {
    const { container, getByText } = testRender(props);
    mockAxios.onPut().reply(200, mockApiAccessRequest);
    fireEvent.click(container);
    const holdButton = getByText('Hold');
    fireEvent.click(holdButton);
    await waitFor(() => {
      expect(mockAxios.history.put).toHaveLength(1);
      expect(mockAxios.history.put[0].url).toBe('/keycloak/access/requests');
    });
  });
  it('decline button', async () => {
    const { container, getByText } = testRender(props);
    mockAxios.onGet().reply(200, mockApiAccessRequest);
    fireEvent.click(container);
    const declineButton = getByText('Decline');
    fireEvent.click(declineButton);
    await waitFor(() => {
      expect(mockAxios.history.put).toHaveLength(1);
      expect(mockAxios.history.put[0].url).toBe('/keycloak/access/requests');
    });
  });
});
