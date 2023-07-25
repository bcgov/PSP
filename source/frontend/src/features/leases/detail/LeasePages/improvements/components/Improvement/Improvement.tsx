import { getIn } from 'formik';

import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { PropertyImprovementTypes } from '@/constants/propertyImprovementTypes';

import { ILeaseImprovementForm } from '../../models';

export interface IImprovementProps {
  improvement: ILeaseImprovementForm;
  improvementTypeCodeId?: string;
}

export const sectionTitles = new Map<string, string>([
  [PropertyImprovementTypes.Residential, 'Residential Improvements'],
  [PropertyImprovementTypes.Commercial, 'Commercial Improvements'],
  [PropertyImprovementTypes.Other, 'Other Improvements'],
]);

/**
 * Sub-form containing lease improvements details
 * @param {IImprovementProps} param0
 */
export const Improvement: React.FunctionComponent<React.PropsWithChildren<IImprovementProps>> = ({
  improvement,
  improvementTypeCodeId,
}) => {
  const typeId = improvementTypeCodeId ?? getIn(improvement, 'propertyImprovementTypeId');
  const title = sectionTitles.get(typeId) ?? 'N/A';
  return (
    <>
      <Section header={title}>
        <SectionField label="Unit #" labelWidth="3">
          {improvement.address}
        </SectionField>
        <SectionField label="Building size" labelWidth="3">
          {improvement.structureSize}
        </SectionField>
        <SectionField label="Description" labelWidth="3">
          {improvement.description}
        </SectionField>
      </Section>
    </>
  );
};
export default Improvement;
