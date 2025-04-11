/* eslint-disable no-template-curly-in-string */
import * as yup from 'yup';

export const ExpropriationForm9YupSchema = yup.object().shape({
  impactedProperties: yup.array().min(1, 'At lease one impacted property is required'),
  expropriationAuthority: yup.object().shape({
    contact: yup.object().required('Expropriation authority is required').nullable(),
  }),
});
