import React from 'react';
import { FaExternalLinkAlt } from 'react-icons/fa';

import EditButton from '@/components/common/buttons/EditButton';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledEditWrapper, StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { StyledLink } from '@/components/common/styles';
import Claims from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import { formatApiPersonNames } from '@/utils/personUtils';

import ProjectProductView from './ProjectProductView';

export interface IProjectSummaryViewProps {
  project: ApiGen_Concepts_Project;
  onEdit: () => void;
}

const ProjectSummaryView: React.FunctionComponent<
  React.PropsWithChildren<IProjectSummaryViewProps>
> = ({ project, onEdit }) => {
  const keycloak = useKeycloakWrapper();

  return (
    <StyledSummarySection>
      <StyledEditWrapper className="mr-3 my-1">
        {keycloak.hasClaim(Claims.PROJECT_EDIT) && project !== undefined ? (
          <EditButton title="Edit project" onClick={onEdit} style={{ float: 'right' }} />
        ) : null}
      </StyledEditWrapper>
      <Section header="Project Details" isCollapsable initiallyExpanded>
        <SectionField label="Project summary" labelWidth={{ xs: 12 }}>
          {project.note}
        </SectionField>
      </Section>
      <Section header="Associated Codes" isCollapsable initiallyExpanded>
        <SectionField label="Cost type" labelWidth={{ xs: 3 }}>
          {project.costTypeCode?.description ?? ''}
        </SectionField>
        <SectionField label="Work activity" labelWidth={{ xs: 3 }}>
          {project.workActivityCode?.description ?? ''}
        </SectionField>
        <SectionField label="Business function" labelWidth={{ xs: 3 }}>
          {project.businessFunctionCode?.description ?? ''}
        </SectionField>
      </Section>
      <ProjectProductView project={project} />
      <Section header="Project Management Team">
        {project?.projectPersons?.map((teamMember, index) => (
          <React.Fragment key={`project-team-${index}`}>
            <SectionField label="Management team member">
              <StyledLink
                target="_blank"
                rel="noopener noreferrer"
                to={`/contact/P${teamMember?.personId}`}
              >
                <span>{formatApiPersonNames(teamMember?.person)}</span>
                <FaExternalLinkAlt className="ml-2" size="1rem" />
              </StyledLink>
            </SectionField>
          </React.Fragment>
        ))}
      </Section>
    </StyledSummarySection>
  );
};

export default ProjectSummaryView;
