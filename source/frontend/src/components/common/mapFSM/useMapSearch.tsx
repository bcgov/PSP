import { FeatureCollection, Geometry } from 'geojson';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { IGeoSearchParams } from '@/constants/API';
import { useFullyAttributedParcelMapLayer } from '@/hooks/repositories/mapLayer/useFullyAttributedParcelMapLayer';
import { usePimsPropertyLayer } from '@/hooks/repositories/mapLayer/usePimsPropertyLayer';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { useModalContext } from '@/hooks/useModalContext';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { PIMS_Property_Location_View } from '@/models/layers/pimsPropertyLocationView';

import {
  emptyFeatureData,
  emptyFullyFeaturedFeatureCollection,
  emptyPimsBoundaryFeatureCollection,
  emptyPimsLocationFeatureCollection,
  MapFeatureData,
} from './models';

export const useMapSearch = () => {
  const fullyAttributedService = useFullyAttributedParcelMapLayer();
  const pimsPropertyLayerService = usePimsPropertyLayer();

  const { setModalContent, setDisplayModal } = useModalContext();
  const keycloak = useKeycloakWrapper();
  const logout = keycloak.obj.logout;

  const loadPimsProperties = pimsPropertyLayerService.loadPropertyLayer.execute;
  const fullyAttributedServiceFindByPin = fullyAttributedService.findByPin;
  const fullyAttributedServiceFindByPid = fullyAttributedService.findByPid;
  const fullyAttributedServiceFindByPLanNumber = fullyAttributedService.findByPlanNumber;

  const fullyAttributedServiceFindOne = fullyAttributedService.findOne;
  const pimsPropertyLayerServiceFindOne = pimsPropertyLayerService.findOne;

  const searchOneLocation = useCallback(
    async (latitude: number, longitude: number) => {
      let result: MapFeatureData = { ...emptyFeatureData };
      try {
        const findOneParcelTask = fullyAttributedServiceFindOne({
          lat: latitude,
          lng: longitude,
        });
        const findOnePropertyTask = pimsPropertyLayerServiceFindOne(
          {
            lat: latitude,
            lng: longitude,
          },
          'GEOMETRY',
        );

        const [parcelFeature, pimsPropertyFeature] = await Promise.all([
          findOneParcelTask,
          findOnePropertyTask,
        ]);

        //if found in inventory return or else non inventory
        if (pimsPropertyFeature !== undefined) {
          toast.info(`Property found`);
          result = {
            ...emptyFeatureData,
            pimsBoundaryFeatures: {
              type: 'FeatureCollection',
              features: [pimsPropertyFeature],
            },
          };
        } else if (parcelFeature !== undefined) {
          toast.info(`Property found`);
          result = {
            ...emptyFeatureData,
            fullyAttributedFeatures: {
              type: 'FeatureCollection',
              features: [parcelFeature],
            },
          };
        } else {
          toast.info('No search results found');
        }
      } catch (error) {
        toast.error((error as Error).message, { autoClose: 7000 });
      } finally {
        // TODO: Remove once try above is no longer necessary
      }
      return result;
    },
    [fullyAttributedServiceFindOne, pimsPropertyLayerServiceFindOne],
  );

  const searchByPlanNumber = useCallback(
    async (filter?: IGeoSearchParams) => {
      let result: MapFeatureData = emptyFeatureData;
      try {
        let loadPropertiesTask: Promise<
          FeatureCollection<Geometry, PIMS_Property_Location_View> | undefined
        >;

        let findByPlanNumberTask:
          | Promise<
              FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties> | undefined
            >
          | undefined = undefined;

        loadPropertiesTask = loadPimsProperties(filter);

        const forceExactMatch = true;

        if (filter?.SURVEY_PLAN_NUMBER) {
          findByPlanNumberTask = fullyAttributedServiceFindByPLanNumber(
            filter?.SURVEY_PLAN_NUMBER,
            forceExactMatch,
          );
        }

        let planNumberInventoryData:
          | FeatureCollection<Geometry, PIMS_Property_Location_View>
          | undefined;
        try {
          planNumberInventoryData = await loadPropertiesTask;
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
        const planNumberFullyAttributedData = await findByPlanNumberTask;

        // If the property was found on the pims inventory, use that.
        if (planNumberInventoryData?.features && planNumberInventoryData?.features?.length > 0) {
          const validFeatures = planNumberInventoryData.features.filter(
            feature => !!feature?.geometry,
          );

          result = {
            pimsLocationFeatures: {
              type: planNumberInventoryData.type,
              bbox: planNumberInventoryData.bbox,
              features: validFeatures,
            },
            pimsBoundaryFeatures: emptyPimsBoundaryFeatureCollection,
            fullyAttributedFeatures: emptyFullyFeaturedFeatureCollection,
          };

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
            features: [...(planNumberFullyAttributedData?.features || [])],
            bbox: planNumberFullyAttributedData?.bbox,
          };
          const validFeatures = attributedFeatures.features.filter(feature => !!feature?.geometry);
          result = {
            pimsLocationFeatures: emptyPimsLocationFeatureCollection,
            pimsBoundaryFeatures: emptyPimsBoundaryFeatureCollection,
            fullyAttributedFeatures: {
              type: attributedFeatures.type,
              bbox: attributedFeatures.bbox,
              features: validFeatures,
            },
          };

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

      return result;
    },
    [
      fullyAttributedServiceFindByPLanNumber,
      loadPimsProperties,
      logout,
      setDisplayModal,
      setModalContent,
    ],
  );

  const searchMany = useCallback(
    async (filter?: IGeoSearchParams) => {
      let result: MapFeatureData = emptyFeatureData;
      //TODO: PSP-4390 currently this loads all matching properties, this should be rewritten to use the bbox and make one request per tile.
      try {
        let loadPropertiesTask: Promise<
          FeatureCollection<Geometry, PIMS_Property_Location_View> | undefined
        >;

        let findByPinTask:
          | Promise<
              FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties> | undefined
            >
          | undefined = undefined;

        let findByPidTask:
          | Promise<
              FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties> | undefined
            >
          | undefined = undefined;

        loadPropertiesTask = loadPimsProperties(filter);
        if (filter?.PIN) {
          findByPinTask = fullyAttributedServiceFindByPin(filter?.PIN);
        }
        if (filter?.PID) {
          findByPidTask = fullyAttributedServiceFindByPid(filter?.PID);
        }

        let pidPinInventoryData:
          | FeatureCollection<Geometry, PIMS_Property_Location_View>
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

          result = {
            pimsLocationFeatures: {
              type: pidPinInventoryData.type,
              bbox: pidPinInventoryData.bbox,
              features: validFeatures,
            },
            pimsBoundaryFeatures: emptyPimsBoundaryFeatureCollection,
            fullyAttributedFeatures: emptyFullyFeaturedFeatureCollection,
          };

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
          result = {
            pimsLocationFeatures: emptyPimsLocationFeatureCollection,
            pimsBoundaryFeatures: emptyPimsBoundaryFeatureCollection,
            fullyAttributedFeatures: {
              type: attributedFeatures.type,
              bbox: attributedFeatures.bbox,
              features: validFeatures,
            },
          };

          if (validFeatures.length === 0) {
            toast.info('No search results found');
          } else {
            toast.info(`${validFeatures.length} properties found`);
          }
        }
      } catch (error) {
        toast.error((error as Error).message, { autoClose: 7000 });
      } finally {
        // TODO: Remove once try above is no longer necessary
      }

      return result;
    },
    [
      logout,
      setDisplayModal,
      setModalContent,
      loadPimsProperties,
      fullyAttributedServiceFindByPin,
      fullyAttributedServiceFindByPid,
    ],
  );

  return {
    searchOneLocation,
    searchByPlanNumber,
    searchMany,
    loadingPimsProperties: pimsPropertyLayerService.loadPropertyLayer,
    loadingPimsPropertiesResponse: pimsPropertyLayerService.loadPropertyLayer.response,
  };
};
