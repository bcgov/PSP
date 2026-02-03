import { Formik, FormikHelpers } from 'formik';
import styled from 'styled-components';

import { Select, TextArea } from '@/components/common/form';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import * as API from '@/constants/API';
import SidebarFooter from '@/features/mapSideBar/shared/SidebarFooter';
import { StyledFormWrapper } from '@/features/mapSideBar/shared/styles';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';

import { PropertyImprovementFormModel } from '../models/PropertyImprovementFormModel';
import { PropertyImprovementFormYupSchema } from '../models/PropertyImprovementFormYupSchema';

export interface IPropertyImprovementFormProps {
  isLoading: boolean;
  initialValues: PropertyImprovementFormModel;
  onSubmit: (
    values: PropertyImprovementFormModel,
    formikHelpers: FormikHelpers<PropertyImprovementFormModel>,
  ) => Promise<void>;
  onCancel: () => void;
}

const PropertyImprovementForm: React.FunctionComponent<IPropertyImprovementFormProps> = ({
  isLoading,
  initialValues,
  onSubmit,
  onCancel,
}) => {
  const { setModalContent, setDisplayModal } = useModalContext();
  const { getOptionsByType } = useLookupCodeHelpers();

  const propertyImprovementTypesOptions = getOptionsByType(API.PROPERTY_IMPROVEMENT_TYPES);

  const cancelFunc = (resetForm: () => void, dirty: boolean) => {
    if (!dirty) {
      resetForm();
      onCancel();
    } else {
      setModalContent({
        ...getCancelModalProps(),
        handleOk: () => {
          resetForm();
          setDisplayModal(false);
          onCancel();
        },
      });
      setDisplayModal(true);
    }
  };

  return (
    <StyledFormWrapper>
      <Formik<PropertyImprovementFormModel>
        enableReinitialize
        initialValues={initialValues}
        validationSchema={PropertyImprovementFormYupSchema}
        onSubmit={onSubmit}
      >
        {formikProps => {
          return (
            <>
              <LoadingBackdrop
                show={formikProps.isSubmitting || isLoading}
                parentScreen={true}
              ></LoadingBackdrop>
              <StyledContent>
                <Section header="Property Improvement Details">
                  <SectionField label="Improvement type" required>
                    <Select
                      options={propertyImprovementTypesOptions}
                      field="propertyImprovementTypeCode"
                      placeholder="Select type"
                    />
                  </SectionField>
                  <SectionField label="Description">
                    <TextArea field="description" />
                  </SectionField>
                </Section>
              </StyledContent>
              <StyledFooter>
                <SidebarFooter
                  onSave={() => formikProps.submitForm()}
                  isOkDisabled={formikProps.isSubmitting || !formikProps.dirty}
                  onCancel={() => cancelFunc(formikProps.resetForm, formikProps.dirty)}
                  displayRequiredFieldError={
                    formikProps.isValid === false && !!formikProps.submitCount
                  }
                />
              </StyledFooter>
            </>
          );
        }}
      </Formik>
    </StyledFormWrapper>
  );
};

export default PropertyImprovementForm;

const StyledContent = styled.div`
  background-color: ${props => props.theme.css.highlightBackgroundColor};
`;

const StyledFooter = styled.div`
  margin-right: 1rem;
  padding-bottom: 1rem;
  z-index: 0;
`;
