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
import { isEqual } from 'lodash';
import React, { useEffect, useRef, useState } from 'react';
import {
  LayerGroup,
  MapContainer as LeafletMapContainer,
  Pane,
  ScaleControl,
  TileLayer,
} from 'react-leaflet';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { MAP_MAX_NATIVE_ZOOM, MAP_MAX_ZOOM, MAX_ZOOM } from '@/constants/strings';
import { useTenant } from '@/tenants';
import { exists, firstOrNull } from '@/utils';

import { DEFAULT_MAP_ZOOM, defaultBounds, defaultLatLng } from './constants';
import AdvancedFilterButton from './leaflet/Control/AdvancedFilter/AdvancedFilterButton';
import BasemapToggle, { BasemapToggleEvent } from './leaflet/Control/BaseMapToggle/BasemapToggle';
import { BaseLayer, isVectorBasemap } from './leaflet/Control/BaseMapToggle/types';
import LayersControl from './leaflet/Control/LayersControl/LayersControl';
import { LegendControl } from './leaflet/Control/Legend/LegendControl';
import { ZoomOutButton } from './leaflet/Control/ZoomOut/ZoomOutButton';
import { LocationPopupContainer } from './leaflet/LayerPopup/LocationPopupContainer';
import { InventoryLayer } from './leaflet/Layers/InventoryLayer';
import { LeafletLayerListener } from './leaflet/Layers/LeafletLayerListener';
import { useConfiguredMapLayers } from './leaflet/Layers/useConfiguredMapLayers';
import { MapEvents } from './leaflet/MapEvents/MapEvents';
import * as Styled from './leaflet/styles';
import { EsriVectorTileLayer } from './leaflet/VectorTileLayer/EsriVectorTileLayer';

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
  const [zoom, setZoom] = useState(DEFAULT_MAP_ZOOM);
  const [isMapReady, setIsMapReady] = useState(false);

  // a reference to the layer popup
  const popupRef = useRef<LeafletPopup>(null);

  const mapRef = useRef<LeafletMap | null>(null);
  const layers = useConfiguredMapLayers();

  const [activeFeatureLayer, setActiveFeatureLayer] = useState<L.GeoJSON>();
  const { doubleClickInterval } = useTenant();

  // add geojson layer to the map
  if (!!mapRef.current && !activeFeatureLayer) {
    setActiveFeatureLayer(geoJSON().addTo(mapRef.current));
  }

  const timer = useRef(null);

  const handleMapClickEvent = (latlng: LatLng) => {
    if (timer?.current !== null) {
      return;
    }
    timer.current = setTimeout(() => {
      mapMachine.mapClick(latlng);
      timer.current = null;
    }, doubleClickInterval ?? 250);
  };

  const handleDoubleClickEvent = () => {
    clearTimeout(timer?.current);
    timer.current = null;
  };

  const handleZoomUpdate = (zoomLevel: number) => {
    setZoom(zoomLevel);
  };

  const mapMachine = useMapStateMachine();

  const mapMachinePendingRefresh = mapMachine.pendingFitBounds;
  const mapMachineProcessFitBounds = mapMachine.processFitBounds;
  const mapMachineRequestedFitBounds = mapMachine.requestedFitBounds;

  const hasPendingFlyTo = mapMachine.pendingFlyTo;
  const hasPendingCenterTo = mapMachine.pendingCenterTo;
  const requestedFlyTo = mapMachine.requestedFlyTo;
  const requestedCenterTo = mapMachine.requestedCenterTo;

  const mapMachineProcessFlyTo = mapMachine.processFlyTo;
  const mapMachineProcessCenterTo = mapMachine.processCenterTo;

  useEffect(() => {
    if (isMapReady && mapMachinePendingRefresh && mapRef.current !== null) {
      // PSP-9347 it is possible that a fit bounds request will be made with an empty array of selected properties. In that case, we do not want to change the screen bounds, so cancel the request with no changes to the map.
      if (exists(mapMachineRequestedFitBounds) && mapMachineRequestedFitBounds.isValid()) {
        mapRef.current.fitBounds(mapMachineRequestedFitBounds, {
          maxZoom: zoom > MAX_ZOOM ? zoom : MAX_ZOOM,
        });
      }
      mapMachineProcessFitBounds();
    }
  }, [
    isMapReady,
    mapMachinePendingRefresh,
    mapMachineProcessFitBounds,
    mapMachineRequestedFitBounds,
    zoom,
  ]);

  const {
    mapLocationFeatureDataset,
    repositioningFeatureDataset,
    isRepositioning,
    setDefaultMapLayers,
  } = mapMachine;

  useEffect(() => {
    if (isMapReady) {
      setDefaultMapLayers(layers);
    }
  }, [isMapReady, layers, setDefaultMapLayers]);

  useEffect(() => {
    activeFeatureLayer?.clearLayers();

    if (isRepositioning) {
      const pimsFeature = repositioningFeatureDataset?.pimsFeature;
      if (exists(pimsFeature)) {
        // File marker repositioning is active - highlight the property and the corresponding boundary when user triggers the relocate action.
        activeFeatureLayer?.addData(pimsFeature);
      }
    } else {
      // Not repositioning - highlight parcels on map click as usual workflow
      if (mapLocationFeatureDataset !== null) {
        const location = mapLocationFeatureDataset.location;

        let activeFeature: Feature<Geometry, GeoJsonProperties> = {
          geometry: { coordinates: [location.lng, location.lat], type: 'Point' },
          type: 'Feature',
          properties: {},
        };
        if (firstOrNull(mapLocationFeatureDataset.parcelFeatures) !== null) {
          activeFeature = mapLocationFeatureDataset.parcelFeatures[0];
          activeFeatureLayer?.addData(activeFeature);
        } else if (firstOrNull(mapLocationFeatureDataset.municipalityFeatures) !== null) {
          activeFeature = mapLocationFeatureDataset.municipalityFeatures[0];
          if (activeFeature?.geometry?.type === 'Polygon') {
            activeFeatureLayer?.addData(activeFeature);
          }
        }
      }
    }
  }, [activeFeatureLayer, isRepositioning, mapLocationFeatureDataset, repositioningFeatureDataset]);

  useEffect(() => {
    if (hasPendingFlyTo && isMapReady) {
      if (requestedFlyTo.bounds !== null) {
        mapRef?.current?.flyToBounds(requestedFlyTo.bounds, { animate: false });
      }
      if (requestedFlyTo.location !== null) {
        mapRef?.current?.flyTo(requestedFlyTo.location, MAP_MAX_ZOOM, {
          animate: false,
        });
        mapRef?.current?.panTo(requestedFlyTo.location);
      }

      mapMachineProcessFlyTo();
    }
  }, [isMapReady, hasPendingFlyTo, requestedFlyTo, mapMachineProcessFlyTo]);

  useEffect(() => {
    if (hasPendingCenterTo && isMapReady && requestedCenterTo.location) {
      mapRef.current?.setView(requestedCenterTo.location);
    }
    mapMachineProcessCenterTo();
  }, [hasPendingCenterTo, isMapReady, mapMachineProcessCenterTo, requestedCenterTo.location]);

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

  const handleMapCreated = (mapInstance: L.Map) => {
    setIsMapReady(true);
    if (mapInstance !== null) {
      mapRef.current = mapInstance;
    }
  };

  const handleBounds = (event: LeafletEvent) => {
    const boundsData: LatLngBounds = event.target.getBounds();
    if (!isEqual(boundsData.getNorthEast(), boundsData.getSouthWest())) {
      mapMachine.setCurrentMapBounds(boundsData);
    }
  };

  return (
    <Styled.MapContainer>
      {baseLayers?.length > 0 && (
        <BasemapToggle baseLayers={baseLayers} onToggle={handleBasemapToggle} />
      )}

      <LeafletMapContainer
        center={[defaultLatLng.lat, defaultLatLng.lng]}
        zoom={DEFAULT_MAP_ZOOM}
        maxZoom={MAP_MAX_ZOOM}
        closePopupOnClick={false}
        ref={handleMapCreated}
        doubleClickZoom
      >
        <MapEvents
          click={e => handleMapClickEvent(e.latlng)}
          dblclick={() => handleDoubleClickEvent()}
          zoomend={e => handleZoomUpdate(e.sourceTarget.getZoom())}
          moveend={handleBounds}
        />
        {/* The basemap is the first layer to draw, followed by data layers and then graphics */}
        <Pane name="basemap" style={{ zIndex: 200 }}>
          {activeBasemap && (
            <LayerGroup attribution={activeBasemap.attribution}>
              {isVectorBasemap(activeBasemap) ? (
                <EsriVectorTileLayer zIndex={0} itemId={activeBasemap.itemId} />
              ) : (
                activeBasemap.urls?.map((tileUrl, index) => (
                  <TileLayer
                    key={`${activeBasemap.name}-${index}`}
                    zIndex={index}
                    url={tileUrl}
                    maxZoom={MAP_MAX_ZOOM}
                    maxNativeZoom={MAP_MAX_NATIVE_ZOOM}
                  />
                ))
              )}
            </LayerGroup>
          )}
        </Pane>

        {/* Data layers (i.e. parcelmap, tantalis, etc) are drawn on top of basemaps */}
        <Pane name="dataLayers" style={{ zIndex: 350 }}>
          <LeafletLayerListener pane="dataLayers" />
        </Pane>

        {mapMachine.showPopup && !mapMachine.isRepositioning && (
          // Draws the popup on top of the map
          <LocationPopupContainer ref={popupRef} />
        )}

        <LegendControl />
        <ZoomOutButton />
        <ScaleControl position="bottomleft" metric={true} imperial={false} />
        <AdvancedFilterButton
          onToggle={mapMachine.toggleMapFilterDisplay}
          active={mapMachine.isFiltering}
        />
        <LayersControl onToggle={mapMachine.toggleMapLayerControl} />
        <InventoryLayer
          zoom={zoom}
          maxZoom={MAP_MAX_ZOOM}
          bounds={mapMachine.currentMapBounds ?? defaultBounds}
        ></InventoryLayer>
      </LeafletMapContainer>
    </Styled.MapContainer>
  );
};

export default MapLeafletView;
