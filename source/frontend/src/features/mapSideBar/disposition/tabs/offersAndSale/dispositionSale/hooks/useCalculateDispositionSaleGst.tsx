import { getIn, useFormikContext } from 'formik';
import { useEffect } from 'react';

import { DispositionSaleFormModel } from '@/features/mapSideBar/disposition/models/DispositionSaleFormModel';
import { SystemConstants, useSystemConstants } from '@/store/slices/systemConstants';

export const useCalculateDispositionSaleGst = (isGstEligible: boolean) => {
  const { values, touched, setFieldValue, isSubmitting } =
    useFormikContext<DispositionSaleFormModel>();
  const finalSaleAmountTouched = getIn(touched, 'finalSaleAmount');
  const finalSaleAmount = getIn(values, 'finalSaleAmount');
  const { getSystemConstant } = useSystemConstants();
  const gstConstant = getSystemConstant(SystemConstants.GST);
  const gstDecimal = gstConstant !== undefined ? parseFloat(gstConstant.value) / 100 : undefined;

  useEffect(() => {
    if (finalSaleAmountTouched && !isSubmitting) {
      if (gstDecimal && isGstEligible) {
        const calculatedGst = finalSaleAmount * gstDecimal;
        setFieldValue('gstCollectedAmount', calculatedGst);
      } else {
        setFieldValue('gstCollectedAmount', '');
      }
    }
  }, [
    gstDecimal,
    setFieldValue,
    isGstEligible,
    isSubmitting,
    finalSaleAmountTouched,
    finalSaleAmount,
  ]);
};
