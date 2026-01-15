import { Formik, FormikHelpers, FormikProps } from 'formik';
import React, { useEffect, useState } from 'react';
import styled from 'styled-components';

import {
  FastDatePicker,
  ProjectSelector,
  Select,
  SelectOption,
  TextArea,
} from '@/components/common/form';
import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import { Input } from '@/components/common/form/Input';
import { PrimaryContactSelector } from '@/components/common/form/PrimaryContactSelector/PrimaryContactSelector';
import { UserRegionSelectContainer } from '@/components/common/form/UserRegionSelect/UserRegionSelectContainer';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { RestrictContactType } from '@/components/contact/ContactManagerView/ContactFilterComponent/ContactFilterComponent';
import * as API from '@/constants/API';
import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { isOrganizationSummary } from '@/interfaces';
import { IAutocompletePrediction } from '@/interfaces/IAutocomplete';
import { ApiGen_Concepts_Product } from '@/models/api/generated/ApiGen_Concepts_Product';
import { exists } from '@/utils/utils';

import ManagementTeamSubForm from '../form/ManagementTeamSubForm';
import { AddManagementFormYupSchema } from '../models/AddManagementFormYupSchema';
import { ManagementFormModel } from '../models/ManagementFormModel';

export interface IUpdateManagementFormProps {
  formikRef: React.Ref<FormikProps<ManagementFormModel>>;
  initialValues: ManagementFormModel;
  canEditDetails: boolean;
  onSubmit: (
    values: ManagementFormModel,
    formikHelpers: FormikHelpers<ManagementFormModel>,
  ) => void | Promise<any>;
  loading: boolean;
}

const UpdateManagementForm: React.FC<IUpdateManagementFormProps> = ({
  formikRef,
  initialValues,
  canEditDetails,
  onSubmit,
  loading,
}) => {
  const [projectProducts, setProjectProducts] = useState<ApiGen_Concepts_Product[] | undefined>(
    undefined,
  );

  const { getOptionsByType } = useLookupCodeHelpers();
  const { retrieveProjectProducts } = useProjectProvider();

  const managementFileStatusTypesOptions = getOptionsByType(API.MANAGEMENT_FILE_STATUS_TYPES);
  const managementFundingTypes = getOptionsByType(API.ACQUISITION_FUNDING_TYPES);
  const managementPurposeTypesOptions = getOptionsByType(API.MANAGEMENT_FILE_PURPOSE_TYPES);

  const onMinistryProjectSelected = React.useCallback(
    async (param: IAutocompletePrediction[]) => {
      if (param.length > 0) {
        if (param[0].id) {
          const result = await retrieveProjectProducts(param[0].id);
          if (result) {
            setProjectProducts(result as unknown as ApiGen_Concepts_Product[]);
          }
        }
      } else {
        setProjectProducts(undefined);
      }
    },
    [retrieveProjectProducts],
  );

  useEffect(() => {
    if (exists(initialValues.project)) {
      onMinistryProjectSelected([initialValues.project]);
    }
  }, [initialValues, onMinistryProjectSelected]);

  return (
    <Formik<ManagementFormModel>
      enableReinitialize
      innerRef={formikRef}
      initialValues={initialValues}
      validationSchema={AddManagementFormYupSchema}
      onSubmit={async (values, formikHelpers) => {
        onSubmit(values, formikHelpers);
      }}
    >
      {formikProps => {
        return (
          <>
            <LoadingBackdrop show={loading} parentScreen />

            <Container>
              <Section>
                <SectionField label="Status" required>
                  <Select
                    field="fileStatusTypeCode"
                    options={managementFileStatusTypesOptions}
                    required
                  />
                </SectionField>
              </Section>
              <Section header="Project">
                <SectionField label="Ministry project">
                  <ProjectSelector
                    field="project"
                    onChange={(vals: IAutocompletePrediction[]) => {
                      onMinistryProjectSelected(vals);
                      if (vals.length === 0) {
                        formikProps.setFieldValue('productId', '');
                      }
                    }}
                    disabled={!canEditDetails}
                  />
                </SectionField>
                {projectProducts && (
                  <SectionField label="Product">
                    <Select
                      field="productId"
                      options={projectProducts.map<SelectOption>(x => {
                        return { label: x.code + ' ' + x.description || '', value: x.id || 0 };
                      })}
                      placeholder="Select..."
                      disabled={!canEditDetails}
                    />
                  </SectionField>
                )}
                <SectionField label="Funding">
                  <Select
                    field="fundingTypeCode"
                    options={managementFundingTypes}
                    placeholder="Select funding..."
                    disabled={!canEditDetails}
                  />
                </SectionField>
              </Section>

              <Section header="Management Details">
                <SectionField label="File name" required>
                  <Input field="fileName" disabled={!canEditDetails} />
                </SectionField>
                <SectionField label="Historical file number">
                  <Input field="legacyFileNum" disabled={!canEditDetails} />
                </SectionField>
                <SectionField label="Purpose" required>
                  <Select
                    field="purposeTypeCode"
                    options={managementPurposeTypesOptions}
                    placeholder="Select..."
                    required
                    disabled={!canEditDetails}
                  />
                </SectionField>
                <SectionField label="Responsible payer">
                  <ContactInputContainer
                    field="responsiblePayer"
                    View={ContactInputView}
                    restrictContactType={RestrictContactType.ALL}
                    displayErrorAsTooltip={false}
                    required={false}
                  />
                  {exists(formikProps.values.responsiblePayer) &&
                    isOrganizationSummary(formikProps.values.responsiblePayer) && (
                      <SectionField
                        label="Primary contact"
                        labelWidth={{ xs: 4 }}
                        contentWidth={{ xs: 6 }}
                      >
                        <PrimaryContactSelector
                          field={`responsiblePayerPrimaryContactId`}
                          contactInfo={formikProps.values.responsiblePayer}
                        />
                      </SectionField>
                    )}
                </SectionField>
                <SectionField label="Additional details">
                  <Input field="additionalDetails" disabled={!canEditDetails} />
                </SectionField>
                <SectionField label="Ministry region">
                  <UserRegionSelectContainer field="regionCode" placeholder="Select region..." />
                </SectionField>
              </Section>

              <Section header="Management Team">
                <ManagementTeamSubForm canEditDetails={canEditDetails} />
              </Section>
              <Section header="Notice of Claim">
                <SectionField label="Received date">
                  <FastDatePicker formikProps={formikProps} field="noticeOfClaim.receivedDate" />
                </SectionField>
                <SectionField label="Comment">
                  <TextArea field="noticeOfClaim.comment" />
                </SectionField>
              </Section>
            </Container>
          </>
        );
      }}
    </Formik>
  );
};

export default UpdateManagementForm;

const Container = styled.div`
  background-color: ${props => props.theme.css.highlightBackgroundColor};
  padding-top: 1rem;

  .tab-pane {
    .form-section {
      margin: 1.5rem;
      padding-left: 1.5rem;
    }
  }

  [name='region'] {
    max-width: 25rem;
  }
`;
