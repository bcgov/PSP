import { Feature, GeoJsonProperties } from 'geojson';
import { LatLng, LatLngBounds } from 'leaflet';
import React from 'react';
import { Popup } from 'react-leaflet';

import { PopupContentConfig } from '.';
import { LayerPopupContainer } from './LayerPopupContainer';

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
  onClose?: () => void;
  onViewPropertyInfo: (pid?: string | null) => void;
}

export const LayerPopup: React.FC<React.PropsWithChildren<ILayerPopupProps>> = props => {
  return (
    <Popup
      position={props.layerPopup.latlng}
      offset={[0, -25]}
      onClose={props?.onClose}
      closeButton={true}
      autoPan={false}
    >
      <LayerPopupContainer {...props} />
    </Popup>
  );
};
