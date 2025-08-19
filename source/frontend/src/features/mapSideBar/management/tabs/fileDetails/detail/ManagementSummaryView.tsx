import { Fragment } from 'react';
import { FaExternalLinkAlt, FaUserPlus } from 'react-icons/fa';

import EditButton from '@/components/common/buttons/EditButton';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledEditWrapper, StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import { StyledLink } from '@/components/common/styles';
import TooltipIcon from '@/components/common/TooltipIcon';
import { Claims, Roles } from '@/constants';
import { cannotEditMessage } from '@/features/mapSideBar/acquisition/common/constants';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { ApiGen_Concepts_ManagementFileContact } from '@/models/api/generated/ApiGen_Concepts_ManagementFileContact';
import { isValidId } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

import ManagementFileContactsList from './contacts/ManagementFileContactsList';
import ManagementStatusUpdateSolver from './ManagementStatusUpdateSolver';

export interface IManagementSummaryViewProps {
  managementFile: ApiGen_Concepts_ManagementFile;
  managementFileContacts: ApiGen_Concepts_ManagementFileContact[];
  fileStatusSolver: ManagementStatusUpdateSolver;
  isLoading: boolean;
  onFileEdit: () => void;
  onAddContact: () => void;
  onEditContact: (contactId: number) => void;
  onDeleteContact: (contactId: number) => void;
}

export const ManagementSummaryView: React.FunctionComponent<IManagementSummaryViewProps> = ({
  managementFile,
  managementFileContacts,
  fileStatusSolver,
  isLoading,
  onFileEdit,
  onAddContact,
  onEditContact,
  onDeleteContact,
}) => {
  const keycloak = useKeycloakWrapper();

  const canEditDetails = () => {
    if (keycloak.hasRole(Roles.SYSTEM_ADMINISTRATOR) || !fileStatusSolver.isAdminProtected()) {
      return true;
    }
    return false;
  };

  const projectName = managementFile.project
    ? managementFile.project?.code + ' - ' + managementFile.project?.description
    : '';

  const productName = managementFile.product
    ? managementFile.product?.code + ' ' + managementFile.product?.description
    : '';

  return (
    <StyledSummarySection>
      <StyledEditWrapper className="mr-3 my-1">
        {keycloak.hasClaim(Claims.MANAGEMENT_EDIT) && managementFile && canEditDetails() ? (
          <EditButton
            title="Edit management file"
            onClick={onFileEdit}
            style={{ float: 'right' }}
          />
        ) : null}
        {keycloak.hasClaim(Claims.MANAGEMENT_EDIT) && managementFile && !canEditDetails() ? (
          <TooltipIcon
            toolTipId={`${managementFile.id || 0}-summary-cannot-edit-tooltip`}
            toolTip={cannotEditMessage}
          />
        ) : null}
      </StyledEditWrapper>
      <Section>
        <SectionField label="Status" labelWidth={{ xs: '5' }} valueTestId="management-status">
          {managementFile.fileStatusTypeCode?.description}
        </SectionField>
      </Section>

      <Section header="Project">
        <SectionField
          label="Ministry project"
          labelWidth={{ xs: '5' }}
          valueTestId="management-project"
        >
          {projectName}
        </SectionField>
        <SectionField label="Product" labelWidth={{ xs: '5' }} valueTestId="management-product">
          {productName}
        </SectionField>
        <SectionField label="Funding" labelWidth={{ xs: '5' }} valueTestId="management-funding">
          {managementFile.fundingTypeCode?.description}
        </SectionField>
      </Section>

      <Section header="Management Details">
        <SectionField
          label="Management file name"
          labelWidth={{ xs: '5' }}
          valueTestId="management-file-name"
        >
          {managementFile.fileName}
        </SectionField>
        <SectionField
          label="Historical file number"
          labelWidth={{ xs: '5' }}
          valueTestId="management-legacy-file-number"
        >
          {managementFile.legacyFileNum}
        </SectionField>
        <SectionField label="Purpose" labelWidth={{ xs: '5' }} valueTestId="management-purpose">
          {managementFile.purposeTypeCode?.description}
        </SectionField>
        <SectionField
          label="Additional details"
          labelWidth={{ xs: '5' }}
          valueTestId="management-additional-details"
        >
          {managementFile.additionalDetails}
        </SectionField>
      </Section>

      <Section
        isCollapsable
        initiallyExpanded
        header={
          <SectionListHeader
            claims={[Claims.MANAGEMENT_EDIT]}
            title="Management Contact"
            addButtonText="Add a Contact"
            addButtonIcon={<FaUserPlus size={'2rem'} />}
            onButtonAction={() => onAddContact()}
          />
        }
      >
        <LoadingBackdrop show={isLoading} />
        <ManagementFileContactsList
          managementFileContacts={managementFileContacts ?? []}
          onContactEdit={onEditContact}
          onContactDelete={onDeleteContact}
        />
      </Section>

      <Section header="Management Team">
        {managementFile.managementTeam?.map((teamMember, index) => (
          <Fragment key={`management-team-${teamMember?.id ?? index}`}>
            <SectionField
              label={teamMember?.teamProfileType?.description || ''}
              labelWidth={{ xs: '5' }}
              valueTestId={`management-team-member-${index}`}
            >
              <StyledLink
                target="_blank"
                rel="noopener noreferrer"
                to={
                  teamMember?.personId
                    ? `/contact/P${teamMember?.personId}`
                    : `/contact/O${teamMember?.organizationId}`
                }
              >
                <span>
                  {teamMember?.personId
                    ? formatApiPersonNames(teamMember?.person)
                    : teamMember?.organization?.name ?? ''}
                </span>
                <FaExternalLinkAlt className="ml-2" size="1rem" />
              </StyledLink>
            </SectionField>
            {isValidId(teamMember?.organizationId) && (
              <SectionField label="Primary contact" labelWidth={{ xs: '5' }}>
                {teamMember?.primaryContactId ? (
                  <StyledLink
                    target="_blank"
                    rel="noopener noreferrer"
                    to={`/contact/P${teamMember?.primaryContactId}`}
                  >
                    <span>{formatApiPersonNames(teamMember?.primaryContact)}</span>
                    <FaExternalLinkAlt className="ml-2" size="1rem" />
                  </StyledLink>
                ) : (
                  'No contacts available'
                )}
              </SectionField>
            )}
          </Fragment>
        ))}
      </Section>
    </StyledSummarySection>
  );
};

export default ManagementSummaryView;
