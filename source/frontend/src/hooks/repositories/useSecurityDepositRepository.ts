import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { ApiGen_Concepts_SecurityDeposit } from '@/models/api/generated/ApiGen_Concepts_SecurityDeposit';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

import { useApiLeaseDeposits } from '../pims-api/useApiLeaseDeposits';
import { useApiRequestWrapper } from '../util/useApiRequestWrapper';

/**
 * hook that interacts with the security deposit API.
 */
export const useSecurityDepositRepository = () => {
  const {
    putLeaseDeposit,
    putLeaseDepositNote,
    postLeaseDeposit,
    deleteLeaseDeposit,
    getLeaseDeposits,
  } = useApiLeaseDeposits();
  const getSecurityDepositsApi = useApiRequestWrapper<
    (leaseId: number) => Promise<AxiosResponse<ApiGen_Concepts_SecurityDeposit[], any>>
  >({
    requestFunction: useCallback(
      async (leaseId: number) => await getLeaseDeposits(leaseId),
      [getLeaseDeposits],
    ),
    requestName: 'getSecurityDeposits',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(),
  });

  const updateSecurityDepositApi = useApiRequestWrapper<
    (
      leaseId: number,
      securityDeposit: ApiGen_Concepts_SecurityDeposit,
    ) => Promise<AxiosResponse<ApiGen_Concepts_SecurityDeposit, any>>
  >({
    requestFunction: useCallback(
      async (leaseId: number, securityDeposit: ApiGen_Concepts_SecurityDeposit) =>
        await putLeaseDeposit(leaseId, securityDeposit),
      [putLeaseDeposit],
    ),
    requestName: 'putLeaseDeposit',
    onSuccess: useAxiosSuccessHandler('Security Deposit updated successfully.'),
    onError: useAxiosErrorHandler(),
  });

  const updateSecurityDepositNoteApi = useApiRequestWrapper<
    (leaseId: number, note: string) => Promise<AxiosResponse<void, any>>
  >({
    requestFunction: useCallback(
      async (leaseId: number, note: string) => await putLeaseDepositNote(leaseId, note),
      [putLeaseDepositNote],
    ),
    requestName: 'putLeaseDepositNote',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(),
  });

  const addSecurityDepositApi = useApiRequestWrapper<
    (
      leaseId: number,
      securityDeposit: ApiGen_Concepts_SecurityDeposit,
    ) => Promise<AxiosResponse<ApiGen_Concepts_SecurityDeposit, any>>
  >({
    requestFunction: useCallback(
      async (leaseId: number, securityDeposit: ApiGen_Concepts_SecurityDeposit) =>
        await postLeaseDeposit(leaseId, securityDeposit),
      [postLeaseDeposit],
    ),
    requestName: 'postLeaseDeposit',
    onSuccess: useAxiosSuccessHandler('Security Deposit added successfully.'),
    onError: useAxiosErrorHandler(),
  });

  const deleteSecurityDepositApi = useApiRequestWrapper<
    (
      leaseId: number,
      securityDeposit: ApiGen_Concepts_SecurityDeposit,
    ) => Promise<AxiosResponse<void, any>>
  >({
    requestFunction: useCallback(
      async (leaseId: number, securityDeposit: ApiGen_Concepts_SecurityDeposit) =>
        await deleteLeaseDeposit(leaseId, securityDeposit),
      [deleteLeaseDeposit],
    ),
    requestName: 'deleteLeaseDeposit',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(),
  });

  return useMemo(
    () => ({
      deleteSecurityDeposit: deleteSecurityDepositApi,
      addSecurityDeposit: addSecurityDepositApi,
      updateSecurityDepositNote: updateSecurityDepositNoteApi,
      updateSecurityDeposit: updateSecurityDepositApi,
      getSecurityDeposits: getSecurityDepositsApi,
    }),
    [
      deleteSecurityDepositApi,
      addSecurityDepositApi,
      updateSecurityDepositNoteApi,
      updateSecurityDepositApi,
      getSecurityDepositsApi,
    ],
  );
};
