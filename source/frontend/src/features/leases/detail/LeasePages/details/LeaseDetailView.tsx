import { useEffect } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaExternalLinkAlt } from 'react-icons/fa';

import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledLink } from '@/components/common/styles';
import TooltipIcon from '@/components/common/TooltipIcon';
import ReminderContainer from '@/features/notifications/ReminderContainer';
import ReminderView from '@/features/notifications/ReminderView';
import { useNotificationRepository } from '@/hooks/repositories/useNotificationRepository';
import { ApiGen_CodeTypes_LeasePaymentReceivableTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeasePaymentReceivableTypes';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';
import { ApiGen_CodeTypes_NotificationTypes } from '@/models/api/generated/ApiGen_CodeTypes_NotificationTypes';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { exists, firstOrNull, prettyFormatDate } from '@/utils';
import { formatMinistryProject } from '@/utils/formUtils';
import { formatApiPersonNames } from '@/utils/personUtils';

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
  const leaseId = lease?.id;
  const projectName = exists(lease?.project)
    ? formatMinistryProject(lease?.project?.code, lease?.project?.description)
    : '';
  const productName = exists(lease?.product)
    ? lease?.product?.code + ' ' + lease?.product?.description
    : '';

  const {
    searchNotifications: { execute: searchNotifications, response: expiryNotifications },
  } = useNotificationRepository();

  function handleSaveLeaseExpiryReminder(isoDate: string): void {
    alert('Lease expiry reminder set for ' + prettyFormatDate(isoDate));
  }

  function handleRemoveLeaseExpiryReminder(): void {
    alert('Reminder removed.');
  }

  // TODO: Replace alert() calls with real API integration to save/remove reminders for the lease expiry date.
  // TODO: Load the existing reminder for the lease expiry date (if any for current user) and pass it as the `savedReminderDate` prop to the ReminderButton component.
  useEffect(() => {
    async function fetchExistingExpiryNotification() {
      try {
        await searchNotifications({
          type: ApiGen_CodeTypes_NotificationTypes.L_RENEWAL,
          leaseId: leaseId,
        });
      } catch (error) {
        // swallow error and assume no existing reminder
      }
    }
    fetchExistingExpiryNotification();
  }, [leaseId, searchNotifications]);

  return (
    <>
      <Section header="Original Agreement">
        <SectionField label="Ministry project" labelWidth={{ xs: 3 }}>
          {projectName}
        </SectionField>
        <SectionField label="Product" labelWidth={{ xs: 3 }}>
          {productName}
        </SectionField>
        <SectionField
          label="Status"
          labelWidth={{ xs: 3 }}
          contentWidth={{ xs: 4 }}
          tooltip={
            <TooltipIcon
              toolTipId="lease-status-tooltip"
              toolTip={
                <ul>
                  <li>Draft: In progress but not finalized.</li>
                  <li>
                    Active: Finalized and all requirements met. Lease/Licence being actively
                    managed.
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
          <SectionField label="Cancellation reason" contentWidth={{ xs: 12 }} required>
            {lease.cancellationReason}
          </SectionField>
        )}

        <SectionField label="Account type" labelWidth={{ xs: 3 }} contentWidth={{ xs: 5 }}>
          {lease.paymentReceivableType.description}
        </SectionField>
        <Row>
          <Col>
            <SectionField label="Commencement" labelWidth={{ xs: 6 }}>
              {prettyFormatDate(lease.startDate)}
            </SectionField>
          </Col>
          <Col>
            <SectionField label="Expiry">
              {prettyFormatDate(lease.expiryDate)}{' '}
              <ReminderContainer
                keyDate={lease.expiryDate}
                keyDateLabel="Lease Expiry"
                notificationType={ApiGen_CodeTypes_NotificationTypes.L_RENEWAL}
                notification={firstOrNull(expiryNotifications)}
                onReminderSaved={}
                onReminderRemoved={}
                View={ReminderView}
              />
            </SectionField>
          </Col>
        </Row>
        {lease.fileStatusTypeCode.id === ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED && (
          <>
            <SectionField
              label="Termination"
              labelWidth={{ xs: 3 }}
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
            <SectionField label="Termination reason" labelWidth={{ xs: 3 }}>
              {lease.terminationReason}
            </SectionField>
          </>
        )}
        {lease?.project?.projectPersons?.map((teamMember, index) => (
          <SectionField label="Project team member" key={`project-team-${index}`}>
            <StyledLink
              target="_blank"
              rel="noopener noreferrer"
              to={`/contact/P${teamMember?.personId}`}
            >
              <span>{formatApiPersonNames(teamMember?.person)}</span>
              <FaExternalLinkAlt className="ml-2" size="1rem" />
            </StyledLink>
          </SectionField>
        ))}
      </Section>

      {lease?.paymentReceivableType?.id !== ApiGen_CodeTypes_LeasePaymentReceivableTypes.RCVBL && (
        <Section header="Progress Statuses">
          <SectionField label="Appraisal">
            {lease.fileAppraisalStatusTypeCode?.description ?? ''}
          </SectionField>
          <SectionField label="Legal survey">
            {lease.fileLegalSurveyStatusTypeCode?.description ?? ''}
          </SectionField>
        </Section>
      )}
    </>
  );
};

export default LeaseDetailView;
