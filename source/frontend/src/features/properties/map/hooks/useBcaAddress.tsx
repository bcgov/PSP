import {
  BC_ASSESSMENT_TYPES,
  IBcAssessmentSummary,
  useBcAssessmentLayer,
} from 'hooks/useBcAssessmentLayer';
import { useTenant } from 'tenants';

export const useBcaAddress = () => {
  const { bcAssessment } = useTenant();
  const { getSummaryWrapper } = useBcAssessmentLayer(bcAssessment.url, bcAssessment.names);
  const getPrimaryAddressByPid = async (
    pid: string,
  ): Promise<
    | {
        address?: IBcAssessmentSummary[BC_ASSESSMENT_TYPES.ADDRESSES][0];
        legalDescription?: IBcAssessmentSummary[BC_ASSESSMENT_TYPES.LEGAL_DESCRIPTION];
      }
    | undefined
  > => {
    if (!!pid) {
      const response = await getSummaryWrapper.execute(pid, [BC_ASSESSMENT_TYPES.ADDRESSES]);
      const addresses = response?.ADDRESSES;
      let primaryAddress = addresses?.find(address => address.PRIMARY_IND === 'true'); // get the first address with the primary indicator set.
      if (!primaryAddress && !!addresses?.length) {
        primaryAddress = addresses[0]; // if no address with primary indicator, just choose the first address.
      }
      return { address: primaryAddress, legalDescription: response?.LEGAL_DESCRIPTION };
    }
    return undefined;
  };
  return { getPrimaryAddressByPid };
};
