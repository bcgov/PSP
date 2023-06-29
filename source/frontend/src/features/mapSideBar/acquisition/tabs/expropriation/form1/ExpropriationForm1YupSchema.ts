/* eslint-disable no-template-curly-in-string */
import * as Yup from 'yup';

export const ExpropriationForm1YupSchema = Yup.object().shape({
  impactedProperties: Yup.array().min(1, 'At lease one impacted property is required'),
  expropriationAuthority: Yup.object().shape({
    contact: Yup.object().required('Expropriation authority is required'),
  }),
});
