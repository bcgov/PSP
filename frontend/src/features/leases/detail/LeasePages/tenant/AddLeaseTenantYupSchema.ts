import * as Yup from 'yup';

export const AddLeaseTenantYupSchema = Yup.object().shape({
  note: Yup.string().max(2000),
});
