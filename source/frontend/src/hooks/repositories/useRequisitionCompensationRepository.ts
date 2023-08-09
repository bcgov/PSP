import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import {
  deleteCompensationRequisitionApi,
  getCompensationRequisitionApi,
  putCompensationRequisitionApi,
} from '@/hooks/pims-api/useApiRequisitionCompensations';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { Api_CompensationRequisition } from '@/models/api/CompensationRequisition';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

/**
 * hook that interacts with the Compensation API.
 */
export const useCompensationRequisitionRepository = () => {
  const getCompensationRequisition = useApiRequestWrapper<
    (compensationId: number) => Promise<AxiosResponse<Api_CompensationRequisition, any>>
  >({
    requestFunction: useCallback(
      async (compensationId: number) => await getCompensationRequisitionApi(compensationId),
      [],
    ),
    requestName: 'getCompensation',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to load Compensation requisition.'),
  });

  const updateCompensationRequisition = useApiRequestWrapper<
    (
      compensation: Api_CompensationRequisition,
    ) => Promise<AxiosResponse<Api_CompensationRequisition, any>>
  >({
    requestFunction: useCallback(
      async (compensation: Api_CompensationRequisition) =>
        await putCompensationRequisitionApi(compensation),
      [],
    ),
    requestName: 'updateCompensation',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to update Compensation requisition.'),
  });

  const deleteCompensation = useApiRequestWrapper<
    (compensationId: number) => Promise<AxiosResponse<boolean, any>>
  >({
    requestFunction: useCallback(
      async (compensationId: number) => await deleteCompensationRequisitionApi(compensationId),
      [],
    ),
    requestName: 'deleteCompensation',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(
      'Failed to delete requisition compensation. Refresh the page to try again.',
    ),
  });

  return useMemo(
    () => ({
      deleteCompensation: deleteCompensation,
      updateCompensationRequisition: updateCompensationRequisition,
      getCompensationRequisition: getCompensationRequisition,
    }),
    [deleteCompensation, getCompensationRequisition, updateCompensationRequisition],
  );
};
