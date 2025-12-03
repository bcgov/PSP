import { useEffect, useState } from 'react';

import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { AddressForm } from '@/features/mapSideBar/shared/models';
import { useBcaAddress } from '@/features/properties/map/hooks/useBcaAddress';
import { isEmptyOrNull } from '@/utils';
import { pidFromFeatureSet } from '@/utils/mapPropertyUtils';

export interface LocationFeatureDatasetWithAddress {
  feature: LocationFeatureDataset;
  address?: AddressForm;
}

export const useLocationFeatureDatasetsWithAddresses = (
  features: LocationFeatureDataset[] | null,
) => {
  const { getPrimaryAddressByPid, bcaLoading } = useBcaAddress();
  const [results, setResults] = useState<LocationFeatureDatasetWithAddress[]>([]);

  useEffect(() => {
    let isMounted = true;
    const fetchAddresses = async () => {
      if (isEmptyOrNull(features)) {
        setResults([]);
        return;
      }
      const promises = features.map(async feature => {
        const pid = pidFromFeatureSet(feature);
        if (pid) {
          const bcaSummary = await getPrimaryAddressByPid(pid, 30000);
          return {
            feature,
            address: bcaSummary?.address
              ? AddressForm.fromBcaAddress(bcaSummary.address)
              : undefined,
          };
        }
        return { feature, address: undefined };
      });
      const resolved = await Promise.all(promises);
      if (isMounted) {
        setResults(resolved);
      }
    };
    fetchAddresses();
    return () => {
      isMounted = false;
    };
  }, [features, getPrimaryAddressByPid]);

  return { locationFeaturesWithAddresses: results, bcaLoading };
};
