import { NoteTypes } from 'constants/index';
import { FormikProps } from 'formik';

import { useAddNotesFormManagement } from '../hooks/useAddNotesFormManagement';
import { AddNotesFormModal } from './AddNotesFormModal';
import { EntityNoteForm } from './models';

export interface IAddNotesContainerProps {
  /** The parent entity type for adding notes - e.g. 'activity' */
  type: NoteTypes;
  /** The parent's ID */
  parentId: number;
  /** Whether to show the notes modal. Default: false */
  isOpened: boolean;
  /** set the value of the externally tracked 'isOpened' prop above. */
  openModal: () => void;
  /** set the value of the externally tracked 'isOpened' prop above. */
  closeModal: () => void;
  /** Optional - callback to execute after note has been added to the datastore */
  onSuccess?: () => void;
}

export const AddNotesContainer: React.FC<IAddNotesContainerProps> = props => {
  const { handleSubmit, initialValues, validationSchema } = useAddNotesFormManagement({
    type: props.type,
    parentId: props.parentId,
    onSuccess: props.onSuccess,
  });

  const onSaveClick = (noteForm: EntityNoteForm, formikProps: FormikProps<EntityNoteForm>) => {
    formikProps?.setSubmitting(true);
    formikProps?.submitForm();
  };

  const onCancelClick = (formikProps: FormikProps<EntityNoteForm>) => {
    formikProps?.resetForm();
  };

  return (
    <AddNotesFormModal
      isOpened={props.isOpened}
      openModal={props.openModal}
      closeModal={props.closeModal}
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={handleSubmit}
      onSaveClick={onSaveClick}
      onCancelClick={onCancelClick}
    />
  );
};
