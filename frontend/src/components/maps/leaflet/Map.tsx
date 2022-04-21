import axios from 'axios';
import classNames from 'classnames';
import { IGeoSearchParams } from 'constants/API';
import { MAP_MAX_ZOOM } from 'constants/strings';
import { PropertyFilter } from 'features/properties/filter';
import { IPropertyFilter } from 'features/properties/filter/IPropertyFilter';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { IProperty } from 'interfaces';
import { LatLngBounds, Map as LeafletMap, TileLayer as LeafletTileLayer } from 'leaflet';
import isEqual from 'lodash/isEqual';
import isEqualWith from 'lodash/isEqualWith';
import React, { useContext, useEffect, useRef, useState } from 'react';
import Container from 'react-bootstrap/Container';
import { MapContainer as ReactLeafletMap, TileLayer } from 'react-leaflet';
import { useDispatch } from 'react-redux';
import { useResizeDetector } from 'react-resize-detector';
import { useMediaQuery } from 'react-responsive';
import { useAppSelector } from 'store/hooks';
import { DEFAULT_MAP_ZOOM, setMapViewZoom } from 'store/slices/mapViewZoom/mapViewZoomSlice';
import styled from 'styled-components';

import { Claims } from '../../../constants';
import BasemapToggle, { BaseLayer, BasemapToggleEvent } from '../BasemapToggle';
import useActiveFeatureLayer from '../hooks/useActiveFeatureLayer';
import { useFilterContext } from '../providers/FIlterProvider';
import { SelectedPropertyContext } from '../providers/SelectedPropertyContext';
import { InventoryLayer } from './InventoryLayer';
import { LayerPopup, LayerPopupInformation } from './LayerPopup';
import LayersControl from './LayersControl';
import { LegendControl } from './Legend/LegendControl';
import LoadingBackdrop from './LoadingBackdrop/LoadingBackdrop';
import { MapEvents } from './MapEvents/MapEvents';
import * as Styled from './styles';
import { ZoomOutButton } from './ZoomOut/ZoomOutButton';

export type MapViewportChangeEvent = {
  bounds: LatLngBounds | null;
  filter?: IGeoSearchParams;
};

export type MapProps = {
  lat: number;
  lng: number;
  zoom?: number;
  onViewportChanged?: (e: MapViewportChangeEvent) => void;
  disableMapFilterBar?: boolean;
  showSideBar: boolean;
  showParcelBoundaries?: boolean;
  whenCreated?: (map: LeafletMap) => void;
  whenReady?: () => void;
  onPropertyMarkerClick: (property: IProperty) => void;
  onViewPropertyClick: (pid?: string | null) => void;
};

type BaseLayerFile = {
  basemaps: BaseLayer[];
};

const defaultFilterValues: IPropertyFilter = {
  searchBy: 'pinOrPid',
  pinOrPid: '',
  address: '',
};

const whitelistedFilterKeys = ['PID', 'PIN', 'STREET_ADDRESS_1', 'LOCATION'];

/**
 * Converts the map filter to a geo search filter.
 * @param filter The map filter.
 */
const getQueryParams = (filter: IPropertyFilter): IGeoSearchParams => {
  // The map will search for either identifier.
  const pinOrPidValue = filter.pinOrPid ? filter.pinOrPid?.replace(/-/g, '') : undefined;
  return {
    PID: pinOrPidValue,
    PIN: undefined,
    STREET_ADDRESS_1: filter.address,
  };
};

const defaultBounds = new LatLngBounds([60.09114547, -119.49609429], [48.78370426, -139.35937554]);

/**
 * Creates a Leaflet map and by default includes a number of preconfigured layers.
 * @param param0
 */
