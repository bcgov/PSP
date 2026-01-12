import { FeatureCollection, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { useCallback, useContext, useMemo } from 'react';

import { IGeoSearchParams } from '@/constants/API';
import CustomAxios from '@/customAxios';
import { toCqlFilter } from '@/hooks/layer-api/layerUtils';
import { useLayerQuery } from '@/hooks/layer-api/useLayerQuery';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { PIMS_Property_Lite_View, PIMS_Property_View } from '@/models/layers/pimsPropertyView';
import { TenantContext } from '@/tenants';
import { exists } from '@/utils';

/**
 * API wrapper to centralize all AJAX requests to WFS endpoints for the pims property location.
 * @returns Object containing functions to make requests to the WFS layer.
 * Note: according to the view PIMS_PROPERTY_VW
 */
export const usePimsPropertyLayer = () => {
  const {
    tenant: { propertiesUrl, minimalPropertiesUrl },
  } = useContext(TenantContext);

  const {
    findMultipleWhereContainsWrapped: {
      execute: findMultipleWhereContainsWrappedExecute,
      loading: findMultipleWhereContainsWrappedLoading,
    },
    findOneWhereContainsWrapped: {
      execute: findOneWhereContainsWrappedExecute,
      loading: findOneWhereContainsWrappedLoading,
    },
  } = useLayerQuery(propertiesUrl, true);

  const loadPropertyLayer = useApiRequestWrapper({
    requestFunction: useCallback(
      (params?: IGeoSearchParams) => {
        const geoserver_params = {
          STREET_ADDRESS_1: params?.STREET_ADDRESS_1,
          PID: params?.PID,
          PID_PADDED: params?.PID_PADDED,
          PIN: params?.PIN,
          SURVEY_PLAN_NUMBER: params?.SURVEY_PLAN_NUMBER,
          HISTORICAL_FILE_NUMBER_STR: params?.HISTORICAL_FILE_NUMBER_STR,
        };
        const url = `${propertiesUrl}${
          geoserver_params ? toCqlFilter(geoserver_params, params?.forceExactMatch) : ''
        }`;
        return CustomAxios().get<FeatureCollection<Geometry, PIMS_Property_View>>(url);
      },
      [propertiesUrl],
    ),
    requestName: 'LOAD_PROPERTIES',
  });

  const loadPropertyBoundaryLayer = useApiRequestWrapper({
    requestFunction: useCallback(
      (params?: IGeoSearchParams) => {
        const geoserver_params = {
          STREET_ADDRESS_1: params?.STREET_ADDRESS_1,
          PID: params?.PID,
          PID_PADDED: params?.PID_PADDED,
          PIN: params?.PIN,
          SURVEY_PLAN_NUMBER: params?.SURVEY_PLAN_NUMBER,
          HISTORICAL_FILE_NUMBER_STR: params?.HISTORICAL_FILE_NUMBER_STR,
        };
        const url = `${boundaryLayerUrl}&srsName=EPSG:4326${
          geoserver_params ? toCqlFilter(geoserver_params, params?.forceExactMatch) : ''
        }`;
        return CustomAxios().get<FeatureCollection<Geometry, PIMS_Property_Boundary_View>>(url);
      },
      [boundaryLayerUrl],
    ),
    requestName: 'LOAD_PROPERTIES_BOUNDARY',
  });

  const loadPropertyLocationOnlyMinimal = useApiRequestWrapper({
    requestFunction: useCallback(() => {
      return CustomAxios().get<FeatureCollection<Geometry, PIMS_Property_Lite_View>>(
        minimalPropertiesUrl + `&cql_filter= GEOMETRY IS NULL AND LOCATION IS NOT NULL`,
      );
    }, [minimalPropertiesUrl]),
    requestName: 'LOAD_PROPERTIES_MINIMAL',
  });

  const findOneByBoundary = useCallback(
    async (
      latlng: LatLngLiteral,
      geometryName?: string | undefined,
      spatialReferenceId?: number | undefined,
    ) => {
      const featureCollection = await findOneWhereContainsWrappedExecute(
        latlng,
        geometryName,
        spatialReferenceId,
        'SORTBY=IS_RETIRED%20ASC',
      );

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as FeatureCollection<Geometry, PIMS_Property_View>;

      return forceCasted !== undefined && forceCasted.features?.length > 0
        ? forceCasted.features[0]
        : undefined;
    },
    [findOneWhereContainsWrappedExecute],
  );

  const findAllByBoundary = useCallback(
    async (
      latlng: LatLngLiteral,
      geometryName?: string | undefined,
      spatialReferenceId?: number | undefined,
    ) => {
      const featureCollection = await findMultipleWhereContainsWrappedExecute(
        latlng,
        geometryName,
        spatialReferenceId,
        'SORTBY=IS_RETIRED%20ASC',
      );

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as FeatureCollection<Geometry, PIMS_Property_View>;

      return forceCasted !== undefined && forceCasted.features?.length > 0
        ? forceCasted.features
        : undefined;
    },
    [findMultipleWhereContainsWrappedExecute],
  );

  const findOneByPidOrPin = useCallback(
    async (pid?: string, pin?: string) => {
      if (!exists(pid) && !exists(pin)) {
        return undefined;
      }

      const params: IGeoSearchParams = {
        PID: pid,
        PIN: pin,
      };
      const featureCollection = await loadPropertyLayer.execute(params);

      return exists(featureCollection) && featureCollection.features?.length > 0
        ? featureCollection.features[0]
        : undefined;
    },
    [loadPropertyLayer],
  );

  return useMemo(
    () => ({
      loadPropertyLayer,
      loadPropertyBoundaryLayer,
      loadPropertyLayerMinimal: loadPropertyLocationOnlyMinimal,
      findAllByBoundary,
      findAllByBoundaryLoading: findMultipleWhereContainsWrappedLoading,
      findOneByBoundary,
      findOneByBoundaryLoading: findOneWhereContainsWrappedLoading,
      findOneByPidOrPin,
    }),
    [
      findAllByBoundary,
      findMultipleWhereContainsWrappedLoading,
      findOneByBoundary,
      findOneByPidOrPin,
      findOneWhereContainsWrappedLoading,
      loadPropertyBoundaryLayer,
      loadPropertyLayer,
      loadPropertyLocationOnlyMinimal,
    ],
  );
};
