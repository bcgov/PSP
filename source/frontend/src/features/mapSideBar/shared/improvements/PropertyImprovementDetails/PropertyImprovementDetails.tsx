import { SectionField } from '@/components/common/Section/SectionField';
import { ApiGen_Concepts_PropertyImprovement } from '@/models/api/generated/ApiGen_Concepts_PropertyImprovement';

export interface IPropertyImprovementDetailsProps {
  propertyImprovement: ApiGen_Concepts_PropertyImprovement;
}

export const PropertyImprovementDetails: React.FunctionComponent<
  IPropertyImprovementDetailsProps
> = ({ propertyImprovement }) => {
  return (
    <>
      <SectionField
        labelWidth={{ xs: 4 }}
        label="Improvement type"
        valueTestId={`improvement[${propertyImprovement.id}].type`}
      >
        {propertyImprovement.propertyImprovementTypeCode?.description}
      </SectionField>

      <SectionField
        label="Description"
        labelWidth={{ xs: 4 }}
        valueTestId={`improvement[${propertyImprovement.id}].description`}
      >
        {propertyImprovement.improvementDescription ?? ''}
      </SectionField>
    </>
  );
};

export default PropertyImprovementDetails;
