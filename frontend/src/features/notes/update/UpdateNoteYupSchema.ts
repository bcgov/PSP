import * as Yup from 'yup';

export const UpdateNoteYupSchema = Yup.object().shape({
  note: Yup.string()
    .required('Notes are required')
    .max(4000, 'Notes must be at most 4000 characters'),
});
