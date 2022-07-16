import { NoteTypes } from 'constants/index';
import { FormikHelpers } from 'formik';
import { useCallback } from 'react';
import { useHistory } from 'react-router-dom';

import { AddNotesYupSchema } from '../add/AddNotesYupSchema';
import { EntityNoteForm } from '../add/models';
import { useNoteRepository } from './useNoteRepository';

export interface IUseAddNotesFormManagementProps {
  /** The parent entity type for adding notes - e.g. 'activity' */
  type: NoteTypes;
  /** The parent's ID */
  parentId: number;
}

/**
 * Hook that provides form state and submit handlers for Add Notes.
 */
export function useAddNotesFormManagement(props: IUseAddNotesFormManagementProps) {
  const history = useHistory();
  const { addNote } = useNoteRepository();

  // save handler
  const handleSubmit = useCallback(
    async (values: EntityNoteForm, formikHelpers: FormikHelpers<EntityNoteForm>) => {
      const apiNote = values.toApi();
      const response = await addNote.execute(props.type, apiNote);
      formikHelpers?.setSubmitting(false);

      if (!!response?.id) {
        formikHelpers?.resetForm();
        // TODO: navigate to Notes LIST VIEW
        history.replace(`/mapview`);
      }
    },
    [addNote, history, props.type],
  );

  const initialValues = new EntityNoteForm();
  initialValues.parentId = props.parentId;

  return {
    handleSubmit,
    initialValues,
    validationSchema: AddNotesYupSchema,
  };
}
