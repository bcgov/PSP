import axios from 'axios';
import {
  LatLngBounds,
  LeafletEvent,
  Map as LeafletMap,
  Popup as LeafletPopup,
  PopupEvent,
} from 'leaflet';
import isEqual from 'lodash/isEqual';
import React, { useContext, useEffect, useRef, useState } from 'react';
import { LayerGroup, MapContainer as ReactLeafletMap, TileLayer } from 'react-leaflet';
import { useDispatch } from 'react-redux';
import { useResizeDetector } from 'react-resize-detector';
import { useMediaQuery } from 'react-responsive';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';

import { IGeoSearchParams } from '@/constants/API';
import { MAP_MAX_NATIVE_ZOOM, MAP_MAX_ZOOM } from '@/constants/strings';
import { IProperty } from '@/interfaces';
import { useAppSelector } from '@/store/hooks';
import { DEFAULT_MAP_ZOOM, setMapViewZoom } from '@/store/slices/mapViewZoom/mapViewZoomSlice';
import { pidParser } from '@/utils/propertyUtils';

import useActiveFeatureLayer from './hooks/useActiveFeatureLayer';
import BasemapToggle, {
  BaseLayer,
  BasemapToggleEvent,
} from './leaflet/Control/BaseMapToggle/BasemapToggle';
import LayersControl from './leaflet/Control/LayersControl/LayersControl';
import { LegendControl } from './leaflet/Control/Legend/LegendControl';
import { ZoomOutButton } from './leaflet/Control/ZoomOut/ZoomOutButton';
import {
  LayerPopupContainer,
  LayerPopupInformation,
} from './leaflet/LayerPopup/LayerPopupContainer';
import { InventoryLayer } from './leaflet/Layers/InventoryLayer';
import { MapEvents } from './leaflet/MapEvents/MapEvents';
import * as Styled from './leaflet/styles';
import { MapStateActionTypes, MapStateContext } from './providers/MapStateContext';

export type MapLeafletViewProps = {
  lat: number;
  lng: number;
  zoom?: number;
  whenCreated?: (map: LeafletMap) => void;
  whenReady?: () => void;
  geoFilter?: IGeoSearchParams;
  mapRef: React.MutableRefObject<LeafletMap | null>;
};

type BaseLayerFile = {
  basemaps: BaseLayer[];
};

const defaultBounds = new LatLngBounds([60.09114547, -119.49609429], [48.78370426, -139.35937554]);

/**
 * Creates a Leaflet map and by default includes a number of preconfigured layers.
 * @param param0
 */
