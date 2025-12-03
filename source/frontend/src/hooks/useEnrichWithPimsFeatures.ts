import { Feature, Geometry } from 'geojson';
import { useCallback, useState } from 'react';

import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { PIMS_Property_Boundary_View } from '@/models/layers/pimsPropertyLocationView';
import { isEmptyOrNull, isValidString, pidFromFeatureSet, pinFromFeatureSet } from '@/utils';

import { usePimsPropertyLayer } from './repositories/mapLayer/usePimsPropertyLayer';

interface UseEnrichWithPimsFeaturesResult {
  datasets: LocationFeatureDataset[];
  loading: boolean;
  error: string | null;
  enrichWithPimsFeatures: (inputDatasets: LocationFeatureDataset[]) => Promise<void>;
}

/**
 * Hook that enriches SelectedFeatureDataset[] with PIMS features using findOneByPidOrPin.
 */
export function useEnrichWithPimsFeatures(): UseEnrichWithPimsFeaturesResult {
  const [datasets, setDatasets] = useState<LocationFeatureDataset[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const { findOneByPidOrPin } = usePimsPropertyLayer();

  const enrichWithPimsFeatures = useCallback(
    async (inputDatasets: LocationFeatureDataset[]) => {
      if (isEmptyOrNull(inputDatasets)) {
        setDatasets([]);
        return;
      }

      setLoading(true);
      setError(null);

      try {
        const updatedPropertyPromises = inputDatasets.map(async dataset => {
          if (isEmptyOrNull(dataset.parcelFeatures)) {
            return dataset;
          }
          if (!isEmptyOrNull(dataset.pimsFeatures)) {
            return dataset; // already enriched
          }

          const pid = pidFromFeatureSet(dataset);
          const pin = pinFromFeatureSet(dataset);

          if (!isValidString(pid) && !isValidString(pin)) return dataset;

          try {
            const pimsFeature: Feature<Geometry, PIMS_Property_Boundary_View> | undefined =
              await findOneByPidOrPin(pid, pin);
            dataset.pimsFeatures = [pimsFeature];
            return dataset;
          } catch (innerErr) {
            return dataset; // leave dataset unchanged if API fails
          }
        });

        const updatedProperties = await Promise.all(updatedPropertyPromises);
        setDatasets(updatedProperties);
      } catch (err) {
        setError(err?.message ?? 'Failed to enrich with PIMS features');
      } finally {
        setLoading(false);
      }
    },
    [findOneByPidOrPin],
  );

  return { datasets, loading, error, enrichWithPimsFeatures };
}
