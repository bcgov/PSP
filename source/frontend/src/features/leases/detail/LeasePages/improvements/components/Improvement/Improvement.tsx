import { Input } from 'components/common/form';
import { PropertyImprovementTypes } from 'constants/propertyImprovementTypes';
import * as Styled from 'features/leases/detail/LeasePages/improvements/styles';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { getIn, useFormikContext } from 'formik';
import { IFormLease } from 'interfaces';
import { withNameSpace } from 'utils/formUtils';

export interface IImprovementProps {
  disabled?: boolean;
  nameSpace?: string;
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
  disabled,
  nameSpace,
  improvementTypeCodeId,
}) => {
  const { values } = useFormikContext<IFormLease>();
  const typeId =
    improvementTypeCodeId ?? getIn(values, withNameSpace(nameSpace, 'propertyImprovementTypeId'));
  const title = sectionTitles.get(typeId) ?? 'N/A';
  return (
    <>
      <Section header={title}>
        <SectionField label="Unit #" labelWidth="3">
          <Input disabled={disabled} field={withNameSpace(nameSpace, 'address')} />{' '}
        </SectionField>
        <SectionField label="Building size" labelWidth="3">
          <Input disabled={disabled} field={withNameSpace(nameSpace, 'structureSize')} />
        </SectionField>
        <SectionField label="Description" labelWidth="3">
          <Styled.FormDescriptionBody
            innerClassName="description"
            rows={5}
            disabled={disabled}
            field={withNameSpace(nameSpace, 'description')}
          />
        </SectionField>
      </Section>
    </>
  );
};
export default Improvement;
