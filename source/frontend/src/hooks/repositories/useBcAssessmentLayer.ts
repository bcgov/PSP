import { AxiosResponse } from 'axios';
import { useCallback } from 'react';

import { BC_ASSESSMENT_TYPES, IBcAssessmentSummary } from '@/models/layers/bcAssesment';
import { pidParser } from '@/utils';

import { useWfsLayer } from '../layer-api/useWfsLayer';
import { IResponseWrapper, useApiRequestWrapper } from '../util/useApiRequestWrapper';

/**
 * API wrapper to centralize all AJAX requests to WFS endpoints on the BC assessment layer.
 * @returns Object containing functions to make requests to the WFS layer.
 */
export const useBcAssessmentLayer = (
  url: string,
  names: { [key: string]: string },
): {
  getSummaryWrapper: IResponseWrapper<
    (
      pid: string,
      typesToLoad?: BC_ASSESSMENT_TYPES[],
      timeout?: number,
    ) => Promise<AxiosResponse<IBcAssessmentSummary | undefined>>
  >;
} => {
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
      timeout?: number,
    ): Promise<AxiosResponse<IBcAssessmentSummary | undefined>> => {
      const parsedPid = pidParser(pid);
      if (parsedPid === undefined) {
        throw Error(`Unable to parse PID, invalid format: ${pid}`);
      }
      let legalDescriptionResponse;
      try {
        legalDescriptionResponse = await getLegalDescriptions(
          { PID_NUMBER: parsedPid.toString() },
          { timeout: timeout ?? 10000, forceExactMatch: true },
        );
      } catch (e: any) {
        return {
          data: undefined,
          status: 500,
          statusText: 'Failed to load BC Assessment data',
          headers: {},
          config: {},
        };
      }

      if (!legalDescriptionResponse?.features?.length) {
        throw Error(
          'Invalid BC Assessment response. Unable to load BC Assessment data for property.',
        );
      }
      const folioId = legalDescriptionResponse?.features[0]?.properties?.FOLIO_ID;
      const rollNumber = legalDescriptionResponse?.features[0]?.properties?.ROLL_NUMBER;
      const jurisdictionCode = legalDescriptionResponse?.features[0]?.properties?.JURISDICTION_CODE;

      if (!folioId || !rollNumber) {
        throw Error(
          'Invalid BC Assessment response. Unable to load BC Assessment data for property.',
        );
      }

      const addressPromise =
        typesToLoad === undefined || !!typesToLoad?.find(t => t === BC_ASSESSMENT_TYPES.ADDRESSES)
          ? getAddresses(
              { FOLIO_ID: folioId, ROLL_NUMBER: rollNumber, JURISDICTION_CODE: jurisdictionCode },
              { timeout: timeout ?? 10000, useCqlOr: false, forceExactMatch: true },
            )
          : Promise.resolve();

      const valuesPromise =
        typesToLoad === undefined || !!typesToLoad?.find(t => t === BC_ASSESSMENT_TYPES.VALUES)
          ? getValues(
              { FOLIO_ID: folioId, ROLL_NUMBER: rollNumber, JURISDICTION_CODE: jurisdictionCode },
              { timeout: timeout ?? 10000, useCqlOr: false, forceExactMatch: true },
            )
          : Promise.resolve();

      const chargesPromise =
        typesToLoad === undefined || !!typesToLoad?.find(t => t === BC_ASSESSMENT_TYPES.CHARGES)
          ? getCharges(
              { FOLIO_ID: folioId, ROLL_NUMBER: rollNumber, JURISDICTION_CODE: jurisdictionCode },
              { timeout: timeout ?? 10000, useCqlOr: false, forceExactMatch: true },
            )
          : Promise.resolve();

      const folioDescriptionsPromise =
        typesToLoad === undefined ||
        !!typesToLoad?.find(t => t === BC_ASSESSMENT_TYPES.FOLIO_DESCRIPTION)
          ? getFolioDescriptions(
              { FOLIO_ID: folioId, ROLL_NUMBER: rollNumber, JURISDICTION_CODE: jurisdictionCode },
              { timeout: timeout ?? 10000, useCqlOr: false, forceExactMatch: true },
            )
          : Promise.resolve();

      const salesPromise =
        typesToLoad === undefined || !!typesToLoad?.find(t => t === BC_ASSESSMENT_TYPES.SALES)
          ? getSales(
              { FOLIO_ID: folioId, ROLL_NUMBER: rollNumber, JURISDICTION_CODE: jurisdictionCode },
              { timeout: timeout ?? 10000, useCqlOr: false, forceExactMatch: true },
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
        legalDescriptionResponse?.features.length < 1 ||
        (typesToLoad?.find(t => t === BC_ASSESSMENT_TYPES.FOLIO_DESCRIPTION) &&
          (responses[3]?.features === undefined || responses[3]?.features.length < 1))
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
    [getAddresses, getValues, getCharges, getFolioDescriptions, getSales, getLegalDescriptions],
  );

  const getSummaryWrapper = useApiRequestWrapper<
    (
      pid: string,
      typesToLoad?: BC_ASSESSMENT_TYPES[],
    ) => Promise<AxiosResponse<IBcAssessmentSummary | undefined>>
  >({ requestFunction: getSummary, requestName: 'BC_ASSESSMENT_SUMMARY' });

  return {
    getSummaryWrapper,
  };
};
