import { latLngBounds } from 'leaflet';
import { assign, createMachine, raise, send } from 'xstate';

import { defaultBounds } from '@/components/maps/constants';
import { defaultPropertyFilter } from '@/features/properties/filter/IPropertyFilter';

import { emptyFeatureData } from '../models';
import { MachineContext, SideBarType } from './types';

const featureViewStates = {
  initial: 'browsing',
  states: {
    browsing: {
      on: {
        START_SELECTION: {
          target: 'selecting',
          actions: [
            assign({ selectingComponentId: (_, event: any) => event.selectingComponentId }),
          ],
        },
        TOGGLE_FILTER: {
          target: 'filtering',
        },
        TOGGLE_LAYERS: {
          target: 'layerControl',
        },
        DEFAULT_MAP_LAYERS: {
          actions: assign({ activeLayers: (_, event: any) => event.activeLayers }),
        },
      },
    },
    selecting: {
      on: {
        FINISH_SELECTION: { target: 'browsing' },
        SET_FILE_PROPERTY_LOCATIONS: {
          actions: [
            assign({ filePropertyLocations: (_, event: any) => event.locations }),
            raise('REQUEST_FIT_BOUNDS'),
          ],
        },
      },
    },
    layerControl: {
      on: {
        TOGGLE_FILTER: {
          target: 'filtering',
        },
        TOGGLE_LAYERS: {
          target: 'browsing',
        },
        SET_MAP_LAYERS: {
          actions: assign({ activeLayers: (_, event: any) => event.activeLayers }),
        },
      },
    },
    filtering: {
      entry: [send({ type: 'REFRESH_PROPERTIES', searchCriteria: defaultPropertyFilter })],
      exit: [send({ type: 'REFRESH_PROPERTIES' })],
      on: {
        TOGGLE_FILTER: {
          target: 'browsing',
          actions: [assign({ showDisposed: () => false }), assign({ showRetired: () => false })],
        },
        TOGGLE_LAYERS: {
          target: 'layerControl',
        },
        SET_VISIBLE_PROPERTIES: {
          actions: assign({ activePimsPropertyIds: (_, event: any) => event.propertyIds }),
        },
        SET_SHOW_DISPOSED: {
          actions: assign({ showDisposed: (_, event: any) => event.show }),
        },
        SET_SHOW_RETIRED: {
          actions: assign({ showRetired: (_, event: any) => event.show }),
        },
      },
    },
  },
};

const featureDataLoaderStates = {
  initial: 'idle',
  states: {
    idle: {
      on: {
        REFRESH_PROPERTIES: {
          actions: assign({ isLoading: () => true }),
          target: 'loading',
        },
        SET_MAP_SEARCH_CRITERIA: {
          actions: assign({
            isLoading: () => true,
            searchCriteria: (_, event: any) => event.searchCriteria,
          }),
          target: 'loading',
        },
      },
    },
    loading: {
      invoke: {
        src: 'loadFeatures',
        onDone: {
          target: 'idle',
          actions: [
            assign({
              isLoading: () => false,
              mapFeatureData: (_, event: any) => event.data,
            }),
            raise('REQUEST_FIT_BOUNDS'),
          ],
        },
      },
    },
  },
};

const mapRequestStates = {
  initial: 'nothingPending',
  states: {
    nothingPending: {
      on: {
        REQUEST_FLY_TO_LOCATION: {
          actions: assign({
            requestedFlyTo: (_, event: any) => ({
              bounds: null,
              location: event.latlng,
            }),
          }),
          target: 'pendingFlyTo',
        },
        REQUEST_FLY_TO_BOUNDS: {
          actions: assign({
            requestedFlyTo: (_, event: any) => ({
              bounds: event.bounds,
              location: null,
            }),
          }),
          target: 'pendingFlyTo',
        },
        REQUEST_FIT_BOUNDS: {
          actions: assign({
            requestedFitBounds: (context: MachineContext) => {
              if (context.filePropertyLocations.length > 0) {
                return latLngBounds(context.filePropertyLocations);
              } else {
                return defaultBounds;
              }
            },
          }),
          target: 'pendingFitBounds',
        },
      },
    },
    pendingFlyTo: {
      on: {
        PROCESS_FLY_TO: {
          actions: assign({
            requestedFlyTo: () => ({
              bounds: null,
              location: null,
            }),
          }),
          target: 'nothingPending',
        },
      },
    },
    pendingFitBounds: {
      on: {
        PROCESS_FIT_BOUNDS: {
          actions: assign({
            requestedFitBounds: () => defaultBounds,
          }),
          target: 'nothingPending',
        },
      },
    },
  },
};

