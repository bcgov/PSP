import { AxiosResponse } from 'axios';
import { Feature, FeatureCollection, GeoJsonProperties, Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { useEffect, useMemo, useState } from 'react';

import { ComposedProperty } from '@/features/mapSideBar/property/ComposedProperty';
import { LtsaOrders } from '@/interfaces/ltsaModels';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { ApiGen_Concepts_PropertyAssociations } from '@/models/api/generated/ApiGen_Concepts_PropertyAssociations';
import { IBcAssessmentSummary } from '@/models/layers/bcAssesment';
import { TANTALIS_CrownLandTenures_Feature_Properties } from '@/models/layers/crownLand';
import { useTenant } from '@/tenants/useTenant';
import { exists, isValidId } from '@/utils';

import { useGeoServer } from '../layer-api/useGeoServer';
import { IWfsGetAllFeaturesOptions } from '../layer-api/useWfsLayer';
import { useLtsa } from '../useLtsa';
import { IResponseWrapper } from '../util/useApiRequestWrapper';
import useDeepCompareCallback from '../util/useDeepCompareCallback';
import { useCrownLandLayer } from './mapLayer/useCrownLandLayer';
import { useFullyAttributedParcelMapLayer } from './mapLayer/useFullyAttributedParcelMapLayer';
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
  CROWN_TENURES = 'CROWN_TENURES',
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
  pin?: number;
  latLng?: LatLngLiteral;
  propertyTypes: PROPERTY_TYPES[];
}

export const useComposedProperties = ({
  id,
  pid,
  pin,
  latLng,
  propertyTypes,
}: IUseComposedPropertiesProps): ComposedPropertyState => {
  const { getPropertyWrapper } = usePimsPropertyRepository();
  const { getPropertyWfsWrapper } = useGeoServer();
  const getLtsaWrapper = useLtsa();
  const getPropertyAssociationsWrapper = usePropertyAssociations();
  const { bcAssessment } = useTenant();
  const { findByPid, findByPin, findByWrapper } = useFullyAttributedParcelMapLayer();
  const { getSummaryWrapper } = useBcAssessmentLayer(bcAssessment.url, bcAssessment.names);

  const { findOneCrownLandTenure, findOneCrownLandTenureLoading } = useCrownLandLayer();
  const [crownResponse, setCrownResponse] = useState<
    Feature<Geometry, TANTALIS_CrownLandTenures_Feature_Properties> | undefined
  >();

  const retrievedPid = getPropertyWrapper?.response?.pid?.toString() ?? pid?.toString();
  const retrievedPin = getPropertyWrapper?.response?.pin?.toString() ?? pin?.toString();

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
    crownTenureFeature: undefined,
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

  // calls to PIMS api
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

  // calls to 3rd-party services (ie LTSA, ParcelMap, Tantalis Crown Land)
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
    } else if (exists(retrievedPin)) {
      typeCheckWrapper(() => findByPin(retrievedPin, true), PROPERTY_TYPES.PARCEL_MAP);
    }

    // Crown land doesn't necessarily have a PIMS ID or PID or PIN so we need to use the lat/long of the selected property
    if (exists(latLng)) {
      typeCheckWrapper(async () => {
        const result = await findOneCrownLandTenure(latLng);
        setCrownResponse(result);
      }, PROPERTY_TYPES.CROWN_TENURES);
    }
  }, [
    findByPid,
    findByPin,
    executeGetLtsa,
    retrievedPid,
    retrievedPin,
    typeCheckWrapper,
    executeBcAssessmentSummary,
    findOneCrownLandTenure,
    latLng,
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
      crownTenureFeature: crownResponse,
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
    crownResponse,
  ]);

  return useMemo(
    () => ({
      id: id,
      pid: pid?.toString() ?? retrievedPid,
      pin: pin?.toString() ?? retrievedPin,
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
        getSummaryWrapper?.loading ||
        findOneCrownLandTenureLoading,
    }),
    [
      id,
      pid,
      retrievedPid,
      pin,
      retrievedPin,
      composedProperty,
      getLtsaWrapper,
      getPropertyWrapper,
      getPropertyAssociationsWrapper,
      findByWrapper,
      getPropertyWfsWrapper,
      getSummaryWrapper,
      findOneCrownLandTenureLoading,
    ],
  );
};
