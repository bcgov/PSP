import { useKeycloak } from '@react-keycloak/web';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import * as actionTypes from '@/constants/actionTypes';
import * as API from '@/constants/API';
import { getMockPagedAccessRequests } from '@/mocks/accessRequest.mock';
import { ILookupCode, lookupCodesSlice } from '@/store/slices/lookupCodes';
import { networkSlice } from '@/store/slices/network/networkSlice';
import { act, render, userEvent, waitFor, waitForElementToBeRemoved } from '@/utils/test-utils';

import ManageAccessRequestsPage from './ManageAccessRequestsPage';

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
const mockAxios = new MockAdapter(axios);

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
  [networkSlice.name]: {
    [actionTypes.GET_REQUEST_ACCESS]: {
      isFetching: false,
    },
  },
  [lookupCodesSlice.name]: lCodes,
});

const componentRender = (store: any) => {
  mockAxios.onGet().reply(200, getMockPagedAccessRequests());
  process.env.REACT_APP_TENANT = 'MOTI';
  const component = render(<ManageAccessRequestsPage />);
  return {
    findFirstRow: () => {
      const rows = component.getAllByRole('row');
      return rows && rows.length > 1 ? rows[1] : null;
    },
    ...component,
  };
};

describe('Manage access requests', () => {
  afterEach(() => {
    jest.restoreAllMocks();
    mockAxios.reset();
  });
  it('Snapshot matches', async () => {
    const { asFragment, getByTitle } = componentRender(successStore);

    await waitForElementToBeRemoved(getByTitle('table-loading'));

    expect(asFragment()).toMatchSnapshot();
  });

  it('Table row count is 2 + th', async () => {
    const { getAllByRole, getByTitle } = componentRender(successStore);

    await waitForElementToBeRemoved(getByTitle('table-loading'));

    const rows = getAllByRole('row');
    expect(rows).toHaveLength(3);
  });

  it('each row contains a link to the access request details page', async () => {
    const { getByText, getByTitle } = componentRender(successStore);

    await waitForElementToBeRemoved(getByTitle('table-loading'));

    const link = getByText('yhashmi');
    expect(link).toHaveAttribute('href', '/admin/access/request/7');
  });

  describe('access request actions', () => {
    it('On Hold menu button is disabled', async () => {
      const { getAllByText, getByTitle } = componentRender(successStore);

      await waitForElementToBeRemoved(getByTitle('table-loading'));

      const holdButton = getAllByText('Hold')[0];
      expect(holdButton).toHaveClass('disabled');
    });

    it('On Approve menu button is enabled', async () => {
      const { getAllByText, getByTitle } = componentRender(successStore);

      await waitForElementToBeRemoved(getByTitle('table-loading'));

      const approveButton = getAllByText('Approve')[0];
      expect(approveButton).not.toHaveClass('disabled');
    });

    it('On Approve action submits a request', async () => {
      mockAxios.onPut().reply(200, {});
      const { getAllByText, getByTitle } = componentRender(successStore);

      await waitForElementToBeRemoved(getByTitle('table-loading'));

      const declineButton = getAllByText('Approve')[0];
      userEvent.click(declineButton);

      await waitFor(() => {
        expect(mockAxios.history.put[0].url).toEqual('/keycloak/access/requests');
      });
    });

    it('On Decline menu button is enabled', async () => {
      const { getAllByText, getByTitle } = componentRender(successStore);

      await waitForElementToBeRemoved(getByTitle('table-loading'));

      const declineButton = getAllByText('Decline')[0];
      expect(declineButton).not.toHaveClass('disabled');
    });

    it('On Decline action submits a request', async () => {
      mockAxios.onPut().reply(200, {});
      const { getAllByText, getByTitle } = componentRender(successStore);

      await waitForElementToBeRemoved(getByTitle('table-loading'));

      const declineButton = getAllByText('Decline')[0];
      userEvent.click(declineButton);

      await waitFor(() => {
        expect(mockAxios.history.put[0].url).toEqual('/keycloak/access/requests');
      });
    });

    it('Delete menu button is enabled', async () => {
      const { getAllByText, getByTitle } = componentRender(successStore);

      await waitForElementToBeRemoved(getByTitle('table-loading'));

      const deleteButton = getAllByText('Delete')[0];
      expect(deleteButton).not.toHaveClass('disabled');
    });

    it('Delete action submits a delete request', async () => {
      mockAxios.onDelete().reply(200, {});
      const { getAllByText, getByTitle } = componentRender(successStore);

      await waitForElementToBeRemoved(getByTitle('table-loading'));

      const deleteButton = getAllByText('Delete')[0];
      userEvent.click(deleteButton);

      await waitFor(() => {
        expect(mockAxios.history.delete[0].url).toEqual('/admin/access/requests/7');
      });
    });
  });
  describe('access request search', () => {
    it('search makes a get request with expected url', async () => {
      const { getByTitle } = componentRender(successStore);

      await waitForElementToBeRemoved(getByTitle('table-loading'));

      const searchInput = getByTitle('Search by IDIR/Last Name');
      userEvent.type(searchInput, 'Smith');
      const searchButton = getByTitle('search');
      userEvent.click(searchButton);

      await waitFor(() => {
        expect(mockAxios.history.get[1].url).toEqual(
          '/admin/access/requests?organization=&page=1&quantity=10&role=&searchText=Smith',
        );
      });
    });

    it('search reset works as expected', async () => {
      const { getByTitle } = componentRender(successStore);

      await waitForElementToBeRemoved(getByTitle('table-loading'));

      const searchInput = getByTitle('Search by IDIR/Last Name');
      act(() => {
        userEvent.type(searchInput, 'Smith');
      });
      const resetButton = getByTitle('reset');
      act(() => {
        userEvent.click(resetButton);
      });

      expect(searchInput).toHaveValue('');
    });
  });
});
