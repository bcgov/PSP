import classNames from 'classnames';
import React from 'react';
import { useResizeDetector } from 'react-resize-detector';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';

import LoadingBackdrop from '../common/LoadingBackdrop';
import * as Styled from './leaflet/styles';
import MapLeafletView from './MapLeafletView';
import MapSearch from './MapSearch';

/**
 * Container for the map component.
 * @param param0
 */

export type MapViewProps = {
  defaultZoom?: number;
};

const MapView: React.FC<React.PropsWithChildren<MapViewProps>> = ({ defaultZoom }) => {
  const { width, ref: resizeRef } = useResizeDetector();

  // hide the top search bar when either the left-hand sidebar or right-hand advanced bar is open
  const mapMachine = useMapStateMachine();
  const isShowingSearchBar = mapMachine.isShowingSearchBar;

  return (
    <Styled.MapGrid
      ref={resizeRef}
      className={classNames('px-0', 'map', {
        hideSearchBar: !isShowingSearchBar,
      })}
    >
      <LoadingBackdrop show={mapMachine.isLoading} parentScreen />
      {isShowingSearchBar && <MapSearch />}
      <MapLeafletView parentWidth={width} defaultZoom={defaultZoom} />
    </Styled.MapGrid>
  );
};

export default MapView;
