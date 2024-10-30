import clsx from 'classnames';
import React, { useState } from 'react';
import { MapContainerProps } from 'react-leaflet';
import styled from 'styled-components';

import DraftSvg from '@/assets/images/pins/icon-draft.svg';
import RelocationSvg from '@/assets/images/pins/icon-relocate.svg';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { FilterContentContainer } from '@/components/maps/leaflet/Control/AdvancedFilter/FilterContentContainer';
import { FilterContentForm } from '@/components/maps/leaflet/Control/AdvancedFilter/FilterContentForm';
import { LayersMenu } from '@/components/maps/leaflet/Control/LayersControl/LayersMenu';
import MapView from '@/components/maps/MapView';
import { FilterProvider } from '@/components/maps/providers/FilterProvider';
import { SideBarContextProvider } from '@/features/mapSideBar/context/sidebarContext';
import MapSideBar from '@/features/mapSideBar/MapSideBar';
import CompensationRequisitionRouter from '@/features/mapSideBar/router/CompensationRequisitionRouter';
import PropertyActivityRouter from '@/features/mapSideBar/router/PropertyActivityRouter';
import RightSideLayout from '@/features/rightSideLayout/RightSideLayout';

enum MapCursors {
  DRAFT = 'draft-cursor',
  REPOSITION = 'reposition-cursor',
  DEFAULT = 'default',
}

const MapContainer: React.FC<React.PropsWithChildren<MapContainerProps>> = () => {
  const [showActionBar, setShowActionBar] = useState(false);
  const {
    isSelecting,
    isShowingMapFilter,
    isShowingMapLayers,
    isRepositioning,
    toggleMapFilterDisplay,
    toggleMapLayerControl,
  } = useMapStateMachine();

  const cursorClass = isSelecting
    ? MapCursors.DRAFT
    : isRepositioning
    ? MapCursors.REPOSITION
    : MapCursors.DEFAULT;

  return (
    <StyleMapView className={clsx(cursorClass)}>
      <SideBarContextProvider>
        <MapSideBar />
        <CompensationRequisitionRouter setShowActionBar={setShowActionBar} />
        <PropertyActivityRouter setShowActionBar={setShowActionBar} />
      </SideBarContextProvider>
      {!showActionBar && (
        <FilterProvider>
          <MapView />
        </FilterProvider>
      )}
      <RightSideLayout
        isOpen={isShowingMapFilter}
        toggle={toggleMapFilterDisplay}
        title="Filter By:"
        closeTooltipText="Close Advanced Map Filters"
        data-testId="advanced-filter-sidebar"
      >
        <FilterContentContainer View={FilterContentForm} />
      </RightSideLayout>
      <RightSideLayout
        title="View Layer By:"
        closeTooltipText="Close Map Layers"
        data-testId="map-layers-sidebar"
        isOpen={isShowingMapLayers}
        toggle={toggleMapLayerControl}
      >
        <LayersMenu />
      </RightSideLayout>
    </StyleMapView>
  );
};

// prettier-ignore
const StyleMapView = styled.div`
  display: flex;
  flex-direction: row;
  width: 100%;
  &.draft-cursor,
  &.draft-cursor .leaflet-grab,
  &.draft-cursor .leaflet-interactive {
    // when passing a URL of SVG to a manually constructed url(), the variable should be wrapped within double quotes.
    // ref: https://vitejs.dev/guide/assets
    cursor: url("${DraftSvg}") 15 45, pointer;
  }

  &.reposition-cursor,
  &.reposition-cursor .leaflet-grab,
  &.reposition-cursor .leaflet-interactive {
    // when passing a URL of SVG to a manually constructed url(), the variable should be wrapped within double quotes.
    // ref: https://vitejs.dev/guide/assets
    cursor: url("${RelocationSvg}") 20 20, pointer;
  }
`;

export default MapContainer;
