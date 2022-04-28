import { useKeycloak } from '@react-keycloak/web';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import * as API from 'constants/API';
import { mount } from 'enzyme';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import React from 'react';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { accessRequestsSlice, IAccessRequestsState } from 'store/slices/accessRequests';
import { useAccessRequests } from 'store/slices/accessRequests/useAccessRequests';
import { ILookupCode, lookupCodesSlice } from 'store/slices/lookupCodes';
import { IGenericNetworkAction } from 'store/slices/network/interfaces';
import { networkSlice } from 'store/slices/network/networkSlice';
import { cleanup, fireEvent, render, waitFor } from 'utils/test-utils';
import { fillInput } from 'utils/test-utils';
import TestCommonWrapper from 'utils/TestCommonWrapper';

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

jest.mock('store/slices/accessRequests/useAccessRequests');
(useAccessRequests as jest.Mock).mockReturnValue({
  fetchCurrentAccessRequest: jest.fn(),
  addAccessRequest: jest.fn(),
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

// Store without status of 200
const store = mockStore({
  [lookupCodesSlice.name]: lCodes,
  [networkSlice.name]: {},
  [accessRequestsSlice.name]: { accessRequest: { id: 0 } } as IAccessRequestsState,
});

// Render component under test
const testRender = (mockStore = successStore) =>
  render(<AccessRequestPage />, { store: mockStore, history });

describe('AccessRequestPage', () => {
  afterEach(() => {
    cleanup();
  });
  afterAll(() => {
    jest.unmock('store/slices/accessRequests/useAccessRequests');
  });

  // Enzyme tests
  describe('component functionality when requestAccess status is 200 and fetching is false', () => {
    it('initializes form with null for organizations and roles', () => {
      const componentRender = mount(
        <TestCommonWrapper store={successStore} history={history}>
          <AccessRequestPage />
        </TestCommonWrapper>,
      );
      expect(
        componentRender
          .find(Formik)
          .first()
          .prop('initialValues'),
      ).toEqual({
        organizationId: undefined,
        id: undefined,
        status: 'RECEIVED',
        note: '',
        roleId: undefined,
        rowVersion: undefined,
        user: {
          displayName: undefined,
          email: undefined,
          firstName: undefined,
          id: undefined,
          surname: undefined,
          position: '',
          businessIdentifierValue: undefined,
          keycloakUserId: undefined,
        },
        userId: undefined,
      });
    });

    it('does not show success message by default', () => {
      const { container } = testRender(store);
      expect(container.querySelector('div.alert')).toBeNull();
    });
  });

  it('renders correctly', () => {
    const { asFragment } = testRender();
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders dropdown for roles', () => {
    const { container } = testRender();
    const dropdown = container.querySelector(`select[name="roleId"]`);
    expect(dropdown).toBeVisible();
  });

  it('displays enabled roles', () => {
    const { queryByText } = testRender();
    expect(queryByText('roleVal')).toBeVisible();
  });

  it('does not display disabled roles', () => {
    const { queryByText } = testRender();
    expect(queryByText('disabledRole')).toBeNull();
  });

  it('does not display private roles', () => {
    const { queryByText } = testRender();
    expect(queryByText('privateRole')).toBeNull();
  });

  it('displays a success message upon form submission', async () => {
    const { addAccessRequest } = useAccessRequests();
    const { container, getByText } = testRender();
    await fillInput(container, 'roleId', '1', 'select');
    await fillInput(container, 'note', 'some notes', 'textarea');
    const submit = getByText('Submit');
    fireEvent.click(submit);
    await waitFor(() => expect(addAccessRequest).toBeCalled());
    expect(getByText('Your request has been submitted.')).toBeVisible();
  });

  it('displays an error message upon failure to submit', async () => {
    const { addAccessRequest } = useAccessRequests();
    (addAccessRequest as jest.Mock).mockRejectedValueOnce(new Error('network-error'));
    const { container, getByText } = testRender();
    await fillInput(container, 'roleId', '1', 'select');
    await fillInput(container, 'note', 'some notes', 'textarea');
    const submit = getByText('Submit');
    fireEvent.click(submit);
    await waitFor(() => expect(addAccessRequest).toBeCalled());
    expect(getByText('Failed to submit your access request.')).toBeVisible();
  });
});
