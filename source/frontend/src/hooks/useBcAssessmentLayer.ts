import axios, { AxiosError, AxiosResponse } from 'axios';
import { IApiError } from 'interfaces/IApiError';
import { getMockAddresses, getMockLegalDescriptions } from 'mocks/bcAssessmentMock';
import { useCallback, useEffect } from 'react';
import { toast } from 'react-toastify';

import { getMockDescription, getMockSales, getMockValues } from './../mocks/bcAssessmentMock';
import { pidParser } from './../utils/propertyUtils';
import { useWfsLayer } from './pims-api';
import { IResponseWrapper, useApiRequestWrapper } from './pims-api/useApiRequestWrapper';
import useKeycloakWrapper from './useKeycloakWrapper';
import { useModalContext } from './useModalContext';

export enum BC_ASSESSMENT_TYPES {
  LEGAL_DESCRIPTION = 'LEGAL_DESCRIPTION',
  ADDRESSES = 'ADDRESSES',
  VALUES = 'VALUES',
  CHARGES = 'CHARGES',
  FOLIO_DESCRIPTION = 'FOLIO_DESCRIPTION',
  SALES = 'SALES',
  CHARACTERISTICS = 'CHARACTERISTICS',
}

/**
 * API wrapper to centralize all AJAX requests to WFS endpoints on the BC assessment layer.
 * @returns Object containing functions to make requests to the WFS layer.
 * Note: according to https://catalogue.data.gov.bc.ca/dataset/parcelmap-bc-parcel-fabric/resource/959af382-fb31-4f57-b8ea-e6dcb6ce2e0b
 */
