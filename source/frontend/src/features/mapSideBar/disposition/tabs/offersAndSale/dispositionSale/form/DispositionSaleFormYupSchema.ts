/* eslint-disable no-template-curly-in-string */
import * as yup from 'yup';

export const DispositionSaleFormYupSchema = yup.object().shape({
  finalConditionRemovalDate: yup.string().nullable(),
  saleCompletionDate: yup.string().nullable(),
  saleFiscalYear: yup.string().nullable(),
  finalSaleAmount: yup.string().nullable(),
  realtorCommissionAmount: yup.string().nullable(),
  isGstRequired: yup.string(),
  gstCollectedAmount: yup.string().nullable(),
  netBookAmount: yup.string().nullable(),
  totalCostAmount: yup.string().nullable(),
  netProceedsBeforeSppAmount: yup.string().nullable(),
  sppAmount: yup.string().nullable(),
  netProceedsAfterSppAmount: yup.string().nullable(),
  remediationAmount: yup.string().nullable(),
});
