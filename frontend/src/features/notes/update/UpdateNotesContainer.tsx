import { FormikProps } from 'formik';

import { useAddNotesFormManagement } from '../hooks/useAddNotesFormManagement';
import { AddNotesFormModal } from './AddNotesFormModal';
import { UpdateEntityNoteForm } from './models';

export interface IUpdateNotesContainerProps {
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

export const UpdateNotesContainer: React.FC<IUpdateNotesContainerProps> = props => {
  const { handleSubmit, initialValues, validationSchema } = useAddNotesFormManagement({
    parentType: props.parentType,
    parentId: props.parentId,
  });

  const onSaveClick = (
    values: UpdateEntityNoteForm,
    formikProps: FormikProps<UpdateEntityNoteForm>,
  ) => {
    formikProps?.setSubmitting(true);
    formikProps?.submitForm();
  };

  const onCancelClick = (formikProps: FormikProps<UpdateEntityNoteForm>) => {
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
