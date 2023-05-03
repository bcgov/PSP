import { AxiosResponse } from 'axios';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import {
  deleteRequisitionCompensationApi,
  getFileRequisitionCompensationsApi,
} from 'hooks/pims-api/useApiRequisitionCompensations';
import { Api_Compensation } from 'models/api/Compensation';
import { useCallback, useMemo } from 'react';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from 'utils';

/**
 * hook that interacts with the Compensation API.
 */
export const useRequisitionCompensationRepository = () => {
  const getFileCompensations = useApiRequestWrapper<
    (fileId: number) => Promise<AxiosResponse<Api_Compensation[], any>>
  >({
    requestFunction: useCallback(
      async (fileId: number) => await getFileRequisitionCompensationsApi(fileId),
      [],
    ),
    requestName: 'getFileCompensations',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(
      'Failed to load requisition compensations. Refresh the page to try again.',
    ),
  });

  const deleteCompensation = useApiRequestWrapper<
    (compensationId: number) => Promise<AxiosResponse<boolean, any>>
  >({
    requestFunction: useCallback(
      async (compensationId: number) => await deleteRequisitionCompensationApi(compensationId),
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
      getFileCompensations: getFileCompensations,
      deleteCompensation: deleteCompensation,
    }),
    [getFileCompensations, deleteCompensation],
  );
};
