import { Formik, FormikHelpers, FormikProps } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons';
import { Input } from '@/components/common/form';
import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import FormItem from '@/components/common/form/FormItem';
import { SectionField } from '@/components/common/Section/SectionField';
import { RestrictContactType } from '@/components/contact/ContactManagerView/ContactFilterComponent/ContactFilterComponent';
import FilePropertiesTable from '@/components/filePropertiesTable/FilePropertiesTable';
import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';
import { Api_PropertyFile } from '@/models/api/PropertyFile';

import { ExpropriationForm9Model } from '../models';
import { ExpropriationForm9YupSchema } from './ExpropriationForm9YupSchema';

export interface IExpropriationForm9Props {
  acquisitionFile: Api_AcquisitionFile;
}

export const ExpropriationForm9: React.FC<IExpropriationForm9Props> = ({ acquisitionFile }) => {
  const onSubmit = async (
    values: ExpropriationForm9Model,
    formikHelpers: FormikHelpers<ExpropriationForm9Model>,
  ) => {
    // TODO: submit json values to Generate endpoint
    alert(JSON.stringify(values, null, 4));
  };

  const onCancel = (formikProps: FormikProps<ExpropriationForm9Model>) => {
    formikProps.resetForm();
  };

  const onGenerate = (formikProps: FormikProps<ExpropriationForm9Model>) => {
    formikProps.setSubmitting(true);
    formikProps.submitForm();
  };

  return (
    <Formik<ExpropriationForm9Model>
      enableReinitialize
      initialValues={new ExpropriationForm9Model()}
      validationSchema={ExpropriationForm9YupSchema}
      onSubmit={onSubmit}
    >
      {formikProps => (
        <React.Fragment>
          <SectionField label="Expropriation authority" required>
            <ContactInputContainer
              field="expropriationAuthority.contact"
              View={ContactInputView}
              restrictContactType={RestrictContactType.ONLY_ORGANIZATIONS}
              displayErrorAsTooltip={false}
            ></ContactInputContainer>
          </SectionField>
          <SectionField
            label="Impacted properties"
            required
            tooltip="For the selected properties - corresponding property and interest details will be captured on the form."
          >
            <FormItem field="impactedProperties">
              <FilePropertiesTable
                disabledSelection={false}
                fileProperties={acquisitionFile.fileProperties ?? []}
                selectedFileProperties={formikProps.values.impactedProperties}
                setSelectedFileProperties={(fileProperties: Api_PropertyFile[]) => {
                  formikProps.setFieldValue('impactedProperties', fileProperties);
                }}
              ></FilePropertiesTable>
            </FormItem>
          </SectionField>
          <SectionField label="Shown on plan(s)">
            <Input field="registeredPlanNumbers" />
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
        </React.Fragment>
      )}
    </Formik>
  );
};

const RightFlexRow = styled(Row)`
  justify-content: end;
  align-items: center;
`;

export default ExpropriationForm9;
