import { Formik, FormikHelpers, FormikProps } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons';
import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import FormItem from '@/components/common/form/FormItem';
import { SectionField } from '@/components/common/Section/SectionField';
import { RestrictContactType } from '@/components/contact/ContactManagerView/ContactFilterComponent/ContactFilterComponent';
import FilePropertiesTable from '@/components/filePropertiesTable/FilePropertiesTable';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';

import { ExpropriationForm4Model } from '../models';
import { ExpropriationForm4YupSchema } from './ExpropriationForm4YupSchema';

export interface IExpropriationForm4Props {
  acquisitionFile: ApiGen_Concepts_AcquisitionFile;
  formikRef: React.Ref<FormikProps<ExpropriationForm4Model>>;
  onGenerate: (
    values: ExpropriationForm4Model,
    formikHelpers: FormikHelpers<ExpropriationForm4Model>,
  ) => void | Promise<void>;
}

export const ExpropriationForm4: React.FC<IExpropriationForm4Props> = ({
  acquisitionFile,
  formikRef,
  onGenerate,
}) => {
  const handleGenerate = async (
    values: ExpropriationForm4Model,
    formikHelpers: FormikHelpers<ExpropriationForm4Model>,
  ) => {
    return await onGenerate(values, formikHelpers);
  };

  const onCancelClick = (formikProps: FormikProps<ExpropriationForm4Model>) => {
    formikProps.resetForm();
  };

  return (
    <StyledFormBorder>
      <Formik<ExpropriationForm4Model>
        enableReinitialize
        innerRef={formikRef}
        initialValues={new ExpropriationForm4Model()}
        validationSchema={ExpropriationForm4YupSchema}
        onSubmit={(
          values: ExpropriationForm4Model,
          formikHelpers: FormikHelpers<ExpropriationForm4Model>,
        ) => {
          handleGenerate(values, formikHelpers);
        }}
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
                <Button variant="primary" onClick={() => onCancelClick(formikProps)}>
                  Clear Form
                </Button>
              </Col>
            </RightFlexRow>
          </React.Fragment>
        )}
      </Formik>
    </StyledFormBorder>
  );
};

const RightFlexRow = styled(Row)`
  justify-content: end;
  align-items: center;
`;

const StyledFormBorder = styled.div`
  border: solid 0.2rem ${props => props.theme.css.iconBlueHover};
  margin-bottom: 0.5rem;
  border-radius: 0.5rem;
  padding: 3.2rem;
`;

export default ExpropriationForm4;
