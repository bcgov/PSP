import { Feature, FeatureCollection, GeoJsonProperties, Geometry } from 'geojson';
import { geoJSON } from 'leaflet';
import { useContext } from 'react';
import { toast } from 'react-toastify';

import { PointFeature } from '@/components/maps/types';
import { IGeoSearchParams } from '@/constants/API';
import { useFullyAttributedParcelMapLayer } from '@/hooks/repositories/mapLayer/useFullyAttributedParcelMapLayer';
import { usePimsPropertyLayer } from '@/hooks/repositories/mapLayer/usePimsPropertyLayer';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { useModalContext } from '@/hooks/useModalContext';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { PIMS_Property_Location_View2 } from '@/models/layers/pimsPropertyLocationView';

import { PropertyContext } from '../providers/PropertyContext';

export const useMapSearch = () => {
  const { properties, setProperties, setPropertiesLoading } = useContext(PropertyContext);

  const fullyAttributedService = useFullyAttributedParcelMapLayer();
  const pimsPropertyLayerService = usePimsPropertyLayer();

  const { setModalContent, setDisplayModal } = useModalContext();
  const keycloak = useKeycloakWrapper();
  const logout = keycloak.obj.logout;

  const searchOne = async (latitude: number, longitude: number) => {
    try {
      setPropertiesLoading(true);
      setProperties([]);

      let foundFeature: Feature<Geometry, GeoJsonProperties> | undefined = undefined;
      const findOneParcelTask = fullyAttributedService.findOne({
        lat: latitude,
        lng: longitude,
      });
      const findOnePropertyTask = pimsPropertyLayerService.findOne(
        {
          lat: latitude,
          lng: longitude,
        },
        'GEOMETRY',
      );
      const parcelFeature = await findOneParcelTask;
      const pimsPropertyFeature = await findOnePropertyTask;

      //if found in inventory return or else non inventory
      foundFeature = pimsPropertyFeature ? pimsPropertyFeature : parcelFeature;

      if (foundFeature && foundFeature.geometry) {
        setProperties([propertyResponseToPointFeature(foundFeature)]);
        toast.info(`Property found`);
      } else {
        toast.info('No search results found');
      }
    } catch (error) {
      toast.error((error as Error).message, { autoClose: 7000 });
    } finally {
      setPropertiesLoading(false);
    }
  };

  const searchMany = async (filter?: IGeoSearchParams) => {
    //TODO: PSP-4390 currently this loads all matching properties, this should be rewritten to use the bbox and make one request per tile.
    try {
      setPropertiesLoading(true);
      setProperties([]);

      let loadPropertiesTask: Promise<
        FeatureCollection<Geometry, PIMS_Property_Location_View2> | undefined
      >;

      let findByPinTask:
        | Promise<FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties> | undefined>
        | undefined = undefined;

      let findByPidTask:
        | Promise<FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties> | undefined>
        | undefined = undefined;

      loadPropertiesTask = pimsPropertyLayerService.loadPropertyLayer.execute(filter);
      if (filter?.PIN) {
        findByPinTask = fullyAttributedService.findByPin(filter?.PIN);
      }
      if (filter?.PID) {
        findByPidTask = fullyAttributedService.findByPid(filter?.PID);
      }

      let pidPinInventoryData:
        | FeatureCollection<Geometry, PIMS_Property_Location_View2>
        | undefined;
      try {
        pidPinInventoryData = await loadPropertiesTask;
      } catch (err) {
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
      const [pinFullyAttributedData, pidFullyAttributedData] = await Promise.all([
        findByPinTask,
        findByPidTask,
      ]);

      // If the property was found on the pims inventory, use that.
      if (pidPinInventoryData?.features && pidPinInventoryData?.features?.length > 0) {
        const validFeatures = pidPinInventoryData.features.filter(feature => !!feature?.geometry);
        setProperties(propertiesResponseToPointFeature(pidPinInventoryData));

        if (validFeatures.length === 0) {
          toast.info('No search results found');
        } else {
          toast.info(`${validFeatures.length} properties found`);
        }
      } else {
        const attributedFeatures: FeatureCollection<
          Geometry,
          PMBC_FullyAttributed_Feature_Properties
        > = {
          type: 'FeatureCollection',
          features: [
            ...(pinFullyAttributedData?.features || []),
            ...(pidFullyAttributedData?.features || []),
          ],
          bbox: pinFullyAttributedData?.bbox || pidFullyAttributedData?.bbox,
        };

        const validFeatures = attributedFeatures.features.filter(feature => !!feature?.geometry);
        setProperties(propertiesResponseToPointFeature(attributedFeatures));

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
  };

  const searchByParams = async (filters: IGeoSearchParams[]) => {
    const filter = filters?.length && filters.length > 1 ? filters[0] : undefined;
    // If there is a lat-long parameter then we are looking for only one feature
    if (filter?.latitude && filter.longitude) {
      searchOne(Number(filter.latitude), Number(filter.longitude));
    } else {
      searchMany(filter);
    }
  };

  return {
    searchByParams,
    searchOne,
    searchMany,
    loadingPimsProperties: pimsPropertyLayerService.loadPropertyLayer,
    loadingPimsPropertiesResponse: pimsPropertyLayerService.loadPropertyLayer.response,
    properties,
  };
};

/**
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

const propertiesResponseToPointFeature = (
  response: FeatureCollection<Geometry, GeoJsonProperties> | undefined,
): PointFeature[] => {
  const validFeatures = response?.features.filter(feature => !!feature?.geometry) ?? [];
  const data: PointFeature[] = validFeatures.map((feature: Feature) => {
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

const propertyResponseToPointFeature = (
  feature: Feature<Geometry, GeoJsonProperties>,
): PointFeature => {
  const data: PointFeature = {
    ...feature,
    geometry: { type: 'Point', coordinates: getLatLng(feature) },
    properties: {
      ...feature.properties,
      id: feature.properties?.id ?? 0,
    },
  };

  return data;
};
