import { createMemoryHistory } from 'history';

import { Claims } from '@/constants/index';
import { IPaginateLeases, useApiLeases } from '@/hooks/pims-api/useApiLeases';
import { useUserInfoRepository } from '@/hooks/repositories/useUserInfoRepository';
import { getEmptyAddress } from '@/mocks/address.mock';
import { getEmptyPerson } from '@/mocks/contacts.mock';
import { mockHistoricalFileNumber } from '@/mocks/historicalFileNumber.mock';
import { getEmptyLeaseStakeholder } from '@/mocks/lease.mock';
import { getEmptyPropertyLease } from '@/mocks/properties.mock';
import { getUserMock } from '@/mocks/user.mock';
import { ApiGen_CodeTypes_HistoricalFileNumberTypes } from '@/models/api/generated/ApiGen_CodeTypes_HistoricalFileNumberTypes';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { getEmptyLease, getEmptyProperty } from '@/models/defaultInitializers';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, render, RenderOptions, screen, userEvent, waitForEffects } from '@/utils/test-utils';

import { LeaseListView } from './LeaseListView';
import { mockLookups } from '@/mocks/lookups.mock';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const history = createMemoryHistory();

vi.mock('@/hooks/pims-api/useApiLeases');
const getLeases = vi.fn();
const exportLeases = vi.fn();
vi.mocked(useApiLeases, { partial: true }).mockReturnValue({
  getLeases,
  getAllLeaseFileTeamMembers: vi.fn().mockResolvedValue({ data: [] }),
  exportLeases,
});

vi.mock('@/hooks/repositories/useUserInfoRepository');
vi.mocked(useUserInfoRepository).mockReturnValue({
  retrieveUserInfo: vi.fn(),
  retrieveUserInfoLoading: true,
  retrieveUserInfoResponse: getUserMock(),
});

// render component under test
const setup = async (renderOptions: RenderOptions = { store: storeState }) => {
  const utils = render(<LeaseListView />, {
    ...renderOptions,
    claims: renderOptions?.claims || [Claims.LEASE_VIEW],
    history,
  });
  await act(async () => {});
  const searchButton = utils.getByTestId('search');
  return { searchButton, ...utils };
};

const setupMockSearch = (searchResults?: ApiGen_Concepts_Lease[]) => {
  const results = searchResults ?? [];
  const len = results.length;
  getLeases.mockResolvedValue({
    data: {
      items: results,
      quantity: len,
      total: len,
      page: 1,
      pageIndex: 0,
    },
  });
};

