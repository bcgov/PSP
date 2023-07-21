import classNames from 'classnames';
import { noop } from 'lodash';
import React, { useState } from 'react';
import { Modal, ModalProps as BsModalProps } from 'react-bootstrap';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';

export enum ModalSize {
  XLARGE = 'modal-xl',
  LARGE = 'modal-l',
  MEDIUM = 'modal-m',
  SMALL = 'modal-s',
}

export interface ModalVisibleState {
  /** allows the parent component to control the display of this modal.
   * Default behaviour is to show this modal on creation and close it on button click. */
  display?: boolean;
  /** set the value of the externally tracked display prop above. */
  setDisplay?: (display: boolean) => void;
}

export interface ModalContent {
  /** Optional function to control behaviour of cancel button. Default is to close the modal. */
  handleCancel?: Function;
  /** Optional function to control behaviour of ok button. Default is to reload the app. */
  handleOk?: Function;
  /** Optional text to display on the cancel button. Default is Cancel. */
  cancelButtonText?: string;
  /** Optional variant that will override the default variant of warning. */
  cancelButtonVariant?:
    | 'link'
    | 'warning'
    | 'primary'
    | 'secondary'
    | 'success'
    | 'danger'
    | 'info'
    | 'dark'
    | 'light'
    | 'outline-primary'
    | 'outline-secondary'
    | 'outline-success'
    | 'outline-danger'
    | 'outline-info'
    | 'outline-dark'
    | 'outline-light';
  /** Optional test to display on the ok button. Default is Ok. */
  okButtonText?: string;
  /** Optional variant that will override the default variant of primary. */
  okButtonVariant?:
    | 'link'
    | 'warning'
    | 'primary'
    | 'secondary'
    | 'success'
    | 'danger'
    | 'info'
    | 'dark'
    | 'light'
    | 'outline-primary'
    | 'outline-secondary'
    | 'outline-success'
    | 'outline-danger'
    | 'outline-info'
    | 'outline-dark'
    | 'outline-light';
  /** Optional title to display - no default. */
  title?: string | React.ReactNode;
  /** Optional message to display - no default. */
  message?: string | React.ReactNode;
  /** optional override to control the x button in the top right of the modal. Default is to show. */
  closeButton?: boolean;
  /** provide the size of the modal, default width is 50.0rem */
  modalSize?: ModalSize;
  className?: string;
  /** display this modal as a popup instead of as a modal, allowing the user to click on underlying elements */
  asPopup?: boolean;
  show?: boolean;
  /** optional override to hide the footer of the modal modal. Default is to show. */
  hideFooter?: boolean;
}

export type ModalProps = ModalVisibleState & ModalContent;

/**
 * Generic Component used to display modal popups to the user.
 * @param props customize the component with custom text, and an operation to take when the component is closed.
 */
export const GenericModal = (props: Omit<BsModalProps, 'onHide'> & ModalProps) => {
  const {
    display,
    setDisplay,
    handleOk,
    handleCancel,
    title,
    message,
    okButtonVariant,
    okButtonText,
    cancelButtonVariant,
    cancelButtonText,
    closeButton,
    hideFooter,
    modalSize,
    ...rest
  } = props;
  const [show, setShow] = useState(true);

  if (
    display !== undefined &&
    setDisplay === undefined &&
    handleOk === undefined &&
    handleCancel === undefined
  ) {
    throw Error('Modal has insufficient parameters');
  }
  const showState = display !== undefined ? display : show;
  const showControl = setDisplay !== undefined ? setDisplay : setShow;

  const close = () => {
    if (handleCancel !== undefined) {
      handleCancel();
    } else {
      showControl(false);
    }
  };

  const ok = () => {
    if (handleOk !== undefined) {
      handleOk();
    } else {
      showControl(false);
    }
  };

  return (
    <ModalContainer {...rest} show={showState} modalSize={modalSize} onHide={noop}>
      <Modal.Header closeButton={closeButton} onHide={close}>
        <Modal.Title>{title}</Modal.Title>
      </Modal.Header>

      <Modal.Body style={{ whiteSpace: 'pre-line' }}>{message}</Modal.Body>

      {!hideFooter && (
        <Modal.Footer>
          {cancelButtonText && (
            <Button
              title="cancel-modal"
              variant={cancelButtonVariant ?? 'secondary'}
              onClick={close}
              style={{ width: 'unset' }}
            >
              {cancelButtonText}
            </Button>
          )}
          <Button title="ok-modal" variant={okButtonVariant ?? 'primary'} onClick={ok}>
            {okButtonText ?? 'Ok'}
          </Button>
        </Modal.Footer>
      )}
    </ModalContainer>
  );
};

const ModalContainer = (props: BsModalProps & ModalProps) => {
  const { modalSize, ...rest } = props;

  return !props.asPopup ? (
    <div>
      <StyledModal
        {...rest}
        show={props.show}
        onHide={props.close}
        dialogClassName={classNames(modalSize, props.className)}
      >
        {props.children}
      </StyledModal>
    </div>
  ) : props.show ? (
    <PopupContainer className={classNames(modalSize, props.className)}>
      {props.children}
    </PopupContainer>
  ) : null;
};

const StyledModal = styled(Modal)`
  .modal-header {
    height: 3.8rem;
    padding: 0 1rem;
    background-color: ${({ theme }) => theme.css.primaryColor};
    .h4 {
      color: white;
      font-family: BcSans-Bold;
      font-size: 2.2rem;
      height: 2.75rem;
    }
    .close {
      color: white;
      opacity: 1;
      font-size: 3rem;
      font-weight: 10;
      text-shadow: none;
      font-family: 'Helvetica Narrow';
    }
  }
`;

const PopupContainer = styled.div`
  position: fixed;
  background-color: white;
  box-shadow: 0px 1px 4px rgba(0, 0, 0, 0.3);
  top: 0;
  z-index: 1;
  .modal-header {
    height: 3.8rem;
    padding: 0 1rem;
    background-color: ${({ theme }) => theme.css.primaryColor};
    .h4 {
      color: white;
      font-family: BcSans-Bold;
      font-size: 2.2rem;
      height: 2.75rem;
    }
    .close {
      color: white;
      opacity: 1;
    }
  }
`;

export default GenericModal;
