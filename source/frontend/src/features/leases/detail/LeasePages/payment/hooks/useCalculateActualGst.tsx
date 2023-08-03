import { getIn, useFormikContext } from 'formik';
import { useEffect } from 'react';

import { SystemConstants, useSystemConstants } from '@/store/slices/systemConstants';

import { FormLeasePayment } from '../models';

/**
 * hook that auto fills formik payment fields based on gst calculation of total amount paid.
 * @param isGstEligible do not perform calculation if payment term is not gst eligible.
 */
export const useCalculateActualGst = (isGstEligible: boolean) => {
  const { values, touched, setFieldValue, isSubmitting } = useFormikContext<FormLeasePayment>();
  const amountTotalTouched = getIn(touched, 'amountTotal');
  const amountTotal = getIn(values, 'amountTotal');
  const { getSystemConstant } = useSystemConstants();
  const gstConstant = getSystemConstant(SystemConstants.GST);
  const gstDecimal = gstConstant !== undefined ? parseFloat(gstConstant.value) : undefined;

  //auto fill dependent fields if amount total is edited.
  useEffect(() => {
    if (amountTotalTouched && !isSubmitting) {
      if (gstDecimal && isGstEligible) {
        const calculatedPreTax = amountTotal / (gstDecimal / 100 + 1);
        const calculatedGst = amountTotal - calculatedPreTax;
        setFieldValue('amountGst', calculatedGst);
        setFieldValue('amountPreTax', calculatedPreTax);
      } else {
        setFieldValue('amountPreTax', amountTotal);
        setFieldValue('amountGst', '');
      }
    }
  }, [gstDecimal, setFieldValue, amountTotalTouched, isGstEligible, amountTotal, isSubmitting]);
};
