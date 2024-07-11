import { Col, Row } from 'react-bootstrap';

import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { prettyFormatDate } from '@/utils';

export interface ILeaseDetailView {
  lease: ApiGen_Concepts_Lease;
}

/**
 * Sub-form displaying the original and renewal period information presented in styled boxes.
 * @param {ILeaseDetailView} param0
 */
export const LeaseDetailView: React.FunctionComponent<
  React.PropsWithChildren<ILeaseDetailView>
> = ({ lease }) => {
  const projectName = lease.project ? `${lease.project.code} - ${lease.project.description}` : '';

  return (
    <Section header="Details">
      <SectionField label="Ministry project">{projectName}</SectionField>
      <SectionField label="Status" labelWidth="3" contentWidth="4">
        {lease.fileStatusTypeCode.description}
      </SectionField>

      {lease.fileStatusTypeCode.id === ApiGen_CodeTypes_LeaseStatusTypes.DISCARD && (
        <SectionField label="Cancellation reason" contentWidth="12" required>
          {lease.cancellationReason}
        </SectionField>
      )}

      <SectionField label="Account type" labelWidth="3" contentWidth="5">
        {lease.paymentReceivableType.description}
      </SectionField>
      <Row>
        <Col>
          <SectionField label="Commencement" labelWidth="6">
            {prettyFormatDate(lease.startDate)}
          </SectionField>
        </Col>
        <Col>
          <SectionField label="Expiry">{prettyFormatDate(lease.expiryDate)}</SectionField>
        </Col>
      </Row>
      {lease.fileStatusTypeCode.id === ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED && (
        <>
          <SectionField label="Termination" labelWidth="3">
            {prettyFormatDate(lease.terminationDate)}
          </SectionField>
          <SectionField label="Termination reason" labelWidth="3">
            {lease.terminationReason}
          </SectionField>
        </>
      )}
    </Section>
  );
};

export default LeaseDetailView;
