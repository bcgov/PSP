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
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';

import { ExpropriationForm1Model } from '../models';
import { ExpropriationForm1YupSchema } from './ExpropriationForm1YupSchema';

export interface IExpropriationForm1Props {
  acquisitionFile: ApiGen_Concepts_AcquisitionFile;
  onGenerate: (acquisitionFileId: number, values: ExpropriationForm1Model) => Promise<void>;
  onError?: (e: Error) => void;
}

export const ExpropriationForm1: React.FC<IExpropriationForm1Props> = ({
  acquisitionFile,
  onGenerate,
  onError,
}) => {
  const onSubmit = async (
    values: ExpropriationForm1Model,
    formikHelpers: FormikHelpers<ExpropriationForm1Model>,
  ) => {
    try {
      if (acquisitionFile.id) {
        await onGenerate(acquisitionFile.id, values);
      }
    } catch (e) {
      if (typeof onError === 'function') {
        onError(e as Error);
      }
    } finally {
      formikHelpers?.setSubmitting(false);
    }
  };

  const onCancelClick = (formikProps: FormikProps<ExpropriationForm1Model>) => {
    formikProps.resetForm();
  };

  const onGenerateClick = (formikProps: FormikProps<ExpropriationForm1Model>) => {
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
                setSelectedFileProperties={(fileProperties: ApiGen_Concepts_FileProperty[]) => {
                  formikProps.setFieldValue('impactedProperties', fileProperties);
                }}
              ></FilePropertiesTable>
            </FormItem>
          </SectionField>
          <SectionField label="Nature of interest">
            <Input field="landInterest" />
          </SectionField>
          <SectionField label="Purpose of expropriation">
            <Input field="purpose" />
          </SectionField>

          <RightFlexRow>
            <Col xs="auto" className="pr-4">
              <Button variant="secondary" onClick={() => onCancelClick(formikProps)}>
                Cancel
              </Button>
            </Col>
            <Col xs="auto">
              <Button onClick={() => onGenerateClick(formikProps)}>Generate</Button>
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

export default ExpropriationForm1;
