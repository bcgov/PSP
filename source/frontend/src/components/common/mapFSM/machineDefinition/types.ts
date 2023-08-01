import { LatLngBounds, LatLngLiteral } from 'leaflet';

import { IPropertyFilter } from '@/features/properties/filter/IPropertyFilter';

import { FeatureSelected, MapFeatureData, RequestedFlyTo } from '../models';
import { LocationFeatureDataset } from '../useLocationFeatureLoader';

export enum SideBarType {
  NOT_DEFINED = 'NOT_DEFINED',
  MAP = 'MAP',
  RESEARCH_FILE = 'RESEARCH_FILE',
  ACQUISITION_FILE = 'ACQUISITION_FILE',
  LEASE_FILE = 'LEASE_FILE',
  PROJECT = 'PROJECT',
  PROPERTY_INFORMATION = 'PROPERTY_INFORMATION',
}

// Local context for the machine - Not related to React Context!
export type MachineContext = {
  mapFeatureSelected: FeatureSelected | null;
  mapLocationSelected: LatLngLiteral | null;
  mapLocationFeatureDataset: LocationFeatureDataset | null;
  selectedFeatureDataset: LocationFeatureDataset | null;

  mapFeatureData: MapFeatureData;

  // TODO: this is partially in the URL. Either move it completly there or remove it
  searchCriteria: IPropertyFilter | null;

  isLoading: boolean;
  sideBarType: SideBarType;
  requestedFitBounds: LatLngBounds;
  requestedFlyTo: RequestedFlyTo;
  filePropertyLocations: LatLngLiteral[];
  activePimsPropertyIds: number[];
};

// Possible state machine states
// TODO:Use types for the different states
/*type Schema =
  | { value: States.NOT_MAP; context: MachineContext }
  | { value: States.BROWSING_MAP; context: MachineContext }
  | { value: States.BROWSING_MAP_WITH_SIDEBAR; context: MachineContext }
  | { value: States.BROWSING_FULL_SIDEBAR; context: MachineContext }
  | { value: States.SELECTING_ON_MAP; context: MachineContext };

  // Possible state machine transitions (i.e. events)
type MachineEvents =
| { type: 'LOGIN' }
| { type: 'LOGIN_ERROR' }
| { type: 'LOGIN_SUCCESS' }
| { type: 'LOGOUT' }
| { type: 'ENTER_MAP'; sideBarType2: SideBarType }
| { type: 'OPEN_SIDEBAR' }
| { type: 'CLOSE_SIDEBAR' }
| { type: 'EXPAND_SIDEBAR' }
| { type: 'SHRINK_SIDEBAR' }
| { type: 'SELECT_ON_MAP' }
| { type: 'SELECT_ON_MAP_SUCCESS' };*/
