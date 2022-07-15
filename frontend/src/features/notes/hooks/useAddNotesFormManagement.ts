import { FormikHelpers } from 'formik';
import { useCallback } from 'react';
import { useHistory } from 'react-router-dom';

import { AddNotesYupSchema } from '../add/AddNotesYupSchema';
import { EntityNoteForm } from '../add/models';
import { useAddNote } from '../hooks/useAddNote';

export interface IUseAddNotesFormManagementProps {
  /** The parent entity type for adding notes - e.g. 'activity' */
  parentType: string;
  /** The parent's ID */
  parentId: number;
}

/**
 * Hook that provides form state and submit handlers for Add Notes.
 */
export function useAddNotesFormManagement(props: IUseAddNotesFormManagementProps) {
  const history = useHistory();
  const { addNote } = useAddNote();

  // save handler
  const handleSubmit = useCallback(
    async (values: EntityNoteForm, formikHelpers: FormikHelpers<EntityNoteForm>) => {
      const apiNote = values.toApi();
      const response = await addNote(props.parentType, apiNote);
      formikHelpers?.setSubmitting(false);

      if (!!response?.id) {
        formikHelpers?.resetForm();
        // TODO: navigate to Notes LIST VIEW
        history.replace(`/mapview`);
      }
    },
    [addNote, history, props.parentType],
  );

  const initialValues = new EntityNoteForm();
  initialValues.parentId = props.parentId;

  return {
    handleSubmit,
    initialValues,
    validationSchema: AddNotesYupSchema,
  };
}
