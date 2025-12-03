import { useEffect, useState } from 'react';

import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { AddressForm } from '@/features/mapSideBar/shared/models';
import { useBcaAddress } from '@/features/properties/map/hooks/useBcaAddress';
import { exists } from '@/utils';
import { pidFromFeatureSet } from '@/utils/mapPropertyUtils';

export interface FeatureDatasetWithAddress {
  feature: SelectedFeatureDataset;
  address?: AddressForm;
}

export const useFeatureDatasetsWithAddresses = (features: SelectedFeatureDataset[] | null) => {
  const { getPrimaryAddressByPid, bcaLoading } = useBcaAddress();
  const [results, setResults] = useState<FeatureDatasetWithAddress[]>([]);

  useEffect(() => {
    let isMounted = true;
    const fetchAddresses = async () => {
      if (!exists(features) || features.length === 0) {
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

  return { featuresWithAddresses: results, bcaLoading };
};
