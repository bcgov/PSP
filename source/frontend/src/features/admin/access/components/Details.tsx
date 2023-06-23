import React from 'react';
import Col from 'react-bootstrap/Col';
import Container from 'react-bootstrap/Container';
import Form from 'react-bootstrap/Form';
import Modal from 'react-bootstrap/Modal';
import Row from 'react-bootstrap/Row';

import { Button } from '@/components/common/buttons/Button';

import { IAccessRequestModel } from '../interfaces';

interface IProps {
  request: IAccessRequestModel;
  onClose: () => void;
}
export const AccessRequestDetails: React.FC<React.PropsWithChildren<IProps>> = ({
  request,
  onClose,
}) => {
  return (
    <Container>
      <Modal show={!!request} onHide={onClose}>
        <Modal.Header>
          <Modal.Title>Access Request Details</Modal.Title>
        </Modal.Header>

        <Modal.Body style={{ maxHeight: '50.0rem' }}>
          <Form>
            <Form.Group as={Row} controlId="businessIdentifier">
              <Form.Label column sm="4">
                IDIR/BCeID:
              </Form.Label>
              <Col sm="8">
                <Form.Control disabled defaultValue={request.businessIdentifierValue} />
              </Col>
            </Form.Group>
            <Form.Group as={Row} controlId="emailAddress">
              <Form.Label column sm="4">
                Email:
              </Form.Label>
              <Col sm="8">
                <Form.Control disabled defaultValue={request.email} />
              </Col>
            </Form.Group>
            <Form.Group as={Row} controlId="firstName">
              <Form.Label column sm="4">
                First name:
              </Form.Label>
              <Col sm="8">
                <Form.Control disabled defaultValue={request.firstName} />
              </Col>
            </Form.Group>
            <Form.Group as={Row} controlId="surname">
              <Form.Label column sm="4">
                Last name:
              </Form.Label>
              <Col sm="8">
                <Form.Control disabled defaultValue={request.surname} />
              </Col>
            </Form.Group>
            <Form.Group as={Row} controlId="position">
              <Form.Label column sm="4">
                Position:
              </Form.Label>
              <Col sm="8">
                <Form.Control disabled defaultValue={request.position} />
              </Col>
            </Form.Group>
            <Form.Group as={Row} controlId="role">
              <Form.Label column sm="4">
                Role:
              </Form.Label>
              <Col sm="8">
                <Form.Control disabled defaultValue={request.role} />
              </Col>
            </Form.Group>
            <Form.Group as={Row} controlId="note">
              <Form.Label column sm="4">
                Note:
              </Form.Label>
              <Col sm="8">
                <Form.Control as="textarea" disabled defaultValue={request.note} />
              </Col>
            </Form.Group>
          </Form>
        </Modal.Body>

        <Modal.Footer>
          <Button variant="primary" onClick={onClose}>
            Close
          </Button>
        </Modal.Footer>
      </Modal>
    </Container>
  );
};
