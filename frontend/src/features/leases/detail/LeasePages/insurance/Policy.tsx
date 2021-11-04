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

const Policy: React.FunctionComponent = () => {
  const columnWidth = 5;
  return (
    <Row className="border">
      <Col>
        <SubTitle>Policy</SubTitle>
        <Row>
          <Col>
            <Row>
              <LabelCol xs={columnWidth}>Insurance in place:</LabelCol>
              <Col>Yes</Col>
            </Row>
            <Row>
              <LabelCol xs={columnWidth}>Limit:</LabelCol>
              <Col>$ 1,224,000</Col>
            </Row>
            <Row>
              <LabelCol xs={columnWidth}>Risk Assessment completed:</LabelCol>
              <Col>May 22, 2020</Col>
            </Row>
            <Row>
              <LabelCol xs={columnWidth}>BCTFA/MOTI (Insurance Payee):</LabelCol>
              <Col>Payee name (?)</Col>
            </Row>
            <Row>
              <LabelCol xs={columnWidth}>Insured value:</LabelCol>
              <Col>Replacement Cost Value</Col>
            </Row>
            <Row>
              <LabelCol xs={columnWidth}>Policy start date:</LabelCol>
              <Col>Jun 1, 2021</Col>
            </Row>
            <Row>
              <LabelCol xs={columnWidth}>Policy expiry date:</LabelCol>
              <Col>Jun 1, 2022</Col>
            </Row>
          </Col>
          <Col>
            <h2>Description of Coverage</h2>
            <div>
              Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer nec odio. Praesent
              libero. Sed cursus ante dapibus diam. Sed nisi. Nulla quis sem at nibh elementum
              imperdiet. Duis sagittis ipsum. Praesent mauris. Fusce nec tellus sed augue semper
              porta. Mauris massa. Vestibulum lacinia arcu eget nulla. Class aptent taciti sociosqu
              ad litora torquent per conubia nostra, per inceptos himenaeos. Curabitur sodales
              ligula in libero.
            </div>
          </Col>
        </Row>
      </Col>
    </Row>
  );
};

export default Policy;
