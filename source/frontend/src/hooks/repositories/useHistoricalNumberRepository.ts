import { AxiosResponse } from 'axios';
import { useCallback, useMemo } from 'react';

import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_Concepts_HistoricalFileNumber } from '@/models/api/generated/ApiGen_Concepts_HistoricalFileNumber';
import { useAxiosErrorHandler } from '@/utils';

import { useApiHistoricalNumbers } from '../pims-api/useApiHistoricalNumbers';

/**
 * hook that interacts with the Historical Number API.
 */
export const useHistoricalNumberRepository = () => {
  const { getByPropertyId: getByPropertyIdApi, putHistoricalNumbers: putHistoricalNumbersApi } =
    useApiHistoricalNumbers();

  const getPropertyHistoricalNumbers = useApiRequestWrapper<
    (propertyId: number) => Promise<AxiosResponse<ApiGen_Concepts_HistoricalFileNumber[], any>>
  >({
    requestFunction: useCallback(
      async (propertyId: number) => await getByPropertyIdApi(propertyId),
      [getByPropertyIdApi],
    ),
    requestName: 'getPropertyHistoricalNumbers',
    onError: useAxiosErrorHandler('Failed to load property historical numbers'),
  });

  const updatePropertyHistoricalNumbers = useApiRequestWrapper<
    (
      propertyId: number,
      historicalNumbers: ApiGen_Concepts_HistoricalNumber[],
    ) => Promise<AxiosResponse<ApiGen_Concepts_HistoricalNumber[], any>>
  >({
    requestFunction: useCallback(
      async (propertyId: number, historicalNumbers: ApiGen_Concepts_HistoricalNumber[]) =>
        await putHistoricalNumbersApi(propertyId, historicalNumbers),
      [putHistoricalNumbersApi],
    ),
    requestName: 'updatePropertyHistoricalNumbers',
    throwError: true,
  });

  return useMemo(
    () => ({
      getPropertyHistoricalNumbers,
      updatePropertyHistoricalNumbers,
    }),
    [getPropertyHistoricalNumbers, updatePropertyHistoricalNumbers],
  );
};
