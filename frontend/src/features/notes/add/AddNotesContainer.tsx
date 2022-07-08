import { Formik, FormikHelpers, FormikProps } from 'formik';
import { useHistory } from 'react-router-dom';

import { useAddNote } from '../hooks/useAddNote';
import { AddNotesForm } from './AddNotesForm';
import { AddNotesYupSchema } from './AddNotesYupSchema';
import { EntityNoteForm } from './models';

export interface IAddNotesContainerProps {
  /** The parent entity type for adding notes - e.g. 'activity' */
  parentType: string;
  /** The parent's ID */
  parentId: number;
  /** Whether to show the notes modal. Default: false */
  showNotes: boolean;
  /** set the value of the externally tracked 'showNotes' prop above. */
  setShowNotes: (show: boolean) => void;
}

export const AddNotesContainer: React.FC<IAddNotesContainerProps> = props => {
  const history = useHistory();
  const { addNote } = useAddNote();

  // save handler
  const saveNote = async (values: EntityNoteForm, formikHelpers: FormikHelpers<EntityNoteForm>) => {
    const apiNote = values.toApi();
    const response = await addNote(props.parentType, apiNote);
    formikHelpers?.setSubmitting(false);

    if (!!response?.id) {
      formikHelpers?.resetForm();
      // TODO: navigate to Notes LIST VIEW
      history.replace(`/mapview`);
    }
  };

  const onSaveClick = (noteForm: EntityNoteForm, formikProps: FormikProps<EntityNoteForm>) => {
    formikProps?.setSubmitting(true);
    formikProps?.submitForm();
  };

  const onCancelClick = (formikProps: FormikProps<EntityNoteForm>) => {
    formikProps?.resetForm();
  };

  const noteForm = new EntityNoteForm();
  noteForm.parentId = props.parentId;

  return (
    <Formik<EntityNoteForm>
      enableReinitialize
      validationSchema={AddNotesYupSchema}
      initialValues={noteForm}
      onSubmit={saveNote}
    >
      {() => (
        <AddNotesForm
          showNotes={props.showNotes}
          setShowNotes={props.setShowNotes}
          onSave={onSaveClick}
          onCancel={onCancelClick}
        />
      )}
    </Formik>
  );
};
