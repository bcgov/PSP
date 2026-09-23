import React from 'react';
import styled from 'styled-components';

import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import ReminderContainer from '@/features/notifications/ReminderContainer';
import ReminderView from '@/features/notifications/ReminderView';
import { ApiGen_CodeTypes_NotificationTypes } from '@/models/api/generated/ApiGen_CodeTypes_NotificationTypes';
import { ApiGen_Concepts_Insurance } from '@/models/api/generated/ApiGen_Concepts_Insurance';
import { exists, formatMoney, prettyFormatDate } from '@/utils';

interface PolicyProps {
  insurance: ApiGen_Concepts_Insurance;
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
  const policy: PolicyView = {
    insuranceInPlace:
      insurance.isInsuranceInPlace == null
        ? 'Unknown'
        : insurance.isInsuranceInPlace
        ? 'Yes'
        : 'No',
    limit: insurance.coverageLimit ? formatMoney(insurance.coverageLimit) : '',
    expiryDate: prettyFormatDate(insurance.expiryDate),
    coverageDescription: insurance.coverageDescription || '',
    otherInsuranceType: insurance.otherInsuranceType ?? undefined,
    insuranceType: insurance.insuranceType?.description ?? undefined,
  };

  return (
    <Section header={policy.insuranceType}>
      {exists(insurance.insuranceType?.id) && insurance.insuranceType?.id === 'OTHER' && (
        <SectionField label="Other insurance type" labelWidth={{ xs: 4 }}>
          {policy.otherInsuranceType}
        </SectionField>
      )}
      <SectionField label="Insurance in place" labelWidth={{ xs: 4 }}>
        {policy.insuranceInPlace}
      </SectionField>
      <SectionField label="Limit" labelWidth={{ xs: 4 }}>
        {policy.limit}
      </SectionField>
      <SectionField label="Policy expiry date" labelWidth={{ xs: 4 }}>
        <StyledReminderContent>
          {policy.expiryDate}
          {policy.expiryDate && (
            <ReminderContainer
              keyDate={policy.expiryDate}
              keyDateLabel="Lease Policy Expiry"
              notificationType={ApiGen_CodeTypes_NotificationTypes.L_INSURANCE}
              notificationSource={{ leaseId: insurance.leaseId, insuranceId: insurance.id }}
              View={ReminderView}
            />
          )}
        </StyledReminderContent>
      </SectionField>
      <SectionField label="Description of coverage" labelWidth={{ xs: 4 }}>
        {policy.coverageDescription}
      </SectionField>
    </Section>
  );
};

export default Policy;

const StyledReminderContent = styled.div`
  display: flex;
  flex-direction: row;
  align-items: top;
  gap: 1.2rem;
`;
