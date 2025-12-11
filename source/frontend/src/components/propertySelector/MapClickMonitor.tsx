import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { usePrevious } from '@/hooks/usePrevious';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import { exists, isValidId } from '@/utils';

import { LocationFeatureDataset } from '../common/mapFSM/useLocationFeatureLoader';

interface IMapClickMonitorProps {
  onNewLocation: (locationDataset: LocationFeatureDataset, hasMultipleProperties: boolean) => void;
}

export const MapClickMonitor: React.FunctionComponent<IMapClickMonitorProps> = ({
  onNewLocation,
}) => {
  const mapMachine = useMapStateMachine();

  const previous = usePrevious(mapMachine.mapLocationFeatureDataset);

  useDeepCompareEffect(() => {
    const selectedFeature = mapMachine.mapLocationFeatureDataset;

    if (exists(selectedFeature) && previous !== selectedFeature) {
      const parcelFeaturesNotInPims =
        selectedFeature.parcelFeatures?.filter(pf => {
          const matchingProperty = selectedFeature.pimsFeatures?.find(
            plp =>
              (isValidId(pf.properties.PID_NUMBER) &&
                plp.properties.PID === pf.properties.PID_NUMBER) ||
              (isValidId(pf.properties.PIN) && plp.properties.PIN === pf.properties.PIN),
          );
          return !exists(matchingProperty);
        }) ?? [];

      const hasMultipleProperties =
        parcelFeaturesNotInPims.length + (selectedFeature.pimsFeatures?.length ?? 0) > 1;
      onNewLocation(selectedFeature, hasMultipleProperties);
    }
  }, [mapMachine.mapLocationFeatureDataset, previous]);
  return <></>;
};

export default MapClickMonitor;
