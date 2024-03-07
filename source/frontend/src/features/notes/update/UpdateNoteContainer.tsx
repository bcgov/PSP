import { FormikProps } from 'formik';
import { useRef } from 'react';

import { CancelConfirmationModal } from '@/components/common/CancelConfirmationModal';
import { NoteTypes } from '@/constants/index';
import { useModalManagement } from '@/hooks/useModalManagement';
import { ApiGen_Concepts_Note } from '@/models/api/generated/ApiGen_Concepts_Note';

import { useUpdateNotesFormManagement } from '../hooks/useUpdateNotesFormManagement';
import { NoteForm } from '../models';
import { UpdateNoteFormModal } from './UpdateNoteFormModal';

export interface IUpdateNoteContainerProps {
  /** Whether the to show a loading spinner instead of the form */
  loading?: boolean;
  /** The parent entity type for adding notes - e.g. 'activity' */
  type: NoteTypes;
  /** The note to update */
  note?: ApiGen_Concepts_Note;
  /** Optional - callback to execute after a successful update */
  onSuccess?: () => void;
  /** Whether to show the notes modal. Default: false */
  isOpened: boolean;
  /** Optional - callback to notify when save button is pressed. */
  onSaveClick?: () => void;
  /** Optional - callback to notify when cancel button is pressed. */
  onCancelClick?: () => void;
}

export const UpdateNoteContainer: React.FC<
  React.PropsWithChildren<IUpdateNoteContainerProps>
> = props => {
  const [showConfirmModal, openConfirmModal, closeConfirmModal] = useModalManagement();
  const formikRef = useRef<FormikProps<NoteForm>>(null);

  const { handleSubmit, initialValues, validationSchema } = useUpdateNotesFormManagement({
    type: props.type,
    note: props.note,
    onSuccess: props.onSuccess,
  });

  const handleSaveClick = async () => {
    if (formikRef.current !== null) {
      formikRef.current.setSubmitting(true);
      await formikRef.current.submitForm();
      if (formikRef.current.isValid) {
        props.onSaveClick && props.onSaveClick();
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
    props.onCancelClick && props.onCancelClick();
  };

  return (
    <>
      <UpdateNoteFormModal
        ref={formikRef}
        isOpened={props.isOpened}
        loading={props.loading}
        initialValues={initialValues}
        validationSchema={validationSchema}
        onSubmit={handleSubmit}
        onSaveClick={handleSaveClick}
        onCancelClick={handleCancelClick}
      />

      <CancelConfirmationModal
        variant="info"
        display={showConfirmModal}
        handleOk={handleCancelConfirm}
        handleCancel={closeConfirmModal}
      />
    </>
  );
};
