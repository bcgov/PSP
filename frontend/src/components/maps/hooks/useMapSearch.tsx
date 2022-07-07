import { PointFeature } from 'components/maps/types';
import { IGeoSearchParams } from 'constants/API';
import { useMapProperties } from 'features/properties/map/hooks/useMapProperties';
import { Feature, FeatureCollection, GeoJsonProperties, Geometry } from 'geojson';
import { geoJSON } from 'leaflet';
import { useContext, useEffect } from 'react';
import { toast } from 'react-toastify';

import { PropertyContext } from '../providers/PropertyContext';

export const useMapSearch = () => {
  const { properties, setProperties, setPropertiesLoading } = useContext(PropertyContext);
  const {
    loadProperties: { execute: loadProperties, loading, response },
  } = useMapProperties();

  useEffect(() => {
    setPropertiesLoading(loading);
  }, [loading, setPropertiesLoading]);

  const search = async (
    filters?: IGeoSearchParams[],
  ): Promise<Feature<Geometry, GeoJsonProperties>[]> => {
    //TODO: currently this loads all matching properties, this should be rewritten to use the bbox and make one request per tile.
    try {
      const tileData = await loadProperties(
        filters?.length && filters.length > 1 ? filters[0] : undefined,
      );
      if (tileData) {
        const validFeatures = tileData.features.filter(feature => !!feature?.geometry);
        setProperties(propertiesResponseToPointFeature(tileData));

        if (validFeatures.length === 0) {
          toast.info('No search results found');
        } else {
          toast.info(`${validFeatures.length} properties found`);
        }
      }
    } catch (error) {
      toast.error((error as Error).message, { autoClose: 7000 });
    } finally {
    }
    return [];
  };

  return {
    search,
    loading,
    response,
    properties,
  };
};

/**
 * TODO: convert polygon features into lat/lng coordinates, remove this when new Point Geoserver layer available.
 * @param feature the feature to obtain lat/lng coordinates for.
 * @returns [lat, lng]
 */
const getLatLng = (feature: any) => {
  if (feature.geometry.type === 'Polygon' || feature.geometry.type === 'MultiPolygon') {
    const latLng = geoJSON(feature.geometry)
      .getBounds()
      .getCenter();
    return [latLng.lng, latLng.lat];
  }
  return feature.geometry.coordinates;
};

export const propertiesResponseToPointFeature = (
  response: FeatureCollection<Geometry, GeoJsonProperties> | undefined,
): PointFeature[] => {
  const validFeatures = response?.features.filter(feature => !!feature?.geometry) ?? [];
  const data: PointFeature[] = validFeatures.map((feature: Feature) => {
    //TODO: this converts all polygons to points, this should be changed to a View that returns the POINT instead of the POLYGON (psp-1859)
    return {
      ...feature,
      geometry: { type: 'Point', coordinates: getLatLng(feature) },
      properties: {
        ...feature.properties,
        id: feature.properties?.id ?? 0,
      },
    };
  });

  return data;
};
