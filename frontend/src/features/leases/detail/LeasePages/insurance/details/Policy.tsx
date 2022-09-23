import { IInsurance } from 'interfaces';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { formatMoney, prettyFormatDate } from 'utils';

import { BoldHeader, BoldValueText, LabelCol, SubTitle } from './styles';

interface PolicyProps {
  insurance: IInsurance;
}

interface PolicyView {
  insuranceInPlace: string;
  limit: string;
  expiryDate: string;
  coverageDescription: string;
  otherInsuranceType?: string;
  insuranceType?: string;
}

const Policy: React.FunctionComponent<PolicyProps> = ({ insurance }) => {
  const columnWidth = 5;
  const policy: PolicyView = {
    insuranceInPlace: insurance.isInsuranceInPlace ? 'Yes' : 'No',
    limit: insurance.coverageLimit ? formatMoney(insurance.coverageLimit) : '',
    expiryDate: prettyFormatDate(insurance.expiryDate),
    coverageDescription: insurance.coverageDescription || '',
    otherInsuranceType: insurance.otherInsuranceType,
    insuranceType: insurance.insuranceType.description,
  };
  return (
    <Row>
      <Col>
        <SubTitle data-testid="insurance-title">
          {policy.insuranceType}
          {policy.otherInsuranceType && <span>: {policy.otherInsuranceType}</span>}
        </SubTitle>

        <Row>
          <Col>
            <Row>
              <LabelCol xs={columnWidth}>Insurance in place:</LabelCol>
              <BoldValueText>{policy.insuranceInPlace}</BoldValueText>
            </Row>
            <Row>
              <LabelCol xs={columnWidth}>Limit:</LabelCol>
              <BoldValueText>{policy.limit}</BoldValueText>
            </Row>
            <Row>
              <LabelCol xs={columnWidth}>Policy expiry date:</LabelCol>
              <BoldValueText>{policy.expiryDate}</BoldValueText>
            </Row>
          </Col>
          <Col>
            <BoldHeader>Description of Coverage</BoldHeader>
            <div>{policy.coverageDescription}</div>
          </Col>
        </Row>
      </Col>
    </Row>
  );
};

export default Policy;
