import { useInterpret, useSelector } from '@xstate/react';
import { dequal } from 'dequal';
import { LatLngBounds, LatLngLiteral } from 'leaflet';
import React, { useCallback, useMemo } from 'react';
import { useHistory } from 'react-router-dom';
import { AnyEventObject } from 'xstate';

import { PropertyFilterFormModel } from '@/components/maps/leaflet/Control/AdvancedFilter/models';
import { ILayerItem } from '@/components/maps/leaflet/Control/LayersControl/types';
import { IGeoSearchParams } from '@/constants/API';
import { IMapSideBarViewState } from '@/features/mapSideBar/MapSideBar';
import {
  defaultPropertyFilter,
  IPropertyFilter,
} from '@/features/properties/filter/IPropertyFilter';
import { exists } from '@/utils';
import { pidParser, pinParser } from '@/utils/propertyUtils';

import { mapMachine } from './machineDefinition/mapMachine';
import { MachineContext, SideBarType } from './machineDefinition/types';
import { FeatureSelected, MapFeatureData, RequestedCenterTo, RequestedFlyTo } from './models';
import useLocationFeatureLoader, { LocationFeatureDataset } from './useLocationFeatureLoader';
import { useMapSearch } from './useMapSearch';

export interface IMapStateMachineContext {
  mapSideBarViewState: IMapSideBarViewState;
  isShowingSearchBar: boolean;
  pendingFlyTo: boolean;
  pendingCenterTo: boolean;
  requestedFlyTo: RequestedFlyTo;
  requestedCenterTo: RequestedCenterTo;
  mapFeatureSelected: FeatureSelected | null;
  mapLocationSelected: LatLngLiteral | null;
  mapLocationFeatureDataset: LocationFeatureDataset | null;
  selectedFeatureDataset: LocationFeatureDataset | null;
  repositioningFeatureDataset: LocationFeatureDataset | null;
  repositioningPropertyIndex: number | null;
  showPopup: boolean;
  isLoading: boolean;
  mapSearchCriteria: IPropertyFilter | null;
  mapFeatureData: MapFeatureData;
  filePropertyLocations: LatLngLiteral[];
  pendingFitBounds: boolean;
  requestedFitBounds: LatLngBounds;
  isSelecting: boolean;
  isRepositioning: boolean;
  selectingComponentId: string | null;
  isFiltering: boolean;
  isShowingMapFilter: boolean;
  isShowingMapLayers: boolean;
  activePimsPropertyIds: number[];
  showDisposed: boolean;
  showRetired: boolean;
  activeLayers: ILayerItem[];
  mapLayersToRefresh: ILayerItem[];
  advancedSearchCriteria: PropertyFilterFormModel;
  isMapVisible: boolean;
  currentMapBounds: LatLngBounds;

  requestFlyToLocation: (latlng: LatLngLiteral) => void;
  requestCenterToLocation: (latlng: LatLngLiteral) => void;
  requestFlyToBounds: (bounds: LatLngBounds) => void;
  processFlyTo: () => void;
  processCenterTo: () => void;
  processFitBounds: () => void;
  openSidebar: (sidebarType: SideBarType) => void;
  closeSidebar: () => void;
  toggleSidebarDisplay: () => void;
  closePopup: () => void;

  mapClick: (latlng: LatLngLiteral) => void;
  mapMarkerClick: (featureSelected: FeatureSelected) => void;

  setMapSearchCriteria: (searchCriteria: IPropertyFilter) => void;
  refreshMapProperties: () => void;
  prepareForCreation: () => void;
  startSelection: (selectingComponentId?: string) => void;
  finishSelection: () => void;
  startReposition: (
    repositioningFeatureDataset: LocationFeatureDataset,
    index: number,
    selectingComponentId?: string,
  ) => void;
  finishReposition: () => void;
  toggleMapFilterDisplay: () => void;
  toggleMapLayerControl: () => void;
  setFilePropertyLocations: (locations: LatLngLiteral[]) => void;
  setMapLayers: (layers: ILayerItem[]) => void;
  setMapLayersToRefresh: (layers: ILayerItem[]) => void;
  setDefaultMapLayers: (layers: ILayerItem[]) => void;

