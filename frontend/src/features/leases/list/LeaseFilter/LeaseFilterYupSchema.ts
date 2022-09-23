import * as Yup from 'yup';

export const LeaseFilterSchema = Yup.object().shape({
  expiryStartDate: Yup.date(),
  expiryEndDate: Yup.date().when(
    'expiryStartDate',
    (expiryStartDate: any, schema: any) =>
      expiryStartDate &&
      schema.min(Yup.ref('expiryStartDate'), 'Expiry "to" Date must be after "from" Date'),
  ),
});
