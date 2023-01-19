/* eslint-disable no-template-curly-in-string */
import moment from 'moment';
import * as Yup from 'yup';

export const AddFinancialCodeYupSchema = Yup.object().shape({
  type: Yup.string().required('Code type is required'),
  code: Yup.string()
    .required('Code value is required')
    .max(20, 'Code value must be at most ${max} characters'),
  description: Yup.string()
    .required('Code description is required')
    .max(200, 'Code description must be at most ${max} characters'),
  effectiveDate: Yup.date().required('Effective date is required'),
  expiryDate: Yup.date().when('effectiveDate', (effectiveDate, schema) =>
    effectiveDate
      ? schema.min(
          moment(effectiveDate).add(1, 'day'),
          'Expiry date must be later than effective date',
        )
      : schema,
  ),
  displayOrder: Yup.number(),
});
