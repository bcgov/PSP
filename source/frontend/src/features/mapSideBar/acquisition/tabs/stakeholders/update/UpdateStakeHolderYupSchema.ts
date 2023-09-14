/* eslint-disable no-template-curly-in-string */
import * as Yup from 'yup';

export const UpdateStakeHolderYupSchema = Yup.object().shape({
  interestHolders: Yup.array().of(
    Yup.object().shape({
      propertyInterestTypeCode: Yup.string().required('Interest type is required').nullable(),
      impactedProperties: Yup.array().min(1, 'At lease one impacted property is required'),
      contact: Yup.object().required('Interest holder is required').nullable(),
    }),
  ),
  nonInterestPayees: Yup.array().of(
    Yup.object().shape({
      impactedProperties: Yup.array().min(1, 'At lease one impacted property is required'),
      contact: Yup.object().required('Interest holder is required').nullable(),
    }),
  ),
});
