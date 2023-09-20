import queryString from 'query-string';
import React from 'react';

import { IPaginateProperties } from '@/constants/API';
import { IPropertyFilter } from '@/features/properties/filter/IPropertyFilter';
import { IPagedItems, IProperty } from '@/interfaces';
import { Api_PropertyFilterCriteria } from '@/models/api/ProjectFilterCriteria';
import { Api_Property, Api_PropertyAssociations } from '@/models/api/Property';
import { Api_PropertyLease } from '@/models/api/PropertyLease';

import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the property endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiProperties = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getPropertiesPagedApi: (params: IPropertyFilter | null) =>
        api.get<IPagedItems<IProperty>>(
          `/properties/search?${params ? queryString.stringify(params) : ''}`,
        ),
      getMatchingPropertiesApi: (params: Api_PropertyFilterCriteria) =>
        api.post<number[]>(`/properties/search/advanced-filter`, params),
      getPropertyAssociationsApi: (id: number) =>
        api.get<Api_PropertyAssociations>(`/properties/${id}/associations`),
      getPropertyLeasesApi: (id: number) =>
        api.get<Api_PropertyLease[]>(`/properties/${id}/leases`),
      exportPropertiesApi: (filter: IPaginateProperties, outputFormat: 'csv' | 'excel' = 'excel') =>
        api.get<Blob>(
          `/reports/properties?${filter ? queryString.stringify({ ...filter, all: true }) : ''}`,
          {
            responseType: 'blob',
            headers: {
              Accept: outputFormat === 'csv' ? 'text/csv' : 'application/vnd.ms-excel',
            },
          },
        ),
      getPropertiesApi: (ids: number[]) =>
        api.get<Api_Property[]>(`/properties?${ids.map(x => `ids=${x}`).join('&')}`),
      getPropertyConceptWithIdApi: (id: number) => api.get<Api_Property>(`/properties/${id}`),
      putPropertyConceptApi: (property: Api_Property) =>
        api.put<Api_Property>(`/properties/${property.id}`, property),
    }),
    [api],
  );
};
