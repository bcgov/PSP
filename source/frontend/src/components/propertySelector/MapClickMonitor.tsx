import { LatLngLiteral } from 'leaflet';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import useDraftMarkerSynchronizer from '@/hooks/useDraftMarkerSynchronizer';
import { usePrevious } from '@/hooks/usePrevious';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import { featuresetToMapProperty } from '@/utils/mapPropertyUtils';

import { LocationFeatureDataset } from '../common/mapFSM/useLocationFeatureLoader';

interface IMapClickMonitorProps {
  addProperty: (property: LocationFeatureDataset) => void;
  repositionProperty: (
    property: LocationFeatureDataset,
    latLng: LatLngLiteral,
    propertyIndex: number | null,
  ) => void;
  modifiedProperties: LocationFeatureDataset[]; // TODO: this should be just a list of lat longs
  selectedComponentId: string | null;
}

export const MapClickMonitor: React.FunctionComponent<IMapClickMonitorProps> = ({
  addProperty,
  repositionProperty,
  modifiedProperties,
  selectedComponentId,
}) => {
  const mapMachine = useMapStateMachine();

  const previous = usePrevious(mapMachine.mapLocationFeatureDataset);
  const modifiedMapProperties = modifiedProperties.map(mp => featuresetToMapProperty(mp));
  useDraftMarkerSynchronizer(selectedComponentId ? [] : modifiedMapProperties); // disable the draft marker synchronizer if the selecting component is set - the parent will need to control the draft markers.

  useDeepCompareEffect(() => {
    if (
      mapMachine.isSelecting &&
      mapMachine.mapLocationFeatureDataset &&
      previous !== mapMachine.mapLocationFeatureDataset &&
      previous !== undefined &&
      (!selectedComponentId ||
        selectedComponentId === mapMachine.mapLocationFeatureDataset.selectingComponentId)
    ) {
      addProperty(mapMachine.mapLocationFeatureDataset);
    }

    if (
      mapMachine.isRepositioning &&
      mapMachine.repositioningFeatureDataset &&
      mapMachine.mapLocationFeatureDataset &&
      previous !== mapMachine.mapLocationFeatureDataset &&
      previous !== undefined &&
      (!selectedComponentId ||
        selectedComponentId === mapMachine.mapLocationFeatureDataset.selectingComponentId)
    ) {
      repositionProperty(
        mapMachine.repositioningFeatureDataset,
        mapMachine.mapLocationFeatureDataset.location,
        mapMachine.repositioningPropertyIndex,
      );
    }
  }, [
    addProperty,
    mapMachine.isSelecting,
    mapMachine.isRepositioning,
    mapMachine.mapLocationFeatureDataset,
    mapMachine.repositioningFeatureDataset,
    previous,
  ]);
  return <></>;
};

export default MapClickMonitor;
