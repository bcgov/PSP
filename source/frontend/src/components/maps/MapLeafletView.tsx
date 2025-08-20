import axios from 'axios';
import {
  geoJSON,
  LatLngBounds,
  LeafletEvent,
  LeafletMouseEvent,
  Map,
  Popup as LeafletPopup,
} from 'leaflet';
import { isEqual } from 'lodash';
import React, { useEffect, useMemo, useRef, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import {
  LayerGroup,
  MapContainer as LeafletMapContainer,
  Pane,
  ScaleControl,
  TileLayer,
} from 'react-leaflet';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import {
  MAP_MAX_NATIVE_ZOOM,
  MAP_MAX_ZOOM,
  MAP_MIN_MARKER_ZOOM,
  MAX_ZOOM,
} from '@/constants/strings';
import { useWorklistContext } from '@/features/properties/worklist/context/WorklistContext';
import WorklistMapClickMonitor from '@/features/properties/worklist/WorklistMapClickMonitor';
import RightSideContainer from '@/features/rightSideLayout/RightSideContainer';
import { useTenant } from '@/tenants';
import { exists, firstOrNull } from '@/utils';

import { defaultBounds, defaultLatLng } from './constants';
import BasemapToggle, { BasemapToggleEvent } from './leaflet/Control/BaseMapToggle/BasemapToggle';
import { BaseLayer, isVectorBasemap } from './leaflet/Control/BaseMapToggle/types';
import Control from './leaflet/Control/Control';
import LayersControl from './leaflet/Control/LayersControl/LayersControl';
import { initialEnabledLayers } from './leaflet/Control/LayersControl/LayersMenuLayout';
import { LegendControl } from './leaflet/Control/Legend/LegendControl';
import { PropertyQuickInfoContainer } from './leaflet/Control/Search/PropertyQuickInfoContainer';
import SearchControl from './leaflet/Control/SearchControl/SearchControl';
import WorklistControl from './leaflet/Control/WorklistControl/WorklistControl';
import { ZoomOutButton } from './leaflet/Control/ZoomOut/ZoomOutButton';
import { LocationPopupContainer } from './leaflet/LayerPopup/LocationPopupContainer';
import { FilePropertiesLayer } from './leaflet/Layers/FilePropertiesLayer';
import HighwayParcelsLayer from './leaflet/Layers/HighwayParcelsLayer';
import { LeafletLayerListener } from './leaflet/Layers/LeafletLayerListener';
import MapsearchParcelsLayer from './leaflet/Layers/MapsearchParcelsLayer';
import { MarkerLayer } from './leaflet/Layers/MarkerLayer';
import WorklistParcelsLayer from './leaflet/Layers/WorklistParcelsLayer';
import { MapEvents } from './leaflet/MapEvents/MapEvents';
import * as Styled from './leaflet/styles';
import { EsriVectorTileLayer } from './leaflet/VectorTileLayer/EsriVectorTileLayer';

export type MapLeafletViewProps = {
  parentWidth: number | undefined;
  defaultZoom?: number;
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
  defaultZoom,
}) => {
  const [baseLayers, setBaseLayers] = useState<BaseLayer[]>([]);
  const [activeBasemap, setActiveBasemap] = useState<BaseLayer | null>(null);
  const [zoom, setZoom] = useState(defaultZoom);
  const [isMapReady, setIsMapReady] = useState(false);

  const popupRef = useRef<LeafletPopup>(null);
  const mapRef = useRef<Map | null>(null);

  const [activeFeatureLayer, setActiveFeatureLayer] = useState<L.GeoJSON>();
  const { doubleClickInterval } = useTenant();

  const { parcels } = useWorklistContext();
  const isWorklistActive = useMemo(() => parcels?.length > 0, [parcels?.length]);

  // add geojson layer to the map
  if (!!mapRef.current && !activeFeatureLayer) {
    setActiveFeatureLayer(geoJSON().addTo(mapRef.current));
  }

  const timer = useRef(null);

  const handleMapClickEvent = (event: LeafletMouseEvent) => {
    if (timer?.current !== null) {
      return;
    }

    const latlng = event?.latlng;
    const isCtrlKeyPressed = event?.originalEvent?.ctrlKey ?? false;

    timer.current = setTimeout(() => {
      timer.current = null;
      // CTRL + click adds the clicked location to the property working list; otherwise treat it as a regular map-click
      if (isCtrlKeyPressed) {
        mapMachine.worklistMapClick(latlng);
      } else {
        mapMachine.mapClick(latlng);
      }
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

  // Initialize layers
  useEffect(() => {
    if (isMapReady) {
      setDefaultMapLayers(new Set(initialEnabledLayers));
    }
  }, [isMapReady, setDefaultMapLayers]);

  useEffect(() => {
    activeFeatureLayer?.clearLayers();

    if (isRepositioning) {
      const pimsFeature = repositioningFeatureDataset?.pimsFeature;
      if (exists(pimsFeature)) {
        // File marker repositioning is active - highlight the property and the corresponding boundary when user triggers the relocate action.
        activeFeatureLayer?.addData(pimsFeature);
      }
    } else if (exists(mapLocationFeatureDataset)) {
      if (firstOrNull(mapLocationFeatureDataset.parcelFeatures) !== null) {
        const activeFeature = mapLocationFeatureDataset.parcelFeatures[0];
        activeFeatureLayer?.addData(activeFeature);
      }
    }
  }, [activeFeatureLayer, isRepositioning, mapLocationFeatureDataset, repositioningFeatureDataset]);

  useEffect(() => {
    if (hasPendingFlyTo && isMapReady) {
      if (requestedFlyTo.bounds !== null) {
        mapRef?.current?.flyToBounds(requestedFlyTo.bounds, { animate: true });
      }
      if (requestedFlyTo.location !== null) {
        mapRef?.current?.flyTo(requestedFlyTo.location, MAP_MAX_ZOOM, {
          animate: true,
        });
      }

      mapMachineProcessFlyTo();
    }
  }, [isMapReady, hasPendingFlyTo, requestedFlyTo, mapMachineProcessFlyTo]);

  useEffect(() => {
    if (hasPendingCenterTo && isMapReady && exists(requestedCenterTo?.location)) {
      mapRef.current?.setView(requestedCenterTo.location);
    }
    mapMachineProcessCenterTo && mapMachineProcessCenterTo();
  }, [hasPendingCenterTo, isMapReady, mapMachineProcessCenterTo, requestedCenterTo?.location]);

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
      {baseLayers?.length > 0 && !mapMachine.mapSideBarViewState.isFullWidth && (
        <BasemapToggle baseLayers={baseLayers} onToggle={handleBasemapToggle} />
      )}

      <LeafletMapContainer
        center={[defaultLatLng.lat, defaultLatLng.lng]}
        zoom={zoom}
        maxZoom={MAP_MAX_ZOOM}
        closePopupOnClick={false}
        ref={handleMapCreated}
        doubleClickZoom
      >
        <MapEvents
          click={handleMapClickEvent}
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

        {mapMachine.showPopup && (
          // Draws the popup on top of the map
          <LocationPopupContainer ref={popupRef} />
        )}

        <LegendControl />
        <ZoomOutButton />
        <ScaleControl position="bottomleft" metric={true} imperial={false} />
        <Control position="topright">
          <Row noGutters className="flex-nowrap">
            <Col xs="auto">
              <LayersControl onToggle={mapMachine.toggleMapLayerControl} />
              <SearchControl onToggle={mapMachine.toggleMapSearchControl} />
              <WorklistControl
                active={isWorklistActive}
                onToggle={mapMachine.toggleWorkListControl}
              />
            </Col>
            <Col xs="auto">
              <RightSideContainer />
            </Col>
          </Row>
        </Control>

        <Control position="bottomright">
          <PropertyQuickInfoContainer />
        </Control>

        <MarkerLayer
          minZoom={MAP_MIN_MARKER_ZOOM}
          zoom={zoom}
          maxZoom={MAP_MAX_ZOOM}
          bounds={mapMachine.currentMapBounds ?? defaultBounds}
        />

        <Pane name="worklistParcels" style={{ zIndex: 500 }}>
          <WorklistParcelsLayer />
          <WorklistMapClickMonitor />
        </Pane>

        <Pane name="searchlistParcels" style={{ zIndex: 400 }}>
          <MapsearchParcelsLayer />
        </Pane>

        <Pane name="highwaylistParcels" style={{ zIndex: 300 }}>
          <HighwayParcelsLayer />
        </Pane>

        {/* Client-side "layer" to highlight file property boundaries (when in the context of a file) */}
        <Pane name="fileProperties" style={{ zIndex: 600 }}>
          <FilePropertiesLayer />
        </Pane>
      </LeafletMapContainer>
    </Styled.MapContainer>
  );
};

export default MapLeafletView;
