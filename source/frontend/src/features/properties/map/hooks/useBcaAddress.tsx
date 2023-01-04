import { useBcAssessmentLayer } from 'hooks/useBcAssessmentLayer';
import { useTenant } from 'tenants';

export const useBcaAddress = () => {
  const { bcAssessment } = useTenant();
  const { getBcaAddressesWrapper } = useBcAssessmentLayer(bcAssessment.url, bcAssessment.names);
  const getPrimaryAddressByPid = async (pid: string) => {
    if (!!pid) {
      const addresses = await getBcaAddressesWrapper.execute(pid);
      let primaryAddress = addresses?.find(address => address.PRIMARY_IND === 'true'); // get the first address with the primary indicator set.
      if (!primaryAddress && !!addresses?.length) {
        primaryAddress = addresses[0]; // if no address with primary indicator, just choose the first address.
      }
      return primaryAddress;
    }
  };
  return { getPrimaryAddressByPid };
};
