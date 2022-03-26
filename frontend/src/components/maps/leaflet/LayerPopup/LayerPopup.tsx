import { Feature } from 'geojson';
import { LatLng, LatLngBounds } from 'leaflet';
import noop from 'lodash/noop';
import React from 'react';
import { Popup } from 'react-leaflet';

import { LayerPopupContent, PopupContentConfig } from '.';
import { LayerPopupTitle } from './styles';

export type LayerPopupInformation = PopupContentConfig & {
  latlng: LatLng;
  title: string;
  center?: LatLng;
  bounds?: LatLngBounds;
  feature: Feature;
};

export interface ILayerPopupProps {
  layerPopup: LayerPopupInformation;
  onClose?: () => void;
  onAddToParcel?: (e: MouseEvent, data: { [key: string]: any }) => void;
}

export const LayerPopup: React.FC<ILayerPopupProps> = ({ layerPopup, onClose, onAddToParcel }) => {
  return (
    <Popup
      position={layerPopup.latlng}
      offset={[0, -25]}
      onClose={onClose ?? noop}
      closeButton={true}
      autoPan={false}
    >
      <LayerPopupTitle>{layerPopup.title}</LayerPopupTitle>
      <LayerPopupContent
        data={layerPopup.data as any}
        config={layerPopup.config as any}
        center={layerPopup.center}
        onAddToParcel={onAddToParcel ?? noop}
        bounds={layerPopup.bounds}
      />
    </Popup>
  );
};
