import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { useFormikContext } from 'formik';

export interface IDetailFeeDeterminationProps {
  nameSpace?: string;
  disabled?: boolean;
}

/**
 * Sub-form containing lease detail administration fields
 * @param {IDetailFeeDeterminationProps} param0
 */
export const DetailFeeDetermination: React.FunctionComponent<
  React.PropsWithChildren<IDetailFeeDeterminationProps>
> = ({ nameSpace, disabled }) => {
  const { values } = useFormikContext<ApiGen_Concepts_Lease>();
  return (
    <Section initiallyExpanded={true} isCollapsable={true} header="Fee Determination">
      <SectionField label="Program" labelWidth="3"></SectionField>

      <SectionField label="Program" labelWidth="3"></SectionField>

      <SectionField label="Program" labelWidth="3"></SectionField>
    </Section>
  );
};
