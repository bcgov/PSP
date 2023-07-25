import { useKeycloak } from '@react-keycloak/web';
import { fireEvent, waitFor, waitForElementToBeRemoved } from '@testing-library/react';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';
import { act } from 'react-dom/test-utils';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import * as actionTypes from '@/constants/actionTypes';
import * as API from '@/constants/API';
import { getMockPagedUsers } from '@/mocks/user.mock';
import { ILookupCode, lookupCodesSlice } from '@/store/slices/lookupCodes';
import { networkSlice } from '@/store/slices/network/networkSlice';
import { prettyFormatDateTime } from '@/utils';
import { fillInput, render, userEvent } from '@/utils/test-utils';

import { ManageUsersPage } from './ManageUsersPage';

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
    [lookupCodesSlice.name]: lCodes,
    [networkSlice.name]: {
      [actionTypes.GET_USERS]: {
        isFetching: false,
      },
    },
  });

describe('Manage Users Component', () => {
  afterEach(() => {
    jest.restoreAllMocks();
    mockAxios.reset();
  });
  const testRender = (store: any) => {
    mockAxios.onPost().reply(200, getMockPagedUsers());
    const component = render(<ManageUsersPage />, { store: store });
    return { ...component };
  };

  it('Snapshot matches', async () => {
    const { container, getByTitle } = testRender(getStore());
    await waitForElementToBeRemoved(getByTitle('table-loading'));
    expect(container.firstChild).toMatchSnapshot();
  });

  it('Table row count is 5', async () => {
    const { container, getByTitle } = testRender(getStore());
    await waitForElementToBeRemoved(getByTitle('table-loading'));
    const rows = container.querySelectorAll('.tbody .tr');
    expect(rows.length).toBe(5);
  });

  it('displays enabled roles', async () => {
    const { queryByText, getByTitle } = testRender(getStore());
    await waitForElementToBeRemoved(getByTitle('table-loading'));
    expect(queryByText('roleVal')).toBeVisible();
  });

  it('Does not display disabled roles', async () => {
    const { queryByText, getByTitle } = testRender(getStore());
    await waitForElementToBeRemoved(getByTitle('table-loading'));
    expect(queryByText('disabledRole')).toBeNull();
  });

  it('Displays the correct last login time', async () => {
    const { getByText, getByTitle } = testRender(getStore(true));
    await waitForElementToBeRemoved(getByTitle('table-loading'));
    expect(getByText(prettyFormatDateTime('2022-06-08T23:24:56.163'))).toBeVisible();
  });

  xit('downloads data when excel icon clicked', async () => {
    const { getByTestId, getByTitle } = testRender(getStore());
    await waitForElementToBeRemoved(getByTitle('table-loading'));
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
    const { container, getByTitle } = testRender(getStore());
    await waitForElementToBeRemoved(getByTitle('table-loading'));
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

  it('search reset works as expected', async () => {
    const { getByTitle, getAllByPlaceholderText, container } = testRender(getStore());

    await waitForElementToBeRemoved(getByTitle('table-loading'));

    await fillInput(container, 'email', 'email');
    const resetButton = getByTitle('reset-button');
    userEvent.click(resetButton);

    const email = getAllByPlaceholderText('Email')[0];

    expect(email.textContent).toBe('');
  });

  it('each row contains a link to the access request details page', async () => {
    const { getByText, getByTitle } = testRender(getStore());

    await waitForElementToBeRemoved(getByTitle('table-loading'));

    const link = getByText('desmith@idir');
    expect(link).toHaveAttribute('href', '/admin/user/30');
  });

  describe('access request actions', () => {
    it('Enable menu button is disabled', async () => {
      const { getAllByText, getByTitle } = testRender(getStore());

      await waitForElementToBeRemoved(getByTitle('table-loading'));

      const enableButton = getAllByText('Enable')[0];
      expect(enableButton).toHaveClass('disabled');
    });

    it('Disable menu button is enabled', async () => {
      const { getAllByText, getByTitle } = testRender(getStore());

      await waitForElementToBeRemoved(getByTitle('table-loading'));

      const disableButton = getAllByText('Disable')[0];
      expect(disableButton).not.toHaveClass('disabled');
    });

    it('Disable action submits a request', async () => {
      mockAxios.onPut().reply(200, {});
      const { getAllByText, getByTitle } = testRender(getStore());

      await waitForElementToBeRemoved(getByTitle('table-loading'));

      const disableButton = getAllByText('Disable')[0];
      userEvent.click(disableButton);

      await waitFor(() => {
        expect(mockAxios.history.put[0].url).toEqual(
          '/keycloak/users/e81274eb-a007-4f2e-ada3-2817efcdb0a6',
        );
      });
    });

    it('Open menu button is enabled', async () => {
      const { getAllByText, getByTitle } = testRender(getStore());

      await waitForElementToBeRemoved(getByTitle('table-loading'));

      const declineButton = getAllByText('Open')[0];
      expect(declineButton).not.toHaveClass('disabled');
    });
  });
});
