import { useInterpret, useSelector } from '@xstate/react';
import { LatLngBounds, LatLngLiteral } from 'leaflet';
import React, { useCallback, useMemo } from 'react';
import { useHistory } from 'react-router-dom';

import { IGeoSearchParams } from '@/constants/API';
import { IPropertyFilter } from '@/features/properties/filter/IPropertyFilter';
import { pidParser } from '@/utils/propertyUtils';

import { mapMachine } from './machineDefinition/mapMachine';
import { SideBarType } from './machineDefinition/types';
import { FeatureSelected, MapFeatureData, RequestedFlyTo } from './models';
import useLoactionFeatureLoader, { LocationFeatureDataset } from './useLoactionFeatureLoader';
import { useMapSearch } from './useMapSearch';

export interface IMapStateMachineContext {
  isSidebarOpen: boolean;
  pendingFlyTo: boolean;
  requestedFlyTo: RequestedFlyTo;
  mapFeatureSelected: FeatureSelected | null;
  mapLocationSelected: LatLngLiteral | null;
  mapLocationFeatureDataset: LocationFeatureDataset | null;
  selectedFeatureDataset: LocationFeatureDataset | null;
  showPopup: boolean;
  isLoading: boolean;
  mapFilter: IPropertyFilter | null;
  mapFeatureData: MapFeatureData;
  draftLocations: LatLngLiteral[];
  pendingFitBounds: boolean;
  requestedFitBounds: LatLngBounds;
  iSelecting: boolean;

  requestFlyToLocation: (latlng: LatLngLiteral) => void;
  requestFlyToBounds: (bounds: LatLngBounds) => void;
  processFlyTo: () => void;
  processFitBounds: () => void;
  openSidebar: (sidebarType: SideBarType) => void;
  closeSidebar: () => void;
  closePopup: () => void;

  mapClick: (latlng: LatLngLiteral) => void;
  mapMarkerClick: (featureSelected: FeatureSelected) => void;

