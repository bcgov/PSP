import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';
import fileDownload from 'js-file-download';

import * as actionTypes from '@/constants/actionTypes';
import * as API from '@/constants/API';
import { mockLookups } from '@/mocks/lookups.mock';
import { getMockPagedUsers } from '@/mocks/user.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { networkSlice } from '@/store/slices/network/networkSlice';
import { prettyFormatDateTime } from '@/utils';
import { act, fillInput, render, userEvent, waitForElementToBeRemoved } from '@/utils/test-utils';

import { ManageUsersPage } from './ManageUsersPage';

const history = createMemoryHistory();
history.push('admin');

const mockAxios = new MockAdapter(axios);

vi.mock('react-router-dom', async importOriginal => {
  const actual = await importOriginal();
  return {
    ...(actual as any), // use actual for all non-hook parts
    useRouteMatch: () => ({ url: '/admin', path: '/admin' }),
  };
});

vi.mock('js-file-download', () => {
  return {
    __esModule: true,
    default: vi.fn(),
  };
});

describe('Manage Users Component', () => {
  let store: any;

  const setup = () => {
    const component = render(<ManageUsersPage />, {
      store: store,
    });
    return { ...component };
  };

  beforeEach(() => {
    mockAxios.onPost().reply(200, getMockPagedUsers());
    store = {
      [lookupCodesSlice.name]: { lookupCodes: mockLookups },
      [networkSlice.name]: {
        [actionTypes.GET_USERS]: {
          isFetching: false,
        },
      },
    };
  });

  afterEach(() => {
    vi.restoreAllMocks();
    mockAxios.reset();
  });

  it('Snapshot matches', async () => {
    const { asFragment, getByTitle } = setup();
    await waitForElementToBeRemoved(getByTitle('table-loading'));
    expect(asFragment()).toMatchSnapshot();
  });

  it('Table row count is 5', async () => {
    const { container, getByTitle } = setup();
    await waitForElementToBeRemoved(getByTitle('table-loading'));
    const rows = container.querySelectorAll('.tbody .tr');
    expect(rows.length).toBe(5);
  });

  it('displays enabled roles', async () => {
    const { queryByText, getByTitle } = setup();
    await waitForElementToBeRemoved(getByTitle('table-loading'));
    expect(queryByText('Acquisition functional')).toBeVisible();
  });

  it('does not display disabled roles', async () => {
    store = {
      ...store,
      [lookupCodesSlice.name]: {
        lookupCodes: [
          ...mockLookups,
          {
            id: 9999,
            key: 'foo-bar',
            name: 'Disabled role',
            description: 'A sample role that is disabled in PIMS',
            isDisabled: true,
            isPublic: true,
            displayOrder: 0,
            type: API.ROLE_TYPES,
            rowVersion: 1,
          },
        ],
      },
    };
    const { queryByText, getByTitle } = setup();
    await waitForElementToBeRemoved(getByTitle('table-loading'));
    expect(queryByText('Disabled role')).toBeNull();
  });

  it(`does not display "Cannot determine" region`, async () => {
    const { queryByText, getByTitle } = setup();
    await waitForElementToBeRemoved(getByTitle('table-loading'));
    expect(queryByText('Cannot determine')).toBeNull();
  });

  it('displays the correct last login time', async () => {
    const { getByText, getByTitle } = setup();
    await waitForElementToBeRemoved(getByTitle('table-loading'));
    expect(getByText(prettyFormatDateTime('2022-06-08T23:24:56.163'))).toBeVisible();
  });

  it('triggers a file download when excel icon is clicked', async () => {
    const { getByTestId, getByTitle } = setup();
    await waitForElementToBeRemoved(getByTitle('table-loading'));
    const excelIcon = getByTestId('excel-icon');
    mockAxios.onGet().reply(200);

    await act(async () => {
      userEvent.click(excelIcon);
    });
    expect(fileDownload).toHaveBeenCalled();
  });

  it('can search for users', async () => {
    const { container, getByTitle } = setup();
    await waitForElementToBeRemoved(getByTitle('table-loading'));
    await fillInput(container, 'firstName', 'testUserFirst1');
    const searchButton = container.querySelector('#search-button');
    mockAxios.onPost().reply(200);
    await act(async () => {
      userEvent.click(searchButton!);
    });
    expect(mockAxios.history.post.length).toBe(1);
  });

  it('search reset works as expected', async () => {
    const { getByTitle, getAllByPlaceholderText, container } = setup();

    await waitForElementToBeRemoved(getByTitle('table-loading'));

    await fillInput(container, 'email', 'email');
    const resetButton = getByTitle('reset-button');
    await act(async () => {
      userEvent.click(resetButton);
    });

    const email = getAllByPlaceholderText('Email')[0];

    expect(email.textContent).toBe('');
  });

  it('each row contains a link to the access request details page', async () => {
    const { getByText, getByTitle } = setup();

    await waitForElementToBeRemoved(getByTitle('table-loading'));

    const link = getByText('desmith@idir');
    expect(link).toHaveAttribute('href', '/admin/user/30');
  });

  describe('access request actions', () => {
    it('Enable menu button is disabled', async () => {
      const { getAllByText, getByTitle } = setup();

      await waitForElementToBeRemoved(getByTitle('table-loading'));

      const enableButton = getAllByText('Enable')[0];
      expect(enableButton).toHaveClass('disabled');
    });

    it('Disable menu button is enabled', async () => {
      const { getAllByText, getByTitle } = setup();

      await waitForElementToBeRemoved(getByTitle('table-loading'));

      const disableButton = getAllByText('Disable')[0];
      expect(disableButton).not.toHaveClass('disabled');
    });

    it('Disable action submits a request', async () => {
      mockAxios.onPut().reply(200, {});

      const { getAllByText, getByTitle } = setup();

      await waitForElementToBeRemoved(getByTitle('table-loading'));

      const disableButton = getAllByText('Disable')[0];
      await act(async () => {
        userEvent.click(disableButton);
      });

      expect(mockAxios.history.put[0].url).toEqual(
        '/keycloak/users/e81274eb-a007-4f2e-ada3-2817efcdb0a6',
      );
    });

    it('Open menu button is enabled', async () => {
      const { getAllByText, getByTitle } = setup();

      await waitForElementToBeRemoved(getByTitle('table-loading'));

      const declineButton = getAllByText('Open')[0];
      expect(declineButton).not.toHaveClass('disabled');
    });
  });
});
