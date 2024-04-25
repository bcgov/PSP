import { useInterpret, useSelector } from '@xstate/react';
import { LatLngBounds, LatLngLiteral } from 'leaflet';
import React, { useCallback, useMemo } from 'react';
import { useHistory } from 'react-router-dom';

import { ILayerItem } from '@/components/maps/leaflet/Control/LayersControl/types';
import { IGeoSearchParams } from '@/constants/API';
import {
  defaultPropertyFilter,
  IPropertyFilter,
} from '@/features/properties/filter/IPropertyFilter';
import { pidParser } from '@/utils/propertyUtils';

import { mapMachine } from './machineDefinition/mapMachine';
import { MachineContext, SideBarType } from './machineDefinition/types';
import { FeatureSelected, MapFeatureData, RequestedFlyTo } from './models';
import useLocationFeatureLoader, { LocationFeatureDataset } from './useLocationFeatureLoader';
import { useMapSearch } from './useMapSearch';

export interface IMapStateMachineContext {
  isSidebarOpen: boolean;
  isShowingSearchBar: boolean;
  pendingFlyTo: boolean;
  requestedFlyTo: RequestedFlyTo;
  mapFeatureSelected: FeatureSelected | null;
  mapLocationSelected: LatLngLiteral | null;
  mapLocationFeatureDataset: LocationFeatureDataset | null;
  selectedFeatureDataset: LocationFeatureDataset | null;
  showPopup: boolean;
  isLoading: boolean;
  mapSearchCriteria: IPropertyFilter | null;
  mapFeatureData: MapFeatureData;
  filePropertyLocations: LatLngLiteral[];
  pendingFitBounds: boolean;
  requestedFitBounds: LatLngBounds;
  isSelecting: boolean;
  selectingComponentId: string | null;
  isFiltering: boolean;
  isShowingMapLayers: boolean;
  activePimsPropertyIds: number[];
  showDisposed: boolean;
  showRetired: boolean;
  activeLayers: ILayerItem[];

  requestFlyToLocation: (latlng: LatLngLiteral) => void;
  requestFlyToBounds: (bounds: LatLngBounds) => void;
  processFlyTo: () => void;
  processFitBounds: () => void;
  openSidebar: (sidebarType: SideBarType) => void;
  closeSidebar: () => void;
  closePopup: () => void;

  mapClick: (latlng: LatLngLiteral) => void;
  mapMarkerClick: (featureSelected: FeatureSelected) => void;

  setMapSearchCriteria: (searchCriteria: IPropertyFilter) => void;
  refreshMapProperties: () => void;
  prepareForCreation: () => void;
  startSelection: (selectingComponentId?: string) => void;
  finishSelection: () => void;
  toggleMapFilter: () => void;
  toggleMapLayer: () => void;
  setFilePropertyLocations: (locations: LatLngLiteral[]) => void;
  setMapLayers: (layers: ILayerItem[]) => void;

  setVisiblePimsProperties: (propertyIds: number[]) => void;
  setShowDisposed: (show: boolean) => void;
  setShowRetired: (show: boolean) => void;
  changeSidebar: () => void;
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
  const locationLoader = useLocationFeatureLoader();
  const mapSearch = useMapSearch();
  const history = useHistory();

