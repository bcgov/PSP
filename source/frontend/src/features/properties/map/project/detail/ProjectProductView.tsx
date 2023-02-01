import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Api_Product, Api_Project } from 'models/api/Project';
import styled from 'styled-components';
import { formatMoney, prettyFormatDate } from 'utils';

export interface IProjectProductViewProps {
  project?: Api_Project;
}

const ProjectProductView: React.FunctionComponent<
  React.PropsWithChildren<IProjectProductViewProps>
> = ({ project }) => {
  const productCount = project?.products?.length || 0;

  return (
    <StyledSummarySection>
      <Section header="Associated Products" isCollapsable initiallyExpanded>
        {project?.products?.map((a: Api_Product, b: number) => (
          <StyledProductWrapper className={b === productCount - 1 ? '' : 'pb-5'}>
            <StyledProductDescription>{`${a.code} - ${a.description}`}</StyledProductDescription>
            <SectionField label="Start Date" labelWidth={'2'}>
              {prettyFormatDate(a.startDate)}
            </SectionField>
            <SectionField label="Cost estimate" labelWidth={'2'}>
              {formatMoney(a.costEstimate)}
              {a.costEstimateDate !== undefined
                ? ` as of ${prettyFormatDate(a.costEstimateDate)}`
                : ' no estimate date entered'}
            </SectionField>
            <SectionField label="Objectives" labelWidth={'12'}>
              {a.objective || 'no objective entered'}
            </SectionField>
            <SectionField label="Scope" labelWidth={'12'}>
              {a.scope || 'no scope entered'}
            </SectionField>
          </StyledProductWrapper>
        ))}
      </Section>
    </StyledSummarySection>
  );
};

export default ProjectProductView;

const StyledSummarySection = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
`;

const StyledProductWrapper = styled.div``;

const StyledProductDescription = styled.div`
  font-weight: bold;
  border-bottom-style: solid;
  border-bottom-color: grey;
  border-bottom-width: 0.1rem;
  margin-bottom: 1rem;
`;
