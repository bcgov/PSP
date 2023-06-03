/* eslint-disable no-template-curly-in-string */
import * as Yup from 'yup';

export const UpdateStakeHolderYupSchema = Yup.object().shape({
  interestHolders: Yup.array().of(
    Yup.object().shape({
      legalSurveyPlanNum: Yup.string().max(
        250,
        'Legal survey plan must be less than 250 characters',
      ),
      interestTypeCode: Yup.string().required('Interest type is required').nullable(),
      impactedProperties: Yup.array().min(1, 'At lease one impacted property is required'),
      contact: Yup.object().required('Interest holder is required').nullable(),
    }),
  ),
  nonInterestPayees: Yup.array().min(0),
});
