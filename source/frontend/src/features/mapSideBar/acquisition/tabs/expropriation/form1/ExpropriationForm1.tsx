import { Formik, FormikHelpers } from 'formik';
import React from 'react';

import { Input } from '@/components/common/form';
import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { RestrictContactType } from '@/components/contact/ContactManagerView/ContactFilterComponent/ContactFilterComponent';
import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';

import { ExpropriationForm1YupSchema } from './ExpropriationForm1YupSchema';
import { ExpropriationForm1Model } from './models';

export interface IExpropriationForm1Props {
  acquisitionFile: Api_AcquisitionFile;
}

export const ExpropriationForm1: React.FC<IExpropriationForm1Props> = ({ acquisitionFile }) => {
  const onSubmit = async (
    values: ExpropriationForm1Model,
    formikHelpers: FormikHelpers<ExpropriationForm1Model>,
  ) => {
    // TODO: submit json values to Generate endpoint
  };

  return (
    <Formik<ExpropriationForm1Model>
      enableReinitialize
      initialValues={new ExpropriationForm1Model()}
      validationSchema={ExpropriationForm1YupSchema}
      onSubmit={onSubmit}
    >
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
    </Formik>
  );
};

export default ExpropriationForm1;
