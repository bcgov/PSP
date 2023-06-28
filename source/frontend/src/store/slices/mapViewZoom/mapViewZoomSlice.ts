import type { PayloadAction } from '@reduxjs/toolkit';
import { createSlice } from '@reduxjs/toolkit';
import { Feature, GeoJsonProperties, Geometry } from 'geojson';
import { LatLngBounds, LatLngExpression, LatLngLiteral, Map, ZoomPanOptions } from 'leaflet';

import { PointFeature } from '@/components/maps/types';
import { MAP_MAX_ZOOM } from '@/constants/strings';
import { Api_Property } from '@/models/api/Property';
import { RootState } from '@/store/store';

/*export const setMapViewZoom = createAction<number>('setMapViewZoom');
export const resetMapViewZoom = createAction('resetMapViewZoom');*/
export const DEFAULT_MAP_ZOOM = 6;
/** rough center of bc Itcha Ilgachuz Provincial Park */
export const defaultLatLng = {
  lat: 52.81604319154934,
  lng: -124.67285156250001,
};

// default BC map bounds
export const defaultBounds = new LatLngBounds(
  [60.09114547, -119.49609429],
  [48.78370426, -139.35937554],
);

export enum MapSidebarState {
  MAP = 'MAP',
  RESEARCH_FILE = 'RESEARCH_FILE',
  ACQUISITION_FILE = 'ACQUISITION_FILE',
  LEASE_FILE = 'LEASE_FILE',
  PROJECT = 'PROJECT',
}

interface MapState {
  //zoomLevel: number;
  //bounds: LatLngBounds;

  /** The current state of the map. reflects if any of the sidebars are visible. May affect the state of other actions */
  mapSidebarState: MapSidebarState;
  /** The currently selected property from the PIMS inventory */
  selectedInventoryProperty: Api_Property | null;
  /** The currently selected feature from the LTSA ParcelMap */
  selectedFeature: Feature<Geometry, GeoJsonProperties> | null;
  /** The currently selected feature in the context of an active file */
  selectedFileFeature: Feature<Geometry, GeoJsonProperties> | null;
  /** A list of draft properties to display on the map. These properties are displayed with a different map marker and click behaviour */
  draftProperties: PointFeature[];
  /** whether or not the underlying map marker layer is loading */
  loading: boolean;

  /**  */
  isSelecting: boolean;
  layersOpen: boolean;
  zoomTo: LatLngLiteral | null;
}

interface MapFunctions {
  flyTo(latlng: LatLngExpression, zoom?: number, options?: ZoomPanOptions): this;
}

const initialState: MapState = {
  mapSidebarState: MapSidebarState.MAP,
  selectedInventoryProperty: null,
  selectedFeature: null,
  selectedFileFeature: null,
  draftProperties: [],
  loading: false,
  isSelecting: false,
  layersOpen: false,
  zoomTo: null,
};

const mapViewZoomSlice = createSlice({
  name: 'mapViewZoom',
  initialState: initialState,
  reducers: {
    setMapSidebarState(state, action: PayloadAction<MapSidebarState>) {
      state.mapSidebarState = action.payload;
    },
    setSelectedInventoryProperty(state, action: PayloadAction<Api_Property | null>) {
      state.selectedInventoryProperty = action.payload;
    },
    setSelectedFeature(state, action: PayloadAction<Feature<Geometry, GeoJsonProperties> | null>) {
      state.selectedFeature = action.payload;
    },
    setSelectedFileFeature(
      state,
      action: PayloadAction<Feature<Geometry, GeoJsonProperties> | null>,
    ) {
      state.selectedFileFeature = action.payload;
    },
    setDraftProperties(state, action: PayloadAction<PointFeature[]>) {
      state.draftProperties = action.payload;
    },
    setLoading(state, action: PayloadAction<boolean>) {
      state.loading = action.payload;
    },
    setIsSelecting(state, action: PayloadAction<boolean>) {
      state.isSelecting = action.payload;
    },
    setLayersOpen(state, action: PayloadAction<boolean>) {
      state.layersOpen = action.payload;
    },
    zoomToPropery(state, action: PayloadAction<Api_Property | null>) {
      console.log('zoomToPropery', action.payload);
      if (action.payload !== null && action.payload.latitude && action.payload.longitude) {
        state.zoomTo = { lat: action.payload.latitude, lng: action.payload.longitude };
      }
      /*if (state.mapRef !== null && state.mapRef.current !== null) {
        const apiProperty = action.payload;
        apiProperty?.longitude &&
          apiProperty?.latitude &&
          state.mapRef?.current?.flyTo(
            { lat: apiProperty?.latitude, lng: apiProperty?.longitude },
            MAP_MAX_ZOOM,
            {
              animate: false,
            },
          );
      }*/
    },
  },
});

// Destructure and export the plain action creators
//export const { setMapZoom, resetMapZoom, setMapBounds, resetMapBounds } = mapViewZoomSlice.actions;

//export const { zoomToPropery } = mapViewZoomSlice.actions;

//export const selectMapZoom = (state: RootState) => state.mapViewZoom.zoomLevel;
//export const selectMapBounds = (state: RootState) => state.mapViewZoom.bounds;

export const zoomRequest2 = (state: RootState) => state.mapViewZoom.zoomTo;

export default mapViewZoomSlice;

/*
//const zoom = useSelector(zoomToPropery);

const dispatch = useDispatch();

const handleZoom = (apiProperty?: Api_Property | undefined) => {
  if (apiProperty !== undefined) {
    dispatch(zoomToPropery(apiProperty));
  }
};
*/
