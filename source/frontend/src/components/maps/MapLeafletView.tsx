import axios from 'axios';
import {
  LatLng,
  LatLngBounds,
  LeafletEvent,
  LeafletMouseEvent,
  Map as LeafletMap,
  Popup as LeafletPopup,
  PopupEvent,
} from 'leaflet';
import isEqual from 'lodash/isEqual';
import React, { useContext, useEffect, useRef, useState } from 'react';
import { LayerGroup, MapContainer as ReactLeafletMap, TileLayer } from 'react-leaflet';
import { useDispatch, useSelector } from 'react-redux';
import { useResizeDetector } from 'react-resize-detector';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import { IGeoSearchParams } from '@/constants/API';
import { MAP_MAX_NATIVE_ZOOM, MAP_MAX_ZOOM } from '@/constants/strings';
import { IPropertyFilter } from '@/features/properties/filter/IPropertyFilter';
import { IProperty } from '@/interfaces';
import {
  DEFAULT_MAP_ZOOM,
  defaultBounds,
  defaultLatLng,
} from '@/store/slices/mapViewZoom/mapViewZoomSlice';
import { pidParser } from '@/utils/propertyUtils';

import { useMapStateMachine } from './hooks/MapStateMachineContext';
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
//import * as Styled from './leaflet/styles';
import { MapStateActionTypes, MapStateContext } from './providers/MapStateContext';

export type MapLeafletViewProps = {
  //showSideBar: boolean;
  parentWidth: number | undefined;
  geoFilter?: IGeoSearchParams;
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
  geoFilter,
}) => {
  const dispatch = useDispatch();

  const [baseLayers, setBaseLayers] = useState<BaseLayer[]>([]);

  const [activeBasemap, setActiveBasemap] = useState<BaseLayer | null>(null);
  //const smallScreen = useMediaQuery({ maxWidth: 1800 });

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

  const {
    setState,
    selectedInventoryProperty,
    loading: mapLoading,
    selectedFeature,
  } = useContext(MapStateContext);

  const mapRef = useRef<LeafletMap | null>(null);

  const parcelLayerFeature = selectedFeature;
  const { showLocationDetails } = useActiveFeatureLayer({
    selectedProperty: selectedInventoryProperty,
    layerPopup,
    mapRef,
    parcelLayerFeature,
    setLayerPopup,
  });

  const handleClickEvent = (latlng: LatLng) => {
    showLocationDetails(latlng);
  };

  const [zoom, setZoom] = useState(DEFAULT_MAP_ZOOM);

  const handleZoomUpdate = (zoomLevel: number) => {
    console.log(zoomLevel);
    setZoom(zoomLevel);
  };

  //const requestedZoom = useSelector(zoomRequest);
  const mapMachine = useMapStateMachine();
  const flyToPending = mapMachine.flyToPending;

  useEffect(() => {
    if (flyToPending !== null) {
      mapRef?.current?.flyTo(flyToPending, MAP_MAX_ZOOM, {
        animate: false,
      });

      //dispatch(zoomToPropery(null));
      mapMachine.processFlyTo();
    }
  }, [flyToPending]);

  // Todo: Verify that the resize is needed
  //const { width } = useResizeDetector();
  useEffect(() => {
    console.log('resized!');
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
      mapRef.current.fitBounds(defaultBounds);
    }
  };

  const handleMapReady = () => {
    console.log('when ready!');

    fitMapBounds();
  };

  const handleMapCreated = (mapInstance: L.Map) => {
    if (mapInstance !== null) {
      mapRef.current = mapInstance;
    }
  };

  const handleBounds = (event: LeafletEvent) => {
    console.log('handleBounds', event);
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
        center={[defaultLatLng.lat, defaultLatLng.lng]}
        zoom={DEFAULT_MAP_ZOOM}
        maxZoom={MAP_MAX_ZOOM}
        closePopupOnClick={true}
        ref={handleMapCreated}
        whenReady={handleMapReady}
      >
        <MapEvents
          click={e => handleClickEvent(e.latlng)}
          zoomend={e => handleZoomUpdate(e.sourceTarget.getZoom())}
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
        <ZoomOutButton />
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

const MapContainerStyled = styled.div`
  border: 5px solid red;
  /*width: 500px;
  height: 500px;*/
  /*width: 100%;
  height: 100%;*/
`;
