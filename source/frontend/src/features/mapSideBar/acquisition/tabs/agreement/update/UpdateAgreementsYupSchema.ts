/* eslint-disable no-template-curly-in-string */
import * as Yup from 'yup';

import { MAX_SQL_MONEY_SIZE } from '@/constants/API';

export const UpdateAgreementsYupSchema = Yup.object().shape({
  acquisitionFileId: Yup.string().required('Acquisition file id is required'),
  agreements: Yup.array().of(
    Yup.object().shape({
      legalSurveyPlanNum: Yup.string().max(
        250,
        'Legal survey plan must be less than 250 characters',
      ),
      agreementTypeCode: Yup.string().required('Agreement type is required'),
      purchasePrice: Yup.lazy(value =>
        value === ''
          ? Yup.string()
          : Yup.number().max(
              MAX_SQL_MONEY_SIZE,
              `Purchace price must be less than ${MAX_SQL_MONEY_SIZE}`,
            ),
      ),
      noLaterThanDays: Yup.lazy(value =>
        value === ''
          ? Yup.string()
          : Yup.number()
              .typeError('Invalid number')
              .integer('Invalid number')
              .min(0, 'Number of days must be greater than 0'),
      ),
      depositAmount: Yup.lazy(value =>
        value === ''
          ? Yup.string()
          : Yup.number().max(
              MAX_SQL_MONEY_SIZE,
              `Deposit ammount must be less than ${MAX_SQL_MONEY_SIZE}`,
            ),
      ),
    }),
  ),
});
