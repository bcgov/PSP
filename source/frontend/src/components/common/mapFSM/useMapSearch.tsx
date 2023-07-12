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
  emptyPimsFeatureCollection,
  MapFeatureData,
} from './models';

export const useMapSearch = () => {
  const fullyAttributedService = useFullyAttributedParcelMapLayer();
  const pimsPropertyLayerService = usePimsPropertyLayer();

  const { setModalContent, setDisplayModal } = useModalContext();
  const keycloak = useKeycloakWrapper();
  const logout = keycloak.obj.logout;

  const pimsExecute = pimsPropertyLayerService.loadPropertyLayer.execute;
  const fullyAttributedServiceFindByPin = fullyAttributedService.findByPin;
  const fullyAttributedServiceFindByPid = fullyAttributedService.findByPid;

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

        loadPropertiesTask = pimsExecute(filter);
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
            pimsFeatures: pidPinInventoryData,
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
            pimsFeatures: emptyPimsFeatureCollection,
            fullyAttributedFeatures: attributedFeatures,
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
      logout,
      setDisplayModal,
      setModalContent,
      pimsExecute,
      fullyAttributedServiceFindByPin,
      fullyAttributedServiceFindByPid,
    ],
  );

  return {
    searchMany,
    loadingPimsProperties: pimsPropertyLayerService.loadPropertyLayer,
    loadingPimsPropertiesResponse: pimsPropertyLayerService.loadPropertyLayer.response,
  };
};