  setVisiblePimsProperties: (propertyIds: number[]) => void;
  setShowDisposed: (show: boolean) => void;
  setShowRetired: (show: boolean) => void;
  setFullWidthSideBar: (fullWidth: boolean) => void;
  resetMapFilter: () => void;
  setAdvancedSearchCriteria: (advancedSearchCriteria: PropertyFilterFormModel) => void;
  setCurrentMapBounds: (bounds: LatLngBounds) => void;
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
        if (exists(selectedFeatureData?.pimsFeature?.properties?.PROPERTY_ID)) {
          const pimsFeature = selectedFeatureData.pimsFeature;
          history.push(`/mapview/sidebar/property/${pimsFeature.properties.PROPERTY_ID}`);
        } else if (exists(selectedFeatureData?.parcelFeature?.properties?.PID)) {
          const parcelFeature = selectedFeatureData?.parcelFeature;
          const parsedPid = pidParser(parcelFeature.properties.PID);
          history.push(`/mapview/sidebar/non-inventory-property/pid/${parsedPid}`);
        } else if (exists(selectedFeatureData?.parcelFeature?.properties?.PIN)) {
          const parcelFeature = selectedFeatureData?.parcelFeature;
          const parsedPin = pinParser(parcelFeature.properties.PIN);
          history.push(`/mapview/sidebar/non-inventory-property/pin/${parsedPin}`);
        }
      },
    },
    services: {
      loadLocationData: async (
        context: MachineContext,
        event:
          | (AnyEventObject & { type: 'MAP_CLICK'; latlng: LatLngLiteral })
          | (AnyEventObject & { type: 'MAP_MARKER_CLICK'; featureSelected: FeatureSelected }),
      ) => {
        let result: LocationFeatureDataset | undefined = undefined;

        if (event.type === 'MAP_CLICK') {
          result = await locationLoader.loadLocationDetails({ latLng: event.latlng });
        } else if (event.type === 'MAP_MARKER_CLICK') {
          result = await locationLoader.loadLocationDetails({
            latLng: event.featureSelected.latlng,
            pimsPropertyId: event.featureSelected?.pimsLocationFeature?.PROPERTY_ID ?? null,
          });
          // In the case of the map marker being clicked, we must use the search result properties, as the minimal layer does not have the necessary feature data.
          // However, use the coordinates of the clicked marker.
          if (exists(result.pimsFeature)) {
            result.pimsFeature = {
              properties: { ...result.pimsFeature?.properties },
              type: 'Feature',
              geometry: {
                type: 'Point',
                coordinates: [event.featureSelected.latlng.lng, event.featureSelected.latlng.lat],
              },
            };
          }
        }

        return result;
      },
      loadFeatures: (context: MachineContext, event: any): Promise<MapFeatureData> => {
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
        } else if (geoFilter?.HISTORICAL_FILE_NUMBER_STR) {
          geoFilter.forceExactMatch = false;
          return mapSearch.searchByHistorical(geoFilter);
        } else {
          return mapSearch.loadMapProperties();
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

  const requestCenterToLocation = useCallback(
    (latlng: LatLngLiteral) => {
      serviceSend({
        type: 'REQUEST_CENTER_TO_LOCATION',
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

  const processCenterTo = useCallback(() => {
    serviceSend({
      type: 'PROCESS_CENTER_TO',
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

  const setAdvancedSearchCriteria = useCallback(
    (advancedSearchCriteria: PropertyFilterFormModel) => {
      serviceSend({ type: 'SET_ADVANCED_SEARCH_CRITERIA', advancedSearchCriteria });
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

  const startReposition = useCallback(
    (
      repositioningFeatureDataset: LocationFeatureDataset,
      index: number,
      selectingComponentId?: string,
    ) => {
      serviceSend({
        type: 'START_REPOSITION',
        repositioningFeatureDataset,
        repositioningPropertyIndex: index,
        selectingComponentId,
      });
    },
    [serviceSend],
  );

  const finishReposition = useCallback(() => {
    serviceSend({ type: 'FINISH_REPOSITION' });
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

  const setMapLayersToRefresh = useCallback(
    (refreshLayers: ILayerItem[]) => {
      serviceSend({ type: 'SET_REFRESH_MAP_LAYERS', refreshLayers });
    },
    [serviceSend],
  );

  const setDefaultMapLayers = useCallback(
    (activeLayers: ILayerItem[]) => {
      serviceSend({ type: 'DEFAULT_MAP_LAYERS', activeLayers });
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

  const setFullWidthSideBar = useCallback(
    (show: boolean) => {
      serviceSend({ type: 'SET_FULL_WIDTH_SIDEBAR', show });
    },
    [serviceSend],
  );

  const setCurrentMapBounds = useCallback(
    (currentMapBounds: LatLngBounds) => {
      serviceSend({ type: 'SET_CURRENT_MAP_BOUNDS', currentMapBounds });
    },
    [serviceSend],
  );

  const toggleSidebarDisplay = useCallback(() => {
    serviceSend({ type: 'TOGGLE_SIDEBAR_SIZE' });
  }, [serviceSend]);

  const toggleMapFilterDisplay = useCallback(() => {
    serviceSend({ type: 'TOGGLE_FILTER' });
  }, [serviceSend]);

  const resetMapFilter = useCallback(() => {
    serviceSend({ type: 'RESET_FILTER' });
  }, [serviceSend]);

  const toggleMapLayerControl = useCallback(() => {
    serviceSend({ type: 'TOGGLE_LAYERS' });
  }, [serviceSend]);

  const isRepositioning = useMemo(() => {
    return state.matches({ mapVisible: { featureView: 'repositioning' } });
  }, [state]);

  // disable map popup when repositioning file markers
  const showPopup = useMemo(() => {
    return state.context.mapLocationFeatureDataset !== null && !isRepositioning;
  }, [isRepositioning, state.context.mapLocationFeatureDataset]);

  const isShowingMapFilter = useMemo(() => {
    return state.matches({ mapVisible: { advancedFilterSideBar: 'mapFilterOpened' } });
  }, [state]);

  const isShowingMapLayers = useMemo(() => {
    return state.matches({ mapVisible: { advancedFilterSideBar: 'layerControl' } });
  }, [state]);

  return (
    <MapStateMachineContext.Provider
      value={{
        mapSideBarViewState: state.context.mapSideBarState,
        isShowingSearchBar:
          !state.context.mapSideBarState.isOpen &&
          !(
            isShowingMapFilter ||
            !dequal(state.context.advancedSearchCriteria, new PropertyFilterFormModel())
          ),
        pendingFlyTo: state.matches({ mapVisible: { mapRequest: 'pendingFlyTo' } }),
        pendingCenterTo: state.matches({ mapVisible: { mapRequest: 'pendingCenterTo' } }),
        requestedFlyTo: state.context.requestedFlyTo,
        requestedCenterTo: state.context.requestedCenterTo,
        mapFeatureSelected: state.context.mapFeatureSelected,
        mapLocationSelected: state.context.mapLocationSelected,
        mapLocationFeatureDataset: state.context.mapLocationFeatureDataset,
        selectedFeatureDataset: state.context.selectedFeatureDataset,
        repositioningFeatureDataset: state.context.repositioningFeatureDataset,
        repositioningPropertyIndex: state.context.repositioningPropertyIndex,
        showPopup: showPopup,
        isLoading: state.context.isLoading,
        mapSearchCriteria: state.context.searchCriteria,
        advancedSearchCriteria: state.context.advancedSearchCriteria,
        mapFeatureData: state.context.mapFeatureData,
        filePropertyLocations: state.context.filePropertyLocations,
        pendingFitBounds: state.matches({ mapVisible: { mapRequest: 'pendingFitBounds' } }),
        requestedFitBounds: state.context.requestedFitBounds,
        isSelecting: state.matches({ mapVisible: { featureView: 'selecting' } }),
        isRepositioning: isRepositioning,
        selectingComponentId: state.context.selectingComponentId,
        isFiltering: !dequal(state.context.advancedSearchCriteria, new PropertyFilterFormModel()),
        isShowingMapFilter: isShowingMapFilter,
        isShowingMapLayers: isShowingMapLayers,
        activeLayers: state.context.activeLayers,
        activePimsPropertyIds: state.context.activePimsPropertyIds,
        showDisposed: state.context.showDisposed,
        showRetired: state.context.showRetired,
        mapLayersToRefresh: state.context.mapLayersToRefresh,
        isMapVisible: state.matches({ mapVisible: {} }),
        currentMapBounds: state.context.currentMapBounds,

        setMapSearchCriteria,
        refreshMapProperties,
        processFlyTo,
        processCenterTo,
        processFitBounds,
        openSidebar,
        closeSidebar,
        requestFlyToLocation,
        requestCenterToLocation,
        requestFlyToBounds,
        mapClick,
        mapMarkerClick,
        closePopup,
        prepareForCreation,
        startSelection,
        finishSelection,
        startReposition,
        finishReposition,
        toggleMapFilterDisplay,
        toggleMapLayerControl,
        toggleSidebarDisplay,
        setFilePropertyLocations,
        setVisiblePimsProperties,
        setShowDisposed,
        setShowRetired,
        setMapLayers,
        setMapLayersToRefresh,
        setDefaultMapLayers,
        setFullWidthSideBar,
        resetMapFilter,
        setAdvancedSearchCriteria,
        setCurrentMapBounds,
      }}
    >
      {children}
    </MapStateMachineContext.Provider>
  );
};

const getQueryParams = (filter: IPropertyFilter): IGeoSearchParams => {
  const pidValue = exists(filter.pid) ? filter.pid?.replaceAll(/-/g, '') : undefined;
  const pinValue = exists(filter.pin) ? filter.pin?.replaceAll(/-/g, '') : undefined;

  return {
    PID_PADDED: pidValue,
    PID: pidValue,
    PIN: pinValue,
    STREET_ADDRESS_1: filter.address,
    SURVEY_PLAN_NUMBER: filter.planNumber,
    HISTORICAL_FILE_NUMBER_STR: filter.historical,
    latitude: filter.latitude,
    longitude: filter.longitude,
    forceExactMatch: pidValue?.length === 9,
  };
};
