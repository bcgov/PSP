import { act, fireEvent, render, waitFor } from '@testing-library/react';
import { useFormikContext } from 'formik';
import debounce from 'lodash/debounce';

import { GeographicNameInput } from './GeographicNameInput';
import { useGeographicNamesRepository } from '@/hooks/useGeographicNamesRepository';
import TestCommonWrapper from '@/utils/TestCommonWrapper';
import userEvent from '@testing-library/user-event';

vi.mock('formik');
vi.mock('lodash/debounce', () => ({
  __esModule: true,
  default: vi.fn(fn => fn),
}));
vi.mock('@/hooks/useGeographicNamesRepository');

const mockSearchName = {
  execute: vi.fn(),
  error: null,
  loading: false,
  status: null,
  response: null,
};

vi.mocked(useGeographicNamesRepository).mockReturnValue({
  searchName: mockSearchName,
});

vi.mocked(useFormikContext).mockReturnValue({
  handleBlur: vi.fn(),
  setFieldValue: vi.fn(),
  values: { testField: '' },
} as unknown as ReturnType<typeof useFormikContext>);

describe('GeographicNameInput', () => {
  const setup = (props = {}) => {
    return render(
      <TestCommonWrapper>
        <GeographicNameInput field="testField" placeholder="Search..." {...props} />
      </TestCommonWrapper>,
    );
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders correctly', () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays tooltip when required', async () => {
    const { getByTestId } = setup({ placeholder: 'Search...' });
    const input = getByTestId('geographic-name-input');
    await act(async () => {
      userEvent.hover(input);
    });

    expect(getByTestId('testField-input-tooltip')).toBeInTheDocument();
  });

  it('calls debounced search when input value changes', async () => {
    const { getByTestId } = setup();
    const input = getByTestId('geographic-name-input');

    await waitFor(() => {
      fireEvent.change(input, { target: { value: 'Test' } });
    });

    expect(mockSearchName.execute).toHaveBeenCalledWith(expect.objectContaining({ name: 'Test' }));
  });

  it('renders suggestions when search results are available', async () => {
    mockSearchName.execute.mockResolvedValueOnce({
      features: [
        {
          properties: {
            name: 'Test Location',
            featureType: 'Type1',
            featureCategoryDescription: 'Category1',
          },
        },
        {
          properties: {
            name: 'Another Location',
            featureType: 'Type2',
            featureCategoryDescription: 'Category2',
          },
        },
      ],
    });

    const { getByTestId, getByText } = await setup();
    const input = getByTestId('geographic-name-input');

    await act(async () => {
      fireEvent.change(input, { target: { value: 'Test' } });
    });

    expect(getByText('Test Location - Type1 - Category1')).toBeInTheDocument();
    expect(getByText('Another Location - Type2 - Category2')).toBeInTheDocument();
  });

  it('clears suggestions when clicking away', async () => {
    mockSearchName.execute.mockResolvedValueOnce({
      features: [
        {
          properties: {
            name: 'Test Location',
            featureType: 'Type1',
            featureCategoryDescription: 'Category1',
          },
        },
      ],
    });

    const { getByTestId, queryByText } = await setup();
    const input = getByTestId('geographic-name-input');

    await act(async () => {
      fireEvent.change(input, { target: { value: 'Test' } });
    });

    expect(queryByText('Test Location - Type1 - Category1')).toBeInTheDocument();

    await act(async () => {
      fireEvent.click(document.body);
    });

    expect(queryByText('Test Location - Type1 - Category1')).not.toBeInTheDocument();
  });

  it('calls onSelectionChanged when a suggestion is selected', async () => {
    const onSelectionChanged = vi.fn();
    mockSearchName.execute.mockResolvedValueOnce({
      features: [
        {
          properties: {
            name: 'Test Location',
            featureType: 'Type1',
            featureCategoryDescription: 'Category1',
          },
        },
      ],
    });

    const { getByTestId, getByText } = setup({ onSelectionChanged });
    const input = getByTestId('geographic-name-input');

    await act(async () => {
      fireEvent.change(input, { target: { value: 'Test' } });
    });

    const suggestion = getByText('Test Location - Type1 - Category1');
    await act(async () => {
      fireEvent.click(suggestion);
    });

    expect(onSelectionChanged).toHaveBeenCalledWith(
      expect.objectContaining({
        properties: {
          name: 'Test Location',
          featureCategoryDescription: 'Category1',
          featureType: 'Type1',
        },
      }),
    );
  });
});
