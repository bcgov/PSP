import * as React from 'react';
import { Button } from 'react-bootstrap';
import { FaEdit } from 'react-icons/fa';
import styled from 'styled-components';

import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { Claims } from '@/constants/index';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import { Api_ResearchFileProperty } from '@/models/api/ResearchFile';

interface PropertyResearchFile {
  id: number;
  descriptiveName: string;
  purpose: string;
  legalOpinionRequired: string;
  legalOpinionObtained: string;
  documentReference: string;
  summaryNotes: string;
}

export interface IPropertyResearchTabViewProps {
  researchFile: Api_ResearchFileProperty;
  setEditMode: (isEditing: boolean) => void;
}

export const PropertyResearchTabView: React.FunctionComponent<
  React.PropsWithChildren<IPropertyResearchTabViewProps>
> = props => {
  const detail: PropertyResearchFile = {
    id: props.researchFile.id || 0,
    descriptiveName: props.researchFile.propertyName || '',
    purpose:
      props.researchFile.purposeTypes
        ?.map(x => x.propertyPurposeType?.description || '')
        .join(', ') || '',
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
    summaryNotes: props.researchFile.researchSummary || '',
  };

  const { hasClaim } = useKeycloakWrapper();

  return (
    <StyledSummarySection>
      <StyledEditWrapper className="mr-3 my-1">
        {hasClaim(Claims.RESEARCH_EDIT) ? (
          <Button
            variant="link"
            onClick={() => {
              props.setEditMode(true);
            }}
          >
            <FaEdit size={'2rem'} />
          </Button>
        ) : null}
      </StyledEditWrapper>
      <Section header="Property of Interest">
        <SectionField label="Descriptive name">{detail.descriptiveName}</SectionField>
        <SectionField label="Purpose">{detail.purpose}</SectionField>
        <SectionField label="Legal opinion req'd?">{detail.legalOpinionRequired}</SectionField>
        <SectionField label="Legal opinion obtained?">{detail.legalOpinionObtained}</SectionField>
        <SectionField label="Document reference">{detail.documentReference}</SectionField>
      </Section>
      <Section header="Research Summary">
        <SectionField label="Summary notes" />
        {detail.summaryNotes}
      </Section>
    </StyledSummarySection>
  );
};

export default PropertyResearchTabView;

const StyledSummarySection = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
`;

const StyledEditWrapper = styled.div`
  color: ${props => props.theme.css.primary};

  text-align: right;
`;
