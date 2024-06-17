import { InlineYesNoSelect } from '@/components/common/form/styles';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';

import * as Styled from './styles';

const ReferenceSubForm: React.FunctionComponent = () => {
  return (
    <Section header="Documentation">
      <SectionField label="Physical lease/licence exists" contentWidth="5">
        <InlineYesNoSelect field="hasPhysicalLicense" />
      </SectionField>
      <SectionField label="Digital lease/licence exists" contentWidth="5">
        <InlineYesNoSelect field="hasDigitalLicense" />
      </SectionField>
      <SectionField
        label="Document location"
        tooltip="Use this space to paste in links or system paths to relevant documents"
      >
        <Styled.MediumTextArea field="documentationReference" />
      </SectionField>

      <SectionField label="Lease notes" labelWidth="2">
        <Styled.MediumTextArea field="note" />
      </SectionField>
    </Section>
  );
};

export default ReferenceSubForm;