describe('Lease and License List View', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('matches snapshot', async () => {
    setupMockSearch();
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('searches by pid', async () => {
    setupMockSearch([
      {
        ...getEmptyLease(),
        id: 1,
        lFileNo: 'L-123-456',
        programName: 'TRAN-IT',
        stakeholders: [
          {
            ...getEmptyLeaseStakeholder(),
            person: { ...getEmptyPerson(), firstName: 'Chester', surname: 'Tester' },
          },
        ],
        fileProperties: [
          {
            ...getEmptyPropertyLease(),
            property: {
              ...getEmptyProperty(),
              id: 12,
              address: { ...getEmptyAddress(), streetAddress1: '123 mock st' },
              pid: 123,
            },
          },
        ],
      },
    ]);
    const { container, searchButton, findByText } = await setup();
    waitForEffects();

    await act(async () => {
      fillInput(container, 'searchBy', 'pid', 'select');
      fillInput(container, 'pid', '123');
    });

    await act(async () => userEvent.click(searchButton));
    waitForEffects();

    expect(getLeases).toHaveBeenCalledWith(
      expect.objectContaining<IPaginateLeases>({
        lFileNo: '',
        pid: '123',
        pin: '',
        searchBy: 'pid',
        historical: '',
        tenantName: '',
        programs: [],
        regions: ["1"],
        leaseStatusTypes: [
          'ACTIVE',
          'DRAFT',
          'DUPLICATE',
          'INACTIVE',
          'DISCARD',
          'TERMINATED',
          'ARCHIVED',
        ],
        expiryStartDate: '',
        expiryEndDate: '',
        details: '',
        leaseTeamOrganizationId: null,
        leaseTeamPersonId: null,
        isReceivable: null,
        quantity: 10,
        sort: undefined,
        page: 1,
      }),
    );

    expect(await findByText(/TRAN-IT/i)).toBeInTheDocument();
  });

  it('searches by pin', async () => {
    setupMockSearch([
      {
        ...getEmptyLease(),
        id: 1,
        lFileNo: 'L-123-456',
        programName: 'TRAN-IT',
        stakeholders: [
          {
            ...getEmptyLeaseStakeholder(),
            person: { ...getEmptyPerson(), firstName: 'Chester', surname: 'Tester' },
          },
        ],
        fileProperties: [
          {
            ...getEmptyPropertyLease(),
            property: {
              ...getEmptyProperty(),
              id: 12,
              address: { ...getEmptyAddress(), streetAddress1: '123 mock st' },
              pin: 123,
            },
          },
        ],
      },
    ]);
    const { container, searchButton, findByText } = await setup();

    await act(async () => {
      fillInput(container, 'searchBy', 'pin', 'select');
      fillInput(container, 'pin', '123');
    });
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith(
      expect.objectContaining<IPaginateLeases>({
        lFileNo: '',
        pid: '',
        pin: '123',
        searchBy: 'pin',
        tenantName: '',
        historical: '',
        programs: [],
        leaseStatusTypes: [
          'ACTIVE',
          'DRAFT',
          'DUPLICATE',
          'INACTIVE',
          'DISCARD',
          'TERMINATED',
          'ARCHIVED',
        ],
        regions: ["1"],
        expiryStartDate: '',
        expiryEndDate: '',
        details: '',
        leaseTeamOrganizationId: null,
        leaseTeamPersonId: null,
        isReceivable: null,
        page: 1,
        quantity: 10,
        sort: undefined,
      }),
    );

    expect(await findByText(/TRAN-IT/i)).toBeInTheDocument();
  });

  it('searches by L-file number', async () => {
    setupMockSearch([
      {
        ...getEmptyLease(),
        id: 1,
        lFileNo: 'L-123-456',
        programName: 'TRAN-IT',
        stakeholders: [
          {
            ...getEmptyLeaseStakeholder(),
            person: { ...getEmptyPerson(), firstName: 'Chester', surname: 'Tester' },
          },
        ],
        fileProperties: [
          {
            ...getEmptyPropertyLease(),
            property: {
              ...getEmptyProperty(),
              id: 1234,
              address: { ...getEmptyAddress(), streetAddress1: '123 mock st' },
              pin: 123,
              historicalFileNumbers: [],
            },
          },
        ],
      },
    ]);
    const { container, searchButton, findByText } = await setup();
    fillInput(container, 'searchBy', 'lFileNo', 'select');
    fillInput(container, 'lFileNo', '123');
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith(
      expect.objectContaining({
        lFileNo: '123',
        pid: '',
        pin: '',
        searchBy: 'lFileNo',
        tenantName: '',
      }),
    );

    expect(await findByText(/L-123-456/i)).toBeInTheDocument();
  });

  it('searches historical file number for LIS', async () => {
    setupMockSearch([
      {
        ...getEmptyLease(),
        id: 1,
        lFileNo: 'L-123-456',
        programName: 'TRAN-IT',
        stakeholders: [
          {
            ...getEmptyLeaseStakeholder(),
            person: { ...getEmptyPerson(), firstName: 'Chester', surname: 'Tester' },
          },
        ],
        fileProperties: [
          {
            ...getEmptyPropertyLease(),
            property: {
              ...getEmptyProperty(),
              id: 123,
              address: { ...getEmptyAddress(), streetAddress1: '123 mock st' },
              pin: 123,
              historicalFileNumbers: [
                mockHistoricalFileNumber(
                  1000,
                  123,
                  '0309-001',
                  ApiGen_CodeTypes_HistoricalFileNumberTypes.LISNO.toString(),
                  'LIS #',
                ),
              ],
            },
          },
        ],
      },
    ]);
    const { container, searchButton, findByText } = await setup();

    fillInput(container, 'searchBy', 'historical', 'select');
    fillInput(container, 'historical', '0309-001');
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith(
      expect.objectContaining({
        historical: '0309-001',
        pid: '',
        pin: '',
        searchBy: 'historical',
      }),
    );

    expect(await findByText(/0309-001/i)).toBeInTheDocument();
  });

  it('searches historical file number for PS', async () => {
    setupMockSearch([
      {
        ...getEmptyLease(),
        id: 1,
        lFileNo: 'L-123-456',
        programName: 'TRAN-IT',
        stakeholders: [
          {
            ...getEmptyLeaseStakeholder(),
            person: { ...getEmptyPerson(), firstName: 'Chester', surname: 'Tester' },
          },
        ],
        fileProperties: [
          {
            ...getEmptyPropertyLease(),
            property: {
              ...getEmptyProperty(),
              id: 123,
              address: { ...getEmptyAddress(), streetAddress1: '123 mock st' },
              pin: 123,
              historicalFileNumbers: [
                mockHistoricalFileNumber(
                  1000,
                  123,
                  '0309-000',
                  ApiGen_CodeTypes_HistoricalFileNumberTypes.PSNO.toString(),
                  'PS',
                ),
              ],
            },
          },
        ],
      },
    ]);
    const { container, searchButton, findByText } = await setup();

    fillInput(container, 'searchBy', 'historical', 'select');
    fillInput(container, 'historical', '0309-000');
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith(
      expect.objectContaining({
        historical: '0309-000',
        pid: '',
        pin: '',
        searchBy: 'historical',
      }),
    );

    expect(await findByText(/0309-000/i)).toBeInTheDocument();
  });

  it('searches historical file number for OTHER', async () => {
    setupMockSearch([
      {
        ...getEmptyLease(),
        id: 1,
        lFileNo: 'L-123-456',
        programName: 'TRAN-IT',
        stakeholders: [
          {
            ...getEmptyLeaseStakeholder(),
            person: { ...getEmptyPerson(), firstName: 'Chester', surname: 'Tester' },
          },
        ],
        fileProperties: [
          {
            ...getEmptyPropertyLease(),
            property: {
              ...getEmptyProperty(),
              id: 123,
              address: { ...getEmptyAddress(), streetAddress1: '123 mock st' },
              pin: 123,
              historicalFileNumbers: [
                mockHistoricalFileNumber(
                  1000,
                  123,
                  '0309-999',
                  ApiGen_CodeTypes_HistoricalFileNumberTypes.OTHER.toString(),
                  'Other',
                  'OTHER',
                ),
              ],
            },
          },
        ],
      },
    ]);
    const { container, searchButton, findByText } = await setup();

    fillInput(container, 'searchBy', 'historical', 'select');
    fillInput(container, 'historical', '0309-999');
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith(
      expect.objectContaining({
        historical: '0309-999',
        pid: '',
        pin: '',
        searchBy: 'historical',
      }),
    );

    expect(await findByText(/0309-999/i)).toBeInTheDocument();
  });

  it('searches tenant name', async () => {
    setupMockSearch([
      {
        ...getEmptyLease(),
        id: 1,
        lFileNo: 'L-123-456',
        programName: 'TRAN-IT',
        stakeholders: [
          {
            ...getEmptyLeaseStakeholder(),
            person: { ...getEmptyPerson(), firstName: 'Chester', surname: 'Tester' },
          },
        ],
        fileProperties: [
          {
            ...getEmptyPropertyLease(),
            property: {
              ...getEmptyProperty(),
              id: 123,
              address: { ...getEmptyAddress(), streetAddress1: '123 mock st' },
              pin: 123,
            },
          },
        ],
      },
    ]);
    const { container, searchButton, findByText } = await setup();

    fillInput(container, 'searchBy', 'pid', 'select');
    fillInput(container, 'tenantName', 'Chester');
    await act(async () => userEvent.click(searchButton));

    expect(getLeases).toHaveBeenCalledWith(
      expect.objectContaining<IPaginateLeases>({
        lFileNo: '',
        pid: '',
        pin: '',
        historical: '',
        searchBy: 'pid',
        tenantName: 'Chester',
        programs: [],
        regions: ["1"],
        leaseStatusTypes: [
          'ACTIVE',
          'DRAFT',
          'DUPLICATE',
          'INACTIVE',
          'DISCARD',
          'TERMINATED',
          'ARCHIVED',
        ],
        expiryStartDate: '',
        expiryEndDate: '',
        details: '',
        leaseTeamOrganizationId: null,
        leaseTeamPersonId: null,
        isReceivable: null,
        page: 1,
        quantity: 10,
        sort: undefined,
      }),
    );

    expect(await findByText(/Chester Tester/i)).toBeInTheDocument();
  });

  it('displays an error when no matching records found', async () => {
    setupMockSearch();
    const { container, searchButton, findAllByText } = await setup();

    fillInput(container, 'searchBy', 'pid', 'select');
    fillInput(container, 'pid', 'foo-bar-baz');
    await act(async () => userEvent.click(searchButton));

    const toasts = await findAllByText('Lease / Licence details do not exist in PIMS inventory');
    expect(toasts[0]).toBeVisible();
  });

  it('displays an error when when Search API is unreachable', async () => {
    // simulate a network error
    getLeases.mockRejectedValue(new Error('network error'));
    const { container, searchButton, findAllByText } = await setup();

    fillInput(container, 'searchBy', 'pid', 'select');
    fillInput(container, 'pid', '123');
    await act(async () => userEvent.click(searchButton));

    const toasts = await findAllByText('network error');
    expect(toasts[0]).toBeVisible();
  });

  it('calls export function when export button clicked', async () => {
    setupMockSearch([
      {
        ...getEmptyLease(),
        id: 1,
        lFileNo: 'L-123-456',
        programName: 'TRAN-IT',
        stakeholders: [
          {
            ...getEmptyLeaseStakeholder(),
            person: { ...getEmptyPerson(), firstName: 'Chester', surname: 'Tester' },
          },
        ],
        fileProperties: [
          {
            ...getEmptyPropertyLease(),
            property: {
              ...getEmptyProperty(),
              id: 123,
              address: { ...getEmptyAddress(), streetAddress1: '123 mock st' },
              pin: 123,
              historicalFileNumbers: [
                mockHistoricalFileNumber(
                  1000,
                  123,
                  '0309-000',
                  ApiGen_CodeTypes_HistoricalFileNumberTypes.PSNO.toString(),
                  'PS',
                ),
              ],
            },
          },
        ],
      },
    ]);
    await setup();

    const button = await screen.findByTestId(/excel-icon/i);
    expect(button).toBeVisible();
    await act(async () => userEvent.click(button));
    expect(exportLeases).toHaveBeenCalled();
  });

  it(`renders the 'add disposition' button when user has permissions`, async () => {
    const { getByText } = await setup({ claims: [Claims.LEASE_VIEW, Claims.LEASE_ADD] });
    const button = getByText(/Create a Lease\/Licence/i);
    expect(button).toBeVisible();
  });

  it(`hides the 'add disposition' button when user has no permissions`, async () => {
    const { queryByText } = await setup({ claims: [Claims.LEASE_VIEW] });
    const button = queryByText(/Create a Lease\/Licence/i);
    expect(button).toBeNull();
  });

  it('navigates to create lease route when user clicks add button', async () => {
    const { getByText } = await setup({ claims: [Claims.LEASE_VIEW, Claims.LEASE_ADD] });
    const button = getByText(/Create a Lease\/Licence/i);
    expect(button).toBeVisible();
    await act(async () => userEvent.click(button));
    expect(history.location.pathname).toBe('/mapview/sidebar/lease/new');
  });
});
