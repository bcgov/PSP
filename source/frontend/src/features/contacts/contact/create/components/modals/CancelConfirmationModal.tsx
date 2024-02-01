import GenericModal, { ModalProps } from '@/components/common/GenericModal';

export type ICancelConfirmationModalProps = Omit<
  ModalProps,
  'title' | 'message' | 'okButtonText' | 'cancelButtonText'
>;

export const CancelConfirmationModal = (props: ICancelConfirmationModalProps) => (
  <GenericModal
    title="Confirm Changes"
    message={
      <>
        <p>If you asdasd choose to cancel now, your changes will not be saved.</p>
        <p>Do you want to proceed?</p>
      </>
    }
    okButtonText="Yes"
    cancelButtonText="No"
    {...props}
    variant="info"
  ></GenericModal>
);
