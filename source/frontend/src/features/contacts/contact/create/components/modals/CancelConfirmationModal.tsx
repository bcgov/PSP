import React from 'react';

import GenericModal, { ModalProps } from '@/components/common/GenericModal';

export type ICancelConfirmationModalProps = Omit<
  ModalProps,
  'title' | 'message' | 'okButtonText' | 'cancelButtonText'
>;

export const CancelConfirmationModal = (props: ICancelConfirmationModalProps) => (
  <GenericModal
    title="Unsaved Changes"
    message={
      <>
        <div>If you choose to cancel now, your changes will not be saved.</div>
        <br />
        <strong>Do you want to proceed?</strong>
      </>
    }
    okButtonText="Yes"
    cancelButtonText="No"
    {...props}
    variant="info"
  ></GenericModal>
);
