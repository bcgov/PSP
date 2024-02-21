import { AxiosResponse } from 'axios';
import { FeatureCollection, GeoJsonProperties, Geometry } from 'geojson';
import { useEffect, useMemo, useState } from 'react';

import { ComposedProperty } from '@/features/mapSideBar/property/ComposedProperty';
import { LtsaOrders } from '@/interfaces/ltsaModels';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { ApiGen_Concepts_PropertyAssociations } from '@/models/api/generated/ApiGen_Concepts_PropertyAssociations';
import { IBcAssessmentSummary } from '@/models/layers/bcAssesment';
import { useTenant } from '@/tenants/useTenant';
import { isValidId } from '@/utils';

import { useGeoServer } from '../layer-api/useGeoServer';
import { IWfsGetAllFeaturesOptions } from '../layer-api/useWfsLayer';
import { useLtsa } from '../useLtsa';
import { IResponseWrapper } from '../util/useApiRequestWrapper';
import useDeepCompareCallback from '../util/useDeepCompareCallback';
import { useParcelMapLayer } from './mapLayer/useParcelMapLayer';
import { useBcAssessmentLayer } from './useBcAssessmentLayer';
import { usePimsPropertyRepository } from './usePimsPropertyRepository';
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

export default interface ComposedPropertyState {
  pid?: string;
  pin?: string;
  id?: number;
  ltsaWrapper?: IResponseWrapper<(pid: string) => Promise<AxiosResponse<LtsaOrders, any>>>;
  apiWrapper?: IResponseWrapper<
    (id: number) => Promise<AxiosResponse<ApiGen_Concepts_Property, any>>
  >;
  propertyAssociationWrapper?: IResponseWrapper<
    (id: number) => Promise<AxiosResponse<ApiGen_Concepts_PropertyAssociations, any>>
  >;
  parcelMapWrapper?: IResponseWrapper<
    (
      filter?: Record<string, string>,
      options?: IWfsGetAllFeaturesOptions | undefined,
    ) => Promise<AxiosResponse<FeatureCollection<Geometry, GeoJsonProperties>, any>>
  >;
  geoserverWrapper?: IResponseWrapper<
    (id: number) => Promise<AxiosResponse<FeatureCollection<Geometry, GeoJsonProperties>, any>>
  >;
  bcAssessmentWrapper?: IResponseWrapper<
    (pid: string) => Promise<AxiosResponse<IBcAssessmentSummary, any>>
  >;
  composedLoading: boolean;
  composedProperty: ComposedProperty;
}

export interface IUseComposedPropertiesProps {
  id?: number;
  pid?: number;
  propertyTypes: PROPERTY_TYPES[];
}

export const useComposedProperties = ({
  id,
  pid,
  propertyTypes,
}: IUseComposedPropertiesProps): ComposedPropertyState => {
  const { getPropertyWrapper } = usePimsPropertyRepository();
  const { getPropertyWfsWrapper } = useGeoServer();
  const getLtsaWrapper = useLtsa();
  const getPropertyAssociationsWrapper = usePropertyAssociations();
  const { bcAssessment } = useTenant();
  const { findByPid, findByPin, findByWrapper } = useParcelMapLayer();
  const { getSummaryWrapper } = useBcAssessmentLayer(bcAssessment.url, bcAssessment.names);
  const retrievedPid = getPropertyWrapper?.response?.pid?.toString() ?? pid?.toString();
  const retrievedPin = getPropertyWrapper?.response?.pin?.toString();

  const [composedProperty, setComposedProperty] = useState<ComposedProperty>({
    pid: undefined,
    pin: undefined,
    id: undefined,
    ltsaOrders: undefined,
    pimsProperty: undefined,
    propertyAssociations: undefined,
    parcelMapFeatureCollection: undefined,
    geoserverFeatureCollection: undefined,
    bcAssessmentSummary: undefined,
  });

  const typeCheckWrapper = useDeepCompareCallback(
    (callback: () => void, currentType: PROPERTY_TYPES) => {
      if (propertyTypes.includes(currentType)) {
        callback();
      }
    },
    [propertyTypes],
  );
  const executeGetApiProperty = getPropertyWrapper.execute;
  const executeGetPropertyWfs = getPropertyWfsWrapper.execute;
  const executeGetPropertyAssociations = getPropertyAssociationsWrapper.execute;

  useEffect(() => {
    if (isValidId(id)) {
      typeCheckWrapper(() => executeGetApiProperty(id), PROPERTY_TYPES.PIMS_API);
      typeCheckWrapper(() => executeGetPropertyWfs(id), PROPERTY_TYPES.PIMS_GEOSERVER);
      typeCheckWrapper(() => executeGetPropertyAssociations(id), PROPERTY_TYPES.ASSOCIATIONS);
    }
  }, [
    executeGetApiProperty,
    executeGetPropertyAssociations,
    executeGetPropertyWfs,
    id,
    typeCheckWrapper,
  ]);

  const executeGetLtsa = getLtsaWrapper.execute;
  const executeBcAssessmentSummary = getSummaryWrapper.execute;

  useEffect(() => {
    if (retrievedPid !== undefined) {
      typeCheckWrapper(() => executeGetLtsa(retrievedPid ?? ''), PROPERTY_TYPES.LTSA);
      typeCheckWrapper(
        () => findByPid((retrievedPid ?? '').padStart(9, '0'), true),
        PROPERTY_TYPES.PARCEL_MAP,
      );
      typeCheckWrapper(
        () => executeBcAssessmentSummary(retrievedPid ?? ''),
        PROPERTY_TYPES.BC_ASSESSMENT,
      );
    } else if (retrievedPin) {
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

  useEffect(() => {
    setComposedProperty({
      id: id,
      pid: retrievedPid,
      pin: retrievedPin,
      ltsaOrders: getLtsaWrapper.response,
      pimsProperty: getPropertyWrapper.response,
      propertyAssociations: getPropertyAssociationsWrapper.response,
      parcelMapFeatureCollection: findByWrapper.response,
      geoserverFeatureCollection: getPropertyWfsWrapper.response,
      bcAssessmentSummary: getSummaryWrapper.response,
    });
  }, [
    setComposedProperty,
    id,
    retrievedPid,
    retrievedPin,
    getLtsaWrapper.response,
    getPropertyWrapper.response,
    getPropertyAssociationsWrapper.response,
    findByWrapper.response,
    getPropertyWfsWrapper.response,
    getSummaryWrapper.response,
  ]);

  return useMemo(
    () => ({
      id: id,
      pid: pid?.toString() ?? retrievedPid,
      pin: retrievedPin,
      composedProperty: composedProperty,
      ltsaWrapper: getLtsaWrapper,
      apiWrapper: getPropertyWrapper,
      propertyAssociationWrapper: getPropertyAssociationsWrapper,
      parcelMapWrapper: findByWrapper,
      geoserverWrapper: getPropertyWfsWrapper,
      bcAssessmentWrapper: getSummaryWrapper,
      composedLoading:
        getLtsaWrapper?.loading ||
        getPropertyWrapper?.loading ||
        getPropertyAssociationsWrapper?.loading ||
        findByWrapper?.loading ||
        getPropertyWfsWrapper?.loading ||
        getSummaryWrapper?.loading,
    }),
    [
      id,
      pid,
      retrievedPid,
      retrievedPin,
      composedProperty,
      getLtsaWrapper,
      getPropertyWrapper,
      getPropertyAssociationsWrapper,
      findByWrapper,
      getPropertyWfsWrapper,
      getSummaryWrapper,
    ],
  );
};
