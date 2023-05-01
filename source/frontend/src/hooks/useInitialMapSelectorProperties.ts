import { useBcaAddress } from 'features/properties/map/hooks/useBcaAddress';
import { AddressForm, PropertyForm } from 'features/properties/map/shared/models';
import { Feature, GeoJsonProperties, Geometry } from 'geojson';
import { useEffect, useState } from 'react';
import { mapFeatureToProperty } from 'utils/mapPropertyUtils';

export const useInitialMapSelectorProperties = (
  selectedFeature: Feature<Geometry, GeoJsonProperties> | null,
) => {
  const { getPrimaryAddressByPid, bcaLoading } = useBcaAddress();
  const [bcaAddress, setBcaAddress] = useState<AddressForm>();

  const pid = selectedFeature?.properties?.PID;
  useEffect(() => {
    const getInitialPropertyAddress = async () => {
      if (pid) {
        const bcaSummary = await getPrimaryAddressByPid(pid, 3000);
        bcaSummary?.address && setBcaAddress(AddressForm.fromBcaAddress(bcaSummary?.address));
      }
    };
    getInitialPropertyAddress();
  }, [getPrimaryAddressByPid, pid]);

  return {
    initialProperty:
      selectedFeature !== null
        ? {
            ...PropertyForm.fromMapProperty(mapFeatureToProperty(selectedFeature)),
            address: bcaAddress,
          }
        : null,
    bcaLoading,
  };
};
