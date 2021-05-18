import MockAdapter from 'axios-mock-adapter';
import PropertyListView from './PropertyListView';
import React from 'react';
import { Router } from 'react-router-dom';
import { createMemoryHistory } from 'history';
import { render, cleanup, act, wait, fireEvent } from '@testing-library/react';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import * as API from 'constants/API';
import { Provider } from 'react-redux';
import service from '../service';
import { useKeycloak } from '@react-keycloak/web';
import axios from 'axios';
import { mockFlatProperty, mockFlatBuildingProperty } from 'mocks/filterDataMock';
import { IProperty } from '.';
import { mockParcel } from 'components/maps/leaflet/InfoSlideOut/InfoContent.test';
import { fillInput } from 'utils/testUtils';
import { ToastContainer } from 'react-toastify';
import { useApi } from 'hooks/useApi';
import { ILookupCode, lookupCodesSlice } from 'store/slices/lookupCodes';

// Set all module functions to jest.fn
jest.mock('../service');
jest.mock('@react-keycloak/web');
jest.mock('hooks/useApi');

const mockedService = service as jest.Mocked<typeof service>;

const mockStore = configureMockStore([thunk]);

window.open = jest.fn();

const lCodes = {
  lookupCodes: [
    { name: 'agencyVal', id: '1', isDisabled: false, type: API.AGENCY_CODE_SET_NAME },
    {
      name: 'classificationVal',
      id: '1',
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
  (useApi as jest.Mock).mockReturnValue({
    updateParcel: jest.fn(),
    updateBuilding: jest.fn(),
  });
  (useKeycloak as jest.Mock).mockReturnValue({
    keycloak: {
      subject: 'test',
      userInfo: {
        roles: ['property-edit', 'property-view'],
        agencies: [1],
      },
    },
  });
};

describe('Property list view', () => {
  // clear mocks before each test
  beforeEach(() => {
    mockedService.getPropertyList.mockClear();
    mockedService.getPropertyReport.mockClear();
  });
  afterEach(() => {
    history.push({ search: '' });
    cleanup();
  });

  it('Matches snapshot', async () => {
    setupTests();

    await act(async () => {
      const { container } = render(
        <Provider store={store}>
          <Router history={history}>
            <PropertyListView />,
          </Router>
        </Provider>,
      );
      expect(container.firstChild).toMatchSnapshot();
      await wait(async () => {
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

    const { findByText } = render(
      <Provider store={store}>
        <Router history={history}>
          <PropertyListView />
        </Router>
      </Provider>,
    );

    // default table message when there is no data to display
    const noResults = await findByText(/No rows to display/i);
    expect(noResults).toBeInTheDocument();
  });

  it('Displays export buttons', async () => {
    setupTests();

    await act(async () => {
      const { getByTestId, container } = render(
        <Provider store={store}>
          <Router history={history}>
            <PropertyListView />
          </Router>
        </Provider>,
      );
      expect(getByTestId('excel-icon')).toBeInTheDocument();
      expect(getByTestId('csv-icon')).toBeInTheDocument();
      expect(container.querySelector('span[class="spinner-border"]')).not.toBeInTheDocument();
    });
  });

  it('Displays edit button', async () => {
    setupTests();

    await act(async () => {
      const { getByTestId } = render(
        <Provider store={store}>
          <Router history={history}>
            <PropertyListView />
          </Router>
        </Provider>,
      );
      expect(getByTestId('edit-icon')).toBeInTheDocument();
    });
  });

  it('Displays save edit button, when edit is enabled', async () => {
    setupTests();

    await act(async () => {
      const { getByTestId } = render(
        <Provider store={store}>
          <Router history={history}>
            <PropertyListView />
          </Router>
        </Provider>,
      );
      expect(getByTestId('edit-icon')).toBeInTheDocument();
      fireEvent(
        getByTestId('edit-icon'),
        new MouseEvent('click', { bubbles: true, cancelable: true }),
      );
      await wait(() => expect(getByTestId('save-changes')).toBeInTheDocument(), { timeout: 500 });
    });
  });

  it('Displays save edit button, when edit is enabled', async () => {
    setupTests();

    await act(async () => {
      const { getByTestId } = render(
        <Provider store={store}>
          <Router history={history}>
            <PropertyListView />
          </Router>
        </Provider>,
      );
      expect(getByTestId('edit-icon')).toBeInTheDocument();
      fireEvent(
        getByTestId('edit-icon'),
        new MouseEvent('click', { bubbles: true, cancelable: true }),
      );
      await wait(() => expect(getByTestId('save-changes')).toBeInTheDocument(), { timeout: 500 });
    });
  });

  it('Enables edit on property rows that the user has the same agency as the property', async () => {
    setupTests([{ ...mockFlatProperty }]);

    await act(async () => {
      const { getByTestId, container } = render(
        <Provider store={store}>
          <Router history={history}>
            <PropertyListView />
          </Router>
        </Provider>,
      );
      expect(getByTestId('edit-icon')).toBeInTheDocument();
      fireEvent(
        getByTestId('edit-icon'),
        new MouseEvent('click', { bubbles: true, cancelable: true }),
      );
      await wait(
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
    setupTests([{ ...mockFlatProperty, agencyId: 2 }]);

    await act(async () => {
      const { getByTestId, container } = render(
        <Provider store={store}>
          <Router history={history}>
            <PropertyListView />
          </Router>
        </Provider>,
      );
      expect(getByTestId('edit-icon')).toBeInTheDocument();
      fireEvent(
        getByTestId('edit-icon'),
        new MouseEvent('click', { bubbles: true, cancelable: true }),
      );
      await wait(
        () => {
          expect(getByTestId('save-changes')).toBeInTheDocument();
          expect(container.querySelector(`input[name="properties.0.market"]`)).toBeNull();
        },
        { timeout: 500 },
      );
    });
  });

  it('Disables property rows that are in an active project', async () => {
    setupTests([{ ...mockFlatProperty, projectNumbers: ['SPP-10000'] }]);

    await act(async () => {
      const { container, getByTestId } = render(
        <Provider store={store}>
          <Router history={history}>
            <PropertyListView />
          </Router>
        </Provider>,
      );
      expect(getByTestId('edit-icon')).toBeInTheDocument();
      fireEvent(
        getByTestId('edit-icon'),
        new MouseEvent('click', { bubbles: true, cancelable: true }),
      );
      await wait(
        () => {
          expect(getByTestId('save-changes')).toBeInTheDocument();
          expect(container.querySelector(`input[name="properties.0.market"]`)).toBeNull();
        },
        { timeout: 500 },
      );
    });
  });

  it('rows act as clickable links to the property details page.', async () => {
    setupTests([mockFlatProperty]);

    const { container } = render(
      <Provider store={store}>
        <Router history={history}>
          <PropertyListView />
        </Router>
      </Provider>,
    );

    await wait(async () => expect(container.querySelector('.spinner-border')).toBeNull());
    const cells = container.querySelectorAll('.td.clickable');
    fireEvent.click(cells[0]);
    await wait(async () => expect(window.open).toHaveBeenCalled());
  });

  it('rows can be edited by clicking the edit button', async () => {
    setupTests([{ ...mockFlatProperty, id: 1 }]);

    const { container, getByTestId } = render(
      <Provider store={store}>
        <Router history={history}>
          <PropertyListView />
        </Router>
      </Provider>,
    );

    await wait(async () => expect(container.querySelector('.spinner-border')).toBeNull());
    const editButton = getByTestId('edit-icon');
    fireEvent.click(editButton);
    await wait(async () =>
      expect(container.querySelector(`input[name="properties.0.assessedLand"]`)).toBeVisible(),
    );
  });

  it('edit mode can be toggled on and off', async () => {
    setupTests([{ ...mockFlatProperty, id: 1 }]);

    const { container, getByTestId, getByText, queryByTestId, queryByText } = render(
      <Provider store={store}>
        <Router history={history}>
          <PropertyListView />
        </Router>
      </Provider>,
    );

    await wait(async () => expect(container.querySelector('.spinner-border')).toBeNull());
    const editButton = getByTestId('edit-icon');
    fireEvent.click(editButton);
    await wait(async () => {
      expect(queryByTestId('edit-icon')).toBeNull();
      expect(getByText('Cancel')).toBeVisible();
    });
    fireEvent.click(getByText('Cancel'));
    await wait(async () => {
      expect(queryByTestId('edit-icon')).toBeVisible();
      expect(queryByText('Cancel')).toBeNull();
    });
  });

  it('updates to financials made in edit mode can be saved', async () => {
    setupTests([{ ...mockFlatProperty, id: 1 }]);

    const { container, getByTestId, getByText } = render(
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
      </Provider>,
    );

    await wait(async () => expect(container.querySelector('.spinner-border')).toBeNull());
    const editButton = getByTestId('edit-icon');
    fireEvent.click(editButton);
    await wait(async () => expect(getByText('Save edits')).toBeVisible());
    await fillInput(container, 'properties.0.assessedLand', '12345');

    (useApi().updateParcel as jest.MockedFunction<any>).mockResolvedValueOnce(mockParcel);
    fireEvent.click(getByText('Save edits'));
    await wait(() => expect(useApi().updateParcel).toHaveBeenCalled());
  });

  it('updates to financials made in edit mode that throw errors are handled', async () => {
    setupTests([{ ...mockFlatProperty, id: 1 }]);

    const { container, getByTestId, getByText } = render(
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
      </Provider>,
    );

    await wait(async () => expect(container.querySelector('.spinner-border')).toBeNull());
    const editButton = getByTestId('edit-icon');
    fireEvent.click(editButton);
    await wait(async () => expect(getByText('Save edits')).toBeVisible());
    await fillInput(container, 'properties.0.assessedLand', '12345');
    (useApi().updateParcel as jest.MockedFunction<any>).mockImplementationOnce(() => {
      throw Error;
    });
    fireEvent.click(getByText('Save edits'));
    await wait(async () => {
      expect(container.querySelector('.Toastify__toast-body')).toHaveTextContent(
        'Failed to save changes for Test Property. undefined',
      );
    });
  });

  it('rows can be expanded by clicking the folder icon', async () => {
    setupTests([mockFlatProperty], [mockFlatBuildingProperty]);

    const { container, getByText } = render(
      <Provider store={store}>
        <Router history={history}>
          <PropertyListView />
        </Router>
      </Provider>,
    );

    await wait(async () => expect(container.querySelector('.spinner-border')).toBeNull());
    const cells = container.querySelectorAll('.td.expander');
    fireEvent.click(cells[0]);
    await wait(async () => expect(getByText('6460 Applecross Road')).toBeVisible());
  });
});
