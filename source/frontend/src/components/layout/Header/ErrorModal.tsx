import React from 'react';
import Col from 'react-bootstrap/Col';
import Modal from 'react-bootstrap/Modal';
import Row from 'react-bootstrap/Row';
import { useDispatch } from 'react-redux';

import { Button } from '@/components/common/buttons/Button';
import { IGenericNetworkAction } from '@/store/slices/network/interfaces';
import { logClear } from '@/store/slices/network/networkSlice';

export interface IErrorModalProps {
  // An array of network action errors.
  errors: IGenericNetworkAction[];
  // Whether to show the modal.
  show?: boolean;
  // Change the visible state.
  setShow: (show: boolean) => void;
}

/**
 * ErrorModal component that displays an error message and information in a modal.
 * @param param0 ErrorModal component properties.
 * @param param0.errors An array of errors.
 * @param [param0.show] Whether to show the model.
 * @param param0.setShow A function to set the show value.
 * @returns ErrorModal component.
 */
export const ErrorModal = ({ errors, show, setShow }: IErrorModalProps) => {
  const dispatch = useDispatch();
  const handleClose = () => setShow(false);
  const handleClear = () => {
    errors.forEach(error => dispatch(logClear(error.name)));
    setShow(false);
  };

  return (
    <Modal show={show} onHide={handleClose}>
      <Modal.Header closeButton>
        <Modal.Title>Errors</Modal.Title>
      </Modal.Header>

      <Modal.Body style={{ maxHeight: '50.0rem', overflowY: 'scroll' }}>
        {errors.map((error: IGenericNetworkAction, index: number) => (
          <Row key={index} style={{ wordBreak: 'break-all' }}>
            {process.env.NODE_ENV === 'development' ? (
              <Col>
                <abbr title={error?.error?.response?.config?.url}>
                  {error?.error?.response?.config?.url?.substr(0, 20)}
                </abbr>
                : {error?.error?.response?.statusText} data:{' '}
                {JSON.stringify(error?.error?.response?.data)}
              </Col>
            ) : (
              <Col>
                <abbr title={error?.error?.response?.config?.url}>
                  {error?.error?.response?.config?.url?.substr(0, 20)}
                </abbr>
                : ({error?.error?.response?.statusText ?? 'unknown'}){' '}
                {(error?.error?.response?.data as unknown & { error: string })?.error ?? ''}
              </Col>
            )}
          </Row>
        ))}
      </Modal.Body>

      <Modal.Footer>
        <Button variant="primary" onClick={handleClear}>
          Close & Clear Errors
        </Button>
      </Modal.Footer>
    </Modal>
  );
};

export default ErrorModal;
