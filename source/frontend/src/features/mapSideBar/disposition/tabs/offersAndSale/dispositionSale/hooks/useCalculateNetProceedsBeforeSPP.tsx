import { getIn, useFormikContext } from 'formik';
import { useEffect } from 'react';

import { DispositionSaleFormModel } from '@/features/mapSideBar/disposition/models/DispositionSaleFormModel';
import { SystemConstants, useSystemConstants } from '@/store/slices/systemConstants';

export const useCalculateNetProceedsBeforeSPP = (isGstEligible: boolean) => {
  const { values, touched, setFieldValue, isSubmitting } =
    useFormikContext<DispositionSaleFormModel>();
  const { getSystemConstant } = useSystemConstants();
  const gstConstant = getSystemConstant(SystemConstants.GST);
  const gstDecimal = gstConstant !== undefined ? parseFloat(gstConstant.value) / 100 : undefined;

  const finalSaleAmountTouched = getIn(touched, 'finalSaleAmount');
  const realtorCommissionTouched = getIn(touched, 'realtorCommissionAmount');
  const totalCostOfSaleTouched = getIn(touched, 'totalCostOfSale');
  const netBookAmountTouched = getIn(touched, 'netBookAmount');
  const gstTouched = getIn(touched, 'gstCollectedAmount');
  const sppTouched = getIn(touched, 'sppAmount');

  const finalSaleAmount = getIn(values, 'finalSaleAmount');
  const gstAmount = getIn(values, 'gstCollectedAmount');
  const realtorCommissionAmount = getIn(values, 'realtorCommissionAmount');
  const totalCostOfSale = getIn(values, 'totalCostAmount');
  const netBookAmount = getIn(values, 'netBookAmount');
  const sppAmount = getIn(values, 'sppAmount');

  useEffect(() => {
    if (
      (finalSaleAmountTouched ||
        realtorCommissionTouched ||
        totalCostOfSaleTouched ||
        netBookAmountTouched ||
        gstTouched ||
        sppTouched) &&
      !isSubmitting
    ) {
      if (gstDecimal && isGstEligible) {
        let calculatedGst;
        if (gstTouched) {
          calculatedGst = gstAmount;
        } else {
          calculatedGst = finalSaleAmount * gstDecimal;
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
    finalSaleAmountTouched,
    finalSaleAmount,
    realtorCommissionTouched,
    realtorCommissionAmount,
    totalCostOfSale,
    totalCostOfSaleTouched,
    netBookAmount,
    netBookAmountTouched,
    gstTouched,
    sppTouched,
    sppAmount,
    gstAmount,
  ]);
};
