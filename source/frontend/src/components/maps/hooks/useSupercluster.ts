import { dequal } from 'dequal';
import { BBox, GeoJsonProperties } from 'geojson';
import debounce from 'lodash/debounce';
import { useRef, useState } from 'react';
import Supercluster from 'supercluster';

import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';

import { ICluster } from '../types';

// P => Properties | C => Cluster
interface SuperclusterOptions<P, C> extends Supercluster.Options<P, C> {
  /** Optionally enable clusters recalculation */
  enableClustering?: boolean;
}
export interface UseSuperclusterProps<P, C> {
  points: Array<Supercluster.PointFeature<P>>;
  bounds?: BBox;
  zoom: number;
  options?: SuperclusterOptions<P, C>;
}

const useSupercluster = <
  P extends GeoJsonProperties = Supercluster.AnyProps,
  C extends GeoJsonProperties = Supercluster.AnyProps,
>({
  points,
  bounds,
  zoom,
  options,
}: UseSuperclusterProps<P, C>) => {
  const superclusterRef = useRef<Supercluster<P, C>>();
  const pointsRef = useRef<Array<Supercluster.PointFeature<P>>>();
  const [clusters, setClusters] = useState<Array<ICluster<P, C>>>([]);
  const zoomInt = Math.round(zoom);
  const getClustersFn = useRef(
    debounce(
      (bounds: BBox, zoomInt: number) => {
        setClusters(superclusterRef.current.getClusters(bounds, zoomInt));
      },
      250,
      { leading: false, trailing: true },
    ),
  ).current;

  // use deep-equals to avoid infinite re-rendering when objects have same data but are different JS instances
  useDeepCompareEffect(() => {
    // Create and initialize the supercluster instance if not existing already.
    if (!superclusterRef.current || !dequal(pointsRef.current, points)) {
      superclusterRef.current = new Supercluster(options);
      superclusterRef.current.load(points);
    }

    /**
     * Only create clusters when zooming to results is done OR number of points is
     * greater than 100 (avoid perfomrance issues when rendering all points)
     */
    if (bounds && (options?.enableClustering || points.length > 100)) {
      getClustersFn(bounds, zoomInt);
    } else {
      setClusters(points);
    }

    pointsRef.current = points;
  }, [points, bounds, zoomInt]);

  return { clusters, supercluster: superclusterRef.current };
};

export default useSupercluster;
