import classNames from 'classnames';
import React from 'react';
import { useResizeDetector } from 'react-resize-detector';
import VisibilitySensor from 'react-visibility-sensor';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';

import LoadingBackdrop from '../common/LoadingBackdrop';
import * as Styled from './leaflet/styles';
import MapLeafletView from './MapLeafletView';
import MapSearch from './MapSearch';

export type MapViewProps = {};

/**
 * Creates a Leaflet map and by default includes a number of preconfigured layers.
 * @param param0
 */

const MapView: React.FC<React.PropsWithChildren<MapViewProps>> = () => {
  const { width, ref: resizeRef } = useResizeDetector();

  const mapMachine = useMapStateMachine();
  const showSideBar = mapMachine.isSidebarOpen;

  return (
    <VisibilitySensor partialVisibility={true}>
      {({ isVisible }: { isVisible: boolean }) => (
        <Styled.MapGrid
          ref={resizeRef}
          className={classNames('px-0', 'map', { sidebar: showSideBar })}
        >
          <LoadingBackdrop show={mapMachine.isLoading} parentScreen />
          {!showSideBar ? <MapSearch /> : null}
          {isVisible && <MapLeafletView parentWidth={width} />}
        </Styled.MapGrid>
      )}
    </VisibilitySensor>
  );
};

export default MapView;
