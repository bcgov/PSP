import { Feature, Geometry } from 'geojson';
import { useCallback, useState } from 'react';

import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { PIMS_Property_Boundary_View } from '@/models/layers/pimsPropertyLocationView';
import { exists, isValidString, pidFromFeatureSet, pinFromFeatureSet } from '@/utils';

import { usePimsPropertyLayer } from './repositories/mapLayer/usePimsPropertyLayer';

interface UseEnrichWithPimsFeaturesResult {
  datasets: SelectedFeatureDataset[];
  loading: boolean;
  error: string | null;
  enrichWithPimsFeatures: (inputDatasets: SelectedFeatureDataset[]) => Promise<void>;
}

/**
 * Hook that enriches SelectedFeatureDataset[] with PIMS features using findOneByPidOrPin.
 */
export function useEnrichWithPimsFeatures(): UseEnrichWithPimsFeaturesResult {
  const [datasets, setDatasets] = useState<SelectedFeatureDataset[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const { findOneByPidOrPin } = usePimsPropertyLayer();

  const enrichWithPimsFeatures = useCallback(
    async (inputDatasets: SelectedFeatureDataset[]) => {
      if (!exists(inputDatasets) || inputDatasets.length === 0) {
        setDatasets([]);
        return;
      }

      setLoading(true);
      setError(null);

      try {
        const updatedPropertyPromises = inputDatasets.map(async dataset => {
          if (!exists(dataset.parcelFeature)) return dataset;
          if (exists(dataset.pimsFeature)) return dataset; // already enriched

          const pid = pidFromFeatureSet(dataset);
          const pin = pinFromFeatureSet(dataset);

          if (!isValidString(pid) && !isValidString(pin)) return dataset;

          try {
            const pimsFeature: Feature<Geometry, PIMS_Property_Boundary_View> | undefined =
              await findOneByPidOrPin(pid, pin);
            dataset.pimsFeature = pimsFeature ?? null;
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
