import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import {
  emptyFeatureDataset,
  LocationFeatureDataset,
} from '@/components/common/mapFSM/useLocationFeatureLoader';
import { usePrevious } from '@/hooks/usePrevious';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import { exists } from '@/utils';

import { useWorklistContext } from './context/WorklistContext';

export const WorklistMapClickMonitor: React.FunctionComponent<unknown> = () => {
  const { add, addRange } = useWorklistContext();
  const { worklistLocationFeatureDataset } = useMapStateMachine();
  const previous = usePrevious(worklistLocationFeatureDataset);

  useDeepCompareEffect(() => {
    if (
      exists(worklistLocationFeatureDataset) &&
      previous !== worklistLocationFeatureDataset &&
      previous !== undefined
    ) {
      // Loop over the location featurecollection, adding it to the worklist if the parcelFeature is not there already
      const worklistParcels: LocationFeatureDataset[] = [];

      if (exists(worklistLocationFeatureDataset.parcelFeatures)) {
        const pmbcParcels =
          worklistLocationFeatureDataset.parcelFeatures
            ?.map(pmbcFeature => {
              const newParcel: LocationFeatureDataset = {
                ...emptyFeatureDataset(),
                parcelFeatures: [pmbcFeature],
                location: worklistLocationFeatureDataset.location,
                regionFeature: worklistLocationFeatureDataset.regionFeature,
                districtFeature: worklistLocationFeatureDataset.districtFeature,
              };

              return newParcel;
            })
            .filter(exists) ?? [];

        worklistParcels.push(...pmbcParcels);
      }

      if (exists(worklistLocationFeatureDataset.pimsFeatures)) {
        const pimsParcels =
          worklistLocationFeatureDataset.pimsFeatures
            ?.map(pimsFeature => {
              const newParcel: LocationFeatureDataset = {
                ...emptyFeatureDataset(),
                pimsFeatures: [pimsFeature],
                location: worklistLocationFeatureDataset.location,
                regionFeature: worklistLocationFeatureDataset.regionFeature,
                districtFeature: worklistLocationFeatureDataset.districtFeature,
              };

              return newParcel;
            })
            .filter(exists) ?? [];

        worklistParcels.push(...pimsParcels);
      }

      if (worklistParcels.length > 0) {
        addRange(worklistParcels);
      } else {
        // We didn't find any parcel-map properties - add a lat/long location to the worklist
        const latLongParcel: LocationFeatureDataset = {
          ...emptyFeatureDataset(),
          location: worklistLocationFeatureDataset.location,
        };

        add(latLongParcel);
      }
    }
  }, [previous, worklistLocationFeatureDataset]);

  return null;
};

export default WorklistMapClickMonitor;
