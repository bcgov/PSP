import { useKeycloak } from '@react-keycloak/web';
import { act, cleanup, fireEvent, render, waitFor } from '@testing-library/react';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import * as API from 'constants/API';
import { createMemoryHistory } from 'history';
import { useApiProperties } from 'hooks/pims-api';
import { IProperty } from 'interfaces';
import { mockParcel } from 'mocks/filterDataMock';
import React from 'react';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { ILookupCode, lookupCodesSlice } from 'store/slices/lookupCodes';
import { TenantProvider } from 'tenants';
import { fillInput } from 'utils/test-utils';

import service from '../service';
import PropertyListView from './PropertyListView';

// Set all module functions to jest.fn
jest.mock('../service');
jest.mock('@react-keycloak/web');
jest.mock('hooks/pims-api/useApiPropertiesProperties');

const mockedService = service as jest.Mocked<typeof service>;

const mockStore = configureMockStore([thunk]);

window.open = jest.fn();

const lCodes = {
  lookupCodes: [
    { id: 1, name: 'organizationVal', isDisabled: false, type: API.ORGANIZATION_CODE_SET_NAME },
    {
      id: 1,
      name: 'classificationVal',
      isDisabled: false,
      type: API.PROPERTY_CLASSIFICATION_CODE_SET_NAME,
    },
  ] as ILookupCode[],
};

const store = mockStore({
  [lookupCodesSlice.name]: lCodes,
});

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
mockAxios.onAny().reply(200, {});

const renderPage = () =>
  render(
    <TenantProvider>
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
          <PropertyListView />
          <ToastContainer />
        </Router>
      </Provider>
    </TenantProvider>,
  );

const setupTests = (items?: IProperty[], buildingItems?: IProperty[]) => {
  // API "returns" no results
  mockedService.getPropertyList.mockResolvedValueOnce({
    quantity: 0,
    total: 0,
    page: 1,
    pageIndex: 0,
    items: items ?? [],
  });
  if (!!buildingItems) {
    mockedService.loadBuildings.mockResolvedValueOnce({
      quantity: 0,
      total: 0,
      page: 1,
      pageIndex: 0,
      items: buildingItems ?? [],
    });
  }
  (useApiProperties as jest.Mock).mockReturnValue({
    updateProperty: jest.fn(),
  });
  (useKeycloak as jest.Mock).mockReturnValue({
    keycloak: {
      subject: 'test',
      userInfo: {
        roles: ['property-edit', 'property-view'],
        organizations: [1],
      },
    },
  });
};

