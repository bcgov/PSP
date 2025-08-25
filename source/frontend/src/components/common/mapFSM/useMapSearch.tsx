import { FeatureCollection, Geometry } from 'geojson';
import { useCallback } from 'react';
import { toast } from 'react-toastify';

import { IGeoSearchParams } from '@/constants/API';
import { useCrownLandLayer } from '@/hooks/repositories/mapLayer/useCrownLandLayer';
import { useFullyAttributedParcelMapLayer } from '@/hooks/repositories/mapLayer/useFullyAttributedParcelMapLayer';
import { usePimsHighwayLayer } from '@/hooks/repositories/mapLayer/useHighwayLayer';
import { usePimsPropertyLayer } from '@/hooks/repositories/mapLayer/usePimsPropertyLayer';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { useModalContext } from '@/hooks/useModalContext';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { ISS_ProvincialPublicHighway } from '@/models/layers/pimsHighwayLayer';
import {
  emptyPropertyLocation,
  PIMS_Property_Lite_View,
  PIMS_Property_Location_View,
} from '@/models/layers/pimsPropertyLocationView';
import { exists } from '@/utils';

import {
  emptyFeatureData,
  emptyHighwayFeatures,
  emptyPimsBoundaryFeatureCollection,
  emptyPimsLocationFeatureCollection,
  emptyPimsLocationLiteFeatureCollection,
  emptyPmbcFeatureCollection,
  emptySurveyedParcelsFeatures,
  MapFeatureData,
} from './models';

export const useMapSearch = () => {
  const fullyAttributedService = useFullyAttributedParcelMapLayer();
  const highwayService = usePimsHighwayLayer();
  const pimsPropertyLayerService = usePimsPropertyLayer();
  const crownLandService = useCrownLandLayer();

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

        let planNumberInventoryData:
          | FeatureCollection<Geometry, PIMS_Property_Location_View>
          | undefined;
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

        const validFeatures = planNumberInventoryData?.features?.filter(
          feature => !!feature?.geometry,
        );

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
        const validPmbcFeatures = attributedFeatures?.features?.filter(
          feature => !!feature?.geometry,
        );

        result = {
          pimsLocationFeatures: exists(validFeatures)
            ? {
                type: planNumberInventoryData.type,
                bbox: planNumberInventoryData.bbox,
                features: validFeatures,
              }
            : emptyPimsLocationFeatureCollection,
          pimsLocationLiteFeatures: emptyPimsLocationLiteFeatureCollection,
          pimsBoundaryFeatures: emptyPimsBoundaryFeatureCollection,
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

  const searchByHistorical = useCallback(
    async (filter?: IGeoSearchParams) => {
      let result: MapFeatureData = emptyFeatureData;
      try {
        const loadPropertiesTask = loadPimsProperties(filter);

        let historicalNumberInventoryData:
          | FeatureCollection<Geometry, PIMS_Property_Location_View>
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
          const validFeatures = historicalNumberInventoryData.features.filter(
            feature => !!feature?.geometry,
          );

          result = {
            pimsLocationFeatures: {
              type: historicalNumberInventoryData.type,
              bbox: historicalNumberInventoryData.bbox,
              features: validFeatures,
            },
            pimsLocationLiteFeatures: emptyPimsLocationLiteFeatureCollection,
            pimsBoundaryFeatures: emptyPimsBoundaryFeatureCollection,
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
        );

        const validCrownSurveyFeatures = response?.features?.filter(feature =>
          exists(feature?.geometry),
        );

        result = {
          pimsLocationFeatures: emptyPimsLocationFeatureCollection,
          pimsLocationLiteFeatures: emptyPimsLocationLiteFeatureCollection,
          pimsBoundaryFeatures: emptyPimsBoundaryFeatureCollection,
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

        let pidPinInventoryData:
          | FeatureCollection<Geometry, PIMS_Property_Location_View>
          | undefined;
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
        const validPimsFeatures = pidPinInventoryData.features.filter(
          feature => !!feature?.geometry,
        );

        //filter out any pmbc features that do not have geometry, or are part of the pims feature result set.
        const validPmbcFeatures = attributedFeatures.features.filter(
          feature => !!feature?.geometry,
        );
        result = {
          pimsLocationFeatures: validPimsFeatures.length
            ? {
                type: pidPinInventoryData.type,
                bbox: pidPinInventoryData.bbox,
                features: validPimsFeatures,
              }
            : emptyPimsLocationFeatureCollection,
          pimsLocationLiteFeatures: emptyPimsLocationLiteFeatureCollection,
          pimsBoundaryFeatures: emptyPimsBoundaryFeatureCollection,
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
          pimsLocationLiteFeatures: {
            type: pidPinInventoryData.type,
            bbox: pidPinInventoryData.bbox,
            features: validFeatures.map(vf => ({
              type: vf.type,
              geometry: vf.geometry ?? vf?.properties?.LOCATION,
              id: vf.id,
              properties: {
                ...emptyPropertyLocation,
                ...vf.properties,
              },
            })),
          },
          pimsLocationFeatures: emptyPimsLocationFeatureCollection,
          pimsBoundaryFeatures: emptyPimsBoundaryFeatureCollection,
          fullyAttributedFeatures: emptyPmbcFeatureCollection,
          surveyedParcelsFeatures: emptySurveyedParcelsFeatures,
          highwayPlanFeatures: emptyHighwayFeatures,
        };
      } else {
        result = {
          pimsLocationFeatures: emptyPimsLocationFeatureCollection,
          pimsLocationLiteFeatures: emptyPimsLocationFeatureCollection,
          pimsBoundaryFeatures: emptyPimsBoundaryFeatureCollection,
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
    searchMany,
    loadMapProperties,
    searchByHistorical,
    searchBySurveyParcel,
    loadingPimsProperties: pimsPropertyLayerService.loadPropertyLayer,
    loadingPimsPropertiesResponse: pimsPropertyLayerService.loadPropertyLayer.response,
  };
};
