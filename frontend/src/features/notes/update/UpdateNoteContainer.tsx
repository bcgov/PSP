import { NoteTypes } from 'constants/index';
import { FormikProps } from 'formik';
import { Api_Note } from 'models/api/Note';

import { useUpdateNotesFormManagement } from '../hooks/useUpdateNotesFormManagement';
import { NoteForm } from '../models';
import { UpdateNoteFormModal } from './UpdateNoteFormModal';

export interface IUpdateNoteContainerProps {
  /** Whether the to show a loading spinner instead of the form */
  loading?: boolean;
  /** The parent entity type for adding notes - e.g. 'activity' */
  type: NoteTypes;
  /** The note to update */
  note?: Api_Note;
  /** Optional - callback to execute after a successful update */
  onSuccess?: () => void;
  /** Whether to show the notes modal. Default: false */
  isOpened: boolean;
  /** set the value of the externally tracked 'isOpened' prop above. */
  openModal: () => void;
  /** set the value of the externally tracked 'isOpened' prop above. */
  closeModal: () => void;
}

export const UpdateNoteContainer: React.FC<IUpdateNoteContainerProps> = props => {
  const { handleSubmit, initialValues, validationSchema } = useUpdateNotesFormManagement({
    type: props.type,
    note: props.note,
    onSuccess: props.onSuccess,
  });

  const onSaveClick = (values: NoteForm, formikProps: FormikProps<NoteForm>) => {
    formikProps?.setSubmitting(true);
    formikProps?.submitForm();
    props.closeModal();
  };

  const onCancelClick = (formikProps: FormikProps<NoteForm>) => {
    formikProps?.resetForm();
    props.closeModal();
  };

  return (
    <UpdateNoteFormModal
      isOpened={props.isOpened}
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={handleSubmit}
      onSaveClick={onSaveClick}
      onCancelClick={onCancelClick}
    />
  );
};
