import { ILeaseFilter } from '@/features/leases';
import { useApiLeases } from '@/hooks/pims-api/useApiLeases';
import { useUserInfoRepository } from '@/hooks/repositories/useUserInfoRepository';
import { getUserMock } from '@/mocks/user.mock';
import { ApiGen_Base_Page } from '@/models/api/generated/ApiGen_Base_Page';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { ILeaseFilterProps, LeaseFilter } from './LeaseFilter';
import { FormikProps } from 'formik';
import { LeaseFilterModel } from './models/LeaseFilterModel';
import { createRef } from 'react';
import { getMockLookUpsByType, mockLookups } from '@/mocks/lookups.mock';

import * as API from '@/constants/API';
import { MultiSelectOption } from '@/interfaces/MultiSelectOption';

const setFilter = vi.fn();
const onResetFilter = vi.fn();

const initialLeaseStatusTypes: string[] = [
  'ACTIVE',
  'ARCHIVED',
  'DISCARD',
  'DRAFT',
  'DUPLICATE',
  'EXPIRED',
  'INACTIVE',
  'TERMINATED',
];

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

const leaseStatusTypes = getMockLookUpsByType(API.LEASE_STATUS_TYPES);
const leaseStatusOptions: MultiSelectOption[] = leaseStatusTypes.map<MultiSelectOption>(x => {
  return { id: x.value as string, text: x.label };
});
const initialStatusOptions = leaseStatusOptions.filter(x => initialLeaseStatusTypes.includes(x.id));

const mockFilterModel = new LeaseFilterModel([], initialStatusOptions);

describe('Lease Filter', () => {
  const setup = async (renderOptions: RenderOptions & { props?: Partial<ILeaseFilterProps> }) => {
    const formikRef = createRef<FormikProps<LeaseFilterModel>>();
    const utils = render(
      <LeaseFilter
        {...renderOptions.props}
        initialValues={renderOptions.props?.initialValues ?? mockFilterModel}
        pimsRegionsOptions={renderOptions.props?.pimsRegionsOptions ?? []}
        leaseTeamOptions={renderOptions.props?.leaseTeamOptions ?? []}
        leaseStatusOptions={renderOptions.props?.leaseStatusOptions ?? leaseStatusOptions}
        leaseProgramOptions={renderOptions.props?.leaseProgramOptions ?? []}
        setFilter={setFilter}
        onResetFilter={onResetFilter}
      />,
      {
        ...renderOptions,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
      },
    );

    return {
      ...utils,
      formikRef,
      getSearchButton: () => utils.getByTestId('search'),
      getResetButton: () => utils.getByTestId('reset-button'),
    };
  };

  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('matches snapshot', async () => {
    const { asFragment } = await setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it('searches by pid', async () => {
    const { container, getSearchButton } = await setup({});

    fillInput(container, 'searchBy', 'pid', 'select');
    fillInput(container, 'pid', '123');
    await act(async () => userEvent.click(getSearchButton()));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        pid: '123',
        searchBy: 'pid',
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
        leaseTeamOrganizationId: null,
        leaseTeamPersonId: null,
        isReceivable: null,
        regions: [],
      }),
    );
  });

  it('searches by pin', async () => {
    const { container, getSearchButton } = await setup({});

    fillInput(container, 'searchBy', 'pin', 'select');
    fillInput(container, 'pin', '123');
    await act(async () => userEvent.click(getSearchButton()));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<ILeaseFilter>({
        lFileNo: '',
        pid: '',
        pin: '123',
        historical: '',
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
        regions: [],
        details: '',
        leaseTeamOrganizationId: null,
        leaseTeamPersonId: null,
        isReceivable: null,
      }),
    );
  });

  it('searches by L-file number', async () => {
    const { container, getSearchButton } = await setup({});

    fillInput(container, 'searchBy', 'lFileNo', 'select');
    fillInput(container, 'lFileNo', '123');
    await act(async () => userEvent.click(getSearchButton()));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<ILeaseFilter>({
        lFileNo: '123',
        pid: '',
        pin: '',
        historical: '',
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
        regions: [],
        details: '',
        leaseTeamOrganizationId: null,
        leaseTeamPersonId: null,
        isReceivable: null,
      }),
    );
  });

  it('searches tenant name', async () => {
    const { container, getSearchButton } = await setup({});

    fillInput(container, 'searchBy', 'pid', 'select');
    fillInput(container, 'tenantName', 'Chester');
    await act(async () => userEvent.click(getSearchButton()));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<ILeaseFilter>({
        lFileNo: '',
        pid: '',
        pin: '',
        historical: '',
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
        regions: [],
        details: '',
        leaseTeamOrganizationId: null,
        leaseTeamPersonId: null,
        isReceivable: null,
      }),
    );
  });

  it('searches by lease payable/receivable', async () => {
    const { container, getSearchButton } = await setup({});

    fillInput(container, 'isReceivable', 'true', 'select');
    await act(async () => userEvent.click(getSearchButton()));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<Partial<ILeaseFilter>>({
        isReceivable: 'true',
      }),
    );
  });

  it('resets the filter when reset button is clicked', async () => {
    const { container, getResetButton } = await setup({});

    fillInput(container, 'searchBy', 'pid', 'select');
    fillInput(container, 'pid', 'foo-bar-baz');
    await act(async () => userEvent.click(getResetButton()));

    expect(onResetFilter).toHaveBeenCalledTimes(1);
  });
});