const selectedFeatureLoaderStates = {
  initial: 'idle',
  states: {
    idle: {
      on: {
        MAP_CLICK: {
          actions: [
            assign({
              isLoading: () => true,
              mapLocationSelected: (_, event: any) => event.latlng,
              mapFeatureSelected: () => null,
              mapLocationFeatureDataset: () => null,
            }),
          ],
          target: 'loading',
        },
        MAP_MARKER_CLICK: {
          actions: [
            assign({
              isLoading: () => true,
              mapLocationSelected: () => null,
              mapFeatureSelected: (_, event: any) => event.featureSelected,
              mapLocationFeatureDataset: () => null,
            }),
          ],
          target: 'loading',
        },
        CLOSE_POPUP: {
          actions: [
            assign({
              mapLocationSelected: () => null,
              mapFeatureSelected: () => null,
              mapLocationFeatureDataset: () => null,
            }),
          ],
        },
      },
    },
    loading: {
      invoke: {
        src: 'loadLocationData',
        onDone: {
          target: 'idle',
          actions: [
            assign({
              isLoading: () => false,
              showPopup: () => true,
              mapLocationFeatureDataset: (context: any, event: any) => {
                return {
                  ...event.data,
                  selectingComponentId: context.selectingComponentId,
                };
              },
            }),
            raise('FINISHED_LOCATION_DATA_LOAD'),
          ],
        },
        onError: {
          target: 'idle',
          actions: assign({
            isLoading: () => false,
          }),
        },
      },
    },
  },
};

const sideBarStates = {
  initial: 'fullScreen',
  states: {
    fullScreen: {
      entry: assign({
        mapSideBarState: () => ({
          type: SideBarType.NOT_DEFINED,
          isOpen: false,
          isCollapsed: false,
          isFullWidth: false,
        }),
        filePropertyLocations: () => [],
      }),
      on: {
        OPEN_SIDEBAR: {
          target: 'sidebarOpen',
        },
        FINISHED_LOCATION_DATA_LOAD: {
          actions: 'navigateToProperty',
        },
      },
    },
    sidebarOpen: {
      entry: [
        assign({
          mapSideBarState: (context: MachineContext, event: any) => ({
            ...context.mapSideBarState,
            type: event ? event.sidebarType : context.mapSideBarState.type,
            isOpen: true,
          }),
        }),
      ],
      on: {
        OPEN_SIDEBAR: {
          actions: [
            assign({
              mapSideBarState: (context: MachineContext, event: any) => ({
                ...context.mapSideBarState,
                type: event ? event.sidebarType : context.mapSideBarState.type,
                isOpen: true,
              }),
            }),
          ],
          target: '#map.mapVisible.featureView.browsing',
        },
        CLOSE_SIDEBAR: {
          actions: assign({
            selectedFeatureDataset: () => null,
            mapSideBarState: () => ({
              type: SideBarType.NOT_DEFINED,
              isOpen: false,
              isCollapsed: false,
              isFullWidth: false,
            }),
          }),
          target: 'fullScreen',
        },

        SET_FILE_PROPERTY_LOCATIONS: {
          actions: [
            assign({
              filePropertyLocations: (context: MachineContext, event: any) => event.locations || [],
            }),
            raise('REQUEST_FIT_BOUNDS'),
          ],
        },

        TOGGLE_SIDEBAR_SIZE: {
          actions: [
            assign({
              mapSideBarState: (context: MachineContext) => ({
                ...context.mapSideBarState,
                isCollapsed: !context.mapSideBarState.isCollapsed,
              }),
            }),
          ],
        },

        SET_FULL_WIDTH_SIDEBAR: {
          actions: [
            assign({
              mapSideBarState: (context: MachineContext, event: any) => ({
                ...context.mapSideBarState,
                isFullWidth: event.show,
              }),
            }),
          ],
        },
      },
    },
  },
};

