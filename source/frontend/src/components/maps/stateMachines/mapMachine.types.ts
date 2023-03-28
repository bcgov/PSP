export enum States {
  LOGGED_OUT = 'loggedOut',
  LOGGING_IN = 'loggingIn',
  BROWSING_MAP = 'browsingMap',
  BROWSING_MAP_WITH_SIDEBAR = 'browsingMapWithSidebar',
  BROWSING_FULL_SIDEBAR = 'browsingFullSidebar',
  SELECTING_ON_MAP = 'selectingOnMap',
}

// Possible state machine transitions (i.e. events)
export type Transition =
  | { type: 'LOGIN' }
  | { type: 'LOGIN_ERROR' }
  | { type: 'LOGIN_SUCCESS' }
  | { type: 'LOGOUT' }
  | { type: 'OPEN_SIDEBAR' }
  | { type: 'CLOSE_SIDEBAR' }
  | { type: 'EXPAND_SIDEBAR' }
  | { type: 'SHRINK_SIDEBAR' }
  | { type: 'SELECT_ON_MAP' }
  | { type: 'SELECT_ON_MAP_SUCCESS' };

// Local context for the machine - Not related to React Context!
export type MachineContext = {
  popup: {};
  sidebar: {};
  map: {
    selectedFeature: any;
  };
};

// Possible state machine states
export type Schema =
  | { value: States.LOGGED_OUT; context: MachineContext }
  | { value: States.LOGGING_IN; context: MachineContext }
  | { value: States.BROWSING_MAP; context: MachineContext }
  | { value: States.BROWSING_MAP_WITH_SIDEBAR; context: MachineContext }
  | { value: States.BROWSING_FULL_SIDEBAR; context: MachineContext }
  | { value: States.SELECTING_ON_MAP; context: MachineContext };
