import React, { useEffect } from 'react';
import { FaExternalLinkAlt } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import styled from 'styled-components';

import EditButton from '@/components/common/EditButton';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledEditWrapper, StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { StyledSectionParagraph } from '@/components/common/styles';
import TooltipIcon from '@/components/common/TooltipIcon';
import { Claims, Roles } from '@/constants';
import { InterestHolderType } from '@/constants/interestHolderTypes';
import { usePersonRepository } from '@/features/contacts/repositories/usePersonRepository';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { exists, prettyFormatDate } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

import { cannotEditMessage } from '../../../common/constants';
import AcquisitionFileStatusUpdateSolver from './AcquisitionFileStatusUpdateSolver';
import AcquisitionOwnersSummaryContainer from './AcquisitionOwnersSummaryContainer';
import AcquisitionOwnersSummaryView from './AcquisitionOwnersSummaryView';
import { DetailAcquisitionFile } from './models';

export interface IAcquisitionSummaryViewProps {
  acquisitionFile?: ApiGen_Concepts_AcquisitionFile;
  onEdit: () => void;
}

const AcquisitionSummaryView: React.FC<IAcquisitionSummaryViewProps> = ({
  acquisitionFile,
  onEdit,
}) => {
  const detail: DetailAcquisitionFile = DetailAcquisitionFile.fromApi(acquisitionFile);
  const { hasRole, hasClaim } = useKeycloakWrapper();

  const projectName = exists(acquisitionFile?.project)
    ? acquisitionFile?.project?.code + ' - ' + acquisitionFile?.project?.description
    : '';

  const productName = exists(acquisitionFile?.product)
    ? acquisitionFile?.product?.code + ' ' + acquisitionFile?.product?.description
    : '';

  const ownerSolicitor = acquisitionFile?.acquisitionFileInterestHolders?.find(
    x => x.interestHolderType?.id === InterestHolderType.OWNER_SOLICITOR,
  );

  const {
    getPersonDetail: { execute: fetchPerson, response: ownerSolicitorPrimaryContact },
  } = usePersonRepository();

  useEffect(() => {
    if (ownerSolicitor?.primaryContactId) {
      fetchPerson(ownerSolicitor?.primaryContactId);
    }
  }, [ownerSolicitor?.primaryContactId, fetchPerson]);

  const ownerRepresentative = acquisitionFile?.acquisitionFileInterestHolders?.find(
    x => x.interestHolderType?.id === InterestHolderType.OWNER_REPRESENTATIVE,
  );

  const statusSolver = new AcquisitionFileStatusUpdateSolver(acquisitionFile.fileStatusTypeCode);

  const canEditDetails = () => {
    if (hasRole(Roles.SYSTEM_ADMINISTRATOR) || statusSolver.canEditDetails()) {
      return true;
    }
    return false;
  };

  return (
    <StyledSummarySection>
      <StyledEditWrapper className="mr-3 my-1">
        {hasClaim(Claims.ACQUISITION_EDIT) && canEditDetails() ? (
          <EditButton title="Edit acquisition file" onClick={onEdit} />
        ) : null}
        {!canEditDetails() && (
          <TooltipIcon
            toolTipId={`${acquisitionFile?.id || 0}-summary-cannot-edit-tooltip`}
            toolTip={cannotEditMessage}
          />
        )}
      </StyledEditWrapper>

      <Section header="Project">
        <SectionField label="Ministry project">{projectName}</SectionField>
        <SectionField label="Product">{productName}</SectionField>
        <SectionField label="Funding">{acquisitionFile?.fundingTypeCode?.description}</SectionField>
        {acquisitionFile?.fundingTypeCode?.id === 'OTHER' && (
          <SectionField label="Other funding">{acquisitionFile.fundingOther}</SectionField>
        )}
      </Section>
      <Section header="Schedule">
        <SectionField label="Assigned date">{prettyFormatDate(detail.assignedDate)}</SectionField>
        <SectionField
          label="Delivery date"
          tooltip="Date for delivery of the property to the project"
        >
          {prettyFormatDate(detail.deliveryDate)}
        </SectionField>
        <SectionField
          label="Estimated date"
          tooltip="Estimated date by which the acquisition would be completed"
        >
          {prettyFormatDate(detail.estimatedCompletionDate)}
        </SectionField>
        <SectionField label="Possession date">
          {prettyFormatDate(detail.possessionDate)}
        </SectionField>
      </Section>
      <Section header="Acquisition Details">
        <SectionField label="Acquisition file name">{detail.fileName}</SectionField>
        <SectionField
          label="Historical file number"
          tooltip="Older file that this file represents (ex: those from the legacy system or other non-digital files.)"
        >
          {detail.legacyFileNumber}
        </SectionField>
        <SectionField label="Physical file status">
          {detail.acquisitionPhysFileStatusTypeDescription}
        </SectionField>
        <SectionField label="Acquisition type">{detail.acquisitionTypeDescription}</SectionField>
        {detail.isSubFile && (
          <SectionField label="Sub-file interest" valueTestId="subFile-interest-type">
            {detail.subfileInterestTypeDescription}
          </SectionField>
        )}
        <SectionField label="Ministry region">{detail.regionDescription}</SectionField>
      </Section>
      <Section header="Acquisition Team">
        {detail.acquisitionTeam.map((teamMember, index) => (
          <React.Fragment key={`acq-team-${index}`}>
            <SectionField label={teamMember?.teamProfileTypeCodeDescription || ''}>
              <StyledLink
                target="_blank"
                rel="noopener noreferrer"
                to={
                  teamMember?.personId
                    ? `/contact/P${teamMember?.personId}`
                    : `/contact/O${teamMember?.organizationId}`
                }
              >
                <span>{teamMember?.teamName}</span>
                <FaExternalLinkAlt className="ml-2" size="1rem" />
              </StyledLink>
            </SectionField>
            {teamMember?.organizationId && (
              <SectionField label="Primary contact">
                {teamMember?.primaryContactId ? (
                  <StyledLink
                    target="_blank"
                    rel="noopener noreferrer"
                    to={`/contact/P${teamMember?.primaryContactId}`}
                  >
                    <span>{teamMember?.primaryContactName}</span>
                    <FaExternalLinkAlt className="m1-2" size="1rem" />
                  </StyledLink>
                ) : (
                  'No contacts available'
                )}
              </SectionField>
            )}
          </React.Fragment>
        ))}
      </Section>
      <Section header={detail.isSubFile ? 'Sub-Interest Information' : 'Owner Information'}>
        {detail.isSubFile ? (
          <StyledSectionParagraph>
            Each property in this sub-file should be impacted by the sub-interest(s) in this section
          </StyledSectionParagraph>
        ) : (
          <StyledSectionParagraph>
            Each property in this file should be owned by the owner(s) in this section
          </StyledSectionParagraph>
        )}
        {acquisitionFile?.id !== undefined && (
          <AcquisitionOwnersSummaryContainer
            acquisitionFileId={acquisitionFile.id}
            View={AcquisitionOwnersSummaryView}
          ></AcquisitionOwnersSummaryContainer>
        )}
        {!!ownerSolicitor && (
          <SectionField label={detail.isSubFile ? 'Sub-interest solicitor' : 'Owner solicitor'}>
            <StyledLink
              target="_blank"
              rel="noopener noreferrer"
              to={
                ownerSolicitor?.personId
                  ? `/contact/P${ownerSolicitor?.personId}`
                  : `/contact/O${ownerSolicitor?.organizationId}`
              }
            >
              <span>
                {ownerSolicitor?.personId
                  ? formatApiPersonNames(ownerSolicitor?.person)
                  : ownerSolicitor?.organization?.name ?? ''}
              </span>
              <FaExternalLinkAlt className="ml-2" size="1rem" />
            </StyledLink>
          </SectionField>
        )}
        {ownerSolicitor?.organization && (
          <SectionField label="Primary contact">
            {ownerSolicitor?.primaryContactId ? (
              <StyledLink
                target="_blank"
                rel="noopener noreferrer"
                to={`/contact/P${ownerSolicitor?.primaryContactId}`}
              >
                <span>{formatApiPersonNames(ownerSolicitorPrimaryContact)}</span>
                <FaExternalLinkAlt className="m1-2" size="1rem" />
              </StyledLink>
            ) : (
              'No contacts available'
            )}
          </SectionField>
        )}
        {!!ownerRepresentative && (
          <>
            <SectionField
              label={detail.isSubFile ? 'Sub-interest representative' : 'Owner representative'}
            >
              <StyledLink
                target="_blank"
                rel="noopener noreferrer"
                to={`/contact/P${ownerRepresentative?.personId}`}
              >
                <span>{formatApiPersonNames(ownerRepresentative?.person ?? undefined)}</span>
                <FaExternalLinkAlt className="ml-2" size="1rem" />
              </StyledLink>
            </SectionField>
            <SectionField label="Comment">{ownerRepresentative?.comment}</SectionField>
          </>
        )}
      </Section>
    </StyledSummarySection>
  );
};

export default AcquisitionSummaryView;

const StyledLink = styled(Link)`
  display: flex;
  align-items: center;
`;
