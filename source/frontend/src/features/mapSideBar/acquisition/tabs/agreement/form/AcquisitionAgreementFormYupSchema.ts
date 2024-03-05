/* eslint-disable no-template-curly-in-string */
import * as yup from 'yup';

import { MAX_SQL_MONEY_SIZE } from '@/constants/API';

export const AcquisitionAgreementFormYupSchema = yup.object().shape({
  legalSurveyPlanNum: yup
    .string()
    .nullable()
    .max(250, 'Legal survey plan must be at most ${max} characters'),
  agreementTypeCode: yup.string().required('Agreement type is required'),
  purchasePrice: yup.lazy(value =>
    value === ''
      ? yup.string().nullable()
      : yup
          .number()
          .nullable()
          .max(MAX_SQL_MONEY_SIZE, `Purchace price must be less than ${MAX_SQL_MONEY_SIZE}`),
  ),
  noLaterThanDays: yup.lazy(value =>
    value === ''
      ? yup.string().nullable()
      : yup
          .number()
          .nullable()
          .typeError('Invalid number')
          .integer('Invalid number')
          .min(0, 'Number of days must be greater than 0'),
  ),
  depositAmount: yup.lazy(value =>
    value === ''
      ? yup.string().nullable()
      : yup
          .number()
          .nullable()
          .max(MAX_SQL_MONEY_SIZE, `Deposit ammount must be less than ${MAX_SQL_MONEY_SIZE}`),
  ),
});
