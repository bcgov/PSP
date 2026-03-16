import { SectionField } from '@/components/common/Section/SectionField';
import { ApiGen_Concepts_PropertyImprovement } from '@/models/api/generated/ApiGen_Concepts_PropertyImprovement';
import { prettyFormatDate } from '@/utils/dateUtils';

export interface IPropertyImprovementDetailsProps {
  propertyImprovement: ApiGen_Concepts_PropertyImprovement;
  propertyImprovementIndex?: number;
}

export const PropertyImprovementDetails: React.FunctionComponent<
  IPropertyImprovementDetailsProps
> = ({ propertyImprovement, propertyImprovementIndex }) => {
  return (
    <>
      <SectionField
        labelWidth={{ xs: 4 }}
        label="Name"
        valueTestId={`improvement[${propertyImprovementIndex}].name`}
      >
        {propertyImprovement.improvementName ?? ''}
      </SectionField>

      <SectionField
        labelWidth={{ xs: 4 }}
        label="Improvement type"
        valueTestId={`improvement[${propertyImprovementIndex}].type`}
      >
        {propertyImprovement.improvementTypeCode?.description}
      </SectionField>

      <SectionField
        labelWidth={{ xs: 4 }}
        label="Improvement date"
        valueTestId={`improvement[${propertyImprovementIndex}].date`}
      >
        {prettyFormatDate(propertyImprovement.improvementDate)}
      </SectionField>

      <SectionField
        labelWidth={{ xs: 4 }}
        label="Improvement status"
        valueTestId={`improvement[${propertyImprovementIndex}].status`}
      >
        {propertyImprovement.improvementStatusCode?.description}
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
