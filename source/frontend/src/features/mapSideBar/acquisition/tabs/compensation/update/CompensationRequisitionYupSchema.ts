import * as yup from 'yup';
/* eslint-disable no-template-curly-in-string */

export const CompensationRequisitionYupSchema = yup.object().shape({
  agreementDate: yup.string(),
  expropriationNoticeServedDateTime: yup.string(),
  expropriationVestingDateTime: yup.string(),
  generationDatetTime: yup.string(),
  status: yup.string().required('Status is required'),
  fiscalYear: yup.string().when('status', {
    is: 'final',
    then: yup.string().required('Fiscal year is required'),
  }),
  specialInstruction: yup
    .string()
    .max(2000, 'Special instructions must be at most ${max} characters'),
  payees: yup.array().of(
    yup.object().shape({
      acquisitionOwnerId: yup.string(),
      pretaxAmount: yup.number(),
      taxAmount: yup.number(),
      totalAmount: yup.number(),
      gstNumber: yup.string().max(50, 'GST # must be at most ${max} characters'),
      isPaymentInTrust: yup.boolean(),
    }),
  ),
  financials: yup.array().of(
    yup.object().shape({
      financialActivityCodeId: yup.string().required('Activity code is required'),
      isGstRequired: yup.string(),
      pretaxAmount: yup
        .number()
        .transform(value => (isNaN(value) || value === null || value === undefined ? 0 : value))
        .required('Amount is required')
        .moreThan(0, 'Amount must be greater than 0'),
      taxAmount: yup.number().when('isGstRequired', {
        is: 'true',
        then: yup.number().moreThan(0, 'Amount must be greater than 0'),
        otherwise: yup.number(),
      }),
    }),
  ),
  detailedRemarks: yup.string().max(2000, 'Detailed remarks must be at most ${max} characters'),
  payeeKey: yup.string().required('Payee is required'),
});
