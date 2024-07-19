import { getIn, useFormikContext } from 'formik';
import styled from 'styled-components';

import { YesNoSelect } from '@/components/common/form/YesNoSelect';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { withNameSpace } from '@/utils/formUtils';

import { getSuggestedFee, SuggestedFeeCode } from '../../../leaseUtils';

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
  const fee = getSuggestedFee(isPublicBenefit, isFinancialGain);

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
        tooltip="Licence Administration Fee (LAF) *: If the financial gain far outweighs the public benefit, Fair Market Value should be considered over Licence Administration Fee"
        labelWidth="3"
      >
        <span data-testid="suggestedFee">{fee}</span>
        <StyledHelpText>
          {(fee === SuggestedFeeCode.NOMINAL || fee === SuggestedFeeCode.ANY) && (
            <p className="m-0">
              <b>Nominal:&nbsp;</b>
              No or nominal fee determinations should include justification in the Comments field.
            </p>
          )}
          {(fee === SuggestedFeeCode.FMV || fee === SuggestedFeeCode.ANY) && (
            <p className="m-0">
              <b>FMV:&nbsp;</b>
              Fair market value fee determination should include the square footage rate in the
              Comments field. The rate determination summary or appraisal should be uploaded.
            </p>
          )}
          {(fee === SuggestedFeeCode.LAF || fee === SuggestedFeeCode.ANY) && (
            <p className="m-0">
              <b>LAF:&nbsp;</b>
              License administration fees are charged when there is either: a financial gain for the
              licensee and a benefit to the public, or when there is no financial gain to the
              licensee and there is no benefit for the public. The LAF can vary based on the impact
              to the land, the length of the LOO, and if MOTI requires a legal review.
            </p>
          )}
        </StyledHelpText>
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

const StyledHelpText = styled.div`
  font-size: 1.4rem;
  margin-top: 1rem;
`;
