import './PointClusterer.scss';

import { MAX_ZOOM } from 'constants/strings';
import { BBox } from 'geojson';
import { useApiProperties } from 'hooks/pims-api';
import useDeepCompareEffect from 'hooks/useDeepCompareEffect';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { IProperty } from 'interfaces';
import L, { LatLng, LatLngLiteral } from 'leaflet';
import { find } from 'lodash';
import React, { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { FeatureGroup, Marker, Polyline, useMap } from 'react-leaflet';
import { toast } from 'react-toastify';
import Supercluster from 'supercluster';

import useSupercluster from '../hooks/useSupercluster';
import { useFilterContext } from '../providers/FIlterProvider';
import { SelectedPropertyContext } from '../providers/SelectedPropertyContext';
import { ICluster, PointFeature } from '../types';
import { getDraftIcon, getMarkerIcon, pointToLayer, zoomToCluster } from './mapUtils';
import SelectedPropertyMarker from './SelectedPropertyMarker/SelectedPropertyMarker';
import { Spiderfier } from './Spiderfier';

export type PointClustererProps = {
  points: Array<PointFeature>;
  draftPoints: Array<PointFeature>;
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
  return {
    pid: property.PID,
    latitude: latitude,
    longitude: longitude,
    address: {
      id: property.ADDRESS_ID,
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
};

/**
 * Clusters pins that are close together geographically based on the zoom level into a single clustered object.
 * @param param0 Point cluster properties.
 */
export const PointClusterer: React.FC<PointClustererProps> = ({
  points,
  draftPoints,
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
  const draftFeatureGroupRef = useRef<L.FeatureGroup>(null);
  const filterState = useFilterContext();
  const selectedPropertyContext = React.useContext(SelectedPropertyContext);
  const { propertyInfo: selected } = selectedPropertyContext;

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
   * Update the map bounds and zoom to make all draft properties visible.
   */
  useDeepCompareEffect(() => {
    const isDraft = draftPoints.length > 0;
    if (draftFeatureGroupRef.current && isDraft) {
      const group: L.FeatureGroup = draftFeatureGroupRef.current;
      const groupBounds = group.getBounds();
      const validLatLngs = draftPoints.map(p => {
        const latLng: LatLng = {
          lat: p.geometry?.coordinates[1],
          lng: p.geometry?.coordinates[0],
        } as LatLng;
        return latLng;
      });

      //react-leaflet is not displaying removed drafts but the layer is still present, this
      //causes the fitbounds calculation to be off. Fixed by manually cleaning up layers referencing removed drafts.
      group.getLayers().forEach((l: any) => {
        if (!find(validLatLngs, vl => (l._latlng as LatLng).equals(vl))) {
          group.removeLayer(l);
        }
      });

      if (groupBounds.isValid()) {
        filterState.setChanged(false);
        mapInstance.fitBounds(groupBounds, { maxZoom: zoom > MAX_ZOOM ? zoom : MAX_ZOOM });
      }
    }
  }, [draftFeatureGroupRef, mapInstance, draftPoints]);

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
      selectedPropertyContext.setLoading(true);
      getProperty(id)
        .then(apiProperty => {
          const propertyData = apiProperty.data;
          const propertyAddress = propertyData.address;
          const tenure = propertyData?.tenure !== undefined ? propertyData?.tenure[0] : undefined;
          const property: IProperty = {
            ...propertyData,
            pid: propertyData.pid || '',
            status: propertyData.status?.id,
            dataSource: propertyData.dataSource?.id,
            tenure: tenure?.id,
            address: {
              provinceId: propertyAddress?.province?.id || 0,
              streetAddress1: propertyAddress?.streetAddress1 || '',
              streetAddress2: propertyAddress?.streetAddress2,
              streetAddress3: propertyAddress?.streetAddress3,
              municipality: propertyAddress?.municipality || '',
              postal: propertyAddress?.postal || '',
            },
            areaUnit: propertyData.areaUnit?.id,
            landArea: propertyData.landArea || 0,
            landLegalDescription: propertyData.landLegalDescription || '',
            latitude: latLng.lat,
            longitude: latLng.lng,
          };
          selectedPropertyContext.setPropertyInfo(property);
        })
        .catch(() => {
          toast.error('Unable to load property details, refresh the page and try again.');
        })
        .finally(() => {
          selectedPropertyContext.setLoading(false);
        });
    },
    [getProperty, selectedPropertyContext],
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
                  setCurrentSelected(convertedProperty);
                  if (keycloak.canUserViewProperty(cluster.properties as IProperty)) {
                    convertedProperty?.id
                      ? fetchProperty(convertedProperty.id, {
                          lat: latitude,
                          lng: longitude,
                        })
                      : toast.dark('This property is invalid, unable to view details');
                  } else {
                    selectedPropertyContext.setPropertyInfo(convertedProperty);
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
                  convertedProperty?.id
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
              className={'active-selected'}
              position={[selected.latitude as number, selected.longitude as number]}
              map={mapInstance}
              eventHandlers={{
                click: () => {
                  selectedPropertyContext.setPropertyInfo(selected);
                  selected && onMarkerClick(selected);
                },
              }}
            />
          )}
      </FeatureGroup>
      <FeatureGroup ref={draftFeatureGroupRef}>
        {draftPoints.map((draftPoint, index) => {
          //render all of the unclustered draft markers.
          if (draftPoint?.geometry?.coordinates) {
            const [longitude, latitude] = draftPoint.geometry.coordinates;
            return (
              <Marker
                {...(draftPoint.properties as any)}
                key={index}
                position={[latitude, longitude]}
                icon={getDraftIcon((index + 1).toString())}
                zIndexOffset={500}
              ></Marker>
            );
          }
          return <></>;
        })}
      </FeatureGroup>
    </>
  );
};

export default PointClusterer;
