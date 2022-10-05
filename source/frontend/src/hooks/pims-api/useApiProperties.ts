import { IPaginateProperties } from 'constants/API';
import { IPagedItems, IProperty } from 'interfaces';
import { Api_Property, Api_PropertyAssociations } from 'models/api/Property';
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
      getPropertyAssociations: (id: number) =>
        api.get<Api_PropertyAssociations>(`/properties/${id}/associations`),
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
      getPropertyConceptWithId: (id: number) => api.get<Api_Property>(`/properties/concept/${id}`),
      putPropertyConcept: (property: Api_Property) =>
        api.put<Api_Property>(`/properties/concept/${property.id}`, property),
    }),
    [api],
  );
};
