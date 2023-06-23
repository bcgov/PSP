import { useCallback } from 'react';

import { useBcAssessmentLayer } from '@/hooks/repositories/useBcAssessmentLayer';
import { BC_ASSESSMENT_TYPES, IBcAssessmentSummary } from '@/models/layers/bcAssesment';
import { useTenant } from '@/tenants';

/**
 * hook that provides a function to retrieve the primary address for a given pid from BC Assessment.
 * @param timeout the number of milliseconds to wait before cancelling the request. Default is 3 seconds.
 * @returns the response from BC Assessment.
 */
export const useBcaAddress = () => {
  const { bcAssessment } = useTenant();
  const {
    getSummaryWrapper: { execute: getBcaSummary, loading: bcaLoading },
  } = useBcAssessmentLayer(bcAssessment.url, bcAssessment.names);
  const getPrimaryAddressByPid = useCallback(
    async (
      pid: string,
      timeout?: number,
    ): Promise<
      | {
          address?: IBcAssessmentSummary[BC_ASSESSMENT_TYPES.ADDRESSES][0];
          legalDescription?: IBcAssessmentSummary[BC_ASSESSMENT_TYPES.LEGAL_DESCRIPTION];
        }
      | undefined
    > => {
      if (!!pid) {
        timeout = timeout ?? 10000;
        const response = await getBcaSummary(pid, [BC_ASSESSMENT_TYPES.ADDRESSES], timeout);
        const addresses = response?.ADDRESSES;
        let primaryAddress = addresses?.find(address => address.PRIMARY_IND === 'true'); // get the first address with the primary indicator set.
        if (!primaryAddress && !!addresses?.length) {
          primaryAddress = addresses[0]; // if no address with primary indicator, just choose the first address.
        }
        return { address: primaryAddress, legalDescription: response?.LEGAL_DESCRIPTION };
      }
      return undefined;
    },
    [getBcaSummary],
  );
  return { getPrimaryAddressByPid, bcaLoading };
};
