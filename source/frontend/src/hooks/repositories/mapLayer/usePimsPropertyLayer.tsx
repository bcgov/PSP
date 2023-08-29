import { FeatureCollection, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { useCallback, useContext, useMemo } from 'react';

import { IGeoSearchParams } from '@/constants/API';
import CustomAxios from '@/customAxios';
import { toCqlFilter } from '@/hooks/layer-api/layerUtils';
import { useLayerQuery } from '@/hooks/layer-api/useLayerQuery';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import {
  PIMS_Property_Boundary_View,
  PIMS_Property_Location_View,
} from '@/models/layers/pimsPropertyLocationView';
import { TenantContext } from '@/tenants';

/**
 * API wrapper to centralize all AJAX requests to WFS endpoints for the pims property location.
 * @returns Object containing functions to make requests to the WFS layer.
 * Note: according to the view PIMS_PROPERTY_LOCATION_VW
 */
export const usePimsPropertyLayer = () => {
  const {
    tenant: { propertiesUrl, boundaryLayerUrl },
  } = useContext(TenantContext);

  const { findOneWhereContainsWrapped } = useLayerQuery(boundaryLayerUrl, true);
  const findOneWhereContainsWrappedExecute = findOneWhereContainsWrapped.execute;
  const findOneWhereContainsWrappedLoading = findOneWhereContainsWrapped.loading;

  const loadPropertyLayer = useApiRequestWrapper({
    requestFunction: useCallback(
      (params?: IGeoSearchParams) => {
        const geoserver_params = {
          STREET_ADDRESS_1: params?.STREET_ADDRESS_1,
          PID: params?.PID,
          PIN: params?.PIN,
          SURVEY_PLAN_NUMBER: params?.SURVEY_PLAN_NUMBER,
        };
        const url = `${propertiesUrl}${
          geoserver_params ? toCqlFilter(geoserver_params, params?.forceExactMatch) : ''
        }`;
        return CustomAxios().get<FeatureCollection<Geometry, PIMS_Property_Location_View>>(url);
      },
      [propertiesUrl],
    ),
    requestName: 'LOAD_PROPERTIES',
  });

  const findOne = useCallback(
    async (
      latlng: LatLngLiteral,
      geometryName?: string | undefined,
      spatialReferenceId?: number | undefined,
    ) => {
      const featureCollection = await findOneWhereContainsWrappedExecute(
        latlng,
        geometryName,
        spatialReferenceId,
      );

      // TODO: Enhance useLayerQuery to allow generics to match the Property types
      const forceCasted = featureCollection as FeatureCollection<
        Geometry,
        PIMS_Property_Boundary_View
      >;

      return forceCasted !== undefined && forceCasted.features.length > 0
        ? forceCasted.features[0]
        : undefined;
    },
    [findOneWhereContainsWrappedExecute],
  );

  return useMemo(
    () => ({
      loadPropertyLayer,
      findOne,
      findOneLoading: findOneWhereContainsWrappedLoading,
    }),
    [loadPropertyLayer, findOne, findOneWhereContainsWrappedLoading],
  );
};
