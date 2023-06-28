import { useInterpret, useMachine, useSelector } from '@xstate/react';
import { useCallback, useEffect, useMemo, useState } from 'react';
import { assign, createMachine, MachineConfig } from 'xstate';

enum States {
  NOT_MAP = 'notMap',
  BROWSING_MAP = 'browsingMap',
  BROWSING_MAP_WITH_SIDEBAR = 'browsingMapWithSidebar',
  BROWSING_FULL_SIDEBAR = 'browsingFullSidebar',
  SELECTING_ON_MAP = 'selectingOnMap',
}

enum SidebarType {
  NO_SIDEBAR,
  RESEARCH,
  ACQUISITION,
}

// Possible state machine transitions (i.e. events)
type MachineEvents =
  | { type: 'LOGIN' }
  | { type: 'LOGIN_ERROR' }
  | { type: 'LOGIN_SUCCESS' }
  | { type: 'LOGOUT' }
  | { type: 'ENTER_MAP'; sideBarType2: SidebarType }
  | { type: 'OPEN_SIDEBAR' }
  | { type: 'CLOSE_SIDEBAR' }
  | { type: 'EXPAND_SIDEBAR' }
  | { type: 'SHRINK_SIDEBAR' }
  | { type: 'SELECT_ON_MAP' }
  | { type: 'SELECT_ON_MAP_SUCCESS' };

// Local context for the machine - Not related to React Context!
type MachineContext = {
  sideBarType: SidebarType;
  isSelecting: boolean;
};

// Possible state machine states
type Schema =
  | { value: States.NOT_MAP; context: MachineContext }
  | { value: States.BROWSING_MAP; context: MachineContext }
  | { value: States.BROWSING_MAP_WITH_SIDEBAR; context: MachineContext }
  | { value: States.BROWSING_FULL_SIDEBAR; context: MachineContext }
  | { value: States.SELECTING_ON_MAP; context: MachineContext };

const sideBarStates = {
  initial: 'fullScreen',
  //context: { sideBarType: 'no_sidebar' },
  states: {
    fullScreen: {
      entry: assign({
        sideBarType: (_, event: any) => 'NONE',
      }),
      on: {
        OPEN_SIDEBAR: {
          actions: assign({
            sideBarType: (_, event: any) => event.sidebarType,
          }),
          target: 'withSidebar',
        },
      },
    },
    withSidebar: {
      entry: assign({
        sideBarType: (_, event: any) => event.sidebarType,
      }),
      on: {
        CLOSE_SIDEBAR: { target: 'fullScreen' },
      },
    },
  },
};

export const mapMachine = createMachine({
  // Machine identifier
  id: 'map',
  initial: 'notMap',

  // Local context for entire machine
  context: {
    isSelecting: false,
    sideBarType: 'NONE',
    requestedFlyTo: null,
  },

  // State definitions
  states: {
    notMap: {
      on: {
        ENTER_MAP: [
          {
            cond: (context, event: any) => event.type.sidebarType === 'NONE',
            target: 'browsinMap.fullScreen',
          },
          {
            target: 'browsinMap.withSidebar',
          },
        ],
        OPEN_SIDEBAR: {
          target: 'browsinMap.withSidebar',
        },

        CLOSE_SIDEBAR: { target: 'browsinMap' },
      },
    },
    browsinMap: {
      on: {
        EXIT_MAP: {
          target: 'notMap',
        },
        REQUEST_FLY_TO: {
          actions: assign({
            requestedFlyTo: (_, event: any) => event.latlng,
          }),
        },
      },

      ...sideBarStates,
    },
  },
});