const advancedFilterSideBarStates = {
  initial: 'fullScreen',
  states: {
    fullScreen: {
      on: {
        OPEN_ADVANCED_FILTER_SIDEBAR: {
          target: 'sidebarOpen',
        },
      },
    },
    sidebarOpen: {
      entry: assign({ mapFilter: () => defaultPropertyFilter }),
      on: {
        CLOSE_ADVANCED_FILTER_SIDEBAR: {
          target: 'fullScreen',
        },
      },
    },
  },
};

export const mapMachine = createMachine<MachineContext>({
  // Machine identifier
  id: 'map',
  initial: 'notMap',
  predictableActionArguments: true,

  // Local context for entire machine
  context: {
    mapSideBarState: {
      type: SideBarType.NOT_DEFINED,
      isOpen: false,
      isCollapsed: false,
      isFullWidth: false,
    },
    requestedFlyTo: {
      location: null,
      bounds: null,
    },
    requestedFitBounds: defaultBounds,
    mapLocationSelected: null,
    mapFeatureSelected: null,
    mapLocationFeatureDataset: null,
    selectedFeatureDataset: null,
    selectingComponentId: null,
    isLoading: false,
    searchCriteria: null,
    mapFeatureData: emptyFeatureData,
    filePropertyLocations: [],
    activePimsPropertyIds: [],
    showDisposed: false,
    showRetired: false,
    activeLayers: [],
  },

  // State definitions
  states: {
    notMap: {
      on: {
        ENTER_MAP: [
          {
            cond: (_, event: any) => event.type.sidebarType === SideBarType.NOT_DEFINED,
            target: 'mapVisible',
          },
          {
            cond: (context: MachineContext) => context.searchCriteria === null,
            actions: assign({ searchCriteria: () => defaultPropertyFilter }),
            target: ['mapVisible.sideBar.sidebarOpen', 'mapVisible.featureDataLoader.loading'],
          },
          {
            target: 'mapVisible.sideBar.sidebarOpen',
          },
        ],
        OPEN_SIDEBAR: [
          {
            cond: (context: MachineContext) => context.searchCriteria === null,
            actions: assign({ searchCriteria: () => defaultPropertyFilter }),
            target: ['mapVisible.sideBar.sidebarOpen', 'mapVisible.featureDataLoader.loading'],
          },
          {
            target: 'mapVisible.sideBar.sidebarOpen',
          },
        ],

        CLOSE_SIDEBAR: {
          target: 'mapVisible.sideBar.fullScreen',
        },
      },
    },
    mapVisible: {
      type: 'parallel',
      entry: [send({ type: 'REFRESH_PROPERTIES', searchCriteria: defaultPropertyFilter })],
      on: {
        EXIT_MAP: {
          target: 'notMap',
        },

        PREPARE_FOR_CREATION: {
          actions: assign({
            selectedFeatureDataset: (context: MachineContext) => context.mapLocationFeatureDataset,
          }),
        },
      },
      states: {
        featureView: featureViewStates,
        featureDataLoader: featureDataLoaderStates,
        mapRequest: mapRequestStates,
        selectedFeatureLoader: selectedFeatureLoaderStates,
        sideBar: sideBarStates,
        advancedFilterSideBar: advancedFilterSideBarStates,
      },
    },
  },
});
