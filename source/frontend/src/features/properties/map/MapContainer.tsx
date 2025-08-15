import clsx from 'classnames';
import React, { useCallback, useEffect, useState } from 'react';
import { MapContainerProps } from 'react-leaflet';
import styled from 'styled-components';

import DraftSvg from '@/assets/images/pins/icon-draft.svg';
import RelocationSvg from '@/assets/images/pins/icon-relocate.svg';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { DEFAULT_MAP_ZOOM } from '@/components/maps/constants';
import MapView from '@/components/maps/MapView';
import { FilterProvider } from '@/components/maps/providers/FilterProvider';
import { SideBarContextProvider } from '@/features/mapSideBar/context/sidebarContext';
import MapSideBar from '@/features/mapSideBar/MapSideBar';
import CompensationRequisitionRouter from '@/features/mapSideBar/router/CompensationRequisitionRouter';
import ManagementFileActivityRouter from '@/features/mapSideBar/router/ManagementFileActivityRouter';
import PropertyActivityRouter from '@/features/mapSideBar/router/PropertyActivityRouter';
import { usePimsPropertyRepository } from '@/hooks/repositories/usePimsPropertyRepository';
import { Api_PropertyFilterCriteria } from '@/models/api/ProjectFilterCriteria';

enum MapCursors {
  DRAFT = 'draft-cursor',
  REPOSITION = 'reposition-cursor',
  DEFAULT = 'default',
}

const MapContainer: React.FC<
  React.PropsWithChildren<MapContainerProps & { defaultZoom?: number }>
> = ({ defaultZoom }) => {
  const [showActionBar, setShowActionBar] = useState(false);
  const {
    isSelecting,
    isRepositioning,
    setVisiblePimsProperties,
    advancedSearchCriteria,
    isMapVisible,
    isLoading,
  } = useMapStateMachine();

  const { getMatchingProperties } = usePimsPropertyRepository();

  const matchProperties = getMatchingProperties.execute;

  const filterProperties = useCallback(
    async (filter: Api_PropertyFilterCriteria) => {
      if (isMapVisible) {
        const retrievedProperties = await matchProperties(filter);

        if (retrievedProperties !== undefined) {
          setVisiblePimsProperties(retrievedProperties);
        }
      }
    },
    [matchProperties, setVisiblePimsProperties, isMapVisible],
  );

  useEffect(() => {
    filterProperties(advancedSearchCriteria?.toApi());
  }, [filterProperties, advancedSearchCriteria, isLoading]);

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
        <ManagementFileActivityRouter setShowActionBar={setShowActionBar} />
      </SideBarContextProvider>
      {!showActionBar && (
        <FilterProvider>
          <MapView defaultZoom={defaultZoom ?? DEFAULT_MAP_ZOOM} />
        </FilterProvider>
      )}
    </StyleMapView>
  );
};

// prettier-ignore
const StyleMapView = styled.div`
  display: flex;
  flex-direction: row;
  width: 100%;
  min-width: 135rem; // any smaller and the right and left side bars conflict
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
