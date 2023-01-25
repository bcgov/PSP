import { AxiosResponse } from 'axios';
import { FinancialCodeTypes } from 'constants/index';
import { getFinancialCodes, postFinancialCode } from 'hooks/pims-api/useApiFinancialCodes';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { Api_FinancialCode } from 'models/api/FinancialCode';
import { useCallback, useMemo } from 'react';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from 'utils';

const ignoreErrorCodes = [409];

/**
 * hook that interacts with the Financial Codes API.
 */
export const useFinancialCodeRepository = () => {
  const getFinancialCodesApi = useApiRequestWrapper<
    () => Promise<AxiosResponse<Api_FinancialCode[], any>>
  >({
    requestFunction: useCallback(async () => await getFinancialCodes(), []),
    requestName: 'GetFinancialCodes',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to load financial codes. Refresh the page to try again.'),
  });

  const addFinancialCodesApi = useApiRequestWrapper<
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

  return useMemo(
    () => ({
      getFinancialCodes: getFinancialCodesApi,
      addFinancialCode: addFinancialCodesApi,
    }),
    [getFinancialCodesApi, addFinancialCodesApi],
  );
};
