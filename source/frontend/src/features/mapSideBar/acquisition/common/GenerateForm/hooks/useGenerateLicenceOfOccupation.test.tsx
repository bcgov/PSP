import { act } from '@testing-library/react';
import { renderHook } from '@testing-library/react-hooks';
import { Provider } from 'react-redux';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';

import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { useApiLeases } from '@/hooks/pims-api/useApiLeases';
import { useInsurancesRepository } from '@/hooks/repositories/useInsuranceRepository';
import { useLeasePeriodRepository } from '@/hooks/repositories/useLeasePeriodRepository';
import { useLeaseRepository } from '@/hooks/repositories/useLeaseRepository';
import { useLeaseStakeholderRepository } from '@/hooks/repositories/useLeaseStakeholderRepository';
import { usePropertyLeaseRepository } from '@/hooks/repositories/usePropertyLeaseRepository';
import { useSecurityDepositRepository } from '@/hooks/repositories/useSecurityDepositRepository';
import { getMockDeposits } from '@/mocks/deposits.mock';
import { getMockApiLease } from '@/mocks/lease.mock';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { ApiGen_CodeTypes_FormTypes } from '@/models/api/generated/ApiGen_CodeTypes_FormTypes';
import { ApiGen_CodeTypes_LeaseLicenceTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseLicenceTypes';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';

import { useGenerateLicenceOfOccupation } from './useGenerateLicenceOfOccupation';

const generateFn = vi
  .fn()
  .mockResolvedValue({ status: ApiGen_CodeTypes_ExternalResponseStatus.Success, payload: {} });
const getLeaseStakeholdersFn = vi.fn();
const getSecurityDepositsFn = vi.fn();
const getInsurancesFn = vi.fn();
const getRenewalsFn = vi.fn();
const getPropertyLeasesFn = vi.fn();
const getApiLeaseFn = vi.fn();
const getLeasePeriodFn = vi.fn();

vi.mock('@/features/documents/hooks/useDocumentGenerationRepository');
vi.mocked(useDocumentGenerationRepository).mockImplementation(
  () =>
    ({
      generateDocumentDownloadWrappedRequest: generateFn,
    } as unknown as ReturnType<typeof useDocumentGenerationRepository>),
);

vi.mock('@/hooks/repositories/useSecurityDepositRepository');
vi.mocked(useSecurityDepositRepository).mockImplementation(
  () =>
    ({
      getSecurityDeposits: { execute: getSecurityDepositsFn },
    } as unknown as ReturnType<typeof useSecurityDepositRepository>),
);

vi.mock('@/hooks/repositories/useLeaseStakeholderRepository');
vi.mocked(useLeaseStakeholderRepository).mockImplementation(
  () =>
    ({
      getLeaseStakeholders: { execute: getLeaseStakeholdersFn },
    } as unknown as ReturnType<typeof useLeaseStakeholderRepository>),
);

vi.mock('@/hooks/repositories/useInsuranceRepository');
vi.mocked(useInsurancesRepository).mockImplementation(
  () =>
    ({
      getInsurances: { execute: getInsurancesFn },
    } as unknown as ReturnType<typeof useInsurancesRepository>),
);

vi.mock('@/hooks/repositories/usePropertyLeaseRepository');
vi.mocked(usePropertyLeaseRepository).mockImplementation(
  () =>
    ({
      getPropertyLeases: { execute: getPropertyLeasesFn },
    } as unknown as ReturnType<typeof usePropertyLeaseRepository>),
);

vi.mock('@/hooks/repositories/useLeasePeriodRepository');
vi.mocked(useLeasePeriodRepository).mockImplementation(
  () =>
    ({
      getLeasePeriods: { execute: getLeasePeriodFn },
    } as unknown as ReturnType<typeof useLeasePeriodRepository>),
);

vi.mock('@/hooks/pims-api/useApiLeases');
vi.mocked(useApiLeases).mockImplementation(
  () =>
    ({
      getApiLease: getApiLeaseFn,
    } as unknown as ReturnType<typeof useApiLeases>),
);

vi.mock('@/hooks/repositories/useLeaseRepository');
vi.mocked(useLeaseRepository).mockImplementation(
  () =>
    ({
      getLeaseRenewals: { execute: getRenewalsFn },
    } as unknown as ReturnType<typeof useLeaseRepository>),
);

let currentStore: MockStoreEnhanced<any, {}>;
const mockStore = configureMockStore([thunk]);
const getStore = (values?: any) => {
  currentStore = mockStore(values ?? {});
  return currentStore;
};
const getWrapper =
  (store: any) =>
  ({ children }: any) =>
    <Provider store={store}>{children}</Provider>;

const setup = (params?: {
  storeValues?: any;
  acquisitionResponse?: ApiGen_Concepts_AcquisitionFile;
}) => {
  const { result } = renderHook(useGenerateLicenceOfOccupation, {
    wrapper: getWrapper(getStore(params?.storeValues)),
  });
  return result.current;
};

describe('useGenerateLicenceOfOccupation functions', () => {
  beforeEach(() => {
    getSecurityDepositsFn.mockResolvedValue(getMockDeposits());
    getLeaseStakeholdersFn.mockResolvedValue(getMockApiLease().stakeholders);
    getApiLeaseFn.mockResolvedValue({ data: getMockApiLease() });
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('makes requests to expected api endpoints', async () => {
    const generate = setup();
    await act(async () => {
      generate(getMockApiLease());
    });
    expect(generateFn).toHaveBeenCalled();
    expect(getLeaseStakeholdersFn).toHaveBeenCalled();
    expect(getInsurancesFn).toHaveBeenCalled();
    expect(getRenewalsFn).toHaveBeenCalled();
    expect(getSecurityDepositsFn).toHaveBeenCalled();
    expect(getLeasePeriodFn).toHaveBeenCalled();
    expect(getPropertyLeasesFn).toHaveBeenCalled();
    expect(getApiLeaseFn).toHaveBeenCalled();
  });

  it('throws an error if no acquisition file is found', async () => {
    const generate = setup();
    getApiLeaseFn.mockResolvedValue({ data: null });
    await act(() =>
      expect(generate(getMockApiLease())).rejects.toThrow(
        'Failed to load lease, reload this page to try again.',
      ),
    );
  });

  it('throws an error if licence type is not valid', async () => {
    const mockLease = getMockApiLease();
    mockLease.type = {
      id: ApiGen_CodeTypes_LeaseLicenceTypes.OTHER.toString(),
      description: 'OTHER',
      isDisabled: false,
      displayOrder: 10,
    };

    const generate = setup();
    await act(async () => {
      generate(mockLease);
    });

    getApiLeaseFn.mockResolvedValue({ data: mockLease });
    await act(() => expect(generate(mockLease)).rejects.toThrow('Invalid licence type.'));
  });

  it('throws an error if generation api call is unsuccessful', async () => {
    generateFn.mockResolvedValue({
      status: ApiGen_CodeTypes_ExternalResponseStatus.Error,
      payload: null,
    });
    const generate = setup();
    await act(() => expect(generate(getMockApiLease())).rejects.toThrow('Failed to generate file'));
  });

  it('it generates the form with template for H1005A', async () => {
    const mockLease = getMockApiLease();
    const generate = setup();
    getApiLeaseFn.mockResolvedValue({ data: mockLease });
    generateFn.mockResolvedValue({
      status: ApiGen_CodeTypes_ExternalResponseStatus.Success,
      payload: {},
    });

    await act(async () => {
      generate(mockLease);
    });

    expect(generateFn).toHaveBeenCalledWith(
      expect.objectContaining({
        templateType: ApiGen_CodeTypes_FormTypes.H1005A,
      }),
    );
    expect(getLeaseStakeholdersFn).toHaveBeenCalled();
    expect(getInsurancesFn).toHaveBeenCalled();
    expect(getSecurityDepositsFn).toHaveBeenCalled();
    expect(getLeasePeriodFn).toHaveBeenCalled();
    expect(getRenewalsFn).toHaveBeenCalled();
    expect(getPropertyLeasesFn).toHaveBeenCalled();
    expect(getApiLeaseFn).toHaveBeenCalled();
  });

  it('it generates the form with template for H1005 - Public Highway', async () => {
    const mockLease = getMockApiLease();
    mockLease.type.id = ApiGen_CodeTypes_LeaseLicenceTypes.LIPPUBHWY;

    getApiLeaseFn.mockResolvedValue({ data: mockLease });
    generateFn.mockResolvedValue({
      status: ApiGen_CodeTypes_ExternalResponseStatus.Success,
      payload: {},
    });

    const generate = setup();
    await act(async () => {
      generate(mockLease);
    });

    expect(generateFn).toHaveBeenCalledWith(
      expect.objectContaining({
        templateType: ApiGen_CodeTypes_FormTypes.H1005,
      }),
    );
    expect(getLeaseStakeholdersFn).toHaveBeenCalled();
    expect(getInsurancesFn).toHaveBeenCalled();
    expect(getSecurityDepositsFn).toHaveBeenCalled();
    expect(getLeasePeriodFn).toHaveBeenCalled();
    expect(getRenewalsFn).toHaveBeenCalled();
    expect(getPropertyLeasesFn).toHaveBeenCalled();
    expect(getApiLeaseFn).toHaveBeenCalled();
  });
});
