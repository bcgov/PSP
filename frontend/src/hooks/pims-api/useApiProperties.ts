import { IPaginateProperties } from 'constants/API';
import { IPagedItems, IProperty } from 'interfaces';
import { IPropertyApiModel } from 'interfaces/IPropertyApiModel';
import queryString from 'query-string';
import React from 'react';

import { useAxiosApi } from '.';

/**
 * PIMS API wrapper to centralize all AJAX requests to the property endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiProperties = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getPropertiesPaged: (params: IPaginateProperties | null) =>
        api.get<IPagedItems<IProperty>>(
          `/properties/search?${params ? queryString.stringify(params) : ''}`,
        ),
      getPropertyWithPid: (pid: string) => api.get<IPropertyApiModel>(`/properties/${pid}`),
      getProperty: (id: number) => api.get<IPropertyApiModel>(`/properties/${id}`),
      exportProperties: (filter: IPaginateProperties, outputFormat: 'csv' | 'excel' = 'excel') =>
        api.get(
          `/reports/properties?${filter ? queryString.stringify({ ...filter, all: true }) : ''}`,
          {
            responseType: 'blob',
            headers: {
              Accept: outputFormat === 'csv' ? 'text/csv' : 'application/vnd.ms-excel',
            },
          },
        ),
    }),
    [api],
  );
};
