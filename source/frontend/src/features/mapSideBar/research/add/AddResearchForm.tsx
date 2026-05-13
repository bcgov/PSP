import React from 'react';
import styled from 'styled-components';

import { Input } from '@/components/common/form/';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';

import { PropertyForm } from '../../shared/models';
import { ResearchFileNameGuide } from '../common/ResearchFileNameGuide';
import { UpdateProjectsSubForm } from '../common/updateProjects/UpdateProjectsSubForm';
import ResearchProperties from './ResearchProperties';

export interface IAddResearchFormProps {
  confirmBeforeAdd: (propertyForm: PropertyForm) => Promise<boolean>;
}

const AddResearchForm: React.FC<IAddResearchFormProps> = props => {
  return (
    <StyledFormWrapper>
      <Section>
        <SectionField label="Name this research file" required>
          <LargeInput
            field="name"
            placeholder="Road name - MOTT District - Location or description"
          />
          A unique file number will be generated for this research file on save.
        </SectionField>
        <ResearchFileNameGuide />
      </Section>
      <Section header="Project">
        <UpdateProjectsSubForm field="researchFileProjects" />
      </Section>
      <ResearchProperties confirmBeforeAdd={props.confirmBeforeAdd} />
    </StyledFormWrapper>
  );
};

export default AddResearchForm;

const LargeInput = styled(Input)`
  input.form-control {
    min-width: 50rem;
    max-width: 50rem;
  }
`;

const StyledFormWrapper = styled.div`
  background-color: ${props => props.theme.css.highlightBackgroundColor};
  padding-top: 0.5rem;
  padding-bottom: 0.5rem;
`;
