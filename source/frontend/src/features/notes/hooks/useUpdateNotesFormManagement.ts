import { FormikHelpers } from 'formik';
import { useCallback } from 'react';

import { NoteTypes } from '@/constants/index';
import { useNoteRepository } from '@/hooks/repositories/useNoteRepository';
import { Api_Note } from '@/models/api/Note';

import { NoteForm } from '../models';
import { UpdateNoteYupSchema } from '../update/UpdateNoteYupSchema';

export interface IUseUpdateNotesFormManagementProps {
  /** The parent entity type for adding notes - e.g. 'activity' */
  type: NoteTypes;
  /** The note to update */
  note?: Api_Note;
  /** Optional - callback to execute after a successful update */
  onSuccess?: () => void;
}

/**
 * Hook that provides form state and submit handlers for Add Notes.
 */
export function useUpdateNotesFormManagement(props: IUseUpdateNotesFormManagementProps) {
  const { updateNote } = useNoteRepository();
  const { note, onSuccess } = props;

  // save handler
  const handleSubmit = useCallback(
    async (values: NoteForm, formikHelpers: FormikHelpers<NoteForm>) => {
      const apiNote = values.toApi();
      const response = await updateNote.execute(apiNote);
      formikHelpers?.setSubmitting(false);

      if (!!response?.id) {
        formikHelpers?.resetForm();
        if (typeof onSuccess === 'function') {
          onSuccess();
        }
      }
    },
    [updateNote, onSuccess],
  );

  return {
    handleSubmit,
    initialValues: note ? NoteForm.fromApi(note) : new NoteForm(),
    validationSchema: UpdateNoteYupSchema,
  };
}
