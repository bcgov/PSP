import userEvent from '@testing-library/user-event';

import { ILeaseFilter } from '@/features/leases';
import { useUserInfoRepository } from '@/hooks/repositories/useUserInfoRepository';
import { getUserMock } from '@/mocks/user.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, render, RenderOptions } from '@/utils/test-utils';

import { ILeaseFilterProps, LeaseFilter } from './LeaseFilter';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: [] },
};

const setFilter = jest.fn();

jest.mock('@react-keycloak/web');

jest.mock('@/hooks/repositories/useUserInfoRepository');
(useUserInfoRepository as jest.Mock).mockReturnValue({
  retrieveUserInfo: jest.fn(),
  retrieveUserInfoLoading: true,
  retrieveUserInfoResponse: getUserMock(),
});

// render component under test
const setup = (
  renderOptions: RenderOptions & ILeaseFilterProps = { store: storeState, setFilter },
) => {
  const { filter, setFilter: setFilterFn, ...rest } = renderOptions;
  const utils = render(<LeaseFilter filter={filter} setFilter={setFilterFn} />, {
    ...rest,
    claims: [],
  });
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

    fillInput(container, 'searchBy', 'pinOrPid', 'select');
    fillInput(container, 'pinOrPid', '123');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<ILeaseFilter>({
        lFileNo: '',
        pinOrPid: '123',
        searchBy: 'pinOrPid',
        tenantName: '',
        programs: [],
        leaseStatusTypes: ['ACTIVE'],
        expiryStartDate: '',
        expiryEndDate: '',
        regionType: '',
        details: '',
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
        pinOrPid: '',
        searchBy: 'lFileNo',
        tenantName: '',
        programs: [],
        leaseStatusTypes: ['ACTIVE'],
        expiryStartDate: '',
        expiryEndDate: '',
        regionType: '',
        details: '',
      }),
    );
  });

  it('searches tenant name', async () => {
    const { container, searchButton, setFilter } = setup();

    fillInput(container, 'searchBy', 'pinOrPid', 'select');
    fillInput(container, 'tenantName', 'Chester');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<ILeaseFilter>({
        lFileNo: '',
        pinOrPid: '',
        searchBy: 'pinOrPid',
        tenantName: 'Chester',
        programs: [],
        leaseStatusTypes: ['ACTIVE'],
        expiryStartDate: '',
        expiryEndDate: '',
        regionType: '',
        details: '',
      }),
    );
  });

  it('resets the filter when reset button is clicked', async () => {
    const { container, resetButton, setFilter } = setup();

    fillInput(container, 'searchBy', 'pinOrPid', 'select');
    fillInput(container, 'pinOrPid', 'foo-bar-baz');
    await act(async () => userEvent.click(resetButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<ILeaseFilter>({
        lFileNo: '',
        pinOrPid: '',
        searchBy: 'lFileNo',
        tenantName: '',
        programs: [],
        leaseStatusTypes: ['ACTIVE'],
        expiryStartDate: '',
        expiryEndDate: '',
        regionType: '',
        details: '',
      }),
    );
  });
});
