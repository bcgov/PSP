import { FeatureCollection, Geometry } from 'geojson';
import React from 'react';

import { IResearchFilter } from '@/features/research/interfaces';
import { useTenant } from '@/tenants';

import { paths as GeographicNamesPaths } from './interfaces/IGeographicNames';
import { IGeographicNamesProperties } from './interfaces/IGeographicNamesProperties';
import { IPaginateRequest } from './interfaces/IPaginateRequest';
import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the geographic names endpoints. https://openapi.apps.gov.bc.ca/?url=https://raw.githubusercontent.com/bcgov/api-specs/master/bcgnws/bcgnws.json#/search/get_names_search
 * @returns Object containing functions to make requests to the geographic names API.
 */

export type GeographicNameSearchCriteria =
  GeographicNamesPaths['/names/search']['get']['parameters']['query'];

export const defaultGeographicNameSearchCriteria: GeographicNameSearchCriteria = {
  outputFormat: 'json',
  name: '',
  featureClass: '',
  exactSpelling: 0,
  sortBy: 'relevance',
  startIndex: 1,
  outputSRS: 4326,
  embed: 0,
  outputStyle: 'summary',
  itemsPerPage: 10,
};

export const useApiGeographicNames = () => {
  const api = useAxiosApi();
  const { geographicNamesUrl, geographicNamesResultLimit } = useTenant();

  return React.useMemo(
    () => ({
      searchName: (searchCriteria: GeographicNameSearchCriteria) =>
        api.get<FeatureCollection<Geometry, IGeographicNamesProperties>>(
          `${geographicNamesUrl}/names/search?${new URLSearchParams(
            { ...searchCriteria, itemsPerPage: geographicNamesResultLimit } as any, //required to bypass integer/enum types
          ).toString()}`,
        ),
    }),
    [api, geographicNamesUrl, geographicNamesResultLimit],
  );
};

export type IPaginateResearch = IPaginateRequest<IResearchFilter>;
