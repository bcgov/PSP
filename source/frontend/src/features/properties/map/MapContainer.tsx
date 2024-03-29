import clsx from 'classnames';
import React, { useState } from 'react';
import { MapContainerProps } from 'react-leaflet';
import styled from 'styled-components';

import DraftSvg from '@/assets/images/pins/icon-draft.svg';
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
  DEFAULT = 'default',
}

const MapContainer: React.FC<React.PropsWithChildren<MapContainerProps>> = () => {
  const [showActionBar, setShowActionBar] = useState(false);
  const { isSelecting, isFiltering, isShowingMapLayers, toggleMapFilter, toggleMapLayer } =
    useMapStateMachine();

  const cursorClass = isSelecting ? MapCursors.DRAFT : MapCursors.DEFAULT;

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
        isOpen={isFiltering}
        toggle={toggleMapFilter}
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
        toggle={toggleMapLayer}
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
`;

export default MapContainer;
