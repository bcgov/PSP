import React from 'react';

import GenericModal, { ModalProps } from '@/components/common/GenericModal';

export type IRemoveTeamMemberModalProps = Omit<ModalProps, 'okButtonText' | 'cancelButtonText'>;

export const AcquisitionFormModal = (props: IRemoveTeamMemberModalProps) => (
  <GenericModal
    title={props.title}
    message={props.message}
    okButtonText="Ok"
    cancelButtonText="Cancel"
    {...props}
  ></GenericModal>
);
