import { Col, Row } from 'react-bootstrap';

import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
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
        <Section key={`lease-renewal-{index}`} header={`Renewal ${index + 1}`} noPadding>
          <SectionField label="Exercised?" labelWidth="3">
            {booleanToYesNoUnknownString(renewal.isExercised)}
          </SectionField>
          <Row>
            <Col>
              <SectionField label="Commencement" labelWidth="6">
                {prettyFormatDate(renewal.commencementDt)}
              </SectionField>
            </Col>
            <Col>
              <SectionField label="Expiry">{prettyFormatDate(renewal.expiryDt)}</SectionField>
            </Col>
          </Row>
          <SectionField label="Notes" labelWidth="3">
            {renewal.renewalNote}
          </SectionField>
        </Section>
      ))}
    </Section>
  );
};
