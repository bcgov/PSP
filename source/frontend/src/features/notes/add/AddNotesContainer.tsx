import { FormikProps } from 'formik';
import { useRef } from 'react';

import { CancelConfirmationModal } from '@/components/common/CancelConfirmationModal';
import { NoteTypes } from '@/constants/index';
import { useModalManagement } from '@/hooks/useModalManagement';

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

export const AddNotesContainer: React.FC<
  React.PropsWithChildren<IAddNotesContainerProps>
> = props => {
  const [showConfirmModal, openConfirmModal, closeConfirmModal] = useModalManagement();
  const formikRef = useRef<FormikProps<EntityNoteForm>>(null);

  const { handleSubmit, initialValues, validationSchema } = useAddNotesFormManagement({
    type: props.type,
    parentId: props.parentId,
    onSuccess: props.onSuccess,
  });

  const handleSaveClick = async () => {
    if (formikRef.current !== null) {
      formikRef.current.setSubmitting(true);
      await formikRef.current.submitForm();
      if (formikRef.current.isValid) {
        props.closeModal && props.closeModal();
      }
    }
  };

  const handleCancelClick = () => {
    if (formikRef.current?.dirty) {
      openConfirmModal();
    } else {
      handleCancelConfirm();
    }
  };

  const handleCancelConfirm = () => {
    closeConfirmModal();
    formikRef.current?.resetForm();
    props.closeModal && props.closeModal();
  };

  return (
    <>
      <AddNotesFormModal
        ref={formikRef}
        isOpened={props.isOpened}
        initialValues={initialValues}
        validationSchema={validationSchema}
        onSubmit={handleSubmit}
        onSaveClick={handleSaveClick}
        onCancelClick={handleCancelClick}
      />

      <CancelConfirmationModal
        display={showConfirmModal}
        handleOk={handleCancelConfirm}
        handleCancel={closeConfirmModal}
      />
    </>
  );
};
