import { IMapStateMachineContext } from '@/components/common/mapFSM/MapStateMachineContext';
import {
  emptyPimsBoundaryFeatureCollection,
  emptyPimsLocationFeatureCollection,
  emptyPmbcFeatureCollection,
} from '@/components/common/mapFSM/models';
import { defaultBounds } from '@/components/maps/constants';

export const mapMachineBaseMock: IMapStateMachineContext = {
  requestFlyToBounds: jest.fn(),
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

  requestFlyToLocation: jest.fn(),

  processFlyTo: jest.fn(),
  processFitBounds: jest.fn(),
  openSidebar: jest.fn(),
  closeSidebar: jest.fn(),
  closePopup: jest.fn(),
  mapClick: jest.fn(),
  mapMarkerClick: jest.fn(),
  setMapSearchCriteria: jest.fn(),
  refreshMapProperties: jest.fn(),
  prepareForCreation: jest.fn(),
  startSelection: jest.fn(),
  finishSelection: jest.fn(),
  setFilePropertyLocations: jest.fn(),
  setVisiblePimsProperties: jest.fn(),
  toggleMapFilter: jest.fn(),

  toggleMapLayer: jest.fn(),
  setShowDisposed: jest.fn(),
  setShowRetired: jest.fn(),
  setMapLayers: jest.fn(),
  changeSidebar: jest.fn(),
};
