/* eslint-disable no-template-curly-in-string */
import * as yup from 'yup';

export const DispositionAppraisalFormYupSchema = yup.object().shape({
  appraisedValueAmount: yup.string().nullable(),
  appraisalDate: yup.string().nullable(),
  bcaValueAmount: yup.string().nullable(),
  bcaRollYear: yup.string().nullable(),
  listPriceAmount: yup.string().nullable(),
});
