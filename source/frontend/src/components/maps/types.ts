import { GeoJsonProperties } from 'geojson';
import Supercluster from 'supercluster';

export type ICluster<
  P extends GeoJsonProperties = GeoJsonProperties,
  C extends GeoJsonProperties = Supercluster.ClusterProperties,
> = Supercluster.ClusterFeature<C> | Supercluster.PointFeature<P>;

//export type ICluster<P, C> = Supercluster.ClusterFeature<C> | Supercluster.PointFeature<P>;

/**
 * Property values for GIS features.
 */
export type PointFeature = Supercluster.PointFeature<{
  id: number;
  organizationId?: number;
  name?: string;
}>;
