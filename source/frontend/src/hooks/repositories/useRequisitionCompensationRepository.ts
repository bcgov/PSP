import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import {
  deleteCompensationRequisitionApi,
  getCompensationRequisitionApi,
  getCompensationRequisitionFinancialsApi,
  getCompensationRequisitionPropertiesApi,
  getFileCompensationsApi,
  postFileCompensationRequisitionApi,
  putCompensationRequisitionApi,
} from '@/hooks/pims-api/useApiRequisitionCompensations';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_AcquisitionFileProperty } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileProperty';
import { ApiGen_Concepts_CompensationFinancial } from '@/models/api/generated/ApiGen_Concepts_CompensationFinancial';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_PropertyLease } from '@/models/api/generated/ApiGen_Concepts_PropertyLease';
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

  const postCompensationRequisition = useApiRequestWrapper<
    (
      fileType: ApiGen_CodeTypes_FileTypes,
      compensationRequisition: ApiGen_Concepts_CompensationRequisition,
    ) => Promise<AxiosResponse<ApiGen_Concepts_CompensationRequisition, any>>
  >({
    requestFunction: useCallback(
      async (
        fileType: ApiGen_CodeTypes_FileTypes,
        compensationRequisition: ApiGen_Concepts_CompensationRequisition,
      ) => await postFileCompensationRequisitionApi(fileType, compensationRequisition),
      [],
    ),
    requestName: 'postCompensation',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to create Compensation requisition.'),
  });

  const updateCompensationRequisition = useApiRequestWrapper<
    (
      fileType: ApiGen_CodeTypes_FileTypes,
      compensation: ApiGen_Concepts_CompensationRequisition,
    ) => Promise<AxiosResponse<ApiGen_Concepts_CompensationRequisition, any>>
  >({
    requestFunction: useCallback(
      async (
        fileType: ApiGen_CodeTypes_FileTypes,
        compensation: ApiGen_Concepts_CompensationRequisition,
      ) => await putCompensationRequisitionApi(fileType, compensation),
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
      fileType: ApiGen_CodeTypes_FileTypes,
      compensationId: number,
    ) => Promise<
      AxiosResponse<
        ApiGen_Concepts_AcquisitionFileProperty[] | ApiGen_Concepts_PropertyLease[],
        any
      >
    >
  >({
    requestFunction: useCallback(
      async (fileType: ApiGen_CodeTypes_FileTypes, compensationId: number) =>
        await getCompensationRequisitionPropertiesApi(fileType, compensationId),
      [],
    ),
    requestName: 'getCompensationRequisitionProperties',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler(
      'Failed to get compensation requisition properties. Refresh the page to try again.',
    ),
  });

  const getFileCompensationRequisitions = useApiRequestWrapper<
    (
      fileType: ApiGen_CodeTypes_FileTypes,
      fileId: number,
    ) => Promise<AxiosResponse<ApiGen_Concepts_CompensationRequisition[], any>>
  >({
    requestFunction: useCallback(
      async (fileType: ApiGen_CodeTypes_FileTypes, fileId: number) =>
        await getFileCompensationsApi(fileType, fileId),
      [],
    ),
    requestName: 'getFileCompensationRequisitions',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to get compensation requisitions for file'),
  });

  const getCompensationRequisitionFinancials = useApiRequestWrapper<
    (compensationId: number) => Promise<AxiosResponse<ApiGen_Concepts_CompensationFinancial[], any>>
  >({
    requestFunction: useCallback(
      async (compensationId: number) =>
        await getCompensationRequisitionFinancialsApi(compensationId),
      [],
    ),
    requestName: 'getCompensationRequisitionFinancials',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to get compensation requisition financials'),
  });

  return useMemo(
    () => ({
      postCompensationRequisition: postCompensationRequisition,
      deleteCompensation: deleteCompensation,
      updateCompensationRequisition: updateCompensationRequisition,
      getCompensationRequisition: getCompensationRequisition,
      getCompensationRequisitionProperties: getCompensationRequisitionProperties,
      getFileCompensationRequisitions: getFileCompensationRequisitions,
      getCompensationRequisitionFinancials: getCompensationRequisitionFinancials,
    }),
    [
      deleteCompensation,
      getCompensationRequisition,
      getCompensationRequisitionFinancials,
      getCompensationRequisitionProperties,
      getFileCompensationRequisitions,
      postCompensationRequisition,
      updateCompensationRequisition,
    ],
  );
};
