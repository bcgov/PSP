import { Badge } from 'react-bootstrap';
import styled from 'styled-components';

import { ExternalLink } from '@/components/common/ExternalLink';
import FormGuideContainer from '@/components/common/form/FormGuide/FormGuideContainer';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { ApiGen_Concepts_PropertyImprovement } from '@/models/api/generated/ApiGen_Concepts_PropertyImprovement';
import { getApiPropertyName } from '@/utils';

import { IFilePropertyImprovements } from '../models/FilePropertyImprovements';
import PropertyImprovementDetails from '../PropertyImprovementDetails/PropertyImprovementDetails';

export interface IFilePropertiesImprovementsViewProps {
  isLoading: boolean;
  filePropertiesImprovements: IFilePropertyImprovements[];
}

export const FilePropertiesImprovementsView: React.FunctionComponent<
  IFilePropertiesImprovementsViewProps
> = ({ isLoading, filePropertiesImprovements }) => {
  const getHeader = (
    property: ApiGen_Concepts_Property,
    improvementsCount: number,
  ): React.ReactNode => {
    const propertyName = getApiPropertyName(property);
    const nameString = `${propertyName.label}: ${propertyName.value}`;

    return (
      <StyledHeaderDiv>
        {nameString}
        <ExternalLink to={`/mapview/sidebar/property/${property.id}/improvements`}>
          {null}
        </ExternalLink>
        <Badge variant="primary" className="rounded-pill">
          {improvementsCount}
        </Badge>
      </StyledHeaderDiv>
    );
  };

  return (
    <StyledSummarySection>
      <LoadingBackdrop show={isLoading} parentScreen={true} />

      <Section>
        <FormGuideContainer
          title="Improvements"
          guideBody={<p>Click on a property to edit that property improvements in a new tab</p>}
        ></FormGuideContainer>
      </Section>

      {filePropertiesImprovements.map((propertyImprovements: IFilePropertyImprovements) => (
        <Section
          header={getHeader(
            propertyImprovements.property,
            propertyImprovements.improvements?.length ?? 0,
          )}
          key={propertyImprovements.property.id}
          data-testid={`property-improvements-${propertyImprovements.property.id}`}
          isCollapsable
          initiallyExpanded={false}
        >
          {propertyImprovements.improvements.map(
            (improvement: ApiGen_Concepts_PropertyImprovement, index: number) => (
              <div key={improvement.id}>
                <PropertyImprovementDetails
                  propertyImprovement={improvement}
                  key={improvement.id}
                ></PropertyImprovementDetails>
                {index < propertyImprovements.improvements.length - 1 && <hr></hr>}
              </div>
            ),
          )}

          {propertyImprovements.improvements.length === 0 && (
            <p>
              There are no commercial, residential, or other improvements indicated with this
              Property.
            </p>
          )}
        </Section>
      ))}

      {(!filePropertiesImprovements || filePropertiesImprovements.length === 0) && (
        <Section>
          <p>
            There are no commercial, residential, or other improvements indicated with this File.
          </p>
        </Section>
      )}
    </StyledSummarySection>
  );
};

const StyledHeaderDiv = styled.div`
  display: flex;

  a {
    margin-left: 0.4rem;
  }

  span {
    margin-left: 0.4rem;
  }

  .badge {
    margin-left: 1rem;
    margin-bottom: 0.25rem;
  }
`;
