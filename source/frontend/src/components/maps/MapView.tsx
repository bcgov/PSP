import classNames from 'classnames';
import React from 'react';
import { useResizeDetector } from 'react-resize-detector';
import VisibilitySensor from 'react-visibility-sensor';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';

import LoadingBackdrop from '../common/LoadingBackdrop';
import * as Styled from './leaflet/styles';
import MapLeafletView from './MapLeafletView';
import MapSearch from './MapSearch';

export type MapViewProps = object;

/**
 * Container for the map component.
 * @param param0
 */

const MapView: React.FC<React.PropsWithChildren<MapViewProps>> = () => {
  const { width, ref: resizeRef } = useResizeDetector();

  // hide the top search bar when either the left-hand sidebar or right-hand advanced bar is open
  const mapMachine = useMapStateMachine();
  const isShowingSearchBar = mapMachine.isShowingSearchBar;

  return (
    <VisibilitySensor partialVisibility={true}>
      {({ isVisible }: { isVisible: boolean }) => (
        <Styled.MapGrid
          ref={resizeRef}
          className={classNames('px-0', 'map', {
            hideSearchBar: !isShowingSearchBar,
          })}
        >
          <LoadingBackdrop show={mapMachine.isLoading} parentScreen />
          {isShowingSearchBar && <MapSearch />}
          {isVisible && <MapLeafletView parentWidth={width} />}
        </Styled.MapGrid>
      )}
    </VisibilitySensor>
  );
};

export default MapView;
