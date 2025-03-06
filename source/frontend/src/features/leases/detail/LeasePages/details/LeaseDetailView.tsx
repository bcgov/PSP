import { Col, Row } from 'react-bootstrap';

import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import TooltipIcon from '@/components/common/TooltipIcon';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { exists, prettyFormatDate } from '@/utils';

export interface ILeaseDetailViewProps {
  lease: ApiGen_Concepts_Lease;
}

/**
 * Sub-form displaying the original and renewal period information presented in styled boxes.
 * @param {ILeaseDetailViewProps} param0
 */
export const LeaseDetailView: React.FunctionComponent<
  React.PropsWithChildren<ILeaseDetailViewProps>
> = ({ lease }) => {
  const projectName = exists(lease?.project)
    ? `${lease?.project?.code} - ${lease?.project?.description}`
    : '';

  const productName = exists(lease?.product)
    ? lease?.product?.code + ' ' + lease?.product?.description
    : '';

  return (
    <Section header={'Original Agreement'}>
      <SectionField label="Ministry project" labelWidth="3">
        {projectName}
      </SectionField>
      <SectionField label="Product" labelWidth="3">
        {productName}
      </SectionField>
      <SectionField
        label="Status"
        labelWidth="3"
        contentWidth="4"
        tooltip={
          <TooltipIcon
            toolTipId="lease-status-tooltip"
            toolTip={
              <ul>
                <li>Draft: In progress but not finalized.</li>
                <li>
                  Active: Finalized and all requirements met. Lease/Licence being actively managed.
                </li>
                <li>
                  Terminated: The expiry date of the last agreement if by effluxion of time or the
                  early termination date for cause.
                </li>
                <li>Cancelled: Request cancelled by requestor or MOTI.</li>
                <li>Duplicate: Duplicate file created by accident or data transfer.</li>
                <li>Hold: Agreement in progress but will not be immediately addressed.</li>
                <li>Archived: File to be archived as per ARCS/ORCS.</li>
              </ul>
            }
            placement="right"
          ></TooltipIcon>
        }
      >
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
          <SectionField
            label="Termination"
            labelWidth="3"
            tooltip={
              <TooltipIcon
                toolTipId="lease-termination-tooltip"
                toolTip="The expiry date of the last agreement if by effluxion of time or the early termination date for cause"
                placement="right"
              />
            }
          >
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
