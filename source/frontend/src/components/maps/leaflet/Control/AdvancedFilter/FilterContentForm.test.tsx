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

const onChange = vi.fn();
const onReset = vi.fn();

describe('FilterContentForm component', () => {
  const setup = (
    renderOptions: RenderOptions & { props?: Partial<IFilterContentFormProps> } = {},
  ) => {
    const utils = render(
      <FilterContentForm
        isLoading={renderOptions?.props?.isLoading ?? false}
        onChange={renderOptions?.props?.onChange ?? onChange}
        onReset={renderOptions?.props?.onReset ?? onReset}
      />,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return {
      ...utils,
    };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('shows loading spinner when loading', () => {
    const { getByTestId } = setup({ props: { isLoading: true } });
    expect(getByTestId('filter-backdrop-loading')).toBeVisible();
  });

  it('displays filters when not loading', async () => {
    const { getByDisplayValue } = setup({ props: { isLoading: false } });

    expect(getByDisplayValue('Select a highway')).toBeVisible();
    expect(getByDisplayValue('Select Lease Transaction')).toBeVisible();
  });

  it(`calls "onChange" when a filter is changed`, async () => {
    const { getByTestId } = setup({ props: { isLoading: false } });

    await act(async () => {
      userEvent.selectOptions(getByTestId('leasePayRcvblType'), ['all']);
    });
    expect(onChange).toHaveBeenCalled();
  });

  it(`calls "onReset" when the reset button is clicked`, async () => {
    const { getByTitle } = setup({ props: { isLoading: false } });

    const resetButton = getByTitle('reset-button');
    expect(resetButton).toBeVisible();
    await act(async () => userEvent.click(resetButton));
    expect(onReset).toHaveBeenCalled();
  });
});
