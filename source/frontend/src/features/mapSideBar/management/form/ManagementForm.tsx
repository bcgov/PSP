import { Formik, FormikHelpers, FormikProps } from 'formik';
import React, { useEffect, useState } from 'react';
import styled from 'styled-components';

import { ProjectSelector, Select, SelectOption } from '@/components/common/form';
import { Input } from '@/components/common/form/Input';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import * as API from '@/constants/API';
import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { IAutocompletePrediction } from '@/interfaces/IAutocomplete';
import { ApiGen_Concepts_Product } from '@/models/api/generated/ApiGen_Concepts_Product';

import { PropertyForm } from '../../shared/models';
import { AddManagementFormYupSchema } from '../models/AddManagementFormYupSchema';
import { ManagementFormModel } from '../models/ManagementFormModel';
import ManagementPropertiesSubForm from './ManagementPropertiesSubForm';
import ManagementTeamSubForm from './ManagementTeamSubForm';

export interface IManagementFormProps {
  initialValues: ManagementFormModel;
  formikRef: React.RefObject<FormikProps<ManagementFormModel>>;
  onSubmit: (
    values: ManagementFormModel,
    formikHelpers: FormikHelpers<ManagementFormModel>,
  ) => void | Promise<any>;
  confirmBeforeAdd: (propertyForm: PropertyForm) => Promise<boolean>;
}

const ManagementForm: React.FC<IManagementFormProps> = props => {
  const { initialValues, onSubmit, confirmBeforeAdd, formikRef } = props;

  const [projectProducts, setProjectProducts] = useState<ApiGen_Concepts_Product[] | undefined>(
    undefined,
  );

  const { getOptionsByType } = useLookupCodeHelpers();
  const { retrieveProjectProducts } = useProjectProvider();

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
    if (initialValues.project) {
      onMinistryProjectSelected([initialValues.project]);
    }
  }, [initialValues, onMinistryProjectSelected]);

  return (
    <Formik<ManagementFormModel>
      innerRef={formikRef}
      initialValues={initialValues}
      validationSchema={AddManagementFormYupSchema}
      onSubmit={onSubmit}
      enableReinitialize={true}
    >
      {formikProps => {
        return (
          <Container>
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
                />
              </SectionField>
              {projectProducts !== undefined && (
                <SectionField label="Product">
                  <Select
                    field="productId"
                    options={projectProducts.map<SelectOption>(x => {
                      return { label: x.code + ' ' + x.description || '', value: x.id || 0 };
                    })}
                    placeholder="Select..."
                  />
                </SectionField>
              )}
              <SectionField label="Funding">
                <Select
                  field="fundingTypeCode"
                  options={managementFundingTypes}
                  placeholder="Select funding..."
                />
              </SectionField>
            </Section>

            <Section header="Properties to include in this file:">
              <ManagementPropertiesSubForm
                formikProps={formikProps}
                confirmBeforeAdd={confirmBeforeAdd}
              />
            </Section>

            <Section header="Management Details">
              <SectionField label="File name" required>
                <Input field="fileName" />
              </SectionField>
              <SectionField label="Historical file number">
                <Input field="legacyFileNum" />
              </SectionField>
              <SectionField label="Purpose" required>
                <Select
                  field="purposeTypeCode"
                  options={managementPurposeTypesOptions}
                  placeholder="Select..."
                  required
                />
              </SectionField>
              <SectionField label="Additional details">
                <Input field="additionalDetails" />
              </SectionField>
            </Section>

            <Section header="Management Team">
              <ManagementTeamSubForm canEditDetails={true} />
            </Section>
          </Container>
        );
      }}
    </Formik>
  );
};

export default ManagementForm;

const Container = styled.div`
  .form-section {
    margin: 0;
    padding-left: 0;
  }

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
