import { FeatureCollection, Geometry } from 'geojson';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { IGeoSearchParams } from '@/constants/API';
import { useParcelMapLayer } from '@/hooks/repositories/mapLayer/useParcelMapLayer';
import { usePimsPropertyLayer } from '@/hooks/repositories/mapLayer/usePimsPropertyLayer';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { useModalContext } from '@/hooks/useModalContext';
import { PMBC_Feature_Properties } from '@/models/layers/parcelMapBC';
import { PIMS_Property_Location_View } from '@/models/layers/pimsPropertyLocationView';

import {
  emptyFeatureData,
  emptyPimsBoundaryFeatureCollection,
  emptyPimsLocationFeatureCollection,
  emptyPmbcFeatureCollection,
  MapFeatureData,
} from './models';

export const useMapSearch = () => {
  const pmbcService = useParcelMapLayer();
  const pimsPropertyLayerService = usePimsPropertyLayer();

  const { setModalContent, setDisplayModal } = useModalContext();
  const keycloak = useKeycloakWrapper();
  const logout = keycloak.obj.logout;

  const loadPimsProperties = pimsPropertyLayerService.loadPropertyLayer.execute;
  const pmbcServiceFindByPin = pmbcService.findByPin;
  const pmbcServiceFindByPid = pmbcService.findByPid;
  const pmbcServiceFindByPlanNumber = pmbcService.findByPlanNumber;

  const pmbcServiceFindOne = pmbcService.findOne;
  const pimsPropertyLayerServiceFindOne = pimsPropertyLayerService.findOneByBoundary;

  const searchOneLocation = useCallback(
    async (latitude: number, longitude: number) => {
      let result: MapFeatureData = { ...emptyFeatureData };
      try {
        const findOneParcelTask = pmbcServiceFindOne({
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
            pmbcFeatures: {
              type: 'FeatureCollection',
              features: [parcelFeature],
            },
          };
        } else {
          toast.info('No search results found');
        }
      } catch (error) {
        toast.error((error as Error).message, { autoClose: 7000 });
      }
      return result;
    },
    [pmbcServiceFindOne, pimsPropertyLayerServiceFindOne],
  );

  const searchByPlanNumber = useCallback(
    async (filter?: IGeoSearchParams) => {
      let result: MapFeatureData = emptyFeatureData;
      try {
        let findByPlanNumberTask:
          | Promise<FeatureCollection<Geometry, PMBC_Feature_Properties> | undefined>
          | undefined = undefined;

        const loadPropertiesTask = loadPimsProperties(filter);

        const forceExactMatch = true;

        if (filter?.SURVEY_PLAN_NUMBER) {
          findByPlanNumberTask = pmbcServiceFindByPlanNumber(
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
            variant: 'error',
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
        const planNumberPmbcData = await findByPlanNumberTask;

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
            pmbcFeatures: emptyPmbcFeatureCollection,
          };

          if (validFeatures.length === 0) {
            toast.info('No search results found');
          } else {
            toast.info(`${validFeatures.length} properties found`);
          }
        } else {
          const attributedFeatures: FeatureCollection<Geometry, PMBC_Feature_Properties> = {
            type: 'FeatureCollection',
            features: [...(planNumberPmbcData?.features || [])],
            bbox: planNumberPmbcData?.bbox,
          };
          const validFeatures = attributedFeatures.features.filter(feature => !!feature?.geometry);
          result = {
            pimsLocationFeatures: emptyPimsLocationFeatureCollection,
            pimsBoundaryFeatures: emptyPimsBoundaryFeatureCollection,
            pmbcFeatures: {
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
      }

      return result;
    },
    [pmbcServiceFindByPlanNumber, loadPimsProperties, logout, setDisplayModal, setModalContent],
  );

  const searchMany = useCallback(
    async (filter?: IGeoSearchParams) => {
      let result: MapFeatureData = emptyFeatureData;
      //TODO: PSP-4390 currently this loads all matching properties, this should be rewritten to use the bbox and make one request per tile.
      try {
        let findByPinTask:
          | Promise<FeatureCollection<Geometry, PMBC_Feature_Properties> | undefined>
          | undefined = undefined;

        let findByPidTask:
          | Promise<FeatureCollection<Geometry, PMBC_Feature_Properties> | undefined>
          | undefined = undefined;

        const loadPropertiesTask = loadPimsProperties(filter);
        if (filter?.PIN) {
          findByPinTask = pmbcServiceFindByPin(filter?.PIN);
        }
        if (filter?.PID) {
          findByPidTask = pmbcServiceFindByPid(filter?.PID);
        }

        let pidPinInventoryData:
          | FeatureCollection<Geometry, PIMS_Property_Location_View>
          | undefined;
        try {
          pidPinInventoryData = await loadPropertiesTask;
        } catch (err) {
          setModalContent({
            variant: 'error',
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

        const [pinPmbcData, pidPmbcData] = await Promise.all([findByPinTask, findByPidTask]);

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
            pmbcFeatures: emptyPmbcFeatureCollection,
          };

          if (validFeatures.length === 0) {
            toast.info('No search results found');
          } else {
            toast.info(`${validFeatures.length} properties found`);
          }
        } else {
          const attributedFeatures: FeatureCollection<Geometry, PMBC_Feature_Properties> = {
            type: 'FeatureCollection',
            features: [...(pinPmbcData?.features || []), ...(pidPmbcData?.features || [])],
            bbox: pinPmbcData?.bbox || pidPmbcData?.bbox,
          };

          const validFeatures = attributedFeatures.features.filter(feature => !!feature?.geometry);
          result = {
            pimsLocationFeatures: emptyPimsLocationFeatureCollection,
            pimsBoundaryFeatures: emptyPimsBoundaryFeatureCollection,
            pmbcFeatures: {
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
      }

      return result;
    },
    [
      logout,
      setDisplayModal,
      setModalContent,
      loadPimsProperties,
      pmbcServiceFindByPin,
      pmbcServiceFindByPid,
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
