import { getIn, useFormikContext } from 'formik';
import { useEffect } from 'react';

import { DispositionSaleFormModel } from '@/features/mapSideBar/disposition/models/DispositionSaleFormModel';
import { SystemConstants, useSystemConstants } from '@/store/slices/systemConstants';

export const useCalculateNetProceeds = (isGstEligible: boolean) => {
  const { values, touched, setFieldValue, isSubmitting } =
    useFormikContext<DispositionSaleFormModel>();
  const { getSystemConstant } = useSystemConstants();
  const gstConstant = getSystemConstant(SystemConstants.GST);
  const gstDecimal = gstConstant !== undefined ? parseFloat(gstConstant.value) / 100 : undefined;

  const gstTouched = getIn(touched, 'gstCollectedAmount');

  const finalSaleAmount = getIn(values, 'finalSaleAmount');
  const gstAmount = getIn(values, 'gstCollectedAmount');
  const realtorCommissionAmount = getIn(values, 'realtorCommissionAmount');
  const totalCostOfSale = getIn(values, 'totalCostAmount');
  const netBookAmount = getIn(values, 'netBookAmount');
  const sppAmount = getIn(values, 'sppAmount');

  useEffect(() => {
    if (!isSubmitting) {
      if (isGstEligible) {
        let calculatedGst;
        if (gstTouched) {
          calculatedGst = gstAmount;
        } else {
          calculatedGst = gstDecimal ? finalSaleAmount * gstDecimal : 0;
        }

        const saleCosts = calculatedGst + realtorCommissionAmount + totalCostOfSale + netBookAmount;
        const proceedsBeforeSPP = finalSaleAmount - saleCosts;
        const proceedsAfterSPP = proceedsBeforeSPP - sppAmount;

        setFieldValue('netProceedsBeforeSppAmount', proceedsBeforeSPP);
        setFieldValue('netProceedsAfterSppAmount', proceedsAfterSPP);
      } else {
        const saleCosts = realtorCommissionAmount + totalCostOfSale + netBookAmount;
        const proceedsBeforeSPP = finalSaleAmount - saleCosts;
        const proceedsAfterSPP = proceedsBeforeSPP - sppAmount;

        setFieldValue('netProceedsBeforeSppAmount', proceedsBeforeSPP);
        setFieldValue('netProceedsAfterSppAmount', proceedsAfterSPP);
      }
    }
  }, [
    gstDecimal,
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
  ]);
};
