import { FormikProps } from 'formik';

import { useAddNotesFormManagement } from '../hooks/useAddNotesFormManagement';
import { AddNotesFormModal } from './AddNotesFormModal';
import { EntityNoteForm } from './models';

export interface IAddNotesContainerProps {
  /** The parent entity type for adding notes - e.g. 'activity' */
  parentType: string;
  /** The parent's ID */
  parentId: number;
  /** Whether to show the notes modal. Default: false */
  isOpened: boolean;
  /** set the value of the externally tracked 'isOpened' prop above. */
  openModal: () => void;
  /** set the value of the externally tracked 'isOpened' prop above. */
  closeModal: () => void;
}

export const AddNotesContainer: React.FC<IAddNotesContainerProps> = props => {
  const { handleSubmit, initialValues, validationSchema } = useAddNotesFormManagement({
    parentType: props.parentType,
    parentId: props.parentId,
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
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={handleSubmit}
      isOpened={props.isOpened}
      openModal={props.openModal}
      closeModal={props.closeModal}
      onSaveClick={onSaveClick}
      onCancelClick={onCancelClick}
    />
  );
};
