import { latLngBounds } from 'leaflet';
import { assign, createMachine, raise } from 'xstate';

import { defaultBounds } from '@/components/maps/constants';
import { defaultPropertyFilter } from '@/features/properties/filter/IPropertyFilter';

import { emptyFeatureData } from '../models';
import { MachineContext, SideBarType } from './types';

const featureDataStates = {
  initial: 'idle',
  states: {
    idle: {
      on: {
        REFRESH_PROPERTIES: {
          actions: assign({ isLoading: () => true }),
          target: 'loading',
        },
        SET_MAP_FILTER: {
          actions: assign({ isLoading: () => true, mapFilter: (_, event: any) => event.mapFilter }),
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
          actions: assign((context: any) => {
            if (context.draftLocations.length > 0) {
              context.requestedFitBounds = latLngBounds(context.draftLocations);
            } else {
              context.requestedFitBounds = defaultBounds;
            }
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

const selectedFeatureStates = {
  initial: 'idle',
  states: {
    idle: {
      on: {
        MAP_CLICK: {
          actions: [
            assign({
              isLoading: () => true,
              mapLocationSelected: (_, event: any) => event.latlng,
              mapFeatureSelected: (_, event: any) => null,
              mapLocationFeatureDataset: () => null,
            }),
          ],
          target: 'loading',
        },
        MAP_MARKER_CLICK: {
          actions: [
            assign({
              isLoading: () => true,
              mapLocationSelected: (_, event: any) => null,
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
              mapLocationFeatureDataset: (context, event: any) => event.data,
            }),
            // 'navigateToProperty', // TODO:uncomment this to show property information sidebar
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
        sideBarType: () => SideBarType.NOT_DEFINED,
        draftLocations: () => [],
      }),
      on: {
        OPEN_SIDEBAR: {
          actions: assign({
            sideBarType: (_, event: any) => event.sidebarType,
          }),
          target: 'sidebarOpen',
        },
      },
    },
    sidebarOpen: {
      entry: assign({
        sideBarType: (_, event: any) => event.sidebarType,
      }),
      on: {
        CLOSE_SIDEBAR: {
          target: 'fullScreen',
          actions: assign({ selectedFeatureDataset: () => null }),
        },
        START_SELECTION: {
          target: 'selecting',
        },
        SET_DRAFT_LOCATIONS: {
          actions: [
            assign((context: any, event: any) => {
              context.draftLocations = event.locations || [];
            }),
            raise('REQUEST_FIT_BOUNDS'),
          ],
        },
      },
    },
    selecting: {
      on: {
        FINISH_SELECTION: { target: 'sidebarOpen' },
        SET_DRAFT_LOCATIONS: {
          actions: [
            assign({ draftLocations: (_, event: any) => event.locations }),
            raise('REQUEST_FIT_BOUNDS'),
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

  // Local context for entire machine
  context: {
    isSelecting: false,
    sideBarType: SideBarType.NOT_DEFINED,
    requestedFlyTo: {
      location: null,
      bounds: null,
    },
    requestedFitBounds: defaultBounds,
    mapLocationSelected: null,
    mapFeatureSelected: null,
    mapLocationFeatureDataset: null,
    selectedFeatureDataset: null,
    isLoading: false,
    mapFilter: null,
    mapFeatureData: emptyFeatureData,
    draftLocations: [],
  },

  // State definitions
  states: {
    notMap: {
      on: {
        ENTER_MAP: [
          {
            cond: (context, event: any) => event.type.sidebarType === SideBarType.NOT_DEFINED,
            target: 'mapVisible',
          },
          {
            cond: (context, event: any) => context.mapFilter === null,
            actions: assign({ mapFilter: () => defaultPropertyFilter }),
            target: ['mapVisible.sideBar.sidebarOpen', 'mapVisible.featureData.loading'],
          },
          {
            target: 'mapVisible.sideBar.sidebarOpen',
          },
        ],
        OPEN_SIDEBAR: [
          {
            cond: (context, event: any) => context.mapFilter === null,
            actions: assign({ mapFilter: () => defaultPropertyFilter }),
            target: ['mapVisible.sideBar.sidebarOpen', 'mapVisible.featureData.loading'],
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
      on: {
        EXIT_MAP: {
          target: 'notMap',
        },

        PREPARE_FOR_CREATION: {
          actions: assign({
            selectedFeatureDataset: context => context.mapLocationFeatureDataset,
          }),
        },
      },
      states: {
        featureData: featureDataStates,
        mapRequest: mapRequestStates,
        selectedFeature: selectedFeatureStates,
        sideBar: sideBarStates,
      },
    },
  },
});
