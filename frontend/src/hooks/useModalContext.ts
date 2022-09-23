import { useEffect } from 'react';
import { useContext } from 'react';

import { ModalContent } from './../components/common/GenericModal';
import { ModalContext } from './../contexts/modalContext';

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

export const getCancelModalProps = () => ({
  title: 'Unsaved Changes',
  message: 'You have made changes on this form. Do you wish to leave without saving?',
  okButtonText: 'Confirm',
  cancelButtonText: 'No',
});

export const getDeleteModalProps = () => ({
  title: 'Confirm Delete',
  message: 'Are you sure you want to delete this item?',
  okButtonText: 'Continue',
  cancelButtonText: 'Cancel',
});
