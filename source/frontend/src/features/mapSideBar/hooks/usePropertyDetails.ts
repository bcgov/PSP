import { useEffect, useMemo, useState } from 'react';

import { useAdminBoundaryMapLayer } from '@/hooks/repositories/mapLayer/useAdminBoundaryMapLayer';
import { useIndianReserveBandMapLayer } from '@/hooks/repositories/mapLayer/useIndianReserveBandMapLayer';
import { useLegalAdminBoundariesMapLayer } from '@/hooks/repositories/mapLayer/useLegalAdminBoundariesMapLayer';
import useIsMounted from '@/hooks/util/useIsMounted';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { exists } from '@/utils';

import {
  IPropertyDetailsForm,
  toFormValues,
} from '../property/tabs/propertyDetails/detail/PropertyDetailsTabView.helpers';

export function usePropertyDetails(
  property?: ApiGen_Concepts_Property,
): IPropertyDetailsForm | undefined {
  const isMounted = useIsMounted();
  const electoralService_ = useAdminBoundaryMapLayer();
  const findElectoralDistrict = electoralService_.findElectoralDistrict;
  const legalAdminService_ = useLegalAdminBoundariesMapLayer();
  const findOneAgriculturalReserve = legalAdminService_.findOneAgriculturalReserve;
  const firstNationsService_ = useIndianReserveBandMapLayer();
  const findFirstNation = firstNationsService_.findOne;

  const [propertyViewForm, setPropertyViewForm] = useState<IPropertyDetailsForm | undefined>(
    undefined,
  );

  const lat = property?.latitude;
  const lng = property?.longitude;
  const location = useMemo(
    () => (!exists(lat) || !exists(lng) ? undefined : { lat, lng }),
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

      if (exists(location)) {
        const electoralDistrictFeature = await findElectoralDistrict(location);
        const alrFeature = await findOneAgriculturalReserve(location, 'GEOMETRY');
        const firstNationsFeature = await findFirstNation(location, 'GEOMETRY');

        if (isMounted()) {
          const newState: IPropertyDetailsForm = {
            ...toFormValues(property),
            electoralDistrict: electoralDistrictFeature,
            isALR: alrFeature !== undefined,
            firstNations: {
              bandName: firstNationsFeature?.properties.BAND_NAME || '',
              reserveName: firstNationsFeature?.properties.ENGLISH_NAME || '',
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
    findElectoralDistrict,
    findOneAgriculturalReserve,
    findFirstNation,
    isMounted,
    location,
    property,
  ]);

  return propertyViewForm;
}
