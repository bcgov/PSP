/* eslint-disable no-template-curly-in-string */
import * as yup from 'yup';

export const DispositionOfferFormYupSchema = yup.object().shape({
  dispositionOfferStatusTypeCode: yup.string().nullable(),
  offerName: yup
    .string()
    .nullable()
    .required('Offer Name is required')
    .max(1000, 'Offer Name must be at most ${max} characters'),
  offerDate: yup.string().nullable().required('Offer Date is required'),
  offerExpiryDate: yup.string().nullable(),
  offerAmount: yup.string().nullable().required('Offer Price is required'),
  offerNote: yup
    .string()
    .nullable()
    .max(2000, 'Offer Price comments must be at most ${max} characters'),
});
