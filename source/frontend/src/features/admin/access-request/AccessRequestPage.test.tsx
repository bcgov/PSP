import { useKeycloak } from '@react-keycloak/web';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import * as API from '@/constants/API';
import { ILookupCode, lookupCodesSlice } from '@/store/slices/lookupCodes';
import { IGenericNetworkAction } from '@/store/slices/network/interfaces';
import { networkSlice } from '@/store/slices/network/networkSlice';
import { cleanup, render, waitForElementToBeRemoved } from '@/utils/test-utils';

import * as actionTypes from '../../../constants/actionTypes';
import AccessRequestPage from './AccessRequestPage';

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

const lCodes = {
  lookupCodes: [
    { id: 1, name: 'One', code: '', isDisabled: false, type: 'core operational' },
    { id: 1, name: 'roleVal', code: '', isDisabled: false, type: API.ROLE_TYPES },
    { id: 2, name: 'disabledRole', code: '', isDisabled: true, type: API.ROLE_TYPES },
    {
      id: 3,
      name: 'privateRole',
      code: '',
      isDisabled: false,
      isPublic: false,
      type: API.ROLE_TYPES,
    },
  ] as ILookupCode[],
};

const requestAccess = {
  status: 200,
  isFetching: false,
} as IGenericNetworkAction;

const mockStore = configureMockStore([thunk]);
const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
mockAxios.onAny().reply(200, {});

// Simulating a successful submit
const successStore = mockStore({
  [lookupCodesSlice.name]: lCodes,
  [networkSlice.name]: {
    [actionTypes.ADD_REQUEST_ACCESS]: requestAccess,
  },
});

// Render component under test
const testRender = (mockStore = successStore) =>
  render(<AccessRequestPage />, { store: mockStore, history });

describe('AccessRequestPage', () => {
  afterEach(() => {
    cleanup();
  });

  it('renders correctly', async () => {
    const { asFragment, getByTestId } = testRender();
    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders dropdown for roles', async () => {
    const { container, getByTestId } = testRender();
    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));
    const dropdown = container.querySelector(`select[name="roleId"]`);
    expect(dropdown).toBeVisible();
  });

  it('displays enabled roles', async () => {
    const { queryByText, getByTestId } = testRender();
    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));
    expect(queryByText('roleVal')).toBeVisible();
  });

  it('does not display disabled roles', async () => {
    const { queryByText, getByTestId } = testRender();
    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));
    expect(queryByText('disabledRole')).toBeNull();
  });

  it('does not display private roles', async () => {
    const { queryByText, getByTestId } = testRender();
    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));
    expect(queryByText('privateRole')).toBeNull();
  });
});
