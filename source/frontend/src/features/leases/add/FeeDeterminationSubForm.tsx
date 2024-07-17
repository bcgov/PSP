import { FormikProps, getIn } from 'formik';
import { useEffect, useState } from 'react';

import { TextArea } from '@/components/common/form';
import { InlineYesNoSelect } from '@/components/common/form/styles';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';

import { getSuggestedFee } from '../leaseUtils';
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

  const [fee, setFee] = useState('');

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
        tooltip="Licence Administration Fee (LAF) *: If the financial gain far outweighs the public benefit, Fair Market Value should be considered over Licence Administration Fee."
        labelWidth="2"
        contentWidth="8"
      >
        <span data-testid="suggestedFee">{fee}</span>
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

export default FeeDeterminationSubForm;
