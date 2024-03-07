import { Formik, FormikProps } from 'formik';
import React from 'react';
import styled from 'styled-components';

import { Multiselect, TextArea } from '@/components/common/form';
import { YesNoSelect } from '@/components/common/form/YesNoSelect';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import * as API from '@/constants/API';
import { useLookupCodeHelpers } from '@/hooks/useLookupCodeHelpers';
import { ApiGen_Concepts_PropertyManagement } from '@/models/api/generated/ApiGen_Concepts_PropertyManagement';

import { ManagementPurposeModel, PropertyManagementFormModel } from './models';
import { PropertyManagementYupSchema } from './validation';

export interface IPropertyManagementUpdateFormProps {
  isLoading: boolean;
  propertyManagement: ApiGen_Concepts_PropertyManagement;
  onSave: (apiModel: ApiGen_Concepts_PropertyManagement) => Promise<void>;
}

export const PropertyManagementUpdateForm = React.forwardRef<
  FormikProps<PropertyManagementFormModel>,
  IPropertyManagementUpdateFormProps
>(({ isLoading, propertyManagement, onSave }, formikRef) => {
  // Lookup codes
  const { getByType } = useLookupCodeHelpers();
  const purposeTypeOptions = getByType(API.PROPERTY_MANAGEMENT_PURPOSE_TYPES)
    .map(x => ManagementPurposeModel.fromLookup(x))
    .sort((a, b) => a.typeDescription.localeCompare(b.typeDescription));

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
              <SectionField label="Property purpose">
                <Multiselect
                  field="managementPurposes"
                  displayValue="typeDescription"
                  placeholder=""
                  hidePlaceholder
                  options={purposeTypeOptions}
                />
              </SectionField>
              <SectionField label="Lease/Licensed">
                {formikProps.values.formattedLeaseInformation ?? ''}
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
