import {
  ALR_LAYER_URL,
  ELECTORAL_LAYER_URL,
  HWY_DISTRICT_LAYER_URL,
  INDIAN_RESERVES_LAYER_URL,
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
  const firstNationsService = useLayerQuery(INDIAN_RESERVES_LAYER_URL);

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

  /**
   * This effect is intended to run AFTER the property info has been loaded from the API.
   * This is guaranteed here because:
   *   1. location is a memo that only gets re-calculated when the property info comes back
   *      from the API with proper lat, long values. So until API results comes back it will be undefined.
   *
   *   2. The second useEffect aborts early if location is undefined so it won't call the layers until
   *      a location is available (because that is needed to query the layers). So effectively if the API
   *      takes longer to return then the second effect will return early and re-run whenever the API
   *      response is set via setState on propertyDetails
   */
  useEffect(() => {
    async function fn() {
      if (location === undefined) {
        return;
      }
      // query BC Geographic Warehouse layers
      const motiRegion = await findByLocation(motiRegionService, location, 'GEOMETRY');
      const highwaysDistrict = await findByLocation(highwaysDistrictService, location, 'GEOMETRY');
      const electoralDistrict = await findByLocation(electoralService, location);
      const alr = await alrService.findOneWhereContains(location, 'GEOMETRY');
      const firstNations = await findByLocation(firstNationsService, location, 'GEOMETRY');

      if (isMounted()) {
        setPropertyDetails(prevState => {
          if (prevState !== undefined) {
            return {
              ...prevState,
              motiRegion,
              highwaysDistrict,
              electoralDistrict,
              isALR: alr?.features?.length > 0,
              firstNations: {
                bandName: firstNations?.BAND_NAME,
                reserveName: firstNations?.ENGLISH_NAME,
              },
            };
          }
        });
      }
    }

    fn();
  }, [
    alrService,
    electoralService,
    firstNationsService,
    highwaysDistrictService,
    isMounted,
    location,
    motiRegionService,
  ]);

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
