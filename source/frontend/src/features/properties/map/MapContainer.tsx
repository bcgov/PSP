import clsx from 'classnames';
import React, { useEffect, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import DraftSvg from '@/assets/images/pins/icon-draft.svg';
import { Button } from '@/components/common/buttons/Button';
import { useMapStateMachine } from '@/components/maps/hooks/MapStateMachineContext';
import MapView from '@/components/maps/MapView';
import { FilterProvider } from '@/components/maps/providers/FIlterProvider';
import { MapStateContext } from '@/components/maps/providers/MapStateContext';
import { PropertyContextProvider } from '@/components/maps/providers/PropertyContext';
import Container from '@/features/contacts/contact/detail/Container';
import { SideBarContextProvider } from '@/features/mapSideBar/context/sidebarContext';
import MapSideBar from '@/features/mapSideBar/MapSideBar';
import ActivityRouter from '@/features/mapSideBar/router/ActivityRouter';
import CompensationRequisitionRouter from '@/features/mapSideBar/router/CompensationRequisitionRouter';
import PopupRouter from '@/features/mapSideBar/router/PopupRouter';

interface MapContainerProps {}

const MapContainer: React.FC<React.PropsWithChildren<MapContainerProps>> = (
  props: MapContainerProps,
) => {
  //const [showSideBar, setShowSideBar] = useState(false);
  const [showActionBar, setShowActionBar] = useState(false);

  const { isSidebarOpen, openSidebar, closeSidebar } = useMapStateMachine();

  return (
    <MapStateContext.Consumer>
      {({ cursor }) => (
        <PropertyContextProvider>
          <StyledRow>
            <Col>
              <Button onClick={() => openSidebar('welp')}>HERE OPEN</Button>
              <Button onClick={() => closeSidebar()}>CLose</Button>
            </Col>
          </StyledRow>
          <StyleMapView className={clsx(cursor)}>
            <SideBarContextProvider>
              <MapSideBar showSideBar={isSidebarOpen} />
              <ActivityRouter setShowActionBar={setShowActionBar} />
              <PopupRouter setShowActionBar={setShowActionBar} />
              <CompensationRequisitionRouter setShowActionBar={setShowActionBar} />
            </SideBarContextProvider>
            {!showActionBar && (
              <FilterProvider>
                <MapView showSideBar={isSidebarOpen} />
              </FilterProvider>
            )}
          </StyleMapView>
        </PropertyContextProvider>
      )}
    </MapStateContext.Consumer>
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

const StyledRow = styled(Row)`
  position: absolute;
  z-index: 1000000;
`;

export default MapContainer;
