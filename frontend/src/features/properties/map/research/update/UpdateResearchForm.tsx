import { Input } from 'components/common/form';
import { InlineInput } from 'components/common/form/styles';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { StyledFormSection, StyledSectionHeader } from 'features/mapSideBar/tabs/SectionStyles';
import * as React from 'react';
import styled from 'styled-components';

const UpdateResearchForm: React.FunctionComponent = () => {
  return (
    <StyledSummarySection>
      <StyledFormSection>
        <StyledSectionHeader>Roads</StyledSectionHeader>
        <SectionField label="Road name">
          <Input field={'parcelIdentifier'} />
        </SectionField>
        <SectionField label="Road alias">
          <Input field={'parcelIdentifier'} />
        </SectionField>
      </StyledFormSection>
      <StyledFormSection>
        <StyledSectionHeader>Research Request</StyledSectionHeader>
        <SectionField label="Research purpose">
          <Input field={'parcelIdentifier'} />
        </SectionField>
        <SectionField label="Request date">
          <Input field={'parcelIdentifier'} />
        </SectionField>
        <SectionField label="Source of request">
          <Input field={'parcelIdentifier'} />
        </SectionField>
        <SectionField label="Requester">
          <Input field={'parcelIdentifier'} />
        </SectionField>
        <SectionField label="Organization">
          <Input field={'parcelIdentifier'} />
        </SectionField>
        <SectionField label="Description of request">
          <Input field={'parcelIdentifier'} />
        </SectionField>
      </StyledFormSection>
      <StyledFormSection>
        <StyledSectionHeader>Result</StyledSectionHeader>
        <SectionField label="Research completed on">
          <Input field={'parcelIdentifier'} />
        </SectionField>
        <SectionField label="Result of request">
          <Input field={'parcelIdentifier'} />
        </SectionField>
      </StyledFormSection>
      <StyledFormSection>
        <StyledSectionHeader>Expropriation</StyledSectionHeader>
        <SectionField label="Expropriation?">
          <Input field={'parcelIdentifier'} />
        </SectionField>
        <SectionField label="Expropriation notes">
          <Input field={'parcelIdentifier'} />
        </SectionField>
      </StyledFormSection>
    </StyledSummarySection>
  );
};

export default UpdateResearchForm;

const StyledSummarySection = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
`;

const LargeInlineInput = styled(InlineInput)`
  input.form-control {
    min-width: 50rem;
    max-width: 50rem;
  }
`;
