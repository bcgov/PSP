import * as Yup from 'yup';

export const PropertyActivityEditFormYupSchema = Yup.object().shape({
  activityTypeCode: Yup.string().required('Activity type is required'),
  activitySubtypeCode: Yup.string().required('Sub-type is required'),
  activityStatusCode: Yup.string().required('Status is required'),
  requestedDate: Yup.string().required('Requested added date is required'),

  completionDate: Yup.string().when('activityStatusCode', {
    is: (activityStatusCode: string) => activityStatusCode === 'COMPLETED',
    then: Yup.string().required('Completion date is required'),
  }),
  description: Yup.string()
    .required('Description is required')
    .max(2000, 'Description must be at most 2000 characters'),

  requestedSource: Yup.string().max(2000, 'Requested source must be at most 2000 characters'),

  invoices: Yup.array().of(
    Yup.object().shape({
      invoiceDateTime: Yup.string().required('Invoice date is required'),
      description: Yup.string()
        .required('Description is required')
        .max(1000, 'Description must be at most 1000 characters'),
      pretaxAmount: Yup.string().required('Pre-tax amount is required'),
      //pstAmount: Yup.number().required('Pst amount is required'),
      isPstRequired: Yup.boolean().required('Is-Pst-required is required'),
      pstAmount: Yup.number().when('isPstRequired', {
        is: (isPstRequired: boolean) => isPstRequired === true,
        then: Yup.number()
          .moreThan(0, 'Pst must be greater than 0')
          .required('Pst amount is required'),
      }),
    }),
  ),
});
