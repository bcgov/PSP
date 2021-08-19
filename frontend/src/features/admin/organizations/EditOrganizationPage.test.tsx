import { useKeycloak } from '@react-keycloak/web';
import { cleanup, fireEvent, render, screen, waitFor } from '@testing-library/react';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import * as API from 'constants/API';
import { createMemoryHistory } from 'history';
import React from 'react';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { ILookupCode, lookupCodesSlice } from 'store/slices/lookupCodes';
import { organizationsSlice } from 'store/slices/organizations';
import { fillInput } from 'utils/test-utils';

import EditOrganizationPage from './EditOrganizationPage';

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
    { name: 'Test Organization', id: 111, isDisabled: false, type: API.ORGANIZATION_CODE_SET_NAME },
  ] as ILookupCode[],
};

const selectedOrganization = {
  code: 'TEST',
  id: 111,
  email: 'test@email.com',
  isDisabled: false,
  sendEmail: true,
  addressTo: 'Good morning',
  type: 'Organization',
  name: 'Test Organization',
};

const store = mockStore({
  [organizationsSlice.name]: { organizationDetail: selectedOrganization },
  [lookupCodesSlice.name]: lCodes,
});

const mockAxios = new MockAdapter(axios);

const renderEditOrganizationPage = () =>
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
        <EditOrganizationPage id={111} />,
      </Router>
    </Provider>,
  );

describe('Edit organization page', () => {
  afterEach(() => {
    cleanup();
    jest.clearAllMocks();
  });
  beforeEach(() => {
    mockAxios.onAny().reply(200, {});
  });
  it('EditOrganizationPage renders', () => {
    const { container } = render(
      <Provider store={store}>
        <Router history={history}>
          <EditOrganizationPage id={111} />,
        </Router>
      </Provider>,
    );
    expect(container.firstChild).toMatchSnapshot();
  });

  describe('appropriate fields are autofilled', () => {
    it('autofills  email, name, send email, and code', () => {
      const { getByLabelText } = renderEditOrganizationPage();
      expect(getByLabelText(/organization e-mail address/i).getAttribute('value')).toEqual(
        'test@email.com',
      );
      expect(getByLabelText('Organization').getAttribute('value')).toEqual('Test Organization');
      expect(getByLabelText('Short Name (Code)').getAttribute('value')).toEqual('TEST');
      expect(getByLabelText(/email notifications?/i).getAttribute('value')).toEqual('true');
    });
  });

  describe('when the organization edit form is submitted', () => {
    it('displays a loading toast', async () => {
      const { getByText, findByText } = renderEditOrganizationPage();
      const saveButton = getByText(/save/i);
      fireEvent.click(saveButton);
      await findByText('Updating Organization...');
    });

    it('displays a success toast if the request passes', async () => {
      const { getByText, findByText } = renderEditOrganizationPage();
      const saveButton = getByText(/save/i);
      fireEvent.click(saveButton);
      await findByText('Organization updated');
    });

    it('displays a error toast if the request fails', async () => {
      const { getByText, findByText } = renderEditOrganizationPage();
      const saveButton = getByText(/save/i);
      mockAxios.reset();
      mockAxios.onAny().reply(500, {});
      fireEvent.click(saveButton);
      await findByText('Failed to update Organization');
    });

    it('displays an error message if a new organization is missing data', async () => {
      history.push('/new');
      const { getByText, findByText } = renderEditOrganizationPage();
      const saveButton = getByText(/Submit Organization/i);
      fireEvent.click(saveButton);
      await findByText('An organization name is required.');
    });

    it('displays a success toast if the request passes for a new organization', async () => {
      history.push('/new');
      const { getByText, findByText, container } = renderEditOrganizationPage();
      mockAxios.reset();
      mockAxios.onAny().reply(200, {});
      await waitFor(() => fillInput(container, 'name', 'test organization'));
      await waitFor(() => fillInput(container, 'code', 'TA'));
      await waitFor(() => fillInput(container, 'email', '1@1.ca'));
      await waitFor(() => fillInput(container, 'addressTo', 'hello you'));
      const saveButton = getByText(/Submit Organization/i);
      fireEvent.click(saveButton);
      await findByText('Organization updated');
    });

    it('can delete organizations with no properties', async () => {
      history.push('');
      const { getByText, container } = renderEditOrganizationPage();
      mockAxios.reset();
      mockAxios.onAny().reply(200, {});
      mockAxios.onGet().reply(200, { total: 0 });
      await waitFor(() => fillInput(container, 'name', 'test organization'));
      await waitFor(() => fillInput(container, 'code', 'TA'));
      await waitFor(() => fillInput(container, 'email', '1@1.ca'));
      await waitFor(() => fillInput(container, 'addressTo', 'hello you'));
      const deleteButton = getByText(/Delete Organization/i);
      fireEvent.click(deleteButton);
      await screen.findByText('Are you sure you want to permanently delete the organization?');
      const deleteConfirm = getByText(/^Delete$/i);
      fireEvent.click(deleteConfirm);
      await waitFor(async () => {
        expect(mockAxios.history.delete).toHaveLength(1);
      });
    });

    it('can not delete organizations with properties', async () => {
      history.push('');
      const { getByText, container } = renderEditOrganizationPage();
      mockAxios.reset();
      mockAxios.onAny().reply(200, {});
      await waitFor(() => fillInput(container, 'name', 'test organization'));
      await waitFor(() => fillInput(container, 'code', 'TA'));
      await waitFor(() => fillInput(container, 'email', '1@1.ca'));
      await waitFor(() => fillInput(container, 'addressTo', 'hello you'));
      const deleteButton = getByText(/Delete Organization/i);
      fireEvent.click(deleteButton);
      await screen.findByText(
        'You are not able to delete this organization as there are properties currently associated with it.',
      );
      const deleteConfirm = getByText(/^ok$/i);
      fireEvent.click(deleteConfirm);
    });
  });
});
