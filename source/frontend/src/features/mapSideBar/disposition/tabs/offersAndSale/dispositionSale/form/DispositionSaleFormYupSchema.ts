/* eslint-disable no-template-curly-in-string */
import * as yup from 'yup';

export const DispositionSaleFormYupSchema = yup.object().shape({
  finalConditionRemovalDate: yup.string().nullable(),
  saleCompletionDate: yup.string().nullable(),
  saleFiscalYear: yup.string().max(4, 'Fiscal year must be at most ${max} characters').nullable(),
  finalSaleAmount: yup.number().nullable(),
  realtorCommissionAmount: yup.number().nullable(),
  isGstRequired: yup.string(),
  gstCollectedAmount: yup.number().nullable(),
  netBookAmount: yup.number().nullable(),
  totalCostAmount: yup.number().nullable(),
  netProceedsBeforeSppAmount: yup.number().nullable(),
  sppAmount: yup.number().nullable(),
  netProceedsAfterSppAmount: yup.number().nullable(),
  remediationAmount: yup.number().nullable(),
});
