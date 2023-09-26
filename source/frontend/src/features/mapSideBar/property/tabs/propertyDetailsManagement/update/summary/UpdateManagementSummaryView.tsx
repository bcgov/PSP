import styled from 'styled-components';

import { Section } from '@/components/common/Section/Section';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { Api_PropertyManagement } from '@/models/api/Property';

export interface IUpdateManagementSummaryViewProps {
  isLoading: boolean;
  propertyManagement: Api_PropertyManagement;
  onSave: (apiModel: Api_PropertyManagement) => Promise<void>;
}

export const UpdateManagementSummaryView: React.FC<IUpdateManagementSummaryViewProps> = ({
  isLoading,
  propertyManagement,
  onSave,
}) => {
  return (
    <StyledFormWrapper>
      <StyledSummarySection>
        <Section header="Summary"></Section>
      </StyledSummarySection>
    </StyledFormWrapper>
  );
};

const StyledFormWrapper = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  overflow-y: auto;
  padding-right: 1rem;
  padding-bottom: 1rem;
`;
