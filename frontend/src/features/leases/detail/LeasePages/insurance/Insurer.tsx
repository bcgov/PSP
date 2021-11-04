import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

const LabelCol = styled(Col)`
  border-right: solid 1px ${props => props.theme.css.primaryColor};
  padding-bottom: 10px;
`;

const SubTitle = styled.h2`
  color: ${props => props.theme.css.primaryColor};
  border-bottom: solid 0.3rem ${props => props.theme.css.primaryColor};
  margin-bottom: 10px;
`;

const Insurer: React.FunctionComponent = () => {
  const columnWidth = 5;
  return (
    <Row className="border">
      <Col>
        <SubTitle>Insurer</SubTitle>
        <Row>
          <Col>
            <Row>
              <LabelCol xs={columnWidth}>Organization:</LabelCol>
              <Col>Acme Auto Insurance Group</Col>
            </Row>
            <Row>
              <LabelCol xs={columnWidth}>Contact name:</LabelCol>
              <Col>Jennifer Smith</Col>
            </Row>
            <Row>
              <LabelCol xs={columnWidth}>Phone number:</LabelCol>
              <Col>
                <div>mobile: 250-555-7777</div>
                <div>landline: 250-555-8888</div>
              </Col>
            </Row>
            <Row>
              <LabelCol xs={columnWidth}>Email:</LabelCol>
              <Col>jen.16.smith@acmeins.co</Col>
            </Row>
          </Col>
        </Row>
      </Col>
    </Row>
  );
};

export default Insurer;