export const useBcAssessmentLayer = (
  url: string,
  names: { [key: string]: string },
): {
  getSummaryWrapper: IResponseWrapper<
    (
      pid: string,
      typesToLoad?: BC_ASSESSMENT_TYPES[],
    ) => Promise<AxiosResponse<IBcAssessmentSummary>>
  >;
} => {
  const keycloak = useKeycloakWrapper();
  const logout = keycloak.obj.logout;
  const { setModalContent, setDisplayModal } = useModalContext();
  useEffect(() => {
    setModalContent({
      title: 'SiteMinder Session Expired',
      message:
        'Your SiteMinder Session has expired, you have not been authorized to access BC Assessment, or BC Assessment is offline. In order to access BC Assessment data, you may try to logout and log back in to the application. If you continue to see this error, contact an administrator',
      okButtonText: 'Log out',
      cancelButtonText: 'Continue working',
      handleOk: () => {
        logout();
      },
      handleCancel: () => {
        setDisplayModal(false);
      },
    });
  }, [setModalContent, logout, setDisplayModal]);

  const getLegalDescriptionsWrapper = useWfsLayer(
    url,
    {
      name: names[BC_ASSESSMENT_TYPES.LEGAL_DESCRIPTION],
      withCredentials: true,
    },
    { throwError: true },
  );
  const getAddressesWrapper = useWfsLayer(url, {
    name: names[BC_ASSESSMENT_TYPES.ADDRESSES],
    withCredentials: true,
  });
  const getValuesWrapper = useWfsLayer(url, {
    name: names[BC_ASSESSMENT_TYPES.VALUES],
    withCredentials: true,
  });
  const getChargesWrapper = useWfsLayer(url, {
    name: names[BC_ASSESSMENT_TYPES.CHARGES],
    withCredentials: true,
  });
  const getFolioDescriptionsWrapper = useWfsLayer(url, {
    name: names[BC_ASSESSMENT_TYPES.FOLIO_DESCRIPTION],
    withCredentials: true,
  });
  const getSalesWrapper = useWfsLayer(url, {
    name: names[BC_ASSESSMENT_TYPES.SALES],
    withCredentials: true,
  });
  const getSales = getSalesWrapper.execute;
  const getFolioDescriptions = getFolioDescriptionsWrapper.execute;
  const getCharges = getChargesWrapper.execute;
  const getValues = getValuesWrapper.execute;
  const getAddresses = getAddressesWrapper.execute;
  const getLegalDescriptions = getLegalDescriptionsWrapper.execute;

  const getSummary = useCallback(
    async (
      pid: string,
      typesToLoad?: BC_ASSESSMENT_TYPES[],
    ): Promise<AxiosResponse<IBcAssessmentSummary>> => {
      const parsedPid = pidParser(pid);
      if (parsedPid === undefined) {
        throw Error(`Unable to parse PID, invalid format: ${pid}`);
      }
      if (process.env.NODE_ENV === 'development') {
        return {
          data: mockBcAssessmentSummary,
          status: 200,
          statusText: 'Success',
          headers: {},
          config: {},
        };
      }
      let legalDescriptionResponse;
      try {
        legalDescriptionResponse = await getLegalDescriptions(
          { PID_NUMBER: parsedPid.toString() },
          { timeout: 40000, forceExactMatch: true, onLayerError: bcAssessmentError },
        );
      } catch (e: any) {
        if (axios.isAxiosError(e)) {
          const axiosError = e as AxiosError<IApiError>;
          try {
            // Test to see if the service is at all available.
            await axios.get(url, { withCredentials: true });
          } catch (err) {
            if (axiosError.response === undefined && axiosError.code === undefined) {
              setDisplayModal(true);
            }
          }
        }
      }

      if (!legalDescriptionResponse?.features?.length) {
        throw Error(
          'Invalid BC Assessment response. Unable to load BC Assessment data for property.',
        );
      }
      let folioId = legalDescriptionResponse?.features[0]?.properties?.FOLIO_ID;
      let rollNumber = legalDescriptionResponse?.features[0]?.properties?.ROLL_NUMBER;

      if (!folioId || !rollNumber) {
        throw Error(
          'Invalid BC Assessment response. Unable to load BC Assessment data for property.',
        );
      }

      const addressPromise = !!typesToLoad?.find(t => t === BC_ASSESSMENT_TYPES.ADDRESSES)
        ? getAddresses(
            { FOLIO_ID: folioId, ROLL_NUMBER: rollNumber },
            { timeout: 40000, forceExactMatch: true, onLayerError: bcAssessmentError },
          )
        : Promise.resolve();

      const valuesPromise = !!typesToLoad?.find(t => t === BC_ASSESSMENT_TYPES.VALUES)
        ? getValues(
            { FOLIO_ID: folioId, ROLL_NUMBER: rollNumber },
            { timeout: 40000, forceExactMatch: true, onLayerError: bcAssessmentError },
          )
        : Promise.resolve();

      const chargesPromise = !!typesToLoad?.find(t => t === BC_ASSESSMENT_TYPES.CHARGES)
        ? getCharges(
            { FOLIO_ID: folioId, ROLL_NUMBER: rollNumber },
            { timeout: 40000, forceExactMatch: true, onLayerError: bcAssessmentError },
          )
        : Promise.resolve();

      const folioDescriptionsPromise = !!typesToLoad?.find(
        t => t === BC_ASSESSMENT_TYPES.FOLIO_DESCRIPTION,
      )
        ? getFolioDescriptions(
            { FOLIO_ID: folioId, ROLL_NUMBER: rollNumber },
            { timeout: 40000, forceExactMatch: true, onLayerError: bcAssessmentError },
          )
        : Promise.resolve();

      const salesPromise = !!typesToLoad?.find(t => t === BC_ASSESSMENT_TYPES.SALES)
        ? getSales(
            { FOLIO_ID: folioId, ROLL_NUMBER: rollNumber },
            { timeout: 40000, forceExactMatch: true, onLayerError: bcAssessmentError },
          )
        : Promise.resolve();

      const responses = await Promise.all([
        addressPromise,
        valuesPromise,
        chargesPromise,
        folioDescriptionsPromise,
        salesPromise,
      ]);

      if (
        responses.length !== 5 ||
        legalDescriptionResponse?.features.length < 1 ||
        responses[3]?.features === undefined ||
        responses[3]?.features.length < 1
      ) {
        throw Error(
          'Invalid BC Assessment response. Unable to load BC Assessment data for property.',
        );
      }
      const summary: IBcAssessmentSummary = {
        LEGAL_DESCRIPTION: legalDescriptionResponse?.features[0]?.properties ?? {},
        ADDRESSES:
          responses[0]?.features?.map((f: { properties: any }) => f.properties ?? {}) ?? [],
        VALUES: responses[1]?.features?.map((f: { properties: any }) => f.properties ?? {}) ?? [],
        CHARGES: responses[2]?.features?.map((f: { properties: any }) => f.properties ?? {}) ?? [],
        FOLIO_DESCRIPTION: responses[3]?.features[0]?.properties ?? {},
        SALES: responses[4]?.features?.map((f: { properties: any }) => f.properties ?? {}) ?? [],
      };
      return { data: summary, status: 200, statusText: 'Success', headers: {}, config: {} };
    },
    [
      getAddresses,
      getValues,
      getCharges,
      getFolioDescriptions,
      getSales,
      getLegalDescriptions,
      setDisplayModal,
      url,
    ],
  );

  const getSummaryWrapper = useApiRequestWrapper<
    (
      pid: string,
      typesToLoad?: BC_ASSESSMENT_TYPES[],
    ) => Promise<AxiosResponse<IBcAssessmentSummary>>
  >({ requestFunction: getSummary, requestName: 'BC_ASSESSMENT_SUMMARY' });

  return {
    getSummaryWrapper,
  };
};

