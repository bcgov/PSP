import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import {
  getChartOfAccountsCodes,
  getFinancialActivitiesCodes,
  getFinancialCode,
  getFinancialCodes,
  getFinancialCodesByType,
  getResponsibilityCodes,
  getYearlyFinancialCodes,
  postFinancialCode,
  putFinancialCode,
} from '@/hooks/pims-api/useApiFinancialCodes';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_Concepts_FinancialCode } from '@/models/api/generated/ApiGen_Concepts_FinancialCode';
import { ApiGen_Concepts_FinancialCodeTypes } from '@/models/api/generated/ApiGen_Concepts_FinancialCodeTypes';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from '@/utils';

const ignoreErrorCodes = [409];

/**
 * hook that interacts with the Financial Codes API.
 */
export const useFinancialCodeRepository = () => {
  const getAllFinancialCodesApi = useApiRequestWrapper<
    () => Promise<AxiosResponse<ApiGen_Concepts_FinancialCode[], any>>
  >({
    requestFunction: useCallback(async () => await getFinancialCodes(), []),
    requestName: 'GetFinancialCodes',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to load financial codes. Refresh the page to try again.'),
  });

  const getFinancialCodesByTypeApi = useApiRequestWrapper<
    (
      codeType: ApiGen_Concepts_FinancialCodeTypes,
    ) => Promise<AxiosResponse<ApiGen_Concepts_FinancialCode[], any>>
  >({
    requestFunction: useCallback(
      async (codeType: ApiGen_Concepts_FinancialCodeTypes) =>
        await getFinancialCodesByType(codeType),
      [],
    ),
    requestName: 'GetFinancialCodesByType',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to load financial codes. Refresh the page to try again.'),
  });

  const getFinancialActivityCodesApi = useApiRequestWrapper<
    () => Promise<AxiosResponse<ApiGen_Concepts_FinancialCode[], any>>
  >({
    requestFunction: useCallback(async () => await getFinancialActivitiesCodes(), []),
    requestName: 'GetFinancialActivityCodes',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to load financial activity codes.'),
  });

  const getChartOfAccountsCodesApi = useApiRequestWrapper<
    () => Promise<AxiosResponse<ApiGen_Concepts_FinancialCode[], any>>
  >({
    requestFunction: useCallback(async () => await getChartOfAccountsCodes(), []),
    requestName: 'GetChartOfAccountsCodes',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to load chart of account codes.'),
  });

  const getResponsibilityCodesApi = useApiRequestWrapper<
    () => Promise<AxiosResponse<ApiGen_Concepts_FinancialCode[], any>>
  >({
    requestFunction: useCallback(async () => await getResponsibilityCodes(), []),
    requestName: 'GetResponsiblityCodes',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to load responsibility codes.'),
  });

  const getYearlyFinancialsCodesApi = useApiRequestWrapper<
    () => Promise<AxiosResponse<ApiGen_Concepts_FinancialCode[], any>>
  >({
    requestFunction: useCallback(async () => await getYearlyFinancialCodes(), []),
    requestName: 'GetYearlyFinancialCodes',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to load Yearly financial codes.'),
  });

  const getFinancialCodeByIdApi = useApiRequestWrapper<
    (
      codeType: ApiGen_Concepts_FinancialCodeTypes,
      id: number,
    ) => Promise<AxiosResponse<ApiGen_Concepts_FinancialCode, any>>
  >({
    requestFunction: useCallback(
      async (codeType: ApiGen_Concepts_FinancialCodeTypes, id: number) =>
        await getFinancialCode(codeType, id),
      [],
    ),
    requestName: 'GetFinancialCodeById',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to load financial code. Refresh the page to try again.'),
  });

  const addFinancialCodeApi = useApiRequestWrapper<
    (
      codeType: ApiGen_Concepts_FinancialCodeTypes,
      financialCode: ApiGen_Concepts_FinancialCode,
    ) => Promise<AxiosResponse<ApiGen_Concepts_FinancialCode, any>>
  >({
    requestFunction: useCallback(
      async (
        codeType: ApiGen_Concepts_FinancialCodeTypes,
        financialCode: ApiGen_Concepts_FinancialCode,
      ) => await postFinancialCode(codeType, financialCode),
      [],
    ),
    requestName: 'AddFinancialCodes',
    skipErrorLogCodes: ignoreErrorCodes,
    throwError: true,
  });

  const updateFinancialCodeApi = useApiRequestWrapper<
    (
      financialCode: ApiGen_Concepts_FinancialCode,
    ) => Promise<AxiosResponse<ApiGen_Concepts_FinancialCode, any>>
  >({
    requestFunction: useCallback(
      async (financialCode: ApiGen_Concepts_FinancialCode) => await putFinancialCode(financialCode),
      [],
    ),
    requestName: 'UpdateFinancialCodes',
    skipErrorLogCodes: ignoreErrorCodes,
    throwError: true,
  });

  return useMemo(
    () => ({
      getFinancialCodes: getAllFinancialCodesApi,
      getFinancialCodesByType: getFinancialCodesByTypeApi,
      getFinancialActivityCodeTypes: getFinancialActivityCodesApi,
      getChartOfAccountsCodeTypes: getChartOfAccountsCodesApi,
      getResponsibilityCodeTypes: getResponsibilityCodesApi,
      getYearlyFinancialsCodeTypes: getYearlyFinancialsCodesApi,
      getFinancialCode: getFinancialCodeByIdApi,
      addFinancialCode: addFinancialCodeApi,
      updateFinancialCode: updateFinancialCodeApi,
    }),
    [
      getAllFinancialCodesApi,
      getFinancialCodesByTypeApi,
      getFinancialActivityCodesApi,
      getChartOfAccountsCodesApi,
      getResponsibilityCodesApi,
      getYearlyFinancialsCodesApi,
      getFinancialCodeByIdApi,
      addFinancialCodeApi,
      updateFinancialCodeApi,
    ],
  );
};
