import { LatLngBounds, LatLngLiteral } from 'leaflet';

import { PropertyFilterFormModel } from '@/components/maps/leaflet/Control/AdvancedFilter/models';
import { IMapSideBarViewState as IMapSideBarState } from '@/features/mapSideBar/MapSideBar';
import { IPropertyFilter } from '@/features/properties/filter/IPropertyFilter';

import {
  LocationBoundaryDataset,
  MapFeatureData,
  MarkerSelected,
  RequestedCenterTo,
  RequestedFlyTo,
} from '../models';
import {
  LocationFeatureDataset,
  SelectedFeatureDataset,
  WorklistLocationFeatureDataset,
} from '../useLocationFeatureLoader';

export enum SideBarType {
  NOT_DEFINED = 'NOT_DEFINED',
  MAP = 'MAP',
  RESEARCH_FILE = 'RESEARCH_FILE',
  ACQUISITION_FILE = 'ACQUISITION_FILE',
  LEASE_FILE = 'LEASE_FILE',
  PROJECT = 'PROJECT',
  PROPERTY_INFORMATION = 'PROPERTY_INFORMATION',
  DISPOSITION = 'DISPOSITION',
  SUBDIVISION = 'SUBDIVISION',
  CONSOLIDATION = 'CONSOLIDATION',
}

// Local context for the machine - Not related to React Context!
export type MachineContext = {
  mapSideBarState: IMapSideBarState | null;
  mapFeatureSelected: MarkerSelected | null;
  mapLocationSelected: LatLngLiteral | null;
  mapLocationFeatureDataset: LocationFeatureDataset | null;
  mapMarkedLocation: LatLngLiteral | null;
  selectedFeatures: SelectedFeatureDataset[];
  repositioningFeatureDataset: SelectedFeatureDataset | null;
  repositioningPropertyIndex: number | null;
  selectingComponentId: string | null;

  mapFeatureData: MapFeatureData;

  // worklist-related state
  worklistSelectedMapLocation: LatLngLiteral | null;
  worklistLocationFeatureDataset: WorklistLocationFeatureDataset | null;

  // TODO: this is partially in the URL. Either move it completly there or remove it
  searchCriteria: IPropertyFilter | null;
  advancedSearchCriteria: PropertyFilterFormModel | null;

  isLoading: boolean;
  requestedFitBounds: LatLngBounds;
  requestedFlyTo: RequestedFlyTo;
  requestedCenterTo: RequestedCenterTo;
  filePropertyLocations: LocationBoundaryDataset[];
  activePimsPropertyIds: number[];
  activeLayers: Set<string>;
  mapLayersToRefresh: Set<string>;
  isFiltering: boolean;
  showDisposed: boolean;
  showRetired: boolean;
  currentMapBounds: LatLngBounds | null;
  isEditPropertiesMode: boolean;
};
