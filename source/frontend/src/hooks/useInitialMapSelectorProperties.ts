import { useEffect, useState } from 'react';

import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { AddressForm, PropertyForm } from '@/features/mapSideBar/shared/models';
import { useBcaAddress } from '@/features/properties/map/hooks/useBcaAddress';
import { featuresetToMapProperty, pidFromFeatureSet } from '@/utils/mapPropertyUtils';

export const useInitialMapSelectorProperties = (selectedFeature: LocationFeatureDataset | null) => {
  const { getPrimaryAddressByPid, bcaLoading } = useBcaAddress();
  const [bcaAddress, setBcaAddress] = useState<AddressForm>();

  const pid = selectedFeature !== null ? pidFromFeatureSet(selectedFeature) : null;
  useEffect(() => {
    const getInitialPropertyAddress = async () => {
      if (pid !== null) {
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
            ...PropertyForm.fromMapProperty(featuresetToMapProperty(selectedFeature)),
            address: bcaAddress,
          }
        : null,
    bcaLoading,
  };
};