const MapLeafletView: React.FC<React.PropsWithChildren<MapLeafletViewProps>> = ({
  lat,
  lng,
  zoom: zoomProp,
  whenReady,
  whenCreated,
  geoFilter,
  mapRef,
}) => {
  const dispatch = useDispatch();

  const [baseLayers, setBaseLayers] = useState<BaseLayer[]>([]);

  const [activeBasemap, setActiveBasemap] = useState<BaseLayer | null>(null);
  const smallScreen = useMediaQuery({ maxWidth: 1800 });

  const [bounds, setBounds] = useState<LatLngBounds>(defaultBounds);
  const [layerPopup, setLayerPopup] = useState<LayerPopupInformation>();

  // a reference to the layer popup
  const popupRef = useRef<LeafletPopup>(null);

  const history = useHistory();
  const onPropertyViewClicked = (pid?: string | null, id?: number) => {
    if (id !== undefined) {
      history.push(`/mapview/sidebar/property/${id}?pid=${pid}`);
    } else if (pid !== undefined && pid !== null) {
      const parsedPid = pidParser(pid);
      history.push(`/mapview/sidebar/non-inventory-property/${parsedPid}`);
    } else {
      console.warn('Invalid marker when trying to see property information');
      toast.warn('A map parcel must have a PID in order to view detailed information');
    }
  };

  const { setState, selectedInventoryProperty, selectedFeature } = useContext(MapStateContext);

  if (mapRef.current && !selectedInventoryProperty) {
    const center = mapRef.current.getCenter();
    lat = center.lat;
    lng = center.lng;
  }

  const parcelLayerFeature = selectedFeature;
  const { showLocationDetails } = useActiveFeatureLayer({
    selectedProperty: selectedInventoryProperty,
    layerPopup,
    mapRef,
    parcelLayerFeature,
    setLayerPopup,
  });

  const lastZoom = useAppSelector(state => state.mapViewZoom) ?? zoomProp;
  const [zoom, setZoom] = useState(lastZoom);
  useEffect(() => {
    if (lastZoom === DEFAULT_MAP_ZOOM) {
      dispatch(setMapViewZoom(smallScreen ? 4.9 : 5.5));
    } else if (lastZoom !== zoom && zoom !== DEFAULT_MAP_ZOOM) {
      dispatch(setMapViewZoom(zoom));
    }
  }, [dispatch, lastZoom, smallScreen, zoom]);

  const { width } = useResizeDetector();
  useEffect(() => {
    mapRef.current?.invalidateSize();
  }, [mapRef, width]);

  const handleBasemapToggle = (e: BasemapToggleEvent) => {
    const { previous, current } = e;
    setBaseLayers([current, previous]);
    setActiveBasemap(current);
  };

  useEffect(() => {
    // fetch GIS base layers configuration from /public folder
    axios.get<BaseLayerFile>('/basemaps.json')?.then(result => {
      setBaseLayers(result.data?.basemaps);
      setActiveBasemap(result.data?.basemaps?.[0]);
    });
  }, []);

  const fitMapBounds = () => {
    if (mapRef.current) {
      mapRef.current.fitBounds([
        [60.09114547, -119.49609429],
        [48.78370426, -139.35937554],
      ]);
    }
  };

  const handleMapReady = () => {
    fitMapBounds();
    if (typeof whenReady === 'function') {
      whenReady();
    }
  };

  const handleMapCreated = (mapInstance: L.Map) => {
    mapRef.current = mapInstance;
    if (typeof whenCreated === 'function') {
      whenCreated(mapInstance);
    }
  };

  const handleBounds = (event: LeafletEvent) => {
    const boundsData: LatLngBounds = event.target.getBounds();
    if (!isEqual(boundsData.getNorthEast(), boundsData.getSouthWest())) {
      setBounds(boundsData);
    }
  };

  const onPopupClose = (event: PopupEvent) => {
    if (event.popup === popupRef.current) {
      setLayerPopup(undefined);
      setState({
        type: MapStateActionTypes.SELECTED_INVENTORY_PROPERTY,
        selectedInventoryProperty: null,
      });
    }
  };

  const [layersOpen, setLayersOpen] = React.useState(false);

  return (
    <Styled.MapContainer>
      {baseLayers?.length > 0 && (
        <BasemapToggle baseLayers={baseLayers} onToggle={handleBasemapToggle} />
      )}

      <ReactLeafletMap
        center={[lat, lng]}
        zoom={lastZoom}
        maxZoom={MAP_MAX_ZOOM}
        closePopupOnClick={true}
        ref={handleMapCreated}
        whenReady={handleMapReady}
      >
        <MapEvents
          click={e => showLocationDetails(e.latlng)}
          zoomend={e => setZoom(e.sourceTarget.getZoom())}
          moveend={handleBounds}
          popupclose={onPopupClose}
        />
        {activeBasemap && (
          // Draws the map itself
          <LayerGroup attribution={activeBasemap.attribution}>
            {activeBasemap.urls?.map((tileUrl, index) => (
              <TileLayer
                key={`${activeBasemap.name}-${index}`}
                zIndex={index}
                url={tileUrl}
                maxZoom={MAP_MAX_ZOOM}
                maxNativeZoom={MAP_MAX_NATIVE_ZOOM}
              />
            ))}
          </LayerGroup>
        )}
        {!!layerPopup && (
          // Draws the popup on top of the map
          <LayerPopupContainer
            ref={popupRef}
            layerPopup={layerPopup}
            onViewPropertyInfo={onPropertyViewClicked}
          />
        )}

        <LegendControl />
        <ZoomOutButton bounds={defaultBounds} />
        <LayersControl
          open={layersOpen}
          setOpen={() => {
            setLayersOpen(!layersOpen);
          }}
        />
        <InventoryLayer
          zoom={zoom}
          bounds={bounds}
          onMarkerClick={(property: IProperty) => {
            setLayersOpen(false);
            onPropertyViewClicked(property.pid, property.id);
          }}
          filter={geoFilter}
        ></InventoryLayer>
      </ReactLeafletMap>
    </Styled.MapContainer>
  );
};

export default MapLeafletView;
