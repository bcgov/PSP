import { point } from '@turf/turf';
import { AxiosResponse } from 'axios';
import { Feature, FeatureCollection, GeoJsonProperties, Geometry } from 'geojson';
import { useCallback, useEffect, useMemo, useState } from 'react';

import { ComposedProperty } from '@/features/mapSideBar/property/ComposedProperty';
import { LtsaOrders, SpcpOrder } from '@/interfaces/ltsaModels';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { ApiGen_Concepts_PropertyAssociations } from '@/models/api/generated/ApiGen_Concepts_PropertyAssociations';
import { ADM_IndianReserveBands_Feature_Properties } from '@/models/layers/admIndianReserveBands';
import { WHSE_AgriculturalLandReservePoly_Feature_Properties } from '@/models/layers/alcAgriculturalReserve';
import { IBcAssessmentSummary } from '@/models/layers/bcAssesment';
import {
  TANTALIS_CrownLandInclusions_Feature_Properties,
  TANTALIS_CrownLandInventory_Feature_Properties,
  TANTALIS_CrownLandLeases_Feature_Properties,
  TANTALIS_CrownLandLicenses_Feature_Properties,
  TANTALIS_CrownLandTenures_Feature_Properties,
} from '@/models/layers/crownLand';
import { EBC_ELECTORAL_DISTS_BS10_SVW_Feature_Properties } from '@/models/layers/electoralBoundaries';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { ISS_ProvincialPublicHighway } from '@/models/layers/pimsHighwayLayer';
import { useTenant } from '@/tenants/useTenant';
import { exists, firstOrNull, isPlanNumberSPCP, isValidId } from '@/utils';

import { useGeoServer } from '../layer-api/useGeoServer';
import { IWfsGetAllFeaturesOptions } from '../layer-api/useWfsLayer';
import { useLtsa } from '../useLtsa';
import { IResponseWrapper } from '../util/useApiRequestWrapper';
import useDeepCompareCallback from '../util/useDeepCompareCallback';
import { WHSE_Municipalities_Feature_Properties } from './../../models/layers/municipalities';
import { useAdminBoundaryMapLayer } from './mapLayer/useAdminBoundaryMapLayer';
import { useCrownLandLayer } from './mapLayer/useCrownLandLayer';
import { useFullyAttributedParcelMapLayer } from './mapLayer/useFullyAttributedParcelMapLayer';
import { usePimsHighwayLayer } from './mapLayer/useHighwayLayer';
import { useIndianReserveBandMapLayer } from './mapLayer/useIndianReserveBandMapLayer';
import { useLegalAdminBoundariesMapLayer } from './mapLayer/useLegalAdminBoundariesMapLayer';
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
  CROWN_LEASES = 'CROWN_LEASES',
  CROWN_LICENSES = 'CROWN_LICENSES',
  CROWN_INVENTORY = 'CROWN_INVENTORY',
  CROWN_INCLUSIONS = 'CROWN_INCLUSIONS',
  CROWN_SURVEYS = 'CROWN_SURVEYS',
  HIGHWAYS = 'HIGHWAY',
  MUNICIPALITY = 'MUNICIPALITY',
  ALR = 'ALR',
  FIRST_NATION = 'FIRST_NATION',
  ELECTORAL = 'ELECTORAL',
}

export default interface ComposedPropertyState {
  pid?: string;
  pin?: string;
  id?: number;
  planNumber?: string;
  ltsaWrapper?: IResponseWrapper<(pid: string) => Promise<AxiosResponse<LtsaOrders, any>>>;
  spcpWrapper?: IResponseWrapper<
    (strataPlanNumber: string) => Promise<AxiosResponse<SpcpOrder, any>>
  >;
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
  highwayLoading: boolean;
  municipalityLoading: boolean;
  electoralLoading: boolean;
  alrLoading: boolean;
  firstNationsLoading: boolean;
}

