import axios, { AxiosResponse } from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';

import * as API from '@/constants/API';
import Claims from '@/constants/claims';
import { useApiGeocoder } from '@/hooks/pims-api/useApiGeocoder';
import { useApiProperties } from '@/hooks/pims-api/useApiProperties';
import { mockApiPropertyView } from '@/mocks/filterData.mock';
import { ApiGen_Base_Page } from '@/models/api/generated/ApiGen_Base_Page';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { ILookupCode } from '@/store/slices/lookupCodes';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  cleanup,
  focusOptionMultiselect,
  render,
  RenderOptions,
  userEvent,
  waitFor,
} from '@/utils/test-utils';

import PropertyListView, { ownershipFilterOptions } from './PropertyListView';
import { MultiSelectOption } from '@/features/acquisition/list/interfaces';
import { IGeocoderPidsResponse } from '@/hooks/pims-api/interfaces/IGeocoder';
import { IPropertyFilter } from '../filter/IPropertyFilter';
import { ApiGen_Concepts_PropertyView } from '@/models/api/generated/ApiGen_Concepts_PropertyView';

// Set all module functions to vi.fn

vi.mock('@/hooks/pims-api/useApiGeocoder');
vi.mock('@/hooks/pims-api/useApiProperties');

const mockApiGetPropertiesPagedApi = vi.fn();
vi.mocked(useApiProperties).mockReturnValue({
  getPropertiesViewPagedApi: mockApiGetPropertiesPagedApi as (
    params: IPropertyFilter | null,
  ) => Promise<AxiosResponse<ApiGen_Base_Page<ApiGen_Concepts_Property>, any>>,
} as unknown as ReturnType<typeof useApiProperties>);

const mockApiGetSitePidsApi = vi.fn();
vi.mocked(useApiGeocoder).mockReturnValue({
  getSitePidsApi: mockApiGetSitePidsApi as unknown as (
    siteId: string,
  ) => Promise<AxiosResponse<IGeocoderPidsResponse, any>>,
} as unknown as ReturnType<typeof useApiGeocoder>);

const mockAxios = new MockAdapter(axios);
const history = createMemoryHistory();

// we need these lookup codes for the property list view to function
const setupReduxStore = () => {
  const lookupCodes = [
    { id: 1, name: 'Victoria', isDisabled: false, type: API.ADMINISTRATIVE_AREA_TYPES },
    { id: 2, name: 'Kamloops', isDisabled: false, type: API.ADMINISTRATIVE_AREA_TYPES },
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

const setupMockApi = (properties?: ApiGen_Concepts_PropertyView[]) => {
  const mockProperties = properties ?? [];
  const len = mockProperties.length;
  mockApiGetPropertiesPagedApi.mockResolvedValue({
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

    mockApiGetPropertiesPagedApi.mockClear();
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
    setupMockApi([mockApiPropertyView()]);
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

  it('displays column icons', async () => {
    setupMockApi([mockApiPropertyView()]);
    const {
      component: { getByTestId },
      findSpinner,
    } = setup();

    // wait for table to finish loading
    await waitFor(async () => expect(findSpinner()).not.toBeInTheDocument());

    expect(getByTestId('view-prop-tab')).toBeInTheDocument();
    expect(getByTestId('view-prop-ext')).toBeInTheDocument();
  });

  it('preselects default property ownership state', async () => {
    setupMockApi([mockApiPropertyView()]);
    const {
      component: { getByText },
      findSpinner,
    } = setup({});

    // wait for table to finish loading
    await waitFor(async () => expect(findSpinner()).not.toBeInTheDocument());

    expect(getByText('Core Inventory')).toBeInTheDocument();
    expect(getByText('Property Of Interest')).toBeInTheDocument();
    expect(getByText('Disposed')).toBeInTheDocument();
  });

  it('allows property ownership to be selected', async () => {
    setupMockApi([mockApiPropertyView()]);
    const {
      component: { container },
      findSpinner,
    } = setup({});

    // wait for table to finish loading
    await waitFor(async () => expect(findSpinner()).not.toBeInTheDocument());

    const optionSelected = ownershipFilterOptions.find(
      o => o.id === 'isDisposed',
    ) as MultiSelectOption;

    // click on the multi-select to show drop-down list
    await act(async () =>
      userEvent.click(container.querySelector(`#properties-selector`) as HTMLInputElement),
    );

    // select an option from the drop-down
    await focusOptionMultiselect(container, optionSelected, ownershipFilterOptions);

    await waitFor(() => {
      expect(mockApiGetPropertiesPagedApi).toHaveBeenCalledWith({
        address: '',
        latitude: '',
        longitude: '',
        ownership: 'isCoreInventory,isPropertyOfInterest,isOtherInterest,isDisposed',
        page: 1,
        pinOrPid: '',
        planNumber: '',
        quantity: 10,
        searchBy: 'pinOrPid',
        sort: undefined,
      });
    });
  });

  it('displays a tooltip beside properties that are retired', async () => {
    setupMockApi([{ ...mockApiPropertyView(), isRetired: true }]);
    const {
      component: { getByTestId },
      findSpinner,
    } = setup({});

    // wait for table to finish loading
    await waitFor(async () => expect(findSpinner()).not.toBeInTheDocument());

    expect(getByTestId('tooltip-icon-retired-tooltip')).toBeVisible();
  });
});
