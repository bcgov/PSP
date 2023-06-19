import { useLeaseTermRepository } from 'hooks/repositories/useLeaseTermRepository';
import { usePropertyLeaseRepository } from 'hooks/repositories/usePropertyLeaseRepository';
import useDeepCompareEffect from 'hooks/useDeepCompareEffect';
import { useCallback, useContext } from 'react';

import { useApiLeases } from '@/hooks/pims-api/useApiLeases';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { IApiError } from '@/interfaces/IApiError';
import { logError } from '@/store/slices/network/networkSlice';
import { useAxiosErrorHandler } from '@/utils';

import { LeaseStateContext } from './../context/LeaseContext';

export function useLeaseDetail(leaseId?: number) {
  const { lease, setLease } = useContext(LeaseStateContext);
  leaseId = leaseId ?? lease?.id ?? undefined;
  const { getApiLease } = useApiLeases();
  const {
    getPropertyLeases: { execute: getPropertyLeases, loading: propertyLeasesLoading },
  } = usePropertyLeaseRepository();
  const {
    getLeaseTenants: { execute: getLeaseTenants, loading: leaseTenantsLoading },
  } = useLeaseTenantRepository();
  const {
    getLeaseTerms: { execute: getLeaseTerms, loading: leaseTermsLoading },
  } = useLeaseTermRepository();

  const getApiLeaseById = useApiRequestWrapper({
    requestFunction: getApiLease,
    requestName: 'getApiLease',
    onError: useAxiosErrorHandler('Failed to load lease, reload this page to try again.'),
  });

  const getApiLeaseByIdFunc = getApiLeaseById.execute;

  const func = useCallback(async () => {
    if (leaseId) {
      const leasePromise = getApiLeaseByIdFunc(leaseId);
      const leaseTenantsPromise = getLeaseTenants(leaseId);
      const propertyLeasesPromise = getPropertyLeases(leaseId);
      const leaseTermsPromise = getLeaseTerms(leaseId);
      const [lease, leaseTenants, propertyLeases, leaseTerms] = await Promise.all([
        leasePromise,
        leaseTenantsPromise,
        propertyLeasesPromise,
        leaseTermsPromise,
      ]);
      lease &&
        setLease({
          ...lease,
          tenants: leaseTenants ?? [],
          properties: propertyLeases ?? [],
          terms: leaseTerms ?? [],
        });
    }
  }, [leaseId, getApiLeaseByIdFunc, setLease, getLeaseTenants, getPropertyLeases, getLeaseTerms]);

  useDeepCompareEffect(() => {
    func();
  }, [func]);

  return {
    lease,
    setLease,
    refresh: () => leaseId && func(),
    getApiLeaseById,
    loading:
      getApiLeaseById.loading || propertyLeasesLoading || leaseTenantsLoading || leaseTermsLoading,
  };
}
