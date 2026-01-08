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
  const getHeader = (property: ApiGen_Concepts_Property): React.ReactNode => {
    const propertyName = getApiPropertyName(property);
    const nameString = `${propertyName.label}: ${propertyName.value}`;
    return <label>{nameString}</label>;
  };

  return (
    <StyledSummarySection>
      <LoadingBackdrop show={isLoading} parentScreen={true} />
      {filePropertiesImprovements.map((propertyImprovements: IFilePropertyImprovements) => (
        <Section
          header={getHeader(propertyImprovements.property)}
          key={propertyImprovements.property.id}
          data-testid={`property-improvements-${propertyImprovements.property.id}`}
          isCollapsable
          initiallyExpanded={false}
        >
          {propertyImprovements.improvements.map(
            (improvement: ApiGen_Concepts_PropertyImprovement, index: number) => (
              <>
                <PropertyImprovementDetails
                  propertyImprovement={improvement}
                  key={improvement.id}
                ></PropertyImprovementDetails>
                {index < propertyImprovements.improvements.length - 1 && <hr></hr>}
              </>
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
    </StyledSummarySection>
  );
};
