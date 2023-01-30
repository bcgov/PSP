import { IUserLayerQuery, useLayerQuery } from 'components/maps/leaflet/LayerPopup';
import { GeoJsonProperties } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { useCallback } from 'react';

import { useTenant } from './../tenants/useTenant';

const initialState: IMapLayerResults = {
  isALR: null,
  motiRegion: null,
  highwaysDistrict: null,
  electoralDistrict: null,
  firstNations: null,
};

export interface IMapLayerResults {
  isALR: boolean | null;
  motiRegion: GeoJsonProperties;
  highwaysDistrict: GeoJsonProperties;
  electoralDistrict: GeoJsonProperties;
  firstNations: GeoJsonProperties;
}

/**
 * A generic hook that will query various DataBC map layers by the supplied location (lat, lng) and return layer metadata for that location.
 * @returns An object with information + metadata returned by the map layers queried by this hook
 */
export function useQueryMapLayersByLocation() {
  const {
    motiRegionLayerUrl,
    hwyDistrictLayerUrl,
    electoralLayerUrl,
    alrLayerUrl,
    reservesLayerUrl,
  } = useTenant();

  const motiRegionService = useLayerQuery(motiRegionLayerUrl);
  const highwaysService = useLayerQuery(hwyDistrictLayerUrl);
  const electoralService = useLayerQuery(electoralLayerUrl);
  const alrService = useLayerQuery(alrLayerUrl);
  const firstNationsService = useLayerQuery(reservesLayerUrl);

  const queryAllCallback = useCallback(
    async function (location: LatLngLiteral | null): Promise<IMapLayerResults> {
      // Query BC Geographic Warehouse layers - ONLY if lat, long have been provided!
      if (location === null) {
        return initialState;
      }

      try {
        // We are using spatial reference = 3005 (BC Albers) here because that's how the backend is returning spatial location
        const alr = await alrService.findOneWhereContains(location, 'GEOMETRY', 3005);
        const motiRegion = await findByLocation(motiRegionService, location, 'GEOMETRY', 3005);
        const highwaysDistrict = await findByLocation(highwaysService, location, 'GEOMETRY', 3005);
        const electoralDistrict = await findByLocation(electoralService, location);
        const firstNations = await findByLocation(firstNationsService, location, 'GEOMETRY', 3005);

        return {
          isALR: alr?.features?.length > 0,
          motiRegion,
          highwaysDistrict,
          electoralDistrict,
          firstNations,
        };
      } catch (e) {
        return initialState;
      }
    },
    [alrService, electoralService, firstNationsService, highwaysService, motiRegionService],
  );

  return {
    queryAll: queryAllCallback,
  };
}

async function findByLocation(
  service: IUserLayerQuery,
  latlng: LatLngLiteral,
  geometryName: string = 'SHAPE',
  spatialReferenceId: number = 4326,
) {
  const response = await service.findOneWhereContains(latlng, geometryName, spatialReferenceId);
  const featureProps = response?.features?.[0]?.properties;
  return featureProps;
}
