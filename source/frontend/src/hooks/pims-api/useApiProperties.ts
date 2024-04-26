import queryString from 'query-string';
import React from 'react';

import { IPaginateProperties } from '@/constants/API';
import { IPropertyFilter } from '@/features/properties/filter/IPropertyFilter';
import { ApiGen_Base_Page } from '@/models/api/generated/ApiGen_Base_Page';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { ApiGen_Concepts_PropertyAssociations } from '@/models/api/generated/ApiGen_Concepts_PropertyAssociations';
import { ApiGen_Concepts_PropertyView } from '@/models/api/generated/ApiGen_Concepts_PropertyView';
import { Api_PropertyFilterCriteria } from '@/models/api/ProjectFilterCriteria';

import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the property endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiProperties = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getPropertiesViewPagedApi: (params: IPropertyFilter | null) =>
        api.get<ApiGen_Base_Page<ApiGen_Concepts_PropertyView>>(
          `/properties/search?${params ? queryString.stringify(params) : ''}`,
        ),
      getMatchingPropertiesApi: (params: Api_PropertyFilterCriteria) =>
        api.post<number[]>(`/properties/search/advanced-filter`, params),
      getPropertyAssociationsApi: (id: number) =>
        api.get<ApiGen_Concepts_PropertyAssociations>(`/properties/${id}/associations`),
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
        api.get<ApiGen_Concepts_Property[]>(`/properties?${ids.map(x => `ids=${x}`).join('&')}`),
      getPropertyConceptWithIdApi: (id: number) =>
        api.get<ApiGen_Concepts_Property>(`/properties/${id}`),
      getPropertyConceptWithPidApi: (pid: string) =>
        api.get<ApiGen_Concepts_Property>(`/properties/pid/${pid}`),
      getPropertyConceptWithPinApi: (pin: number) =>
        api.get<ApiGen_Concepts_Property>(`/properties/pin/${pin}`),
      putPropertyConceptApi: (property: ApiGen_Concepts_Property) =>
        api.put<ApiGen_Concepts_Property>(`/properties/${property.id}`, property),
    }),
    [api],
  );
};
