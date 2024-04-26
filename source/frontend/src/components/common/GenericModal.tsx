import classNames from 'classnames';
import noop from 'lodash/noop';
import React, { useState } from 'react';
import {
  Modal,
  ModalDialog,
  ModalDialogProps as BsModalDialogProps,
  ModalProps as BsModalProps,
} from 'react-bootstrap';
import Draggable from 'react-draggable';
import { FaExclamationCircle, FaTimesCircle, FaWindowClose } from 'react-icons/fa';
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
  handleCancel?: () => void;
  /** Optional function to control behaviour of ok button. Default is to reload the app. */
  handleOk?: () => void;
  handleOkDisabled?: boolean;
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
  /** Optional href for the OK button. Providing a href will render an <a> element, styled as a button. */
  okButtonHref?: string;
  /** Optional title to display - no default. */
  title?: string | React.ReactNode;
  /** Optional Heacer Icon to display - no default. */
  headerIcon?: string | React.ReactNode;
  /** Optional message to display - no default. */
  message?: string | React.ReactNode;
  /** optional override to control the x button in the top right of the modal. Default is to show. */
  closeButton?: boolean;
  /** provide the size of the modal, default width is 50.0rem */
  modalSize?: ModalSize;
  variant: 'info' | 'warning' | 'error';
  className?: string;
  /** display this modal as a popup instead of as a modal, allowing the user to click on underlying elements */
  asPopup?: boolean;
  show?: boolean;
  /** optional override to hide the footer of the modal modal. Default is to show. */
  hideFooter?: boolean;
  /** optional override to make the modal draggable. Defaults to false. */
  draggable?: boolean;
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
    handleOkDisabled,
    handleCancel,
    title,
    message,
    okButtonVariant,
    okButtonText,
    okButtonHref,
    cancelButtonVariant,
    cancelButtonText,
    closeButton,
    hideFooter,
    modalSize = ModalSize.MEDIUM,
    variant,
    className,
    headerIcon,
    draggable,
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

  const getHeaderIcon = () => {
    if (headerIcon) {
      return <>{headerIcon}</>;
    }

    switch (variant) {
      case 'info':
      case 'warning': {
        return <FaExclamationCircle size={22} />;
      }
      case 'error': {
        return <FaTimesCircle size={22} />;
      }
      default: {
        return null;
      }
    }
  };

  function getVariantClass() {
    switch (variant) {
      case 'info':
        return 'info-variant';
      case 'warning': {
        return 'warning-variant';
      }
      case 'error': {
        return 'error-variant';
      }
      default: {
        return 'info-variant';
      }
    }
  }

  function getModalClass() {
    if (className) {
      return className + ' ' + getVariantClass();
    }

    return getVariantClass();
  }

  const headerIconValue = getHeaderIcon();

  return (
    <ModalContainer
      {...rest}
      draggable={draggable}
      variant={variant}
      show={showState}
      modalSize={modalSize}
      onHide={noop}
      className={getModalClass()}
    >
      <Modal.Header closeButton={closeButton} onHide={close}>
        {headerIconValue && (
          <>
            <div className="header-icon">{headerIconValue}</div>
            <Spacing8 />
          </>
        )}
        <Modal.Title>{title}</Modal.Title>
        {!closeButton && (
          <div className="modal-close-btn">
            <FaWindowClose size={24} onClick={close} />
          </div>
        )}
      </Modal.Header>

      <Modal.Body style={{ whiteSpace: 'pre-line' }}>{message}</Modal.Body>

      <Spacer />
      {!hideFooter && (
        <Modal.Footer>
          <div className="button-wrap">
            {cancelButtonText && (
              <Button
                title="cancel-modal"
                variant={cancelButtonVariant ?? 'secondary'}
                onClick={close}
                data-testid="cancel-modal-button"
              >
                {cancelButtonText}
              </Button>
            )}
            <Spacing24 />
            <Button
              title="ok-modal"
              variant={okButtonVariant ?? 'primary'}
              onClick={ok}
              disabled={handleOkDisabled}
              href={okButtonHref}
              data-testid="ok-modal-button"
            >
              {okButtonText ?? 'Ok'}
            </Button>
          </div>
        </Modal.Footer>
      )}
    </ModalContainer>
  );
};

