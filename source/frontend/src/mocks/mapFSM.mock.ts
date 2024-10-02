import { SideBarType } from '@/components/common/mapFSM/machineDefinition/types';
import { IMapStateMachineContext } from '@/components/common/mapFSM/MapStateMachineContext';
import {
  emptyPimsBoundaryFeatureCollection,
  emptyPimsLocationFeatureCollection,
  emptyPmbcFeatureCollection,
} from '@/components/common/mapFSM/models';
import { defaultBounds } from '@/components/maps/constants';
import { layersTree } from '@/components/maps/leaflet/Control/LayersControl/data';

export const mapMachineBaseMock: IMapStateMachineContext = {
  requestFlyToBounds: vi.fn(),
  mapFeatureData: {
    pimsLocationFeatures: emptyPimsLocationFeatureCollection,
    pimsBoundaryFeatures: emptyPimsBoundaryFeatureCollection,
    fullyAttributedFeatures: emptyPmbcFeatureCollection,
  },
  mapSideBarViewState: {
    isCollapsed: false,
    type: SideBarType.NOT_DEFINED,
    isOpen: false,
    isFullWidth: false,
  },
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
  repositioningFeatureDataset: null,
  repositioningPropertyIndex: null,
  selectingComponentId: null,
  selectedFeatureDataset: null,
  showPopup: false,
  isLoading: false,
  mapSearchCriteria: null,

  filePropertyLocations: [],
  activePimsPropertyIds: [],
  activeLayers: layersTree,
  isSelecting: false,
  isRepositioning: false,
  isFiltering: false,
  isShowingMapFilter: false,
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
  startReposition: vi.fn(),
  finishReposition: vi.fn(),
  setFilePropertyLocations: vi.fn(),
  setVisiblePimsProperties: vi.fn(),
  toggleMapFilterDisplay: vi.fn(),

  toggleMapLayerControl: vi.fn(),
  setShowDisposed: vi.fn(),
  setShowRetired: vi.fn(),
  setMapLayers: vi.fn(),
  setDefaultMapLayers: vi.fn(),
  toggleSidebarDisplay: vi.fn(),
  setFullWidthSideBar: vi.fn(),
  resetMapFilter: vi.fn(),
};
