import './PointClusterer.scss';

import { AddressTypes } from 'constants/index';
import { PropertyTypes } from 'constants/propertyTypes';
import { MAX_ZOOM } from 'constants/strings';
import { BBox } from 'geojson';
import { useApiProperties } from 'hooks/pims-api';
import useDeepCompareEffect from 'hooks/useDeepCompareEffect';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { IProperty } from 'interfaces';
import L, { LatLngLiteral } from 'leaflet';
import queryString from 'query-string';
import React, { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { FeatureGroup, Marker, Polyline, useMap } from 'react-leaflet';
import { useLocation } from 'react-router-dom';
import { toast } from 'react-toastify';
import Supercluster from 'supercluster';

import useSupercluster from '../hooks/useSupercluster';
import { useFilterContext } from '../providers/FIlterProvider';
import { PropertyPopUpContext } from '../providers/PropertyPopUpProvider';
import { ICluster, PointFeature } from '../types';
import { getMarkerIcon, pointToLayer, zoomToCluster } from './mapUtils';
import SelectedPropertyMarker from './SelectedPropertyMarker/SelectedPropertyMarker';
import { Spiderfier } from './Spiderfier';

export type PointClustererProps = {
  points: Array<PointFeature>;
  bounds?: BBox;
  zoom: number;
  minZoom?: number;
  maxZoom?: number;
  /** When you click a cluster we zoom to its bounds. Default: true */
  zoomToBoundsOnClick?: boolean;
  /** When you click a cluster at the bottom zoom level we spiderfy it so you can see all of its markers. Default: true */
  spiderfyOnMaxZoom?: boolean;
  /** Action when a marker is clicked */
  onMarkerClick: (property: IProperty) => void;
  tilesLoaded: boolean;
};

/**
 * Converts the flat list of properties into the correct type of inventory property.
 * @param property A flat list of property values (from a Feature).
 */
export const convertToProperty = (
  property: any,
  latitude?: number,
  longitude?: number,
): IProperty | null => {
  if ([PropertyTypes.Land, PropertyTypes.Subdivision].includes(property.propertyTypeId)) {
    return {
      pid: property.PID,
      latitude: latitude,
      longitude: longitude,
      propertyTypeId: property.propertyTypeId,
      address: {
        id: property.ADDRESS_ID,
        addressTypeId: AddressTypes.Mailing,
        municipality: property.MUNICIPALITY_NAME,
        provinceId: 1,
        province: property.PROVINCE_STATE_CODE,
        streetAddress1: property.STREET_ADDRESS_1,
        postal: property.POSTAL_CODE,
        country: property.COUNTRY_CODE,
      },
      pin: property.PIN,
      landArea: property.LAND_AREA,
      landLegalDescription: property.LAND_LEGAL_DESCRIPTION,
      name: property.NAME,
      description: property.DESCRIPTION,
      isSensitive: property.IS_SENSITIVE,
      isOwned: property.IS_OWNED,
      encumbranceReason: property.ENCUMBRANCE_REASON,
      isPropertyOfInterest: property.IS_PROPERTY_OF_INTEREST,
      isVisibleToOtherAgencies: property.IS_VISIBLE_TO_OTHER_AGENCIES,
      areaUnit: property.PROPERTY_AREA_UNIT_TYPE_CODE,
      classificationId: property.PROPERTY_CLASSIFICATION_TYPE_CODE,
      id: property.PROPERTY_ID,
      status: property.PROPERTY_STATUS_TYPE_CODE,
      tenure: property.PROPERTY_TENURE_TYPE_CODE,
      regionId: property.REGION_CODE,
      zoning: property.ZONING,
      zoningPotential: property.ZONING_POTENTIAL,
    };
  } else if (
    [PropertyTypes.DraftBuilding, PropertyTypes.DraftLand].includes(property.propertyTypeId)
  ) {
    return property;
  }
  return null;
};

/**
 * Clusters pins that are close together geographically based on the zoom level into a single clustered object.
 * @param param0 Point cluster properties.
 */
export const PointClusterer: React.FC<PointClustererProps> = ({
  points,
  bounds,
  zoom,
  onMarkerClick,
  minZoom,
  maxZoom,
  zoomToBoundsOnClick = true,
  spiderfyOnMaxZoom = true,
  tilesLoaded,
}) => {
  // state and refs
  const spiderfierRef = useRef<Spiderfier>();
  const featureGroupRef = useRef<L.FeatureGroup>(null);
  const filterState = useFilterContext();
  const location = useLocation();
  const { parcelId, buildingId } = queryString.parse(location.search);
  const popUpContext = React.useContext(PropertyPopUpContext);
  const { propertyInfo: selected } = popUpContext;

  const [currentSelected, setCurrentSelected] = useState(selected);
  const [currentCluster, setCurrentCluster] = useState<
    ICluster<any, Supercluster.AnyProps> | undefined
  >(undefined);

  const mapInstance: L.Map = useMap();
  if (!mapInstance) {
    throw new Error('<PointClusterer /> must be used under a <Map> leaflet component');
  }

  minZoom = minZoom ?? 0;
  maxZoom = maxZoom ?? 18;

  const [spider, setSpider] = useState<any>({});

  // get clusters
  // clusters are an array of GeoJSON Feature objects, but some of them
  // represent a cluster of points, and some represent individual points.
  const { clusters, supercluster } = useSupercluster({
    points,
    bounds,
    zoom,
    options: { radius: 60, extent: 256, minZoom, maxZoom, enableClustering: !filterState.changed },
  });
  const currentClusterIds = useMemo(() => {
    if (!currentCluster?.properties?.cluster_id) {
      return [];
    }
    try {
      const clusteredPoints =
        supercluster?.getLeaves(currentCluster?.properties?.cluster_id, Infinity) ?? [];
      return clusteredPoints.map(p => p.properties.id);
    } catch (error) {
      return [];
    }
  }, [currentCluster, supercluster]);

  //Optionally create a new pin to represent the active property if not already displayed in a spiderfied cluster.
  useDeepCompareEffect(() => {
    if (!currentClusterIds.includes(+(selected?.id ?? 0))) {
      setCurrentSelected(selected);
      if (!!parcelId && !!selected) {
        mapInstance.setView(
          [selected?.latitude as number, selected?.longitude as number],
          Math.max(MAX_ZOOM, mapInstance.getZoom()),
        );
      }
    } else {
      setCurrentSelected(null);
    }
  }, [selected, setCurrentSelected]);

  // Register event handlers to shrink and expand clusters when map is interacted with
  const componentDidMount = useCallback(() => {
    if (!spiderfierRef.current) {
      spiderfierRef.current = new Spiderfier(mapInstance, {
        getClusterId: cluster => cluster?.properties?.cluster_id as number,
        getClusterPoints: clusterId => supercluster?.getLeaves(clusterId, Infinity) ?? [],
        pointToLayer: pointToLayer,
      });
    }

    const spiderfier = spiderfierRef.current;

    mapInstance.on('click', spiderfier.unspiderfy, spiderfier);
    mapInstance.on('zoomstart', spiderfier.unspiderfy, spiderfier);
    mapInstance.on('clear', spiderfier.unspiderfy, spiderfier);

    // cleanup function
    return function componentWillUnmount() {
      mapInstance.off('click', spiderfier.unspiderfy, spiderfier);
      mapInstance.off('zoomstart', spiderfier.unspiderfy, spiderfier);
      mapInstance.off('clear', spiderfier.unspiderfy, spiderfier);
      spiderfierRef.current = undefined;
    };
  }, [mapInstance, supercluster]);

  useEffect(componentDidMount, [componentDidMount]);

  // on-click handler
  const zoomOrSpiderfy = useCallback(
    (cluster: ICluster) => {
      if (!supercluster || !spiderfierRef.current || !cluster) {
        return;
      }
      const { cluster_id } = cluster.properties;
      const expansionZoom = Math.min(
        supercluster.getClusterExpansionZoom(cluster_id as number),
        maxZoom as number,
      );

      // already at maxZoom, need to spiderfy child markers
      if (expansionZoom === maxZoom && spiderfyOnMaxZoom) {
        const res = spiderfierRef.current.spiderfy(cluster);
        setSpider(res);
        if (res.markers === undefined) {
          setCurrentCluster(undefined);
        } else {
          setCurrentCluster(cluster);
        }
      } else if (zoomToBoundsOnClick) {
        zoomToCluster(cluster, expansionZoom, mapInstance);
      }
    },
    [spiderfierRef, mapInstance, maxZoom, spiderfyOnMaxZoom, supercluster, zoomToBoundsOnClick],
  );

  /**
   * Update the map bounds and zoom to make all property clusters visible.
   */
  useDeepCompareEffect(() => {
    if (featureGroupRef.current) {
      const group: L.FeatureGroup = featureGroupRef.current;
      const groupBounds = group.getBounds();

      if (groupBounds.isValid() && filterState.changed && !selected && tilesLoaded) {
        filterState.setChanged(false);
        mapInstance.fitBounds(groupBounds, { maxZoom: zoom > MAX_ZOOM ? zoom : MAX_ZOOM });
      }

      setSpider({});
      spiderfierRef.current?.unspiderfy();
      setCurrentCluster(undefined);
    }
  }, [featureGroupRef, mapInstance, clusters, tilesLoaded]);

  const { getProperty } = useApiProperties();
  const fetchProperty = React.useCallback(
    (id: number, latLng: LatLngLiteral) => {
      popUpContext.setLoading(true);
      getProperty(id)
        .then(apiProperty => {
          const property: IProperty = {
            ...apiProperty.data,
            latitude: latLng.lat,
            longitude: latLng.lng,
          };
          popUpContext.setPropertyInfo(property);
        })
        .catch(() => {
          toast.error('Unable to load property details, refresh the page and try again.');
        })
        .finally(() => {
          popUpContext.setLoading(false);
        });
    },
    [getProperty, popUpContext],
  );

  const keycloak = useKeycloakWrapper();

  return (
    <>
      <FeatureGroup ref={featureGroupRef}>
        {/**
         * Render all visible clusters
         */}
        {clusters.map((cluster, index) => {
          // every cluster point has coordinates
          const [longitude, latitude] = cluster.geometry.coordinates;
          const {
            cluster: isCluster,
            point_count: pointCount,
            point_count_abbreviated,
          } = cluster.properties as any;
          const size = pointCount < 100 ? 'small' : pointCount < 1000 ? 'medium' : 'large';

          // we have a cluster to render
          if (isCluster) {
            return (
              // render the cluster marker
              <Marker
                key={index}
                position={[latitude, longitude]}
                eventHandlers={{
                  click: e => {
                    zoomOrSpiderfy(cluster);
                    e.target.closePopup();
                  },
                }}
                icon={
                  new L.DivIcon({
                    html: `<div><span>${point_count_abbreviated}</span></div>`,
                    className: `marker-cluster marker-cluster-${size}`,
                    iconSize: [40, 40],
                  })
                }
              />
            );
          }

          return (
            // render single marker, not in a cluster
            <Marker
              {...(cluster.properties as any)}
              key={index}
              position={[latitude, longitude]}
              icon={getMarkerIcon(cluster)}
              eventHandlers={{
                click: () => {
                  const convertedProperty = convertToProperty(
                    cluster.properties,
                    latitude,
                    longitude,
                  );
                  convertedProperty && onMarkerClick(convertedProperty); //open information slideout
                  if (keycloak.canUserViewProperty(cluster.properties as IProperty)) {
                    convertedProperty?.id
                      ? fetchProperty(convertedProperty.id, {
                          lat: latitude,
                          lng: longitude,
                        })
                      : toast.dark('This property is invalid, unable to view details');
                  } else {
                    popUpContext.setPropertyInfo(convertedProperty);
                  }
                },
              }}
            />
          );
        })}
        {/**
         * Render markers from a spiderfied cluster click
         */}
        {spider.markers?.map((m: any, index: number) => (
          <Marker
            {...m.properties}
            key={index}
            position={m.position}
            //highlight pin if currently selected
            icon={getMarkerIcon(m, (m.properties.id as number) === (selected?.id as number))}
            eventHandlers={{
              click: () => {
                //sets this pin as currently selected
                const convertedProperty = convertToProperty(
                  m.properties,
                  m.geometry.coordinates[1],
                  m.geometry.coordinates[0],
                );

                if (keycloak.canUserViewProperty(m.properties as IProperty)) {
                  convertedProperty?.id && convertedProperty.propertyTypeId
                    ? fetchProperty(convertedProperty.id, {
                        lat: convertedProperty.latitude ?? 0,
                        lng: convertedProperty.longitude ?? 0,
                      })
                    : toast.dark('This property is invalid, unable to view details');
                  convertedProperty && onMarkerClick(convertedProperty); //open information slideout
                }
              },
            }}
          />
        ))}
        {/**
         * Render lines/legs from a spiderfied cluster click
         */}
        {spider.lines?.map((m: any, index: number) => (
          <Polyline key={index} positions={m.coords} {...m.options} />
        ))}
        {/**
         * render selected property marker, auto opens the property popup
         */}
        {!!selected &&
          selected?.id === currentSelected?.id &&
          !currentClusterIds.includes(+(selected?.id as number)) && (
            <SelectedPropertyMarker
              {...selected}
              icon={getMarkerIcon({ properties: selected } as any, true)}
              className={Number(parcelId ?? buildingId) === selected?.id ? 'active-selected' : ''}
              position={[selected.latitude as number, selected.longitude as number]}
              map={mapInstance}
              eventHandlers={{
                click: () => {
                  popUpContext.setPropertyInfo(selected);
                  selected && onMarkerClick(selected);
                },
              }}
            />
          )}
      </FeatureGroup>
    </>
  );
};

export default PointClusterer;
