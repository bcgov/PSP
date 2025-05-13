/* eslint-disable no-template-curly-in-string */
import * as Yup from 'yup';

export const ManagementActivityEditFormYupSchema = Yup.object().shape({
  activityTypeCode: Yup.string().required('Activity type is required'),
  activitySubtypeCode: Yup.string().required('Sub-type is required'),
  activityStatusCode: Yup.string().required('Status is required'),
  requestedDate: Yup.date().required('Commencement date is required'),

  completionDate: Yup.date().when('activityStatusCode', {
    is: (activityStatusCode: string) => activityStatusCode === 'COMPLETED',
    then: Yup.date()
      .min(Yup.ref('requestedDate'), 'Completion date must be after Commencement date')
      .required('Completion date is required'),
    otherwise: Yup.date()
      .min(Yup.ref('requestedDate'), 'Completion date must be after Commencement date')
      .nullable(),
  }),
  description: Yup.string()
    .required('Description is required')
    .max(2000, 'Description must be at most ${max} characters'),

  requestedSource: Yup.string()
    .nullable()
    .max(2000, 'Contact manager must be at most ${max} characters'),

  invoices: Yup.array().of(
    Yup.object().shape({
      invoiceDateTime: Yup.string().required('Invoice date is required'),
      description: Yup.string()
        .required('Description is required')
        .max(1000, 'Description must be at most ${max} characters'),
      pretaxAmount: Yup.string().required('Pre-tax amount is required'),
      isPstRequired: Yup.boolean().required('Is-Pst-required is required'),
      pstAmount: Yup.number().when('isPstRequired', {
        is: (isPstRequired: boolean) => isPstRequired === true,
        then: Yup.number()
          .moreThan(0, 'PST must be greater than 0')
          .required('PST amount is required'),
      }),
    }),
  ),
});
