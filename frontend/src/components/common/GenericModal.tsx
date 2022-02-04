import classNames from 'classnames';
import React, { useState } from 'react';
import Button from 'react-bootstrap/Button';
import Container from 'react-bootstrap/Container';
import Modal from 'react-bootstrap/Modal';

export enum ModalSize {
  XLARGE = 'modal-xl',
  LARGE = 'modal-l',
  MEDIUM = 'modal-m',
  SMALL = 'modal-s',
}

export interface ModalProps {
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
  title?: string;
  /** Optional message to display - no default. */
  message?: string | React.ReactNode;
  /** allows the parent component to control the display of this modal.
   * Default behaviour is to show this modal on creation and close it on button click. */
  display?: boolean;
  /** set the value of the externally tracked display prop above. */
  setDisplay?: (display: boolean) => void;
  /** optional override to control the x button in the top right of the modal. Default is to show. */
  closeButton?: boolean;
  /** provide the size of the modal, default width is 50.0rem */
  size?: ModalSize;
  className?: string;
}

/**
 * Generic Component used to display modal popups to the user.
 * @param props customize the component with custom text, and an operation to take when the component is closed.
 */
const GenericModal = (props: ModalProps) => {
  const [show, setShow] = useState(true);

  if (
    props.display !== undefined &&
    props.setDisplay === undefined &&
    props.handleOk === undefined &&
    props.handleCancel === undefined
  ) {
    throw Error('Modal has insufficient parameters');
  }
  const showState = props.display !== undefined ? props.display : show;
  const showControl = props.setDisplay !== undefined ? props.setDisplay : setShow;

  const close = () => {
    if (props.handleCancel !== undefined) {
      props.handleCancel();
    } else {
      showControl(false);
    }
  };

  const ok = () => {
    if (props.handleOk !== undefined) {
      props.handleOk();
    } else {
      showControl(false);
    }
  };

  return (
    <Container>
      <Modal
        show={showState}
        onHide={close}
        dialogClassName={classNames(props.size, props.className)}
      >
        <Modal.Header closeButton={props.closeButton}>
          <Modal.Title>{props.title}</Modal.Title>
        </Modal.Header>

        <Modal.Body>{props.message}</Modal.Body>

        <Modal.Footer>
          <Button variant={props.okButtonVariant ?? 'primary'} onClick={ok}>
            {props.okButtonText ?? 'Ok'}
          </Button>
          {props.cancelButtonText && (
            <Button
              variant={props.cancelButtonVariant ?? 'secondary'}
              onClick={close}
              style={{ width: 'unset' }}
            >
              {props.cancelButtonText}
            </Button>
          )}
        </Modal.Footer>
      </Modal>
    </Container>
  );
};

export default GenericModal;
