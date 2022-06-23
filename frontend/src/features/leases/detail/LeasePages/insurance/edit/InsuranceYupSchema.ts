import { MAX_SQL_MONEY_SIZE } from 'constants/API';
import * as Yup from 'yup';

export const InsuranceYupSchema = Yup.object().shape({
  insurances: Yup.array().of(
    Yup.object().shape({
      coverageLimit: Yup.number().max(MAX_SQL_MONEY_SIZE),
      coverageDescription: Yup.string().max(2000),
      expiryDate: Yup.date(),
      otherInsuranceType: Yup.string().max(200),
    }),
  ),
});
