import { Formik, FormikHelpers, FormikProps } from 'formik';

import { AddNotesForm } from './AddNotesForm';
import { NoteForm } from './models';

export interface IAddNotesContainerProps {
  /** Whether to show the notes modal. Default: false */
  showNotes: boolean;
  /** set the value of the externally tracked 'showNotes' prop above. */
  setShowNotes: (show: boolean) => void;
}

export const AddNotesContainer: React.FC<IAddNotesContainerProps> = props => {
  // save handler
  const saveNote = async (values: NoteForm, formikHelpers: FormikHelpers<NoteForm>) => {
    // TODO: implement
    const apiNote = values.toApi();

    console.log(apiNote);

    formikHelpers?.setSubmitting(false);
    formikHelpers?.resetForm();
  };

  const onSaveClick = (noteForm: NoteForm, formikProps: FormikProps<NoteForm>) => {
    formikProps?.setSubmitting(true);
    formikProps?.submitForm();
  };

  const onCancelClick = (formikProps: FormikProps<NoteForm>) => {
    formikProps?.resetForm();
  };

  return (
    <Formik<NoteForm> enableReinitialize initialValues={new NoteForm()} onSubmit={saveNote}>
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
