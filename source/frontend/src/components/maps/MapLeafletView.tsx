import axios from 'axios';
import { Feature, GeoJsonProperties, Geometry } from 'geojson';
import {
  geoJSON,
  LatLng,
  LatLngBounds,
  LeafletEvent,
  Map as LeafletMap,
  Popup as LeafletPopup,
} from 'leaflet';
import isEqual from 'lodash/isEqual';
import React, { useEffect, useRef, useState } from 'react';
import { LayerGroup, MapContainer as ReactLeafletMap, TileLayer } from 'react-leaflet';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { MAP_MAX_NATIVE_ZOOM, MAP_MAX_ZOOM, MAX_ZOOM } from '@/constants/strings';

import { DEFAULT_MAP_ZOOM, defaultBounds, defaultLatLng } from './constants';
import AdvancedFilterButton from './leaflet/Control/AdvancedFilter/AdvancedFilterButton';
import BasemapToggle, {
  BaseLayer,
  BasemapToggleEvent,
} from './leaflet/Control/BaseMapToggle/BasemapToggle';
import LayersControl from './leaflet/Control/LayersControl/LayersControl';
import { LegendControl } from './leaflet/Control/Legend/LegendControl';
import { ZoomOutButton } from './leaflet/Control/ZoomOut/ZoomOutButton';
import { LayerPopupContainer } from './leaflet/LayerPopup/LayerPopupContainer';
import { InventoryLayer } from './leaflet/Layers/InventoryLayer';
import { LeafletLayerListener } from './leaflet/Layers/LeafletLayerListener';
import { MapEvents } from './leaflet/MapEvents/MapEvents';
import * as Styled from './leaflet/styles';

export type MapLeafletViewProps = {
  parentWidth: number | undefined;
};

type BaseLayerFile = {
  basemaps: BaseLayer[];
};

/**
 * Creates a Leaflet map and by default includes a number of preconfigured layers.
 * @param param0
 */
