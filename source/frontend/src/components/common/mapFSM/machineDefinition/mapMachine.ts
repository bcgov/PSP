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
        START_REPOSITION: {
          target: 'repositioning',
          actions: [
            assign({
              selectingComponentId: (_, event: any) => event.selectingComponentId,
              repositioningFeatureDataset: (_, event: any) => event.repositioningFeatureDataset,
              repositioningPropertyIndex: (_, event: any) => event.repositioningPropertyIndex,
            }),
          ],
        },
      },
    },
    selecting: {
      on: {
        FINISH_SELECTION: {
          target: 'browsing',
          actions: [assign({ selectingComponentId: () => null })],
        },
        SET_FILE_PROPERTY_LOCATIONS: {
          actions: [
            assign({ filePropertyLocations: (_, event: any) => event.locations }),
            raise('REQUEST_FIT_BOUNDS'),
          ],
        },
      },
    },
    repositioning: {
      on: {
        FINISH_REPOSITION: {
          target: 'browsing',
          actions: [
            assign({
              repositioningFeatureDataset: () => null,
              repositioningPropertyIndex: () => null,
              selectingComponentId: () => null,
            }),
          ],
        },
        SET_FILE_PROPERTY_LOCATIONS: {
          actions: [
            assign({ filePropertyLocations: (_, event: any) => event.locations }),
            raise('REQUEST_FIT_BOUNDS'),
          ],
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
            fitToResultsAfterLoading: () => true,
          }),
          target: 'loading',
        },
      },
    },
    loading: {
      invoke: {
        src: 'loadFeatures',
        onDone: [
          {
            cond: (context: MachineContext) => context.fitToResultsAfterLoading === true,
            actions: [
              assign({
                isLoading: () => false,
                mapFeatureData: (_, event: any) => event.data,
                fitToResultsAfterLoading: () => false,
              }),
              raise('REQUEST_FIT_BOUNDS'),
            ],
            target: 'idle',
          },
          {
            actions: [
              assign({
                isLoading: () => false,
                mapFeatureData: (_, event: any) => event.data,
                fitToResultsAfterLoading: () => false,
              }),
            ],
            target: 'idle',
          },
        ],
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
  initial: 'closed',
  states: {
    closed: {
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
          target: 'opened',
        },
        FINISHED_LOCATION_DATA_LOAD: {
          actions: 'navigateToProperty',
        },
      },
    },
    opened: {
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
          target: 'closed',
        },

        SET_FILE_PROPERTY_LOCATIONS: {
          actions: [
            assign({
              filePropertyLocations: (_: MachineContext, event: any) => event.locations || [],
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
  initial: 'closed',
  states: {
    closed: {
      on: {
        TOGGLE_FILTER: {
          target: 'mapFilterOpened',
        },
        TOGGLE_LAYERS: {
          target: 'layerControl',
        },
      },
    },
    layerControl: {
      on: {
        TOGGLE_FILTER: {
          target: 'mapFilterOpened',
        },
        TOGGLE_LAYERS: {
          target: 'closed',
        },
        SET_MAP_LAYERS: {
          actions: assign({ activeLayers: (_, event: any) => event.activeLayers }),
        },
      },
    },
    mapFilterOpened: {
      entry: [assign({ isFiltering: () => true })],
      on: {
        TOGGLE_FILTER: {
          target: 'closed',
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
        RESET_FILTER: {
          target: 'closed',
          actions: [
            send({ type: 'REFRESH_PROPERTIES' }),
            assign({
              isFiltering: () => false,
              showDisposed: () => false,
              showRetired: () => false,
              activePimsPropertyIds: () => [],
            }),
          ],
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
    repositioningFeatureDataset: null,
    repositioningPropertyIndex: null,
    selectingComponentId: null,
    isLoading: false,
    fitToResultsAfterLoading: false,
    searchCriteria: null,
    mapFeatureData: emptyFeatureData,
    filePropertyLocations: [],
    activePimsPropertyIds: [],
    isFiltering: false,
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
            target: ['mapVisible.sideBar.opened', 'mapVisible.featureDataLoader.loading'],
          },
          {
            target: 'mapVisible.sideBar.opened',
          },
        ],
        OPEN_SIDEBAR: [
          {
            cond: (context: MachineContext) => context.searchCriteria === null,
            actions: assign({ searchCriteria: () => defaultPropertyFilter }),
            target: ['mapVisible.sideBar.opened', 'mapVisible.featureDataLoader.loading'],
          },
          {
            target: 'mapVisible.sideBar.opened',
          },
        ],

        CLOSE_SIDEBAR: {
          target: 'mapVisible.sideBar.closed',
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
        DEFAULT_MAP_LAYERS: {
          actions: assign({ activeLayers: (_, event: any) => event.activeLayers }),
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
