import { LatLngBounds, LatLngLiteral } from 'leaflet';

import { ILayerItem } from '@/components/maps/leaflet/Control/LayersControl/types';
import { IMapSideBarViewState as IMapSideBarState } from '@/features/mapSideBar/MapSideBar';
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
  DISPOSITION = 'DISPOSITION',
  SUBDIVISION = 'SUBDIVISION',
  CONSOLIDATION = 'CONSOLIDATION',
}

// Local context for the machine - Not related to React Context!
export type MachineContext = {
  mapSideBarState: IMapSideBarState | null;
  mapFeatureSelected: FeatureSelected | null;
  mapLocationSelected: LatLngLiteral | null;
  mapLocationFeatureDataset: LocationFeatureDataset | null;
  selectedFeatureDataset: LocationFeatureDataset | null;
  selectingComponentId: string | null;

  mapFeatureData: MapFeatureData;

  // TODO: this is partially in the URL. Either move it completly there or remove it
  searchCriteria: IPropertyFilter | null;

  isLoading: boolean;
  requestedFitBounds: LatLngBounds;
  requestedFlyTo: RequestedFlyTo;
  filePropertyLocations: LatLngLiteral[];
  activePimsPropertyIds: number[];
  activeLayers: ILayerItem[];
  showDisposed: boolean;
  showRetired: boolean;
};
