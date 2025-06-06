import classNames from 'classnames';
import React from 'react';
import { useResizeDetector } from 'react-resize-detector';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';

import LoadingBackdrop from '../common/LoadingBackdrop';
import * as Styled from './leaflet/styles';
import MapLeafletView from './MapLeafletView';

export type MapViewProps = object;

/**
 * Container for the map component.
 * @param param0
 */

const MapView: React.FC<React.PropsWithChildren<MapViewProps>> = () => {
  const { width, ref: resizeRef } = useResizeDetector();

  // hide the top search bar when either the left-hand sidebar or right-hand advanced bar is open
  const mapMachine = useMapStateMachine();

  return (
    <Styled.MapGrid ref={resizeRef} className={classNames('px-0', 'map')}>
      <LoadingBackdrop show={mapMachine.isLoading} parentScreen />
      <MapLeafletView parentWidth={width} />
    </Styled.MapGrid>
  );
};

export default MapView;
