import { LeafletEventHandlerFnMap } from 'leaflet';
import React from 'react';
import { useMapEvents } from 'react-leaflet';

export type MapEventsProps = LeafletEventHandlerFnMap;

export const MapEvents: React.FC<React.PropsWithChildren<MapEventsProps>> = ({ ...props }) => {
  // hook attaching the provided event handlers to the underlying leaflet map instance and returning it
  useMapEvents(props);
  return null;
};
