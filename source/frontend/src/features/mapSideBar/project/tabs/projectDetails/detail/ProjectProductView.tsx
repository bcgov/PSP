import styled from 'styled-components';

import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { ApiGen_Concepts_Product } from '@/models/api/generated/ApiGen_Concepts_Product';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import { exists, formatMoney, prettyFormatDate } from '@/utils';

export interface IProjectProductViewProps {
  project: ApiGen_Concepts_Project;
}

const ProjectProductView: React.FunctionComponent<
  React.PropsWithChildren<IProjectProductViewProps>
> = ({ project }) => {
  const productCount = project.projectProducts?.length || 0;
  const products = project.projectProducts?.map(x => x.product).filter(exists);
  return (
    <StyledSummarySection>
      <Section header="Associated Products" isCollapsable initiallyExpanded>
        {products?.map((product: ApiGen_Concepts_Product, index: number) => (
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
              {product.costEstimateDate
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

const StyledProductDescription = styled.div`
  font-weight: bold;
  border-bottom-style: solid;
  border-bottom-color: grey;
  border-bottom-width: 0.1rem;
  margin-bottom: 1rem;
`;
