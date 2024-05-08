import { Fragment } from 'react';
import { FaExternalLinkAlt } from 'react-icons/fa';

import EditButton from '@/components/common/EditButton';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledEditWrapper, StyledSummarySection } from '@/components/common/Section/SectionStyles';
import TooltipIcon from '@/components/common/TooltipIcon';
import { StyledLink } from '@/components/maps/leaflet/LayerPopup/styles';
import { Claims, Roles } from '@/constants';
import { cannotEditMessage } from '@/features/mapSideBar/acquisition/common/constants';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { prettyFormatDate } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

import DispositionStatusUpdateSolver from './DispositionStatusUpdateSolver';

export interface IDispositionSummaryViewProps {
  dispositionFile?: ApiGen_Concepts_DispositionFile;
  onEdit: () => void;
}

export const DispositionSummaryView: React.FunctionComponent<IDispositionSummaryViewProps> = ({
  dispositionFile,
  onEdit,
}) => {
  const keycloak = useKeycloakWrapper();
  const statusSolver = new DispositionStatusUpdateSolver(dispositionFile);
  const canEditDetails = () => {
    if (keycloak.hasRole(Roles.SYSTEM_ADMINISTRATOR) || statusSolver.canEditProperties()) {
      return true;
    }
    return false;
  };

  const projectName = dispositionFile?.project
    ? dispositionFile?.project?.code + ' - ' + dispositionFile?.project?.description
    : '';

  const productName = dispositionFile?.product
    ? dispositionFile?.product?.code + ' ' + dispositionFile?.product?.description
    : '';

  return (
    <StyledSummarySection>
      <StyledEditWrapper className="mr-3 my-1">
        {keycloak.hasClaim(Claims.DISPOSITION_EDIT) &&
        dispositionFile !== undefined &&
        canEditDetails() ? (
          <EditButton title="Edit disposition file" onClick={onEdit} />
        ) : null}
        {keycloak.hasClaim(Claims.DISPOSITION_EDIT) &&
        dispositionFile !== undefined &&
        !canEditDetails() ? (
          <TooltipIcon
            toolTipId={`${dispositionFile?.id || 0}-summary-cannot-edit-tooltip`}
            toolTip={cannotEditMessage}
          />
        ) : null}
      </StyledEditWrapper>
      <Section>
        <SectionField label="Status" labelWidth="5">
          {dispositionFile?.fileStatusTypeCode?.description}
        </SectionField>
      </Section>
      <Section header="Project">
        <SectionField label="Ministry project" labelWidth="5" valueTestId="dsp-project">
          {projectName}
        </SectionField>
        <SectionField label="Product" labelWidth="5" valueTestId="dsp-product">
          {productName}
        </SectionField>
        <SectionField label="Funding" labelWidth="5">
          {dispositionFile?.fundingTypeCode?.description}
        </SectionField>
      </Section>
      <Section header="Schedule">
        <SectionField label="Assigned date" labelWidth="5">
          {prettyFormatDate(dispositionFile?.assignedDate)}
        </SectionField>
        <SectionField label="Disposition completed date" labelWidth="5">
          {prettyFormatDate(dispositionFile?.completionDate)}
        </SectionField>
      </Section>
      <Section header="Disposition Details">
        <SectionField label="Disposition file name" labelWidth="5">
          {dispositionFile?.fileName}
        </SectionField>
        <SectionField
          label="Reference number"
          tooltip="Provide available reference number for historic program or file number (e.g.  RAEG, Acquisition File, etc.)."
          labelWidth="5"
        >
          {dispositionFile?.fileReference}
        </SectionField>
        <SectionField label="Disposition status" labelWidth="5">
          {dispositionFile?.dispositionStatusTypeCode?.description}
        </SectionField>
        <SectionField label="Disposition type" labelWidth="5">
          {dispositionFile?.dispositionTypeCode?.description}
        </SectionField>
        {dispositionFile?.dispositionTypeCode?.id === 'OTHER' && (
          <SectionField label="Other (disposition type)" labelWidth="5">
            {dispositionFile?.dispositionTypeOther}
          </SectionField>
        )}
        <SectionField
          label="Initiating document"
          tooltip="Provide the type of document that has initiated the disposition process."
          labelWidth="5"
        >
          {dispositionFile?.initiatingDocumentTypeCode?.description}
        </SectionField>
        {dispositionFile?.initiatingDocumentTypeCode?.id === 'OTHER' && (
          <SectionField label="Other (initiating document)" labelWidth="5">
            {dispositionFile?.initiatingDocumentTypeOther}
          </SectionField>
        )}
        <SectionField
          label="Initiating document date"
          tooltip="Provide the date initiating document was signed off."
          labelWidth="5"
        >
          {prettyFormatDate(dispositionFile?.initiatingDocumentDate)}
        </SectionField>
        <SectionField label="Physical file status" labelWidth="5">
          {dispositionFile?.physicalFileStatusTypeCode?.description}
        </SectionField>
        <SectionField label="Initiating branch" labelWidth="5">
          {dispositionFile?.initiatingBranchTypeCode?.description}
        </SectionField>
        <SectionField label="Ministry region" labelWidth="5">
          {dispositionFile?.regionCode?.description}
        </SectionField>
      </Section>
      <Section header="Disposition Team">
        {dispositionFile?.dispositionTeam?.map((teamMember, index) => (
          <Fragment key={`disp-team-${index}`}>
            <SectionField label={teamMember?.teamProfileType?.description || ''} labelWidth="5">
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
            {teamMember?.organizationId && (
              <SectionField label="Primary contact" labelWidth="5">
                {teamMember?.primaryContactId ? (
                  <StyledLink
                    target="_blank"
                    rel="noopener noreferrer"
                    to={`/contact/P${teamMember?.primaryContactId}`}
                  >
                    <span>{formatApiPersonNames(teamMember?.primaryContact)}</span>
                    <FaExternalLinkAlt className="m1-2" size="1rem" />
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

export default DispositionSummaryView;
