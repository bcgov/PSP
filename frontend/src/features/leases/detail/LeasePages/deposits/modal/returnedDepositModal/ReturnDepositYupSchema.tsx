import * as Yup from 'yup';

export const ReturnDepositYupSchema = Yup.object().shape({
  terminationDate: Yup.string().required('Termination Date is required'),
  returnAmount: Yup.string().required('Return amount is required'),
  returnDate: Yup.string().required('Return Date is required'),
});
