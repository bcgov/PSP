import { Feature, GeoJsonProperties, Geometry } from 'geojson';
import produce from 'immer';
import { IProperty } from 'interfaces';
import { noop } from 'lodash';
import React, { useCallback, useReducer } from 'react';

import { PointFeature } from '../types';

export enum MapCursors {
  DRAFT = 'draft-cursor',
  DEFAULT = 'default',
}

export enum MapState {
  MAP = 'MAP',
  RESEARCH_FILE = 'RESEARCH_FILE',
  ACQUISITION_FILE = 'ACQUISITION_FILE',
  LEASE_FILE = 'LEASE_FILE',
}

export enum MapStateActionTypes {
  MAP_STATE = 'MAP_STATE',
  SELECTED_FEATURE = 'SELECTED_FEATURE',
  SELECTED_INVENTORY_PROPERTY = 'SELECTED_INVENTORY_PROPERTY',
  SELECTED_LEASE_PROPERTY = 'SELECTED_LEASE_PROPERTY',
  SELECTED_FILE_FEATURE = 'SELECTED_FILE_FEATURE',
  DRAFT_PROPERTIES = 'DRAFT_PROPERTIES',
  LOADING = 'LOADING',
  CURSOR = 'CURSOR',
  IS_SELECTING = 'IS_SELECTING',
  LAYERS_OPEN = 'LAYERS_OPEN',
}

export interface IMapStateContext {
  /** The current state of the map. reflects if any of the sidebars are visible. May affect the state of other actions */
  mapState: MapState;
  /** The currently selected property from the PIMS inventory */
  selectedInventoryProperty: IProperty | null;
  /** The currently selected property from the PIMS inventory intended for L&L */
  selectedLeaseProperty: IProperty | null;
  /** The currently selected feature from the LTSA ParcelMap */
  selectedFeature: Feature<Geometry, GeoJsonProperties> | null;
  /** The currently selected feature in the context of an active file */
  selectedFileFeature: Feature<Geometry, GeoJsonProperties> | null;
  /** A list of draft properties to display on the map. These properties are displayed with a different map marker and click behaviour */
  draftProperties: PointFeature[];
  /** whether or not the underlying map marker layer is loading */
  loading: boolean;
  /** The active cursor displayed when hovering over the map */
  cursor: MapCursors;
  /**  */
  isSelecting: boolean;
  layersOpen: boolean;
  setState: React.Dispatch<MapStateActions>;
}

const initialState = {
  mapState: MapState.MAP,
  selectedInventoryProperty: null,
  selectedLeaseProperty: null,
  selectedFeature: null,
  selectedFileFeature: null,
  draftProperties: [],
  loading: false,
  isSelecting: false,
  layersOpen: false,
  cursor: MapCursors.DEFAULT,
  setState: noop,
};

export type MapStateActions =
  | {
      type: MapStateActionTypes.MAP_STATE;
      mapState: MapState;
    }
  | {
      type: MapStateActionTypes.SELECTED_FEATURE;
      selectedFeature: Feature<Geometry, GeoJsonProperties> | null;
    }
  | {
      type: MapStateActionTypes.SELECTED_INVENTORY_PROPERTY;
      selectedInventoryProperty: IProperty | null;
    }
  | {
      type: MapStateActionTypes.SELECTED_LEASE_PROPERTY;
      selectedLeaseProperty: IProperty | null;
    }
  | {
      type: MapStateActionTypes.SELECTED_FILE_FEATURE;
      selectedFileFeature: Feature<Geometry, GeoJsonProperties> | null;
    }
  | { type: MapStateActionTypes.DRAFT_PROPERTIES; draftProperties: PointFeature[] }
  | { type: MapStateActionTypes.LOADING; loading: boolean }
  | { type: MapStateActionTypes.CURSOR; cursor: MapCursors }
  | { type: MapStateActionTypes.IS_SELECTING; isSelecting: boolean }
  | { type: MapStateActionTypes.LAYERS_OPEN; layersOpen: boolean };

export const MapStateContext = React.createContext<IMapStateContext>(initialState);

interface IMapStateContextComponent {
  values?: Partial<IMapStateContext>;
}

export const MapStateContextProvider: React.FC<
  React.PropsWithChildren<IMapStateContextComponent>
> = ({ children, values }) => {
  const mapStateReducer = useCallback(
    (prevState: IMapStateContext, action: MapStateActions): IMapStateContext => {
      switch (action.type) {
        case MapStateActionTypes.MAP_STATE:
          return produce(prevState, draft => {
            draft.mapState = action.mapState;
          });
        case MapStateActionTypes.SELECTED_FEATURE:
          return produce(prevState, draft => {
            draft.selectedFeature = action.selectedFeature;
          });
        case MapStateActionTypes.SELECTED_FILE_FEATURE:
          return produce(prevState, draft => {
            draft.selectedFileFeature = action.selectedFileFeature;
          });
        case MapStateActionTypes.SELECTED_INVENTORY_PROPERTY:
          return produce(prevState, draft => {
            draft.selectedInventoryProperty = action.selectedInventoryProperty;
          });
        case MapStateActionTypes.SELECTED_LEASE_PROPERTY:
          return produce(prevState, draft => {
            draft.selectedLeaseProperty = action.selectedLeaseProperty;
          });
        case MapStateActionTypes.LOADING:
          return produce(prevState, draft => {
            draft.loading = action.loading;
            draft.selectedInventoryProperty = null;
            draft.selectedFileFeature = null;
            draft.selectedLeaseProperty = null;
            draft.selectedFeature = null;
          });
        case MapStateActionTypes.CURSOR:
          return produce(prevState, draft => {
            draft.cursor = action.cursor;
          });
        case MapStateActionTypes.IS_SELECTING:
          if (
            ![MapState.ACQUISITION_FILE, MapState.RESEARCH_FILE, MapState.LEASE_FILE].includes(
              prevState.mapState,
            )
          ) {
            throw Error(
              `Cannot enter selection mode unless in the context of a file. Current ${prevState.mapState}`,
            );
          }
          return produce(prevState, draft => {
            draft.isSelecting = action.isSelecting;
            draft.cursor = action.isSelecting ? MapCursors.DRAFT : MapCursors.DEFAULT;
          });
        case MapStateActionTypes.LAYERS_OPEN:
          return produce(prevState, draft => {
            draft.layersOpen = action.layersOpen;
          });
        case MapStateActionTypes.DRAFT_PROPERTIES:
          return produce(prevState, draft => {
            draft.draftProperties = action.draftProperties;
          });
        default:
          throw Error('action type unsupported');
      }
    },
    [],
  );
  const [mapState, dispatch] = useReducer(mapStateReducer, initialState);

  return (
    <MapStateContext.Provider
      value={{
        ...mapState,
        ...values,
        setState:
          values?.setState === initialState.setState || values?.setState === undefined
            ? dispatch
            : values?.setState ?? noop,
      }}
    >
      {children}
    </MapStateContext.Provider>
  );
};
