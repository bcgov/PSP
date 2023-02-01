import EditButton from 'components/common/EditButton';
import Claims from 'constants/claims';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Api_Project } from 'models/api/Project';
import styled from 'styled-components';

export interface IProjectSummaryViewProps {
  project?: Api_Project;
  onEdit: () => void;
}

const ProjectSummaryView: React.FunctionComponent<
  React.PropsWithChildren<IProjectSummaryViewProps>
> = ({ project, onEdit }) => {
  const keycloak = useKeycloakWrapper();

  return (
    <StyledSummarySection>
      <StyledEditWrapper className="mr-3 my-1">
        {keycloak.hasClaim(Claims.ACQUISITION_EDIT) && project !== undefined ? (
          <EditButton title="Edit project" onClick={onEdit} />
        ) : null}
      </StyledEditWrapper>
      <Section header="Project Details" isCollapsable initiallyExpanded>
        <SectionField label="Project summary" labelWidth={'12'}>
          {project?.note}
        </SectionField>
      </Section>
    </StyledSummarySection>
  );
};

export default ProjectSummaryView;

const StyledSummarySection = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
`;

const StyledEditWrapper = styled.div`
  color: ${props => props.theme.css.primary};

  text-align: right;
`;
