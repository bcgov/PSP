import { useEffect } from 'react';
import { useContext } from 'react';

import { ModalProps } from './../components/common/GenericModal';
import { ModalContext } from './../contexts/modalContext';
export const useModalContext = (newModalProps?: ModalProps) => {
  const { modalProps, setModalProps, setDisplayModal } = useContext(ModalContext);
  // if the modal props were passed in to the hook as a param, set them during hook initialization only. Ignore all future updates to newModalProps.
  useEffect(() => {
    if (newModalProps !== undefined) {
      setModalProps({ ...newModalProps });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return { modalProps, setModalProps, displayModal: modalProps?.display, setDisplayModal };
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
