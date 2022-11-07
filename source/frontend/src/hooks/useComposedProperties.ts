import ComposedProperty from 'features/properties/map/propertyInformation/ComposedProperty';
import { useEffect } from 'react';
import { useTenant } from 'tenants/useTenant';

import { useGetProperty } from '../features/mapSideBar/tabs/propertyDetails/hooks/useGetProperty';
import { useFullyAttributedParcelMapLayer } from './pims-api/useFullyAttributedParcelMapLayer';
import { useGeoServer } from './pims-api/useGeoServer';
import { useBcAssessmentLayer } from './useBcAssessmentLayer';
import useDeepCompareCallback from './useDeepCompareCallback';
import { useLtsa } from './useLtsa';
import { usePropertyAssociations } from './usePropertyAssociations';
export enum PROPERTY_TYPES {
  PIMS_API = 'PIMS_API',
  PIMS_GEOSERVER = 'PIMS_GEOSERVER',
  PARCEL_MAP = 'PARCEL_MAP',
  LTSA = 'LTSA',
  ASSOCIATIONS = 'ASSOCIATIONS',
  BC_ASSESSMENT = 'BC_ASSESSMENT',
}

export const ALL_PROPERTY_TYPES = Object.values(PROPERTY_TYPES);

export interface IUseComposedPropertiesProps {
  id?: number;
  pid?: number;
  propertyTypes: PROPERTY_TYPES[];
}

export const useComposedProperties = ({
  id,
  pid,
  propertyTypes,
}: IUseComposedPropertiesProps): ComposedProperty => {
  const getApiPropertyWrapper = useGetProperty();
  const { getPropertyWfsWrapper } = useGeoServer();
  const getLtsaWrapper = useLtsa();
  const getPropertyAssociationsWrapper = usePropertyAssociations();
  const { parcelMapFullyAttributed, bcAssessment } = useTenant();
  const { findByPid, findByPin, getAllFeaturesWrapper } = useFullyAttributedParcelMapLayer(
    parcelMapFullyAttributed.url,
    parcelMapFullyAttributed.name,
  );
  const { getSummaryWrapper } = useBcAssessmentLayer(bcAssessment.url, bcAssessment.names);
  const retrievedPid = getApiPropertyWrapper?.response?.pid?.toString();
  const retrievedPin = getApiPropertyWrapper?.response?.pin?.toString();

  const typeCheckWrapper = useDeepCompareCallback(
    (callback: () => void, currentType: PROPERTY_TYPES) => {
      if (propertyTypes.includes(currentType)) {
        callback();
      }
    },
    [propertyTypes],
  );
  const executeGetApiProperty = getApiPropertyWrapper.execute;
  const executeGetPropertyWfs = getPropertyWfsWrapper.execute;
  const executeGetPropertyAssociations = getPropertyAssociationsWrapper.execute;
  const executeGetAllFeatures = getAllFeaturesWrapper.execute;

  useEffect(() => {
    if (id !== undefined && !isNaN(id)) {
      typeCheckWrapper(() => executeGetApiProperty(id), PROPERTY_TYPES.PIMS_API);
      typeCheckWrapper(() => executeGetPropertyWfs(id), PROPERTY_TYPES.PIMS_GEOSERVER);
      typeCheckWrapper(() => executeGetPropertyAssociations(id), PROPERTY_TYPES.ASSOCIATIONS);
    }
    if (!!retrievedPid) {
      typeCheckWrapper(
        () =>
          executeGetAllFeatures({ PID: retrievedPid }, { forceSimplePid: true, timeout: 30000 }),
        PROPERTY_TYPES.PARCEL_MAP,
      );
    }
    if (!!retrievedPin) {
      typeCheckWrapper(
        () => executeGetAllFeatures({ PIN: retrievedPin }, { timeout: 30000 }),
        PROPERTY_TYPES.PARCEL_MAP,
      );
    }
  }, [
    executeGetApiProperty,
    executeGetPropertyAssociations,
    executeGetPropertyWfs,
    executeGetAllFeatures,
    id,
    retrievedPid,
    retrievedPin,
    typeCheckWrapper,
  ]);

  const executeGetLtsa = getLtsaWrapper.execute;
  const executeBcAssessmentSummary = getSummaryWrapper.execute;

  useEffect(() => {
    if (!!retrievedPid) {
      typeCheckWrapper(() => executeGetLtsa(retrievedPid), PROPERTY_TYPES.LTSA);
      typeCheckWrapper(() => findByPid(retrievedPid), PROPERTY_TYPES.PARCEL_MAP);
      typeCheckWrapper(
        () => executeBcAssessmentSummary(retrievedPid),
        PROPERTY_TYPES.BC_ASSESSMENT,
      );
    } else if (!!retrievedPin) {
      typeCheckWrapper(() => findByPin(retrievedPin, true), PROPERTY_TYPES.PARCEL_MAP);
    }
  }, [
    findByPid,
    findByPin,
    executeGetLtsa,
    retrievedPid,
    retrievedPin,
    typeCheckWrapper,
    executeBcAssessmentSummary,
  ]);

  return {
    id: id,
    pid: pid?.toString() ?? retrievedPid,
    pin: retrievedPin,
    ltsaWrapper: getLtsaWrapper,
    apiWrapper: getApiPropertyWrapper,
    propertyAssociationWrapper: getPropertyAssociationsWrapper,
    parcelMapWrapper: getAllFeaturesWrapper,
    geoserverWrapper: getPropertyWfsWrapper,
    bcAssessmentWrapper: getSummaryWrapper,
    composedLoading:
      getLtsaWrapper?.loading ||
      getApiPropertyWrapper?.loading ||
      getPropertyAssociationsWrapper?.loading ||
      getAllFeaturesWrapper?.loading ||
      getPropertyWfsWrapper?.loading ||
      getSummaryWrapper?.loading,
  };
};
