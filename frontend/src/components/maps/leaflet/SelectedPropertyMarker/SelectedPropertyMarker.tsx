import useDeepCompareEffect from 'hooks/useDeepCompareEffect';
import L from 'leaflet';
import React, { useEffect, useRef } from 'react';
import { Marker, MarkerProps } from 'react-leaflet';

/**
 * Wrapper of the React Leaflet marker to auto open the popup for a selected property
 */
const SelectedPropertyMarker: React.FC<MarkerProps & { map: L.Map; className: string }> = props => {
  const { map, position, icon, className } = props;
  const ref = useRef<L.Marker>(null);

  useDeepCompareEffect(() => {
    if (ref.current) {
      map.setView(position, map.getZoom());
    }
  }, [map, position]);

  useEffect(() => {
    if (icon?.options && ref.current?.setIcon) {
      icon.options.className = className;
      ref.current.setIcon(icon);
    }
  }, [className, icon]);

  return <Marker {...props} ref={ref} zIndexOffset={9} />;
};

export default SelectedPropertyMarker;
