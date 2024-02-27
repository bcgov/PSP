import * as yup from 'yup';

import { exists } from '@/utils';
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
  payee: yup.object().shape({
    acquisitionOwnerId: yup.string(),
    pretaxAmount: yup.number(),
    taxAmount: yup.number(),
    totalAmount: yup.number(),
    gstNumber: yup.string().max(50, 'GST # must be at most ${max} characters'),
    isPaymentInTrust: yup.boolean(),
    payeeKey: yup.string().required('Payee is required'),
  }),
  financials: yup.array().of(
    yup.object().shape({
      financialActivityCodeId: yup.object().nullable().required('Activity code is required'),
      isGstRequired: yup.string(),
      pretaxAmount: yup
        .number()
        .transform(value => (isNaN(value) || !exists(value) ? 0 : value))
        .required('Amount is required'),
      taxAmount: yup.number().when('isGstRequired', {
        is: 'true',
        then: yup.number(),
        otherwise: yup.number(),
      }),
    }),
  ),
  detailedRemarks: yup.string().max(2000, 'Detailed remarks must be at most ${max} characters'),
});
