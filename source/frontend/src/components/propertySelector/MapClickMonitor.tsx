import { MapStateContext } from 'components/maps/providers/MapStateContext';
import useDeepCompareEffect from 'hooks/useDeepCompareEffect';
import useDraftMarkerSynchronizer from 'hooks/useDraftMarkerSynchronizer';
import { usePrevious } from 'hooks/usePrevious';
import * as React from 'react';
import { useContext } from 'react';
import { mapFeatureToProperty } from 'utils/mapPropertyUtils';

import { IMapProperty } from './models';

interface IMapClickMonitorProps {
  addProperty: (property: IMapProperty) => void;
  modifiedProperties: IMapProperty[];
}

export const MapClickMonitor: React.FunctionComponent<
  React.PropsWithChildren<IMapClickMonitorProps>
> = ({ addProperty, modifiedProperties }) => {
  const { selectedFeature } = useContext(MapStateContext);
  const previous = usePrevious(selectedFeature);
  useDraftMarkerSynchronizer(modifiedProperties);

  useDeepCompareEffect(() => {
    if (
      selectedFeature &&
      previous !== selectedFeature &&
      previous !== undefined &&
      selectedFeature?.properties?.IS_SELECTED
    ) {
      addProperty(mapFeatureToProperty(selectedFeature));
    }
  }, [addProperty, selectedFeature, previous]);
  return <></>;
};

export default MapClickMonitor;
