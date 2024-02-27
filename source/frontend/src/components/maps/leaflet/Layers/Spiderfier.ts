import {
  GeoJSON,
  LatLng,
  LatLngExpression,
  Layer,
  Map,
  Marker,
  Point as LeafletPoint,
  PolylineOptions,
} from 'leaflet';
import { cloneDeep } from 'lodash';
import { AnyProps, PointFeature } from 'supercluster';
import invariant from 'tiny-invariant';

import { ICluster } from '@/components/maps/types';
import { exists } from '@/utils/utils';

export interface SpiderfierOptions<P> {
  /** Increase from 1 to increase the distance away from the center that spiderfied markers are placed. Use if you are using big marker icons (Default: 1). */
  spiderfyDistanceMultiplier: number;
  /** Allows you to specify PolylineOptions to style spider legs. By default, they are `{ weight: 1.5, color: '#222', opacity: 0.5 }`. */
  spiderLegPolylineOptions: PolylineOptions;
  /** A function that returns the cluster ID */
  getClusterId(cluster: ICluster): number;
  /** A function that returns all the points of a cluster */
  getClusterPoints(clusterId: number): Array<PointFeature<P>>;
  /** Adds a GeoJSON object to the layer. */
  pointToLayer(geoJsonPoint: PointFeature<P>, latlng: LatLngExpression): Layer;
  /** What happens when a cluster child pin is clicked */
  onMarkerClick?: (point: PointFeature<P>, position?: [number, number]) => void;
}

interface SpiderLine {
  coords: LatLng[];
  options: PolylineOptions;
}

interface SpiderMarker<P> extends PointFeature<P> {
  position: LatLng;
}

export interface SpiderSet<P> {
  lines?: SpiderLine[];
  markers?: SpiderMarker<P>[];
}

const defaultOptions = <P>(): SpiderfierOptions<P> => {
  return {
    spiderfyDistanceMultiplier: 1,
    spiderLegPolylineOptions: { weight: 1.5, color: '#222', opacity: 0.5 },
    getClusterId: null as any,
    getClusterPoints: null as any,
    pointToLayer: null as any,
    onMarkerClick: null as any,
  };
};

/** Deals with overlapping markers in the Leaflet maps API, Google Earth-style */
export class Spiderfier<P> {
  private readonly twoPI = Math.PI * 2;
  private readonly circleFootSeparation = 25; // related to circumference of circle
  private readonly circleStartAngle = 0;

  private readonly spiralFootSeparation = 28; // related to size of spiral (experiment!)
  private readonly spiralLengthStart = 11;
  private readonly spiralLengthFactor = 5;

  // shows a spiral instead of circle from this marker count upwards.
  // 0 -> always spiral; Infinity -> always circle
  private readonly circleSpiralSwitchover = 9;

  // internal state - the currently spiderfied cluster (if any)
  private cluster: ICluster | null = null;

  private readonly options: SpiderfierOptions<P>;

  constructor(public map: Map, options: Partial<SpiderfierOptions<P>> = {}) {
    this.options = { ...defaultOptions<P>(), ...options };
    // check required values - throws an error if callbacks are null
    const { getClusterId, getClusterPoints, pointToLayer } = this.options;
    invariant(getClusterId, 'Must supply getClusterId callback');
    invariant(getClusterPoints, 'Must supply getClusterPoints callback');
    invariant(pointToLayer, 'Must supply pointToLayer callback');
  }

  // expand a cluster (spiderfy)
  public spiderfy(cluster: ICluster): SpiderSet<P> {
    const { getClusterId, getClusterPoints } = this.options;

    // only one cluster expanded at a time
    if (this.cluster === cluster || cluster == null) {
      this.cluster = null;
      this.unspiderfy();
      return {};
    }
    this.unspiderfy();
    this.cluster = cluster;
    if (cluster.geometry.coordinates !== undefined && cluster.geometry.coordinates.length > 1) {
      const clusterX = cluster.geometry.coordinates[0];
      const clusterY = cluster.geometry.coordinates[1];
      const centerLatlng = GeoJSON.coordsToLatLng([clusterX, clusterY]);

      const centerXY = this.map.latLngToLayerPoint(centerLatlng); // screen coordinates
      const clusterId = getClusterId(cluster);
      const children = getClusterPoints(clusterId).map(p => cloneDeep(p)); // work with a copy of the data

      let positions: LeafletPoint[];
      if (children.length >= this.circleSpiralSwitchover) {
        positions = this.generatePointsSpiral(children.length, centerXY);
      } else {
        positions = this.generatePointsCircle(children.length, centerXY);
      }

      // add expanded cluster points to map
      const results = this.addToMap(centerXY, children, positions);

      // dim cluster icon
      this.map.eachLayer(layer => {
        if (this.layerMatchesCluster(layer, this.cluster)) {
          const clusterMarker = layer as Marker;
          if (clusterMarker.setOpacity) {
            clusterMarker.setOpacity(0.75);
          }
        }
      });
      return results;
    } else {
      return {};
    }
  }

