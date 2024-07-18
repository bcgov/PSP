import { getIn } from 'formik';
import { ReactNode } from 'react';

import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import TooltipIcon from '@/components/common/TooltipIcon';
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

  const HeaderDisplayNode = (typeId: string): ReactNode => {
    if (typeId === 'OTHER') {
      return (
        <div className="d-flex align-items-center">
          <span>{title}</span>
          <TooltipIcon
            toolTipId="contactInfoToolTip"
            innerClassName="ml-4 mb-1"
            toolTip="This could include lighting, fencing, irrigation, parking etc"
          />
        </div>
      );
    }

    return title;
  };

  return (
    <>
      <Section header={HeaderDisplayNode(typeId)}>
        <SectionField label="Unit #" labelWidth="3">
          {improvement.address}
        </SectionField>
        <SectionField
          label={typeId === PropertyImprovementTypes.Residential ? 'House size' : 'Building size'}
          labelWidth="3"
        >
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
