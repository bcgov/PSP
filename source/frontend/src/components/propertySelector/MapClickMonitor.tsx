import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import useDraftMarkerSynchronizer from '@/hooks/useDraftMarkerSynchronizer';
import { usePrevious } from '@/hooks/usePrevious';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import { featuresetToMapProperty } from '@/utils/mapPropertyUtils';

import { IMapProperty } from './models';

interface IMapClickMonitorProps {
  addProperty: (property: IMapProperty) => void; // TODO: This should be a featureDataset
  modifiedProperties: IMapProperty[]; // TODO: this should be just a list of lat longs
  selectedComponentId: string | null;
}

export const MapClickMonitor: React.FunctionComponent<
  React.PropsWithChildren<IMapClickMonitorProps>
> = ({ addProperty, modifiedProperties, selectedComponentId }) => {
  const mapMachine = useMapStateMachine();

  const previous = usePrevious(mapMachine.mapLocationFeatureDataset);
  useDraftMarkerSynchronizer(selectedComponentId ? [] : modifiedProperties); // disable the draft marker synchronizer if the selecting component is set - the parent will need to control the draft markers.

  useDeepCompareEffect(() => {
    if (
      mapMachine.isSelecting &&
      mapMachine.mapLocationFeatureDataset &&
      previous !== mapMachine.mapLocationFeatureDataset &&
      previous !== undefined &&
      (!selectedComponentId ||
        selectedComponentId === mapMachine.mapLocationFeatureDataset.selectingComponentId)
    ) {
      addProperty(featuresetToMapProperty(mapMachine.mapLocationFeatureDataset));
    }
  }, [addProperty, mapMachine.isSelecting, mapMachine.mapLocationFeatureDataset, previous]);
  return <></>;
};

export default MapClickMonitor;
