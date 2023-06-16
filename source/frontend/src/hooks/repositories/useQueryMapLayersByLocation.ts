import { GeoJsonProperties } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { useCallback } from 'react';

import { useAdminBoundaryMapLayer } from './mapLayer/useAdminBoundaryMapLayer';
import { useIndianReserveBandMapLayer } from './mapLayer/useIndianReserveBandMapLayer';
import { useLegalAdminBoundariesMapLayer } from './mapLayer/useLegalAdminBoundariesMapLayer';

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
  const adminBoundaryLayerService = useAdminBoundaryMapLayer();
  const adminBoundaryService = useLegalAdminBoundariesMapLayer();
  const firstNationsService = useIndianReserveBandMapLayer();

  const queryAllCallback = useCallback(
    async function (location: LatLngLiteral | null): Promise<IMapLayerResults> {
      // Query BC Geographic Warehouse layers - ONLY if lat, long have been provided!
      if (location === null) {
        return initialState;
      }

      try {
        // We are using spatial reference = 3005 (BC Albers) here because that's how the backend is returning spatial location
        const alrFeature = await adminBoundaryService.findOneAgriculturalReserve(
          location,
          'GEOMETRY',
          3005,
        );
        const motiRegionFeature = await adminBoundaryLayerService.findRegion(
          location,
          'GEOMETRY',
          3005,
        );
        const highwaysDistrictFeature = await adminBoundaryLayerService.findDistrict(
          location,
          'GEOMETRY',
          3005,
        );
        const electoralDistrictFeature = await adminBoundaryLayerService.findElectoralDistrict(
          location,
        );
        const firstNationsFeature = await firstNationsService.findOne(location, 'GEOMETRY', 3005);

        return {
          isALR: alrFeature !== undefined,
          motiRegion: motiRegionFeature?.properties || {},
          highwaysDistrict: highwaysDistrictFeature?.properties || {},
          electoralDistrict: electoralDistrictFeature?.properties || {},
          firstNations: firstNationsFeature?.properties || {},
        };
      } catch (e) {
        return initialState;
      }
    },
    [adminBoundaryLayerService, adminBoundaryService, firstNationsService],
  );

  return {
    queryAll: queryAllCallback,
  };
}
