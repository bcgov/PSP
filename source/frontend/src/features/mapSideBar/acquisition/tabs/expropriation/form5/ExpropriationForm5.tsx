import { Formik, FormikHelpers, FormikProps } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaFileContract } from 'react-icons/fa';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons';
import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import FormItem from '@/components/common/form/FormItem';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledAddButton } from '@/components/common/styles';
import { RestrictContactType } from '@/components/contact/ContactManagerView/ContactFilterComponent/ContactFilterComponent';
import FilePropertiesTable from '@/components/filePropertiesTable/FilePropertiesTable';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';

import { ExpropriationForm5Model } from '../models';
import { ExpropriationForm5YupSchema } from './ExpropriationForm5YupSchema';

export interface IExpropriationForm5Props {
  acquisitionFile: ApiGen_Concepts_AcquisitionFile;
  onGenerate: (acquisitionFileId: number, values: ExpropriationForm5Model) => Promise<void>;
  onError?: (e: Error) => void;
}

export const ExpropriationForm5: React.FC<IExpropriationForm5Props> = ({
  acquisitionFile,
  onGenerate,
  onError,
}) => {
  const onSubmit = async (
    values: ExpropriationForm5Model,
    formikHelpers: FormikHelpers<ExpropriationForm5Model>,
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

  const onCancelClick = (formikProps: FormikProps<ExpropriationForm5Model>) => {
    formikProps.resetForm();
  };

  const onGenerateClick = (formikProps: FormikProps<ExpropriationForm5Model>) => {
    formikProps.setSubmitting(true);
    formikProps.submitForm();
  };

  return (
    <Formik<ExpropriationForm5Model>
      enableReinitialize
      initialValues={new ExpropriationForm5Model()}
      validationSchema={ExpropriationForm5YupSchema}
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
            tooltip="For the selected properties - corresponding property and interest details will be captured on the form"
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

          <RightFlexRow>
            <Col xs="auto" className="pr-4">
              <Button variant="secondary" onClick={() => onCancelClick(formikProps)}>
                Cancel
              </Button>
            </Col>
            <Col xs="auto">
              <StyledAddButton title="Download File" onClick={() => onGenerateClick(formikProps)}>
                <FaFileContract size={28} className="mr-2" />
                Generate Form 5
              </StyledAddButton>
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

export default ExpropriationForm5;
