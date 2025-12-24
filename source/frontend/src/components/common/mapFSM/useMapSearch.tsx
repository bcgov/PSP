import bbox from '@turf/bbox';
import { FeatureCollection, Geometry } from 'geojson';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { IGeoSearchParams } from '@/constants/API';
import { useCrownLandLayer } from '@/hooks/repositories/mapLayer/useCrownLandLayer';
import { useFullyAttributedParcelMapLayer } from '@/hooks/repositories/mapLayer/useFullyAttributedParcelMapLayer';
import { usePimsHighwayLayer } from '@/hooks/repositories/mapLayer/useHighwayLayer';
import { usePimsPropertyLayer } from '@/hooks/repositories/mapLayer/usePimsPropertyLayer';
import { usePimsPropertyRepository } from '@/hooks/repositories/usePimsPropertyRepository';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { useModalContext } from '@/hooks/useModalContext';
import { defaultPropertyFilterCriteria } from '@/models/api/ProjectFilterCriteria';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { ISS_ProvincialPublicHighway } from '@/models/layers/pimsHighwayLayer';
import {
  emptyProperty,
  PIMS_Property_Lite_View,
  PIMS_Property_View,
} from '@/models/layers/pimsPropertyView';
import { exists } from '@/utils';

import {
  emptyFeatureData,
  emptyHighwayFeatures,
  emptyPimsFeatureCollection,
  emptyPimsLiteFeatureCollection,
  emptyPmbcFeatureCollection,
  emptySurveyedParcelsFeatures,
  MapFeatureData,
} from './models';

