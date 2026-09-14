import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import ReminderContainer from '@/features/notifications/ReminderContainer';
import { ReminderView } from '@/features/notifications/ReminderView';
import { ApiGen_CodeTypes_NotificationTypes } from '@/models/api/generated/ApiGen_CodeTypes_NotificationTypes';
import { ApiGen_Concepts_LeaseRenewal } from '@/models/api/generated/ApiGen_Concepts_LeaseRenewal';
import { prettyFormatDate } from '@/utils';
import { booleanToYesNoUnknownString } from '@/utils/formUtils';

export interface ILeaseRenewalsViewProps {
  renewals: ApiGen_Concepts_LeaseRenewal[];
}

export const LeaseRenewalsView: React.FunctionComponent<ILeaseRenewalsViewProps> = ({
  renewals,
}) => {
  if (renewals.length === 0)
    return (
      <Section header="Renewal Options">
        <div data-testid="empty-renewals">No Renewal Information</div>
      </Section>
    );
  return (
    <Section header="Renewal Options">
      {renewals.map((renewal, index) => (
        <Section
          key={`lease-renewal-${index}`}
          header={`Renewal ${index + 1}`}
          noPadding
          data-testid={`renewal[${index}].header`}
        >
          <SectionField
            label="Exercised?"
            labelWidth={{ xs: 3 }}
            valueTestId={`renewal[${index}].exercised`}
          >
            {booleanToYesNoUnknownString(renewal.isExercised)}
          </SectionField>
          <Row>
            <Col>
              <SectionField
                label="Commencement"
                labelWidth={{ xs: 6 }}
                valueTestId={`renewal[${index}].commencementDt`}
              >
                {prettyFormatDate(renewal.commencementDt)}
              </SectionField>
            </Col>
            <Col>
              <SectionField label="Expiry">
                <StyledReminderContent>
                  <div data-testid={`renewal[${index}].expiryDt`}>
                    {prettyFormatDate(renewal.expiryDt)}
                  </div>
                  <ReminderContainer
                    keyDate={renewal.expiryDt}
                    keyDateLabel="Lease RenewalExpiry"
                    notificationType={ApiGen_CodeTypes_NotificationTypes.L_RENEWAL}
                    notificationSource={{ leaseId: renewal.leaseId, leaseRenewalId: renewal.id }}
                    View={ReminderView}
                  />
                </StyledReminderContent>
              </SectionField>
            </Col>
          </Row>
          <SectionField
            label="Comments"
            labelWidth={{ xs: 3 }}
            valueTestId={`renewal[${index}].renewalNote`}
          >
            {renewal.renewalNote}
          </SectionField>
        </Section>
      ))}
    </Section>
  );
};

const StyledReminderContent = styled.div`
  display: flex;
  flex-direction: row;
  align-items: top;
  gap: 1.2rem;
`;
