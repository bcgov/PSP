import React from 'react';

import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { Api_Insurance } from '@/models/api/Insurance';
import { formatMoney, prettyFormatDate } from '@/utils';

interface PolicyProps {
  insurance: Api_Insurance;
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
    otherInsuranceType: insurance.otherInsuranceType ?? undefined,
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
