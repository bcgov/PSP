import { FormSection } from 'components/common/form/styles';
import { IInsurance } from 'interfaces';
import React from 'react';
import { useMemo } from 'react';
import { Col, Row } from 'react-bootstrap';
import { ILookupCode } from 'store/slices/lookupCodes';

import Policy from './Policy';
import { InsuranceTypeList, SectionHeader } from './styles';

export interface InsuranceDetailsViewProps {
  insuranceList: IInsurance[];
  insuranceTypes: ILookupCode[];
}

const InsuranceDetailsView: React.FunctionComponent<InsuranceDetailsViewProps> = ({
  insuranceList,
  insuranceTypes,
}) => {
  const sortedInsuranceList = useMemo(
    () =>
      !!insuranceList?.length
        ? insuranceList.sort((a, b) => {
            return (
              insuranceTypes.findIndex(i => i.id === a.insuranceType.displayOrder) -
              insuranceTypes.findIndex(i => i.id === b.insuranceType.displayOrder)
            );
          })
        : [],
    [insuranceList, insuranceTypes],
  );
  return !!sortedInsuranceList.length ? (
    <>
      <SectionHeader>Required insurance</SectionHeader>
      <InsuranceTypeList>
        {sortedInsuranceList.map((insurance: IInsurance, index: number) => (
          <li key={index + insurance.id}>
            {insurance.insuranceType.description}
            {insurance.insuranceType.id === 'OTHER' && insurance.otherInsuranceType
              ? `: ${insurance.otherInsuranceType}`
              : ''}
          </li>
        ))}
      </InsuranceTypeList>
      <SectionHeader>Policy information</SectionHeader>
      {sortedInsuranceList.map((insurance: IInsurance, index: number) => (
        <div key={index + insurance.id}>
          <FormSection>
            <Row>
              <Col>
                <Policy insurance={insurance} />
              </Col>
            </Row>
          </FormSection>
          <br />
        </div>
      ))}
    </>
  ) : (
    <p>There are no insurance policies indicated with this lease/license</p>
  );
};

export default InsuranceDetailsView;
