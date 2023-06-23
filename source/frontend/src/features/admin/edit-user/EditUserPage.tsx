import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { H1 } from '@/components/common/styles';

import EditUserContainer from './EditUserContainer';

interface IEditUserPageProps {
  userKey: string;
  match?: any;
}

const EditUserPage = (props: IEditUserPageProps) => {
  const userId = props?.match?.params?.key || props.userKey;

  return (
    <StyledContainer className="EditUserPage">
      <Row>
        <Col md={7}>
          <H1>User Information</H1>
        </Col>
      </Row>
      <Row>
        <Col md={7}>
          <EditUserContainer userId={userId} />
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

export default EditUserPage;