describe('Property list view', () => {
  // clear mocks before each test
  beforeEach(() => {
    process.env.REACT_APP_TENANT = 'MOTI';
    mockedService.getPropertyList.mockClear();
    mockedService.getPropertyReport.mockClear();
  });
  afterEach(() => {
    delete process.env.REACT_APP_TENANT;
    history.push({ search: '' });
    cleanup();
  });

  it('Matches snapshot', async () => {
    setupTests();

    await act(async () => {
      const { container } = renderPage();
      expect(container.firstChild).toMatchSnapshot();
      await waitFor(async () => {
        expect(container.querySelector('span[class="spinner-border"]')).not.toBeInTheDocument();
      });
    });
  });

  it('Displays message for empty list', async () => {
    mockedService.getPropertyList.mockResolvedValueOnce({
      quantity: 0,
      total: 0,
      page: 1,
      pageIndex: 0,
      items: [],
    });

    const { findByText } = renderPage();

    // default table message when there is no data to display
    const noResults = await findByText(/No rows to display/i);
    expect(noResults).toBeInTheDocument();
  });

  it('Displays export buttons', async () => {
    setupTests();

    await act(async () => {
      const { getByTestId, container } = renderPage();
      expect(getByTestId('excel-icon')).toBeInTheDocument();
      expect(getByTestId('csv-icon')).toBeInTheDocument();
      expect(container.querySelector('span[class="spinner-border"]')).not.toBeInTheDocument();
    });
  });

  it('Displays edit button', async () => {
    setupTests();

    await act(async () => {
      const { getByTestId } = renderPage();
      expect(getByTestId('edit-icon')).toBeInTheDocument();
    });
  });

  it('Displays save edit button, when edit is enabled', async () => {
    setupTests();

    await act(async () => {
      const { getByTestId } = renderPage();
      expect(getByTestId('edit-icon')).toBeInTheDocument();
      fireEvent(
        getByTestId('edit-icon'),
        new MouseEvent('click', { bubbles: true, cancelable: true }),
      );
      await waitFor(() => expect(getByTestId('save-changes')).toBeInTheDocument(), {
        timeout: 500,
      });
    });
  });

  it('Displays save edit button, when edit is enabled', async () => {
    setupTests();

    await act(async () => {
      const { getByTestId } = renderPage();
      expect(getByTestId('edit-icon')).toBeInTheDocument();
      fireEvent(
        getByTestId('edit-icon'),
        new MouseEvent('click', { bubbles: true, cancelable: true }),
      );
      await waitFor(() => expect(getByTestId('save-changes')).toBeInTheDocument(), {
        timeout: 500,
      });
    });
  });

  it('Enables edit on property rows that the user has the same organization as the property', async () => {
    setupTests([{ ...mockParcel }]);

    await act(async () => {
      const { getByTestId, container } = renderPage();
      expect(getByTestId('edit-icon')).toBeInTheDocument();
      fireEvent(
        getByTestId('edit-icon'),
        new MouseEvent('click', { bubbles: true, cancelable: true }),
      );
      await waitFor(
        () => {
          expect(getByTestId('save-changes')).toBeInTheDocument();
          expect(getByTestId('cancel-changes')).toBeInTheDocument();
          expect(container.querySelector(`input[name="properties.0.market"]`)).toBeDefined();
        },
        { timeout: 500 },
      );
    });
  });

  it('Disables property rows that the user does not have edit permissions for', async () => {
    setupTests([{ ...mockParcel }]);

    await act(async () => {
      const { getByTestId, container } = renderPage();
      expect(getByTestId('edit-icon')).toBeInTheDocument();
      fireEvent(
        getByTestId('edit-icon'),
        new MouseEvent('click', { bubbles: true, cancelable: true }),
      );
      await waitFor(
        () => {
          expect(getByTestId('save-changes')).toBeInTheDocument();
          expect(container.querySelector(`input[name="properties.0.market"]`)).toBeNull();
        },
        { timeout: 500 },
      );
    });
  });

  it('rows act as clickable links to the property details page.', async () => {
    setupTests([mockParcel]);

    const { container } = renderPage();

    await waitFor(async () => expect(container.querySelector('.spinner-border')).toBeNull());
    const cells = container.querySelectorAll('.td.clickable');
    fireEvent.click(cells[0]);
    await waitFor(async () => expect(window.open).toHaveBeenCalled());
  });

  it('rows can be edited by clicking the edit button', async () => {
    setupTests([{ ...mockParcel, id: 1 }]);

    const { container, getByTestId } = renderPage();

    await waitFor(async () => expect(container.querySelector('.spinner-border')).toBeNull());
    const editButton = getByTestId('edit-icon');
    fireEvent.click(editButton);
    await waitFor(async () =>
      expect(container.querySelector(`input[name="properties.0.assessedLand"]`)).toBeVisible(),
    );
  });

  it('edit mode can be toggled on and off', async () => {
    setupTests([{ ...mockParcel, id: 1 }]);

    const { container, getByTestId, getByText, queryByTestId, queryByText } = renderPage();

    await waitFor(async () => expect(container.querySelector('.spinner-border')).toBeNull());
    const editButton = getByTestId('edit-icon');
    fireEvent.click(editButton);
    await waitFor(async () => {
      expect(queryByTestId('edit-icon')).toBeNull();
      expect(getByText('Cancel')).toBeVisible();
    });
    fireEvent.click(getByText('Cancel'));
    await waitFor(async () => {
      expect(queryByTestId('edit-icon')).toBeVisible();
      expect(queryByText('Cancel')).toBeNull();
    });
  });

  it('updates to financials made in edit mode can be saved', async () => {
    setupTests([{ ...mockParcel, id: 1 }]);

    const { container, getByTestId, getByText } = renderPage();

    await waitFor(async () => expect(container.querySelector('.spinner-border')).toBeNull());
    const editButton = getByTestId('edit-icon');
    fireEvent.click(editButton);
    await waitFor(async () => expect(getByText('Save edits')).toBeVisible());
    await fillInput(container, 'properties.0.assessedLand', '12345');

    (useApiProperties().putProperty as jest.MockedFunction<any>).mockResolvedValueOnce(mockParcel);
    fireEvent.click(getByText('Save edits'));
    await waitFor(() => expect(useApiProperties().putProperty).toHaveBeenCalled());
  });

  it('updates to financials made in edit mode that throw errors are handled', async () => {
    setupTests([{ ...mockParcel, id: 1 }]);

    const { container, getByTestId, getByText } = renderPage();

    await waitFor(async () => expect(container.querySelector('.spinner-border')).toBeNull());
    const editButton = getByTestId('edit-icon');
    fireEvent.click(editButton);
    await waitFor(async () => expect(getByText('Save edits')).toBeVisible());
    await fillInput(container, 'properties.0.assessedLand', '12345');
    (useApiProperties().putProperty as jest.MockedFunction<any>).mockImplementationOnce(() => {
      throw Error;
    });
    fireEvent.click(getByText('Save edits'));
    await waitFor(async () => {
      expect(container.querySelector('.Toastify__toast-body')).toHaveTextContent(
        'Failed to save changes for Test Property. undefined',
      );
    });
  });
});
