import { LatLngBounds, LatLngLiteral } from 'leaflet';

import { PropertyFilterFormModel } from '@/components/maps/leaflet/Control/AdvancedFilter/models';
import { ILayerItem } from '@/components/maps/leaflet/Control/LayersControl/types';
import { IFilePropertyLocation } from '@/components/maps/types';
import { IMapSideBarViewState as IMapSideBarState } from '@/features/mapSideBar/MapSideBar';
import { IPropertyFilter } from '@/features/properties/filter/IPropertyFilter';

import { MapFeatureData, MarkerSelected, RequestedCenterTo, RequestedFlyTo } from '../models';
import { LocationFeatureDataset, SelectedFeatureDataset } from '../useLocationFeatureLoader';

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
  selectedFeatureDataset: SelectedFeatureDataset | null;
  repositioningFeatureDataset: SelectedFeatureDataset | null;
  repositioningPropertyIndex: number | null;
  selectingComponentId: string | null;

  mapFeatureData: MapFeatureData;

  // TODO: this is partially in the URL. Either move it completly there or remove it
  searchCriteria: IPropertyFilter | null;
  advancedSearchCriteria: PropertyFilterFormModel | null;

  isLoading: boolean;
  fitToResultsAfterLoading: boolean;
  requestedFitBounds: LatLngBounds;
  requestedFlyTo: RequestedFlyTo;
  requestedCenterTo: RequestedCenterTo;
  filePropertyLocations: IFilePropertyLocation[];
  activePimsPropertyIds: number[];
  activeLayers: ILayerItem[];
  mapLayersToRefresh: ILayerItem[];
  isFiltering: boolean;
  showDisposed: boolean;
  showRetired: boolean;
  currentMapBounds: LatLngBounds | null;
};
