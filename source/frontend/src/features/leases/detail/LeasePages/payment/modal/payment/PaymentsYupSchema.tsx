import * as Yup from 'yup';

import { MAX_SQL_MONEY_SIZE } from '@/constants/API';

export const PaymentsYupSchema = Yup.object().shape({
  receivedDate: Yup.date().required('Required'),
  amountPreTax: Yup.number()
    .transform((value, originalValue) => (String(originalValue).trim() === '' ? null : value))
    .max(MAX_SQL_MONEY_SIZE),
  amountTotal: Yup.number()
    .transform((value, originalValue) => (String(originalValue).trim() === '' ? null : value))
    .min(0, 'must be a value greater than 0')
    .max(MAX_SQL_MONEY_SIZE),
  amountGst: Yup.number()
    .transform((value, originalValue) => (String(originalValue).trim() === '' ? null : value))
    .max(MAX_SQL_MONEY_SIZE),
});