  // shrink an expanded cluster (unspiderfy)
  public unspiderfy() {
    this.map.eachLayer((layer: Layer & AnyProps) => {
      if (layer._spiderfied) {
        layer.remove();
        delete layer._spiderfied;
      }

      // restore cluster opacity
      if (this.layerMatchesCluster(layer, this.cluster)) {
        const clusterMarker = layer as Marker;
        if (clusterMarker.setOpacity) {
          clusterMarker.setOpacity(1);
        }
      }
    });

    this.cluster = null;
  }

  private addToMap(
    centerXY: LeafletPoint,
    points: Array<PointFeature<P>>,
    positions: Array<LeafletPoint>,
  ): SpiderSet<P> {
    const { spiderLegPolylineOptions: legOptions } = this.options;
    const centerLatLng = this.map.layerPointToLatLng(centerXY);

    const markers: SpiderMarker<P>[] = [];
    const lines: SpiderLine[] = [];
    for (let i = 0; i < points.length; i++) {
      const newPos: LatLng = this.map.layerPointToLatLng(positions[i]);
      const geojson: PointFeature<P> = points[i];
      markers.push({ ...geojson, position: newPos });
      lines.push({ coords: [centerLatLng, newPos], options: legOptions });
    }

    return { lines, markers };
  }

  private layerMatchesCluster(layer: Layer, cluster: ICluster | null): boolean {
    if (!layer || !cluster) {
      return false;
    }
    const { getClusterId } = this.options;
    const geojsonObj = (layer as Marker)?.feature;
    const id = geojsonObj ? getClusterId(geojsonObj) : null;
    const targetId = getClusterId(cluster);
    return exists(id) && id === targetId;
  }

  private generatePointsCircle(count: number, center: LeafletPoint): LeafletPoint[] {
    const circumference =
      this.options.spiderfyDistanceMultiplier * this.circleFootSeparation * (2 + count);
    const angleStep = this.twoPI / count;
    let legLength = circumference / this.twoPI; //radius from circumference
    let angle;
    const result: LeafletPoint[] = [];

    legLength = Math.max(legLength, 35); // Minimum distance to get outside the cluster icon.

    // Clockwise, like spiral.
    for (let i = 0; i < count; i++) {
      angle = this.circleStartAngle + i * angleStep;
      result.push(
        new LeafletPoint(
          center.x + legLength * Math.cos(angle),
          center.y + legLength * Math.sin(angle),
        ).round(),
      );
    }

    return result;
  }

  private generatePointsSpiral(count: number, center: LeafletPoint): LeafletPoint[] {
    const spiderfyDistanceMultiplier = this.options.spiderfyDistanceMultiplier;
    const separation = spiderfyDistanceMultiplier * this.spiralFootSeparation;
    const lengthFactor = spiderfyDistanceMultiplier * this.spiralLengthFactor * this.twoPI;
    let legLength = spiderfyDistanceMultiplier * this.spiralLengthStart;
    let angle = 0;
    const result: LeafletPoint[] = [];

    // Higher index, closer position to cluster center.
    for (let i = count; i >= 0; i--) {
      // Skip the first position, so that we are already farther from center and we avoid
      // being under the default cluster icon (especially important for Circle Markers).
      if (i < count) {
        result.push(
          new LeafletPoint(
            center.x + legLength * Math.cos(angle),
            center.y + legLength * Math.sin(angle),
          ).round(),
        );
      }
      angle += separation / legLength + i * 0.0005;
      legLength += lengthFactor / angle;
    }
    return result;
  }
}
