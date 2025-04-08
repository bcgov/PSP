import './PointClusterer.scss';

import { BBox, Feature, FeatureCollection, Geometry, MultiPolygon, Polygon } from 'geojson';
import L, { geoJSON, LatLng, LatLngLiteral } from 'leaflet';
import { find } from 'lodash';
import polylabel from 'polylabel';
import React, { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { FeatureGroup, Marker, Polyline, useMap } from 'react-leaflet';
import Supercluster, { ClusterFeature, ClusterProperties, PointFeature } from 'supercluster';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import useSupercluster from '@/components/maps/hooks/useSupercluster';
import { useFilterContext } from '@/components/maps/providers/FilterProvider';
import { ICluster } from '@/components/maps/types';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import {
  PIMS_Property_Boundary_View,
  PIMS_Property_Location_View,
} from '@/models/layers/pimsPropertyLocationView';
import { exists } from '@/utils';

import { ONE_HUNDRED_METER_PRECISION } from '../../constants';
import SinglePropertyMarker from '../Markers/SingleMarker';
import { Spiderfier, SpiderSet } from './Spiderfier';
import { getDraftIcon, getMarkerIcon, pointToLayer, zoomToCluster } from './util';

export type PointClustererProps = {
  bounds?: BBox;
  zoom: number;
  minZoom?: number;
  maxZoom?: number;
  /** When you click a cluster we zoom to its bounds. Default: true */
  zoomToBoundsOnClick?: boolean;
  /** When you click a cluster at the bottom zoom level we spiderfy it so you can see all of its markers. Default: true */
  spiderfyOnMaxZoom?: boolean;

  tilesLoaded: boolean;
};

/**
 * Clusters pins that are close together geographically based on the zoom level into a single clustered object.
 * @param param0 Point cluster properties.
 */
export const PointClusterer: React.FC<React.PropsWithChildren<PointClustererProps>> = ({
  bounds,
  zoom,
  minZoom: minZoomProps,
  maxZoom: maxZoomProps,
  zoomToBoundsOnClick = true,
  spiderfyOnMaxZoom = true,
  tilesLoaded,
}) => {
  // state and refs
  const spiderfierRef =
    useRef<
      Spiderfier<
        | PIMS_Property_Location_View
        | PIMS_Property_Boundary_View
        | PMBC_FullyAttributed_Feature_Properties
      >
    >();
  const featureGroupRef = useRef<L.FeatureGroup>(null);
  const draftFeatureGroupRef = useRef<L.FeatureGroup>(null);
  const filterState = useFilterContext();

  // TODO: Figure out if the currentCluster is needed
  const [, setCurrentCluster] = useState<ICluster<any, Supercluster.AnyProps> | undefined>(
    undefined,
  );

  const mapMachine = useMapStateMachine();

  const selectedFeature = mapMachine.mapFeatureSelected;

  const mapInstance: L.Map = useMap();
  if (!mapInstance) {
    throw new Error('<PointClusterer /> must be used under a <Map> leaflet component');
  }

  const minZoom = minZoomProps ?? 0;
  const maxZoom = maxZoomProps ?? 18;

  const [spider, setSpider] = useState<
    SpiderSet<
      | PIMS_Property_Location_View
      | PIMS_Property_Boundary_View
      | PMBC_FullyAttributed_Feature_Properties
    >
  >({});

  const draftPoints = useMemo<LatLngLiteral[]>(() => {
    return mapMachine.filePropertyLocations.map(x => {
      // The values on the feature are rounded to the 4th decimal. Do the same to the draft points.
      return {
        lat: x.lat,
        lng: x.lng,
      };
    });
  }, [mapMachine.filePropertyLocations]);

  const pimsLocationFeatures: FeatureCollection<Geometry, PIMS_Property_Location_View> =
    useMemo(() => {
      let filteredFeatures = mapMachine.mapFeatureData.pimsLocationFeatures.features.filter(x =>
        mapMachine.activePimsPropertyIds.includes(Number(x.properties.PROPERTY_ID)),
      );

      if (!mapMachine.showRetired) {
        filteredFeatures = filteredFeatures.filter(x => !x.properties.IS_RETIRED);
      }

      // Do not cluster any points that do not have markers on the map.
      const displayableFeatures = filteredFeatures.filter(f =>
        exists(getMarkerIcon(f, false, mapMachine.showDisposed, mapMachine.showRetired)),
      );

      return {
        type: mapMachine.mapFeatureData.pimsLocationFeatures.type,
        features: displayableFeatures,
      };
    }, [
      mapMachine.activePimsPropertyIds,
      mapMachine.mapFeatureData.pimsLocationFeatures.features,
      mapMachine.mapFeatureData.pimsLocationFeatures.type,
      mapMachine.showDisposed,
      mapMachine.showRetired,
    ]);

  const pimsBoundaryFeatures = mapMachine.mapFeatureData.pimsBoundaryFeatures;

  const pmbcFeatures = mapMachine.mapFeatureData.fullyAttributedFeatures;

  const featurePoints: Supercluster.PointFeature<
    | PIMS_Property_Location_View
    | PIMS_Property_Boundary_View
    | PMBC_FullyAttributed_Feature_Properties
  >[] = useMemo(() => {
    const pimsLocationPoints =
      featureCollectionResponseToPointFeature<PIMS_Property_Location_View>(pimsLocationFeatures);
    const pimsBoundaryPoints =
      featureCollectionResponseToPointFeature<PIMS_Property_Boundary_View>(pimsBoundaryFeatures);
    const pmbcPoints =
      featureCollectionResponseToPointFeature<PMBC_FullyAttributed_Feature_Properties>(
        pmbcFeatures,
      );
    return [...pimsLocationPoints, ...pimsBoundaryPoints, ...pmbcPoints];
  }, [pimsLocationFeatures, pimsBoundaryFeatures, pmbcFeatures]);

  // get clusters
  // clusters are an array of GeoJSON Feature objects, but some of them
  // represent a cluster of points, and some represent individual points.
  const { clusters, supercluster } = useSupercluster({
    points: featurePoints,
    bounds,
    zoom,
    options: {
      radius: 60,
      extent: 256,
      minZoom,
      maxZoom,
      enableClustering: !filterState.changed,
      log: true,
    },
  });

  // Register event handlers to shrink and expand clusters when map is interacted with
  const componentDidMount = useCallback(() => {
    if (!spiderfierRef.current) {
      spiderfierRef.current = new Spiderfier(mapInstance, {
        getClusterId: cluster => cluster?.properties?.cluster_id,
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
    (cluster: ClusterFeature<ClusterProperties>) => {
      if (!supercluster || !spiderfierRef.current || !cluster) {
        return;
      }
      const { cluster_id } = cluster.properties;
      const expansionZoom = Math.min(supercluster.getClusterExpansionZoom(cluster_id), maxZoom);

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
   * Cleanup draft layers.
   * TODO: Figure out if this is still necessary now that this does not fit the map bounds
   */
  useDeepCompareEffect(() => {
    const hasDraftPoints = draftPoints.length > 0;
    if (draftFeatureGroupRef.current && hasDraftPoints) {
      const group: L.FeatureGroup = draftFeatureGroupRef.current;

      //react-leaflet is not displaying removed drafts but the layer is still present, this
      //causes the fitbounds calculation to be off. Fixed by manually cleaning up layers referencing removed drafts.
      group.getLayers().forEach((l: any) => {
        if (!find(draftPoints, vl => (l._latlng as LatLng).equals(vl))) {
          group.removeLayer(l);
        }
      });

      const groupBounds = group.getBounds();

      if (groupBounds.isValid()) {
        filterState.setChanged(false);
      }
    }
  }, [draftFeatureGroupRef, mapInstance, draftPoints]);

  /**
   * Updates the state of the cluster if the feature group has been updated.
   * TODO: Figure out if this is still necessary
   */
  useDeepCompareEffect(() => {
    if (featureGroupRef.current) {
      const group: L.FeatureGroup = featureGroupRef.current;
      const groupBounds = group.getBounds();

      if (groupBounds.isValid() && filterState.changed && !selectedFeature && tilesLoaded) {
        filterState.setChanged(false);
      }

      setSpider({});
      spiderfierRef.current?.unspiderfy();
      setCurrentCluster(undefined);
    }
  }, [featureGroupRef, mapInstance, clusters, selectedFeature, tilesLoaded]);

  const mapMarkerClickFn = mapMachine.mapMarkerClick;
  const renderedPoints = useMemo(() => {
    return (
      <>
        <FeatureGroup ref={featureGroupRef}>
          {/**
           * Render all visible clusters
           */}
          {(clusters ?? []).map((cluster, index) => {
            // every cluster point has coordinates
            const [longitude, latitude] = cluster.geometry.coordinates;

            // Only clusters have the cluster property, if so we have a cluster to render
            if ('cluster' in cluster.properties) {
              const clusterFeature = cluster as ClusterFeature<ClusterProperties>;
              const { point_count: pointCount, point_count_abbreviated } =
                clusterFeature.properties;

              const sizeClass = pointCount < 100 ? 'small' : pointCount < 1000 ? 'medium' : 'large';
              return (
                // render the cluster marker
                <Marker
                  key={index}
                  position={[latitude, longitude]}
                  eventHandlers={{
                    click: e => {
                      zoomOrSpiderfy(clusterFeature);
                      e.target.closePopup();
                    },
                  }}
                  icon={
                    new L.DivIcon({
                      html: `<div><span>${point_count_abbreviated}</span></div>`,
                      className: `marker-cluster marker-cluster-${sizeClass}`,
                      iconSize: [40, 40],
                    })
                  }
                />
              );
            } else {
              const clusterFeature = cluster as PointFeature<
                | PIMS_Property_Location_View
                | PIMS_Property_Boundary_View
                | PMBC_FullyAttributed_Feature_Properties
              >;

              const isSelected =
                selectedFeature !== null ? clusterFeature.id === selectedFeature.clusterId : false;

              const latlng = { lat: latitude, lng: longitude };

              return (
                <SinglePropertyMarker
                  key={index}
                  pointFeature={clusterFeature}
                  markerPosition={latlng}
                  isSelected={isSelected}
                />
              );
            }
          })}
          {/**
           * Render markers from a spiderfied cluster click
           */}
          {spider.markers?.map((m, index: number) => {
            const clusterFeature = m as PointFeature<
              | PIMS_Property_Location_View
              | PIMS_Property_Boundary_View
              | PMBC_FullyAttributed_Feature_Properties
            >;

            return (
              <SinglePropertyMarker
                key={index}
                pointFeature={clusterFeature}
                markerPosition={m.position}
                isSelected={false}
              />
            );
          })}
          {/**
           * Render lines/legs from a spiderfied cluster click
           */}
          {spider.lines?.map((m, index: number) => (
            <Polyline key={index} positions={m.coords} {...m.options} />
          ))}
        </FeatureGroup>
        {/**
         * Render all of the unclustered DRAFT MARKERS.
         **/}
        <FeatureGroup ref={draftFeatureGroupRef}>
          {draftPoints.map((draftPoint, index) => {
            return (
              <Marker
                key={index}
                position={draftPoint}
                icon={getDraftIcon((index + 1).toString())}
                zIndexOffset={500}
                eventHandlers={{
                  click: e => {
                    // stop propagation of 'click' event to the underlying leaflet map
                    e.originalEvent.preventDefault();
                    e.originalEvent.stopPropagation();

                    mapMarkerClickFn({
                      clusterId: 'NO_ID',
                      latlng: draftPoint,
                      pimsLocationFeature: null,
                      pimsBoundaryFeature: null,
                      fullyAttributedFeature: null,
                    });
                  },
                }}
              ></Marker>
            );
          })}
        </FeatureGroup>
      </>
    );
  }, [
    clusters,
    draftPoints,
    selectedFeature,
    spider.lines,
    spider.markers,
    zoomOrSpiderfy,
    mapMarkerClickFn,
  ]);
  return renderedPoints;
};

export default PointClusterer;

/**
 * @param feature the feature to obtain lat/lng coordinates for.
 * @returns [lat, lng]
 */
export const getFeatureLatLng = <P,>(feature: Feature<Geometry, P>) => {
  if (feature?.geometry?.type === 'Polygon') {
    return polylabel((feature.geometry as Polygon).coordinates, ONE_HUNDRED_METER_PRECISION);
  } else if (feature?.geometry?.type === 'MultiPolygon') {
    return polylabel(
      (feature.geometry as MultiPolygon).coordinates[0],
      ONE_HUNDRED_METER_PRECISION,
    );
  } else if ('coordinates' in feature.geometry) {
    // TODO: This is only needed to satisfy the types. Fix this.
    const latLng = geoJSON(feature.geometry).getBounds().getCenter();
    return [latLng.lng, latLng.lat];
  } else {
    return [];
  }
};

const featureCollectionResponseToPointFeature = <P,>(
  response: FeatureCollection<Geometry, P> | undefined,
): PointFeature<P>[] => {
  const validFeatures = response?.features.filter(feature => !!feature?.geometry) ?? [];
  const data: PointFeature<P>[] = validFeatures.map(feature => {
    return featureResponseToPointFeature(feature);
  });

  return data;
};

const featureResponseToPointFeature = <P,>(feature: Feature<Geometry, P>): PointFeature<P> => {
  const data: PointFeature<P> = {
    ...feature,
    geometry: { type: 'Point', coordinates: getFeatureLatLng(feature) },
    properties: {
      ...feature.properties,
    },
  };

  return data;
};
