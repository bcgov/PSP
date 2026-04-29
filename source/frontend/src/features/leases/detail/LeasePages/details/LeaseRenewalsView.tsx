import { Col, Row } from 'react-bootstrap';

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
    return <Section header="Renewal Options">No Renewal Information</Section>;
  return (
    <Section header="Renewal Options">
      {renewals.map((renewal, index) => (
        <Section key={`lease-renewal-${index}`} header={`Renewal ${index + 1}`} noPadding>
          <SectionField label="Exercised?" labelWidth={{ xs: 3 }}>
            {booleanToYesNoUnknownString(renewal.isExercised)}
          </SectionField>
          <Row>
            <Col>
              <SectionField label="Commencement" labelWidth={{ xs: 6 }}>
                {prettyFormatDate(renewal.commencementDt)}
              </SectionField>
            </Col>
            <Col>
              <SectionField label="Expiry" valueClassName="d-flex align-items-top">
                {prettyFormatDate(renewal.expiryDt)}
                <ReminderContainer
                  keyDate={renewal.expiryDt}
                  keyDateLabel="Lease RenewalExpiry"
                  notificationType={ApiGen_CodeTypes_NotificationTypes.L_RENEWAL}
                  notificationSource={{ leaseId: renewal.leaseId, leaseRenewalId: renewal.id }}
                  View={ReminderView}
                />
              </SectionField>
            </Col>
          </Row>
          <SectionField label="Comments" labelWidth={{ xs: 3 }}>
            {renewal.renewalNote}
          </SectionField>
        </Section>
      ))}
    </Section>
  );
};
