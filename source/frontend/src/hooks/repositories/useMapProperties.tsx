import { FeatureCollection, Geometry } from 'geojson';
import { useCallback, useContext } from 'react';

import { IGeoSearchParams } from '@/constants/API';
import CustomAxios from '@/customAxios';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { PIMS_Property_Location_View } from '@/models/layers/pimsPropertyLocationView';
import { TenantContext } from '@/tenants';

import { toCqlFilter } from '../layer-api/layerUtils';

export const useMapProperties = () => {
  const {
    tenant: { propertiesUrl },
  } = useContext(TenantContext);

  const loadPropertiesRequest = useCallback(
    (params?: IGeoSearchParams) => {
      const geoserver_params = {
        STREET_ADDRESS_1: params?.STREET_ADDRESS_1,
        PID_PADDED: params?.PID?.replace(/[-\s]/g, ''),
        PIN: params?.PIN,
      };
      const url = `${propertiesUrl}${
        geoserver_params ? toCqlFilter(geoserver_params, params?.forceExactMatch) : ''
      }`;
      return CustomAxios().get<FeatureCollection<Geometry, PIMS_Property_Location_View>>(url);
    },
    [propertiesUrl],
  );

  const loadProperties = useApiRequestWrapper({
    requestFunction: loadPropertiesRequest,
    requestName: 'LOAD_PROPERTIES',
    throwError: true,
  });
  return { loadProperties };
};
