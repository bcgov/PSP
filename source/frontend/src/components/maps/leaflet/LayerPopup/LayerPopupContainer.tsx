import { Feature, GeoJsonProperties } from 'geojson';
import { LatLng, LatLngBounds, Popup as LeafletPopup } from 'leaflet';
import React from 'react';
import { Popup } from 'react-leaflet';

import { PopupContentConfig } from './components/LayerPopupContent';
import { LayerPopupView } from './LayerPopupView';

export type LayerPopupInformation = {
  latlng: LatLng;
  title: string;
  center?: LatLng;
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
  pimsProperty?: Feature;
};

export interface ILayerPopupProps {
  layerPopup: LayerPopupInformation;
  onViewPropertyInfo: (pid?: string | null) => void;
}

export const LayerPopupContainer = React.forwardRef<
  LeafletPopup,
  React.PropsWithChildren<ILayerPopupProps>
>((props, ref) => {
  return (
    <Popup
      ref={ref}
      position={props.layerPopup.latlng}
      offset={[0, -25]}
      closeButton={true}
      autoPan={false}
    >
      <LayerPopupView {...props} />
    </Popup>
  );
});
