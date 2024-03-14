import { getIn, useFormikContext } from 'formik';
import { useEffect } from 'react';

import { DispositionSaleFormModel } from '@/features/mapSideBar/disposition/models/DispositionSaleFormModel';
import { SystemConstants, useSystemConstants } from '@/store/slices/systemConstants';
import { exists } from '@/utils/utils';

export const useCalculateNetProceeds = (isGstEligible: boolean) => {
  const { values, touched, setFieldValue, isSubmitting } =
    useFormikContext<DispositionSaleFormModel>();
  const { getSystemConstant } = useSystemConstants();
  const gstConstant = getSystemConstant(SystemConstants.GST);
  const gstDecimal = exists(gstConstant) ? parseFloat(gstConstant.value) / 100 : 0;

  const gstTouched = getIn(touched, 'gstCollectedAmount');
  const gstAmount = getIn(values, 'gstCollectedAmount');

  const finalSaleAmount = getIn(values, 'finalSaleAmount');
  const realtorCommissionAmount = getIn(values, 'realtorCommissionAmount');
  const totalCostOfSale = getIn(values, 'totalCostAmount');
  const netBookAmount = getIn(values, 'netBookAmount');
  const sppAmount = getIn(values, 'sppAmount');

  useEffect(() => {
    if (!isSubmitting) {
      let saleCosts = 0;
      if (isGstEligible) {
        let calculatedGst;
        if (gstTouched) {
          calculatedGst = gstAmount;
        } else {
          calculatedGst = finalSaleAmount - finalSaleAmount / (1 + gstDecimal);
        }

        saleCosts = calculatedGst + realtorCommissionAmount + totalCostOfSale + netBookAmount;
      } else {
        saleCosts = realtorCommissionAmount + totalCostOfSale + netBookAmount;
      }
      const proceedsBeforeSPP = finalSaleAmount - saleCosts;
      const proceedsAfterSPP = proceedsBeforeSPP - sppAmount;

      setFieldValue('netProceedsBeforeSppAmount', proceedsBeforeSPP);
      setFieldValue('netProceedsAfterSppAmount', proceedsAfterSPP);
    }
  }, [
    setFieldValue,
    isGstEligible,
    isSubmitting,
    finalSaleAmount,
    realtorCommissionAmount,
    totalCostOfSale,
    netBookAmount,
    gstTouched,
    sppAmount,
    gstAmount,
    gstDecimal,
  ]);
};