export const LAYER_UNAVAILABLE = [
  'Error returned from BC Assessment.',
  'Please notify ',
  'pims@gov.bc.ca',
  ' if this problem persists.',
];

const bcAssessmentError = () =>
  toast.error(LAYER_UNAVAILABLE.join('\n'), { toastId: 'LAYER_DATA_ERROR_ID' });

export const mockBcAssessmentSummary: IBcAssessmentSummary = {
  LEGAL_DESCRIPTION: getMockLegalDescriptions()?.features[0].properties ?? {},
  ADDRESSES: getMockAddresses()?.features.map(f => f.properties ?? {}) ?? [],
  VALUES: getMockValues()?.features.map(f => f.properties ?? {}) ?? [],
  CHARGES: [],
  FOLIO_DESCRIPTION: (getMockDescription()?.features[0].properties as any) ?? {},
  SALES: getMockSales()?.features.map(f => f.properties ?? {}) ?? [],
};
export interface IBcAssessmentSummary {
  FOLIO_DESCRIPTION: Partial<{
    BCA_FD_SYSID: number;
    ROLL_NUMBER: string;
    FOLIO_ID: string;
    FOLIO_STATUS: string;
    FOLIO_STATUS_DESCRIPTION: string;
    ACTUAL_USE_CODE: string;
    ACTUAL_USE_DESCRIPTION: string;
    ALR_CODE: string;
    ALR_DESCRIPTION: string;
    BC_TRANSIT_IND: string;
    LAND_DEPTH: number;
    LAND_SIZE: number;
    LAND_DIMENSION_TYPE: string;
    LAND_UNITS: string;
    LAND_WIDTH: number;
    NEIGHBOURHOOD_CODE: string;
    NEIGHBOURHOOD: string;
    MANUAL_CLASS_CODE: string;
    MANUAL_CLASS_DESCRIPTION: string;
    REGIONAL_DISTRICT_CODE: string;
    REGIONAL_DISTRICT: string;
    HOSPITAL_DISTRICT_CODE: string;
    HOSPITAL_DISTRICT: string;
    SCHOOL_DISTRICT_CODE: string;
    SCHOOL_DISTRICT: string;
    TENURE_CODE: string;
    TENURE_DESCRIPTION: string;
    JURISDICTION_CODE: string;
    JURISDICTION: string;
    WHEN_CREATED: string;
    WHEN_UPDATED: string;
  }>;
  LEGAL_DESCRIPTION: Partial<{
    BCA_FLD_SYSID: number;
    ROLL_NUMBER: string;
    FOLIO_ID: string;
    FOLIO_STATUS: string;
    FOLIO_STATUS_DESCRIPTION: string;
    LEGAL_DESCRIPTIONS_COUNT: number;
    LEGAL_DESCRIPTION_ID: string;
    BLOCK: string;
    SUB_BLOCK: string;
    DISTRICT_LOT: string;
    EXCEPT_PLAN: string;
    FORMATTED_LEGAL_DESCRIPTION: string;
    LAND_BRANCH_FILE_NUMBER: string;
    LAND_DISTRICT: string;
    LAND_DISTRICT_DESCRIPTION: string;
    LEGAL_TEXT: string;
    LOT: string;
    PID_NUMBER: number;
    PID: string;
    PART_1: string;
    PART_2: string;
    PART_3: string;
    PART_4: string;
    PORTION: string;
    SUB_LOT: string;
    TOWNSHIP: string;
    PLAN: string;
    RANGE: string;
    SECTION: string;
    STRATA_LOT: string;
    LEGAL_SUBDIVISION: string;
    PARCEL: string;
    LEASE_LICENCE_NUMBER: string;
    MERIDIAN: string;
    MERIDIAN_SHORT: string;
    BCA_GROUP: string;
    FIRST_NATION_RESERVE_NUMBER: string;
    FIRST_NATION_RESERVE_DESC: string;
    AIR_SPACE_PARCEL_NUMBER: string;
    JURISDICTION_CODE: string;
    JURISDICTION: string;
    WHEN_CREATED: string;
    WHEN_UPDATE: string;
  }>;
  ADDRESSES: Partial<{
    SE_ANNO_CAD_DATA: string;
    BCA_FA_SYSID: number;
    ROLL_NUMBER: string;
    FOLIO_ID: string;
    FOLIO_STATUS: string;
    FOLIO_STATUS_DESCRIPTION: string;
    ADDRESS_COUNT: number;
    ADDRESS_ID: string;
    UNIT_NUMBER: string;
    STREET_NUMBER: string;
    STREET_DIRECTION_PREFIX: string;
    STREET_NAME: string;
    STREET_TYPE: string;
    STREET_DIRECTION_SUFFIX: string;
    CITY: string;
    POSTAL_CODE: string;
    PROVINCE: string;
    PRIMARY_IND: string;
    MAP_REFERENCE_NUMBER: string;
    JURISDICTION_CODE: string;
    JURISDICTION: string;
    WHEN_CREATED: string;
    WHEN_UPDATED: string;
    EXPIRY_DATE: string;
    FEATURE_AREA_SQM: number;
    FEATURE_LENGTH_M: number;
    SHAPE: any;
    OBJECTID: number;
  }>[];
  SALES: Partial<{
    BCA_FS_SYSID: number;
    ROLL_NUMBER: string;
    FOLIO_ID: string;
    FOLIO_STATUS: string;
    FOLIO_STATUS_DESCRIPTION: string;
    SALES_COUNT: number;
    SALES_ID: string;
    DOCUMENT_NUMBER: string;
    CONVEYANCE_DATE: string;
    CONVEYANCE_PRICE: number;
    CONVEYANCE_TYPE: string;
    CONVEYANCE_TYPE_DESCRIPTION: string;
    JURISDICTION_CODE: string;
    JURISDICTION: string;
    WHEN_CREATED: string;
    WHEN_UPDATED: string;
    EXPIRY_DATE: string;
    FEATURE_AREA_SQM: number;
    FEATURE_LENGTH_M: number;
    SHAPE: any;
    OBJECTID: number;
    SE_ANNO_CAD_DATA: string;
  }>[];
  VALUES: Partial<{
    BCA_FGPV_SYSID: number;
    ROLL_NUMBER: string;
    FOLIO_ID: string;
    FOLIO_STATUS: string;
    FOLIO_STATUS_DESCRIPTION: string;
    GEN_VALUES_COUNT: number;
    GEN_GROSS_IMPROVEMENT_VALUE: number;
    GEN_GROSS_LAND_VALUE: number;
    GEN_NET_IMPROVEMENT_VALUE: number;
    GEN_NET_LAND_VALUE: number;
    GEN_TXXMT_IMPROVEMENT_VALUE: number;
    GEN_TXXMT_LAND_VALUE: number;
    GEN_PROPERTY_CLASS_CODE: string;
    GEN_PROPERTY_CLASS_DESC: string;
    GEN_PROPERTY_SUBCLASS_CODE: string;
    GEN_PROPERTY_SUBCLASS_DESC: string;
    JURISDICTION_CODE: string;
    JURISDICTION: string;
    WHEN_CREATED: string;
    WHEN_UPDATE: string;
  }>[];
  CHARGES: Partial<{
    SE_ANNO_CAD_DATA: string;
    BCA_FLC_SYSID: number;
    ROLL_NUMBER: string;
    FOLIO_ID: string;
    FOLIO_STATUS: string;
    FOLIO_STATUS_DESCRIPTION: string;
    LAND_CHARACTERISTICS_COUNT: number;
    LAND_CHARACTERISTIC_CODE: string;
    LAND_CHARACTERISTIC_DESC: string;
    JURISDICTION_CODE: string;
    JURISDICTION: string;
    WHEN_CREATED: string;
    WHEN_UPDATED: string;
    EXPIRY_DATE: string;
    FEATURE_AREA_SQM: number;
    FEATURE_LENGTH_M: number;
    SHAPE: any;
    OBJECTID: number;
  }>[];
}
