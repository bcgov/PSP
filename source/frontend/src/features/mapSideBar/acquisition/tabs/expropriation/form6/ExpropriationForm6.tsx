import { Formik, FormikHelpers, FormikProps } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons';
import FormItem from '@/components/common/form/FormItem';
import { SectionField } from '@/components/common/Section/SectionField';
import FilePropertiesTable from '@/components/filePropertiesTable/FilePropertiesTable';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';

import { ExpropriationForm6Model } from '../models';
import { ExpropriationForm6YupSchema } from './ExpropriationForm6YupSchema';

export interface IExpropriationForm6Props {
  acquisitionFile: ApiGen_Concepts_AcquisitionFile;
  formikRef: React.Ref<FormikProps<ExpropriationForm6Model>>;
  onGenerate: (
    values: ExpropriationForm6Model,
    formikHelpers: FormikHelpers<ExpropriationForm6Model>,
  ) => void | Promise<void>;
}

export const ExpropriationForm6: React.FC<IExpropriationForm6Props> = ({
  acquisitionFile,
  formikRef,
  onGenerate,
}) => {
  const handleGenerate = async (
    values: ExpropriationForm6Model,
    formikHelpers: FormikHelpers<ExpropriationForm6Model>,
  ) => {
    return await onGenerate(values, formikHelpers);
  };

  const onCancelClick = (formikProps: FormikProps<ExpropriationForm6Model>) => {
    formikProps.resetForm();
  };

  return (
    <StyledForm6Border>
      <Formik<ExpropriationForm6Model>
        enableReinitialize
        innerRef={formikRef}
        initialValues={new ExpropriationForm6Model()}
        validationSchema={ExpropriationForm6YupSchema}
        onSubmit={(
          values: ExpropriationForm6Model,
          formikHelpers: FormikHelpers<ExpropriationForm6Model>,
        ) => {
          handleGenerate(values, formikHelpers);
        }}
      >
        {formikProps => (
          <React.Fragment>
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
    </StyledForm6Border>
  );
};

const RightFlexRow = styled(Row)`
  justify-content: end;
  align-items: center;
`;

const StyledForm6Border = styled.div`
  border: solid 0.2rem ${props => props.theme.css.iconBlueHover};
  margin-bottom: 0.5rem;
  border-radius: 0.5rem;
  padding: 3.2rem;
`;

export default ExpropriationForm6;
