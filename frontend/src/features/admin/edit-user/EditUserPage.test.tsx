import { useKeycloak } from '@react-keycloak/web';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import * as API from 'constants/API';
import { createMemoryHistory } from 'history';
import { getUserMock } from 'mocks/userMock';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { ILookupCode, lookupCodesSlice } from 'store/slices/lookupCodes';
import { cleanup, render, waitForElementToBeRemoved } from 'utils/test-utils';

import EditUserPage from './EditUserPage';

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

const mockStore = configureMockStore([thunk]);
const history = createMemoryHistory();

const lCodes = {
  lookupCodes: [
    { name: 'roleVal', id: 1, isDisabled: false, type: API.ROLE_TYPES },
    { name: 'disabledRole', id: 2, isDisabled: true, type: API.ROLE_TYPES },
  ] as ILookupCode[],
};

const store = mockStore({
  [lookupCodesSlice.name]: lCodes,
});

const mockAxios = new MockAdapter(axios);

const renderEditUserPage = () =>
  render(
    <Provider store={store}>
      <Router history={history}>
        <ToastContainer
          autoClose={5000}
          hideProgressBar
          newestOnTop={false}
          closeOnClick={false}
          rtl={false}
          pauseOnFocusLoss={false}
        />
        <EditUserPage userKey="TEST-ID" />,
      </Router>
    </Provider>,
  );

describe('Edit user page', () => {
  afterEach(() => {
    cleanup();
    jest.clearAllMocks();
  });
  beforeEach(() => {
    mockAxios.onAny().reply(200, getUserMock());
  });

  it('EditUserPage renders', async () => {
    const { asFragment, getByTestId } = render(
      <Provider store={store}>
        <Router history={history}>
          <EditUserPage userKey="TEST-ID" />,
        </Router>
      </Provider>,
    );
    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));
    expect(asFragment()).toMatchSnapshot();
  });

  it('contains role options from lookup code + please select disabled option', async () => {
    const { getAllByText, getByTestId } = renderEditUserPage();
    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));
    expect(getAllByText(/roleVal/i));
    expect(getByTestId('isDisabled').getAttribute('value')).toEqual('false');
  });

  it('Does not display disabled roles', async () => {
    const { queryByText, getByTestId } = renderEditUserPage();
    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));
    expect(queryByText('disabledRole')).toBeNull();
  });

  describe('appropriate fields are autofilled', () => {
    it('autofills  email, businessIdentifier, first and last name', async () => {
      const { getByTestId } = renderEditUserPage();
      await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));
      expect(getByTestId('email').getAttribute('value')).toEqual('devin.smith@gov.bc.ca');
      expect(getByTestId('businessIdentifier').getAttribute('value')).toEqual('desmith@idir');
      expect(getByTestId('firstName').getAttribute('value')).toEqual('Devin');
      expect(getByTestId('surname').getAttribute('value')).toEqual('Smith');
      expect(getByTestId('position').getAttribute('value')).toEqual('pos');
    });
  });

  describe('when the user edit form is submitted', () => {
    it('displays a loading toast', async () => {
      const { getByText, findByText, getByTestId } = renderEditUserPage();
      await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));
      const saveButton = getByText('Save');
      saveButton.click();
      expect(await findByText('Updating User...')).toBeVisible();
    });

    it('displays a success toast if the request passes', async () => {
      const { getByText, findByText, getByTestId } = renderEditUserPage();
      await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));
      const saveButton = getByText('Save');
      saveButton.click();
      expect(await findByText('User updated')).toBeVisible();
    });

    it('displays an error toast if the request fails', async () => {
      const { getByText, findByText, getByTestId } = renderEditUserPage();
      await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));
      const saveButton = getByText('Save');
      mockAxios.reset();
      mockAxios.onAny().reply(500, {});
      saveButton.click();
      expect(await findByText('Failed to update User')).toBeVisible();
    });
  });
});
