import { PointFeature } from 'components/maps/types';
import { IGeoSearchParams } from 'constants/API';
import { useMapProperties } from 'features/properties/map/hooks/useMapProperties';
import { Feature, FeatureCollection, GeoJsonProperties, Geometry } from 'geojson';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { useModalContext } from 'hooks/useModalContext';
import { geoJSON } from 'leaflet';
import { useContext, useEffect } from 'react';
import { toast } from 'react-toastify';

import { PARCELS_LAYER_URL, PIMS_BOUNDARY_LAYER_URL, useLayerQuery } from '../leaflet/LayerPopup';
import { PropertyContext } from '../providers/PropertyContext';

export const useMapSearch = () => {
  const { properties, setProperties, setPropertiesLoading } = useContext(PropertyContext);
  const {
    loadProperties: { execute: loadProperties, loading, response },
  } = useMapProperties();
  const parcelsService = useLayerQuery(PARCELS_LAYER_URL);
  const pimsService = useLayerQuery(PIMS_BOUNDARY_LAYER_URL, true);
  const { setModalContent, setDisplayModal } = useModalContext();
  const keycloak = useKeycloakWrapper();
  const logout = keycloak.obj.logout;

  const search = async (
    filters?: IGeoSearchParams[],
  ): Promise<Feature<Geometry, GeoJsonProperties>[]> => {
    //TODO: PSP-4390 currently this loads all matching properties, this should be rewritten to use the bbox and make one request per tile.
    let tileData;
    try {
      setProperties([]);
      const filter = filters?.length && filters.length > 1 ? filters[0] : undefined;
      if (filter?.latitude && filter.longitude) {
        const task1 = parcelsService.findOneWhereContains({
          lat: +filter.latitude,
          lng: +filter.longitude,
        });
        const task2 = pimsService.findOneWhereContains(
          {
            lat: +filter.latitude,
            lng: +filter.longitude,
          },
          'GEOMETRY',
        );
        const parcel = await task1;
        const pimsProperties = await task2;
        //if found in inventory return or else non inventory
        tileData = pimsProperties.features.length ? pimsProperties : parcel;
      } else {
        let task1, task2, task3;
        task1 = loadProperties(filter);
        if (filter?.PIN) {
          task2 = parcelsService.findByPin(filter?.PIN);
        }
        if (filter?.PID) {
          task3 = parcelsService.findByPid(filter?.PID);
        }

        const pidPinInventoryData = await task1;
        const pinNonInventoryData = await task2;
        const pidNonInventoryData = await task3;

        if (pidPinInventoryData?.features === undefined) {
          setModalContent({
            title: 'Unable to connect to PIMS Inventory',
            message:
              'PIMS is unable to connect to connect to the PIMS Inventory map service. You may need to log out and log into the application in order to restore this functionality. If this error persists, contact a site administrator.',
            okButtonText: 'Log out',
            cancelButtonText: 'Continue working',
            handleOk: () => {
              logout();
            },
            handleCancel: () => {
              setDisplayModal(false);
            },
          });
          setDisplayModal(true);
        }

        tileData = pidPinInventoryData?.features?.length
          ? pidPinInventoryData
          : ({
              type: 'FeatureCollection',
              features: [
                ...(pinNonInventoryData?.features || []),
                ...(pidNonInventoryData?.features || []),
              ],
              bbox: pinNonInventoryData?.bbox || pidNonInventoryData?.bbox,
            } as FeatureCollection);
      }
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
      setPropertiesLoading(false);
    }
    return propertiesResponseToPointFeature(tileData);
  };

  return {
    search,
    loading,
    response,
    properties,
  };
};

/**
 * TODO: PSP-4390 convert polygon features into lat/lng coordinates, remove this when new Point Geoserver layer available.
 * @param feature the feature to obtain lat/lng coordinates for.
 * @returns [lat, lng]
 */
const getLatLng = (feature: any) => {
  if (feature.geometry.type === 'Polygon' || feature.geometry.type === 'MultiPolygon') {
    const latLng = geoJSON(feature.geometry).getBounds().getCenter();
    return [latLng.lng, latLng.lat];
  }
  return feature.geometry.coordinates;
};

export const propertiesResponseToPointFeature = (
  response: FeatureCollection<Geometry, GeoJsonProperties> | undefined,
): PointFeature[] => {
  const validFeatures = response?.features.filter(feature => !!feature?.geometry) ?? [];
  const data: PointFeature[] = validFeatures.map((feature: Feature) => {
    //TODO: PSP-4390 this converts all polygons to points, this should be changed to a View that returns the POINT instead of the POLYGON (psp-1859)
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
