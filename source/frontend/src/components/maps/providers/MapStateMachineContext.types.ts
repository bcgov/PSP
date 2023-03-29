import { Feature, GeoJsonProperties, Geometry } from 'geojson';
import { IProperty } from 'interfaces';
import { LeafletMouseEvent } from 'leaflet';
import { ActorRefFrom, StateFrom } from 'xstate';

import { LayerPopupInformation } from '../leaflet/LayerPopup';
import { mapMachine } from '../stateMachines/mapMachine';

export interface MapInformation {
  /** whether or not the map is loading */
  isLoading: boolean;
  /** whether or not the map is in "selection mode" */
  isSelecting: boolean;
  /** The currently selected boundary from the LTSA ParcelMap */
  activeParcelMapFeature: Feature<Geometry, GeoJsonProperties> | null;
  /** The currently selected feature in the context of an active file */
  activeFileFeature: Feature<Geometry, GeoJsonProperties> | null;
  /** The currently selected property from the PIMS inventory */
  activeInventoryProperty: IProperty | null;
}

export interface IMapStateMachineContext {
  // shared map state
  map: MapInformation | undefined;
  sidebar: {} | undefined;
  popup: LayerPopupInformation | undefined;

  // event handlers
  useMapClick: () => (event: LeafletMouseEvent) => Promise<Feature | undefined>;
  closePopup: () => void;

  // access to underlying state machine
  service: ActorRefFrom<typeof mapMachine>;
  useMachine: () => [StateFrom<typeof mapMachine>, ActorRefFrom<typeof mapMachine>['send']];
}
