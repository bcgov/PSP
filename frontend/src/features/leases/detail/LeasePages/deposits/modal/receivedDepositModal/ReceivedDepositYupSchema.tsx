import * as Yup from 'yup';

export const ReceivedDepositYupSchema = Yup.object().shape({
  depositTypeCode: Yup.string().required('Deposit Type is required'),
  otherTypeDescription: Yup.string().when('depositTypeCode', {
    is: (depositTypeCode: string) => depositTypeCode && depositTypeCode === 'OTHER',
    then: Yup.string().required('Other Description required'),
    otherwise: Yup.string().nullable(),
  }),
  amountPaid: Yup.string().required('Deposit amount is required'),
  depositDate: Yup.string().required('Deposit Date is required'),
});
