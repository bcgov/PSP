import './Map.scss';

import axios from 'axios';
import classNames from 'classnames';
import { useLayerQuery } from 'components/maps/leaflet/LayerPopup';
import { IGeoSearchParams } from 'constants/API';
import { MAP_MAX_ZOOM } from 'constants/strings';
import { SidebarSize } from 'features/mapSideBar/hooks/useQueryParamSideBar';
import { PropertyFilter } from 'features/properties/filter';
import { IPropertyFilter } from 'features/properties/filter/IPropertyFilter';
import { Feature } from 'geojson';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { IProperty } from 'interfaces';
import { geoJSON, LatLng, LatLngBounds, LeafletMouseEvent, Map as LeafletMap } from 'leaflet';
import isEmpty from 'lodash/isEmpty';
import isEqual from 'lodash/isEqual';
import isEqualWith from 'lodash/isEqualWith';
import React, { useEffect, useRef, useState } from 'react';
import Col from 'react-bootstrap/Col';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import { MapContainer as ReactLeafletMap, Popup, TileLayer } from 'react-leaflet';
import { useDispatch } from 'react-redux';
import { useResizeDetector } from 'react-resize-detector';
import { useMediaQuery } from 'react-responsive';
import { useAppSelector } from 'store/hooks';
import { ILookupCode } from 'store/slices/lookupCodes';
import { DEFAULT_MAP_ZOOM, setMapViewZoom } from 'store/slices/mapViewZoom/mapViewZoomSlice';
import { saveParcelLayerData } from 'store/slices/parcelLayerData/parcelLayerDataSlice';
import { IPropertyDetail, storeProperty } from 'store/slices/properties';
import { floatOrUndefined } from 'utils';

import { Claims } from '../../../constants';
import BasemapToggle, { BaseLayer, BasemapToggleEvent } from '../BasemapToggle';
import useActiveFeatureLayer from '../hooks/useActiveFeatureLayer';
import { useFilterContext } from '../providers/FIlterProvider';
import { PropertyPopUpContextProvider } from '../providers/PropertyPopUpProvider';
import FilterBackdrop from './FilterBackdrop';
import InfoSlideOut from './InfoSlideOut/InfoSlideOut';
import { InventoryLayer } from './InventoryLayer';
import {
  MUNICIPALITY_LAYER_URL,
  municipalityLayerPopupConfig,
  parcelLayerPopupConfig,
  PARCELS_LAYER_URL,
} from './LayerPopup/constants';
import {
  LayerPopupContent,
  LayerPopupTitle,
  PopupContentConfig,
} from './LayerPopup/LayerPopupContent';
import LayersControl from './LayersControl';
import { LegendControl } from './Legend/LegendControl';
import { MapEvents } from './MapEvents/MapEvents';
import { ZoomOutButton } from './ZoomOut/ZoomOutButton';

export type MapViewportChangeEvent = {
  bounds: LatLngBounds | null;
  filter?: IGeoSearchParams;
};

export type MapProps = {
  lat: number;
  lng: number;
  zoom?: number;
  properties: IProperty[];
  organizations: ILookupCode[];
  administrativeAreas: ILookupCode[];
  selectedProperty?: IPropertyDetail | null;
  onViewportChanged?: (e: MapViewportChangeEvent) => void;
  onMapClick?: (e: LeafletMouseEvent) => void;
  disableMapFilterBar?: boolean;
  interactive?: boolean;
  showParcelBoundaries?: boolean;
  sidebarSize?: SidebarSize;
  whenCreated?: (map: LeafletMap) => void;
  whenReady?: () => void;
};

export type LayerPopupInformation = PopupContentConfig & {
  latlng: LatLng;
  title: string;
  center?: LatLng;
  bounds?: LatLngBounds;
  feature: Feature;
};

const defaultFilterValues: IPropertyFilter = {
  searchBy: 'address',
  pid: '',
  address: '',
  municipality: '',
  organizations: '',
  minLotSize: '',
  maxLotSize: '',
  name: '',
};

const whitelistedFilterKeys = [
  'pid',
  'address',
  'municipality',
  'classificationId',
  'organizations',
  'minLandArea',
  'maxLandArea',
  'name',
];

