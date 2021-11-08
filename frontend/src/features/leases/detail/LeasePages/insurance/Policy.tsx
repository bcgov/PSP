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
  assessmentDate: string;
  payee: string;
  insuredValue: string;
  startDate: string;
  expiryDate: string;
  coverageDescription: string;
}

const Policy: React.FunctionComponent<PolicyProps> = ({ insurance }) => {
  const columnWidth = 5;
  const policy: PolicyView = {
    insuranceInPlace: insurance.insuranceInPlace ? 'Yes' : 'no',
    limit: formatMoney(insurance.coverageLimit),
    assessmentDate: prettyFormatDate(insurance.riskAssessmentCompletedDate) || '',
    payee: insurance.insurancePayeeType.description,
    insuredValue: formatMoney(insurance.insuredValue),
    startDate: prettyFormatDate(insurance.startDate),
    expiryDate: prettyFormatDate(insurance.expiryDate),
    coverageDescription: insurance.coverageDescription,
  };
  return (
    <Row>
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
              <LabelCol xs={columnWidth}>Risk Assessment completed:</LabelCol>
              <Col>{policy.assessmentDate}</Col>
            </Row>
            <Row>
              <LabelCol xs={columnWidth}>BCTFA/MOTI (Insurance Payee):</LabelCol>
              <Col>{policy.payee}</Col>
            </Row>
            <Row>
              <LabelCol xs={columnWidth}>Insured value:</LabelCol>
              <Col>{policy.insuredValue}</Col>
            </Row>
            <Row>
              <LabelCol xs={columnWidth}>Policy start date:</LabelCol>
              <Col>{policy.startDate}</Col>
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
