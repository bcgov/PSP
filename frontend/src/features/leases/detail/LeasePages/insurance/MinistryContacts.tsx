import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

export const LabelCol = styled(Col)`
  border-right: solid 1px ${props => props.theme.css.primaryColor};
  padding-bottom: 10px;
`;

const SubTitle = styled.h2`
  color: ${props => props.theme.css.primaryColor};
  border-bottom: solid 0.3rem ${props => props.theme.css.primaryColor};
  margin-bottom: 10px;
`;

const MinistryContacts: React.FunctionComponent = () => {
  const columnWidth = 7;
  return (
    <Row className="border">
      <Col>
        <SubTitle>Ministry Contacts</SubTitle>
        <Row>
          <Col>
            <Row>
              <LabelCol xs={columnWidth}>MOTI Risk Management:</LabelCol>
              <Col>Jane Smith</Col>
            </Row>
            <Row>
              <LabelCol xs={columnWidth}>Ministry of Finance::</LabelCol>
              <Col>Joan Jones</Col>
            </Row>
          </Col>
        </Row>
      </Col>
    </Row>
  );
};

export default MinistryContacts;
