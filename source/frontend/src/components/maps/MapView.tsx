import classNames from 'classnames';
import React, { useContext } from 'react';
import { useResizeDetector } from 'react-resize-detector';
import VisibilitySensor from 'react-visibility-sensor';

import LoadingBackdrop from '../common/LoadingBackdrop';
import * as Styled from './leaflet/styles';
import MapLeafletView from './MapLeafletView';
import MapSearch from './MapSearch';
import { MapStateContext } from './providers/MapStateContext';
import { PropertyContext } from './providers/PropertyContext';

export type MapViewProps = {
  showSideBar: boolean;
};

/**
 * Creates a Leaflet map and by default includes a number of preconfigured layers.
 * @param param0
 */

const MapView: React.FC<React.PropsWithChildren<MapViewProps>> = ({ showSideBar }) => {
  const { loading: mapLoading } = useContext(MapStateContext);
  const { propertiesLoading } = useContext(PropertyContext);
  const { width, ref: resizeRef } = useResizeDetector();

  return (
    <VisibilitySensor partialVisibility={true}>
      {({ isVisible }: { isVisible: boolean }) => (
        <Styled.MapGrid
          ref={resizeRef}
          className={classNames('px-0', 'map', { sidebar: showSideBar })}
        >
          <LoadingBackdrop show={propertiesLoading || mapLoading} parentScreen />
          {!showSideBar ? <MapSearch /> : null}
          {isVisible && <MapLeafletView parentWidth={width} />}
        </Styled.MapGrid>
      )}
    </VisibilitySensor>
  );
};

export default MapView;