export const useMapSearch = () => {
  const fullyAttributedService = useFullyAttributedParcelMapLayer();
  const highwayService = usePimsHighwayLayer();
  const pimsPropertyLayerService = usePimsPropertyLayer();
  const crownLandService = useCrownLandLayer();
  const { getMatchingProperties } = usePimsPropertyRepository();

  const { setModalContent, setDisplayModal } = useModalContext();
  const keycloak = useKeycloakWrapper();
  const logout = keycloak.obj.logout;

  const loadPimsPropertiesMinimal = pimsPropertyLayerService.loadPropertyLayerMinimal.execute;
  const loadPimsProperties = pimsPropertyLayerService.loadPropertyLayer.execute;
  const pmbcServiceFindByPin = fullyAttributedService.findByPin;
  const pmbcServiceFindByPid = fullyAttributedService.findByPid;
  const pmbcServiceFindByPlanNumber = fullyAttributedService.findByPlanNumber;
  const highwayServiceFindByPlanNumber = highwayService.findBySurveyPlanNumber;

  const pmbcServiceFindOne = fullyAttributedService.findOne;
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
            pimsFeatures: {
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
          | Promise<
              FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties> | undefined
            >
          | undefined = undefined;

        let findByHighwayPlanNumberTask:
          | Promise<FeatureCollection<Geometry, ISS_ProvincialPublicHighway> | undefined>
          | undefined = undefined;

        const loadPropertiesTask = loadPimsProperties(filter);

        const forceExactMatch = true;

        if (filter?.SURVEY_PLAN_NUMBER) {
          findByPlanNumberTask = pmbcServiceFindByPlanNumber(
            filter?.SURVEY_PLAN_NUMBER,
            forceExactMatch,
          );

          findByHighwayPlanNumberTask = highwayServiceFindByPlanNumber(
            filter?.SURVEY_PLAN_NUMBER,
            forceExactMatch,
          );
        }

        let planNumberInventoryData: FeatureCollection<Geometry, PIMS_Property_View> | undefined;
        try {
          planNumberInventoryData = await loadPropertiesTask;
        } catch {
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

        const planHighwayData = await findByHighwayPlanNumberTask;

        const validFeatures = planNumberInventoryData?.features?.filter(feature => exists(feature));

        const attributedFeatures: FeatureCollection<
          Geometry,
          PMBC_FullyAttributed_Feature_Properties
        > =
          planNumberPmbcData?.features?.length > 0
            ? {
                type: 'FeatureCollection',
                features: [...(planNumberPmbcData?.features || [])],
                bbox: planNumberPmbcData?.bbox,
              }
            : null;
        const validPmbcFeatures = attributedFeatures?.features?.filter(feature => exists(feature));

        result = {
          pimsFeatures: exists(validFeatures)
            ? {
                type: planNumberInventoryData.type,
                bbox: planNumberInventoryData.bbox,
                features: validFeatures,
              }
            : emptyPimsFeatureCollection,
          pimsLiteFeatures: emptyPimsLiteFeatureCollection,
          fullyAttributedFeatures: exists(validPmbcFeatures)
            ? {
                type: attributedFeatures.type,
                bbox: attributedFeatures.bbox,
                features: validPmbcFeatures,
              }
            : emptyPmbcFeatureCollection,
          highwayPlanFeatures: planHighwayData,
          surveyedParcelsFeatures: emptySurveyedParcelsFeatures,
        };

        if ((validFeatures?.length ?? 0) + (validPmbcFeatures?.length ?? 0) === 0) {
          toast.info('No search results found');
        } else {
          toast.info(
            `${(validFeatures?.length ?? 0) + (validPmbcFeatures?.length ?? 0)} properties found`,
          );
        }
      } catch (error) {
        toast.error((error as Error).message, { autoClose: 7000 });
      }

      return result;
    },
    [
      loadPimsProperties,
      pmbcServiceFindByPlanNumber,
      highwayServiceFindByPlanNumber,
      setModalContent,
      setDisplayModal,
      logout,
    ],
  );

  const searchByProject = useCallback(
    async (filter?: IGeoSearchParams) => {
      let result: MapFeatureData = emptyFeatureData;
      try {
        let findPropertyIdsByProjectTask: Promise<number[]> | undefined = undefined;

        const loadPropertiesTask = loadPimsProperties(filter);

        if (exists(filter?.PROJECT)) {
          findPropertyIdsByProjectTask = getMatchingProperties.execute({
            ...defaultPropertyFilterCriteria,
            projectId: +filter?.PROJECT,
          });
        }

        const [properties, projectPropertyIds] = await Promise.all([
          loadPropertiesTask,
          findPropertyIdsByProjectTask,
        ]);

        const validFeatures = properties.features?.filter(
          feature =>
            !!feature?.geometry && projectPropertyIds.includes(feature.properties.PROPERTY_ID),
        );

        result = {
          pimsFeatures: exists(validFeatures)
            ? {
                type: 'FeatureCollection',
                bbox: bbox({ type: 'FeatureCollection', features: validFeatures }),
                features: validFeatures,
              }
            : emptyPimsFeatureCollection,
          pimsLiteFeatures: emptyPimsLiteFeatureCollection,
          fullyAttributedFeatures: emptyPmbcFeatureCollection,
          highwayPlanFeatures: emptyHighwayFeatures,
          surveyedParcelsFeatures: emptySurveyedParcelsFeatures,
        };

        if ((validFeatures?.length ?? 0) === 0) {
          toast.info('No search results found');
        }
      } catch (error) {
        toast.error((error as Error).message, { autoClose: 7000 });
      }

      return result;
    },
    [loadPimsProperties, getMatchingProperties],
  );

  const searchByHistorical = useCallback(
    async (filter?: IGeoSearchParams) => {
      let result: MapFeatureData = emptyFeatureData;
      try {
        const loadPropertiesTask = loadPimsProperties(filter);

        let historicalNumberInventoryData:
          | FeatureCollection<Geometry, PIMS_Property_View>
          | undefined;
        try {
          historicalNumberInventoryData = await loadPropertiesTask;
        } catch {
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

        // If the property was found on the pims inventory, use that.
        if (
          historicalNumberInventoryData?.features &&
          historicalNumberInventoryData?.features?.length > 0
        ) {
          const validFeatures = historicalNumberInventoryData.features.filter(feature =>
            exists(feature),
          );

          result = {
            pimsFeatures: {
              type: historicalNumberInventoryData.type,
              bbox: historicalNumberInventoryData.bbox,
              features: validFeatures,
            },
            pimsLiteFeatures: emptyPimsLiteFeatureCollection,
            fullyAttributedFeatures: emptyPmbcFeatureCollection,
            surveyedParcelsFeatures: emptySurveyedParcelsFeatures,
            highwayPlanFeatures: emptyHighwayFeatures,
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
    [loadPimsProperties, setModalContent, setDisplayModal, logout],
  );

  const searchBySurveyParcel = useCallback(
    async (filter?: IGeoSearchParams) => {
      let result: MapFeatureData = emptyFeatureData;
      try {
        const response = await crownLandService.findMultipleSurveyParcel(
          filter?.SECTION,
          filter?.TOWNSHIP,
          filter?.RANGE,
          filter?.DISTRICT,
          filter?.DISTRICT_LOT,
        );

        const validCrownSurveyFeatures = response?.features?.filter(feature =>
          exists(feature?.geometry),
        );

        result = {
          pimsFeatures: emptyPimsFeatureCollection,
          pimsLiteFeatures: emptyPimsLiteFeatureCollection,
          fullyAttributedFeatures: emptyPmbcFeatureCollection,
          surveyedParcelsFeatures: exists(validCrownSurveyFeatures)
            ? {
                type: response?.type,
                bbox: response.bbox,
                features: validCrownSurveyFeatures,
              }
            : emptySurveyedParcelsFeatures,
          highwayPlanFeatures: emptyHighwayFeatures,
        };

        if (response?.features?.length === 0) {
          toast.info('No search results found');
        } else {
          toast.info(`${response?.features.length ?? 0} properties found`);
        }
      } catch (error) {
        toast.error((error as Error).message, { autoClose: 7000 });
      }

      return result;
    },
    [crownLandService],
  );

  const searchMany = useCallback(
    async (filter?: IGeoSearchParams) => {
      let result: MapFeatureData = emptyFeatureData;
      try {
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

        const loadPropertiesTask = loadPimsProperties(filter);
        if (filter?.PIN) {
          findByPinTask = pmbcServiceFindByPin(filter?.PIN);
        }
        if (filter?.PID) {
          findByPidTask = pmbcServiceFindByPid(filter?.PID);
        }

        let pidPinInventoryData: FeatureCollection<Geometry, PIMS_Property_View> | undefined;
        try {
          pidPinInventoryData = await loadPropertiesTask;
        } catch {
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

        const attributedFeatures: FeatureCollection<
          Geometry,
          PMBC_FullyAttributed_Feature_Properties
        > = {
          type: 'FeatureCollection',
          features: [...(pinPmbcData?.features || []), ...(pidPmbcData?.features || [])],
          bbox: pinPmbcData?.bbox || pidPmbcData?.bbox,
        };
        const validPimsFeatures = pidPinInventoryData.features.filter(feature => exists(feature));

        //filter out any pmbc features that do not have geometry, or are part of the pims feature result set.
        const validPmbcFeatures = attributedFeatures.features.filter(feature => exists(feature));
        result = {
          pimsFeatures: validPimsFeatures.length
            ? {
                type: pidPinInventoryData.type,
                bbox: pidPinInventoryData.bbox,
                features: validPimsFeatures,
              }
            : emptyPimsFeatureCollection,
          pimsLiteFeatures: emptyPimsLiteFeatureCollection,
          fullyAttributedFeatures: validPmbcFeatures
            ? {
                type: attributedFeatures.type,
                bbox: attributedFeatures.bbox,
                features: validPmbcFeatures,
              }
            : emptyPmbcFeatureCollection,
          surveyedParcelsFeatures: emptySurveyedParcelsFeatures,
          highwayPlanFeatures: emptyHighwayFeatures,
        };

        if (validPmbcFeatures.length === 0 && validPimsFeatures.length === 0) {
          toast.info('No search results found');
        } else {
          toast.info(`${validPmbcFeatures.length + validPimsFeatures.length} properties found`);
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

  const loadMapProperties = useCallback(async () => {
    let result: MapFeatureData = emptyFeatureData;
    try {
      const loadPropertiesTask = loadPimsPropertiesMinimal();

      let pidPinInventoryData: FeatureCollection<Geometry, PIMS_Property_Lite_View> | undefined;
      try {
        pidPinInventoryData = await loadPropertiesTask;
      } catch {
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

      // If the property was found on the pims inventory, use that.
      if (pidPinInventoryData?.features && pidPinInventoryData?.features?.length > 0) {
        const validFeatures = pidPinInventoryData.features.filter(
          feature => exists(feature?.geometry) || exists(feature?.properties?.LOCATION),
        );

        result = {
          pimsLiteFeatures: {
            type: pidPinInventoryData.type,
            bbox: pidPinInventoryData.bbox,
            features: validFeatures.map(vf => ({
              type: vf.type,
              geometry: vf.geometry ?? vf?.properties?.LOCATION,
              id: vf.id,
              properties: {
                ...emptyProperty,
                ...vf.properties,
              },
            })),
          },
          pimsFeatures: emptyPimsFeatureCollection,
          fullyAttributedFeatures: emptyPmbcFeatureCollection,
          surveyedParcelsFeatures: emptySurveyedParcelsFeatures,
          highwayPlanFeatures: emptyHighwayFeatures,
        };
      } else {
        result = {
          pimsFeatures: emptyPimsFeatureCollection,
          pimsLiteFeatures: emptyPimsLiteFeatureCollection,
          fullyAttributedFeatures: emptyPmbcFeatureCollection,
          surveyedParcelsFeatures: emptySurveyedParcelsFeatures,
          highwayPlanFeatures: emptyHighwayFeatures,
        };
      }
    } catch (error) {
      toast.error((error as Error).message, { autoClose: 7000 });
    }

    return result;
  }, [logout, setDisplayModal, setModalContent, loadPimsPropertiesMinimal]);

  return {
    searchOneLocation,
    searchByPlanNumber,
    searchByProject,
    searchMany,
    loadMapProperties,
    searchByHistorical,
    searchBySurveyParcel,
    loadingPimsProperties: pimsPropertyLayerService.loadPropertyLayer,
    loadingPimsPropertiesResponse: pimsPropertyLayerService.loadPropertyLayer.response,
  };
};
