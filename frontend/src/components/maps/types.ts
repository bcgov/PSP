import { GeoJsonProperties } from 'geojson';
import Supercluster from 'supercluster';

export type ICluster<
  P extends GeoJsonProperties = Supercluster.AnyProps,
  C extends GeoJsonProperties = Supercluster.AnyProps
> = Supercluster.ClusterFeature<C> | Supercluster.PointFeature<P>;

/**
 * Property values for GIS features.
 */
export type PointFeature = Supercluster.PointFeature<{
  id: number;
  organizationId?: number;
  name?: string;
}>;
