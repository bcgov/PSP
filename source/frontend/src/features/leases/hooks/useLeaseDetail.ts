import { useCallback, useContext } from 'react';

import { useApiLeases } from '@/hooks/pims-api/useApiLeases';
import { useLeaseTenantRepository } from '@/hooks/repositories/useLeaseTenantRepository';
import { useLeaseTermRepository } from '@/hooks/repositories/useLeaseTermRepository';
import { usePropertyLeaseRepository } from '@/hooks/repositories/usePropertyLeaseRepository';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import { Api_Lease } from '@/models/api/Lease';
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

  const getCompleteLease = useCallback(async () => {
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
      if (!!lease) {
        const mergedLeases: Api_Lease = {
          ...lease,
          tenants: leaseTenants ?? [],
          properties: propertyLeases ?? [],
          terms: leaseTerms ?? [],
        };
        setLease(mergedLeases);
        return mergedLeases;
      }
      return undefined;
    }
  }, [leaseId, getApiLeaseByIdFunc, setLease, getLeaseTenants, getPropertyLeases, getLeaseTerms]);

  const loading =
    getApiLeaseById.loading || propertyLeasesLoading || leaseTenantsLoading || leaseTermsLoading;
  useDeepCompareEffect(() => {
    if (!lease) {
      getCompleteLease();
    }
  }, [getCompleteLease, lease]);

  return {
    lease,
    setLease,
    refresh: () => leaseId && getCompleteLease(),
    getCompleteLease,
    loading: loading,
  };
}
