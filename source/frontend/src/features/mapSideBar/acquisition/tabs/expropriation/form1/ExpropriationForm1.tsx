import { Formik, FormikHelpers, FormikProps } from 'formik';
import { Fragment } from 'react';
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
  formikRef: React.Ref<FormikProps<ExpropriationForm1Model>>;
  onGenerate: (
    values: ExpropriationForm1Model,
    formikHelpers: FormikHelpers<ExpropriationForm1Model>,
  ) => void | Promise<void>;
}

export const ExpropriationForm1: React.FC<IExpropriationForm1Props> = ({
  acquisitionFile,
  formikRef,
  onGenerate,
}) => {
  const handleGenerate = async (
    values: ExpropriationForm1Model,
    formikHelpers: FormikHelpers<ExpropriationForm1Model>,
  ) => {
    return await onGenerate(values, formikHelpers);
  };

  const onCancelClick = (formikProps: FormikProps<ExpropriationForm1Model>) => {
    formikProps.resetForm();
  };

  return (
    <StyledForm1Border>
      <Formik<ExpropriationForm1Model>
        enableReinitialize
        innerRef={formikRef}
        initialValues={new ExpropriationForm1Model()}
        validationSchema={ExpropriationForm1YupSchema}
        onSubmit={(
          values: ExpropriationForm1Model,
          formikHelpers: FormikHelpers<ExpropriationForm1Model>,
        ) => {
          handleGenerate(values, formikHelpers);
        }}
      >
        {formikProps => (
          <Fragment>
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
            <SectionField label="Nature of interest">
              <Input field="landInterest" />
            </SectionField>
            <SectionField label="Purpose of expropriation">
              <Input field="purpose" />
            </SectionField>

            <RightFlexRow>
              <Col xs="auto" className="pr-4">
                <Button variant="primary" onClick={() => onCancelClick(formikProps)}>
                  Clear Form
                </Button>
              </Col>
            </RightFlexRow>
          </Fragment>
        )}
      </Formik>
    </StyledForm1Border>
  );
};

const RightFlexRow = styled(Row)`
  justify-content: end;
  align-items: center;
`;

const StyledForm1Border = styled.div`
  border: solid 0.2rem ${props => props.theme.css.iconBlueHover};
  margin-bottom: 0.5rem;
  border-radius: 0.5rem;
  padding: 3.2rem;
`;

export default ExpropriationForm1;
