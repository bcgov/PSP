import { FeatureCollection } from 'geojson';
import { useMemo } from 'react';

import { useTenant } from '@/tenants/useTenant';

import { wfsAxios } from './wfsAxios';

export const useParcelMap = () => {
  const { parcelMapQueryUrl } = useTenant();

  return useMemo(
    () => ({
      queryParcelMap: (query: string | null) =>
        wfsAxios().get<FeatureCollection>(`${parcelMapQueryUrl}${query}`),
    }),
    [parcelMapQueryUrl],
  );
};
