import React, { useEffect } from 'react';
import { FaExternalLinkAlt } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import styled from 'styled-components';

import EditButton from '@/components/common/EditButton';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledEditWrapper, StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { Claims } from '@/constants';
import { InterestHolderType } from '@/constants/interestHolderTypes';
import { usePersonRepository } from '@/features/contacts/repositories/usePersonRepository';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';
import { prettyFormatDate } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

import AcquisitionOwnersSummaryContainer from './AcquisitionOwnersSummaryContainer';
import AcquisitionOwnersSummaryView from './AcquisitionOwnersSummaryView';
import { DetailAcquisitionFile } from './models';

export interface IAcquisitionSummaryViewProps {
  acquisitionFile?: Api_AcquisitionFile;
  onEdit: () => void;
}

const AcquisitionSummaryView: React.FC<IAcquisitionSummaryViewProps> = ({
  acquisitionFile,
  onEdit,
}) => {
  const keycloak = useKeycloakWrapper();
  const detail: DetailAcquisitionFile = DetailAcquisitionFile.fromApi(acquisitionFile);

  const projectName =
    acquisitionFile?.project !== undefined
      ? acquisitionFile?.project?.code + ' - ' + acquisitionFile?.project?.description
      : '';

  const productName =
    acquisitionFile?.product !== undefined
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

  return (
    <StyledSummarySection>
      <StyledEditWrapper className="mr-3 my-1">
        {keycloak.hasClaim(Claims.ACQUISITION_EDIT) && acquisitionFile !== undefined ? (
          <EditButton title="Edit acquisition file" onClick={onEdit} />
        ) : null}
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
          label="Acquisition completed date"
          tooltip={`This will be enabled when the file status is set to "Completed"`}
        >
          {prettyFormatDate(detail.completionDate)}
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
        <SectionField label="Ministry region">{detail.regionDescription}</SectionField>
      </Section>
      <Section header="Acquisition Team">
        {detail.acquisitionTeam.map((person, index) => (
          <SectionField
            key={`acq-team-${index}`}
            label={person.personProfileTypeCodeDescription || ''}
          >
            <StyledLink
              target="_blank"
              rel="noopener noreferrer"
              to={`/contact/P${person.personId}`}
            >
              <span>{person.personName}</span>
              <FaExternalLinkAlt className="ml-2" size="1rem" />
            </StyledLink>
          </SectionField>
        ))}
      </Section>
      <Section header="Owner Information">
        {acquisitionFile !== undefined && (
          <AcquisitionOwnersSummaryContainer
            acquisitionFileId={acquisitionFile.id!}
            View={AcquisitionOwnersSummaryView}
          ></AcquisitionOwnersSummaryContainer>
        )}
        {!!ownerSolicitor && (
          <SectionField label="Owner solicitor">
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
          <SectionField label="Primary Contact">
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
            <SectionField label="Owner representative">
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
