import { createMachine } from 'xstate';

import { MachineContext, Schema, States, Transition } from './mapMachine.types';

export const mapMachine = createMachine<MachineContext, Transition, Schema>(
  {
    // Machine identifier
    id: 'map',
    initial: States.LOGGED_OUT,

    // Local context for entire machine
    context: {
      popup: {},
      sidebar: {},
      map: {
        selectedFeature: undefined,
      },
    },

    // State definitions
    states: {
      [States.LOGGED_OUT]: {
        on: {
          LOGIN: { target: States.LOGGING_IN },
        },
      },
      [States.LOGGING_IN]: {
        on: {
          LOGIN_ERROR: { target: States.LOGGED_OUT },
          LOGIN_SUCCESS: { target: States.BROWSING_MAP },
        },
      },
      [States.BROWSING_MAP]: {
        on: {
          LOGOUT: { target: States.LOGGED_OUT },
          OPEN_SIDEBAR: { target: States.BROWSING_MAP_WITH_SIDEBAR },
        },
      },
      [States.BROWSING_MAP_WITH_SIDEBAR]: {
        on: {
          CLOSE_SIDEBAR: { target: States.BROWSING_MAP },
          EXPAND_SIDEBAR: { target: States.BROWSING_FULL_SIDEBAR },
          SELECT_ON_MAP: { target: States.SELECTING_ON_MAP },
        },
      },
      [States.BROWSING_FULL_SIDEBAR]: {
        on: {
          SHRINK_SIDEBAR: { target: States.BROWSING_MAP_WITH_SIDEBAR },
          CLOSE_SIDEBAR: { target: States.BROWSING_MAP },
        },
      },
      [States.SELECTING_ON_MAP]: {
        on: {
          SELECT_ON_MAP_SUCCESS: { target: States.BROWSING_MAP_WITH_SIDEBAR },
        },
      },
    },
  },
  {},
);