const Map: React.FC<MapProps> = ({
  lat,
  lng,
  zoom: zoomProp,
  showSideBar,
  whenReady,
  whenCreated,
  disableMapFilterBar,
  onPropertyMarkerClick,
  onViewPropertyClick,
}) => {
  const keycloak = useKeycloakWrapper();
  const dispatch = useDispatch();
  const [geoFilter, setGeoFilter] = useState<IGeoSearchParams>({
    ...defaultFilterValues,
    includeAllProperties: keycloak.hasClaim(Claims.ADMIN_PROPERTIES),
  } as any); // Todo: remove type coercion
  const [baseLayers, setBaseLayers] = useState<BaseLayer[]>([]);
  const [triggerFilterChanged, setTriggerFilterChanged] = useState(true);
  const [activeBasemap, setActiveBasemap] = useState<BaseLayer | null>(null);
  const smallScreen = useMediaQuery({ maxWidth: 1800 });

  const [bounds, setBounds] = useState<LatLngBounds>(defaultBounds);
  const { setChanged } = useFilterContext();
  const [layerPopup, setLayerPopup] = useState<LayerPopupInformation>();

  // a reference to the internal Leaflet map instance (this is NOT a react-leaflet class but the underlying leaflet map)
  const mapRef = useRef<LeafletMap | null>(null);
  // a reference to the basemap tile layer since the layer url is immutable
  const tileRef = useRef<LeafletTileLayer>(null);

  const { setPropertyInfo, propertyInfo, selectedFeature } = useContext(SelectedPropertyContext);

  if (mapRef.current && !propertyInfo) {
    const center = mapRef.current.getCenter();
    lat = center.lat;
    lng = center.lng;
  }

  const parcelLayerFeature = selectedFeature;
  const { showLocationDetails } = useActiveFeatureLayer({
    selectedProperty: propertyInfo,
    layerPopup,
    mapRef,
    parcelLayerFeature,
    setLayerPopup,
  });
  const [showLoadingBackdrop, setShowLoadingBackdrop] = useState(true);

  const lastZoom = useAppSelector(state => state.mapViewZoom) ?? zoomProp;
  const [zoom, setZoom] = useState(lastZoom);
  useEffect(() => {
    if (lastZoom === DEFAULT_MAP_ZOOM) {
      dispatch(setMapViewZoom(smallScreen ? 4.9 : 5.5));
    } else if (lastZoom !== zoom && zoom !== DEFAULT_MAP_ZOOM) {
      dispatch(setMapViewZoom(zoom));
    }
  }, [dispatch, lastZoom, smallScreen, zoom]);

  const { width, ref: resizeRef } = useResizeDetector();
  useEffect(() => {
    mapRef.current?.invalidateSize();
  }, [mapRef, width]);

  const handleMapFilterChange = async (filter: IPropertyFilter) => {
    const compareValues = (objValue: any, othValue: any) => {
      return whitelistedFilterKeys.reduce((acc, key) => {
        return (isEqual(objValue[key], othValue[key]) || (!objValue[key] && !othValue[key])) && acc;
      }, true);
    };
    // Search button will always trigger filter changed (triggerFilterChanged is set to true when search button is clicked)
    if (!isEqualWith(geoFilter, getQueryParams(filter), compareValues) || triggerFilterChanged) {
      setPropertyInfo(null);
      setGeoFilter(getQueryParams(filter));
      setChanged(true);
      setTriggerFilterChanged(false);
    }
  };

  const handleBasemapToggle = (e: BasemapToggleEvent) => {
    const { previous, current } = e;
    setBaseLayers([current, previous]);
    setActiveBasemap(current);
    tileRef?.current?.setUrl(current.url);
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

  const handleBounds = (e: any) => {
    const boundsData: LatLngBounds = e.target.getBounds();
    if (!isEqual(boundsData.getNorthEast(), boundsData.getSouthWest())) {
      setBounds(boundsData);
    }
  };

  const [layersOpen, setLayersOpen] = React.useState(false);

  return (
    <Styled.MapGrid ref={resizeRef} className={classNames('px-0', 'map', { sidebar: showSideBar })}>
      <LoadingBackdrop show={showLoadingBackdrop} />
      {!showSideBar && !disableMapFilterBar ? (
        <StyledFilterContainer fluid className="px-0">
          <PropertyFilter
            defaultFilter={{
              ...defaultFilterValues,
            }}
            onChange={handleMapFilterChange}
            setTriggerFilterChanged={setTriggerFilterChanged}
          />
        </StyledFilterContainer>
      ) : null}
      <Styled.MapContainer>
        {baseLayers?.length > 0 && (
          <BasemapToggle baseLayers={baseLayers} onToggle={handleBasemapToggle} />
        )}

        <ReactLeafletMap
          center={[lat, lng]}
          zoom={lastZoom}
          maxZoom={MAP_MAX_ZOOM}
          closePopupOnClick={true}
          whenCreated={handleMapCreated}
          whenReady={handleMapReady}
        >
          <MapEvents
            click={e => showLocationDetails(e.latlng)}
            zoomend={e => setZoom(e.sourceTarget.getZoom())}
            moveend={handleBounds}
          />
          {activeBasemap && (
            <TileLayer
              ref={tileRef}
              attribution={activeBasemap.attribution}
              url={activeBasemap.url}
              zIndex={0}
            />
          )}
          {!!layerPopup && (
            <LayerPopup
              layerPopup={layerPopup}
              onViewPropertyInfo={onViewPropertyClick}
              onClose={() => {
                setLayerPopup(undefined);
                setPropertyInfo(null);
              }}
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
              onPropertyMarkerClick(property);
            }}
            filter={geoFilter}
            onRequestData={setShowLoadingBackdrop}
          ></InventoryLayer>
        </ReactLeafletMap>
      </Styled.MapContainer>
    </Styled.MapGrid>
  );
};

export default Map;

const StyledFilterContainer = styled(Container)`
  transition: margin 1s;

  grid-area: filter;
  background-color: #f2f2f2;
  box-shadow: 0px 4px 5px rgba(0, 0, 0, 0.2);
  z-index: 500;
  .map-filter-bar {
    align-items: center;
    justify-content: center;
    padding: 0.5rem 0;
    .vl {
      border-left: 6px solid rgba(96, 96, 96, 0.2);
      height: 4rem;
      margin-left: 1%;
      margin-right: 1%;
      border-width: 0.2rem;
    }
    .btn-primary {
      color: white;
      font-weight: bold;
      height: 3.5rem;
      width: 3.5rem;
      min-height: unset;
      padding: 0;
    }
    .form-control {
      font-size: 1.4rem;
    }
  }
  .form-group {
    margin-bottom: 0;
  }
`;
