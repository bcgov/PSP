import { AxiosResponse } from 'axios';
import { getFinancialCodes } from 'hooks/pims-api/useApiFinancialCodes';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { Api_FinancialCode } from 'models/api/FinancialCode';
import { useCallback, useMemo } from 'react';
import { useAxiosErrorHandler, useAxiosSuccessHandler } from 'utils';

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

  return useMemo(
    () => ({
      getFinancialCodes: getFinancialCodesApi,
    }),
    [getFinancialCodesApi],
  );
};
