import * as Yup from 'yup';

export const AddLeaseTenantYupSchema = Yup.object().shape({
  tenants: Yup.array().of(
    Yup.object().shape({
      note: Yup.string().max(2000, 'note must be at most 2000 characters'),
    }),
  ),
});
