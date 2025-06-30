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

import { ExpropriationForm5Model } from '../models';
import { ExpropriationForm5YupSchema } from './ExpropriationForm5YupSchema';

export interface IExpropriationForm5Props {
  acquisitionFile: ApiGen_Concepts_AcquisitionFile;
  formikRef: React.Ref<FormikProps<ExpropriationForm5Model>>;
  onGenerate: (
    values: ExpropriationForm5Model,
    formikHelpers: FormikHelpers<ExpropriationForm5Model>,
  ) => void | Promise<void>;
}

export const ExpropriationForm5: React.FC<IExpropriationForm5Props> = ({
  acquisitionFile,
  formikRef,
  onGenerate,
}) => {
  const handleGenerate = async (
    values: ExpropriationForm5Model,
    formikHelpers: FormikHelpers<ExpropriationForm5Model>,
  ) => {
    return await onGenerate(values, formikHelpers);
  };

  const onCancelClick = (formikProps: FormikProps<ExpropriationForm5Model>) => {
    formikProps.resetForm();
  };

  return (
    <StyledForm5Border>
      <Formik<ExpropriationForm5Model>
        enableReinitialize
        innerRef={formikRef}
        initialValues={new ExpropriationForm5Model()}
        validationSchema={ExpropriationForm5YupSchema}
        onSubmit={(
          values: ExpropriationForm5Model,
          formikHelpers: FormikHelpers<ExpropriationForm5Model>,
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
    </StyledForm5Border>
  );
};

const RightFlexRow = styled(Row)`
  justify-content: end;
  align-items: center;
`;

const StyledForm5Border = styled.div`
  border: solid 0.2rem ${props => props.theme.css.iconBlueHover};
  margin-bottom: 0.5rem;
  border-radius: 0.5rem;
  padding: 3.2rem;
`;

export default ExpropriationForm5;
