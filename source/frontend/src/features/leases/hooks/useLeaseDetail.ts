import { useCallback, useContext } from 'react';

import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import { useApiLeases } from '@/hooks/pims-api/useApiLeases';
import { useLeasePeriodRepository } from '@/hooks/repositories/useLeasePeriodRepository';
import { useLeaseRepository } from '@/hooks/repositories/useLeaseRepository';
import { useLeaseStakeholderRepository } from '@/hooks/repositories/useLeaseStakeholderRepository';
import { usePropertyLeaseRepository } from '@/hooks/repositories/usePropertyLeaseRepository';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { exists, useAxiosErrorHandler } from '@/utils';

import { LeaseStateContext } from './../context/LeaseContext';

export function useLeaseDetail(leaseId?: number) {
  const { lease, setLease } = useContext(LeaseStateContext);
  const { setFile } = useContext(SideBarContext);

  leaseId = leaseId ?? lease?.id ?? undefined;
  const { getApiLease } = useApiLeases();
  const {
    getPropertyLeases: { execute: getPropertyLeases, loading: propertyLeasesLoading },
  } = usePropertyLeaseRepository();
  const {
    getLeaseStakeholders: { execute: getLeaseStakeholders, loading: getLeaseStakeholdersLoading },
  } = useLeaseStakeholderRepository();
  const {
    getLeasePeriods: { execute: getLeasePeriods, loading: leasePeriodsLoading },
  } = useLeasePeriodRepository();

  const {
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
      const leaseTenantsPromise = getLeaseStakeholders(leaseId);
      const propertyLeasesPromise = getPropertyLeases(leaseId);
      const leasePeriodsPromise = getLeasePeriods(leaseId);
      const leaseChecklistPromise = getLeaseChecklist(leaseId);
      const leaseRenewalsPromise = getLeaseRenewals(leaseId);

      const [
        leaseResponse,
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

      if (exists(leaseResponse)) {
        const mergedLeases: ApiGen_Concepts_Lease = {
          ...leaseResponse,
          stakeholders: leaseTenants ?? [],
          fileProperties: propertyLeases ?? [],
          periods: leasePeriods ?? [],
          fileChecklistItems: leaseChecklistItems ?? [],
          renewals: leaseRenewals,
        };

        setLease(mergedLeases);
        setFile({ ...mergedLeases, fileType: ApiGen_CodeTypes_FileTypes.Lease });

        return mergedLeases;
      } else {
        setFile(undefined);
      }

      return;
    }
  }, [
    leaseId,
    getApiLeaseByIdFunc,
    getLeaseStakeholders,
    getPropertyLeases,
    getLeasePeriods,
    getLeaseChecklist,
    getLeaseRenewals,
    setLease,
    setFile,
  ]);

  const loading =
    getApiLeaseById.loading ||
    propertyLeasesLoading ||
    getLeaseStakeholdersLoading ||
    leasePeriodsLoading ||
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
      await getCompleteLease();
    },
    getCompleteLease,
    loading: loading,
  };
}
