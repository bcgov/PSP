import clsx from 'classnames';
import React, { useState } from 'react';
import styled from 'styled-components';

import DraftSvg from '@/assets/images/pins/icon-draft.svg';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { FilterContentContainer } from '@/components/maps/leaflet/Control/AdvancedFilter/FilterContentContainer';
import { FilterContentForm } from '@/components/maps/leaflet/Control/AdvancedFilter/FilterContentForm';
import MapView from '@/components/maps/MapView';
import { FilterProvider } from '@/components/maps/providers/FIlterProvider';
import AdvancedFilterBar from '@/features/advancedFilterBar/AdvancedFilterBar';
import { SideBarContextProvider } from '@/features/mapSideBar/context/sidebarContext';
import MapSideBar from '@/features/mapSideBar/MapSideBar';
import CompensationRequisitionRouter from '@/features/mapSideBar/router/CompensationRequisitionRouter';
import PropertyActivityRouter from '@/features/mapSideBar/router/PropertyActivityRouter';
import { MapContainerProps } from 'react-leaflet';

enum MapCursors {
  DRAFT = 'draft-cursor',
  DEFAULT = 'default',
}

const MapContainer: React.FC<React.PropsWithChildren<MapContainerProps>> = () => {
  const [showActionBar, setShowActionBar] = useState(false);
  const { isSelecting, isFiltering, toggleMapFilter } = useMapStateMachine();

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
      <AdvancedFilterBar isOpen={isFiltering} toggle={toggleMapFilter}>
        <FilterContentContainer View={FilterContentForm} />
      </AdvancedFilterBar>
    </StyleMapView>
  );
};

const StyleMapView = styled.div`
  display: flex;
  flex-direction: row;
  width: 100%;
  &.draft-cursor,
  &.draft-cursor .leaflet-grab,
  &.draft-cursor .leaflet-interactive {
    cursor: url(${DraftSvg}) 15 45, pointer;
  }
`;

export default MapContainer;
