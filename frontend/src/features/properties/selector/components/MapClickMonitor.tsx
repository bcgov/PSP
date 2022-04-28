import { SelectedPropertyContext } from 'components/maps/providers/SelectedPropertyContext';
import { Feature, GeoJsonProperties, Geometry } from 'geojson';
import useDeepCompareEffect from 'hooks/useDeepCompareEffect';
import useDraftMarkerSynchronizer from 'hooks/useDraftMarkerSynchronizer';
import { usePrevious } from 'hooks/usePrevious';
import { geoJSON } from 'leaflet';
import * as React from 'react';
import { useContext } from 'react';

import { IMapProperty } from '../models';

interface IMapClickMonitorProps {
  addProperty: (property: IMapProperty) => void;
}

export const MapClickMonitor: React.FunctionComponent<IMapClickMonitorProps> = ({
  addProperty,
}) => {
  const { selectedFeature } = useContext(SelectedPropertyContext);
  const previous = usePrevious(selectedFeature);
  useDraftMarkerSynchronizer('properties');

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

export const mapFeatureToProperty = (selectedFeature: Feature<Geometry, GeoJsonProperties>) => {
  const latLng = geoJSON(selectedFeature.geometry)
    .getBounds()
    .getCenter();
  return {
    pid: selectedFeature?.properties?.PID_NUMBER ?? '',
    pin: selectedFeature?.properties?.PIN ?? '',
    latitude: selectedFeature?.properties?.CLICK_LAT_LNG?.lat ?? latLng.lat ?? '',
    longitude: selectedFeature?.properties?.CLICK_LAT_LNG?.lng ?? latLng.lng ?? '',
    planNumber: selectedFeature?.properties?.PLAN_NUMBER ?? '',
    address: 'placeholder', //todo: need alternate source for this
    legalDescription: 'placeholder', //todo: need access to fully attributed parcelmap bc layer,
    district: selectedFeature?.properties?.REGIONAL_DISTRICT ?? '', // todo: this returns a named district,
  } as IMapProperty;
};

export default MapClickMonitor;
