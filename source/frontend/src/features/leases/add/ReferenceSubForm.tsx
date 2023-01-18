import { TextArea } from 'components/common/form';
import { InlineYesNoSelect } from 'components/common/form/styles';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import * as React from 'react';

import * as Styled from './styles';

const ReferenceSubForm: React.FunctionComponent = () => {
  return (
    <Section header="Documentation">
      <SectionField label="Physical lease/license exists" contentWidth="5">
        <InlineYesNoSelect field="hasPhysicalLicense" />
      </SectionField>
      <SectionField label="Digital lease/license exists" contentWidth="5">
        <InlineYesNoSelect field="hasDigitalLicense" />
      </SectionField>
      <SectionField
        label="Document location"
        tooltip="Use this space to paste in links or system paths to relevant documents"
      >
        <Styled.MediumTextArea field="documentationReference" />
      </SectionField>

      <SectionField label="LIS #" labelWidth="2">
        <Styled.LargeInlineInput field="tfaFileNumber" />
      </SectionField>
      <SectionField label="PS #" labelWidth="2">
        <Styled.LargeInlineInput field="psFileNo" />
      </SectionField>

      <SectionField label="Lease Notes" labelWidth="2">
        <Styled.MediumTextArea field="note" />
      </SectionField>
    </Section>
  );
};

export default ReferenceSubForm;
