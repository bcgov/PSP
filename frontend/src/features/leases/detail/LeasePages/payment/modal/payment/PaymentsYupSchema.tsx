import * as Yup from 'yup';

export const PaymentsYupSchema = Yup.object().shape({
  receivedDate: Yup.date().required('Required'),
  amountPreTax: Yup.number().transform((value, originalValue) =>
    String(originalValue).trim() === '' ? null : value,
  ),
  amountTotal: Yup.number()
    .transform((value, originalValue) => (String(originalValue).trim() === '' ? null : value))
    .min(0, 'must be a value greater then 0'),
  amountGst: Yup.number().transform((value, originalValue) =>
    String(originalValue).trim() === '' ? null : value,
  ),
});
