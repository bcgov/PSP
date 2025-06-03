import { AxiosResponse } from 'axios';
import { FeatureCollection } from 'geojson';
import { useCallback, useMemo } from 'react';

import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { useAxiosErrorHandler } from '@/utils';

import { useParcelMap } from '../layer-api/useParcelMap';

/**
 * hook that retrieves a property from the inventory.
 */
export const useParcelMapRepository = () => {
  const { queryParcelMap } = useParcelMap();

  const queryParcelMapWrapper = useApiRequestWrapper<
    (query: string) => Promise<AxiosResponse<FeatureCollection, any>>
  >({
    requestFunction: useCallback(
      async (query: string) => {
        return await queryParcelMap(query);
      },
      [queryParcelMap],
    ),
    requestName: 'queryParcelMapWrapper',
    onError: useAxiosErrorHandler('Failed to retrieve parcel map query results.'),
  });

  return useMemo(
    () => ({
      queryParcelMapWrapper,
    }),
    [queryParcelMapWrapper],
  );
};
