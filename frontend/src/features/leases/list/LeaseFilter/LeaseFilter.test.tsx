import userEvent from '@testing-library/user-event';
import { ILeaseFilter } from 'features/leases';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { act, fillInput, render, RenderOptions } from 'utils/test-utils';

import { ILeaseFilterProps, LeaseFilter } from './LeaseFilter';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: [] },
};

const setFilter = jest.fn();

// render component under test
const setup = (
  renderOptions: RenderOptions & ILeaseFilterProps = { store: storeState, setFilter },
) => {
  const { filter, setFilter: setFilterFn, ...rest } = renderOptions;
  const utils = render(<LeaseFilter filter={filter} setFilter={setFilterFn} />, { ...rest });
  const searchButton = utils.getByTestId('search');
  const resetButton = utils.getByTestId('reset-button');
  return { searchButton, resetButton, setFilter: setFilterFn, ...utils };
};

describe('Lease Filter', () => {
  beforeEach(() => {
    setFilter.mockClear();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('searches by pid/pin', async () => {
    const { container, searchButton, setFilter } = setup();

    fillInput(container, 'searchBy', 'pidOrPin', 'select');
    fillInput(container, 'pidOrPin', '123');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<ILeaseFilter>({
        lFileNo: '',
        pidOrPin: '123',
        searchBy: 'pidOrPin',
        tenantName: '',
        programs: [],
      }),
    );
  });

  it('searches by L-file number', async () => {
    const { container, searchButton, setFilter } = setup();

    fillInput(container, 'searchBy', 'lFileNo', 'select');
    fillInput(container, 'lFileNo', '123');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<ILeaseFilter>({
        lFileNo: '123',
        pidOrPin: '',
        searchBy: 'lFileNo',
        tenantName: '',
        programs: [],
      }),
    );
  });

  it('searches tenant name', async () => {
    const { container, searchButton, setFilter } = setup();

    fillInput(container, 'searchBy', 'pidOrPin', 'select');
    fillInput(container, 'tenantName', 'Chester');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<ILeaseFilter>({
        lFileNo: '',
        pidOrPin: '',
        searchBy: 'pidOrPin',
        tenantName: 'Chester',
        programs: [],
      }),
    );
  });

  it('resets the filter when reset button is clicked', async () => {
    const { container, resetButton, setFilter } = setup();

    fillInput(container, 'searchBy', 'pidOrPin', 'select');
    fillInput(container, 'pidOrPin', 'foo-bar-baz');
    await act(async () => userEvent.click(resetButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<ILeaseFilter>({
        lFileNo: '',
        pidOrPin: '',
        searchBy: 'lFileNo',
        tenantName: '',
        programs: [],
      }),
    );
  });
});
