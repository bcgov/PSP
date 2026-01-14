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

import { ExpropriationForm7Model } from '../models';
import { ExpropriationForm7YupSchema } from './ExpropriationForm7YupSchema';

export interface IExpropriationForm7Props {
  acquisitionFile: ApiGen_Concepts_AcquisitionFile;
  formikRef: React.Ref<FormikProps<ExpropriationForm7Model>>;
  onGenerate: (
    values: ExpropriationForm7Model,
    formikHelpers: FormikHelpers<ExpropriationForm7Model>,
  ) => void | Promise<void>;
}

export const ExpropriationForm7: React.FC<IExpropriationForm7Props> = ({
  acquisitionFile,
  formikRef,
  onGenerate,
}) => {
  const handleGenerate = async (
    values: ExpropriationForm7Model,
    formikHelpers: FormikHelpers<ExpropriationForm7Model>,
  ) => {
    return await onGenerate(values, formikHelpers);
  };

  const onCancelClick = (formikProps: FormikProps<ExpropriationForm7Model>) => {
    formikProps.resetForm();
  };

  return (
    <StyledForm7Border>
      <Formik<ExpropriationForm7Model>
        enableReinitialize
        innerRef={formikRef}
        initialValues={new ExpropriationForm7Model()}
        validationSchema={ExpropriationForm7YupSchema}
        onSubmit={(
          values: ExpropriationForm7Model,
          formikHelpers: FormikHelpers<ExpropriationForm7Model>,
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
    </StyledForm7Border>
  );
};

const RightFlexRow = styled(Row)`
  justify-content: end;
  align-items: center;
`;

const StyledForm7Border = styled.div`
  border: solid 0.2rem ${props => props.theme.css.iconBlueHover};
  margin-bottom: 0.5rem;
  border-radius: 0.5rem;
  padding: 3.2rem;
`;

export default ExpropriationForm7;
