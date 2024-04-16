import userEvent from '@testing-library/user-event';

import { Roles } from '@/constants/index';
import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_Concepts_FinancialCodeTypes } from '@/models/api/generated/ApiGen_Concepts_FinancialCodeTypes';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, render, RenderOptions, waitFor } from '@/utils/test-utils';

import {
  defaultFinancialCodeFilter,
  FinancialCodeFilter,
  IFinancialCodeFilter,
} from './FinancialCodeFilter';

jest.mock('@react-keycloak/web');

const setFilter = jest.fn();

// render component under test
const setup = (renderOptions: RenderOptions = {}) => {
  const utils = render(<FinancialCodeFilter setFilter={setFilter} />, {
    store: {
      [lookupCodesSlice.name]: { lookupCodes: mockLookups },
    },
    roles: [Roles.SYSTEM_ADMINISTRATOR],
    ...renderOptions,
  });
  const searchButton = utils.getByTestId('search');
  const resetButton = utils.getByTestId('reset-button');
  return { searchButton, setFilter, resetButton, ...utils };
};

describe('Financial Code Filter', () => {
  beforeEach(() => {
    setFilter.mockClear();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup();

    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('searches for active financial codes by default', async () => {
    const { resetButton, container } = setup();
    const expiredCheck = container.querySelector(`#input-showExpiredCodes`);
    await act(async () => userEvent.click(resetButton));

    expect(setFilter).toHaveBeenCalledWith(defaultFinancialCodeFilter);
    expect(expiredCheck).not.toBeChecked();
  });

  it('searches by code type', async () => {
    const { container, searchButton } = setup();

    fillInput(
      container,
      'financialCodeType',
      ApiGen_Concepts_FinancialCodeTypes.ChartOfAccounts,
      'select',
    );
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<IFinancialCodeFilter>({
        financialCodeType: ApiGen_Concepts_FinancialCodeTypes.ChartOfAccounts,
        codeValueOrDescription: '',
        showExpiredCodes: false,
      }),
    );
  });

  it('searches by code value or description', async () => {
    const { container, searchButton } = setup();
    fillInput(container, 'codeValueOrDescription', 'foo bar baz');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<IFinancialCodeFilter>({
        financialCodeType: undefined,
        codeValueOrDescription: 'foo bar baz',
        showExpiredCodes: false,
      }),
    );
  });

  it('searches for inactive codes if checkbox is unchecked', async () => {
    const { container } = setup();
    const expiredCheck = container.querySelector(`#input-showExpiredCodes`);
    expect(expiredCheck).not.toBeNull();
    await act(async () => userEvent.click(expiredCheck as Element));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<IFinancialCodeFilter>({
        ...defaultFinancialCodeFilter,
        showExpiredCodes: true,
      }),
    );
  });

  it('resets the filter when reset button is clicked', async () => {
    const { container, resetButton, setFilter } = setup();

    fillInput(container, 'codeValueOrDescription', 'breaking');
    await act(async () => userEvent.click(resetButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<IFinancialCodeFilter>(defaultFinancialCodeFilter),
    );
  });
});
