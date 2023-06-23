import React from 'react';

import GenericModal, { ModalProps } from '@/components/common/GenericModal';

export type ICancelConfirmationModalProps = Omit<
  ModalProps,
  'title' | 'message' | 'okButtonText' | 'cancelButtonText'
>;

export const CancelConfirmationModal = (props: ICancelConfirmationModalProps) => (
  <GenericModal
    title="Unsaved Changes"
    message="Confirm cancel adding this contact? Changes will not be saved."
    okButtonText="Confirm"
    cancelButtonText="No"
    {...props}
  ></GenericModal>
);
