import axios, { AxiosResponse } from 'axios';
import MockAdapter from 'axios-mock-adapter';
import * as API from 'constants/API';
import { createMemoryHistory } from 'history';
import { useApiProperties } from 'hooks/pims-api';
import { IPagedItems, IProperty } from 'interfaces';
import { mockParcel } from 'mocks/filterDataMock';
import { ILookupCode } from 'store/slices/lookupCodes';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { cleanup, render, RenderOptions, waitFor } from 'utils/test-utils';

import { Claims } from '../../../constants';
import PropertyListView from './PropertyListView';

// Set all module functions to jest.fn
jest.mock('@react-keycloak/web');
jest.mock('hooks/pims-api');

const mockApiGetPropertiesPaged = jest.fn<Promise<AxiosResponse<IPagedItems<IProperty>>>, any>();
((useApiProperties as unknown) as jest.Mock<Partial<typeof useApiProperties>>).mockReturnValue({
  getPropertiesPaged: mockApiGetPropertiesPaged,
});

const mockAxios = new MockAdapter(axios);
const history = createMemoryHistory();

// we need these lookup codes for the property list view to function
const setupReduxStore = () => {
  const lookupCodes = [
    { id: 1, name: 'Victoria', isDisabled: false, type: API.ADMINISTRATIVE_AREA_CODE_SET_NAME },
    { id: 2, name: 'Kamloops', isDisabled: false, type: API.ADMINISTRATIVE_AREA_CODE_SET_NAME },
  ] as ILookupCode[];

  return { [lookupCodesSlice.name]: { lookupCodes } };
};

const setup = (renderOptions: RenderOptions = {}) => {
  // ensure the tests have relevant claims enabled
  const { claims = [], store = setupReduxStore() } = renderOptions;
  const allClaims = [...claims, Claims.PROPERTY_VIEW, Claims.PROPERTY_EDIT];

  // render component under test
  const component = render(<PropertyListView />, { ...renderOptions, claims: allClaims, store });

  return {
    component,
    findSpinner: () => component.queryByRole('status'),
  };
};

const setupMockApi = (properties?: IProperty[]) => {
  const mockProperties = properties ?? [];
  const len = mockProperties.length;
  mockApiGetPropertiesPaged.mockResolvedValue({
    data: {
      quantity: len,
      total: len,
      page: 1,
      pageIndex: 0,
      items: mockProperties,
    },
  } as any);
};

describe('Property list view', () => {
  // clear mocks before each test
  beforeEach(() => {
    mockAxios.reset();
    mockAxios.onAny().reply(200, {});

    mockApiGetPropertiesPaged.mockClear();
  });
  afterEach(() => {
    history.push({ search: '' });
    cleanup();
  });

  it('matches snapshot', async () => {
    setupMockApi();
    const { component, findSpinner } = setup();
    await waitFor(async () => expect(findSpinner()).not.toBeInTheDocument());
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('displays message for empty list', async () => {
    setupMockApi();
    const {
      component: { findByText },
    } = setup();

    // default table message when there is no data to display
    const noResults = await findByText(/No rows to display/i);
    expect(noResults).toBeInTheDocument();
  });

  it('displays list of properties', async () => {
    setupMockApi([mockParcel]);
    const {
      component: { findByText },
    } = setup();

    const results = await findByText(/1234 mock Street/i);
    expect(results).toBeInTheDocument();
  });

  it('displays export buttons', async () => {
    setupMockApi();
    const {
      component: { getByTestId },
      findSpinner,
    } = setup();

    // wait for table to finish loading
    await waitFor(async () => expect(findSpinner()).not.toBeInTheDocument());

    expect(getByTestId('excel-icon')).toBeInTheDocument();
    expect(getByTestId('csv-icon')).toBeInTheDocument();
  });
});
