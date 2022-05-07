import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { StyledFormSection, StyledSectionHeader } from 'features/mapSideBar/tabs/SectionStyles';
import { Api_ResearchFileProperty } from 'models/api/ResearchFile';
import * as React from 'react';
import styled from 'styled-components';

interface PropertyResearchFile {
  id: number;
  descriptiveName: string;
  purpose: string;
  legalOpinionRequired: string;
  legalOpinionObtained: string;
  documentReference: string;
  sumaryNotes: string;
}

export interface IPropertyResearchTabViewProps {
  researchFile: Api_ResearchFileProperty;
}

const PropertyResearchTabView: React.FunctionComponent<IPropertyResearchTabViewProps> = props => {
  const detail: PropertyResearchFile = {
    id: props.researchFile.id || 0,
    descriptiveName: props.researchFile.propertyName || '',
    purpose:
      props.researchFile.propertyPurpose?.map(x => x.propertyPurposeType?.description).join(', ') ||
      '',
    legalOpinionRequired:
      props.researchFile.isLegalOpinionRequired !== undefined
        ? props.researchFile.isLegalOpinionRequired
          ? 'Yes'
          : 'No'
        : '',
    legalOpinionObtained:
      props.researchFile.isLegalOpinionObtained !== undefined
        ? props.researchFile.isLegalOpinionObtained
          ? 'Yes'
          : 'No'
        : '',
    documentReference: props.researchFile.documentReference || '',
    sumaryNotes: props.researchFile.researchSummary || '',
  };

  return (
    <StyledSummarySection>
      <StyledFormSection>
        <StyledSectionHeader>Property of Interest</StyledSectionHeader>
        <SectionField label="Descriptive name">{detail.descriptiveName}</SectionField>
        <SectionField label="Purpose">{detail.purpose}</SectionField>
        <SectionField label="Legal opinion req'd?">{detail.legalOpinionRequired}</SectionField>
        <SectionField label="Legal opinion obtained?">{detail.legalOpinionObtained}</SectionField>
        <SectionField label="Document reference">{detail.documentReference}</SectionField>
      </StyledFormSection>
      <StyledFormSection>
        <StyledSectionHeader>Research Summary</StyledSectionHeader>
        <SectionField label="Summary notes" />
        {detail.sumaryNotes}
      </StyledFormSection>
    </StyledSummarySection>
  );
};

export default PropertyResearchTabView;

const StyledSummarySection = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
`;
