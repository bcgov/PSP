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
  const { getByPropertyId: getByPropertyIdApi } = useApiHistoricalNumbers();

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

  return useMemo(
    () => ({
      getPropertyHistoricalNumbers,
    }),
    [getPropertyHistoricalNumbers],
  );
};
