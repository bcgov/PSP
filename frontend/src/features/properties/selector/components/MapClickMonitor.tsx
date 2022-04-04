import useDeepCompareEffect from 'hooks/useDeepCompareEffect';
import useDraftMarkerSynchronizer from 'hooks/useDraftMarkerSynchronizer';
import { usePrevious } from 'hooks/usePrevious';
import { IProperty } from 'interfaces';
import { geoJSON } from 'leaflet';
import * as React from 'react';
import { useAppSelector } from 'store/hooks';

interface IMapClickMonitorProps {
  addProperty: (property: IProperty) => void;
}

export const MapClickMonitor: React.FunctionComponent<IMapClickMonitorProps> = ({
  addProperty,
}) => {
  const feature = useAppSelector(store => store.parcelLayerData?.parcelLayerFeature);
  const previous = usePrevious(feature);
  useDraftMarkerSynchronizer('properties');

  useDeepCompareEffect(() => {
    if (
      feature &&
      previous !== feature &&
      previous !== undefined &&
      feature?.properties?.IS_SELECTED
    ) {
      const latLng = geoJSON(feature.geometry)
        .getBounds()
        .getCenter();
      addProperty({
        pid: feature?.properties?.PID_NUMBER ?? '',
        pin: feature?.properties?.PIN ?? '',
        latitude: latLng.lat ?? '',
        longitude: latLng.lng ?? '',
        planNumber: feature?.properties?.PLAN_NUMBER ?? '',
        address: 'placeholder', //todo: need alternate source for this
        legalDescription: 'placeholder', //todo: need access to fully attributed parcelmap bc layer
        name: '',
      } as any);
    }
  }, [addProperty, feature, previous]);
  return <></>;
};

export default MapClickMonitor;
