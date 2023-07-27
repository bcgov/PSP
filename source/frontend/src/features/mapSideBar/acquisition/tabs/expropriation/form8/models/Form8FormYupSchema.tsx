import * as yup from 'yup';
/* eslint-disable no-template-curly-in-string */

export const Form8FormModelYupSchema = yup.object().shape({
  description: yup.string().max(2000, 'Description must be at most ${max} characters'),
  paymentItems: yup.array().of(
    yup.object().shape({
      paymentItemTypeCode: yup.string().required('Type is required'),
      pretaxAmount: yup
        .number()
        .transform(value => (isNaN(value) || value === null || value === undefined ? 0 : value)),
    }),
  ),
});
