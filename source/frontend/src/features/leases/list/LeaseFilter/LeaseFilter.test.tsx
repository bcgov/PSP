import { ILeaseFilter } from '@/features/leases';
import { useApiLeases } from '@/hooks/pims-api/useApiLeases';
import { useUserInfoRepository } from '@/hooks/repositories/useUserInfoRepository';
import { getUserMock } from '@/mocks/user.mock';
import { ApiGen_Base_Page } from '@/models/api/generated/ApiGen_Base_Page';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { ILeaseFilterProps, LeaseFilter } from './LeaseFilter';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: [] },
};

const setFilter = vi.fn();

vi.mock('@/hooks/repositories/useUserInfoRepository');
vi.mocked(useUserInfoRepository).mockReturnValue({
  retrieveUserInfo: vi.fn(),
  retrieveUserInfoLoading: true,
  retrieveUserInfoResponse: getUserMock(),
});

vi.mock('@/hooks/pims-api/useApiLeases');
vi.mocked(useApiLeases).mockReturnValue({
  getLeases: vi.fn().mockResolvedValue({
    data: {
      items: [{ lFileNo: 'l-1234' }],
      page: 1,
      total: 1,
      quantity: 1,
    } as ApiGen_Base_Page<ApiGen_Concepts_Lease>,
  }),
  getApiLease: vi.fn(),
  getLastUpdatedByApi: vi.fn(),
  postLease: vi.fn(),
  putApiLease: vi.fn(),
  exportLeases: vi.fn(),
  exportAggregatedLeases: vi.fn(),
  exportLeasePayments: vi.fn(),
  putLeaseChecklist: vi.fn(),
  getLeaseChecklist: vi.fn(),
  getLeaseRenewals: vi.fn(),
  getLeaseStakeholderTypes: vi.fn(),
  putLeaseProperties: vi.fn(),
  getAllLeaseFileTeamMembers: vi.fn().mockResolvedValue({ data: [] }),
  getLeaseAtTime: vi.fn(),
});

// render component under test
const setup = async (
  renderOptions: RenderOptions & ILeaseFilterProps = { store: storeState, setFilter },
) => {
  const { filter, setFilter: setFilterFn, ...rest } = renderOptions;
  const utils = render(<LeaseFilter filter={filter} setFilter={setFilterFn} />, {
    ...rest,
    claims: [],
  });
  // wait for effects to run
  await act(async () => {});

  const searchButton = utils.getByTestId('search');
  const resetButton = utils.getByTestId('reset-button');
  return { searchButton, resetButton, setFilter: setFilterFn, ...utils };
};

describe('Lease Filter', () => {
  beforeEach(() => {
    setFilter.mockClear();
  });

  it('matches snapshot', async () => {
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('searches by pid', async () => {
    const { container, searchButton, setFilter } = await setup();

    fillInput(container, 'searchBy', 'pid', 'select');
    fillInput(container, 'pid', '123');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<ILeaseFilter>({
        lFileNo: '',
        pid: '123',
        pin: '',
        searchBy: 'pid',
        tenantName: '',
        programs: [],
        leaseStatusTypes: [
          'ACTIVE',
          'ARCHIVED',
          'DISCARD',
          'DRAFT',
          'DUPLICATE',
          'EXPIRED',
          'INACTIVE',
          'TERMINATED',
        ],
        expiryStartDate: '',
        expiryEndDate: '',
        regionType: '',
        details: '',
        leaseTeamOrganizationId: undefined,
        leaseTeamPersonId: null,
        isReceivable: null,
      }),
    );
  });

  it('searches by pin', async () => {
    const { container, searchButton, setFilter } = await setup();

    fillInput(container, 'searchBy', 'pin', 'select');
    fillInput(container, 'pin', '123');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<ILeaseFilter>({
        lFileNo: '',
        pid: '',
        pin: '123',
        searchBy: 'pin',
        tenantName: '',
        programs: [],
        leaseStatusTypes: [
          'ACTIVE',
          'ARCHIVED',
          'DISCARD',
          'DRAFT',
          'DUPLICATE',
          'EXPIRED',
          'INACTIVE',
          'TERMINATED',
        ],
        expiryStartDate: '',
        expiryEndDate: '',
        regionType: '',
        details: '',
        leaseTeamOrganizationId: undefined,
        leaseTeamPersonId: null,
        isReceivable: null,
      }),
    );
  });

  it('searches by L-file number', async () => {
    const { container, searchButton, setFilter } = await setup();

    fillInput(container, 'searchBy', 'lFileNo', 'select');
    fillInput(container, 'lFileNo', '123');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<ILeaseFilter>({
        lFileNo: '123',
        pid: '',
        pin: '',
        searchBy: 'lFileNo',
        tenantName: '',
        programs: [],
        leaseStatusTypes: [
          'ACTIVE',
          'ARCHIVED',
          'DISCARD',
          'DRAFT',
          'DUPLICATE',
          'EXPIRED',
          'INACTIVE',
          'TERMINATED',
        ],
        expiryStartDate: '',
        expiryEndDate: '',
        regionType: '',
        details: '',
        leaseTeamOrganizationId: undefined,
        leaseTeamPersonId: null,
        isReceivable: null,
      }),
    );
  });

  it('searches tenant name', async () => {
    const { container, searchButton, setFilter } = await setup();

    fillInput(container, 'searchBy', 'pid', 'select');
    fillInput(container, 'tenantName', 'Chester');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<ILeaseFilter>({
        lFileNo: '',
        pid: '',
        pin: '',
        searchBy: 'pid',
        tenantName: 'Chester',
        programs: [],
        leaseStatusTypes: [
          'ACTIVE',
          'ARCHIVED',
          'DISCARD',
          'DRAFT',
          'DUPLICATE',
          'EXPIRED',
          'INACTIVE',
          'TERMINATED',
        ],
        expiryStartDate: '',
        expiryEndDate: '',
        regionType: '',
        details: '',
        leaseTeamOrganizationId: undefined,
        leaseTeamPersonId: null,
        isReceivable: null,
      }),
    );
  });

  it('searches by lease payable/receivable', async () => {
    const { container, searchButton, setFilter } = await setup();

    fillInput(container, 'isReceivable', 'true', 'select');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<Partial<ILeaseFilter>>({
        isReceivable: 'true',
      }),
    );
  });

  it('resets the filter when reset button is clicked', async () => {
    const { container, resetButton, setFilter } = await setup();

    fillInput(container, 'searchBy', 'pid', 'select');
    fillInput(container, 'pid', 'foo-bar-baz');
    await act(async () => userEvent.click(resetButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<ILeaseFilter>({
        lFileNo: '',
        pid: '',
        pin: '',
        searchBy: 'lFileNo',
        tenantName: '',
        programs: [],
        leaseStatusTypes: [
          'ACTIVE',
          'ARCHIVED',
          'DISCARD',
          'DRAFT',
          'DUPLICATE',
          'EXPIRED',
          'INACTIVE',
          'TERMINATED',
        ],
        expiryStartDate: '',
        expiryEndDate: '',
        regionType: '',
        details: '',
        leaseTeamOrganizationId: null,
        leaseTeamPersonId: null,
        isReceivable: null,
      }),
    );
  });
});
