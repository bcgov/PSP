import { IInsurance } from 'interfaces';
import { Col, Row } from 'react-bootstrap';
import { formatMoney, prettyFormatDate } from 'utils';

import { LabelCol, SubTitle } from './styles';

interface PolicyProps {
  insurance: IInsurance;
}

interface PolicyView {
  insuranceInPlace: string;
  limit: string;
  expiryDate: string;
  coverageDescription: string;
}

const Policy: React.FunctionComponent<PolicyProps> = ({ insurance }) => {
  const columnWidth = 5;
  const policy: PolicyView = {
    insuranceInPlace: insurance.isInsuranceInPlace ? 'Yes' : 'No',
    limit: formatMoney(insurance.coverageLimit),
    expiryDate: prettyFormatDate(insurance.expiryDate),
    coverageDescription: insurance.coverageDescription,
  };
  return (
    <Row className="pt-3">
      <Col>
        <SubTitle>Policy</SubTitle>
        <Row>
          <Col>
            <Row>
              <LabelCol xs={columnWidth}>Insurance in place:</LabelCol>
              <Col>{policy.insuranceInPlace}</Col>
            </Row>
            <Row>
              <LabelCol xs={columnWidth}>Limit:</LabelCol>
              <Col>{policy.limit}</Col>
            </Row>
            <Row>
              <LabelCol xs={columnWidth}>Policy expiry date:</LabelCol>
              <Col>{policy.expiryDate}</Col>
            </Row>
          </Col>
          <Col>
            <h2>Description of Coverage</h2>
            <div>{policy.coverageDescription}</div>
          </Col>
        </Row>
      </Col>
    </Row>
  );
};

export default Policy;
