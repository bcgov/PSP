import { getIn, useFormikContext } from 'formik';

import { YesNoSelect } from '@/components/common/form/YesNoSelect';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { withNameSpace } from '@/utils/formUtils';

import { getSuggestedFee } from '../../../leaseUtils';

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
  const formikProps = useFormikContext<ApiGen_Concepts_Lease>();
  const isPublicBenefit = getIn(formikProps.values, withNameSpace(nameSpace, 'isPublicBenefit'));
  const isFinancialGain = getIn(formikProps.values, withNameSpace(nameSpace, 'isFinancialGain'));
  const feeDeterminationNote = getIn(
    formikProps.values,
    withNameSpace(nameSpace, 'feeDeterminationNote'),
  );

  return (
    <Section initiallyExpanded={true} isCollapsable={true} header="Fee Determination">
      <SectionField label="Public benefit" labelWidth="3">
        <YesNoSelect disabled={disabled} field={withNameSpace(nameSpace, 'isPublicBenefit')} />
      </SectionField>

      <SectionField label="Financial gain" labelWidth="3">
        <YesNoSelect disabled={disabled} field={withNameSpace(nameSpace, 'isFinancialGain')} />
      </SectionField>

      <SectionField
        label="Suggested fee"
        tooltip="If the financial gain far outweighs the public benefit, Fair Market Value should be considered over Licence Administration Fee."
        labelWidth="3"
      >
        {getSuggestedFee(isPublicBenefit, isFinancialGain)}
      </SectionField>

      <SectionField
        label="Notes"
        tooltip="Deviations from standard fees should be explained here"
        labelWidth="3"
      >
        {feeDeterminationNote}
      </SectionField>
    </Section>
  );
};
