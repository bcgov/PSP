import { IMapStateMachineContext } from '@/components/common/mapFSM/MapStateMachineContext';
import {
  emptyPimsBoundaryFeatureCollection,
  emptyPimsLocationFeatureCollection,
  emptyPmbcFeatureCollection,
} from '@/components/common/mapFSM/models';
import { defaultBounds } from '@/components/maps/constants';

export const mapMachineBaseMock: IMapStateMachineContext = {
  requestFlyToBounds: vi.fn(),
  mapFeatureData: {
    pimsLocationFeatures: emptyPimsLocationFeatureCollection,
    pimsBoundaryFeatures: emptyPimsBoundaryFeatureCollection,
    pmbcFeatures: emptyPmbcFeatureCollection,
  },

  isSidebarOpen: false,
  isShowingSearchBar: false,
  pendingFlyTo: false,
  pendingFitBounds: false,
  requestedFlyTo: {
    location: null,
    bounds: null,
  },
  requestedFitBounds: defaultBounds,
  mapFeatureSelected: null,
  mapLocationSelected: null,
  mapLocationFeatureDataset: null,
  selectingComponentId: null,
  selectedFeatureDataset: null,
  showPopup: false,
  isLoading: false,
  mapSearchCriteria: null,

  filePropertyLocations: [],
  activePimsPropertyIds: [],
  activeLayers: [],
  isSelecting: false,
  isFiltering: false,
  isShowingMapLayers: false,
  showDisposed: false,
  showRetired: false,

  requestFlyToLocation: vi.fn(),

  processFlyTo: vi.fn(),
  processFitBounds: vi.fn(),
  openSidebar: vi.fn(),
  closeSidebar: vi.fn(),
  closePopup: vi.fn(),
  mapClick: vi.fn(),
  mapMarkerClick: vi.fn(),
  setMapSearchCriteria: vi.fn(),
  refreshMapProperties: vi.fn(),
  prepareForCreation: vi.fn(),
  startSelection: vi.fn(),
  finishSelection: vi.fn(),
  setFilePropertyLocations: vi.fn(),
  setVisiblePimsProperties: vi.fn(),
  toggleMapFilter: vi.fn(),

  toggleMapLayer: vi.fn(),
  setShowDisposed: vi.fn(),
  setShowRetired: vi.fn(),
  changeSidebar: vi.fn(),
  setMapLayers: vi.fn(),
};
