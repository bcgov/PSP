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
        {project?.products?.map((product: Api_Product, index: number) => (
          <div
            className={index === productCount - 1 ? '' : 'pb-5'}
            key={`project-${project.id}-product-${product.id}`}
          >
            <StyledProductDescription>{`${product.code} - ${product.description}`}</StyledProductDescription>
            <SectionField label="Start Date" labelWidth={'2'}>
              {prettyFormatDate(product.startDate)}
            </SectionField>
            <SectionField label="Cost estimate" labelWidth={'2'}>
              {formatMoney(product.costEstimate)}
              {!!product.costEstimateDate
                ? ` as of ${prettyFormatDate(product.costEstimateDate)}`
                : ' no estimate date entered'}
            </SectionField>
            <SectionField label="Objectives" labelWidth={'12'}>
              {product.objective || 'no objective entered'}
            </SectionField>
            <SectionField label="Scope" labelWidth={'12'}>
              {product.scope || 'no scope entered'}
            </SectionField>
          </div>
        ))}
      </Section>
    </StyledSummarySection>
  );
};

export default ProjectProductView;

const StyledSummarySection = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
`;

const StyledProductDescription = styled.div`
  font-weight: bold;
  border-bottom-style: solid;
  border-bottom-color: grey;
  border-bottom-width: 0.1rem;
  margin-bottom: 1rem;
`;
