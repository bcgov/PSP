import { Formik, FormikHelpers, FormikProps } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons';
import { Input } from '@/components/common/form';
import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import { SectionField } from '@/components/common/Section/SectionField';
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
    alert(JSON.stringify(values));
  };

  const onCancel = (formikProps: FormikProps<ExpropriationForm1Model>) => {
    formikProps.resetForm();
  };

  const onGenerate = (formikProps: FormikProps<ExpropriationForm1Model>) => {
    formikProps.setSubmitting(true);
    formikProps.submitForm();
  };

  return (
    <Formik<ExpropriationForm1Model>
      enableReinitialize
      initialValues={new ExpropriationForm1Model()}
      validationSchema={ExpropriationForm1YupSchema}
      onSubmit={onSubmit}
    >
      {formikProps => (
        <>
          <SectionField label="Expropriation authority" required>
            <ContactInputContainer
              field="expropriationAuthorityContact"
              View={ContactInputView}
              restrictContactType={RestrictContactType.ONLY_ORGANIZATIONS}
              displayErrorAsTooltip={false}
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

          <RightFlexRow>
            <Col xs="auto" className="pr-4">
              <Button variant="secondary" onClick={() => onCancel(formikProps)}>
                Cancel
              </Button>
            </Col>
            <Col xs="auto">
              <Button onClick={() => onGenerate(formikProps)}>Generate</Button>
            </Col>
          </RightFlexRow>
        </>
      )}
    </Formik>
  );
};

const RightFlexRow = styled(Row)`
  justify-content: end;
  align-items: center;
`;

export default ExpropriationForm1;
