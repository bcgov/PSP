import { FormikHelpers } from 'formik';
import { useCallback } from 'react';

import { NoteTypes } from '@/constants/index';
import { useNoteRepository } from '@/hooks/repositories/useNoteRepository';
import { isValidId } from '@/utils';

import { AddNotesYupSchema } from '../add/AddNotesYupSchema';
import { EntityNoteForm } from '../add/models';

export interface IUseAddNotesFormManagementProps {
  /** The parent entity type for adding notes - e.g. 'activity' */
  type: NoteTypes;
  /** The parent's ID */
  parentId: number;
  /** Optional - callback to execute after note has been added to the datastore */
  onSuccess?: () => void;
}

/**
 * Hook that provides form state and submit handlers for Add Notes.
 */
export function useAddNotesFormManagement(props: IUseAddNotesFormManagementProps) {
  const { addNote } = useNoteRepository();
  const { type, parentId, onSuccess } = props;

  // save handler
  const handleSubmit = useCallback(
    async (values: EntityNoteForm, formikHelpers: FormikHelpers<EntityNoteForm>) => {
      const apiNote = values.toApi();
      const response = await addNote.execute(type, apiNote);
      formikHelpers?.setSubmitting(false);

      if (isValidId(response?.id)) {
        formikHelpers?.resetForm();
        if (typeof onSuccess === 'function') {
          onSuccess();
        }
      }
    },
    [addNote, onSuccess, type],
  );

  const initialValues = new EntityNoteForm();
  initialValues.parentId = parentId;

  return {
    handleSubmit,
    initialValues,
    validationSchema: AddNotesYupSchema,
  };
}
