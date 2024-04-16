import GenericModal, { ModalProps } from '@/components/common/GenericModal';

export type IDuplicateContactModalProps = Omit<
  ModalProps,
  'title' | 'message' | 'okButtonText' | 'cancelButtonText'
>;

export const DuplicateContactModal = (props: IDuplicateContactModalProps) => (
  <GenericModal
    title="Duplicate Contact"
    message="A contact matching this information already exists in the system."
    okButtonText="Continue Save"
    cancelButtonText="No"
    {...props}
  ></GenericModal>
);