export interface IUseComposedPropertiesProps {
  id?: number;
  pid?: number;
  pin?: number;
  planNumber?: string;
  boundary?: Geometry;
  propertyTypes: PROPERTY_TYPES[];
}

export const useComposedProperties = ({
  id,
  pid,
  pin,
  planNumber,
  boundary,
  propertyTypes,
}: IUseComposedPropertiesProps): ComposedPropertyState => {
  const { getPropertyWrapper } = usePimsPropertyRepository();
  const { getPropertyWfsWrapper, getPropertyByBoundaryWfsWrapper } = useGeoServer();
  const { ltsaRequestWrapper, getStrataPlanCommonProperty } = useLtsa();
  const getPropertyAssociationsWrapper = usePropertyAssociations();
  const { bcAssessment } = useTenant();
  const {
    findByPid,
    findByPin,
    findByWrapper: findParcelByWrapper,
  } = useFullyAttributedParcelMapLayer();
  const { getSummaryWrapper } = useBcAssessmentLayer(bcAssessment.url, bcAssessment.names);
  const { findMultipleHighwayBoundary, findMultipleHighwayWhereContainsWrappedLoading } =
    usePimsHighwayLayer();
  const {
    findMultipleMunicipalityBoundary,
    findMultipleAgriculturalReserveBoundary,
    findMultipleAgriculturalReserveLoading,
    findMultipleMunicipalBoundaryLoading,
  } = useLegalAdminBoundariesMapLayer();
  const { findElectoralDistrictBoundary, findElectoralDistrictBoundaryLoading } =
    useAdminBoundaryMapLayer();
  const {
    findMultipleBoundary: findMultipleFirstNationsBoundary,
    findMultipleBoundaryLoading: findMultipleFirstNationsBoundaryLoading,
  } = useIndianReserveBandMapLayer();

  const {
    findMultipleCrownLandTenure,
    findMultipleCrownLandTenureLoading,
    findMultipleCrownLandInclusion,
    findMultipleCrownLandInclusionsLoading,
    findMultipleCrownLandInventory,
    findMultipleCrownLandInventoryLoading,
    findMultipleCrownLandLease,
    findMultipleCrownLandLicenseLoading,
    findMultipleCrownLandLicense,
    findMultipleCrownLandLeaseLoading,
  } = useCrownLandLayer();
  const [crownResponse, setCrownResponse] = useState<{
    crownTenureFeatures: Feature<Geometry, TANTALIS_CrownLandTenures_Feature_Properties>[];
    crownLeaseFeatures: Feature<Geometry, TANTALIS_CrownLandLeases_Feature_Properties>[];
    crownLicenseFeatures: Feature<Geometry, TANTALIS_CrownLandLicenses_Feature_Properties>[];
    crownInclusionFeatures: Feature<Geometry, TANTALIS_CrownLandInclusions_Feature_Properties>[];
    crownInventoryFeatures: Feature<Geometry, TANTALIS_CrownLandInventory_Feature_Properties>[];
  }>();
  const [highwayResponse, setHighwayResponse] = useState<
    Feature<Geometry, ISS_ProvincialPublicHighway>[] | undefined
  >();

  const [municipalityResponse, setMunicipalityResponse] = useState<
    Feature<Geometry, WHSE_Municipalities_Feature_Properties>[] | undefined
  >();
  const [alrResponse, setAlrResponse] = useState<
    Feature<Geometry, WHSE_AgriculturalLandReservePoly_Feature_Properties>[] | undefined
  >();
  const [electoralResponse, setElectoralResponse] = useState<
    Feature<Geometry, EBC_ELECTORAL_DISTS_BS10_SVW_Feature_Properties>[] | undefined
  >();
  const [firstNationsResponse, setFirstNationsResponse] = useState<
    Feature<Geometry, ADM_IndianReserveBands_Feature_Properties>[] | undefined
  >();
  const [retrievedBoundary, setRetrievedBoundary] = useState<Geometry>(boundary);

  // overwrite pid/pin/plan if the id is set, as that implies that the user explicitly clicked on a PIMS property.
  const retrievedPid = exists(id) ? getPropertyWrapper?.response?.pid?.toString() : pid?.toString();
  const retrievedPin = exists(id) ? getPropertyWrapper?.response?.pin?.toString() : pin?.toString();
  const retrievedPlanNumber = exists(id)
    ? getPropertyWrapper?.response?.planNumber?.toString()
    : planNumber?.toString();

  const getPimsProperty = getPropertyWrapper.execute;
  const getParcelMapParcel = findParcelByWrapper.execute;
  const parcelResponse = findParcelByWrapper.response as
    | FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties>
    | undefined;

  const getBoundaries = useCallback(async () => {
    if (exists(id)) {
      const result = await getPimsProperty(id);
      if (exists(result?.boundary)) {
        setRetrievedBoundary(result?.boundary);
      } else if (exists(result?.location)) {
        setRetrievedBoundary(
          point([result?.location?.coordinate?.x, result?.location?.coordinate?.y]).geometry,
        );
      }
    } else if (exists(pid)) {
      if (exists(pid)) {
        const result = await getParcelMapParcel(
          { PID_NUMBER: pid.toString() },
          { forceExactMatch: true },
        );
        setRetrievedBoundary(firstOrNull(result?.features)?.geometry);
      } else if (exists(pin)) {
        const result = await getParcelMapParcel({ PIN: pin.toString() }, { forceExactMatch: true });
        setRetrievedBoundary(firstOrNull(result?.features)?.geometry);
      }
    }
  }, [id, pid, getPimsProperty, pin, getParcelMapParcel]);

  useEffect(() => {
    if (!retrievedBoundary) {
      getBoundaries();
    }
  }, [retrievedBoundary, getBoundaries]);

  const [composedProperty, setComposedProperty] = useState<ComposedProperty>({
    pid: undefined,
    pin: undefined,
    planNumber: undefined,
    id: undefined,
    ltsaOrders: undefined,
    spcpOrder: undefined,
    pimsProperty: undefined,
    propertyAssociations: undefined,
    parcelMapFeatureCollection: undefined,
    pimsGeoserverFeatureCollection: undefined,
    bcAssessmentSummary: undefined,
    crownTenureFeatures: undefined,
    crownLeaseFeatures: undefined,
    crownLicenseFeatures: undefined,
    crownInclusionFeatures: undefined,
    crownInventoryFeatures: undefined,
    highwayFeatures: undefined,
    municipalityFeatures: undefined,
    firstNationFeatures: undefined,
    electoralFeatures: undefined,
    alrFeatures: undefined,
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
  const executeGetPropertyBoundaryWfs = getPropertyByBoundaryWfsWrapper.execute;

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

  //calls to pims proxy geoserver
  useEffect(() => {
    if (exists(retrievedBoundary) && !exists(id)) {
      typeCheckWrapper(
        () => executeGetPropertyBoundaryWfs(retrievedBoundary),
        PROPERTY_TYPES.PIMS_GEOSERVER,
      );
    }
  }, [retrievedBoundary, executeGetPropertyBoundaryWfs, id, typeCheckWrapper]);

  const executeGetLtsa = ltsaRequestWrapper.execute;
  const executeGetStrataLtsa = getStrataPlanCommonProperty.execute;
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
    } else if (exists(retrievedPlanNumber) && isPlanNumberSPCP(retrievedPlanNumber)) {
      typeCheckWrapper(() => executeGetStrataLtsa(retrievedPlanNumber), PROPERTY_TYPES.LTSA);
    }

    // Crown land doesn't necessarily have a PIMS ID or PID or PIN so we need to use the lat/long of the selected property
    if (exists(retrievedBoundary)) {
      (async () => {
        const [
          crownTenures,
          crownLeases,
          crownLicenses,
          crownInclusions,
          crownInventory,
          highway,
          municipality,
          alr,
          firstNations,
          electoral,
        ] = await Promise.all([
          propertyTypes.includes(PROPERTY_TYPES.CROWN_TENURES)
            ? findMultipleCrownLandTenure(retrievedBoundary)
            : Promise.resolve(undefined),
          propertyTypes.includes(PROPERTY_TYPES.CROWN_LEASES)
            ? findMultipleCrownLandLease(retrievedBoundary)
            : Promise.resolve(undefined),
          propertyTypes.includes(PROPERTY_TYPES.CROWN_LICENSES)
            ? findMultipleCrownLandLicense(retrievedBoundary)
            : Promise.resolve(undefined),
          propertyTypes.includes(PROPERTY_TYPES.CROWN_INCLUSIONS)
            ? findMultipleCrownLandInclusion(retrievedBoundary)
            : Promise.resolve(undefined),
          propertyTypes.includes(PROPERTY_TYPES.CROWN_INVENTORY)
            ? findMultipleCrownLandInventory(retrievedBoundary)
            : Promise.resolve(undefined),
          propertyTypes.includes(PROPERTY_TYPES.HIGHWAYS)
            ? findMultipleHighwayBoundary(retrievedBoundary)
            : Promise.resolve(undefined),
          propertyTypes.includes(PROPERTY_TYPES.MUNICIPALITY)
            ? findMultipleMunicipalityBoundary(retrievedBoundary)
            : Promise.resolve(undefined),
          propertyTypes.includes(PROPERTY_TYPES.ALR)
            ? findMultipleAgriculturalReserveBoundary(retrievedBoundary, 'GEOMETRY')
            : Promise.resolve(undefined),
          propertyTypes.includes(PROPERTY_TYPES.FIRST_NATION)
            ? findMultipleFirstNationsBoundary(retrievedBoundary, 'GEOMETRY')
            : Promise.resolve(undefined),
          propertyTypes.includes(PROPERTY_TYPES.ELECTORAL)
            ? findElectoralDistrictBoundary(retrievedBoundary)
            : Promise.resolve(undefined),
        ]);
        setCrownResponse({
          crownTenureFeatures: crownTenures ?? [],
          crownLeaseFeatures: crownLeases ?? [],
          crownLicenseFeatures: crownLicenses ?? [],
          crownInclusionFeatures: crownInclusions ?? [],
          crownInventoryFeatures: crownInventory ?? [],
        });
        setHighwayResponse(highway);
        setMunicipalityResponse(municipality);
        setAlrResponse(alr?.features);
        setFirstNationsResponse(firstNations?.features);
        setElectoralResponse(electoral?.features);
      })();
    }
  }, [
    findByPid,
    findByPin,
    executeGetLtsa,
    retrievedPid,
    retrievedPin,
    retrievedPlanNumber,
    typeCheckWrapper,
    executeBcAssessmentSummary,
    findMultipleCrownLandTenure,
    retrievedBoundary,
    executeGetStrataLtsa,
    propertyTypes,
    findMultipleCrownLandLease,
    findMultipleCrownLandLicense,
    findMultipleCrownLandInclusion,
    findMultipleCrownLandInventory,
    findMultipleHighwayBoundary,
    setHighwayResponse,
    findMultipleMunicipalityBoundary,
    findMultipleAgriculturalReserveBoundary,
    findMultipleFirstNationsBoundary,
    findElectoralDistrictBoundary,
  ]);

  useEffect(() => {
    setComposedProperty({
      id: id,
      pid: retrievedPid,
      pin: retrievedPin,
      planNumber: retrievedPlanNumber,
      ltsaOrders: ltsaRequestWrapper.response,
      spcpOrder: getStrataPlanCommonProperty.response,
      pimsProperty: getPropertyWrapper.response,
      propertyAssociations: getPropertyAssociationsWrapper.response,
      parcelMapFeatureCollection: parcelResponse,
      pimsGeoserverFeatureCollection:
        getPropertyWfsWrapper.response ?? getPropertyByBoundaryWfsWrapper.response,
      bcAssessmentSummary: getSummaryWrapper.response,
      ...crownResponse,
      highwayFeatures: highwayResponse,
      municipalityFeatures: municipalityResponse,
      electoralFeatures: electoralResponse,
      alrFeatures: alrResponse,
      firstNationFeatures: firstNationsResponse,
    });
  }, [
    setComposedProperty,
    id,
    retrievedPid,
    retrievedPin,
    getPropertyWrapper.response,
    getPropertyAssociationsWrapper.response,
    findParcelByWrapper.response,
    getPropertyWfsWrapper.response,
    getSummaryWrapper.response,
    crownResponse,
    retrievedPlanNumber,
    ltsaRequestWrapper.response,
    getStrataPlanCommonProperty.response,
    highwayResponse,
    municipalityResponse,
    getPropertyByBoundaryWfsWrapper.response,
    parcelResponse,
    electoralResponse,
    alrResponse,
    firstNationsResponse,
  ]);

  return useMemo(
    () => ({
      id: id,
      pid: pid?.toString() ?? retrievedPid,
      pin: pin?.toString() ?? retrievedPin,
      planNumber: planNumber?.toString() ?? retrievedPlanNumber,
      composedProperty: composedProperty,
      ltsaWrapper: ltsaRequestWrapper,
      spcpWrapper: getStrataPlanCommonProperty,
      apiWrapper: getPropertyWrapper,
      propertyAssociationWrapper: getPropertyAssociationsWrapper,
      parcelMapWrapper: findParcelByWrapper,
      geoserverWrapper: getPropertyWfsWrapper,
      bcAssessmentWrapper: getSummaryWrapper,
      composedLoading:
        ltsaRequestWrapper?.loading ||
        getPropertyWrapper?.loading ||
        getPropertyAssociationsWrapper?.loading ||
        getStrataPlanCommonProperty?.loading ||
        findParcelByWrapper?.loading ||
        getPropertyWfsWrapper?.loading ||
        getSummaryWrapper?.loading,
      // does not include optional crown/highway
      crownLoading:
        findMultipleCrownLandInclusionsLoading ||
        findMultipleCrownLandInventoryLoading ||
        findMultipleCrownLandLeaseLoading ||
        findMultipleCrownLandLicenseLoading ||
        findMultipleCrownLandTenureLoading,
      highwayLoading: findMultipleHighwayWhereContainsWrappedLoading,
      municipalityLoading: findMultipleMunicipalBoundaryLoading,
      electoralLoading: findElectoralDistrictBoundaryLoading,
      alrLoading: findMultipleAgriculturalReserveLoading,
      firstNationsLoading: findMultipleFirstNationsBoundaryLoading,
    }),
    [
      id,
      pid,
      retrievedPid,
      pin,
      retrievedPin,
      planNumber,
      retrievedPlanNumber,
      composedProperty,
      ltsaRequestWrapper,
      getStrataPlanCommonProperty,
      getPropertyWrapper,
      getPropertyAssociationsWrapper,
      findParcelByWrapper,
      getPropertyWfsWrapper,
      getSummaryWrapper,
      findMultipleCrownLandInclusionsLoading,
      findMultipleCrownLandInventoryLoading,
      findMultipleCrownLandLeaseLoading,
      findMultipleCrownLandLicenseLoading,
      findMultipleCrownLandTenureLoading,
      findMultipleHighwayWhereContainsWrappedLoading,
      findMultipleMunicipalBoundaryLoading,
      findElectoralDistrictBoundaryLoading,
      findMultipleAgriculturalReserveLoading,
      findMultipleFirstNationsBoundaryLoading,
    ],
  );
};
