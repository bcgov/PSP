import { useKeycloak } from '@react-keycloak/web';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import * as API from 'constants/API';
import { createMemoryHistory } from 'history';
import moment from 'moment-timezone';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { ILookupCode, lookupCodesSlice } from 'store/slices/lookupCodes';
import { usersSlice } from 'store/slices/users';
import { act, cleanup, render } from 'utils/test-utils';

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

const selectedUser = {
  appCreateTimestamp: '2021-05-04T19:07:09.6920606',
  displayName: 'User, Admin',
  email: 'admin@pims.gov.bc.ca',
  firstName: 'George',
  id: 1,
  key: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
  surname: 'User',
  position: '',
  rowVersion: 1,
  businessIdentifier: 'admin',
  lastLogin: '2020-10-14T17:45:39.7381599',
};

const store = mockStore({
  [usersSlice.name]: { userDetail: selectedUser },
  [lookupCodesSlice.name]: lCodes,
});

const noDateStore = mockStore({
  [usersSlice.name]: { userDetail: { ...selectedUser, lastLogin: null } },
  [lookupCodesSlice.name]: lCodes,
});

const mockAxios = new MockAdapter(axios);

const testRender = () =>
  render(
    <Provider store={store}>
      <Router history={history}>
        <EditUserPage userKey="TEST-ID" />,
      </Router>
    </Provider>,
  );

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
    mockAxios.onAny().reply(200, {});
  });
  it('EditUserPage renders', () => {
    const { container } = render(
      <Provider store={noDateStore}>
        <Router history={history}>
          <EditUserPage userKey="TEST-ID" />,
        </Router>
      </Provider>,
    );
    expect(container.firstChild).toMatchSnapshot();
  });

  it('contains role options from lookup code + please select disabled option', () => {
    const { getAllByText, getByTestId } = renderEditUserPage();
    expect(getAllByText(/Roles/i));
    expect(getAllByText(/roleVal/i));
    expect(getByTestId('isDisabled').getAttribute('value')).toEqual('false');
  });

  it('displays enabled roles', () => {
    const { queryByText } = testRender();
    expect(queryByText('roleVal')).toBeVisible();
  });

  it('Does not display disabled roles', () => {
    const { queryByText } = testRender();
    expect(queryByText('disabledRole')).toBeNull();
  });

  describe('appropriate fields are autofilled', () => {
    it('autofills  email, businessIdentifier, first and last name', () => {
      const { getByTestId } = renderEditUserPage();
      expect(getByTestId('email').getAttribute('value')).toEqual('admin@pims.gov.bc.ca');
      expect(getByTestId('businessIdentifier').getAttribute('value')).toEqual('admin');
      expect(getByTestId('firstName').getAttribute('value')).toEqual('George');
      expect(getByTestId('surname').getAttribute('value')).toEqual('User');
      expect(getByTestId('surname').getAttribute('value')).toEqual('User');
    });
  });

  describe('when the user edit form is submitted', () => {
    it('displays a loading toast', async () => {
      const { getByText, findByText } = renderEditUserPage();
      const saveButton = getByText('Save');
      act(() => {
        saveButton.click();
      });
      await findByText('Updating User...');
    });

    it('displays a success toast if the request passes', async () => {
      const { getByText, findByText } = renderEditUserPage();
      const saveButton = getByText('Save');
      act(() => {
        saveButton.click();
      });
      await findByText('User updated');
    });

    it('displays an error toast if the request fails', async () => {
      const { getByText, findByText } = renderEditUserPage();
      const saveButton = getByText('Save');
      mockAxios.reset();
      mockAxios.onAny().reply(500, {});
      act(() => {
        saveButton.click();
      });
      await findByText('Failed to update User');
    });

    it('Displays the correct last login time', () => {
      const dateTime = moment
        .utc('2020-10-14T17:45:39.7381599')
        .local()
        .format('YYYY-MM-DD hh:mm a');
      const { getByTestId } = renderEditUserPage();
      expect(getByTestId('lastLogin')).toHaveValue(dateTime);
    });
  });
});
