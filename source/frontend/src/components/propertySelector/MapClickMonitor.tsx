import * as React from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import useDraftMarkerSynchronizer from '@/hooks/useDraftMarkerSynchronizer';
import { usePrevious } from '@/hooks/usePrevious';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import { featuresetToMapProperty } from '@/utils/mapPropertyUtils';

import { IMapProperty } from './models';

interface IMapClickMonitorProps {
  addProperty: (property: IMapProperty) => void; // TODO: This should be a featureDataset
  modifiedProperties: IMapProperty[]; // TODO: this should be just a list of lat longs
}

export const MapClickMonitor: React.FunctionComponent<
  React.PropsWithChildren<IMapClickMonitorProps>
> = ({ addProperty, modifiedProperties }) => {
  const mapMachine = useMapStateMachine();

  const previous = usePrevious(mapMachine.mapLocationFeatureDataset);
  useDraftMarkerSynchronizer(modifiedProperties);

  useDeepCompareEffect(() => {
    if (
      mapMachine.isSelecting &&
      mapMachine.mapLocationFeatureDataset &&
      previous !== mapMachine.mapLocationFeatureDataset &&
      previous !== undefined
    ) {
      addProperty(featuresetToMapProperty(mapMachine.mapLocationFeatureDataset));
    }
  }, [addProperty, mapMachine.isSelecting, mapMachine.mapLocationFeatureDataset, previous]);
  return <></>;
};

export default MapClickMonitor;
