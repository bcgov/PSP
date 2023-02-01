import { AxiosResponse } from 'axios';
import { FinancialCodeTypes } from 'constants/index';
import {
  getFinancialCode,
  getFinancialCodes,
  postFinancialCode,
  putFinancialCode,
} from 'hooks/pims-api/useApiFinancialCodes';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { Api_FinancialCode } from 'models/api/FinancialCode';
import { useCallback, useMemo } from 'react';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from 'utils';

const ignoreErrorCodes = [409];

/**
 * hook that interacts with the Financial Codes API.
 */
export const useFinancialCodeRepository = () => {
  const getAllFinancialCodesApi = useApiRequestWrapper<
    () => Promise<AxiosResponse<Api_FinancialCode[], any>>
  >({
    requestFunction: useCallback(async () => await getFinancialCodes(), []),
    requestName: 'GetFinancialCodes',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to load financial codes. Refresh the page to try again.'),
  });

  const getFinancialCodeByIdApi = useApiRequestWrapper<
    (codeType: FinancialCodeTypes, id: number) => Promise<AxiosResponse<Api_FinancialCode, any>>
  >({
    requestFunction: useCallback(
      async (codeType: FinancialCodeTypes, id: number) => await getFinancialCode(codeType, id),
      [],
    ),
    requestName: 'GetFinancialCodeById',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to load financial code. Refresh the page to try again.'),
  });

  const addFinancialCodeApi = useApiRequestWrapper<
    (
      codeType: FinancialCodeTypes,
      financialCode: Api_FinancialCode,
    ) => Promise<AxiosResponse<Api_FinancialCode, any>>
  >({
    requestFunction: useCallback(
      async (codeType: FinancialCodeTypes, financialCode: Api_FinancialCode) =>
        await postFinancialCode(codeType, financialCode),
      [],
    ),
    requestName: 'AddFinancialCodes',
    skipErrorLogCodes: ignoreErrorCodes,
    throwError: true,
  });

  const updateFinancialCodeApi = useApiRequestWrapper<
    (financialCode: Api_FinancialCode) => Promise<AxiosResponse<Api_FinancialCode, any>>
  >({
    requestFunction: useCallback(
      async (financialCode: Api_FinancialCode) => await putFinancialCode(financialCode),
      [],
    ),
    requestName: 'UpdateFinancialCodes',
    skipErrorLogCodes: ignoreErrorCodes,
    throwError: true,
  });

  return useMemo(
    () => ({
      getFinancialCodes: getAllFinancialCodesApi,
      getFinancialCode: getFinancialCodeByIdApi,
      addFinancialCode: addFinancialCodeApi,
      updateFinancialCode: updateFinancialCodeApi,
    }),
    [getAllFinancialCodesApi, getFinancialCodeByIdApi, addFinancialCodeApi, updateFinancialCodeApi],
  );
};
