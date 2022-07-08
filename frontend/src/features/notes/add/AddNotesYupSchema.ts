import * as Yup from 'yup';

export const AddNotesYupSchema = Yup.object().shape({
  note: Yup.object().shape({
    note: Yup.string().max(4000, 'Notes must be at most 4000 characters'),
  }),
});
