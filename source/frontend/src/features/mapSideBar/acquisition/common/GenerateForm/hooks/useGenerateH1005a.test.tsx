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

const generateFn = jest
  .fn()
  .mockResolvedValue({ status: ApiGen_CodeTypes_ExternalResponseStatus.Success, payload: {} });
const getLeaseTenantsFn = jest.fn<Promise<ApiGen_Concepts_LeaseTenant[] | null>, any[]>();
const getSecurityDepositsFn = jest.fn();
const getInsurancesFn = jest.fn();
const getPropertyLeasesFn = jest.fn();
const getApiLeaseFn = jest.fn();
const getLeaseTermFn = jest.fn();

jest.mock('@/features/documents/hooks/useDocumentGenerationRepository');
(useDocumentGenerationRepository as jest.Mock).mockImplementation(() => ({
  generateDocumentDownloadWrappedRequest: generateFn,
}));

jest.mock('@/hooks/repositories/useSecurityDepositRepository');
(useSecurityDepositRepository as jest.Mock).mockImplementation(() => ({
  getSecurityDeposits: { execute: getSecurityDepositsFn },
}));

jest.mock('@/hooks/repositories/useLeaseTenantRepository');
(useLeaseTenantRepository as jest.Mock).mockImplementation(() => ({
  getLeaseTenants: { execute: getLeaseTenantsFn },
}));

jest.mock('@/hooks/repositories/useInsuranceRepository');
(useInsurancesRepository as jest.Mock).mockImplementation(() => ({
  getInsurances: { execute: getInsurancesFn },
}));

jest.mock('@/hooks/repositories/usePropertyLeaseRepository');
(usePropertyLeaseRepository as jest.Mock).mockImplementation(() => ({
  getPropertyLeases: { execute: getPropertyLeasesFn },
}));

jest.mock('@/hooks/repositories/useLeaseTermRepository');
(useLeaseTermRepository as jest.Mock).mockImplementation(() => ({
  getLeaseTerms: { execute: getLeaseTermFn },
}));

jest.mock('@/hooks/pims-api/useApiLeases');
(useApiLeases as jest.Mock).mockImplementation(() => ({
  getApiLease: getApiLeaseFn,
}));

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
    await expect(generate(getMockApiLease())).rejects.toThrow(
      'Failed to load lease, reload this page to try again.',
    );
  });

  it('throws an error if generation api call is unsuccessful', async () => {
    generateFn.mockResolvedValue({
      status: ApiGen_CodeTypes_ExternalResponseStatus.Error,
      payload: null,
    });
    const generate = setup();
    await expect(generate(getMockApiLease())).rejects.toThrow('Failed to generate file');
  });
});
