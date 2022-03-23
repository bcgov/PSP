import {
  ALR_LAYER_URL,
  ELECTORAL_LAYER_URL,
  HWY_DISTRICT_LAYER_URL,
  IUserLayerQuery,
  MOTI_REGION_LAYER_URL,
  useLayerQuery,
} from 'components/maps/leaflet/LayerPopup';
import { useProperties } from 'hooks';
import useIsMounted from 'hooks/useIsMounted';
import { LatLngLiteral } from 'leaflet';
import { useEffect, useMemo, useState } from 'react';
import { pidFormatter } from 'utils';

import { IPropertyDetailsForm, toFormValues } from './PropertyDetailsTabView.helpers';

export function usePropertyDetails(pid?: string) {
  const { getPropertyWithPid } = useProperties();
  const isMounted = useIsMounted();
  const [propertyDetails, setPropertyDetails] = useState<IPropertyDetailsForm | undefined>(
    undefined,
  );

  const lat = propertyDetails?.latitude;
  const lng = propertyDetails?.longitude;
  const location = useMemo(
    () => (lat === undefined || lng === undefined ? undefined : { lat, lng }),
    [lat, lng],
  );

  const motiRegionService = useLayerQuery(MOTI_REGION_LAYER_URL);
  const highwaysDistrictService = useLayerQuery(HWY_DISTRICT_LAYER_URL);
  const electoralService = useLayerQuery(ELECTORAL_LAYER_URL);
  const alrService = useLayerQuery(ALR_LAYER_URL);

  useEffect(() => {
    async function fn() {
      if (!pid) {
        return;
      }
      const propInfo = await getPropertyWithPid(pid);
      if (isMounted() && propInfo.pid === pidFormatter(pid)) {
        setPropertyDetails(toFormValues(propInfo));
      }
    }

    fn();
  }, [getPropertyWithPid, isMounted, pid]);

  useEffect(() => {
    async function fn() {
      if (location === undefined) {
        return;
      }
      // query BC Geographic Warehouse layers
      const moti = await findByLocation(motiRegionService, location, 'GEOMETRY');
      const highway = await findByLocation(highwaysDistrictService, location, 'GEOMETRY');
      const electoral = await findByLocation(electoralService, location);
      const alr = await findByLocation(alrService, location, 'GEOMETRY');

      if (isMounted()) {
        setPropertyDetails(prevState => {
          if (prevState !== undefined) {
            return {
              ...prevState,
              motiRegion: moti?.REGION_NAME,
              highwaysDistrict: highway?.a,
            };
          }
        });
      }
    }

    fn();
  }, [isMounted, location, motiRegionService]);

  return { propertyDetails };
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