const DraggableModalDialog = (props: BsModalDialogProps) => {
  return (
    <Draggable handle=".modal-header">
      <ModalDialog {...props} />
    </Draggable>
  );
};

const ModalContainer = (props: BsModalProps & ModalProps) => {
  const { modalSize, draggable, ...rest } = props;

  return !props.asPopup ? (
    <div>
      <StyledModal
        {...rest}
        show={!!props.show}
        onHide={props.close}
        dialogClassName={classNames(modalSize, props.className)}
        dialogAs={draggable ? DraggableModalDialog : ModalDialog}
        $draggable={draggable}
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

const Spacing8 = styled.span`
  width: 0.8rem;
`;

const Spacing24 = styled.span`
  width: 2.4rem;
`;

const Spacer = styled.hr`
  padding: 0px;
  margin: 0px;
  margin-left: 1.6rem;
  margin-right: 1.6rem;
`;

const StyledModal = styled(Modal)<{ $draggable?: boolean }>`
  .modal-header {
    height: 4.8rem;
    padding-left: 1.6rem;
    padding-right: 1.6rem;

    display: flex;
    justify-content: flex-start;

    color: ${props => props.theme.css.primaryBackgroundColor};
    background-color: ${props => props.theme.css.primaryColor};

    /* show move cursor (crosshair) for draggable modals */
    cursor: ${props => (props.$draggable ? 'move' : 'default')};

    .header-icon {
    }

    .modal-title {
      font-family: BcSans-Bold;
      font-size: 2.2rem;

      align-self: center;
    }

    .modal-close-btn {
      margin-left: auto;
      cursor: pointer;
    }

    .close {
      margin-left: auto;
      color: white;
      opacity: 1;
      font-size: 3rem;
      font-weight: 10;
      text-shadow: none;
      font-family: 'Helvetica Narrow';
    }
  }

  .modal-body {
    padding-top: 2.4rem;
    padding-bottom: 2.4rem;
    padding-left: 1.6rem;
    padding-right: 1.6rem;
  }

  .modal-footer {
    border: 0px;
    padding-top: 3.6rem;
    padding-bottom: 3.6rem;
    padding-left: 1.6rem;
    padding-right: 3.6rem;

    .button-wrap {
      margin: 0px;
      padding: 0px;
      display: inline-flex;
      .Button {
        min-width: 9.5rem;
        height: 3.9rem;
      }
    }
  }

  .close {
    color: black;
  }

  .modal-xl {
    max-width: 100rem;
  }

  .modal-l {
    max-width: 75rem;
  }

  .modal-m {
    max-width: 60rem;
  }

  .modal-s {
    max-width: 40rem;
  }

  &.info-variant {
    .modal-header {
      color: ${props => props.theme.css.darkBlue};
      background-color: ${props => props.theme.css.filterBoxColor};
    }

    .modal-close-btn {
      color: ${props => props.theme.css.textColor};
      cursor: pointer;
    }
  }

  &.error-variant {
    .modal-header {
      color: ${props => props.theme.css.fontDangerColor};
      background-color: ${props => props.theme.css.dangerBackgroundColor};
    }

    .modal-close-btn {
      color: ${props => props.theme.css.textColor};
      cursor: pointer;
    }
  }

  &.warning-variant {
    .modal-header {
      color: ${props => props.theme.css.fontWarningColor};
      background-color: ${props => props.theme.css.summaryColor};
    }

    .modal-close-btn {
      color: ${props => props.theme.css.textColor};
      cursor: pointer;
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
