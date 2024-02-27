import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { ApiGen_Concepts_SecurityDepositReturn } from '@/models/api/generated/ApiGen_Concepts_SecurityDepositReturn';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

import { useApiLeaseDepositReturns } from '../pims-api/useApiLeaseDepositsReturn';
import { useApiRequestWrapper } from '../util/useApiRequestWrapper';

/**
 * hook that interacts with the security deposit return API.
 */
export const useSecurityDepositReturnRepository = () => {
  const { deleteLeaseDepositReturn, postLeaseDepositReturn, putLeaseDepositReturn } =
    useApiLeaseDepositReturns();

  const updateSecurityDepositReturnApi = useApiRequestWrapper<
    (
      leaseId: number,
      securityDepositReturn: ApiGen_Concepts_SecurityDepositReturn,
    ) => Promise<AxiosResponse<ApiGen_Concepts_SecurityDepositReturn, any>>
  >({
    requestFunction: useCallback(
      async (leaseId: number, securityDepositReturn: ApiGen_Concepts_SecurityDepositReturn) =>
        await putLeaseDepositReturn(leaseId, securityDepositReturn),
      [putLeaseDepositReturn],
    ),
    requestName: 'putLeaseDepositReturn',
    onSuccess: useAxiosSuccessHandler('Security Deposit Return updated successfully.'),
    onError: useAxiosErrorHandler(),
  });

  const addSecurityDepositReturnApi = useApiRequestWrapper<
    (
      leaseId: number,
      securityDepositReturn: ApiGen_Concepts_SecurityDepositReturn,
    ) => Promise<AxiosResponse<ApiGen_Concepts_SecurityDepositReturn, any>>
  >({
    requestFunction: useCallback(
      async (leaseId: number, securityDepositReturn: ApiGen_Concepts_SecurityDepositReturn) =>
        await postLeaseDepositReturn(leaseId, securityDepositReturn),
      [postLeaseDepositReturn],
    ),
    requestName: 'postLeaseDepositReturn',
    onSuccess: useAxiosSuccessHandler('Security Deposit Return added successfully.'),
    onError: useAxiosErrorHandler(),
  });

  const deleteSecurityDepositReturnApi = useApiRequestWrapper<
    (
      leaseId: number,
      securityDepositReturn: ApiGen_Concepts_SecurityDepositReturn,
    ) => Promise<AxiosResponse<void, any>>
  >({
    requestFunction: useCallback(
      async (leaseId: number, securityDepositReturn: ApiGen_Concepts_SecurityDepositReturn) =>
        await deleteLeaseDepositReturn(leaseId, securityDepositReturn),
      [deleteLeaseDepositReturn],
    ),
    requestName: 'deleteLeaseDepositReturn',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(),
  });

  return useMemo(
    () => ({
      deleteSecurityDepositReturn: deleteSecurityDepositReturnApi,
      addSecurityDepositReturn: addSecurityDepositReturnApi,
      updateSecurityDepositReturn: updateSecurityDepositReturnApi,
    }),
    [deleteSecurityDepositReturnApi, addSecurityDepositReturnApi, updateSecurityDepositReturnApi],
  );
};
