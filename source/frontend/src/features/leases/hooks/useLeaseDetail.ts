import { useCallback, useContext, useEffect } from 'react';

import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import { useApiLeases } from '@/hooks/pims-api/useApiLeases';
import { useLeasePeriodRepository } from '@/hooks/repositories/useLeasePeriodRepository';
import { useLeaseRepository } from '@/hooks/repositories/useLeaseRepository';
import { useLeaseTenantRepository } from '@/hooks/repositories/useLeaseTenantRepository';
import { usePropertyLeaseRepository } from '@/hooks/repositories/usePropertyLeaseRepository';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { useAxiosErrorHandler } from '@/utils';

import { LeaseStateContext } from './../context/LeaseContext';

export function useLeaseDetail(leaseId?: number) {
  const { lease, setLease } = useContext(LeaseStateContext);
  const { setLastUpdatedBy, lastUpdatedBy, setStaleLastUpdatedBy, staleLastUpdatedBy } =
    useContext(SideBarContext);

  leaseId = leaseId ?? lease?.id ?? undefined;
  const { getApiLease } = useApiLeases();
  const {
    getPropertyLeases: { execute: getPropertyLeases, loading: propertyLeasesLoading },
  } = usePropertyLeaseRepository();
  const {
    getLeaseTenants: { execute: getLeaseTenants, loading: leaseTenantsLoading },
  } = useLeaseTenantRepository();
  const {
    getLeasePeriods: { execute: getLeasePeriods, loading: leasePeriodsLoading },
  } = useLeasePeriodRepository();

  const {
    getLastUpdatedBy: { execute: getLastUpdatedBy, loading: getLastUpdatedByLoading },
    getLeaseChecklist: { execute: getLeaseChecklist, loading: getLeaseChecklistLoading },
    getLeaseRenewals: { execute: getLeaseRenewals, loading: getLeaseRenewalsLoading },
  } = useLeaseRepository();

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
      const leasePeriodsPromise = getLeasePeriods(leaseId);
      const leaseChecklistPromise = getLeaseChecklist(leaseId);
      const leaseRenewalsPromise = getLeaseRenewals(leaseId);

      const [
        lease,
        leaseTenants,
        propertyLeases,
        leasePeriods,
        leaseChecklistItems,
        leaseRenewals,
      ] = await Promise.all([
        leasePromise,
        leaseTenantsPromise,
        propertyLeasesPromise,
        leasePeriodsPromise,
        leaseChecklistPromise,
        leaseRenewalsPromise,
      ]);
      if (lease) {
        const mergedLeases: ApiGen_Concepts_Lease = {
          ...lease,
          stakeholders: leaseTenants ?? [],
          fileProperties: propertyLeases ?? [],
          periods: leasePeriods ?? [],
          fileChecklistItems: leaseChecklistItems ?? [],
          renewals: leaseRenewals,
        };
        setLease(mergedLeases);
        return mergedLeases;
      }
      return undefined;
    }
  }, [
    leaseId,
    getApiLeaseByIdFunc,
    getLeaseTenants,
    getPropertyLeases,
    getLeasePeriods,
    getLeaseChecklist,
    getLeaseRenewals,
    setLease,
  ]);

  const fetchLastUpdatedBy = useCallback(async () => {
    if (leaseId) {
      const retrieved = await getLastUpdatedBy(leaseId);
      if (retrieved !== undefined) {
        setLastUpdatedBy(retrieved);
      } else {
        setLastUpdatedBy(null);
      }
    }
  }, [leaseId, getLastUpdatedBy, setLastUpdatedBy]);

  useEffect(() => {
    if (lastUpdatedBy === undefined || leaseId !== lastUpdatedBy?.parentId || staleLastUpdatedBy) {
      fetchLastUpdatedBy();
    }
  }, [fetchLastUpdatedBy, lastUpdatedBy, leaseId, staleLastUpdatedBy]);

  const loading =
    getApiLeaseById.loading ||
    propertyLeasesLoading ||
    leaseTenantsLoading ||
    leasePeriodsLoading ||
    getLastUpdatedByLoading ||
    getLeaseRenewalsLoading ||
    getLeaseChecklistLoading;

  useDeepCompareEffect(() => {
    if (!lease) {
      getCompleteLease();
    }
  }, [getCompleteLease, lease]);

  return {
    lease,
    setLease,
    refresh: async () => {
      setStaleLastUpdatedBy(true);
      await getCompleteLease();
    },
    getCompleteLease,
    loading: loading,
  };
}
