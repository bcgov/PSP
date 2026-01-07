import { FormikProps } from 'formik';
import { useRef } from 'react';

import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';

/**
 * Custom hook to handle form cancellation with confirmation modal.
 * Manages the formikRef, modal state, and cancel logic for forms with unsaved changes.
 *
 * @template T - The form model type
 * @returns Object with formikRef, handleCancelClick, and handleCancelConfirm
 */
export const useFormikCancel = <T>() => {
  const formikRef = useRef<FormikProps<T>>(null);
  const { setModalContent, setDisplayModal } = useModalContext();

  const handleCancelConfirm = () => {
    if (formikRef !== undefined) {
      formikRef.current?.resetForm();
    }
  };

  const handleCancelClick = (onCancelConfirm?: () => void) => {
    if (formikRef !== undefined) {
      if (formikRef.current?.dirty) {
        setModalContent({
          ...getCancelModalProps(),
          handleOk: () => {
            handleCancelConfirm();
            onCancelConfirm && onCancelConfirm();
            // Close modal AFTER all callbacks complete
            setDisplayModal(false);
          },
          handleCancel: () => setDisplayModal(false),
        });
        setDisplayModal(true);
      } else {
        handleCancelConfirm();
        onCancelConfirm && onCancelConfirm();
      }
    } else {
      handleCancelConfirm();
      onCancelConfirm && onCancelConfirm();
    }
  };

  return {
    formikRef,
    handleCancelClick,
    handleCancelConfirm,
  };
};
