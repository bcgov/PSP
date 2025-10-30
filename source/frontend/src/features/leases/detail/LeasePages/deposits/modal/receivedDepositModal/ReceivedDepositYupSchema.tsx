import * as Yup from 'yup';

import { MAX_SQL_MONEY_SIZE } from '@/constants/API';
import { ApiGen_CodeTypes_LeaseSecurityDepositTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseSecurityDepositTypes';

export const ReceivedDepositYupSchema = Yup.object().shape({
  depositTypeCode: Yup.string().required('Deposit Type is required'),
  otherTypeDescription: Yup.string().when('depositTypeCode', {
    is: (depositTypeCode: string) => depositTypeCode && depositTypeCode === 'OTHER',
    then: Yup.string()
      .required('Other Description required')
      .max(200, 'Other type description must be at most 200 characters'),
    otherwise: Yup.string().nullable(),
  }),
  description: Yup.string().when('depositTypeCode', {
    is: (depositTypeCode: string) =>
      depositTypeCode && depositTypeCode === ApiGen_CodeTypes_LeaseSecurityDepositTypes.OTHER,
    then: Yup.string()
      .required('Description required when Deposit type "Other" is selected')
      .max(2000, 'Description must be at most 2000 characters'),
    otherwise: Yup.string().max(2000, 'Description must be at most 2000 characters'),
  }),
  amountPaid: Yup.number().max(
    MAX_SQL_MONEY_SIZE,
    `Amount paid must be less than ${MAX_SQL_MONEY_SIZE}`,
  ),
  contactHolder: Yup.object().required('Deposit Holder is required'),
});
