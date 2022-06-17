import { MAX_SQL_MONEY_SIZE } from 'constants/API';
import * as Yup from 'yup';

export const ReceivedDepositYupSchema = Yup.object().shape({
  depositTypeCode: Yup.string().required('Deposit Type is required'),
  otherTypeDescription: Yup.string().when('depositTypeCode', {
    is: (depositTypeCode: string) => depositTypeCode && depositTypeCode === 'OTHER',
    then: Yup.string()
      .required('Other Description required')
      .max(200),
    otherwise: Yup.string().nullable(),
  }),
  description: Yup.string().max(2000),
  amountPaid: Yup.string()
    .required('Deposit amount is required')
    .max(MAX_SQL_MONEY_SIZE),
  depositDate: Yup.string().required('Deposit Date is required'),
  contactHolder: Yup.object().required('Deposit Holder is required'),
});
