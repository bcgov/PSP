import React from 'react';
import { Alert } from 'react-bootstrap';
import { Col } from 'react-bootstrap';
import Row from 'react-bootstrap/Row';
import styled from 'styled-components';

import { H1 } from '@/components/common/styles';

import { AccessRequestContainer } from './AccessRequestContainer';

export interface IAccessRequestPageProps {
  match?: any;
}

/**
 * The AccessRequestPage provides a way to new authenticated users to submit a request
 * that associates them with a specific organization and a role within the organization.
 * If they have an active access request already submitted, it will allow them to update it until it has been approved or disabled.
 * If their prior request was disabled they will then be able to submit a new request.
 */
export const AccessRequestPage: React.FunctionComponent<
  React.PropsWithChildren<IAccessRequestPageProps>
> = props => {
  return (
    <StyledContainer>
      <Row>
        <Col md={7}>
          <H1>Request Access to PIMS</H1>
        </Col>
      </Row>
      <Row>
        <Col md={7}>
          <Alert variant="info">You will receive an email when your request is reviewed</Alert>
        </Col>
      </Row>
      <Row>
        <Col md={7}>
          <AccessRequestContainer accessRequestId={props?.match?.params?.id} />
        </Col>
      </Row>
    </StyledContainer>
  );
};

const StyledContainer = styled.div`
  width: 100%;
  overflow-y: auto;
  padding: 3rem;
  > .row {
    justify-content: center;
  }
  form {
    text-align: left;
  }
`;

export default AccessRequestPage;
