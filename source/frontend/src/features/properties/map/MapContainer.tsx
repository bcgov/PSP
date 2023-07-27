import clsx from 'classnames';
import React, { useState } from 'react';
import styled from 'styled-components';

import DraftSvg from '@/assets/images/pins/icon-draft.svg';
import { SideBarType } from '@/components/common/mapFSM/machineDefinition/types';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import MapView from '@/components/maps/MapView';
import { FilterProvider } from '@/components/maps/providers/FIlterProvider';
import AdvancedFilterSideBar from '@/features/advancedFilterSideBar/AdvancedFilterSideBar';
import { SideBarContextProvider } from '@/features/mapSideBar/context/sidebarContext';
import MapSideBar from '@/features/mapSideBar/MapSideBar';
import ActivityRouter from '@/features/mapSideBar/router/ActivityRouter';
import CompensationRequisitionRouter from '@/features/mapSideBar/router/CompensationRequisitionRouter';
import PopupRouter from '@/features/mapSideBar/router/PopupRouter';

enum MapCursors {
  DRAFT = 'draft-cursor',
  DEFAULT = 'default',
}

interface MapContainerProps {}

const MapContainer: React.FC<React.PropsWithChildren<MapContainerProps>> = () => {
  const [showActionBar, setShowActionBar] = useState(false);
  const {
    isSelecting,
    isSidebarOpen,
    isAdvancedFilterSidebarOpen,
    sideBarType,
    closeAdvancedFilterSidebar,
  } = useMapStateMachine();

  // Given advanced search is open, when user navigates to a file, the advanced search should be closed.
  React.useEffect(() => {
    const fileTypes = [
      SideBarType.RESEARCH_FILE,
      SideBarType.ACQUISITION_FILE,
      SideBarType.LEASE_FILE,
      SideBarType.PROJECT,
    ];
    if (isAdvancedFilterSidebarOpen && isSidebarOpen && fileTypes.includes(sideBarType)) {
      closeAdvancedFilterSidebar();
    }
  }, [closeAdvancedFilterSidebar, isAdvancedFilterSidebarOpen, isSidebarOpen, sideBarType]);

  const cursorClass = isSelecting ? MapCursors.DRAFT : MapCursors.DEFAULT;

  return (
    <StyleMapView className={clsx(cursorClass)}>
      <SideBarContextProvider>
        <MapSideBar />
        <ActivityRouter setShowActionBar={setShowActionBar} />
        <PopupRouter setShowActionBar={setShowActionBar} />
        <CompensationRequisitionRouter setShowActionBar={setShowActionBar} />
      </SideBarContextProvider>
      {!showActionBar && (
        <FilterProvider>
          <MapView />
        </FilterProvider>
      )}
      <AdvancedFilterSideBar
        isOpen={isAdvancedFilterSidebarOpen}
        onClose={closeAdvancedFilterSidebar}
      />
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
