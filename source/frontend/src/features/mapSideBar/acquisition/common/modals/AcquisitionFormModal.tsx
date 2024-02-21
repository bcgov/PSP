import GenericModal, { ModalProps } from '@/components/common/GenericModal';

export type IRemoveTeamMemberModalProps = Omit<
  ModalProps,
  'variant' | 'okButtonText' | 'cancelButtonText'
>;

export const AcquisitionFormModal = (props: IRemoveTeamMemberModalProps) => (
  <GenericModal
    variant="info"
    title={props.title}
    message={props.message}
    okButtonText="Yes"
    cancelButtonText="No"
    {...props}
  ></GenericModal>
);
