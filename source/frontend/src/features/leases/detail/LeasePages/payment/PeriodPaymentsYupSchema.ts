import * as Yup from 'yup';

export const PeriodPaymentsYupSchema = Yup.object().shape({
  periods: Yup.array().of(
    Yup.object().shape({
      payments: Yup.array().of(
        Yup.object().shape({ note: Yup.string().max(2000, 'note can be at most 2000 characters') }),
      ),
    }),
  ),
});
