import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { usePrevious } from '@/hooks/usePrevious';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import { exists } from '@/utils';

import { useWorklistContext } from './context/WorklistContext';
import { ParcelFeature } from './models';

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
      const worklistParcels: ParcelFeature[] =
        worklistLocationFeatureDataset.fullyAttributedFeatures?.features?.map(pmbcFeature => {
          const newParcel = ParcelFeature.fromFullyAttributedFeature(pmbcFeature);
          newParcel.location = worklistLocationFeatureDataset.location;
          return newParcel;
        }) ?? [];

      if (worklistParcels.length > 0) {
        addRange(worklistParcels);
      } else {
        // We didn't find any parcel-map properties - add a lat/long location to the worklist
        const latLongParcel = new ParcelFeature();
        latLongParcel.location = worklistLocationFeatureDataset.location;
        latLongParcel.pmbcFeature = null;
        add(latLongParcel);
      }
    }
  }, [previous, worklistLocationFeatureDataset]);

  return null;
};

export default WorklistMapClickMonitor;
