import { FormikProps } from 'formik';
import styled from 'styled-components';

import { Input, TextArea } from '@/components/common/form';
import { InlineYesNoSelect } from '@/components/common/form/styles';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';

import { LeaseFormModel } from '../models';

export interface IFeeDeterminationSubFormProps {
  formikProps: FormikProps<LeaseFormModel>;
}

const FeeDeterminationSubForm: React.FunctionComponent<
  React.PropsWithChildren<IFeeDeterminationSubFormProps>
> = ({ formikProps }) => {
  const { values, setFieldValue } = formikProps;
  return (
    <Section header="Fee Determination">
      <SectionField label="Public benefit" labelWidth="2" contentWidth="8">
        <InlineYesNoSelect field="isPublicBenefit" />
      </SectionField>

      <SectionField label="Financial gain" labelWidth="2" contentWidth="8">
        <InlineYesNoSelect field="isFinancialGain" />
      </SectionField>

      <SectionField
        label="Sugested fee"
        tooltip="If the financial gain far outweighs the public benefit, Fair Market Value should be considered over Licence Administration Fee."
        labelWidth="2"
        contentWidth="8"
      >
        <Input disabled={true} field="suggestedFee" />
      </SectionField>

      <SectionField
        label="Notes"
        tooltip="Deviations from standard fees should be explained here"
        labelWidth="2"
        contentWidth="8"
      >
        <MediumTextArea field="feeDeterminationNote" />
      </SectionField>
    </Section>
  );
};

export default FeeDeterminationSubForm;

const MediumTextArea = styled(TextArea)`
  textarea.form-control {
    min-width: 100%;
    height: 7rem;
    resize: none;
  }
`;
