import { Feature, GeoJsonProperties } from 'geojson';
import { geoJSON, LatLngBounds, LatLngLiteral, Popup as LeafletPopup } from 'leaflet';
import React, { useEffect, useState } from 'react';
import { Popup } from 'react-leaflet';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';

import { PopupContentConfig } from './components/LayerPopupContent';
import { municipalityLayerPopupConfig, parcelLayerPopupConfig } from './constants';
import { LayerPopupView } from './LayerPopupView';

export type LayerPopupInformation = {
  latlng: LatLngLiteral;
  title: string;
  bounds?: LatLngBounds;
  feature?: Feature;
  /**
   * Data coming from the GeoJSON feature.properties
   * @property
   * @example
   * feature: {
   *  properties: {
   *    'ADMIN_AREA_ID': 1,
   *    'ADMIN_AERA_NAME: 'West Saanich'
   *  }
   * }
   */
  data: GeoJsonProperties;
  /**
   * A configuration used to display the properties fields in the popup content
   * @property
   * @example
   * {ADMIN_AREA_SQFT: (data: any) => `${data.ADMIN_AREA_SQFT} ft^2`}
   */
  config: PopupContentConfig;
};

export interface ILayerPopupProps {}

const emptyLayerPopupInformation: LayerPopupInformation = {
  latlng: { lat: 0, lng: 0 },
  title: '',
  data: null,
  config: {},
};

export const LayerPopupContainer = React.forwardRef<
  LeafletPopup,
  React.PropsWithChildren<ILayerPopupProps>
>((props, ref) => {
  const mapMachine = useMapStateMachine();

  const [layerPopup, setLayerPopup] = useState<LayerPopupInformation>(emptyLayerPopupInformation);

  useEffect(() => {
    if (mapMachine.mapLocationFeatureDataset) {
      const featureSet = mapMachine.mapLocationFeatureDataset;

      let title__ = 'Location Information';
      let mapBounds__: LatLngBounds | undefined;
      let displayConfig__: PopupContentConfig = {};
      let properties__: GeoJsonProperties = {};
      let feature__;

      if (featureSet.parcelFeature !== null) {
        title__ = 'LTSA ParcelMap data';
        mapBounds__ = featureSet.parcelFeature.geometry
          ? geoJSON(featureSet.parcelFeature.geometry).getBounds()
          : undefined;
        displayConfig__ = parcelLayerPopupConfig;
        properties__ = featureSet.parcelFeature.properties;
        feature__ = featureSet.parcelFeature;
      } else if (featureSet.municipalityFeature !== null) {
        title__ = 'Municipality Information';
        mapBounds__ = featureSet.municipalityFeature.geometry
          ? geoJSON(featureSet.municipalityFeature.geometry).getBounds()
          : undefined;
        displayConfig__ = municipalityLayerPopupConfig;
        properties__ = featureSet.municipalityFeature.properties;
        feature__ = featureSet.municipalityFeature;
      }

      setLayerPopup({
        latlng: mapMachine.mapLocationFeatureDataset.location,
        title: title__,
        bounds: mapBounds__,
        feature: feature__,
        data: properties__,
        config: displayConfig__,
      });
    }
  }, [mapMachine]);

  return (
    <Popup
      ref={ref}
      position={layerPopup.latlng}
      offset={[0, -25]}
      closeButton={false}
      autoPan={false}
      autoClose={false}
      closeOnClick={false}
      closeOnEscapeKey={false}
    >
      <LayerPopupView
        {...props}
        layerPopup={layerPopup}
        featureDataset={mapMachine.mapLocationFeatureDataset}
      />
    </Popup>
  );
});
