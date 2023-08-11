import * as yup from 'yup';
/* eslint-disable no-template-curly-in-string */

export const Form8FormModelYupSchema = yup.object().shape({
  payeeKey: yup.string().required('Payee is required'),
  description: yup.string().max(2000, 'Description must be at most ${max} characters'),
  expropriationAuthority: yup.object().shape({
    contact: yup.object().required('Expropriation authority is required').nullable(),
  }),
  paymentItems: yup.array().of(
    yup.object().shape({
      paymentItemTypeCode: yup.string().required('Type is required'),
      pretaxAmount: yup
        .number()
        .transform(value => (isNaN(value) || value === null || value === undefined ? 0 : value)),
    }),
  ),
});
