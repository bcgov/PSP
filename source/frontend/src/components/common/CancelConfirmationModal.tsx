import React from 'react';

import GenericModal, { ModalProps } from '@/components/common/GenericModal';

export const CancelConfirmationModal: React.FC<React.PropsWithChildren<ModalProps>> = props => {
  const {
    title = 'Unsaved Changes',
    message = 'You have made changes on this form. Do you wish to leave without saving?',
    okButtonText = 'Confirm',
    cancelButtonText = 'No',
    ...rest
  } = props;

  return (
    <GenericModal
      title={title}
      message={message}
      okButtonText={okButtonText}
      cancelButtonText={cancelButtonText}
      {...rest}
    ></GenericModal>
  );
};
