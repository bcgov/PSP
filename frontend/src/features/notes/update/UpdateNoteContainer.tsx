import { CancelConfirmationModal } from 'components/common/CancelConfirmationModal';
import { NoteTypes } from 'constants/index';
import { FormikProps } from 'formik';
import { useModalManagement } from 'hooks/useModalManagement';
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
  /** Optional - callback to notify when save button is pressed. */
  onSaveClick?: (noteForm: NoteForm, formikProps: FormikProps<NoteForm>) => void;
  /** Optional - callback to notify when cancel button is pressed. */
  onCancelClick?: (formikProps: FormikProps<NoteForm>) => void;
}

export const UpdateNoteContainer: React.FC<IUpdateNoteContainerProps> = props => {
  const [showConfirmModal, openConfirmModal, closeConfirmModal] = useModalManagement();

  const { handleSubmit, initialValues, validationSchema } = useUpdateNotesFormManagement({
    type: props.type,
    note: props.note,
    onSuccess: props.onSuccess,
  });

  const handleSaveClick = async (values: NoteForm, formikProps: FormikProps<NoteForm>) => {
    formikProps?.setSubmitting(true);
    await formikProps?.submitForm();
    props.onSaveClick && props.onSaveClick(values, formikProps);
  };

  const handleCancelClick = (formikProps: FormikProps<NoteForm>) => {
    if (formikProps?.dirty && formikProps.submitCount === 0) {
      openConfirmModal();
    } else {
      handleCancelConfirm(formikProps);
    }
  };

  const handleCancelConfirm = (formikProps: FormikProps<NoteForm>) => {
    closeConfirmModal();
    formikProps?.resetForm();
    props.onCancelClick && props.onCancelClick(formikProps);
  };

  return (
    <>
      <UpdateNoteFormModal
        isOpened={props.isOpened}
        loading={props.loading}
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
