import { useContext, useEffect } from 'react';

import { ModalContent } from '../components/common/GenericModal';
import { ModalContext } from '../contexts/modalContext';

export const useModalContext = (newModalContent?: ModalContent, isVisible?: boolean) => {
  const { modalProps, setModalContent, setDisplayModal } = useContext(ModalContext);
  // if the modal props were passed in to the hook as a param, set them during hook initialization only. Ignore all future updates to newModalProps.
  useEffect(() => {
    if (newModalContent !== undefined) {
      setModalContent({ ...newModalContent });
    }
    if (isVisible !== undefined) {
      setDisplayModal(isVisible);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return {
    modalProps,
    setModalContent,
    displayModal: modalProps?.display,
    setDisplayModal,
  };
};

export const getCancelModalProps = (): ModalContent => ({
  title: 'Confirm Changes',
  message: (
    <>
      <p>If you choose to cancel now, your changes will not be saved.</p>
      <p>Do you want to proceed?</p>
    </>
  ),
  okButtonText: 'Yes',
  cancelButtonText: 'No',
  variant: 'info',
});

export const getDeleteModalProps = (): ModalContent => ({
  title: 'Confirm Delete',
  message: 'Are you sure you want to delete this item?',
  okButtonText: 'Yes',
  cancelButtonText: 'No',
  variant: 'info',
});
