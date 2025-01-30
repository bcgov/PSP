import styled from 'styled-components';

import EditButton from '@/components/common/buttons/EditButton';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledEditWrapper } from '@/components/common/Section/SectionStyles';
import { Claims } from '@/constants/index';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_ResearchFileProperty } from '@/models/api/generated/ApiGen_Concepts_ResearchFileProperty';
import { exists } from '@/utils/utils';

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
  researchFileProperty: ApiGen_Concepts_ResearchFileProperty;
  setEditMode: (isEditing: boolean) => void;
}

export const PropertyResearchTabView: React.FunctionComponent<
  React.PropsWithChildren<IPropertyResearchTabViewProps>
> = props => {
  const detail: PropertyResearchFile = {
    id: props.researchFileProperty.id || 0,
    descriptiveName: props.researchFileProperty.propertyName || '',
    purpose:
      props.researchFileProperty.propertyResearchPurposeTypes
        ?.map(x => x.propertyResearchPurposeTypeCode?.description || '')
        .join(', ') || '',
    legalOpinionRequired: exists(props.researchFileProperty.isLegalOpinionRequired)
      ? props.researchFileProperty.isLegalOpinionRequired
        ? 'Yes'
        : 'No'
      : '',
    legalOpinionObtained: exists(props.researchFileProperty.isLegalOpinionObtained)
      ? props.researchFileProperty.isLegalOpinionObtained
        ? 'Yes'
        : 'No'
      : '',
    documentReference: props.researchFileProperty.documentReference || '',
    summaryNotes: props.researchFileProperty.researchSummary || '',
  };

  const { hasClaim } = useKeycloakWrapper();

  return (
    <StyledSummarySection>
      <StyledEditWrapper className="mr-3 my-1">
        {hasClaim(Claims.RESEARCH_EDIT) ? (
          <EditButton
            title="Edit Property Research"
            onClick={() => {
              props.setEditMode(true);
            }}
            style={{ float: 'right' }}
          />
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
        <SectionField label="Summary comments" />
        {detail.summaryNotes}
      </Section>
    </StyledSummarySection>
  );
};

export default PropertyResearchTabView;

const StyledSummarySection = styled.div`
  background-color: ${props => props.theme.css.highlightBackgroundColor};
`;