const MapLeafletView: React.FC<React.PropsWithChildren<MapLeafletViewProps>> = ({
  parentWidth,
}) => {
  const [baseLayers, setBaseLayers] = useState<BaseLayer[]>([]);

  const [activeBasemap, setActiveBasemap] = useState<BaseLayer | null>(null);

  const [bounds, setBounds] = useState<LatLngBounds>(defaultBounds);

  // a reference to the layer popup
  const popupRef = useRef<LeafletPopup>(null);

  const mapRef = useRef<LeafletMap | null>(null);

  const [activeFeatureLayer, setActiveFeatureLayer] = useState<L.GeoJSON>();

  // add geojson layer to the map
  if (!!mapRef.current && !activeFeatureLayer) {
    setActiveFeatureLayer(geoJSON().addTo(mapRef.current));
  }

  const handleMapClickEvent = (latlng: LatLng) => {
    mapMachine.mapClick(latlng);
  };

  const [zoom, setZoom] = useState(DEFAULT_MAP_ZOOM);
  const [isMapReady, setIsMapReady] = useState(false);

  const handleZoomUpdate = (zoomLevel: number) => {
    setZoom(zoomLevel);
  };

  const mapMachine = useMapStateMachine();

  const mapMachinePendingRefresh = mapMachine.pendingFitBounds;
  const mapMachineProcessFitBounds = mapMachine.processFitBounds;
  const mapMachineRequestedFitBounds = mapMachine.requestedFitBounds;
  useEffect(() => {
    if (isMapReady && mapMachinePendingRefresh && mapRef.current !== null) {
      mapRef.current.fitBounds(mapMachineRequestedFitBounds, {
        maxZoom: zoom > MAX_ZOOM ? zoom : MAX_ZOOM,
      });
      mapMachineProcessFitBounds();
    }
  }, [
    isMapReady,
    mapMachinePendingRefresh,
    mapMachineProcessFitBounds,
    mapMachineRequestedFitBounds,
    zoom,
  ]);

  const mapMachineMapLocationFeatureDataset = mapMachine.mapLocationFeatureDataset;
  useEffect(() => {
    activeFeatureLayer?.clearLayers();
    if (mapMachineMapLocationFeatureDataset !== null) {
      const location = mapMachineMapLocationFeatureDataset.location;

      let activeFeature: Feature<Geometry, GeoJsonProperties> = {
        geometry: { coordinates: [location.lng, location.lat], type: 'Point' },
        type: 'Feature',
        properties: {},
      };
      if (mapMachineMapLocationFeatureDataset.parcelFeature !== null) {
        activeFeature = mapMachineMapLocationFeatureDataset.parcelFeature;
        activeFeatureLayer?.addData(activeFeature);
      } else if (mapMachineMapLocationFeatureDataset.municipalityFeature !== null) {
        activeFeature = mapMachineMapLocationFeatureDataset.municipalityFeature;
        if (mapMachineMapLocationFeatureDataset.municipalityFeature?.geometry?.type === 'Polygon') {
          activeFeatureLayer?.addData(activeFeature);
        }
      }
    }
  }, [activeFeatureLayer, mapMachineMapLocationFeatureDataset]);

  const hasPendingFlyTo = mapMachine.pendingFlyTo;
  const requestedFlyTo = mapMachine.requestedFlyTo;
  const mapMachineProcessFlyTo = mapMachine.processFlyTo;
  useEffect(() => {
    if (hasPendingFlyTo && isMapReady) {
      if (requestedFlyTo.bounds !== null) {
        mapRef?.current?.flyToBounds(requestedFlyTo.bounds, { animate: false });
      }
      if (requestedFlyTo.location !== null) {
        mapRef?.current?.flyTo(requestedFlyTo.location, MAP_MAX_ZOOM, {
          animate: false,
        });
      }

      mapMachineProcessFlyTo();
    }
  }, [isMapReady, hasPendingFlyTo, requestedFlyTo, mapMachineProcessFlyTo]);

  useEffect(() => {
    mapRef.current?.invalidateSize();
  }, [mapRef, parentWidth]);

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
      // TODO: Is this necessary?
      //mapRef.current.fitBounds(defaultBounds);
    }
  };

  const handleMapReady = () => {
    fitMapBounds();
  };

  const handleMapCreated = (mapInstance: L.Map) => {
    setIsMapReady(true);
    if (mapInstance !== null) {
      mapRef.current = mapInstance;
    }
  };

  const handleBounds = (event: LeafletEvent) => {
    const boundsData: LatLngBounds = event.target.getBounds();
    if (!isEqual(boundsData.getNorthEast(), boundsData.getSouthWest())) {
      setBounds(boundsData);
    }
  };

  return (
    <Styled.MapContainer>
      {baseLayers?.length > 0 && (
        <BasemapToggle baseLayers={baseLayers} onToggle={handleBasemapToggle} />
      )}

      <ReactLeafletMap
        center={[defaultLatLng.lat, defaultLatLng.lng]}
        zoom={DEFAULT_MAP_ZOOM}
        maxZoom={MAP_MAX_ZOOM}
        closePopupOnClick={false}
        ref={handleMapCreated}
        whenReady={handleMapReady}
      >
        <MapEvents
          click={e => handleMapClickEvent(e.latlng)}
          zoomend={e => handleZoomUpdate(e.sourceTarget.getZoom())}
          moveend={handleBounds}
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
        {mapMachine.showPopup && (
          // Draws the popup on top of the map
          <LayerPopupContainer ref={popupRef} />
        )}

        <LegendControl />
        <ZoomOutButton />
        <AdvancedFilterButton onToggle={mapMachine.toggleMapFilter} />
        <LayersControl onToggle={mapMachine.toggleMapLayer} />
        <InventoryLayer zoom={zoom} bounds={bounds} maxZoom={MAP_MAX_ZOOM}></InventoryLayer>
        <LeafletLayerListener />
      </ReactLeafletMap>
    </Styled.MapContainer>
  );
};

export default MapLeafletView;