  setMapFilter: (mapFilter: IPropertyFilter) => void;
  refreshMapProperties: () => void;
  prepareForCreation: () => void;
  startSelection: () => void;
  finishSelection: () => void;
  setDraftLocations: (locations: LatLngLiteral[]) => void;
}

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
  const locationLoader = useLoactionFeatureLoader();
  const mapSearch = useMapSearch();
  const history = useHistory();

  const service = useInterpret(mapMachine, {
    actions: {
      navigateToProperty: (context, event: any) => {
        const selectedFeatureData = context.mapLocationFeatureDataset;
        if (selectedFeatureData?.pimsFeature?.properties.PROPERTY_ID) {
          const pimsFeature = selectedFeatureData.pimsFeature;
          history.push(`/mapview/sidebar/property/${pimsFeature.properties.PROPERTY_ID}`);
        } else if (selectedFeatureData?.parcelFeature?.properties.PID) {
          const parcelFeature = selectedFeatureData.parcelFeature;
          const parsedPid = pidParser(parcelFeature.properties.PID);
          history.push(`/mapview/sidebar/non-inventory-property/${parsedPid}`);
        }
      },
    },
    services: {
      loadLocationData: (context, event: any) => {
        let latLng: LatLngLiteral = { lat: 0, lng: 0 };
        if (event.type === 'MAP_CLICK') {
          latLng = event.latlng;
        } else if (event.type === 'MAP_MARKER_CLICK') {
          latLng = event.featureSelected.latlng;
        }
        const result = locationLoader.showLocationDetails(latLng, context.isSelecting);

        return result;
      },
      loadFeatures: (context: any, event: any) => {
        const geoFilter = getQueryParams(context.mapFilter);
        if (geoFilter.latitude !== undefined && geoFilter.longitude) {
          return mapSearch.searchOneLocation(
            Number(geoFilter.latitude),
            Number(geoFilter.longitude),
          );
        } else {
          return mapSearch.searchMany(geoFilter);
        }
      },
    },
  });
  const state = useSelector(service, state => state);
  const serviceSend = service.send;

  const openSidebar = useCallback(
    (sidebarType: SideBarType) => {
      serviceSend({
        type: 'OPEN_SIDEBAR',
        sidebarType,
      });
    },
    [serviceSend],
  );

  const closeSidebar = useCallback(() => {
    serviceSend({
      type: 'CLOSE_SIDEBAR',
    });
  }, [serviceSend]);

  const mapClick = useCallback(
    (latlng: LatLngLiteral) => {
      serviceSend({
        type: 'MAP_CLICK',
        latlng,
      });
    },
    [serviceSend],
  );

  const mapMarkerClick = useCallback(
    (featureSelected: FeatureSelected) => {
      serviceSend({
        type: 'MAP_MARKER_CLICK',
        featureSelected,
      });
    },
    [serviceSend],
  );

  const requestFlyToLocation = useCallback(
    (latlng: LatLngLiteral) => {
      serviceSend({
        type: 'REQUEST_FLY_TO_LOCATION',
        latlng,
      });
    },
    [serviceSend],
  );

  const requestFlyToBounds = useCallback(
    (bounds: LatLngBounds) => {
      serviceSend({
        type: 'REQUEST_FLY_TO_BOUNDS',
        bounds,
      });
    },
    [serviceSend],
  );

  const processFlyTo = useCallback(() => {
    serviceSend({
      type: 'PROCESS_FLY_TO',
    });
  }, [serviceSend]);

  const processFitBounds = useCallback(() => {
    serviceSend({
      type: 'PROCESS_FIT_BOUNDS',
    });
  }, [serviceSend]);

  const closePopup = useCallback(() => {
    serviceSend({ type: 'CLOSE_POPUP' });
  }, [serviceSend]);

  const refreshMapProperties = useCallback(() => {
    serviceSend({ type: 'REFRESH_PROPERTIES' });
  }, [serviceSend]);

  const setMapFilter = useCallback(
    (mapFilter: IPropertyFilter) => {
      serviceSend({ type: 'SET_MAP_FILTER', mapFilter });
    },
    [serviceSend],
  );

  const prepareForCreation = useCallback(() => {
    serviceSend({ type: 'PREPARE_FOR_CREATION' });
  }, [serviceSend]);

  const startSelection = useCallback(() => {
    serviceSend({ type: 'START_SELECTION' });
  }, [serviceSend]);

  const finishSelection = useCallback(() => {
    serviceSend({ type: 'FINISH_SELECTION' });
  }, [serviceSend]);

  const setDraftLocations = useCallback(
    (locations: LatLngLiteral[]) => {
      serviceSend({ type: 'SET_DRAFT_LOCATIONS', locations });
    },
    [serviceSend],
  );

  const showPopup = useMemo(() => {
    return state.context.mapLocationFeatureDataset !== null;
  }, [state.context.mapLocationFeatureDataset]);

  return (
    <MapStateMachineContext.Provider
      value={{
        isSidebarOpen: [
          { mapVisible: { sideBar: 'sidebarOpen' } },
          { mapVisible: { sideBar: 'selecting' } },
        ].some(state.matches),
        pendingFlyTo: state.matches({ mapVisible: { mapRequest: 'pendingFlyTo' } }),
        requestedFlyTo: state.context.requestedFlyTo,
        mapFeatureSelected: state.context.mapFeatureSelected,
        mapLocationSelected: state.context.mapLocationSelected,
        mapLocationFeatureDataset: state.context.mapLocationFeatureDataset,
        selectedFeatureDataset: state.context.selectedFeatureDataset,
        showPopup: showPopup,
        isLoading: state.context.isLoading,
        mapFilter: state.context.mapFilter,
        mapFeatureData: state.context.mapFeatureData,
        draftLocations: state.context.draftLocations,
        pendingFitBounds: state.matches({ mapVisible: { mapRequest: 'pendingFitBounds' } }),
        requestedFitBounds: state.context.requestedFitBounds,
        iSelecting: state.matches({ mapVisible: { sideBar: 'selecting' } }),

        setMapFilter,
        refreshMapProperties,
        processFlyTo,
        processFitBounds,
        openSidebar,
        closeSidebar,
        requestFlyToLocation,
        requestFlyToBounds,
        mapClick,
        mapMarkerClick,
        closePopup,
        prepareForCreation,
        startSelection,
        finishSelection,
        setDraftLocations,
      }}
    >
      {children}
    </MapStateMachineContext.Provider>
  );
};

const getQueryParams = (filter: IPropertyFilter): IGeoSearchParams => {
  // The map will search for either identifier.
  const pinOrPidValue = filter.pinOrPid ? filter.pinOrPid?.replace(/-/g, '') : undefined;
  debugger;
  return {
    PID: pinOrPidValue,
    PIN: pinOrPidValue,
    STREET_ADDRESS_1: filter.address,
    latitude: filter.latitude,
    longitude: filter.longitude,
    forceExactMatch: true,
  };
};
