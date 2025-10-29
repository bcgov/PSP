import { render, RenderOptions, waitFor } from '@/utils/test-utils';
import DocumentSearchFilter from './DocumentSearchFilter';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { mockDocumentTypesResponse } from '@/mocks/documents.mock';
import { SelectOption } from '@/components/common/form/Select';

const setFilter = vi.fn();
const docTypeOptions: SelectOption[] = mockDocumentTypesResponse().map(dt => {
  return {
    label: dt.documentTypeDescription || '',
    value: dt.id ? dt.id.toString() : '',
  };
});

// render component under test
const setup = (renderOptions: RenderOptions = {}) => {
  const utils = render(
    <DocumentSearchFilter
      setFilter={setFilter}
      documentTypeOptions={docTypeOptions}
      filter={undefined}
    />,
    {
      store: {
        [lookupCodesSlice.name]: { lookupCodes: mockLookups },
      },
      claims: [],
      ...renderOptions,
    },
  );
  const searchButton = utils.getByTestId('search');
  const resetButton = utils.getByTestId('reset-button');
  return { searchButton, setFilter, resetButton, ...utils };
};

describe('Document search Filter', () => {
  beforeEach(() => {
    setFilter.mockClear();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup();

    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });
});
