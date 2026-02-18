import { SectionField } from '@/components/common/Section/SectionField';
import { ApiGen_Concepts_PropertyImprovement } from '@/models/api/generated/ApiGen_Concepts_PropertyImprovement';

export interface IPropertyImprovementDetailsProps {
  propertyImprovement: ApiGen_Concepts_PropertyImprovement;
  propertyImprovementIndex: number;
}

export const PropertyImprovementDetails: React.FunctionComponent<
  IPropertyImprovementDetailsProps
> = ({ propertyImprovement, propertyImprovementIndex }) => {
  return (
    <>
      <SectionField
        labelWidth={{ xs: 4 }}
        label="Improvement type"
        valueTestId={`improvement[${propertyImprovementIndex}].type`}
      >
        {propertyImprovement.propertyImprovementTypeCode?.description}
      </SectionField>

      <SectionField
        label="Description"
        labelWidth={{ xs: 4 }}
        valueTestId={`improvement[${propertyImprovementIndex}].description`}
      >
        {propertyImprovement.improvementDescription ?? ''}
      </SectionField>
    </>
  );
};

export default PropertyImprovementDetails;
