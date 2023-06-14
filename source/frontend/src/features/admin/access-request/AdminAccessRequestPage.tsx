import React from 'react';
import { Col } from 'react-bootstrap';
import Row from 'react-bootstrap/Row';
import { useHistory } from 'react-router';
import styled from 'styled-components';

import { H1 } from '@/components/common/styles';

import { AccessRequestContainer } from './AccessRequestContainer';

export interface IAdminAccessRequestPageProps {
  match?: any;
}

/**
 * The AdminAccessRequestPage provides a way to new authenticated users to submit a request
 * that associates them with a specific organization and a role within the organization.
 * If they have an active access request already submitted, it will allow them to update it until it has been approved or disabled.
 * If their prior request was disabled they will then be able to submit a new request.
 */
export const AdminAccessRequestPage: React.FunctionComponent<
  React.PropsWithChildren<IAdminAccessRequestPageProps>
> = props => {
  const history = useHistory();
  return (
    <StyledContainer>
      <Row>
        <Col md={7}>
          <H1>Edit Access Request</H1>
        </Col>
      </Row>
      <Row>
        <Col md={7}>
          <AccessRequestContainer
            accessRequestId={props?.match?.params?.id}
            onSave={() => history.push('/admin/access/requests')}
            asAdmin
          />
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

export default AdminAccessRequestPage;
