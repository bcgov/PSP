import { useInterpret, useSelector } from '@xstate/react';
import { Feature, GeoJsonProperties, Geometry } from 'geojson';
import { LatLngBoundsLiteral, LatLngLiteral, LeafletMouseEvent } from 'leaflet';
import React, { useCallback, useMemo } from 'react';

import { IProperty } from '@/interfaces';

import { mapMachine } from './useMapMachine';

interface MapInformation {
  /** whether or not the map is loading */
  isLoading: boolean;
  /** whether or not the map is in "selection mode" */
  isSelecting: boolean;
  /** The currently selected boundary from the LTSA ParcelMap */
  activeParcelMapFeature: Feature<Geometry, GeoJsonProperties> | null;
  /** The currently selected feature in the context of an active file */
  activeFileFeature: Feature<Geometry, GeoJsonProperties> | null;
  /** The currently selected property from the PIMS inventory */
  activeInventoryProperty: IProperty | null;
}

interface IMapStateMachineContext {
  // event handlers
  //useMapClick: () => (event: LeafletMouseEvent) => Promise<Feature | undefined>;

  isSidebarOpen: boolean;
  flyToPending: LatLngLiteral | null;
  requestFlyTo: (latlng: LatLngLiteral) => void;
  processFlyTo: () => void;
  openSidebar: (sidebarType: string) => void;
  closeSidebar: () => void;
}

const defaultMapInfo: MapInformation = {
  isLoading: false,
  isSelecting: false,
  activeParcelMapFeature: null,
  activeFileFeature: null,
  activeInventoryProperty: null,
};

const MapStateMachineContext = React.createContext<IMapStateMachineContext>(
  {} as IMapStateMachineContext,
);

export function useMapStateMachine() {
  const context = React.useContext(MapStateMachineContext);
  if (context === undefined) {
    throw new Error('useMapStateMachine must be used within a MapStateMachineContextProvider');
  }
  return context;
}

export const MapStateMachineProvider: React.FC<React.PropsWithChildren<unknown>> = ({
  children,
}) => {
  const service = useInterpret(mapMachine);
  const state = useSelector(service, state => state);

  console.log('useMapMachine.state\t\t', state.value);
  console.log('useMapMachine.context\t', state.context);

  const openSidebar = useCallback(
    (sidebarType: string) => {
      console.log('openSidebar', sidebarType);
      service.send({
        type: 'OPEN_SIDEBAR',
        sidebarType,
      });
    },
    [service],
  );

  const closeSidebar = useCallback(() => {
    console.log('closeSidebar');
    service.send({
      type: 'CLOSE_SIDEBAR',
    });
  }, [service.send]);

  const useMapClick = useCallback(() => {}, []);

  const requestFlyTo = useCallback(
    (latlng: LatLngLiteral) => {
      console.log('requestFlyTo', latlng);
      service.send({
        type: 'REQUEST_FLY_TO',
        latlng,
      });
    },
    [service],
  );
  const processFlyTo = useCallback(() => {
    console.log('processFlyTo');
    service.send({
      type: 'PROCESS_FLY_TO',
    });
  }, [service]);

  return (
    <MapStateMachineContext.Provider
      value={{
        isSidebarOpen: state.matches({ browsinMap: 'withSidebar' }),
        flyToPending: state.context.requestedFlyTo,
        processFlyTo,
        openSidebar,
        closeSidebar,
        requestFlyTo,
      }}
    >
      {children}
    </MapStateMachineContext.Provider>
  );
};
