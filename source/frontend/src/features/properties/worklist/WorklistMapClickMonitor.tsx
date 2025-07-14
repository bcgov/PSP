import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { usePrevious } from '@/hooks/usePrevious';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import { exists } from '@/utils';

import { useWorklistContext } from './context/WorklistContext';
import { ParcelFeature } from './models';

export const WorklistMapClickMonitor: React.FunctionComponent<unknown> = () => {
  const { addRange } = useWorklistContext();
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
      }
    }
  }, [previous, worklistLocationFeatureDataset]);

  return null;
};

export default WorklistMapClickMonitor;
