import { useKeycloak } from '@react-keycloak/web';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom/cjs/react-router-dom';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import * as API from '@/constants/API';
import { getUserMock } from '@/mocks/user.mock';
import { ILookupCode, lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, cleanup, render, waitForElementToBeRemoved } from '@/utils/test-utils';

import EditUserPage from './EditUserPage';

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
        <EditUserPage userKey="TEST-ID" />,
      </Router>
    </Provider>,
  );

describe('Edit user page', () => {
  afterEach(() => {
    cleanup();
    vi.clearAllMocks();
  });
  beforeEach(() => {
    mockAxios.reset();
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
    const { getAllByText, getByTestId, findByDisplayValue } = renderEditUserPage();
    await findByDisplayValue('pos');
    expect(getAllByText(/roleVal/i)).toHaveLength(1);
    expect(getByTestId('isDisabled').getAttribute('value')).toEqual('false');
  });

  it('Does not display disabled roles', async () => {
    const { queryByText, findByDisplayValue } = renderEditUserPage();
    await findByDisplayValue('pos');
    expect(queryByText('disabledRole')).toBeNull();
  });

  describe('appropriate fields are autofilled', () => {
    it('autofills  email, businessIdentifier, first and last name', async () => {
      const { getByTestId, findByDisplayValue } = renderEditUserPage();
      await findByDisplayValue('pos');

      expect(getByTestId('email').getAttribute('value')).toEqual('devin.smith@gov.bc.ca');
      expect(getByTestId('businessIdentifierValue').getAttribute('value')).toEqual('desmith@idir');
      expect(getByTestId('firstName').getAttribute('value')).toEqual('Devin');
      expect(getByTestId('surname').getAttribute('value')).toEqual('Smith');
      expect(getByTestId('position').getAttribute('value')).toEqual('pos');
    });
  });

  describe('when the user edit form is submitted', () => {
    it('displays a loading toast', async () => {
      const { getByText, findByText, findByDisplayValue } = renderEditUserPage();
      const saveButton = getByText('Save');
      mockAxios.onGet().reply(200, getUserMock());
      await findByDisplayValue('pos');
      await act(async () => {
        saveButton.click();
      });
      await findByText('Updating User...');
    });

    it('displays a success toast if the request passes', async () => {
      const { getByText, findByText, findByDisplayValue } = renderEditUserPage();
      const saveButton = getByText('Save');
      mockAxios.onGet().reply(200, getUserMock());
      mockAxios.onPut().reply(200, getUserMock());
      await findByDisplayValue('pos');
      await act(async () => {
        saveButton.click();
      });
      await findByText('User updated');
    });

    it('displays an error toast if the request fails', async () => {
      const { getByText, findByText, findByDisplayValue } = renderEditUserPage();
      const saveButton = getByText('Save');
      mockAxios.onGet().replyOnce(200, getUserMock());
      mockAxios.onPut().reply(500, {});
      await findByDisplayValue('pos');
      await act(async () => {
        saveButton.click();
      });
      await findByText('Failed to update User');
    });
  });
});
