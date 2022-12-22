import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { IInsurance } from 'interfaces';
import React from 'react';
import { formatMoney, prettyFormatDate } from 'utils';

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

const Policy: React.FunctionComponent<React.PropsWithChildren<PolicyProps>> = ({ insurance }) => {
  const policy: PolicyView = {
    insuranceInPlace: insurance.isInsuranceInPlace ? 'Yes' : 'No',
    limit: insurance.coverageLimit ? formatMoney(insurance.coverageLimit) : '',
    expiryDate: prettyFormatDate(insurance.expiryDate),
    coverageDescription: insurance.coverageDescription || '',
    otherInsuranceType: insurance.otherInsuranceType,
    insuranceType: insurance.insuranceType.description,
  };
  return (
    <Section header={policy.insuranceType}>
      <SectionField label="Insurance in place" labelWidth="3">
        {policy.insuranceInPlace}
      </SectionField>
      <SectionField label="Limit" labelWidth="3">
        {policy.limit}
      </SectionField>
      <SectionField label="Policy expiry date" labelWidth="3">
        {policy.expiryDate}
      </SectionField>
      <SectionField label="Description of Coverage" labelWidth="3">
        {policy.coverageDescription}
      </SectionField>
    </Section>
  );
};

export default Policy;
