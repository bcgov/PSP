import { act } from '@testing-library/react';
import { renderHook } from '@testing-library/react-hooks';
import { Provider } from 'react-redux';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';

import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { useApiLeases } from '@/hooks/pims-api/useApiLeases';
import { useInsurancesRepository } from '@/hooks/repositories/useInsuranceRepository';
import { useLeaseTenantRepository } from '@/hooks/repositories/useLeaseTenantRepository';
import { useLeaseTermRepository } from '@/hooks/repositories/useLeaseTermRepository';
import { usePropertyLeaseRepository } from '@/hooks/repositories/usePropertyLeaseRepository';
import { useSecurityDepositRepository } from '@/hooks/repositories/useSecurityDepositRepository';
import { getMockDeposits } from '@/mocks/deposits.mock';
import { getMockApiLease } from '@/mocks/lease.mock';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_LeaseTenant } from '@/models/api/generated/ApiGen_Concepts_LeaseTenant';

import { useGenerateH1005a } from './useGenerateH1005a';

const generateFn = vi
  .fn()
  .mockResolvedValue({ status: ApiGen_CodeTypes_ExternalResponseStatus.Success, payload: {} });
const getLeaseTenantsFn = vi.fn();
const getSecurityDepositsFn = vi.fn();
const getInsurancesFn = vi.fn();
const getPropertyLeasesFn = vi.fn();
const getApiLeaseFn = vi.fn();
const getLeaseTermFn = vi.fn();

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

vi.mock('@/hooks/repositories/useLeaseTenantRepository');
vi.mocked(useLeaseTenantRepository).mockImplementation(
  () =>
    ({
      getLeaseTenants: { execute: getLeaseTenantsFn },
    } as unknown as ReturnType<typeof useLeaseTenantRepository>),
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

vi.mock('@/hooks/repositories/useLeaseTermRepository');
vi.mocked(useLeaseTermRepository).mockImplementation(
  () =>
    ({
      getLeaseTerms: { execute: getLeaseTermFn },
    } as unknown as ReturnType<typeof useLeaseTermRepository>),
);

vi.mock('@/hooks/pims-api/useApiLeases');
vi.mocked(useApiLeases).mockImplementation(
  () =>
    ({
      getApiLease: getApiLeaseFn,
    } as unknown as ReturnType<typeof useApiLeases>),
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
  const { result } = renderHook(useGenerateH1005a, {
    wrapper: getWrapper(getStore(params?.storeValues)),
  });
  return result.current;
};

describe('useGenerateH10005a functions', () => {
  beforeEach(() => {
    getSecurityDepositsFn.mockResolvedValue(getMockDeposits());
    getLeaseTenantsFn.mockResolvedValue(getMockApiLease().tenants);
    getApiLeaseFn.mockResolvedValue({ data: getMockApiLease() });
  });

  it('makes requests to expected api endpoints', async () => {
    const generate = setup();
    await act(async () => {
      generate(getMockApiLease());
    });
    expect(generateFn).toHaveBeenCalled();
    expect(getLeaseTenantsFn).toHaveBeenCalled();
    expect(getInsurancesFn).toHaveBeenCalled();
    expect(getSecurityDepositsFn).toHaveBeenCalled();
    expect(getLeaseTermFn).toHaveBeenCalled();
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

  it('throws an error if generation api call is unsuccessful', async () => {
    generateFn.mockResolvedValue({
      status: ApiGen_CodeTypes_ExternalResponseStatus.Error,
      payload: null,
    });
    const generate = setup();
    await act(() => expect(generate(getMockApiLease())).rejects.toThrow('Failed to generate file'));
  });
});
