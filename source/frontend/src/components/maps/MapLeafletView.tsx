import axios from 'axios';
import { dequal } from 'dequal';
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
import { exists } from '@/utils';

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
import { useConfiguredMapLayers } from './leaflet/Layers/useConfiguredMapLayers';
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
  const layers = useConfiguredMapLayers();

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

  const hasPendingFlyTo = mapMachine.pendingFlyTo;
  const requestedFlyTo = mapMachine.requestedFlyTo;
  const mapMachineProcessFlyTo = mapMachine.processFlyTo;

  // Set the bounds when the map is ready. Not called from existing handleMapCreated as that function is called every time a state change occurs.
  useEffect(() => {
    const bounds = mapRef?.current?.getBounds();
    if (exists(bounds) && isMapReady && !dequal(bounds.getNorthEast(), bounds.getSouthWest())) {
      setBounds(bounds);
    }
  }, [isMapReady, setBounds]);

  useEffect(() => {
    if (isMapReady && mapMachinePendingRefresh && mapRef.current !== null) {
      // PSP-9347 it is possible that a fit bounds request will be made with an empty array of selected properties. In that case, we do not want to change the screen bounds, so cancel the request with no changes to the map.
      if (exists(mapMachineRequestedFitBounds)) {
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

  const { mapLocationFeatureDataset, repositioningFeatureDataset, isRepositioning } = mapMachine;

  useEffect(() => {
    activeFeatureLayer?.clearLayers();

    if (isRepositioning) {
      if (
        repositioningFeatureDataset !== null &&
        repositioningFeatureDataset.pimsFeature !== null
      ) {
        // File marker repositioning is active - highlight the property and the corresponding boundary when user triggers the relocate action.
        activeFeatureLayer?.addData(repositioningFeatureDataset.pimsFeature);
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
        if (mapLocationFeatureDataset.parcelFeature !== null) {
          activeFeature = mapLocationFeatureDataset.parcelFeature;
          activeFeatureLayer?.addData(activeFeature);
        } else if (mapLocationFeatureDataset.municipalityFeature !== null) {
          activeFeature = mapLocationFeatureDataset.municipalityFeature;
          if (mapLocationFeatureDataset.municipalityFeature?.geometry?.type === 'Polygon') {
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

  useEffect(() => {
    activeFeatureLayer?.clearLayers();
    if (
      mapMachine.mapFeatureData.fullyAttributedFeatures.features.length === 1 &&
      mapMachine.mapFeatureData.pimsLocationFeatures.features.length === 0 &&
      mapMachine.mapFeatureData.pimsBoundaryFeatures.features.length === 0
    ) {
      const searchFeature = mapMachine.mapFeatureData.fullyAttributedFeatures.features[0];
      if (activeFeatureLayer && searchFeature?.geometry?.type === 'Polygon') {
        activeFeatureLayer?.addData(searchFeature);
        const bounds = activeFeatureLayer.getBounds();
        mapRef?.current?.flyToBounds(bounds, { animate: false });
        mapMachineProcessFlyTo();
      }
    }
  }, [
    activeFeatureLayer,
    mapLocationFeatureDataset?.parcelFeature,
    mapMachine.mapFeatureData.fullyAttributedFeatures.features,
    mapMachine.mapFeatureData.fullyAttributedFeatures.features.length,
    mapMachine.mapFeatureData.pimsBoundaryFeatures.features.length,
    mapMachine.mapFeatureData.pimsLocationFeatures.features.length,
    mapMachineProcessFlyTo,
  ]);

  const handleMapReady = () => {
    mapMachine.setDefaultMapLayers(layers);
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
        <AdvancedFilterButton
          onToggle={mapMachine.toggleMapFilterDisplay}
          active={mapMachine.isFiltering}
        />
        <LayersControl onToggle={mapMachine.toggleMapLayerControl} />
        <InventoryLayer zoom={zoom} bounds={bounds} maxZoom={MAP_MAX_ZOOM}></InventoryLayer>
        <LeafletLayerListener />
      </ReactLeafletMap>
    </Styled.MapContainer>
  );
};

export default MapLeafletView;
