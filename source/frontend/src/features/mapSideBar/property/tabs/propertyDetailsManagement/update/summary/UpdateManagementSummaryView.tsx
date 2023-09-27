import { Formik, FormikProps } from 'formik';
import React from 'react';
import styled from 'styled-components';

import { TextArea } from '@/components/common/form';
import { YesNoSelect } from '@/components/common/form/YesNoSelect';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { Api_PropertyManagement } from '@/models/api/Property';

import { PropertyManagementFormModel } from './models';
import { PropertyManagementYupSchema } from './validation';

export interface IUpdateManagementSummaryViewProps {
  isLoading: boolean;
  propertyManagement: Api_PropertyManagement;
  onSave: (apiModel: Api_PropertyManagement) => Promise<void>;
}

export const UpdateManagementSummaryView = React.forwardRef<
  FormikProps<PropertyManagementFormModel>,
  IUpdateManagementSummaryViewProps
>(({ isLoading, propertyManagement, onSave }, formikRef) => {
  const savePropertyManagement = async (values: PropertyManagementFormModel) => {
    await onSave(values.toApi());
  };

  return (
    <StyledFormWrapper>
      <StyledSummarySection>
        <LoadingBackdrop show={isLoading} />
        <Formik<PropertyManagementFormModel>
          enableReinitialize
          innerRef={formikRef}
          validationSchema={PropertyManagementYupSchema}
          initialValues={PropertyManagementFormModel.fromApi(propertyManagement)}
          onSubmit={savePropertyManagement}
        >
          {formikProps => (
            <Section header="Summary">
              <SectionField label="Property purpose"></SectionField>
              <SectionField label="Lease/Licensed">
                {formikProps.values.formatLeaseInformation()}
              </SectionField>
              <SectionField label="Utilities payable">
                <YesNoSelect field="isUtilitiesPayable" />
              </SectionField>
              <SectionField label="Taxes payable">
                <YesNoSelect field="isTaxesPayable" />
              </SectionField>
              <SectionField
                label="Additional details"
                contentWidth="12"
                tooltip="Describe the purpose of the property for the Ministry."
              >
                <TextArea field="additionalDetails" />
              </SectionField>
            </Section>
          )}
        </Formik>
      </StyledSummarySection>
    </StyledFormWrapper>
  );
});

const StyledFormWrapper = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  overflow-y: auto;
  padding-right: 1rem;
  padding-bottom: 1rem;
`;
