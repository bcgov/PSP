import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils/axiosUtils';

import { ApiGen_Concepts_LeaseStakeholder } from '../../models/api/generated/ApiGen_Concepts_LeaseStakeholder';
import { getLeaseStakeholders, updateLeaseStakeholders } from '../pims-api/useApiLeaseStakeholders';
import { useApiRequestWrapper } from '../util/useApiRequestWrapper';

/**
 * hook that interacts with the property improvements API.
 */
export const useLeaseStakeholderRepository = () => {
  const getLeaseStakeholdersApi = useApiRequestWrapper<
    (leaseId: number) => Promise<AxiosResponse<ApiGen_Concepts_LeaseStakeholder[], any>>
  >({
    requestFunction: useCallback(
      async (leaseId: number) => await getLeaseStakeholders(leaseId),
      [],
    ),
    requestName: 'getLeaseStakeholders',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(),
  });

  const updateLeaseStakeholdersApi = useApiRequestWrapper<
    (
      leaseId: number,
      stakeholders: ApiGen_Concepts_LeaseStakeholder[],
    ) => Promise<AxiosResponse<ApiGen_Concepts_LeaseStakeholder[], any>>
  >({
    requestFunction: useCallback(
      async (leaseId: number, stakeholders: ApiGen_Concepts_LeaseStakeholder[]) =>
        await updateLeaseStakeholders(leaseId, stakeholders),
      [],
    ),
    requestName: 'updateLeaseStakeholders',
    onSuccess: useAxiosSuccessHandler('Stakeholders saved successfully.'),
    throwError: true,
  });

  return useMemo(
    () => ({
      getLeaseStakeholders: getLeaseStakeholdersApi,
      updateLeaseStakeholders: updateLeaseStakeholdersApi,
    }),
    [getLeaseStakeholdersApi, updateLeaseStakeholdersApi],
  );
};
