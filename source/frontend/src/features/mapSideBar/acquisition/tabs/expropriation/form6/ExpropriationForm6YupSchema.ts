import * as yup from 'yup';

export const ExpropriationForm6YupSchema = yup.object().shape({
  impactedProperties: yup.array().min(1, 'At least one impacted property is required'),
});
