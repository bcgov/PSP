import { toCqlFilter } from 'components/maps/leaflet/mapUtils';
import { IGeoSearchParams } from 'constants/API';
import CustomAxios from 'customAxios';
import { FeatureCollection } from 'geojson';
import { useApiRequestWrapper } from 'hooks/pims-api/useApiRequestWrapper';
import { useCallback, useContext } from 'react';
import { TenantContext } from 'tenants';

export const useMapProperties = () => {
  const {
    tenant: { propertiesUrl },
  } = useContext(TenantContext);

  const loadPropertiesRequest = useCallback(
    (params?: IGeoSearchParams) => {
      const geoserver_params = {
        STREET_ADDRESS_1: params?.STREET_ADDRESS_1,
        PID: params?.PID,
        PIN: params?.PIN,
      };
      const url = `${propertiesUrl}${
        geoserver_params ? toCqlFilter(geoserver_params, false, params?.forceExactMatch) : ''
      }`;
      return CustomAxios().get<FeatureCollection>(url);
    },
    [propertiesUrl],
  );

  const loadProperties = useApiRequestWrapper({
    requestFunction: loadPropertiesRequest,
    requestName: 'LOAD_PROPERTIES',
  });
  return { loadProperties };
};
