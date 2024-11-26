import { createMemoryHistory } from 'history';

import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { getByTestId, getByText, render, RenderOptions } from '@/utils/test-utils';

import { vi } from 'vitest';
import {
  HistoricalNumberFieldView,
  IHistoricalNumbersViewProps,
} from './HistoricalNumberFieldView';
import { mockHistoricalFileNumber } from '@/mocks/historicalFileNumber.mock';
import { ApiGen_CodeTypes_HistoricalFileNumberTypes } from '@/models/api/generated/ApiGen_CodeTypes_HistoricalFileNumberTypes';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const mockNumberOne = mockHistoricalFileNumber(
  1,
  1,
  '123',
  ApiGen_CodeTypes_HistoricalFileNumberTypes.LISNO,
  'LIS',
);

describe('HistoricalNumberFieldView component', () => {
  // render component under test

  const setup = (
    renderOptions: RenderOptions & { props?: Partial<IHistoricalNumbersViewProps> },
  ) => {
    const utils = render(
      <HistoricalNumberFieldView
        {...renderOptions.props}
        historicalNumbers={renderOptions?.props?.historicalNumbers ?? []}
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

  it('renders nothing when passed an empty list of historical numbers', () => {
    const { container } = setup({});
    expect(container.textContent).toBe('');
  });

  it('renders the other description instead of other when historical number is of type other', () => {
    const { container } = setup({
      props: {
        historicalNumbers: [
          mockHistoricalFileNumber(
            1,
            1,
            'hist-1',
            ApiGen_CodeTypes_HistoricalFileNumberTypes.OTHER,
            'Other',
            'oth-description',
          ),
        ],
      },
    });
    expect(getByText(container, 'oth-description', { exact: false })).toBeVisible();
    expect(getByText(container, 'hist-1')).toBeVisible();
  });

  it('groups multiple historical file numbers if in same category', () => {
    const { container } = setup({
      props: {
        historicalNumbers: [
          mockHistoricalFileNumber(
            1,
            1,
            '123',
            ApiGen_CodeTypes_HistoricalFileNumberTypes.LISNO,
            'LIS',
          ),
          mockHistoricalFileNumber(
            2,
            2,
            '456',
            ApiGen_CodeTypes_HistoricalFileNumberTypes.LISNO,
            'LIS',
          ),
        ],
      },
    });
    expect(getByTestId(container, 'historical-group-LISNO-').textContent).toBe('LIS:123; 456');
  });

  it('collapses duplicate numbers', () => {
    const { container } = setup({
      props: {
        historicalNumbers: [mockNumberOne],
      },
    });
    expect(getByTestId(container, 'historical-group-LISNO-').textContent).toBe('LIS:123');
  });

  it('displays +1 more when there are more then 3 entries', () => {
    const { container } = setup({
      props: {
        historicalNumbers: [
          { ...mockNumberOne, historicalFileNumber: '1', id: 1 },
          { ...mockNumberOne, historicalFileNumber: '2', id: 2 },
          { ...mockNumberOne, historicalFileNumber: '3', id: 3 },
          { ...mockNumberOne, historicalFileNumber: '4', id: 4 },
        ],
      },
    });
    expect(getByTestId(container, 'historical-group-LISNO-').textContent).toBe(
      'LIS:1; 2; 3; [+1 more...]',
    );
  });

  it('displays one line per category', () => {
    const { container } = setup({
      props: {
        historicalNumbers: [
          mockHistoricalFileNumber(
            1,
            1,
            '123',
            ApiGen_CodeTypes_HistoricalFileNumberTypes.LISNO,
            'LIS',
          ),
          mockHistoricalFileNumber(
            2,
            2,
            '456',
            ApiGen_CodeTypes_HistoricalFileNumberTypes.PSNO,
            'PS',
          ),
        ],
      },
    });
    expect(getByTestId(container, 'historical-group-LISNO-').textContent).toBe('LIS:123');
    expect(getByTestId(container, 'historical-group-PSNO-').textContent).toBe('PS:456');
  });

  it('displays [+x more...] if there are more then 2 categories', () => {
    const { container } = setup({
      props: {
        historicalNumbers: [
          mockHistoricalFileNumber(
            1,
            1,
            '123',
            ApiGen_CodeTypes_HistoricalFileNumberTypes.LISNO,
            'LIS',
          ),
          mockHistoricalFileNumber(
            2,
            2,
            '456',
            ApiGen_CodeTypes_HistoricalFileNumberTypes.PSNO,
            'PS',
          ),
          mockHistoricalFileNumber(
            2,
            2,
            '789',
            ApiGen_CodeTypes_HistoricalFileNumberTypes.PROPNEG,
            'Property Negotiation (PN)',
          ),
        ],
      },
    });
    expect(getByTestId(container, 'historical-group-LISNO-').textContent).toBe('LIS:123');
    expect(getByTestId(container, 'historical-group-PSNO-').textContent).toBe('PS:456');
    expect(getByText(container, '[+1 more categories...]')).toBeVisible();
  });
});
