import { createMemoryHistory } from 'history';

import { mockLookups } from '@/mocks/lookups.mock';
import { getMockApiPropertyManagement } from '@/mocks/propertyManagement.mock';
import { ApiGen_Concepts_PropertyManagement } from '@/models/api/generated/ApiGen_Concepts_PropertyManagement';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { FilterContentForm, IFilterContentFormProps } from './FilterContentForm';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const mockGetApi = {
  error: undefined,
  response: undefined as ApiGen_Concepts_PropertyManagement | undefined,
  execute: vi.fn(),
  loading: false,
};

vi.mock('@/hooks/repositories/usePropertyManagementRepository', () => ({
  usePropertyManagementRepository: () => {
    return {
      getPropertyManagement: mockGetApi,
    };
  },
}));

describe('FilterContentForm component', () => {
  const onChange = vi.fn();

  const setup = (renderOptions: RenderOptions & { props: IFilterContentFormProps }) => {
    renderOptions = renderOptions ?? ({} as any);
    const utils = render(<FilterContentForm {...renderOptions.props} />, {
      ...renderOptions,
      store: storeState,
      history,
    });

    return {
      ...utils,
    };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('shows loading spinner when loading', () => {
    mockGetApi.execute.mockResolvedValue(getMockApiPropertyManagement(1));
    const { getByTestId } = setup({ props: { onChange, isLoading: true } });
    expect(getByTestId('filter-backdrop-loading')).toBeVisible();
  });

  it('displays filters when not loading', async () => {
    const apiManagement = getMockApiPropertyManagement(1);
    mockGetApi.response = apiManagement;
    const { getByDisplayValue } = setup({ props: { onChange, isLoading: false } });
    expect(getByDisplayValue('Select a highway')).toBeVisible();
    expect(getByDisplayValue('Select Lease Transaction')).toBeVisible();
  });

  it('calls onChange when a filter is changed', async () => {
    const apiManagement = getMockApiPropertyManagement(1);
    mockGetApi.response = apiManagement;
    const { getByTestId } = setup({ props: { onChange, isLoading: false } });
    await act(async () => {
      userEvent.selectOptions(getByTestId('leasePayRcvblType'), ['all']);
      expect(onChange).toBeCalled();
    });
  });
});
