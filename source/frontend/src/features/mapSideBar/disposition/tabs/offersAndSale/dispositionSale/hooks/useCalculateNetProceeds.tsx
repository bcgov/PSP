import { getIn, useFormikContext } from 'formik';
import { useEffect } from 'react';

import { DispositionSaleFormModel } from '@/features/mapSideBar/disposition/models/DispositionSaleFormModel';

export const useCalculateNetProceeds = (isGstEligible: boolean) => {
  const { values, touched, setFieldValue, isSubmitting } =
    useFormikContext<DispositionSaleFormModel>();

  const gstTouched = getIn(touched, 'gstCollectedAmount');

  const finalSaleAmount = getIn(values, 'finalSaleAmount');
  const realtorCommissionAmount = getIn(values, 'realtorCommissionAmount');
  const totalCostOfSale = getIn(values, 'totalCostAmount');
  const netBookAmount = getIn(values, 'netBookAmount');
  const sppAmount = getIn(values, 'sppAmount');

  useEffect(() => {
    if (!isSubmitting) {
      const saleCosts = realtorCommissionAmount + totalCostOfSale + netBookAmount;
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
  ]);
};
