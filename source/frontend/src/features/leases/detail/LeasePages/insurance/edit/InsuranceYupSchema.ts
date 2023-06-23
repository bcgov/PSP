import * as Yup from 'yup';

import { MAX_SQL_MONEY_SIZE } from '@/constants/API';

export const InsuranceYupSchema = Yup.object().shape({
  insurances: Yup.array().of(
    Yup.object().shape({
      coverageLimit: Yup.lazy(value =>
        value === ''
          ? Yup.string()
          : Yup.number().max(
              MAX_SQL_MONEY_SIZE,
              `Coverage Limit must be less than ${MAX_SQL_MONEY_SIZE}`,
            ),
      ),
      coverageDescription: Yup.string().max(
        2000,
        'Description of Coverage must be at most 2000 characters',
      ),
      expiryDate: Yup.lazy(value => (value === '' ? Yup.string() : Yup.date())),
      otherInsuranceType: Yup.string().max(
        200,
        'Other Insurance type must be at most 200 characters',
      ),
    }),
  ),
});
