import React from 'react';

import GenericModal, { ModalProps } from '@/components/common/GenericModal';

export const CancelConfirmationModal: React.FC<
  React.PropsWithChildren<Partial<ModalProps>>
> = props => {
  const {
    variant = 'info',
    title = 'Confirm Changes',
    message = (
      <>
        <p>If you choose to cancel now, your changes will not be saved.</p>
        <p>Do you want to proceed?</p>
      </>
    ),
    okButtonText = 'Yes',
    cancelButtonText = 'No',
    ...rest
  } = props;

  return (
    <GenericModal
      variant={variant}
      title={title}
      message={message}
      okButtonText={okButtonText}
      cancelButtonText={cancelButtonText}
      {...rest}
    ></GenericModal>
  );
};
