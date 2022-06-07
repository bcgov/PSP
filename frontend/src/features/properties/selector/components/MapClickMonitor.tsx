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
    pid: selectedFeature?.properties?.PID_NUMBER ?? undefined,
    pin: selectedFeature?.properties?.PIN ?? undefined,
    latitude: selectedFeature?.properties?.CLICK_LAT_LNG?.lat ?? latLng.lat ?? undefined,
    longitude: selectedFeature?.properties?.CLICK_LAT_LNG?.lng ?? latLng.lng ?? undefined,
    planNumber: selectedFeature?.properties?.PLAN_NUMBER ?? undefined,
    address: 'placeholder', //todo: need alternate source for this
    legalDescription: 'placeholder', //todo: need access to fully attributed parcelmap bc layer,
    region: selectedFeature?.properties?.REGION_NUMBER,
    regionName: selectedFeature?.properties?.REGION_NAME,
    district: selectedFeature?.properties?.DISTRICT_NUMBER,
    districtName: selectedFeature?.properties?.DISTRICT_NAME,
  } as IMapProperty;
};

export default MapClickMonitor;