/**
 * Converts the map filter to a geo search filter.
 * @param filter The map filter.
 */
const getQueryParams = (filter: IPropertyFilter): IGeoSearchParams => {
  return {
    pid: filter.pid,
    address: filter.address,
    municipality: filter.municipality,
    classificationId: filter.classificationId,
    organizations: filter.organizations,
    minLandArea: floatOrUndefined(filter.minLotSize),
    maxLandArea: floatOrUndefined(filter.maxLotSize),
    name: filter.name,
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
  organizations,
  administrativeAreas,
  selectedProperty,
  onMapClick,
  disableMapFilterBar,
  sidebarSize,
  whenReady,
  whenCreated,
}) => {
  const keycloak = useKeycloakWrapper();
  const dispatch = useDispatch();
  const [geoFilter, setGeoFilter] = useState<IGeoSearchParams>({
    ...defaultFilterValues,
    includeAllProperties: keycloak.hasClaim(Claims.ADMIN_PROPERTIES),
  } as any);
  const [baseLayers, setBaseLayers] = useState<BaseLayer[]>([]);
  const [triggerFilterChanged, setTriggerFilterChanged] = useState(true);
  const [activeBasemap, setActiveBasemap] = useState<BaseLayer | null>(null);
  const smallScreen = useMediaQuery({ maxWidth: 1800 });
  const municipalitiesService = useLayerQuery(MUNICIPALITY_LAYER_URL);
  const parcelsService = useLayerQuery(PARCELS_LAYER_URL);
  const [bounds, setBounds] = useState<LatLngBounds>(defaultBounds);
  const { setChanged } = useFilterContext();
  const [layerPopup, setLayerPopup] = useState<LayerPopupInformation>();

  // a reference to the internal Leaflet map instance (this is NOT a react-leaflet class but the underlying leaflet map)
  const mapRef = useRef<LeafletMap | null>(null);

  if (mapRef.current && !selectedProperty?.propertyDetail) {
    const center = mapRef.current.getCenter();
    lat = center.lat;
    lng = center.lng;
  }

  const parcelLayerFeature = useAppSelector(state => state.parcelLayerData?.parcelLayerFeature);
  useActiveFeatureLayer({
    selectedProperty,
    layerPopup,
    mapRef,
    parcelLayerFeature,
    setLayerPopup,
  });
  const [showFilterBackdrop, setShowFilterBackdrop] = useState(true);

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
    if (!isEqualWith(geoFilter, filter, compareValues) || triggerFilterChanged) {
      dispatch(storeProperty(null));
      setGeoFilter(getQueryParams(filter));
      setChanged(true);
      setTriggerFilterChanged(false);
    }
  };

  const handleBasemapToggle = (e: BasemapToggleEvent) => {
    const { previous, current } = e;
    setBaseLayers([current, previous]);
    setActiveBasemap(current);
  };

  useEffect(() => {
    // fetch GIS base layers configuration from /public folder
    axios.get('/basemaps.json')?.then(result => {
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

  const showLocationDetails = async (event: LeafletMouseEvent) => {
    !!onMapClick && onMapClick(event);
    const municipality = await municipalitiesService.findOneWhereContains(event.latlng);
    const parcel = await parcelsService.findOneWhereContains(event.latlng);

    let properties = {};
    let center: LatLng | undefined;
    let bounds: LatLngBounds | undefined;
    let displayConfig = {};
    let title = 'Municipality Information';
    let feature = {};
    if (municipality?.features?.length === 1) {
      properties = municipality.features[0].properties!;
      displayConfig = municipalityLayerPopupConfig;
      feature = municipality.features[0];
      bounds = municipality.features[0]?.geometry
        ? geoJSON(municipality.features[0].geometry).getBounds()
        : undefined;
    }

    if (parcel?.features?.length === 1) {
      title = 'Parcel Information';
      properties = parcel.features[0].properties!;
      displayConfig = parcelLayerPopupConfig;
      bounds = parcel.features[0]?.geometry
        ? geoJSON(parcel.features[0].geometry).getBounds()
        : undefined;
      center = bounds?.getCenter();
      feature = parcel.features[0];
    }

    if (!isEmpty(properties)) {
      setLayerPopup({
        title,
        data: properties as any,
        config: displayConfig as any,
        latlng: event.latlng,
        center,
        bounds,
        feature,
      } as any);
    }
  };

  const handleBounds = (e: any) => {
    const boundsData: LatLngBounds = e.target.getBounds();
    if (!isEqual(boundsData.getNorthEast(), boundsData.getSouthWest())) {
      setBounds(boundsData);
    }
  };

  const property = useAppSelector(state => state.properties.propertyDetail);
  const [infoOpen, setInfoOpen] = React.useState(!!property);
  const [layersOpen, setLayersOpen] = React.useState(false);
  return (
    <Container
      ref={resizeRef}
      fluid
      className={classNames('px-0 map', { narrow: sidebarSize === 'narrow' })}
    >
      <FilterBackdrop show={showFilterBackdrop} />
      {!disableMapFilterBar ? (
        <Container fluid className="px-0 map-filter-container">
          <Container className="px-0">
            <PropertyFilter
              defaultFilter={{
                ...defaultFilterValues,
              }}
              organizationLookupCodes={organizations}
              adminAreaLookupCodes={administrativeAreas}
              onChange={handleMapFilterChange}
              setTriggerFilterChanged={setTriggerFilterChanged}
              showAllOrganizationSelect={true}
            />
          </Container>
        </Container>
      ) : null}
      <Row noGutters>
        <Col>
          {baseLayers?.length > 0 && (
            <BasemapToggle baseLayers={baseLayers} onToggle={handleBasemapToggle} />
          )}
          <PropertyPopUpContextProvider>
            <ReactLeafletMap
              center={[lat, lng]}
              zoom={lastZoom}
              maxZoom={MAP_MAX_ZOOM}
              closePopupOnClick={true}
              whenCreated={handleMapCreated}
              whenReady={handleMapReady}
            >
              <MapEvents
                click={showLocationDetails}
                zoomend={e => setZoom(e.sourceTarget.getZoom())}
                moveend={handleBounds}
              />
              {activeBasemap && (
                <TileLayer
                  attribution={activeBasemap.attribution}
                  url={activeBasemap.url}
                  zIndex={0}
                />
              )}
              {!!layerPopup && (
                <Popup
                  position={layerPopup.latlng}
                  offset={[0, -25]}
                  onClose={() => {
                    setLayerPopup(undefined);
                    dispatch(storeProperty(null));
                  }}
                  closeButton={true}
                  autoPan={false}
                >
                  <LayerPopupTitle>{layerPopup.title}</LayerPopupTitle>
                  <LayerPopupContent
                    data={layerPopup.data as any}
                    config={layerPopup.config as any}
                    center={layerPopup.center}
                    onAddToParcel={(e: MouseEvent, data: { [key: string]: any }) => {
                      dispatch(
                        saveParcelLayerData({
                          e: { timeStamp: document?.timeline?.currentTime ?? 0 } as any,
                          data: {
                            ...data,
                            CENTER: { lat: data?.CENTER.lat, lng: data?.CENTER.lng },
                          },
                        }),
                      );
                    }}
                    bounds={layerPopup.bounds}
                  />
                </Popup>
              )}
              <LegendControl />
              <ZoomOutButton bounds={defaultBounds} />
              <LayersControl
                open={layersOpen}
                setOpen={() => {
                  setLayersOpen(!layersOpen);
                  setInfoOpen(false);
                }}
              />
              <InfoSlideOut
                open={infoOpen}
                setOpen={(state: boolean) => {
                  setInfoOpen(state);
                  setLayersOpen(false);
                }}
                onHeaderActionClick={() => {
                  setInfoOpen(false);
                }}
              />
              <InventoryLayer
                zoom={zoom}
                bounds={bounds}
                onMarkerClick={() => {
                  if (!infoOpen) {
                    setLayersOpen(false);
                    setInfoOpen(true);
                  }
                }}
                selected={selectedProperty}
                filter={geoFilter}
                onRequestData={setShowFilterBackdrop}
              ></InventoryLayer>
            </ReactLeafletMap>
          </PropertyPopUpContextProvider>
        </Col>
      </Row>
    </Container>
  );
};

export default Map;
