import React from 'react';

import { Input } from '@/components/common/form';
import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { RestrictContactType } from '@/components/contact/ContactManagerView/ContactFilterComponent/ContactFilterComponent';

export const Form1: React.FC = () => {
  return (
    <StyledSummarySection>
      <Section header="Form 1 Notice of Expropriation">
        <SectionField label="Expropriation authority" required>
          <ContactInputContainer
            field="expropriationAuthority.contact"
            View={ContactInputView}
            restrictContactType={RestrictContactType.ONLY_ORGANIZATIONS}
          ></ContactInputContainer>
        </SectionField>
        <SectionField
          label="Impacted properties"
          required
          tooltip="For the selected properties - corresponding property and interest details will be captured on the form."
        ></SectionField>
        <SectionField label="Nature of interest">
          <Input field="landInterest" />
        </SectionField>
        <SectionField label="Purpose of expropriation">
          <Input field="purpose" />
        </SectionField>
      </Section>
    </StyledSummarySection>
  );
};

export default Form1;
