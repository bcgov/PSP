import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Api_Project } from 'models/api/Project';
import styled from 'styled-components';

export interface IProjectSummaryViewProps {
  project?: Api_Project;
  onEdit: () => void;
}

const ProjectSummaryView: React.FunctionComponent<
  React.PropsWithChildren<IProjectSummaryViewProps>
> = ({ project, onEdit }) => {
  return (
    <StyledSummarySection>
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
