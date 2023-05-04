import * as yup from 'yup';
/* eslint-disable no-template-curly-in-string */

export const CompensationRequisitionYupSchema = yup.object().shape({
  agreementDate: yup.string(),
  status: yup.string(),
  fiscalYear: yup.string().when('status', {
    is: 'final',
    then: yup.string().required('Fiscal year is required'),
  }),
  specialInstruction: yup
    .string()
    .max(2000, 'Special instructions must be at most ${max} characters'),
  detailedRemarks: yup.string().max(2000, 'Detailed remarks must be at most ${max} characters'),
});
