import {
  ALR_LAYER_URL,
  ELECTORAL_LAYER_URL,
  HWY_DISTRICT_LAYER_URL,
  INDIAN_RESERVES_LAYER_URL,
  IUserLayerQuery,
  MOTI_REGION_LAYER_URL,
  useLayerQuery,
} from 'components/maps/leaflet/LayerPopup';
import { GeoJsonProperties } from 'geojson';
import useIsMounted from 'hooks/useIsMounted';
import { LatLngLiteral } from 'leaflet';
import { useEffect, useState } from 'react';

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
 * @param location The location (lat, lng) to query
 * @returns An object with information + metadata returned by the map layers queried by this hook
 */
export function useQueryMapLayersByLocation(location: LatLngLiteral): IMapLayerResults {
  const isMounted = useIsMounted();
  const motiRegionService = useLayerQuery(MOTI_REGION_LAYER_URL);
  const highwaysService = useLayerQuery(HWY_DISTRICT_LAYER_URL);
  const electoralService = useLayerQuery(ELECTORAL_LAYER_URL);
  const alrService = useLayerQuery(ALR_LAYER_URL);
  const firstNationsService = useLayerQuery(INDIAN_RESERVES_LAYER_URL);

  const [results, setResults] = useState<IMapLayerResults>({ ...initialState });

  useEffect(() => {
    async function func() {
      // Query BC Geographic Warehouse layers - ONLY if lat, long have been provided!
      if (location === undefined) {
        return;
      }

      try {
        const alr = await alrService.findOneWhereContains(location, 'GEOMETRY');
        const motiRegion = await findByLocation(motiRegionService, location, 'GEOMETRY');
        const highwaysDistrict = await findByLocation(highwaysService, location, 'GEOMETRY');
        const electoralDistrict = await findByLocation(electoralService, location);
        const firstNations = await findByLocation(firstNationsService, location, 'GEOMETRY');

        if (isMounted()) {
          const newState: IMapLayerResults = {
            isALR: alr?.features?.length > 0,
            motiRegion,
            highwaysDistrict,
            electoralDistrict,
            firstNations,
          };

          setResults(newState);
        }
      } catch (e) {
        setResults(initialState);
      }
    }

    func();
  }, [
    alrService,
    electoralService,
    firstNationsService,
    highwaysService,
    isMounted,
    location,
    motiRegionService,
  ]);

  return results;
}

async function findByLocation(
  service: IUserLayerQuery,
  latlng: LatLngLiteral,
  geometryName: string = 'SHAPE',
) {
  const response = await service.findOneWhereContains(latlng, geometryName);
  const featureProps = response?.features?.[0]?.properties;
  return featureProps;
}
