import { FormikProps, getIn } from 'formik';
import { useEffect, useState } from 'react';
import styled from 'styled-components';

import { TextArea } from '@/components/common/form';
import { InlineYesNoSelect } from '@/components/common/form/styles';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';

import { getSuggestedFee, SuggestedFeeCode } from '../leaseUtils';
import { LeaseFormModel } from '../models';

export interface IFeeDeterminationSubFormProps {
  formikProps: FormikProps<LeaseFormModel>;
}

const FeeDeterminationSubForm: React.FunctionComponent<IFeeDeterminationSubFormProps> = ({
  formikProps,
}) => {
  const { values } = formikProps;
  const isPublicBenefit = getIn(values, 'isPublicBenefit');
  const financialGain = getIn(values, 'isFinancialGain');

  const [fee, setFee] = useState<SuggestedFeeCode>(SuggestedFeeCode.UNKNOWN);

  useEffect(() => {
    setFee(getSuggestedFee(isPublicBenefit, financialGain));
  }, [isPublicBenefit, financialGain]);

  return (
    <Section header="Fee Determination">
      <SectionField label="Public benefit" labelWidth="2" contentWidth="8">
        <InlineYesNoSelect field="isPublicBenefit" />
      </SectionField>

      <SectionField label="Financial gain" labelWidth="2" contentWidth="8">
        <InlineYesNoSelect field="isFinancialGain" />
      </SectionField>

      <SectionField
        label="Suggested fee"
        tooltip="Licence Administration Fee (LAF) *: If the financial gain far outweighs the public benefit, Fair Market Value should be considered over Licence Administration Fee"
        labelWidth="2"
        contentWidth="8"
      >
        <span data-testid="suggestedFee">
          {fee}
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
                License administration fees are charged when there is either: a financial gain for
                the licensee and a benefit to the public, or when there is no financial gain to the
                licensee and there is no benefit for the public. The LAF can vary based on the
                impact to the land, the length of the LOO, and if MOTI requires a legal review.
              </p>
            )}
          </StyledHelpText>
        </span>
      </SectionField>

      <SectionField
        label="Notes"
        tooltip="Deviations from standard fees should be explained here"
        labelWidth="2"
        contentWidth="8"
      >
        <TextArea field="feeDeterminationNote" />
      </SectionField>
    </Section>
  );
};

const StyledHelpText = styled.div`
  font-size: 1.4rem;
  margin-top: 1rem;
`;

export default FeeDeterminationSubForm;