  const service = useInterpret(mapMachine, {
    actions: {
      navigateToProperty: context => {
        const selectedFeatureData = context.mapLocationFeatureDataset;
        if (selectedFeatureData?.pimsFeature?.properties?.PROPERTY_ID) {
          const pimsFeature = selectedFeatureData.pimsFeature;
          history.push(`/mapview/sidebar/property/${pimsFeature.properties.PROPERTY_ID}`);
        } else if (selectedFeatureData?.parcelFeature?.properties?.PID) {
          const parcelFeature = selectedFeatureData.parcelFeature;
          const parsedPid = pidParser(parcelFeature.properties.PID);
          history.push(`/mapview/sidebar/non-inventory-property/${parsedPid}`);
        }
      },
    },
    services: {
      loadLocationData: (context: MachineContext, event: any) => {
        const result = locationLoader.loadLocationDetails(
          event.type === 'MAP_CLICK' ? event.latlng : event.featureSelected.latlng,
        );
        if (event.type === 'MAP_MARKER_CLICK') {
          // In the case of the map marker being clicked, always used the clicked marker instead of the search result.
          // TODO: refactor loadLocationDetails method to allow for optional loading of various feature types.
          result.then(data => {
            data.pimsFeature = {
              properties: event.featureSelected?.pimsLocationFeature,
              type: 'Feature',
              geometry: {
                type: 'Point',
                coordinates: [event.featureSelected.latlng.lng, event.featureSelected.latlng.lat],
              },
            };
          });
        }

        return result;
      },
      loadFeatures: (context: MachineContext, event: any) => {
        // If there is data in the event, use that criteria.
        // Otherwise, use the stored one in the context.
        let searchCriteria = context.searchCriteria || defaultPropertyFilter;
        if (event.searchCriteria !== undefined) {
          searchCriteria = event.searchCriteria;
        }

        const geoFilter = getQueryParams(searchCriteria);

        if (geoFilter?.PID || geoFilter?.PID_PADDED || geoFilter?.PIN) {
          return mapSearch.searchMany(geoFilter);
        } else if (geoFilter?.latitude && geoFilter?.longitude) {
          const geoLat = Number(geoFilter.latitude);
          const geoLng = Number(geoFilter.longitude);
          return mapSearch.searchOneLocation(geoLat, geoLng);
        } else if (geoFilter?.SURVEY_PLAN_NUMBER && !geoFilter?.PID && !geoFilter?.PIN) {
          return mapSearch.searchByPlanNumber(geoFilter);
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

  const setMapSearchCriteria = useCallback(
    (searchCriteria: IPropertyFilter) => {
      serviceSend({ type: 'SET_MAP_SEARCH_CRITERIA', searchCriteria });
    },
    [serviceSend],
  );

  const prepareForCreation = useCallback(() => {
    serviceSend({ type: 'PREPARE_FOR_CREATION' });
  }, [serviceSend]);

  const startSelection = useCallback(
    (selectingComponentId?: string) => {
      serviceSend({ type: 'START_SELECTION', selectingComponentId });
    },
    [serviceSend],
  );

  const finishSelection = useCallback(() => {
    serviceSend({ type: 'FINISH_SELECTION' });
  }, [serviceSend]);

  const changeSidebar = useCallback(() => {
    serviceSend({ type: 'CHANGE_SIDEBAR' });
  }, [serviceSend]);

  const setFilePropertyLocations = useCallback(
    (locations: LatLngLiteral[]) => {
      serviceSend({ type: 'SET_FILE_PROPERTY_LOCATIONS', locations });
    },
    [serviceSend],
  );

  const setMapLayers = useCallback(
    (activeLayers: ILayerItem[]) => {
      serviceSend({ type: 'SET_MAP_LAYERS', activeLayers });
    },
    [serviceSend],
  );

  const setVisiblePimsProperties = useCallback(
    (propertyIds: number[]) => {
      serviceSend({ type: 'SET_VISIBLE_PROPERTIES', propertyIds });
    },
    [serviceSend],
  );

  const setShowDisposed = useCallback(
    (show: boolean) => {
      serviceSend({ type: 'SET_SHOW_DISPOSED', show });
    },
    [serviceSend],
  );

  const setShowRetired = useCallback(
    (show: boolean) => {
      serviceSend({ type: 'SET_SHOW_RETIRED', show });
    },
    [serviceSend],
  );

  const toggleMapFilter = useCallback(() => {
    serviceSend({ type: 'TOGGLE_FILTER' });
  }, [serviceSend]);

  const toggleMapLayer = useCallback(() => {
    serviceSend({ type: 'TOGGLE_LAYERS' });
  }, [serviceSend]);

  const showPopup = useMemo(() => {
    return state.context.mapLocationFeatureDataset !== null;
  }, [state.context.mapLocationFeatureDataset]);

  const isSidebarOpen = useMemo(() => {
    return [
      { mapVisible: { sideBar: 'sidebarOpen' } },
      { mapVisible: { sideBar: 'selecting' } },
    ].some(state.matches);
  }, [state.matches]);

  const isFiltering = useMemo(() => {
    return state.matches({ mapVisible: { featureView: 'filtering' } });
  }, [state]);

  const isShowingMapLayers = useMemo(() => {
    return state.matches({ mapVisible: { featureView: 'layerControl' } });
  }, [state]);

  return (
    <MapStateMachineContext.Provider
      value={{
        isSidebarOpen: isSidebarOpen,
        isShowingSearchBar: !isSidebarOpen && !isFiltering,
        pendingFlyTo: state.matches({ mapVisible: { mapRequest: 'pendingFlyTo' } }),
        requestedFlyTo: state.context.requestedFlyTo,
        mapFeatureSelected: state.context.mapFeatureSelected,
        mapLocationSelected: state.context.mapLocationSelected,
        mapLocationFeatureDataset: state.context.mapLocationFeatureDataset,
        selectedFeatureDataset: state.context.selectedFeatureDataset,
        showPopup: showPopup,
        isLoading: state.context.isLoading,
        mapSearchCriteria: state.context.searchCriteria,
        mapFeatureData: state.context.mapFeatureData,
        filePropertyLocations: state.context.filePropertyLocations,
        pendingFitBounds: state.matches({ mapVisible: { mapRequest: 'pendingFitBounds' } }),
        requestedFitBounds: state.context.requestedFitBounds,
        isSelecting: state.matches({ mapVisible: { featureView: 'selecting' } }),
        selectingComponentId: state.context.selectingComponentId,
        isFiltering: isFiltering,
        isShowingMapLayers: isShowingMapLayers,
        activeLayers: state.context.activeLayers,
        activePimsPropertyIds: state.context.activePimsPropertyIds,
        showDisposed: state.context.showDisposed,
        showRetired: state.context.showRetired,

        setMapSearchCriteria,
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
        toggleMapFilter,
        toggleMapLayer,
        setFilePropertyLocations,
        setVisiblePimsProperties,
        setShowDisposed,
        setShowRetired,
        setMapLayers,
        changeSidebar,
      }}
    >
      {children}
    </MapStateMachineContext.Provider>
  );
};

const getQueryParams = (filter: IPropertyFilter): IGeoSearchParams => {
  // The map will search for either identifier.
  const pinOrPidValue = filter.pinOrPid ? filter.pinOrPid?.replace(/-/g, '') : undefined;
  return {
    PID_PADDED: pinOrPidValue,
    PID: pinOrPidValue,
    PIN: pinOrPidValue,
    STREET_ADDRESS_1: filter.address,
    SURVEY_PLAN_NUMBER: filter.planNumber,
    latitude: filter.latitude,
    longitude: filter.longitude,
    forceExactMatch: true,
  };
};
