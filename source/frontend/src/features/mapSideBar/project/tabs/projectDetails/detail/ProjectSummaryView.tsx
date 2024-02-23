import EditButton from '@/components/common/EditButton';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledEditWrapper, StyledSummarySection } from '@/components/common/Section/SectionStyles';
import Claims from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';

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
          <EditButton title="Edit project" onClick={onEdit} />
        ) : null}
      </StyledEditWrapper>
      <Section header="Project Details" isCollapsable initiallyExpanded>
        <SectionField label="Project summary" labelWidth={'12'}>
          {project.note}
        </SectionField>
      </Section>
      <Section header="Associated Codes" isCollapsable initiallyExpanded>
        <SectionField label="Cost type" labelWidth="3">
          {project.costTypeCode?.description ?? ''}
        </SectionField>
        <SectionField label="Work activity" labelWidth="3">
          {project.workActivityCode?.description ?? ''}
        </SectionField>
        <SectionField label="Business function" labelWidth="3">
          {project.businessFunctionCode?.description ?? ''}
        </SectionField>
      </Section>
      <ProjectProductView project={project} />
    </StyledSummarySection>
  );
};

export default ProjectSummaryView;
