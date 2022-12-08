import {
  ALR_LAYER_URL,
  ELECTORAL_LAYER_URL,
  HWY_DISTRICT_LAYER_URL,
  INDIAN_RESERVES_LAYER_URL,
  IUserLayerQuery,
  MOTI_REGION_LAYER_URL,
  useLayerQuery,
} from 'components/maps/leaflet/LayerPopup';
import useIsMounted from 'hooks/useIsMounted';
import { LatLngLiteral } from 'leaflet';
import { Api_Property } from 'models/api/Property';
import { useEffect, useMemo, useState } from 'react';

import {
  IPropertyDetailsForm,
  toFormValues,
} from '../tabs/propertyDetails/detail/PropertyDetailsTabView.helpers';

export function usePropertyDetails(property?: Api_Property): IPropertyDetailsForm | undefined {
  const isMounted = useIsMounted();
  const motiRegionService = useLayerQuery(MOTI_REGION_LAYER_URL);
  const highwaysDistrictService = useLayerQuery(HWY_DISTRICT_LAYER_URL);
  const electoralService = useLayerQuery(ELECTORAL_LAYER_URL);
  const alrService = useLayerQuery(ALR_LAYER_URL);
  const firstNationsService = useLayerQuery(INDIAN_RESERVES_LAYER_URL);

  const [propertyViewForm, setPropertyViewForm] = useState<IPropertyDetailsForm | undefined>(
    undefined,
  );

  const lat = property?.latitude;
  const lng = property?.longitude;
  const location = useMemo(
    () => (lat === undefined || lng === undefined ? undefined : { lat, lng }),
    [lat, lng],
  );

  /**
   * This effect is intended to run AFTER the property info has been loaded from the API.
   * This is guaranteed here because:
   *   1. location is a memo that only gets re-calculated when the property info comes back
   *      from the API with proper lat, long values. So until API results comes back it will be undefined.
   *
   *   2. The useEffect below aborts early if location is undefined so it won't call the layers until
   *      a location is available (because that is needed to query the layers). So effectively if the API
   *      takes longer to return then the second effect will return early and re-run whenever the API
   *      response is set via setState on propertyDetails
   */
  useEffect(() => {
    async function fn() {
      // Query BC Geographic Warehouse layers - ONLY if lat, long have been provided!
      if (location !== undefined) {
        const electoralDistrict = await findByLocation(electoralService, location);
        const alr = await alrService.findOneWhereContains(location, 'GEOMETRY');
        const firstNations = await findByLocation(firstNationsService, location, 'GEOMETRY');

        if (isMounted()) {
          const newState: IPropertyDetailsForm = {
            ...toFormValues(property),
            electoralDistrict,
            isALR: alr?.features?.length > 0,
            firstNations: {
              bandName: firstNations?.BAND_NAME,
              reserveName: firstNations?.ENGLISH_NAME,
            },
          };

          setPropertyViewForm(newState);
        }
      } else {
        // in this case we don't know the lat/lng of this property, so do not query the layers.

        if (isMounted()) {
          const newState: IPropertyDetailsForm = {
            ...toFormValues(property),
          };

          setPropertyViewForm(newState);
        }
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
    property,
  ]);

  return propertyViewForm;
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
