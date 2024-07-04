import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import {
  deleteCompensationRequisitionApi,
  getCompensationRequisitionApi,
  getCompensationRequisitionPropertiesApi,
  putCompensationRequisitionApi,
} from '@/hooks/pims-api/useApiRequisitionCompensations';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_Concepts_AcquisitionFileProperty } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileProperty';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

/**
 * hook that interacts with the Compensation Requisition Endpoint.
 */
export const useCompensationRequisitionRepository = () => {
  const getCompensationRequisition = useApiRequestWrapper<
    (compensationId: number) => Promise<AxiosResponse<ApiGen_Concepts_CompensationRequisition, any>>
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
      compensation: ApiGen_Concepts_CompensationRequisition,
    ) => Promise<AxiosResponse<ApiGen_Concepts_CompensationRequisition, any>>
  >({
    requestFunction: useCallback(
      async (compensation: ApiGen_Concepts_CompensationRequisition) =>
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

  const getCompensationRequisitionProperties = useApiRequestWrapper<
    (
      compensationId: number,
    ) => Promise<AxiosResponse<ApiGen_Concepts_AcquisitionFileProperty[], any>>
  >({
    requestFunction: useCallback(
      async (compensationId: number) =>
        await getCompensationRequisitionPropertiesApi(compensationId),
      [],
    ),
    requestName: 'getCompensationRequisitionProperties',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(
      'Failed to get compensation requisition properties. Refresh the page to try again.',
    ),
  });

  return useMemo(
    () => ({
      deleteCompensation: deleteCompensation,
      updateCompensationRequisition: updateCompensationRequisition,
      getCompensationRequisition: getCompensationRequisition,
      getCompensationRequisitionProperties: getCompensationRequisitionProperties,
    }),
    [
      deleteCompensation,
      getCompensationRequisition,
      getCompensationRequisitionProperties,
      updateCompensationRequisition,
    ],
  );
};
